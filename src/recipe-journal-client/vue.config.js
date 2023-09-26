module.exports = {
    configureWebpack: {
        plugins: [
            new this.configureWebpack.DefinePlugin({
                'process.appInfo': {
                    appVersion: '"' + version + '"'
                }
            })
        ]
    },
    outDir: "../RecipeJournalApi/wwwroot",
    filenameHashing: false,
    devServer: {
        proxy: {
            "^/api": {
                target: 'http://localhost:5002'
            }
        }
    }
}