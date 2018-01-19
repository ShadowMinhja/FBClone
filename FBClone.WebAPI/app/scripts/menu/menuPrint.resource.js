'use strict';

//TODO: Switch to use https://github.com/samwiseduzer/angularPrint

angular.module('fbCloneApp')
    .factory('menuPrintResource', ['$q', '$rootScope', '$http', '$compile', '$timeout', 'appSettings', 'currentUser', menuPrintResource])
function menuPrintResource($q, $rootScope, $http, $compile, $timeout, appSettings, currentUser) {
    var menuPrintResource = this;

    menuPrintResource.print = print;
    menuPrintResource.printHtml = printHtml;

    function print(templateUrl, data, preview) {
        $http.get(templateUrl).success(function (template) {
            var printScope = $rootScope.$new();
            angular.extend(printScope, data);
            var element = $compile($('<div>' + template + '</div>'))(printScope);
            var waitForRenderAndPrint = function () {
                if (printScope.$$phase || $http.pendingRequests.length) {
                    $timeout(waitForRenderAndPrint);
                } else {
                    if (!preview)
                        printHtml(element.html());
                    else 
                        openNewWindow(element.html());
                    printScope.$destroy();
                }
            };
            waitForRenderAndPrint();
        });
    };

    function printHtml(html) {
        var deferred = $q.defer();
        var hiddenFrame = $('<iframe></iframe>').appendTo('body')[0];
        $(hiddenFrame).load(function () {
            if (!hiddenFrame.contentDocument.execCommand('print', false, null)) {
                hiddenFrame.contentWindow.focus();
                hiddenFrame.contentWindow.print();
            }
            $(hiddenFrame).remove();
        });
        var htmlContent = "<!doctype html>"+
                    "<html>"+
                        '<body>' +
                            html +
                        '</body>'+
                    "</html>";
        var doc = hiddenFrame.contentWindow.document.open("text/html", "replace");
        doc.write(htmlContent);
        deferred.resolve();
        doc.close();
        return deferred.promise;
    }

    function openNewWindow(html) {
        var newWindow = window.open("MenuPreview.html");
        newWindow.addEventListener('load', function () {
            $(newWindow.document.body).html(html);
        }, false);
    };

    return menuPrintResource;
}