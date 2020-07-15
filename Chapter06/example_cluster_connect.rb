require "bunny"

begin
  connection = Bunny.new(
      hosts: ['rmq-prod-01', 'rmq-prod-02'])
  connection.start
rescue Bunny::TCPConnectionFailed => e
  puts "Connection to server failed"
end
