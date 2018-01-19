'use strict';

/**
 * Config
 */

function getBaseUrl() {    
    return window.location.origin;
}

function getEnvironment() {
    var environment = "";
    switch (window.location.hostname) {
        case "localhost":
            environment = 'DEVELOPMENT';
            break;
        case "surveytab.azurewebsites.net":
            environment = 'TEST';
            break;
        case "fbClone.io":
            environment = 'PROD';
            break;
        case "www.fbClone.io":
            environment = 'PROD';
            break;
        default:
            environment = 'PROD';
            break;
    }
    return environment;
}

function getImgixUrl() {
    var ImgixUrl = "";
    switch (window.location.hostname) {
        case "localhost":
            ImgixUrl = "https://fbClone-test.imgix.net";
            break;
        case "surveytab.azurewebsites.net":
            ImgixUrl = "https://fbClone-test.imgix.net";
            break;
        case "fbClone.io":
            ImgixUrl = "https://fbClone.imgix.net";
            break;
        case "www.fbClone.io":
            ImgixUrl = "https://fbClone.imgix.net";
            break;
        default:
            ImgixUrl = "https://fbClone.imgix.net";
            break;
    }
    return ImgixUrl;
}

function getAlgoliaContainers(container) {
    var containerName = "";
    switch (window.location.hostname) {
        case "localhost":
            containerName = "dev_" + container;
            break;
        case "XXXXXXXXXXXXXXXX":
            containerName = "test_" + container;
            break;
        case "fbClone.io":
            containerName = container;
            break;
        case "www.fbClone.io":
            containerName = container;
            break;
        default:
            containerName = container;
            break;
    }
    return containerName;
}

module.exports = {
    name: 'fbClone',
    version: '1.0.0',
    env: getEnvironment(),
    //mapbox: {
    //    accessToken: process.env.MAPBOX_ACCESS_TOKEN,
    //},
    clientId: 'fbClone',
    //stream: {
    //    appId: 13723,
    //    key: '7n7w23mw7pbv',
    //},
    api: {
        //baseUrl: 'https://localhost:44359',
        //baseUrl: 'http://surveytab.azurewebsites.net',
        //baseUrl: 'https://fbClone.io',
        baseUrl: getBaseUrl()
    },
    imgix: {
        //baseUrl: 'https://fbClone-test.imgix.net',
        //baseUrl: 'https://fbClone.imgix.net',
        baseUrl: getImgixUrl(),
        imageQuality: 30,
        thumbQuality: 30,
    },
    algolia: {
        appId: 'XXXXXXXXXXXXXXX',
        searchOnlyKey: 'XXXXXXXXXXXXXXXXXXXXXXXXXXX',
        //userIndex: 'dev_fsUsers',
        //userIndex: 'test_fsUsers',
        //userIndex: 'fsUsers',
        userIndex: getAlgoliaContainers("fsUsers")
    },
  
};