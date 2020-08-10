const path = require('path');
const fs = require('fs');
const webpack = require('webpack');

const packageJson = JSON.parse(fs.readFileSync('./package.json', 'utf8'));
const babelConfig = require('./babel.config');

module.exports = {
    mode: 'development',
    entry: {
        index: './src/index.js',
        'account.login': './src/login.js',
        'account.logout': './src/logout.js',
    },
    devtool: 'source-map',
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
    },
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
                        cacheDirectory: true,
                    },
                },
            },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
        ],
    },
    plugins: [
        new webpack.DefinePlugin({
            'process.env.VERSION': JSON.stringify(packageJson.version),
        }),
    ],
};
