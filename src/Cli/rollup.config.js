import fable from 'rollup-plugin-fable'
const path = require('path')

function resolve(filePath) {
    return path.resolve(__dirname, filePath)
}

export default {
    input: resolve('Cli.fsproj'),
    output: {
        file: resolve('../../build/cli.js'),
        format: 'cjs'
    },
    plugins: [fable({})],
}