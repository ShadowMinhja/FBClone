import webpack from 'webpack';
import path from 'path';
import ExtractTextPlugin from 'extract-text-webpack-plugin';

const GLOBALS = {
    'process.env.NODE_ENV': JSON.stringify('development'),
    __DEV__: true
};

export default {
    debug: true,
    devtool: 'cheap-module-eval-source-map', // more info:https://webpack.github.io/docs/build-performance.html#sourcemaps and https://webpack.github.io/docs/configuration.html#devtool
    noInfo: true, // set to false to see a list of every file being bundled.
    entry: [
      //'webpack-hot-middleware/client?reload=true',
      './Scripts/app/index'
    ],
    target: 'web', // necessary per https://webpack.github.io/docs/testing.html#compile-and-test
    output: {
        path: `${__dirname}/Scripts/app/dev`, // Note: Physical files are only output by the production build task `npm run build`.
        publicPath: 'http://localhost:3000/', // Use absolute paths to avoid the way that URLs are resolved by Chrome when they're parsed from a dynamically loaded CSS blob. Note: Only necessary in Dev.
        filename: 'bundle.js'   
    },
    devServer: {
            contentBase: "./Scripts/app",
            host: "localhost",
            port: 9000
    },
    plugins: [
      new webpack.DefinePlugin(GLOBALS), // Tells React to build in prod mode. https://facebook.github.io/react/downloads.htmlnew webpack.HotModuleReplacementPlugin());
      //new webpack.HotModuleReplacementPlugin(),
      new webpack.NoErrorsPlugin(),
      // Extract the CSS into a seperate file
      new ExtractTextPlugin('bundle.css')
    ],
    module: {
        loaders: [
          {test: /\.js$/, include: path.join(__dirname, 'Scripts/app'), loaders: ['babel']},
          {test: /\.eot(\?v=\d+.\d+.\d+)?$/, loader: 'file'},
          {test: /\.(woff|woff2)$/, loader: 'file-loader?prefix=font/&limit=5000'},
          {test: /\.ttf(\?v=\d+.\d+.\d+)?$/, loader: 'file-loader?limit=10000&mimetype=application/octet-stream'},
          {test: /\.svg(\?v=\d+.\d+.\d+)?$/, loader: 'file-loader?limit=10000&mimetype=image/svg+xml'},
          {test: /\.(jpe?g|png|gif)$/i, loaders: ['file']},
          {test: /\.ico$/, loader: 'file-loader?name=[name].[ext]'},
          {test: /\.scss$/, exclude: /node_modules/, loaders: ['style', 'css?sourceMap', 'sass?sourceMap']},
          {test: /\.css$/, exclude: /node_modules/, 
              loader: ExtractTextPlugin.extract(
                'style-loader',
                'css-loader'
              )
          }
        ]
    }
};
