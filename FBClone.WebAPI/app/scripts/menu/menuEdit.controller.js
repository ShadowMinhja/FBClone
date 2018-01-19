'use strict';

angular.module('fbCloneApp').controller('MenuEditCtrl', function ($scope, $modalInstance, modalTitle, entity, possibleLocations, locationResource) {
    $scope.possibleLocations = angular.copy(possibleLocations); //Removes the $$hashKey
    $scope.modalTitle = modalTitle;    

    $scope.entity = angular.copy(entity);

    $scope.save = save;
    $scope.cancel = cancel;

    init();

    function init() {
        var modelLocations = $scope.entity.locations;
        //Workaround, select list unable to match up directly with entity.locations
        var locationsSelectList = [];
        _.forEach($scope.possibleLocations, function (obj, key) {
            if (_.find(modelLocations, { "id": obj.id }) !== undefined) {
                locationsSelectList.push(obj);
            }
        });
        $scope.entity.locations = locationsSelectList;
    }

    function cancel() {
        $modalInstance.dismiss('cancel');
    };

    function save() {        
        entity = angular.extend(entity, $scope.entity);
        $modalInstance.close(entity);
    }
});