app.controller('SalesRequisitionUploadCtrl', ['$scope', 'hostDire', 'crudService', 'conversion', '$http', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, hostDire, crudService, conversion, $http, $filter, $localStorage, uiGridConstants) {

        var baseUrl = "/Sales/api/SalesRequisitionUpload/";


        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptions = [];
        $scope.gridOptionsPreviewData = [];
        $scope.gridOptionsARSU = [];
        $scope.IsLoadingIcon = false;
        var page = 1;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;
        $scope.LoadingMessage = 'Loading...';
        $scope.PageTitle = 'Sales Requisition Upload';
        $scope.DocumentMasterList = 'Sales Requisition Upload Information';
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
        frmName = ''; DelFunc = 'Delete'; DelMsg = 'DocName'; EditFunc = 'getDocumentListByID';
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

        $scope.loaderMoreDTML = false;

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
                $scope.loadAutoRiceSalesFileUploadList(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadAutoRiceSalesFileUploadList(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadAutoRiceSalesFileUploadList(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadAutoRiceSalesFileUploadList(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadAutoRiceSalesFileUploadList(1);
                }
            }
        };
        $scope.loadAutoRiceSalesFileUploadList = function (isPaging) {

            $scope.gridOptionsARSU.enableFiltering = true;

            $scope.loaderMoreDTML = true;
            $scope.result = "color-red";

            $scope.cmnParam();
            objcmnParam.pageNumber = ($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize;
            objcmnParam.pageSize = $scope.pagination.pageSize;
            objcmnParam.IsPaging = isPaging;
            objcmnParam.TransactionTypeID = 4;

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsARSU = {
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "DOCUMENTID", displayName: "DOCUMENTID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DOCUMENTNAME", displayName: "File Name", width: "70%", title: "File Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CREATED_ON", displayName: "Upload On", width: "20%", title: "Upload On ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "START_DATE", displayName: "START DATE ", width: "15%", title: "START DATE ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "END_DATE", displayName: "END DATE ", width: "15%", title: "END DATE ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    //{ name: "AREA_NATIONAL", displayName: "AREA NATIONAL ", width: "25%", title: "AREA NATIONAL ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },

                    { name: "VIRTUALPATH", displayName: "VIRTUAL PATH", width: "5%", title: "VIRTUAL PATH", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Option',
                        displayName: "Option",
                        width: '10%',
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
                            '</span>'
                        //+
                        //'<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                        //'<a href="" title="Delete" ng-click="grid.appScope.DeleteDistributorTargetRecord(row.entity)">' +
                        //'<i class="icon-check" aria-hidden="true"></i> Delete' +
                        //'</a>' +
                        //'</span>'
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
            var apiRoute = baseUrl + 'GetCmnDocument/';
            var listDistTargetMaster = crudService.getAPIRequestWithCmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listDistTargetMaster.then(function (response) {

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsARSU.data = response.data.objMasterRecord;
                $scope.loaderMoreDTML = false;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadAutoRiceSalesFileUploadList(0);
            $scope.IsHiddenDetail = true;
        }
        $scope.RefreshMasterList();

        $scope.ItemsToBeOpen = '';
        $scope.ItemMsg = 'These materials are not opened in Legacy System : ';
        $scope.Save = function () {
            $scope.isPass = !$scope.isPass ? $scope.convertExcel(false) : $scope.isPass
            setTimeout(() => {
                if ($scope.isPass) {
                    $scope.IsLoadingIcon = true;

                    $scope.cmnParam();
                    files = document.getElementById('file').files;
                    if (files.length > 0) {
                        $scope.uploadDocumentFile(files);
                    }
                    else {
                        Command: toastr["warning"]("No file chosen");
                        $scope.IsLoadingIcon = false;
                    }
                }
            }, 1000);
        }

        //$scope.uploadDocumentFile = function (files) {
        //    var param = { loggeduser: $scope.UserCommonEntity.loggedUserID };
        //    var formData = new FormData();
        //    angular.forEach(files, function (file, index) {
        //        formData.append("uploadedFile", files[index]);
        //    });

        //    var HeaderTokenPutPost = $scope.HeaderToken.post;
        //    var ModelsArray = [param];

        //    var apiRoute = baseUrl + 'UploadDocuments/';

        //    var DistTargetSaveUpdate = crudService.postFormWithMultipleModels(apiRoute, ModelsArray, formData, HeaderTokenPutPost);
        //    DistTargetSaveUpdate.then(function (response) {
        //        debugger;
        //        if (response.BDUpdate.returnValue == "1") {

        //            $scope.DocumentRows = response.ListDocuments;

        //            var HeaderTokenPutPost = $scope.HeaderToken.post;
        //            var param = { loggeduser: $scope.UserCommonEntity.loggedUserID };
        //            ModelsArray = [$scope.DocumentRows, param];
        //            var apiRoute = baseUrl + 'InsertDocumentList/';
        //            var DistTargetSaveUpdate = crudService.postMultipleModels(apiRoute, ModelsArray, HeaderTokenPutPost);
        //            DistTargetSaveUpdate.then(function (response) {
        //                var result = 0;
        //                if (response != "") {
        //                }
        //                else if (response == "") {
        //                }
        //            },
        //                function (error) {
        //                });

        //            ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
        //            $scope.IsLoadingIcon = false;
        //            $scope.IsHiddenDetail = false;
        //            $scope.ListTargetUploadedData = response.objvmPIMasterWithOutPaging;
        //            $scope.clear();
        //        }
        //        else {
        //            ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
        //            $scope.IsLoadingIcon = false;
        //            if (response.BDUpdate.returnItem != '') {
        //                $scope.clear();
        //                $scope.ItemsToBeOpen = response.BDUpdate.returnItem;
        //            }
        //        }
        //    })
        //        .error(function () {
        //        });
        //};

        $scope.uploadDocumentFile = function (files) {
            var param = { loggeduser: $scope.UserCommonEntity.loggedUserID };
            var formData = new FormData();
            angular.forEach(files, function (file, index) {
                formData.append("uploadedFile", files[index]);
            });

            formData.append("data", JSON.stringify(param));

            var upURL = hostDire + baseUrl;
            $http.post(upURL + "UploadDocuments/", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (response) {
                    debugger;
                    if (response.BDUpdate.returnValue == "1") {

                        $scope.DocumentRows = response.ListDocuments;

                        var HeaderTokenPutPost = $scope.HeaderToken.post;
                        var param = { loggeduser: $scope.UserCommonEntity.loggedUserID };
                        ModelsArray = [$scope.DocumentRows, param];
                        var apiRoute = baseUrl + 'InsertDocumentList/';
                        var DistTargetSaveUpdate = crudService.postMultipleModels(apiRoute, ModelsArray, HeaderTokenPutPost);
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
                        if (response.BDUpdate.returnItem != '') {
                            $scope.clear();
                            $scope.ItemsToBeOpen = response.BDUpdate.returnItem;
                        }
                    }
                })
                .error(function () {
                });
        };

        $scope.getFileInfo = function (dataModel) {
            $scope.DOCUMENTNAME = dataModel.DOCUMENTNAME;
            $scope.VIRTUALPATH = dataModel.VIRTUALPATH;
        }

        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                //$scope.clear();
            }
            else {
                $scope.RefreshMasterList();
                $scope.IsShow = false;
            }
        };

        $scope.SelectedFile = null;
        $scope.isPass = false;
        onFileChange = function () {
            $scope.loaderMore = false;
            var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx)$/;
            var tempFile = document.getElementById('file').files[0];
            if (tempFile != undefined && regex.test(tempFile.name.toLowerCase())) {
                $scope.$apply(function () {
                    $scope.SelectedFile = tempFile;
                    $scope.cmnbtnShowHideEnDisable('false');
                });
            }
            else {
                $scope.$apply(function () {
                    $('#file').val('');
                    $scope.SelectedFile = null;
                    $scope.cmnbtnShowHideEnDisable('true');
                });
            }
        }

        $scope.ExcelRows = null;
        $scope.convertExcel = function (isView) {
            debugger;
            if ($scope.SelectedFile != null) {
                $scope.loaderMore = isView;
                //Common task to convert excel to array
                conversion.ExcelToArray($scope.SelectedFile, $scope.UserCommonEntity);
                setTimeout(() => {
                    var ExcelRows = $scope.UserCommonEntity.ExcelRows;
                    $scope.getExcelRows(ExcelRows, isView);
                    $scope.UserCommonEntity.ExcelRows = undefined;
                }, 1000);
                //Common task to convert excel to array
            }
        }

        $scope.getExcelRows = function (ExcelRows, isView) {
            var modifiedExcelRows = [];
            if (ExcelRows.length > 0) {
                var undeFinedCol = ExcelRows.filter(x => x.Quantity_CTN != undefined);
                if (undeFinedCol.length == 0) {
                    angular.forEach(ExcelRows, function (item) {
                        modifiedExcelRows.push({
                            UniqueReferenceNo: item.UniqueReferenceNo, CompanyCode: item.CompanyCode, CustomerCode: item.CustomerCode,
                            SalesType: item.SalesType, ProductCode: item.ProductCode, QuantityCTN: item.Quantity_CTN
                        });
                    });

                    if (isView) {
                        //Display the data from Excel file in Table.
                        $scope.$apply(function () {
                            $scope.loadExcelUploadedDataList(modifiedExcelRows);
                        });
                    }

                    $scope.isPass = true;
                }
                else {
                    ShowCustomToastr('warning', 'Column(s) missmatch, please check column name in excel file and try again !!!');
                    $scope.$apply(function () {
                        $('#file').val('');
                        $scope.loaderMore = false;
                        $scope.SelectedFile = null;
                        $scope.gridOptionsPreviewData.data = [];
                        $scope.isHideViewData = true;
                        $scope.cmnbtnShowHideEnDisable('true');
                    });

                    $scope.isPass = false;
                }
            }
        }

        $scope.loaderMore = false;
        $scope.isHideViewData = true;
        $scope.loadExcelUploadedDataList = function (dataList) {
            $scope.isHideViewData = true;
            $scope.gridOptionsPreviewData.enableFiltering = true;

            $scope.result = "color-red";

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsPreviewData = {
                columnDefs: [
                    { name: "UniqueReferenceNo", displayName: "Reference No", width: "7%", visible: true, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CompanyCode", displayName: "Company Code", width: "8%", visible: true, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CustomerCode", displayName: "Customer Code", width: "8%", visible: true, headerCellClass: $scope.highlightFilteredHeader },                    
                    { name: "SalesType", displayName: "Sales Type", width: "10%", title: "Sales Type", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ProductCode", displayName: "Product Code", width: "8%", title: "Product Code", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "QuantityCTN", displayName: "Quantity CTN", width: "20%", title: "Quantity CTN", headerCellClass: $scope.highlightFilteredHeader },
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

            $scope.isHideViewData = false;
            $scope.loaderMore = false;
            $scope.gridOptionsPreviewData.data = dataList;
            $scope.SelectedFile = null;
            //$('#file').val('');
            //$scope.cmnbtnShowHideEnDisable('false');
        };

        $scope.clear = function () {
            $scope.gridOptionsPreviewData.data = [];
            $scope.SelectedFile = null;
            $('#file').val('');
            $scope.isHideViewData = true;
            $scope.cmnbtnShowHideEnDisable('true');
            $scope.ItemsToBeOpen = '';
        }
    }]);
