rabbitmqctl clear_policy -p cc-prod-vhost Q_TTL_DLX
rabbitmqctl set_policy -p cc-prod-vhost HA_Q_TTL_DLX
