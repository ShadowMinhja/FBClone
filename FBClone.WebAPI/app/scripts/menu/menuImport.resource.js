'use strict';

angular.module('fbCloneApp')
    .factory('menuImportResource', ['$http', 'appSettings', 'currentUser', menuImportResource])
    function menuImportResource($http, appSettings, currentUser) {
        var menuImportResource = this;

        menuImportResource.importMenu = importMenu;

        function importMenu(menuImportFile, menuId) {
            var req = {
                method: 'POST',
                url: appSettings.serverPath + "/api/Menu/ImportMenu",
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token, 'Content-Type': 'multipart/form-data' },
                data: {
                    menuImportFile: menuImportFile
                },
                params: { menuId: menuId },
                transformRequest: function (data, headersGetter) {
                    var formData = new FormData();
                    angular.forEach(data, function (value, key) {
                        formData.append(key, value);
                    });

                    var headers = headersGetter();
                    delete headers['Content-Type'];

                    return formData;
                }
            }

            return $http(req);
        }

        return menuImportResource;
    }