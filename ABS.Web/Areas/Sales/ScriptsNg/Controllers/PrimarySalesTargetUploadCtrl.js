app.controller('PrimarySalesTargetUploadCtrl', ['$scope', 'hostDire', 'crudService', 'conversion', '$http', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, hostDire, crudService, conversion, $http, $filter, $localStorage, uiGridConstants) {

        var baseUrl = '/Sales/api/PrimarySalesTargetUpload/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptions = [];
        $scope.gridOptionsDTU = [];
        $scope.IsLoadingIcon = false;
        var page = 1;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;
        $scope.LoadingMessage = 'Loading...';
        $scope.PageTitle = 'Primary Sales Distributor Target Upload';
        $scope.DocumentMasterList = 'Distributor Target Upload Information';
        $scope.ListTitle = 'Document List';
        $scope.UploadedFileData = 'Uploaded File Data'
        $scope.IsHidden = true;
        $scope.IsShow = false;
        //$scope.IsBrows = true;
        $scope.FileId = null;
        $scope.FileName = '';
        $scope.FileSize = '';
        $scope.FileType = '';
        $scope.DocumentRows = [];
        $scope.DocumentLists = [];
        files = [];
        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmPrimarySalesTargetUpload'; DelFunc = 'DeleteBuildingMasterRecord'; DelMsg = 'DocName'; EditFunc = 'getDocumentListByID';
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

        $scope.pagination = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.pagination.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                $scope.loadDistTargetUploadList(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadDistTargetUploadList(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadDistTargetUploadList(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadDistTargetUploadList(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadDistTargetUploadList(1);
                }
            }
        };
        $scope.loadDistTargetUploadList = function (isPaging) {

            $scope.gridOptionsDTU.enableFiltering = true;

            $scope.loaderMore = true;
            $scope.result = "color-red";

            $scope.cmnParam();
            objcmnParam.pageNumber = ($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize;
            objcmnParam.pageSize = $scope.pagination.pageSize;
            objcmnParam.IsPaging = isPaging;

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsDTU = {
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "DOCUMENTID", displayName: "DOCUMENTID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DOCUMENTNAME", displayName: "FILE NAME", width: "25%", title: "FILE NAME", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "START_DATE", displayName: "START DATE ", width: "15%", title: "START DATE ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "END_DATE", displayName: "END DATE ", width: "15%", title: "END DATE ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CREATED_ON", displayName: "UPLOAD DATE ", width: "15%", title: "UPLOAD DATE ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "AREA_NATIONAL", displayName: "AREA NATIONAL ", width: "25%", title: "AREA NATIONAL ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "VIRTUALPATH", displayName: "VIRTUAL PATH", width: "5%", title: "VIRTUAL PATH", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Option',
                        displayName: "Option",
                        width: '15%',
                        pinnedRight: true,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.visible,
                        cellTemplate: '<span class="label label-info label-mini">' +
                            '<a href="javascript:void(0);" ng-href="#FileModal" data-toggle="modal" class="bs-tooltip" title="Show File List">' +
                            '<i class="glyphicon glyphicon-file" aria-hidden="true" ng-click="grid.appScope.getFileInfo(row.entity)">&nbsp;File</i>' +
                            '</a>' +
                            '</span>' +
                            '<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                            '<a href="" title="Delete" ng-click="grid.appScope.DeleteDistributorTargetRecord(row.entity)">' +
                            '<i class="icon-check" aria-hidden="true"></i> Delete' +
                            '</a>' +
                            '</span>'
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'DistributorTarget.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Distributor Target", style: 'headerStyle' },
                exporterPdfFooter: function (currentPage, pageCount) {
                    return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
                },
                exporterPdfCustomFormatter: function (docDefinition) {
                    docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
                    docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
                    return docDefinition;
                },
                exporterPdfOrientation: 'portrait',
                exporterPdfPageSize: 'LETTER',
                exporterPdfMaxGridWidth: 500,
                exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            };
            var apiRoute = baseUrl + 'GetDistTargetDocUploadMaster/';
            var listDistTargetMaster = crudService.getAPIRequestWithCmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listDistTargetMaster.then(function (response) {

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsDTU.data = response.data.objMasterRecord;
                $scope.loaderMoreDTML = false;
                $scope.loaderMore = false;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadDistTargetUploadList(0);
            $scope.IsHiddenDetail = true;
        }
        $scope.RefreshMasterList();


        $scope.Save = function () {
            $scope.IsLoadingIcon = true;

            debugger

            $scope.cmnParam();
            files = document.getElementById('file').files;
            var fileName = files[0].name;

            if (!fileName.startsWith("Consumer") && !fileName.startsWith("Sun") && !fileName.startsWith("JMW")) {
                Command: toastr["warning"]("File Name is not appropriate.");
                $scope.IsLoadingIcon = false;
                return false;
            }


            debugger
            var itemMaster = {
                START_DATE: conversion.getStringToDate($scope.StartDate),
                END_DATE: conversion.getStringToDate($scope.EndDate),
                FILE_NAME: fileName
            };

            objcmnParam = {
                loggeduser: $scope.UserCommonEntity.loggedUserID
            };

            var exist = 0;



            var HeaderTokenPutPost = $scope.HeaderToken.post;
            ModelsArray = [itemMaster, objcmnParam];
            var apiRoute = baseUrl + 'KeepTargetDateRange/';
            var DistTargetSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            DistTargetSaveUpdate.then(function (response) {
                debugger
                if (response != "") {
                    Command: toastr["warning"](response.toString());

                    exist = 1;
                }
            },
                function (error) {
                });

            debugger
            if (exist == 1) {
                $scope.IsLoadingIcon = false;
                return false;
            }

            if (files.length > 0) {
                $scope.uploadDocumentFile(files);
            }
            else {
                Command: toastr["warning"]("No file chosen");
                $scope.IsLoadingIcon = false;
            }
        }
        

        $scope.uploadDocumentFile = function (files) {
            var formData = new FormData();
            angular.forEach(files, function (file, index) {
                formData.append("uploadedFile", files[index]);
            });

            var upURL = hostDire + baseUrl;
            $http.post(upURL + "UploadDocuments/", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (response) {

                    if (response.IsBDUpdate == true) {

                        $scope.DocumentRows = response.ListDocuments;

                        var HeaderTokenPutPost = $scope.HeaderToken.post;
                        ModelsArray = [$scope.DocumentRows];
                        var apiRoute = baseUrl + 'InsertDocumentList/';
                        var DistTargetSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
                        DistTargetSaveUpdate.then(function (response) {
                            var result = 0;
                            if (response != "") {
                            }
                            else if (response == "") {
                            }
                        },
                            function (error) {
                            });

                        ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                        $scope.IsLoadingIcon = false;
                        $scope.IsHiddenDetail = false;
                        $scope.ListTargetUploadedData = response.objvmPIMasterWithOutPaging;
                        $scope.clear();
                    }
                    else {
                        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                        $scope.IsLoadingIcon = false;
                    }
                })
                .error(function () {
                });
        };

        $scope.getFileInfo = function (dataModel) {
            $scope.DOCUMENTNAME = dataModel.DOCUMENTNAME;
            $scope.VIRTUALPATH = dataModel.VIRTUALPATH;
        }

        $scope.getFiles = function (url, fileName) {
            debugger;
            conversion.downloadDocument(url, fileName);
        }

        //Test Upload
        $scope.UploadTest = function () {
            files = document.getElementById('file').files;

            if (files.length == 0) {
                return;
            }

            var formData = new FormData();
            angular.forEach(files, function (file, index) {
                formData.append("uploadedFile", files[index]);
            });

            var upURL = hostDire + baseUrl;
            $http.post(upURL + "UploadDocument_Test/", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (response) {
                    debugger;
                })
                .error(function () {
                });
        };

        $scope.clear = function () {
            $scope.frmPrimarySalesTargetUpload.$setPristine();
            $scope.frmPrimarySalesTargetUpload.$setUntouched();
            $scope.IsLoadingIcon = false;

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
            $scope.ListSelectedDocuments = [];
            $scope.ListDDLDocuments = [];
            $scope.DocumentLists = [];
            $scope.gridOptions.data = [];
            $scope.IsShow = false;
            $scope.IsHidden = true;

            $scope.StartDate = '';
            $scope.EndDate = '';

            files = [];
            $('#file').val('');
        }

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
            }
            else {
                $scope.RefreshMasterList();
                $scope.IsShow = false;
            }
        };

        $scope.DeleteDistributorTargetRecord = function (dataModel) {
            var IsConf = confirm('You are about to delete distributor target from ' + dataModel.START_DATE + ' to ' + dataModel.END_DATE + '. This operation can not be undone. Are you sure?');
            if (IsConf) {

                $scope.IsLoadingIcon = true;

                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    startDate: dataModel.START_DATE,
                    endDate: dataModel.END_DATE
                };

                var apiRoute = baseUrl + 'DeleteDistributorTargetRecord/';
                var DocumentId = dataModel.DOCUMENTID;
                var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(DocumentId) + "]";

                var documentDelete = crudService.GetLists(apiRoute, cmnParam, $scope.HeaderToken.delete);
                documentDelete.then(function (response) {
                    debugger
                    if (response.data.result == 1) {
                        $scope.loadDistTargetUploadList(0);
                        $scope.clear();

                        $scope.btnShowText = "Create";
                        $scope.IsShow = false;
                        $scope.IsHidden = false;

                        //change
                        $scope.IsHiddenDetail = true;
                        //change

                        $scope.IsCreateIcon = true;
                        $scope.IsListIcon = false;

                        Command: toastr["success"]("Your selected distributor target file and record has been deleted successfully!");
                        $scope.IsLoadingIcon = false;
                    }
                    else {
                        $scope.IsLoadingIcon = false;
                        Command: toastr["warning"]("Selected record has not been deleted, Please Check and Try Again!");
                    }
                }, function (error) {
                    $scope.IsLoadingIcon = false;
                    Command: toastr["warning"]("Selected record has not been deleted, Please Check and Try Again!");
                    console.log("Error: " + error);
                });
            }
        };

    }]);
