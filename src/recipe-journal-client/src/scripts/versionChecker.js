const CLIENT_VERSION = process.appInfo.appVersion;

export default class VersionChecker {
    async ensureCorrectVersion() {
        var params = new URLSearchParams(window.location.search);
        var queryVersion = params.get("v");

        if (queryVersion)
            return;

        let version = CLIENT_VERSION;
        try {
            let response = await axios.get('rss/api/v2/version');
            if (response.data) {
                version = response.data;
            }
        } catch (error) {
            console.error(error);
        }

        if (!version.startsWith(CLIENT_VERSION)) {
            console.log('old version detected: ');
            window.location = '?v=' + v + window.location.hash;
        }
    }
}
