#!/bin/bash

queues_to_purge=`rabbitmqctl list_queues -p cc-dev-vhost name messages_ready consumers | grep "taxi\.[[:digit:]]\+[[:space:]]\+[1-9][[:digit:]]*[[:space:]]\+0" | awk "{print $1}"`

for queue in $queues_to_purge ; do
  echo -n "Purging $queue ... "
  rabbitmqadmin -V cc-dev-vhost -u cc-admin -p taxi123 purge queue name=$queue
done
