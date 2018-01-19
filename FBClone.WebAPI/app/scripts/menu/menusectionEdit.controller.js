'use strict';

angular.module('fbCloneApp')
    .controller('MenuSectionEditCtrl', function ($scope, $modalInstance, modalTitle, entity) {

        $scope.modalTitle = modalTitle;

        $scope.schema = {
            type: 'object',
            title: 'Edit Menu Section',
            properties: {
                id: { type: 'string', title: 'ID', readonly: true },
                sequence: { type: 'integer', title: 'Sequence' },
                sectionTitle: {
                    title: 'Section Title',
                    type: 'string',
                    maxLength: 50
                },
                sectionSubTitle: {
                    title: 'Section Title Line 2',
                    type: 'string',
                    maxLength: 50
                },
                active: {
                    title: 'Active',
                    type: 'string',
                    disableSuccessState: true,
                    enum: ['Active', 'Inactive']
                }
            },
            required: ["sequence", "sectionTitle", "active"]
        };


        $scope.form = ['*'];

        $scope.entity = angular.copy(entity);

        $scope.save = save;
        $scope.cancel = cancel;

        function cancel() {
            $modalInstance.dismiss('cancel');
        };

        function save() {
            $scope.$broadcast('schemaFormValidate');
            if ($scope.modalForm.$valid) {
                // Copy row values over
                entity = angular.extend(entity, $scope.entity);
                $modalInstance.close(entity);
            }
        }
    }
);