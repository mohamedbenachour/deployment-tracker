module.exports = {
    presets: [
        '@babel/preset-env',
        ["@babel/preset-react", {
            "runtime": "automatic"
        }],
        '@babel/preset-typescript',
    ],
    plugins: [
        '@babel/plugin-proposal-object-rest-spread',
        ['import', { libraryName: 'antd', libraryDirectory: 'es', style: 'css' }], // `style: true` for less
    ],
};
