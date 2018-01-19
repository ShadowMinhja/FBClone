'use strict';

angular.module('fbCloneApp')
    .controller('MenuItemEditCtrl', ["$scope", "$modalInstance", "$timeout", "modalTitle", "entity", "menuImageResource", "helpers",
        function ($scope, $modalInstance, $timeout, modalTitle, entity, menuImageResource, helpers) {
            $scope.imageUrl = '';
            $scope.myImage = '';
            $scope.myCroppedImage = '';
            $scope.cropType = 'square';
        
            $scope.modalTitle = modalTitle;
            $scope.schema = {
                type: 'object',
                title: 'Edit Menu Item',
                properties: {
                    id: { type: 'string', title: 'ID', readonly: true },
                    sequence: { type: 'integer', title: 'Sequence' },
                    itemText: {
                        title: 'Item Text',
                        type: 'string',
                        format: "html"
                    },
                    price: {
                        title: 'Price',
                        type: 'number',
                    },
                    daysOfWeek : {
                        title: 'Days to Show Item',
                        type: 'object',
                        properties: {
                            "Sunday": {
                                type: 'boolean',
                                title: 'Sun&nbsp;',
                                "x-schema-form": { id: "sunday-checkbox", type: "checkbox-toggle", htmlClass: "orange" }
                            },
                            "Monday": {
                                type: 'boolean',
                                title: ' Mon&nbsp;',
                                "x-schema-form": { id: "monday-checkbox", type: "checkbox-toggle", htmlClass: "orange" }
                            },
                            "Tuesday": {
                                type: 'boolean',
                                title: ' Tue&nbsp;',
                                "x-schema-form": { id: "tuesday-checkbox", type: "checkbox-toggle", htmlClass: "orange" }
                            },
                            "Wednesday": {
                                type: 'boolean',
                                title: ' Wed&nbsp;',
                                "x-schema-form": { id: "wednesday-checkbox", type: "checkbox-toggle", htmlClass: "orange" }
                            },
                            "Thursday": {
                                type: 'boolean',
                                title: ' Thu&nbsp;',
                                "x-schema-form": { id: "thursday-checkbox", type: "checkbox-toggle", htmlClass: "orange" }
                            },
                            "Friday": {
                                type: 'boolean',
                                title: ' Fri&nbsp;',
                                "x-schema-form": { id: "friday-checkbox", type: "checkbox-toggle", htmlClass: "orange" }
                            },
                            "Saturday": {
                                type: 'boolean',
                                title: ' Sat',
                                "x-schema-form": { id: "saturday-checkbox", type: "checkbox-toggle", htmlClass: "orange" }
                            }
                        }
                    },
                    active: {
                        title: 'Active',
                        type: 'string',
                        disableSuccessState: true,
                        enum: ['Active', 'Inactive']
                    }
                },
                required: ["sequence", "itemText", "price", "active"]
            };
            $scope.form = [
               {
                   type: "section",
                   htmlClass: "row",
                   items: [
                       {
                           type: "section",
                           htmlClass: "col-md-4",
                           items: [
                               "id"
                           ]
                       },
                       {
                           type: "section",
                           htmlClass: "col-md-4",
                           items: [
                               "sequence"
                           ]
                       },
                   ]
               },
               {
                   type: "section",
                   htmlClass: "row",
                   items: [
                       {
                           type: "section",
                           htmlClass: "col-md-12",
                           items: [
                              {
                                  key: "itemText",
                                  tinymceOptions: {
                                      menubar: false,
                                      plugins: "textcolor colorpicker",
                                      toolbar: [
                                        "undo redo | styleselect  | bold italic | alignleft aligncenter alignright alignjustify | fontselect fontsizeselect forecolor | bullist numlist | outdent indent "
                                      ]
                                  },
                                  validationMessage: "Required",
                              },
                           ]
                       }
                   ]
               },
               {
                   type: "section",
                   htmlClass: "row",
                   items: [
                       {
                           type: "section",
                           htmlClass: "col-md-3",
                           items: [
                               {
                                   key: "price",
                               }
                           ]
                       },
                       {
                           type: "section",
                           htmlClass: "col-md-9",
                           items: [
                               {
                                   type: "section", htmlClass: "row",
                                   items: [
                                       {
                                           type: "section", htmlClass: "col-md-offset-1 col-md-11 text-bold text-italic", items: [{ "title": "Days to Show On Menu" }]
                                       }
                                   ]
                               },
                               {
                                   type: "section", htmlClass: "row",
                                   items: [
                                       {
                                           type: "section", htmlClass: "col-md-offset-1 col-md-11", 
                                           items: [
                                               "daysOfWeek.Sunday",
                                               "daysOfWeek.Monday",
                                               "daysOfWeek.Tuesday",
                                               "daysOfWeek.Wednesday",
                                               "daysOfWeek.Thursday",
                                               "daysOfWeek.Friday",
                                               "daysOfWeek.Saturday"
                                           ]
                                       }, 
                                   
                                   ]
                               },
                           ],
                       }
                   ]
               },
               {
                   type: "section",
                   htmlClass: "row",
                   items: [               
                       {
                           type: "section",
                           htmlClass: "col-md-6",
                           items: [
                                "active"
                           ]
                       }
                   ],
                   disableSuccessState: "true"
               }
            ];

            $scope.entity = angular.copy(entity);
            $scope.save = save;
            $scope.cancel = cancel;

            $scope.showBackImage = showBackImage;

            init();

            function init() {
                //tinymce.remove();
                refreshLogo();
            }

            function refreshLogo() {
                if (entity.itemImageUrl !== undefined) {
                    $scope.imageUrl = entity.itemImageUrl;
                }
            }

            function cancel() {
                $modalInstance.dismiss('cancel');
            };

            function save() {
                $scope.$broadcast('schemaFormValidate');
                if ($scope.modalForm.$valid) {
                    // Copy row values over
                    entity = angular.extend(entity, $scope.entity);
                    $modalInstance.close({ "menuItem": entity, "croppedImage": $scope.myCroppedImage});
                }

            }
            
            function showBackImage() {
                if ($scope.myCroppedImage == "")
                    return true;
                else
                    return false;
            }

            var handleFileSelect = function (evt) {
                var file = evt.currentTarget.files[0];
                var reader = new FileReader();
                reader.onload = function (evt) {
                    $scope.$apply(function ($scope) {
                        $scope.myImage = evt.target.result;
                    });
                };
                reader.readAsDataURL(file);
            };
            $timeout(function () {
                angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);
            }, 500);

        }
]);