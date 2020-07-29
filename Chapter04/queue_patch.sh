rabbitmqctl set_policy -p cc-dev-vhost Q_TTL_DLX "taxi\.\d+" "{"message-ttl":604800000, "dead-letter-exchange":"taxi-dlx"}" --apply-to queues
