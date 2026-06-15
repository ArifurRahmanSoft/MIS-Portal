app.controller('CardConsumerDataUploadCtrl', ['$scope', 'hostDire', 'crudService', 'conversion', '$http', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, hostDire, crudService, conversion, $http, $filter, $localStorage, uiGridConstants) {

        var baseUrl = '/Sales/api/CardConsumerDataUpload/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptions = [];
        //$scope.gridOptionsCCDU = [];
        $scope.IsLoadingIcon = false;
        var page = 1;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;
        $scope.LoadingMessage = 'Loading...';
        $scope.PageTitle = 'CARD Consumer Data Upload';
        $scope.DocumentMasterList = 'CARD Consumer Data Upload Information';
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
        $scope.IsHiddenReportData = true;
        $scope.IsHiddenMasterGrid = false;
        files = [];
        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmCardConsumerDataUpload'; DelFunc = 'DeleteBuildingMasterRecord'; DelMsg = 'DocName'; EditFunc = 'getDocumentListByID';
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

            $scope.gridOptions.enableFiltering = true;

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

            $scope.gridOptions = {
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "DOCUMENTID", displayName: "DOCUMENTID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DOCUMENTNAME", displayName: "File Name", width: "65%", title: "File Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "START_DATE", displayName: "Start Date", width: "10%", title: "Upload On ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "END_DATE", displayName: "End Date", width: "10%", title: "Upload On ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
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
                $scope.gridOptions.data = response.data.objMasterRecord;
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


        $scope.ViewFileData = function () {
            $scope.IsLoadingIcon = true;

            /////////////////////////////////////

            var HeaderTokenPutPost = $scope.HeaderToken.post;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'KeepTargetDateRange/';
            var DistTargetSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            DistTargetSaveUpdate.then(function (response) {
            },
                function (error) {
                });
            //////////////////////////////////////

            files = document.getElementById('file').files;
            if (files.length > 0) {
                $scope.uploadDocumentFileView(files);
            }
            else {
                Command: toastr["warning"]("No file chosen");
                $scope.IsLoadingIcon = false;
            }
        }
        $scope.uploadDocumentFileView = function (files) {
            var formData = new FormData();
            angular.forEach(files, function (file, index) {
                formData.append("uploadedFile", files[index]);
            });

            var upURL = hostDire + baseUrl;
            $http.post(upURL + "UploadDocumentsView/", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (response) {

                    if (response.IsBDUpdate == true) {
                        $scope.IsLoadingIcon = false;
                        $scope.IsHiddenDetail = false;
                        $scope.ListCardConsumerUploadedData = response.objvmCardConsumerDataUpload;
                       // $scope.clear(); // think about this later.
                    }
                    else {
                        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                        $scope.IsLoadingIcon = false;
                    }
                })
                .error(function () {
                });
        };    

        $scope.Save = function () {
            debugger
            $scope.cmnParam();
            objcmnParam.loggedCompany = 1;
            if ($scope.ListCardConsumerUploadedData.length > 0) {
                var HeaderTokenPutPost = $scope.HeaderToken.post;
                ModelsArray = [$scope.ListCardConsumerUploadedData, objcmnParam];
                var apiRoute = baseUrl + 'UploadDocumentsSave/';
                var UploadDocumentsSave = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
                UploadDocumentsSave.then(function (response) {
                    debugger
                    if (response == "" || response == "1") {
                        $scope.clear();
                        ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                    }
                    else {
                        Command: toastr["warning"](response);
                    }
                },
                    function (error) {
                        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    });
            }
            else if ($scope.ListCardConsumerUploadedData.length <= 0) {
                $("#save").prop("disabled", false);
                Command: toastr["warning"]("File Data Must Not Empty!!!!");
            }
        };          

        $scope.ShowCardConsumerReportData = function () {

            $scope.listCCRData = [];

            $scope.cmnParam();
            objcmnParam.challanNo = $scope.ngChallanNo;
            objcmnParam.distributorId = $scope.ngDistributorID;
            //objcmnParam.startDate = $scope.StartDate == null || "" || undefined ? null : conversion.getStringToDate($scope.StartDate);            
            //objcmnParam.endDate = $scope.StartDate == null || "" || undefined ? null : conversion.getStringToDate($scope.EndDate);

            objcmnParam.fromDate = $scope.StartDate;            
            objcmnParam.toDate = $scope.EndDate;

            var apiRoute = baseUrl + 'CardConsumerReportData/';
            var listCCRData = crudService.getAPIRequestWithCmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listCCRData.then(function (response) {
                $scope.listCCRData = response.data.objReportData;
                $scope.IsHiddenReportData = false;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.getFileInfo = function (dataModel) {
            $scope.DOCUMENTNAME = dataModel.DOCUMENTNAME;
            $scope.VIRTUALPATH = dataModel.VIRTUALPATH;
        }

        $scope.getFiles = function (url, fileName) {
            debugger;
            conversion.downloadDocument(url, fileName);
        }

        $scope.clear = function () {
            $scope.frmCardConsumerDataUpload.$setPristine();
            $scope.frmCardConsumerDataUpload.$setUntouched();
            $scope.IsLoadingIcon = false;
                    
            $scope.FilePath = '';
            $scope.VirtualPath = '';
            $scope.FileId = 0;
            $scope.FileName = '';
            $scope.FileSize = '';
            $scope.FileType = '';
            $scope.ListCardConsumerUploadedData = [];

            $scope.StartDate = '';
            $scope.EndDate = '';
            $scope.ngDistributorID = '';
            $scope.ngChallanNo = '';

            $scope.listCCRData = [];
            $scope.IsHiddenReportData = true;

            $scope.gridOptions.data = [];
            $scope.IsShow = false;
            $scope.IsHidden = true;
            
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
                $scope.IsHiddenReportForm = false;
                $scope.IsShow = false;
            }
        };            

        $scope.DeleteDistributorTargetRecord = function (dataModel) {
            var IsConf = confirm('You are about to delete data. This operation can not be undone. Are you sure?');
            if (IsConf) {

                $scope.IsLoadingIcon = true;

                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID
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
