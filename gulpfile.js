var gulp = require('gulp');
var bundle = require('aurelia-bundler').bundle;

var config = {
    force: true,
    baseURL: '.', // baseURL of the application
    configPath: './config.js', // config.js file. Must be within `baseURL`
    bundles: {
        //"dist/app-build": {
        //    includes: [
        //        '[aurelia-app/*.js]',
        //        '[aurelia-app/**/*.js]'
        //    ],
        //    excludes: [
        //        'config.js'
        //    ],
        //    options: {
        //        inject: true,
        //        minify: true
        //    }
        //},
        "dist/aurelia-build": {
            includes: [
                'aurelia-binding',
                'aurelia-bootstrapper',
                'aurelia-dependency-injection',
                'aurelia-event-aggregator',
                'aurelia-framework',
                'aurelia-history',
                'aurelia-http-client',
                'aurelia-loader',
                'aurelia-loader-default',
                'aurelia-logging',
                'aurelia-metadata',
                'aurelia-pal',
                'aurelia-pal-browser',
                'aurelia-path',
                'aurelia-route-recognizer',
                'aurelia-router',
                'aurelia-task-queue',
                'aurelia-templating',
                'aurelia-templating-resources',
                'aurelia-templating-router'
                //'bootstrap/css/bootstrap.css!text'
            ],
            options: {
                inject: true,
                minify: true
            }
        }
    }
};

gulp.task('bundle', function () {
    return bundle(config);
});