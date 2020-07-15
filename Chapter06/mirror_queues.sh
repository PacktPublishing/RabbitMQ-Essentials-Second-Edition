sudo rabbitmqctl clear_policy -p cc-prod-vhost Q_TTL_DLX
sudo rabbitmqctl set_policy -p cc-prod-vhost HA_Q_TTL_DLX