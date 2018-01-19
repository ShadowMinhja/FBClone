'use strict';

angular.module('fbCloneApp')
    .factory('menuResource', ['$q', '$resource', 'appSettings', 'currentUser', menuResource])

function menuResource($q, $resource, appSettings, currentUser) {
    var service = {};

    var Menus = $resource(appSettings.serverPath + '/api/Menu/', {},
    {
        'query': { //GET
            method: 'GET',
            isArray: true,
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        'get': { //GET
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        'update': { //PUT
            method: 'PUT',
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        }
    });
    var MenuSections = $resource(appSettings.serverPath + '/api/MenuSections/', {},
    {
        'query': { //GET
            method: 'GET',
            isArray: true,
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        'get': { //GET
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        'update': { //PUT
            method: 'PUT',
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        }
    });
    var MenuItems = $resource(appSettings.serverPath + '/api/MenuItems/', {},
    {
        'query': { //GET
            method: 'GET',
            isArray: true,
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        'get': { //GET
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        'update': { //PUT
            method: 'PUT',
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        //USED $HTTP Method instead
        //'uploadMenuImage': {
        //    method: 'POST',
        //    isArray: false,
        //    url: appSettings.serverPath + '/api/MenuItems/UploadMenuImage',
        //    transformRequest: angular.identity,
        //    headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token, 'Content-Type': undefined }
        //}   
    });

    service.menuHeaders = [];
    service.currentMenu = {};
    service.setMenuByID = setMenuByID;
    service.getMenuHeaders = getMenuHeaders;

    service.newMenu = newMenu;
    service.newSection = newSection;
    service.newItem = newItem;

    service.addMenu = addMenu;
    service.updateMenu = updateMenu;

    service.addSection = addSection;
    service.deleteSection = deleteSection;
    service.updateSection = updateSection;
    service.reorderSections = reorderSections;

    service.getItem = getItem;
    service.addItem = addItem;
    service.deleteItem = deleteItem;
    service.updateItem = updateItem;
    service.uploadMenuImage = uploadMenuImage;
    service.reorderItems = reorderItems;
    service.print = print;

    function getMenuHeaders() {
        var deferred = $q.defer();

        Menus.query().$promise.then(function (result) {
            service.menuHeaders = result;
            deferred.resolve(result);
        }, function (error) {
            deferred.reject(error);
        });

        return deferred.promise;
    }

    function setMenuByID(id) {
        var deferred = $q.defer();

        Menus.get({ id: id }).$promise.then(function (menu) {
            service.currentMenu = menu;
            deferred.resolve();
        }, function (error) {
            deferred.reject('There was an error retrieving menus');
        });

        return deferred.promise;
    }


    function newMenu() {

        var t = {
            id: "",
            description: "",
            //createdAt: new Date(),
            //createdBy: "",
            active: "Active",
            menuSections: [],
            locations: [],
            userid: currentUser.getProfile().id,
            createdBy: currentUser.getProfile().userName,
            updatedBy: currentUser.getProfile().userName

        }

        return t;
    }

    function newSection() {
        var o = 1;
        if (service.currentMenu.menuSections.length > 0) {
            o = _.max(service.currentMenu.menuSections, 'sequence').id + 1;
        }

        var s = {
            id: "",
            sequence: o,
            menuID: service.currentMenu.id,
            sectionTitle: "",
            sectionSubTitle: "",
            active: "Active",
            items: [],
            userid: currentUser.getProfile().id,
            createdBy: currentUser.getProfile().userName,
            updatedBy: currentUser.getProfile().userName
        }

        return s;
    }


    function newItem(section) {
        var o = 1;
        if (section.menuItems.length > 0) {
            o = _.max(section.menuItems, 'sequence').sequence + 1;
        }


        var i = {
            id: "",
            menuSectionId: section.id,
            sequence: o,
            itemType: "",
            required: false,
            itemText: "",
            active: "Active",
            price: 0,
            daysOfWeek: { "Sunday" : true, "Monday" : true, "Tuesday" : true, "Wednesday" : true, "Thursday" : true, "Friday" : true, "Saturday": true },
            userid: currentUser.getProfile().id,
            createdBy: currentUser.getProfile().userName,
            updatedBy: currentUser.getProfile().userName
        };

        return i;
    }

    function addMenu(menu) {
        var deferred = $q.defer();

        Menus.save(menu).$promise.then(function (result) {
            service.currentMenu = result;
            getMenuHeaders();

            deferred.resolve({
                id: result.id,
                message: "Menu " + result.description + " added successfully"
            });
        }, function (error) {
            deferred.reject("There was an error");
        });

        return deferred.promise;
    }

    function updateMenu(menu) {
        var deferred = $q.defer();
        menu.updatedBy = currentUser.getProfile().userName;
        Menus.update({ id: menu.id }, menu).$promise.then(function (result) {
            deferred.resolve({
                id: result.id,
                message: "Menu " + result.description + " saved successfully"
            });
        }, function (error) {
            deferred.reject("There was an error");
        });

        return deferred.promise;
    }



    function addSection(section) {
        var deferred = $q.defer();

        MenuSections.save(section).$promise.then(function (result) {
            if (service.currentMenu) {
                service.currentMenu.menuSections.push(result);
            }

            deferred.resolve({
                id: result.id,
                message: "Menu Section " + result.sectionTitle + " added successfully"
            });
        }, function (error) {
            deferred.reject("There was an error");
        });

        return deferred.promise;
    }

    function deleteSection(section) {
        var deferred = $q.defer();

        MenuSections.delete({ id: section.id }).$promise.then(function (result) {
            if (service.currentMenu) {
                var index = service.currentMenu.menuSections.indexOf(result);
                service.currentMenu.menuSections.splice(index, 1);
            }
            reorderSections();
            deferred.resolve({
                id: result.id,
                message: "Menu Section " + result.sectionTitle + " deleted successfully"
            });
        }, function (error) {
            deferred.reject("There was an error");
        });

        return deferred.promise;
    }

    function updateSection(section) {
        var deferred = $q.defer();
        section.updatedBy = currentUser.getProfile().userName;
        MenuSections.update({ id: section.id }, section).$promise.then(function (result) {

            deferred.resolve({
                id: result.id,
                message: "Menu Section " + result.sectionTitle + " saved successfully"
            });
        }, function (error) {
            deferred.reject("There was an error");
        });

        return deferred.promise;
    }

    function getItem(menuItemId) {
        var deferred = $q.defer();
        MenuItems.get({ id: menuItemId }).$promise.then(function (result) {
            deferred.resolve(result);
        }, function (error) {
            deferred.reject("There was an error retrieving menu item");
        });
        return deferred.promise;
    }

    function addItem(item, croppedImage) {
        var deferred = $q.defer();
        item.daysOfWeek = JSON.stringify(item.daysOfWeek);
        MenuItems.save(item).$promise.then(function (result) {
            if (service.currentMenu) {
                var section = _.findWhere(service.currentMenu.menuSections, { 'id': result.menuSectionId });
                section.menuItems.push(result);
            }
            deferred.resolve({
                id: result.id,
                message: "Menu Item added successfully"
            });
        }, function (error) {
            deferred.reject("There was an error");
        });

        return deferred.promise;
    }

    function deleteItem(section, item) {
        var deferred = $q.defer();
        MenuItems.delete({ id: item.id }).$promise.then(function (result) {
            if (service.currentMenu) {
                var index = section.menuItems.indexOf(item);
                section.menuItems.splice(index, 1);

            }
            reorderItems(section);
            deferred.resolve({
                id: result.id,
                message: "Menu Item " + result.itemText + " deleted successfully"
            });
        }, function (error) {
            deferred.reject("There was an error");
        });

        return deferred.promise;
    }

    function updateItem(item, croppedImage) {
        var deferred = $q.defer();
        item.daysOfWeek = JSON.stringify(item.daysOfWeek);
        item.updatedBy = currentUser.getProfile().userName;
        MenuItems.update({ id: item.id }, item).$promise.then(function (result) {
            deferred.resolve({
                id: result.id,
                message: "Menu Item saved successfully"
            });
        }, function (error) {
            deferred.reject("There was an error");
        });

        return deferred.promise;
    }

    function uploadMenuImage(imageBlob, itemId) {
        var deferred = $q.defer();
        menuImageResource.uploadMenuImage(imageBlob, itemId).then(function (result) {
            deferred.resolve(result);
        }, function (error) {
            deferred.reject("");
        });
        return deferred.promise;
    }

    function reorderSections() {
        for (var index in service.currentMenu.menuSections) {
            service.currentMenu.menuSections[index].sequence = Number(index);
            updateSection(service.currentMenu.menuSections[index]);
        }
    }

    function reorderItems(section) {
        for (var index in section.menuItems) {
            section.menuItems[index].sequence = Number(index);
            updateItem(section.menuItems[index]);
        }
    }

    function print() {

    }

    return service;
}