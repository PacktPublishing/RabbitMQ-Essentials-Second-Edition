rabbitmq-plugins enable rabbitmq_web_stomp
rabbitmqctl add_vhost cc-dev-ws
rabbitmqctl set_permissions -p cc-dev-ws cc-dev ".*" ".*" ".*"
rabbitmqadmin -u cc-dev -p taxi123 --vhost=cc-dev-ws declare queue name=taxi_information durable=true
rabbitmqadmin -u cc-dev -p taxi123 --vhost=cc-dev-ws declare exchange name=taxi_exchange type=direct
rabbitmqadmin -u cc-dev -p taxi123 --vhost=cc-dev-ws declare exchange name=taxi_header_exchange type=headers