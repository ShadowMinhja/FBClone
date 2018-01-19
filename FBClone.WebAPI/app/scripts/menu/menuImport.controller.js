'use strict';

angular.module('fbCloneApp')
    .controller('MenuImportCtrl', ["$scope", "$modalInstance", "$timeout", "modalTitle", "entity", "possibleLocations", "helpers",
        function ($scope, $modalInstance, $timeout, modalTitle, entity, possibleLocations, helpers) {
            $scope.entity = angular.copy(entity);
            $scope.importFile = '';

            $scope.importCsv = importCsv;
            $scope.cancel = cancel;

            function cancel() {
                $modalInstance.dismiss('cancel');
            };

            function importCsv() {
                entity = angular.extend(entity, $scope.entity);
                $modalInstance.close({ "menu": entity, "importFile": $scope.importFile });
            }

            var handleFileSelect = function (evt) {
                var file = evt.currentTarget.files[0];
                var reader = new FileReader();
                reader.onload = function (evt) {
                    $scope.$apply(function ($scope) {
                        $scope.importFile = evt.target.result;
                    });
                };
                reader.readAsDataURL(file);
            };
            $timeout(function () {
                angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);
            }, 500);
        }
    ]);