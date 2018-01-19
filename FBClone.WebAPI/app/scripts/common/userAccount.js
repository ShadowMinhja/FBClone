'use strict';

angular
    .module('common.services')
    .factory("userAccount", ["$resource", "$q", "appSettings", "localStorageService", userAccount])

function userAccount($resource, $q, appSettings, localStorageService) {
    var resource = {};
    resource.registration = $resource(appSettings.serverPath + "/api/Account/RegisterBusiness", null,
                {
                    'registerBusinessUser': { 
                        method: 'POST',
                        headers: { 'Authorization': 'Bearer ' + localStorageService.get("fsLogin").access_token }
                    }
                });
    resource.login = $resource(appSettings.serverPath + "/Token", null,
                {
                    'loginUser': {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        transformRequest: function (data, headersGetter) {
                            var str = [];
                            for (var d in data)
                                str.push(encodeURIComponent(d) + "=" +
                                                    encodeURIComponent(data[d]));
                            return str.join("&");
                        }
                    }
                });
        
    resource.accountActions = $resource(appSettings.serverPath + "/api/Account/Logout", null,
                {
                    'logOutUser': {
                        method: 'POST',
                        headers: { 'Authorization': 'Bearer ' + localStorageService.get("fsLogin").access_token }
                    }
                });

    return resource;     
}