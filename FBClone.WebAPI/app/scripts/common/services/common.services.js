'use strict';

angular
    .module('common.services',
                                    ["ngResource"]) 
    .constant("appSettings",
    {
        serverPath: ""
        //serverPath: "http://localhost:55246"
        //serverPath: "http://surveytab.azurewebsites.net"
        //serverPath: "http://www.fbClone.io"
    });