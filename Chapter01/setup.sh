#	sync package metadata $ sudo apt-get update
#	install dependencies manually
sudo apt-get -y install socat logrotate init-system-helpers adduser

# download the package
sudo apt-get -y install wget
wget https://github.com/rabbitmq/rabbitmq-server/releases/download/v3.7.14/rabbi tmq-server_3.7.14-1_all.deb

# install the package with dpkg
sudo dpkg -i rabbitmq-server_3.7.14-1_all.deb
rm rabbitmq-server_3.7.14-1_all.deb
sudo rabbitmq-plugins enable

# user setup
sudo rabbitmqctl add_user cc-admin taxi123
sudo rabbitmqctl set_user_tags cc-admin administrator
sudo rabbitmqctl change_password guest guest123
sudo rabbitmqctl add_user cc-dev taxi123
sudo rabbitmqctl set_permissions -p cc-dev-vhost cc-admin ".*" ".*" ".*"
sudo rabbitmqctl set_permissions -p cc-dev-vhost cc-dev ".*" ".*" ".*"