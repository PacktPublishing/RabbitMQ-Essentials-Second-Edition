rabbitmq-plugins enable rabbitmq_federation
rabbitmq-plugins enable rabbitmq_federation_management
rabbitmqctl -p logs-prod set_parameter federation-upstream app-prod-logs "{"uri":"amqp://cc-prod:******@app-prod-1:5672/cc-prod-vhost"}"
rabbitmqctl set_policy -p logs-prod --apply-to exchanges log-exchange-federation "^app-logs*" "{"federation-upstream-set":"all"}" --apply-to exchanges
rabbitmqctl eval rabbit_federation_status:status()
