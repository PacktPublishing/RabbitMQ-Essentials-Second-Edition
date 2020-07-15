#	1. Require client library
require "bunny"

#	2. Read RABBITMQ_URI from ENV
connection = Bunny.new ENV['RABBITMQ_URI']

#	3. Communication session
connection.start
channel = connection.create_channel

# 4. Declare a queue
queue1 = channel.queue('taxi-inbox.1', durable: true,

                       arguments: {'x-message-ttl'=> 604800000, 'x-dead-letter-exchange'=> 'taxi-dlx'})

# 5. Declare a queue
queue2 = channel.queue('taxi-inbox.2', durable: true,
                       arguments: {'x-message-ttl'=> 604800000, 'x-dead-letter-exchange'=> 'taxi-dlx'})

# 6. Declare a fanout exchange
exchange = channel.fanout('taxi-fanout')

#	7. Bind queues to exchanges
queue1.bind(exchange, routing_key: "")
queue2.bind(exchange, routing_key: "")

#	8. Declare a dead letter queue
taxi_dlq = channel.queue('taxi-dlq', durable: true)

#	9. Declare dead letter fanout exchange
dlx_exchange = channel.fanout('taxi-dlx')

#	10. Bind taxi-dlx to taxi-dlq
taxi_dlq.bind(dlx_exchange, routing_key: "")

# 11. Publish a message
exchange.publish("Hello! This is an information message!",	key: "")

#	12. Close the connection
connection.close
