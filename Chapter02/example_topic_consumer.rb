#	Require client library
require "bunny"

#	Read RABBITMQ_URI from ENV
connection = Bunny.new ENV['RABBITMQ_URI']

#	Start a communication session with RabbitMQ
connection.start
channel = connection.create_channel

def taxi_topic_subscribe(channel, taxi, type)
  # Declare a queue for a given taxi
  queue = channel.queue(taxi, durable: true)

  # Declare a topic exchange
  exchange = channel.topic('taxi-topic', durable: true, auto_delete: true)

  #	Bind the queue to the exchange
  queue.bind(exchange, routing_key: type)

  #	Bind the queue to the exchange to make sure the taxi will get any order
  queue.bind(exchange, routing_key: 'taxi')

  #	Subscribe from the queue
  queue.subscribe(block:true,manual_ack: false) do |delivery_info, properties, payload|

    process_order(payload)
  end
end

taxi = "taxi.3"
taxi_topic_subscribe(channel, taxi, "taxi.eco.3")
