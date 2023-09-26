. ./deploy-config.sh

# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2;
fi

# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2;
fi

# install components client app
echo "step 1/5 client install STARTING"
sudo -u pi npm install --prefix $vueClientPath >> publish.log
echo "step 1/5 client install COMPLETE"

# build client app
echo "step 2/5 client build STARTING"
if [ "$#" -eq 1 ]; then
    echo "publishing client version $1"
    sudo -u pi npm run build --prefix $vueClientPath -- --appVersion $1 >> publish.log
else 
    sudo -u pi npm run build --prefix $vueClientPath >> publish.log
fi
echo "step 2/5 client build COMPLETE"

# stop site
echo "step 3/5 service stop STARTING"
systemctl stop $serviceDeployedFilename >> publish.log
echo "step 3/5 service stop COMPLETE"

# move to api directory
cd $dotnetApiPath

# publishes everything
echo "step 4/5 server build STARTING"
if [ "$#" -eq 1 ]; then
    echo "publishing server version $1"
    sudo -u pi dotnet publish -c Release /property:Version=$1 -r linux-arm >> publish.log
else 
    sudo -u pi dotnet publish -c Release -r linux-arm >> publish.log
fi
echo "step 4/5 server build COMPLETE"

# start site
echo "step 5/5 service start STARTING"
systemctl start $serviceDeployedFilename >> publish.log
echo "step 5/5 service start COMPLETE"

exit 0;
