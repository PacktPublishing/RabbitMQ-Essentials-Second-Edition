#	1. Require client librar
require "bunny"

#	2. Read RABBITMQ_URI from ENV
connection = Bunny.new ENV["RABBITMQ_URI"]

#	3. Communication session with RabbitMQ
connection.start
channel = connection.create_channel

#	4. Declare a default exchange
exchange = channel.default_exchange

# 5. Return handler
exchange.on_return do |return_info, properties, content| puts "A returned message!"
end

# 6. Declare a inbox queue (taxi-inbox.100)
queue = channel.queue("taxi-inbox.100", durable: true)

# 7. Subscribe messages
queue.subscribe do |delivery_info, properties, content| puts "A message is consumed."
end

# 8. Publish a mandatory message to taxi-inbox.100
exchange.publish("A message published to a queue that does exist, it should NOT be returned", :mandatory => true, :routing_key => queue.name)

#	9. Publish another mandatory message to a random queue
exchange.publish("A message published to a queue that does not exist, it should be returned", :mandatory => true, :routing_key => "random-key")
sleep 0.5
connection.close
