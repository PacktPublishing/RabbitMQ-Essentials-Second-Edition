rabbitmq-plugins enable rabbitmq_web_stomp
sudo rabbitmqctl add_vhost cc-dev-ws
sudo rabbitmqctl set_permissions -p cc-dev-ws cc-dev ".*" ".*" ".*"
rabbitmqadmin declare queue name=taxi_information durable=true vhost=cc-dev-ws
rabbitmqadmin declare exchange name=taxi_exchange type=direct vhost=cc-dev-ws
