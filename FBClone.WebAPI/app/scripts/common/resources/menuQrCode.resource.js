'use strict';

angular
    .module('common.services')
    .factory("menuQrCodeResource",
        ["$http", "$resource", "appSettings","currentUser",menuQrCodeResource])
    
function menuQrCodeResource($http, $resource, appSettings, currentUser) {
    var menuQrCodeResource = this;

    menuQrCodeResource.getMenuQrCodes = getMenuQrCodes;
    menuQrCodeResource.createMenuQrCode = createMenuQrCode;
    menuQrCodeResource.uploadMenuQrCode = uploadMenuQrCode;

    var params = {
        locationId: '@locationId',
        menuId: '@menuId'
    };

    var MenuQrCodes = $resource(appSettings.serverPath + "/api/MenuQrCode", params,
        {
            'query': { //GET
                method: 'GET',
                isArray: true,
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            },
             'save': { //POST
                method: 'POST', 
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
             }
        });

    function getMenuQrCodes(locationId) {
        return MenuQrCodes.query({ "locationId": locationId });
    }

    function createMenuQrCode(locationId, menuId) {
        return MenuQrCodes.save({ locationId: locationId, menuId: menuId });
    }

    function uploadMenuQrCode(imageBlob, itemId) {
        var req = {
            method: 'POST',
            url: appSettings.serverPath + "/api/MenuQrCode/UploadMenuQrCodeImage",
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token, 'Content-Type': 'multipart/form-data' },
            data: {
                menuQrCode: imageBlob
            },
            params: { menuQrCodeId: itemId },
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

    return menuQrCodeResource;
}