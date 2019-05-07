var config = require('./webpack.config.js');

module.exports = Object.assign(config, {
    mode: 'production',
    devtool: 'source-map'
});
