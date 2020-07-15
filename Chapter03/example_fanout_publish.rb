#	1. Require client library
require "bunny"

#	2. Read RABBITMQ_URI from ENV
connection = Bunny.new ENV['RABBITMQ_URI']

#	3. Communication session with RabbitMQ
connection.start
channel = connection.create_channel

#	4. Declare queues for taxis
queue1 = channel.queue('taxi-inbox.1', durable: true)
queue2 = channel.queue('taxi-inbox.2', durable: true)

# 5. Declare a fanout exchange
exchange = channel.fanout('taxi-fanout')

#	6. Bind the queue
queue1.bind(exchange, routing_key: "")
queue2.bind(exchange, routing_key: "")

#	7. Publish a message
exchange.publish("Hello everybody! This is an information message from the crew!", key: "")

#	8. Close the connection
connection.close
