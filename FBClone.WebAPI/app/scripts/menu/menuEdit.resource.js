'use strict';

angular.module('fbCloneApp')
    .factory('menuEditResource', ['$rootScope', '$modal', '$q', 'menuResource', 'menuImageResource', 'menuImportResource', 'locationResource', 'helpers', menuEditResource])

function menuEditResource($rootScope, $modal, $q, menuResource, menuImageResource, menuImportResource, locationResource, helpers) {

    var service = {};
    service.addMenu = addMenu;
    service.editMenu = editMenu;
    service.importMenu = importMenu;
    service.editSection = editSection;
    service.addSection = addSection;
    service.editItem = editItem;
    service.addItem = addItem;


    function addMenu(possibleLocations) {
        var deferred = $q.defer();
        var t = menuResource.newMenu();
        $modal.open({
            templateUrl: '/app/scripts/menu/partials/menuEditModal.html',
            controller: 'MenuEditCtrl',
            size: 'lg',
            backdropClass: 'splash splash-2 splash-ef-14',
            windowClass: 'splash splash-2 splash-ef-14',
            resolve: {
                modalTitle: function () { return 'Add Menu' },
                entity: function () { return t; },
                possibleLocations : function () { return possibleLocations }
            }
        }).result.then(function (response) {
            menuResource.addMenu(response).then(function (result) {
                deferred.resolve(result);
            }, function (error) {
                deferred.reject(error);
            });
        });

        return deferred.promise;
    }

    function editMenu(menu, possibleLocations) {
        var deferred = $q.defer();

        $modal.open({
            templateUrl: '/app/scripts/menu/partials/menuEditModal.html',
            controller: 'MenuEditCtrl',
            size: 'lg',
            backdropClass: 'splash splash-2 splash-ef-14',
            windowClass: 'splash splash-2 splash-ef-14',
            resolve: {
                modalTitle: function () { return 'Edit Menu' },
                entity: function () { return menu; },
                possibleLocations: function() { return possibleLocations }
            }
        }).result.then(function (response) {
            menuResource.updateMenu(response).then(function (result) {
                menuResource.getMenuHeaders();
                deferred.resolve(result);
            }, function (error) {
                deferred.reject(error);
            });
        });

        return deferred.promise;
    }

    function importMenu(menu, possibleLocations) {
        var deferred = $q.defer();
        $modal.open({
            templateUrl: '/app/scripts/menu/partials/menuImportModal.html',
            controller: 'MenuImportCtrl',
            size: 'lg',
            backdropClass: 'splash splash-2 splash-ef-14',
            windowClass: 'splash splash-2 splash-ef-14',
            resolve: {
                modalTitle: function () { return 'Import Menu' },
                entity: function () { return menu; },
                possibleLocations: function () { return possibleLocations },
                plugins: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                      '/assets/vendor/filestyle/bootstrap-filestyle.min.js'
                    ]);
                }]
            },
        }).result.then(function (response) {
            //Re-upload the image, then update the menu item
            if (response.importFile !== undefined && response.importFile !== "") {
                var menuImportFile = helpers.dataURItoBlob(response.importFile);
                menuImportResource.importMenu(menuImportFile, response.menu.id).then(function (result) {
                    deferred.resolve(result);
                }, function (error) {
                    deferred.reject(error);
                });
            }
        });

        return deferred.promise;
    }

    function addSection() {
        var deferred = $q.defer();
        var s = menuResource.newSection();

        $modal.open({
            templateUrl: '/app/scripts/menu/partials/menuSectionEditModal.html',
            controller: 'MenuSectionEditCtrl',
            size: 'lg',
            backdropClass: 'splash splash-2 splash-ef-14',
            windowClass: 'splash splash-2 splash-ef-14',
            resolve: {
                modalTitle: function () { return 'Add Menu Section' },
                entity: function () { return s; }
            }
        }).result.then(function (response) {
            menuResource.addSection(response).then(function (result) {
                deferred.resolve(result);
            }, function (error) {
                deferred.reject(error);
            });
        });

        return deferred.promise;
    }


    function editSection(section) {
        var deferred = $q.defer();

        $modal.open({
            templateUrl: '/app/scripts/menu/partials/menuSectionEditModal.html',
            controller: 'MenuSectionEditCtrl',
            size: 'lg',
            backdropClass: 'splash splash-2 splash-ef-14',
            windowClass: 'splash splash-2 splash-ef-14',
            resolve: {
                modalTitle: function () { return 'Edit Menu Section' },
                entity: function () { return section; }
            }
        }).result.then(function (response) {
            menuResource.updateSection(response).then(function (result) {
                deferred.resolve(result);
            }, function (error) {
                deferred.reject(error);
            });
        });

        return deferred.promise;
    }


    function editItem(item) {
        var deferred = $q.defer();

        $modal.open({
            templateUrl: '/app/scripts/menu/partials/menuItemEditModal.html',
            controller: 'MenuItemEditCtrl',
            size: 'lg',
            backdropClass: 'splash splash-2 splash-ef-14',
            windowClass: 'splash splash-2 splash-ef-14',
            resolve: {
                modalTitle: function () { return 'Edit Menu Item' },
                entity: function () {
                    if (item.daysOfWeek != null && item.daysOfWeek.toString() != "[object Object]")
                        item.daysOfWeek = JSON.parse(item.daysOfWeek);
                    else
                        item.daysOfWeek = { "Sunday" : true, "Monday" : true, "Tuesday" : true, "Wednesday" : true, "Thursday" : true, "Friday" : true, "Saturday": true };
                    return item;
                },
                plugins: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                      '/assets/vendor/filestyle/bootstrap-filestyle.min.js'
                    ]);
                }]
            }
        }).result.then(function (response) {
            //Re-upload the image, then update the menu item
            if (response.croppedImage !== undefined && response.croppedImage !== "") {
                var imageBlob = helpers.dataURItoBlob(response.croppedImage);
                menuImageResource.uploadMenuImage(imageBlob, response.menuItem.id).then(function (result) {
                    if (result != null && result.data != null && result.data != "") {
                        response.menuItem.itemImageUrl = result.data;
                        menuResource.updateItem(response.menuItem).then(function (result) {
                            deferred.resolve(result);
                        }, function (error) {
                            deferred.reject(error);
                        });
                    }
                }, function (error) {
                    deferred.reject(error);
                });
            }
            else {
                menuResource.updateItem(response.menuItem).then(function (result) {
                    deferred.resolve(result);
                }, function (error) {
                    deferred.reject(error);
                });
            }
        });

        return deferred.promise;
    }


    function addItem(section) {
        var deferred = $q.defer();
        var item = menuResource.newItem(section);

        $modal.open({
            templateUrl: '/app/scripts/menu/partials/menuItemEditModal.html',
            controller: 'MenuItemEditCtrl',
            size: 'lg',
            backdropClass: 'splash splash-2 splash-ef-14',
            windowClass: 'splash splash-2 splash-ef-14',
            resolve: {
                modalTitle: function () { return 'Add Menu Item' },
                entity: function () { return item; },
                plugins: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                      '/assets/vendor/filestyle/bootstrap-filestyle.min.js'
                    ]);
                }]
            }
        }).result.then(function (response) {
            //Add the menu item, then get the menu id, upload the image, then update the menu item
            if (response.croppedImage !== undefined && response.croppedImage !== "") {
                var imageBlob = helpers.dataURItoBlob(response.croppedImage);
                menuResource.addItem(response.menuItem)
                .then(function (result) {
                    if (result != null && result.id != null && result.id != undefined) {
                        response.menuItem.id = result.id;
                    }
                })
                .then(function() {
                    menuImageResource.uploadMenuImage(imageBlob, response.menuItem.id)
                        .then(function (result) {
                            if(result != null && result.data != undefined)
                            response.menuItem.itemImageUrl = result.data;
                            menuResource.updateItem(response.menuItem)
                        .then(function (result) {
                            deferred.resolve(response.menuItem);
                        });
                    });
                }, function (error) {
                    deferred.reject(error);
                });
            }
            else {
                menuResource.addItem(response.menuItem).then(function (result) {
                    deferred.resolve(result);
                }, function (error) {
                    deferred.reject(error);
                });
            }
        });

        return deferred.promise;
    }

    return service;
}