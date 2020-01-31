const path = require('path');

const babelConfig = require('./babel.config');

module.exports = {
    mode: 'development',
    entry: {
        index: './src/index.js',
        'account.login': './src/login.js',
        'account.logout': './src/logout.js'
    },
    devtool: 'source-map',
    output: {
        path: path.resolve(__dirname, '..', 'wwwroot', 'js'),
        filename: '[name].bundle.js',
    },
    target: 'web',
    module: {
        rules: [
            {
                test: /\.m?js$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        ...babelConfig,
                        cacheDirectory: true
                    },
                },
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader',
                ],
            },
        ],
    },
};
