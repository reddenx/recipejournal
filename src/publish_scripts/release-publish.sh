cd "${0%/*}"

# ensure args
if [ "$#" -eq 2]; then
    echo "missing variables"
    exit 2;
fi

# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2;
fi


sudo -u pi git fetch
sudo -u pi git checkout $1
sudo -u pi git reset --hard

bash ./site_publish.sh $2
