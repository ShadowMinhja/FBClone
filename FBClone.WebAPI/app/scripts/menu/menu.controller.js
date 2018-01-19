'use strict';

angular.module('fbCloneApp')
    .controller('MenuCtrl', ['$scope', '$sce', '$anchorScroll', '$location', 'toastr', 'toastrConfig', 'menuResource', 'menuEditResource', 'menuPrintResource', 'locationResource',
        function ($scope, $sce, $anchorScroll, $location, toastr, toastrConfig, menuResource, menuEditResource, menuPrintResource, locationResource) {
            $scope.page = {
                title: 'Manage Menu',
                subtitle: 'Build Your Store Menu So Customer Can Browse and Order',
            };
            var self = this;

            var currentSection = null;
            var currentUser = {};
            var selectedMenu = {};
            var possibleLocations = [];

            $scope.keywords = ""
            $scope.bookmarkAnchors = [];
            $scope.menus = menuResource.menuHeaders;
            $scope.menu = menuResource.currentMenu;

            $scope.$watch(
                function () { return menuResource.menuHeaders; },
                function (newVal) {
                    $scope.menus = newVal;
                }
            );

            $scope.$watch(
                function () { return menuResource.currentMenu; },
                function (newVal) {
                    newVal.menuSections = _.sortBy(newVal.menuSections, function (obj) { return obj.sequence; });
                    $scope.menu = newVal;
                }
            );

            $scope.currentSection = currentSection;
            $scope.currentUser = currentUser;
            $scope.selectedMenu = selectedMenu;
            $scope.isSectionCurrent = isSectionCurrent;
            $scope.setMenu = setMenu;
            $scope.setCurrentSection = setCurrentSection;
            $scope.addMenu = addMenu;
            $scope.editMenu = editMenu;
            $scope.addNewSection = addNewSection;
            $scope.editSection = editSection;
            $scope.deleteSection = deleteSection;
            $scope.editItem = editItem;
            $scope.deleteItem = deleteItem;
            $scope.addItem = addItem;
            $scope.printMenu = printMenu;
            $scope.importMenu = importMenu;
            $scope.highlightSearchString = highlightSearchString;
            $scope.keywordChanged = keywordChanged;
            $scope.gotoAnchor = gotoAnchor;

            $scope.getTrustedHtml = getTrustedHtml;

            init();

            function setMenu(item, model) {
                menuResource.setMenuByID(model.id).then(function (result) {
                    $scope.menu = menuResource.currentMenu;
                    $scope.currentSection = $scope.menu.sections !== undefined ? $scope.menu.sections[0] : null;
                });
            }

            function addMenu() {
                menuEditResource.addMenu(possibleLocations).then(function (result) {
                    $scope.currentSection = null;
                    _.defer(function () {
                        var t = _.findWhere($scope.menus, { id: result.id });
                        if (t) {
                            $scope.selectedMenu.selected = t;
                            $scope.currentSection = $scope.menu.sections[0];
                        }
                        toastrConfig.positionClass = "toast-top-full-width";
                        toastr["success"](result.message, "Menu Add Success", {
                            iconClass: 'bg-greensea',
                            iconType: 'fa-check'
                        });
                    });

                }, function (error) {
                    toastr["error"](error, "Menu Add Error", {
                        iconClass: 'bg-lightred',
                        iconType: 'fa-warning'
                    });
                });

            }

            function editMenu() {
                menuEditResource.editMenu($scope.menu, possibleLocations).then(function (result) {
                    toastrConfig.positionClass = "toast-top-full-width";
                    toastr["success"](result.message, "Menu Edit Success", {
                        iconClass: 'bg-greensea',
                        iconType: 'fa-check'
                    });
                }, function (error) {
                    toastr["error"](error, "Menu Edit Error", {
                        iconClass: 'bg-lightred',
                        iconType: 'fa-warning'
                    });
                });
            }

            function addNewSection() {
                menuEditResource.addSection().then(function (result) {
                    toastrConfig.positionClass = "toast-top-full-width";
                    toastr["success"](result.message, "Menu Section Add Success", {
                        iconClass: 'bg-greensea',
                        iconType: 'fa-check'
                    });
                }, function (error) {
                    toastr["error"](error, "Menu Section Add Error", {
                        iconClass: 'bg-lightred',
                        iconType: 'fa-warning'
                    });
                });
            }

            function editSection(section) {
                menuEditResource.editSection(section).then(function (result) {
                    toastrConfig.positionClass = "toast-top-full-width";
                    toastr["success"](result.message, "Menu Section Edit Success", {
                        iconClass: 'bg-greensea',
                        iconType: 'fa-check'
                    });
                }, function (error) {
                    toastr["error"](error, "Menu Section Edit Error", {
                        iconClass: 'bg-lightred',
                        iconType: 'fa-warning'
                    });
                });
            }

            function deleteSection(section) {
                menuResource.deleteSection(section).then(function (result) {
                    toastrConfig.positionClass = "toast-top-full-width";
                    toastr["success"](result.message, "Menu Section Delete Success", {
                        iconClass: 'bg-greensea',
                        iconType: 'fa-check'
                    });
                }, function (error) {
                    toastr["error"](error, "Menu Section Delete Error", {
                        iconClass: 'bg-lightred',
                        iconType: 'fa-warning'
                    });
                });
            }

            function addItem() {
                menuEditResource.addItem($scope.currentSection).then(function (result) {
                    toastrConfig.positionClass = "toast-top-full-width";
                    toastr["success"](result.message, "Menu Add Success", {
                        iconClass: 'bg-greensea',
                        iconType: 'fa-check'
                    });
                }, function (error) {
                    toastr["error"](error, "Menu Item Add Error", {
                        iconClass: 'bg-lightred',
                        iconType: 'fa-warning'
                    });
                });
            }

            function editItem(item) {
                menuEditResource.editItem(item).then(function (result) {
                    toastr["success"](result.message, "Menu Edit Success", {
                        iconClass: 'bg-greensea',
                        iconType: 'fa-check'
                    });
                }, function (error) {
                    toastr["error"](error, "Menu Item Edit Error", {
                        iconClass: 'bg-lightred',
                        iconType: 'fa-warning'
                    });
                });
            }

            function deleteItem(item) {
                menuResource.deleteItem($scope.currentSection, item).then(function (result) {
                    toastrConfig.positionClass = "toast-top-full-width";
                    toastr["success"](result.message, "Menu Delete Success", {
                        iconClass: 'bg-greensea',
                        iconType: 'fa-check'
                    });
                }, function (error) {
                    toastr["error"](error, "Menu Item Delete Error", {
                        iconClass: 'bg-lightred',
                        iconType: 'fa-warning'
                    });
                });
            }

            //Methods
            function init() {
                if (menuResource.menuHeaders.length === 0) {
                    menuResource.getMenuHeaders().then(function (result) {

                    }, function (error) {
                        console.log("Error getting menu headers");
                    });

                }
                locationResource.query(function (data) {
                    //Have to use _.map to convert resource to object for multiselect (_.filter doens't work)
                    possibleLocations = _.map(data, function (obj, key) {
                        obj.longName = obj.name + " - " + obj.address;
                        return obj;
                    });
                });

            }

            function setCurrentSection(section) {
                section.menuItems = _.sortBy(section.menuItems, function (obj) { return obj.sequence; });
                $scope.currentSection = section;
            }

            function isSectionCurrent(section) {
                return $scope.currentSection === section;
            }

            function printMenu(preview) {
                menuPrintResource.print('/app/views/tmpl/menu/menuPrintTemplate.html', $scope.selectedMenu.selected, preview);
            }

            function importMenu() {
                menuEditResource.importMenu($scope.menu, possibleLocations).then(function (result) {
                    if (result != null && result.data != null)
                        $scope.menu = menuResource.currentMenu = result.data;
                    toastrConfig.positionClass = "toast-top-full-width";
                    toastr["success"](result.message, "Menu Import Success", {
                        iconClass: 'bg-greensea',
                        iconType: 'fa-check'
                    });
                }, function (error) {
                    toastr["error"](error, "Menu Add Error", {
                        iconClass: 'bg-lightred',
                        iconType: 'fa-warning'
                    });
                });
            }

            function highlightSearchString(text, search) {
                if (!search) {
                    return $sce.trustAsHtml(text);
                }
                return $sce.trustAsHtml(text.replace(new RegExp(search, 'gi'), '<span class="search-highlight">$&</span>'));
            }

            function keywordChanged(keywords) {
                $scope.bookmarkAnchors = [];
                //Loop through each section
                _.forEach($scope.menu.menuSections, function (obj, key) {
                    //For each section loop through menu items
                    _.forEach(obj.menuItems, function (o) {
                        if (o.itemText.toUpperCase().indexOf(keywords.toUpperCase()) != -1) {
                            //Add Matched Item to Anchors
                            $scope.bookmarkAnchors.push({ "menuSection": obj, "menuItem": o });
                        }
                    });
                });
            }

            function gotoAnchor(bookmarkAnchor) {
                var tabHash = 'tab' + bookmarkAnchor.menuSection.id;
                var menuItemHash = 'menuItem' + bookmarkAnchor.menuItem.id;
                //Go to menu section tab
                if ($location.hash() !== tabHash) {
                    $location.hash(tabHash);
                    setCurrentSection(bookmarkAnchor.menuSection);
                }
                else {
                    $anchorScroll();
                }
                //Go to menu item
                if ($location.hash() !== menuItemHash) {
                    $location.hash(menuItemHash);
                }
                else {
                    $anchorScroll();
                }
            }

            function getTrustedHtml(html) {
                var s = $sce.trustAsHtml(html);

                return s;
            }


            $scope.sortableOptions = {
                handle: '.dragHandle',
                update: function (e, ui) {

                },
                stop: function (e, ui) {
                    menuResource.reorderSections().then(function (result) {
                        toastrConfig.positionClass = "toast-top-full-width";
                        toastr["success"](result.message, "Sequence Changed Success", {
                            iconClass: 'bg-greensea',
                            iconType: 'fa-check'
                        });
                    }, function (error) {
                        toastr["error"](error, "Menu Item Reorder Error", {
                            iconClass: 'bg-lightred',
                            iconType: 'fa-warning'
                        });
                    });

                },
                items: "li:not(.not-sortable)"
            };
        }
]);
