. ./deploy-config.sh

# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2
fi

# check if things are where we think they are
if [ ! -f "./$serviceConfigBackupFilename" ];
then
    echo "backup not found, abandoning rollback";
    exit 2;
fi

if [ ! -f "/lib/systemd/system/$serviceDeployedFilename" ];
then
    echo "destination not found, abandoning rollback";
    exit 2;
fi

# copy backup to systemd
echo "step 1/3 deploying backup"
sudo cp ./$serviceConfigBackupFilename /lib/systemd/system/$serviceDeployedFilename

# reload configuration file
echo "step 2/3 reload service configuration"
sudo systemctl daemon-reload

# restart service
echo "step 3/3 starting service"
systemctl restart $serviceDeployedFilename

echo "-- rollback complete --"