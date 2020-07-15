# Require client library
require 'bunny'

# Read RABBITMQ_URI from ENV
connection = Bunny.new ENV['RABBITMQ_URI']

# Start a communication session with RabbitMQ
connection.start
channel = connection.create_channel

# Ensure topic exists
def on_start(channel)
  # Declare and return the topic exchange, taxi-topic
  channel.topic("taxi-topic", durable: true, auto_delete: true)
end

# Publishing an order to the exchange
def order_taxi(type, exchange)
  payload = "example-message"
  message_id = exchange.publish(payload,
                                     routing_key: type,
                                     content_type: "application/json",
                                     content_encoding: "UTF-8",
                                     persistent: true,
                                     message_id: message_id)
end
exchange = on_start(channel)

#	Order will go to any eco taxi
order_taxi('taxi.eco', exchange)
#	Order will go to any taxi
order_taxi('taxi', exchange)
