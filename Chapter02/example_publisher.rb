#	1. Require client library
require 'bunny'

#	2. Read RABBITMQ_URI from ENV
connection = Bunny.new ENV['RABBITMQ_URI']

#	3. Start a communication session with RabbitMQ
connection.start
channel = connection.create_channel



# 4. define methods
def on_start(channel)
  queue = channel.queue('taxi.1', durable: true)
  exchange = channel.direct('taxi-direct', durable: true, auto_delete: true)
  queue.bind(exchange, routing_key: 'taxi.1')
  exchange
end

def order_taxi(taxi, exchange)
  payload = "example-message"
  message_id = rand
  exchange.publish(payload,
                   routing_key: taxi,
                   content_type: "application/json",
                   content_encoding: "UTF-8",
                   persistent: true,
                   message_id: message_id)
end

#5. define exchange
exchange = on_start(channel)

#6. Publish message
order_taxi("taxi.1", exchange)
