. ./deploy-config.sh

# ensure running with appropriate privilages
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2;
fi

# is the site already set up?
if [ ! -f "/etc/nginx/sites-available/$nginxAvailableSitesFilename" ];
then
    echo "site is not already configured"
    exit 2;
fi

# copy the current site to a backup
echo "step 1/3 backing up"
sudo cp /etc/nginx/sites-available/$nginxAvailableSitesFilename ./$nginxConfigBackupFilename

echo "step 2/3 removing config"
sudo rm /etc/nginx/sites-enabled/$nginxAvailableSitesFilename
sudo rm /etc/nginx/sites-available/$nginxAvailableSitesFilename

# reset nginx
echo "step 3/3 loading configuration"
sudo nginx -s reload

echo "deployment complete"
exit 0;