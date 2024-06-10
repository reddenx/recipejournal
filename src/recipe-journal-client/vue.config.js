const webpack = require('webpack');
const argv = require('yargs').argv;
const version = argv?.appVersion || '0.0.0';

module.exports = {
    configureWebpack: {
        plugins: [
            new webpack.DefinePlugin({
                'process.appInfo': {
                    appVersion: '"' + version + '"'
                }
            })
        ]
    },
    outputDir: "../RecipeJournalApi/wwwroot",
    filenameHashing: false,
    devServer: {
        proxy: {
            "^/(api|login|integrationauth)": {
                target: 'http://localhost:5002'
            }
        }
    }
}