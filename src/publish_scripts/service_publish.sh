. ./deploy-config.sh

# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2
fi

# check if things are where we think they are
if [ ! -f "./$serviceConfigFilename" ];
then
    echo "config to be deployed not found";
    exit 2;
fi

if [ ! -f "/lib/systemd/system/$serviceDeployedFilename" ];
then
    echo "destination not found, abandoning publish";
    exit 2;
fi

# copy current install to backup file
echo "step 1/4 backing up current version"
sudo cp /lib/systemd/system/$serviceDeployedFilename $serviceConfigBackupFilename

# copy new config to systemd
echo "step 2/4 deploying new version"
sudo cp ./$serviceConfigFilename /lib/systemd/system/$serviceDeployedFilename

# reload configuration file
echo "step 3/4 reload service configuration"
sudo systemctl daemon-reload

# restart service
echo "step 4/4 restarting service"
systemctl restart $serviceDeployedFilename

echo "-- deployment complete --"