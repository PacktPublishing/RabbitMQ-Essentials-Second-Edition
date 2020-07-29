require "bunny"

connection = Bunny.new ENV["RABBITMQ_URI"]
connection.start
channel = connection.create_channel
channel.prefetch(1)

#	Method for the processing
def process_order(info)
  puts "Handling taxi order"
  puts info
  sleep 5.0
  puts "Processing done"
end

def taxi_subscribe(channel, taxi)
  #	Declare a queue for a given taxi
  queue = channel.queue(taxi, durable: true)

  #	Declare a direct exchange, taxi-direct
  exchange = channel.fanout("taxi-fanout")

  #	Bind the queue to the exchange
  queue.bind(exchange, routing_key: taxi)

  # Subscribe from the queue
  queue.subscribe(block: true, manual_ack: true) do |delivery_info, properties, payload|
    process_order(payload)
    channel.acknowledge(delivery_info.delivery_tag, false)
  end

end
taxi = ""
taxi_subscribe(channel, taxi)
