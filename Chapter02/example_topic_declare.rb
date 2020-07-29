#	1. Require client library
require "bunny"

#	2. Read RABBITMQ_URI from ENV
connection = Bunny.new ENV["RABBITMQ_URI"]

#	3. Start a communication session with RabbitMQ
connection.start
channel = connection.create_channel

def on_start(channel)
  #4. Declare and return the topic exchange, taxi-topic
  channel.topic("taxi-topic", durable: true, auto_delete: true)
end
exchange = on_start(channel)
