'use strict';

angular.module('fbCloneApp')

    .controller('BrandingCtrl', ["$scope", "$state", "$filter", "$modal", "appSettings", "currentUser", "FileUploader", "toastr", "toastrConfig", "brandingResource", "applicationSettingService", "helpers",
    function ($scope, $state, $filter, $modal, appSettings, currentUser, FileUploader, toastr, toastrConfig, brandingResource, applicationSettingService, helpers) {
            $scope.page = {
                title: 'My Branding',
                subtitle: 'Add Your Logo and Personalizations',
                imageUrl: "/assets/images/logoSample.jpg", 
            };
            $scope.myImage = '';
            $scope.myCroppedImage = '';
            $scope.uploadCroppedImage = uploadCroppedImage;

            var uploader = $scope.uploader = new FileUploader({
                url: appSettings.serverPath + "/api/Branding",
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token },
            });

            var self = this;
            var isFirstTime = false;
            var originalData = [];
            init();
        
            function init() {
                refreshLogo();
                if (applicationSettingService.settings == null) {
                    //Get Application Setting
                    applicationSettingService.retrieveSettings().then(function (data) {
                        if (data != null) {
                            applicationSettingService.settings = data;
                            if (applicationSettingService.settings.brandingSetup == false) {
                                openFirstTimeMessage();
                                isFirstTime = true;
                            }
                        }
                    });
                }
                else {
                    if (applicationSettingService.settings.brandingSetup == false) {
                        openFirstTimeMessage();
                        isFirstTime = true;
                    }
                }
            }

            function uploadCroppedImage() {
                //Remove original file
                uploader.clearQueue();

                //Change cropped image into uploadable blob
                var file = helpers.dataURItoBlob($scope.myCroppedImage);
                uploader.addToQueue(file);
                uploader.uploadAll();
            }

            function refreshLogo() {
                brandingResource.getLogoUrl().then(function (data) {
                    if (data != null)
                        $scope.page.imageUrl = data;
                });
            }

            function openFirstTimeMessage() {
                var options = null;//angular.element(event.target.parentElement).data('options');

                var modalInstance = $modal.open({
                    templateUrl: 'firstTimeBrandingEntry.html',
                    controller: 'ModalInstanceCtrl',
                    size: 'lg',
                    backdropClass: 'splash splash-2 splash-ef-14',
                    windowClass: 'splash splash-2 splash-ef-14',
                    resolve: {
                        items: function () {
                            return [];
                        }
                    }
                });
            }
            uploader.filters.push({
                name: 'imageFilter',
                fn: function (item /*{File|FileLikeObject}*/, options) {                    
                    var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                    return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
                }
            });

            $scope.showUploaderHint = function () {
                return uploader.queue.length > 0 ? true : false;
            }

            var handleFileSelect = function (evt) {
                var file = evt.currentTarget.files[0];
                //uploader.addToQueue(file);
                var reader = new FileReader();
                reader.onload = function (evt) {
                    $scope.$apply(function ($scope) {
                        $scope.myImage = evt.target.result;
                        uploader.addToQueue($scope.myCroppedImage);
                    });
                };
                reader.readAsDataURL(file);
            };
            angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);


            // CALLBACKS

            uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
                console.info('onWhenAddingFileFailed', item, filter, options);
            };
            uploader.onAfterAddingFile = function (fileItem) {    
                var file = fileItem._file;                
                var reader = new FileReader();
                reader.onload = function (evt) {
                    $scope.$apply(function ($scope) {
                        $scope.myImage = evt.target.result;
                    });
                };
                reader.readAsDataURL(file);
                console.info('onAfterAddingFile', fileItem);
            };
            uploader.onAfterAddingAll = function (addedFileItems) {
                console.info('onAfterAddingAll', addedFileItems);
            };
            uploader.onBeforeUploadItem = function (item) {
                $scope.page.imageUrl = "/app/images/ajax-loader.gif";
                console.info('onBeforeUploadItem', item);
            };
            uploader.onProgressItem = function (fileItem, progress) {
                console.info('onProgressItem', fileItem, progress);
            };
            uploader.onProgressAll = function (progress) {
                console.info('onProgressAll', progress);
            };
            uploader.onSuccessItem = function (fileItem, response, status, headers) {
                $scope.page.imageUrl = response + "?" + moment().unix().toString();
                uploader.clearQueue()
                if (isFirstTime == true) {
                    toastrConfig.timeOut = 7500;
                    toastrConfig.positionClass = "toast-top-full-width";
                    toastrConfig.onHidden = function () {
                        $state.go("app.dashboard");
                    }
                    toastr["success"]("Branding logo uploaded succesfully!  Return to this page later if you wish to change it.", "Step 3 Success", {
                        iconClass: 'bg-greensea',
                        iconType: 'fa-check'
                    });
                }
                else {
                    toastr["success"]("Yay! Your logo was uploaded successfully!", "Logo Uploaded Successfully!", {
                        iconClass: 'bg-greensea',
                        iconType: 'fa-check'
                    });
                }
                console.info('onSuccessItem', fileItem, response, status, headers);
            };
            uploader.onErrorItem = function (fileItem, response, status, headers) {                
                console.info('onErrorItem', fileItem, response, status, headers);
                if(response.indexOf("error") != -1)
                {
                    toastr["error"]("Oh no! Something went wrong with the upload. Please try again.", "Logo Upload Error", {
                        iconClass: 'bg-lightred',
                        iconType: 'fa-warning'
                    });
                    $scope.page.imageUrl = "/app/images/logo-placeholder.jpg";
                    fileItem.uploader.clearQueue();
                }
            };
            uploader.onCancelItem = function (fileItem, response, status, headers) {
                console.info('onCancelItem', fileItem, response, status, headers);
            };
            uploader.onCompleteItem = function (fileItem, response, status, headers) {
                if (response.indexOf("Maximum request length exceeded") > 0) {
                    toastr["error"]("The image file is too large. Please try another file.", "Promotion Upload Error", {
                        iconClass: 'bg-lightred',
                        iconType: 'fa-warning'
                    });
                    $scope.page.imageUrl = "/app/images/logo-placeholder.jpg";
                    fileItem.uploader.clearQueue();
                }
                console.info('onCompleteItem', fileItem, response, status, headers);
            };
            uploader.onCompleteAll = function () {
                console.info('onCompleteAll');
            };
        }
    ]);