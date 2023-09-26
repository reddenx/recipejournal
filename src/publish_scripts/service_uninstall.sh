. ./deploy-config.sh

# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2
fi

if [ ! -f "/lib/systemd/system/$serviceDeployedFilename" ];
then
    echo "destination not found, abandoning publish";
    exit 2;
fi

# copy current install to backup file
echo "step 1/3 backing up current version"
sudo cp /lib/systemd/system/$serviceDeployedFilename $serviceConfigBackupFilename

# disable and stop all instances
echo "step 2/3 stopping, disabling, and removing service"
sudo systemctl stop $serviceDeployedFilename
sudo systemctl disable $serviceDeployedFilename
sudo rm /lib/systemd/system/$serviceDeployedFilename

# reload configuration file
echo "step 3/3 reload service configuration"
sudo systemctl daemon-reload
systemctl reset-failed

echo "-- uninstall complete --"