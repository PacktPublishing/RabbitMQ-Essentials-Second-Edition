require "bunny"

#	1. Require client library
require 'bunny'

#	2. Read RABBITMQ_URI from ENV
connection = Bunny.new ENV['RABBITMQ_URI']

#	3. Start a communication session with RabbitMQ
connection.start
channel = connection.create_channel

def on_start(channel)

  #	4.	Declare a queue for a given taxi
  queue = channel.queue('taxi.1', durable: true)
  #	5.	Declare a direct exchange, taxi-direct
  exchange = channel.direct('taxi-direct', durable: true, auto_delete: true)
  #	6. Bind the queue to the exchange
  queue.bind(exchange, routing_key: 'taxi.1')
  #	7. Return the exchange
  exchange
end
exchange = on_start(channel)