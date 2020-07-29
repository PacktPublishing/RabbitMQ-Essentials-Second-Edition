#	1. Require client library
require "bunny"

#	2. Read RABBITMQ_URI from ENV
connection = Bunny.new ENV["RABBITMQ_URI"]

#	3. Start a communication session with RabbitMQ
connection.start
channel = connection.create_channel

#	Method for the processing
def process_order(info)
  puts "Handling taxi order"
  puts info
  sleep 5.0
  puts "Processing done"
end

def taxi_subscribe(channel, taxi)
  #	4. Declare a queue for a given taxi
  queue = channel.queue(taxi, durable: true)

  #	5. Declare a direct exchange, taxi-direct
  exchange = channel.direct("taxi-direct", durable: true, auto_delete:
      true)

  #	6. Bind the queue to the exchange
  queue.bind(exchange, routing_key: taxi)

  # 7. Subscribe from the queue

  queue.subscribe(block: true, manual_ack: false) do |delivery_info, properties, payload|
  process_order(payload)
  end

end
taxi = "taxi.1"
taxi_subscribe(channel, taxi)

