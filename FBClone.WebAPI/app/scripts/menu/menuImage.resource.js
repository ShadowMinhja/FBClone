'use strict';

angular.module('fbCloneApp')
    .factory('menuImageResource', ['$http', 'appSettings', 'currentUser', menuImageResource])
    function menuImageResource($http, appSettings, currentUser) {
        var menuImageResource = this;

        menuImageResource.uploadMenuImage = uploadMenuImage;

        function uploadMenuImage(imageBlob, itemId) {
            var req = {
                method: 'POST',
                url: appSettings.serverPath + "/api/MenuItems/UploadMenuImage",
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token, 'Content-Type': 'multipart/form-data' },
                data: {
                    croppedImage: imageBlob
                },
                params: { menuItemId: itemId },
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

        return menuImageResource;
    }