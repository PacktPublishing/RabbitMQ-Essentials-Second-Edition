require "bunny"

RABBITMQ_HOST1 = ENV["RABBITMQ_HOST1"]
RABBITMQ_HOST2 = ENV["RABBITMQ_HOST2"]
USERNAME = ENV["RABBITMQ_USER"]
PASSWORD = ENV["RABBITMQ_PASSWORD"]
VHOST = ENV["RABBITMQ_VHOST"]

begin
  # try to connect to one of the two hosts where the queue is mirrored or federated
  # alternatively, uses addresses: [addr1, addr2,...] with ipv4 host and port in place
  # of hosts
  connection = Bunny.new(
      hosts: [RABBITMQ_HOST1, RABBITMQ_HOST2], username: USERNAME, password: PASSWORD)
  connection.start
rescue Bunny::TCPConnectionFailed => e
  puts "Connection to server failed"
end
