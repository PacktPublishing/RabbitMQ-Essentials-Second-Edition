#	1. Require client library
require "bunny"

#	2. Read RABBITMQ_URI from ENV
connection = Bunny.new ENV["RABBITMQ_URI"]
connection.start

DELAYED_QUEUE="work.later"
DESTINATION_QUEUE="work.now"

#	3. Define publish method
def publish(connection)
  #	4. Communication session
  channel = connection.create_channel

  # 5 Declare queue

  channel.queue(DELAYED_QUEUE, arguments: { "x-dead-letter-exchange" => "", "x-dead-letter-routing-key" => DESTINATION_QUEUE, "x-message-ttl" => 3})

  # 6. Publish a message
  channel.default_exchange.publish "message content", routing_key:
      DELAYED_QUEUE
  puts "#{Time.now}: Published the message"
  channel.close
end

#	7 Define subscribe method
def subscribe(connection)
  channel = connection.create_channel
  q = channel.queue DESTINATION_QUEUE, durable: true
  q.subscribe do |delivery, headers, body|
    puts "#{Time.now}: Got the message"
  end
end

subscribe(connection)
publish(connection)
