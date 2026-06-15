/*
  SecondarySalesUploadCtrl.js
 */
app.controller('SecondarySalesUploadCtrl', ['$scope', 'hostDire', 'crudService', 'conversion', '$http', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, hostDire, crudService, conversion, $http, $filter, $localStorage, uiGridConstants) {

        var baseUrl = '/Sales/api/SecondarySalesUpload/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptions = [];
        var page = 1;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;
        $scope.loadLoadingProcessArticleNo = false;
        $scope.LoadingMessage = 'Loading...';
        $scope.PageTitle = 'Secondary Sales Upload';
        $scope.ListTitleDetail = 'Parent Number List';
        $scope.ListTitle = 'Document List';
        $scope.IsHidden = true;
        $scope.IsShow = false;
        //$scope.IsBrows = true;
        $scope.FileId = null;
        $scope.FileName = '';
        $scope.FileSize = '';
        $scope.FileType = '';
      
        $scope.DocumentRows = [];
        $scope.ListSelectedDocuments = [];
        $scope.ListDDLDocuments = [];
        $scope.DocumentLists = [];
        $scope.ListParentDocuments = [];
        $scope.DocumentPaths = "";
        $scope.DelParentDocuments = [];
        files = [];
        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmSecondarySalesUpload'; DelFunc = 'DeleteDocumentListData'; DelMsg = 'DocName'; EditFunc = 'getDocumentListByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 
        
        

        $scope.OpenFile = function (datafile) {
            debugger
            //window.open(datafile.ViewPath);
            var myWindow = window.open();
            myWindow.document.write('<html><head><title>' + datafile.FileName + '</title></head><body height="100%" width="100%"><iframe src="' + datafile.ViewPath + '" height="100%" width="100%"></iframe></body></html>');
        }


        $scope.Save = function () {


            var itemMaster = {
                START_DATE: conversion.getStringToDate($scope.StartDate),
                END_DATE: conversion.getStringToDate($scope.EndDate)
            };

            var HeaderTokenPutPost = $scope.HeaderToken.post;
            ModelsArray = [itemMaster];
            var apiRoute = baseUrl + 'KeepSecondarySalesDate/';
            var DistTargetSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            DistTargetSaveUpdate.then(function (response) {
                var result = 0;
                //message = $scope.DistributorTargetID == 0 ? 'Saved' : 'Updated';
                if (response != "") {
                    //$scope.clear();
                    //$scope.DistributorTargetNo = response;
                    //ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                }
                else if (response == "") {
                    //ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    ////$("#save").prop("disabled", false);
                }
            },
                function (error) {
                    //ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                });



            $scope.cmnParam();

            files = document.getElementById('file').files;

            if (files.length > 0) {
                $scope.uploadDocumentFile(files);
            }
            else {
                Command: toastr["warning"]("No file chosen");
            }

        }
        
        $scope.uploadDocumentFile = function (files) {
            var formData = new FormData();
            $scope.DocumentPaths = "";
            angular.forEach(files, function (file, index) {
                formData.append("uploadedFile", files[index]);
                $scope.DocumentPaths = { FilePath: $scope.FilePath, VirtualPath: $scope.VirtualPath };
            });

            var upURL = hostDire + baseUrl;
            $http.post(upURL + "UploadDocuments/", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (response) {

                    if (response == '1') {
                        ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                        //isSave = 1;
                        $scope.clear();
                    }
                    else {
                        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    }

                    //$scope.FileId = 0;
                    //$scope.FileName = response.ListDocuments[0].FileName;
                    //$scope.FileSize = response.ListDocuments[0].FileSize;
                    //$scope.FileType = response.ListDocuments[0].FileType;
                    //$scope.FilePath = response.ListDocuments[0].FilePath;
                    //$scope.VirtualPath = response.ListDocuments[0].VirtualPath;
                })
                .error(function () {

                });



            //SaveMasterDetail.then(function (response) {
            //    if (response == '1') {
            //        ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
            //        //isSave = 1;
            //        $scope.clear();
            //    }
            //    else {
            //        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
            //    }
            //},
            //    function (error) {
            //        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
            //        console.log("Error: " + error);
            //    });

        };

        //$scope.onUpdateDocument = function () {
        //    var master = {
        //        DocName: $scope.DocName.replace(/[' ']/g, ''),

        //        Remarks: $scope.Remarks,
        //        DocumentTypeID: $scope.DocumentTypeID,
        //        DocumentPahtID: $scope.DocumentPahtID,
        //        DocumentTypeName: $scope.DocumentTypeName,
        //        FilePath: $scope.FilePath,
        //        VirtualPath: $scope.VirtualPath,
        //        FileId: $scope.FileId,
        //        FileName: $scope.FileName,
        //        FileSize: $scope.FileSize,
        //        FileType: $scope.FileType
        //    }

        //    ModelsArray = [master, $scope.DocumentLists, objcmnParam];
        //    var apiRoute = baseUrl + 'SaveUpdateDocumentList/';
        //    var SaveMasterDetail = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.put);
        //    SaveMasterDetail.then(function (response) {
        //        if (response == '1') {
        //            ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
        //            //isSave = 1;
        //            $scope.clear();
        //        }
        //        else {
        //            ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
        //        }
        //    },
        //        function (error) {
        //            ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
        //            console.log("Error: " + error);
        //        });
        //}

        $scope.clear = function () {
            $scope.frmSecondarySalesUpload.$setPristine();
            $scope.frmSecondarySalesUpload.$setUntouched();

            $scope.DocName = '';
            $scope.Remarks = null;
            $scope.DocumentTypeID = null;
            $scope.DocumentPahtID = null;
            $scope.ParentTypeID = null;
            $scope.DocumentTypeName = '';
            $scope.FilePath = '';
            $scope.VirtualPath = '';
            $scope.FileId = 0;
            $scope.FileName = '';
            $scope.FileSize = '';
            $scope.FileType = '';
            $scope.PrevDocumentTypeID = null;
            $scope.PrevFilePath = '';
            $scope.PrevVirtualPath = '';
            $scope.ListSelectedDocuments = [];
            $scope.ListDDLDocuments = [];
            $scope.DocumentLists = [];
            $scope.ListParentDocuments = [];
            $scope.DocumentPaths = "";
            $scope.DelParentDocuments = [];
            $scope.gridOptions.data = [];
            $scope.IsShow = false;
            $scope.IsHidden = true;
            files = [];
            $('#file').val('');
        }
        
        //$scope.ShowHide = function () {
        //    $scope.IsHidden = $scope.IsHidden ? false : true;
        //    if ($scope.IsHidden == true) {
        //        $scope.clear();
        //    }
        //    else {
        //        $scope.RefreshMasterList();
        //        $scope.IsShow = false;
        //    }
        //};

        //}
             

        //$scope.ClearDocumentFile = function () {
        //    $scope.ListSelectedDocuments = [];
        //    $scope.ListDDLDocuments = [];
        //    $scope.DocumentLists = [];
        //    $scope.DelParentDocuments = [];
        //    $scope.ListParentDocuments = [];
        //    $scope.IsShow = $scope.ListParentDocuments.length > 0 ? true : false;
        //}

       
    }]);