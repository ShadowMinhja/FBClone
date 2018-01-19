﻿/**********
  The following directives are necessary in order to track dirty state and validity of the rows 
  in the table as the user pages within the grid
------------------------
*/

(function () {
    angular.module("fbCloneApp").directive("rowTrackedTable", rowTrackedTable);

    rowTrackedTable.$inject = [];

    function rowTrackedTable() {
        return {
            restrict: "A",
            priority: -1,
            require: "ngForm",
            controller: rowTrackedTableController
        };
    }

    rowTrackedTableController.$inject = ["$scope", "$parse", "$attrs", "$element"];

    function rowTrackedTableController($scope, $parse, $attrs, $element) {
        var self = this;
        var tableForm = $element.controller("form");
        var dirtyCellsByRow = [];
        var invalidCellsByRow = [];

        init();

        ////////

        function init() {
            var setter = $parse($attrs.rowTrackedTable).assign;
            setter($scope, self);
            //$scope.$on("$destroy", function () {
            //    setter(null);
            //});

            self.reset = reset;
            self.isCellDirty = isCellDirty;
            self.setCellDirty = setCellDirty;
            self.setCellInvalid = setCellInvalid;
            self.untrack = untrack;
        }

        function getCellsForRow(row, cellsByRow) {
            return _.find(cellsByRow, function (entry) {
                return entry.row === row;
            })
        }

        function isCellDirty(row, cell) {
            var rowCells = getCellsForRow(row, dirtyCellsByRow);
            return rowCells && rowCells.cells.indexOf(cell) !== -1;
        }

        function reset() {
            dirtyCellsByRow = [];
            invalidCellsByRow = [];
            setInvalid(false);
        }

        function setCellDirty(row, cell, isDirty) {
            setCellStatus(row, cell, isDirty, dirtyCellsByRow);
        }

        function setCellInvalid(row, cell, isInvalid) {
            setCellStatus(row, cell, isInvalid, invalidCellsByRow);
            setInvalid(invalidCellsByRow.length > 0);
        }

        function setCellStatus(row, cell, value, cellsByRow) {
            var rowCells = getCellsForRow(row, cellsByRow);
            if (!rowCells && !value) {
                return;
            }

            if (value) {
                if (!rowCells) {
                    rowCells = {
                        row: row,
                        cells: []
                    };
                    cellsByRow.push(rowCells);
                }
                if (rowCells.cells.indexOf(cell) === -1) {
                    rowCells.cells.push(cell);
                }
            } else {
                _.remove(rowCells.cells, function (item) {
                    return cell === item;
                });
                if (rowCells.cells.length === 0) {
                    _.remove(cellsByRow, function (item) {
                        return rowCells === item;
                    });
                }
            }
        }

        function setInvalid(isInvalid) {
            self.$invalid = isInvalid;
            self.$valid = !isInvalid;
        }

        function untrack(row) {
            _.remove(invalidCellsByRow, function (item) {
                return item.row === row;
            });
            _.remove(dirtyCellsByRow, function (item) {
                return item.row === row;
            });
            setInvalid(invalidCellsByRow.length > 0);
        }
    }
})();

(function () {
    angular.module("fbCloneApp").directive("rowTrackedTableRow", rowTrackedTableRow);

    rowTrackedTableRow.$inject = [];

    function rowTrackedTableRow() {
        return {
            restrict: "A",
            priority: -1,
            require: ["^rowTrackedTable", "ngForm"],
            controller: rowTrackedTableRowController
        };
    }

    rowTrackedTableRowController.$inject = ["$attrs", "$element", "$parse", "$scope"];

    function rowTrackedTableRowController($attrs, $element, $parse, $scope) {
        var self = this;
        var row = $parse($attrs.rowTrackedTableRow)($scope);
        var rowFormCtrl = $element.controller("form");
        var trackedTableCtrl = $element.controller("rowTrackedTable");

        self.isCellDirty = isCellDirty;
        self.setCellDirty = setCellDirty;
        self.setCellInvalid = setCellInvalid;

        function isCellDirty(cell) {
            return trackedTableCtrl.isCellDirty(row, cell);
        }

        function setCellDirty(cell, isDirty) {
            trackedTableCtrl.setCellDirty(row, cell, isDirty)
        }

        function setCellInvalid(cell, isInvalid) {
            trackedTableCtrl.setCellInvalid(row, cell, isInvalid)
        }
    }
})();

(function () {
    angular.module("fbCloneApp").directive("rowTrackedTableCell", rowTrackedTableCell);

    rowTrackedTableCell.$inject = [];

    function rowTrackedTableCell() {
        return {
            restrict: "A",
            priority: -1,
            scope: true,
            require: ["^rowTrackedTableRow", "ngForm"],
            controller: rowTrackedTableCellController
        };
    }

    rowTrackedTableCellController.$inject = ["$attrs", "$element", "$scope"];

    function rowTrackedTableCellController($attrs, $element, $scope) {
        var self = this;
        var cellFormCtrl = $element.controller("form");
        var cellName = cellFormCtrl.$name;
        var trackedTableRowCtrl = $element.controller("rowTrackedTableRow");

        if (trackedTableRowCtrl.isCellDirty(cellName)) {
            cellFormCtrl.$setDirty();
        } else {
            cellFormCtrl.$setPristine();
        }
        // note: we don't have to force setting validaty as angular will run validations
        // when we page back to a row that contains invalid data

        $scope.$watch(function () {
            return cellFormCtrl.$dirty;
        }, function (newValue, oldValue) {
            if (newValue === oldValue) return;

            trackedTableRowCtrl.setCellDirty(cellName, newValue);
        });

        $scope.$watch(function () {
            return cellFormCtrl.$invalid;
        }, function (newValue, oldValue) {
            if (newValue === oldValue) return;

            trackedTableRowCtrl.setCellInvalid(cellName, newValue);
        });
    }
})();