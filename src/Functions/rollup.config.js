import fable from 'rollup-plugin-fable'
const path = require('path')

function resolve(filePath) {
    return path.resolve(__dirname, filePath)
}

export default {
    input: resolve('Functions.fsproj'),
    output: {
        file: resolve('../../functions/functions.js'),
        format: 'cjs'
    },
    plugins: [fable({})],
    external: [
        'adal-node',
        'buffer',
        'es6-promise',
        'fs',
        'isomorphic-fetch',
        'path',
        'react',
        'react-dom/server'
    ]
}
