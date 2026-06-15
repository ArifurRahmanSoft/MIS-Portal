app.controller('ProductDetailsPerLCBasedCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, $http, $window, crudService, conversion, $filter, $localStorage, uiGridConstants) {

        //**************************************************Start Vairiable Initialize**************************************************      
        var baseUrl = '/MISREPORT/api/ProductDetailsPerLCBased/';
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        var costingUrl = '/COSTING/api/ProductConsting/';
        var repBaseUrl = '/Areas/MISREPORT/Reports/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.IsbtSaveReviseShow = true;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;


        $scope.ngmReportFromDate = conversion.NowDateCustom();
        $scope.ngmReportToDate = conversion.NowDateCustom();
        $scope.ngmStockDate = conversion.NowDateCustom();
        $scope.ngmReceivedDate = conversion.NowDateCustom();
        //$scope.EndDate = conversion.NowDateCustom();

        var objcmnParam = {};
        objcmnParam = {};

        $scope.btnPISaveText = "Save";
        $scope.btnPIShowText = "Show List";
        $scope.CustomerWiseDeliveryPageTitle = 'Product Details Per LC Based';

        $scope.HoldDataModel = '';

        $scope.ListDistributorTargetDetails = [];
        $scope.listDistTargetMaster = [];
        $scope.showDtgrid = 0;
        $scope.listDistributor = [];
        $scope.ngModelDistributor = '';

        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;

        $scope.IsShowReportButtonDisabled = false;

        // start of 31-03-2020 //
        //Ui Grid
        $scope.gridProductDetailsPerLCBased = [];

        //Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;
        $scope.IsShowD = false;

        $scope.ShowHideOpeningStock = function () {
            $scope.IsShow = $scope.IsShow ? false : true;
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnShowText = "Show List";
                $scope.IsShow = true;
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
            }
            else {
                $scope.btnShowText = "Create";
                $scope.IsShow = false;
                $scope.IsHidden = false;
                $scope.IsShowD = false;
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                loadOpeningStockRecords(0);
            }
        }

        // end of 31-03-2020 //

        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmDistributorTarget'; DelFunc = 'DeleteDistributorTargetMasterDetail'; DelMsg = 'DistributorTargetNo'; EditFunc = 'loadPIMasterDetailsByActivePI';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        


        $scope.pagination = {
            paginationPageSizes: [10, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 10, pageNumber: 1, pageSize: 10, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.pagination.totalItems;
                else
                    this.pageSize = this.ddlpageSize
                this.pageNumber = 1

                loadOpeningStockRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    loadOpeningStockRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    loadOpeningStockRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    loadOpeningStockRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    loadOpeningStockRecords(1);
                }
            }
        };
        function loadOpeningStockRecords(isPaging) {
            // For Paging
            if (isPaging == 0)
                $scope.pagination.pageNumber = 1;

            // For Loading
            $scope.loaderMoreIssueMaster = true;
            $scope.lblMessageForQCMaster = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridProductDetailsPerLCBased = {
                columnDefs: [
                    { name: "OPENING_STOCK_OID", displayName: "OPENING_STOCK_OID", title: "OPENING_STOCK_OID", visible: false, width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "OPENINGQUANTITY", displayName: "OPENINGQUANTITY", title: "OPENINGQUANTITY", visible: false, width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Action',
                        displayName: "Action",
                        pinnedRight: true,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        width: '12%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.visible,

                        cellTemplate: '<span class="label label-success label-mini" >' +
                            '<a href="" title="Edit" ng-click="grid.appScope.GET_MV_RECEIVE_INFO_BY_ID(row.entity)">' +
                            '<i class="icon-check" aria-hidden="true"></i> Edit' +
                            '</a>' +
                            '</span>'
                    }
                ],
                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'OpenStockList.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Tenant Tagging", style: 'headerStyle' },
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

            var apiRoute = baseUrl + 'getOpenStockList/';
            var listTenantTagMaster = crudService.getList(apiRoute, objcmnParam);
            listTenantTagMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridProductDetailsPerLCBased.data = response.data.listOpenStock;
                $scope.loaderMore = false;
            },
                function (error) {
                    //console.log("Error: " + error);
                });
        }
        loadOpeningStockRecords(0);







        // Get All Company list
        //$scope.GetAllCompany = function () {
        //    mCUID = $scope.ngModelCompany;
        //    $http.get(rootUrl + "/COSTING/api/ProductConsting/getAllCompanyList").then(function (d) {
        //        $scope.listCompany = d.data;
        //    }, function (error) {
        //        alert("Failed");
        //    })
        //};
        //$scope.GetAllCompany();

        $scope.GetAllCompany = function () {
            var apiRoute = costingUrl + 'getAllCompanyList/';
            var listModel = crudService.getAllList(apiRoute);
            listModel.then(function (response) {
                $scope.listCompany = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.GetAllCompany();

        $scope.loadRowMetarialsProductList = function () {
            debugger
            $scope.ListProducts = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            var apiRoute = sysCommonUrl + 'GetRowMetarialsProductList/';
            var mode = '70';
            var trackNo = 0;

            var ListProducts = crudService.getModelByID(apiRoute, $scope.HeaderToken.get);
            ListProducts.then(function (response) {
                $scope.ListProducts = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadRowMetarialsProductList();
        //// Get all Product
        //$scope.loadProductAll = function () {
        //    debugger
        //    $scope.ListProducts = [];
        //    $scope.ngModelProduct = '';
        //    $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

        //    var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
        //    var mode = '7';
        //    var trackNo = 0;

        //    var ListProducts = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
        //    ListProducts.then(function (response) {
        //        $scope.ListProducts = response.data;
        //    },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //}
        //$scope.loadProductAll();

        $scope.clearConsumerPartyList = function () {
            $scope.ngModelProductTypeSSTYP = '';
            $("#ddlProductTypeSSTYP").select2("data", { id: '', text: '--Select Product Type--' });
        }

        $scope.ShowProductDetailsPerLCBased = function () {
            debugger
            var productId = $scope.ngModelreportProduct;
            if (productId.length == 0) {
                //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined) {
                Command: toastr["warning"]("Please select Product");
                return;
            }

            var startDate = $scope.ngmReportFromDate;
            var endDate = $scope.ngmReportToDate;
            if (startDate.length == 0 || endDate.length == 0) {
                //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }


            var productId = $scope.ngModelreportProduct == undefined ? '' : $scope.ngModelreportProduct;

            var commonQueryString = productId + ',' + startDate + ',' + endDate;

            var reportPath = repBaseUrl + 'ProductDetailsPerLCBased/ProductDetailsPerLCBased.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };
        // Clear info after sucessful transaction
        $scope.clearReportPart = function () {

            //$scope.ListProducts = [];
            $scope.ngModelreportProduct = '';
            $("#ddModelreportProduct").select2("data", { id: '', text: '--Select Product--' });


            $scope.ngmReportFromDate = conversion.NowDateCustom();
            $scope.ngmReportToDate = conversion.NowDateCustom();
        };
        //**********---- End Clear records ----***************//
        // Clear info after sucessful transaction
        $scope.clearOpeningStock = function () {

            //$scope.ListProducts = [];
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });


            $scope.ngmStockDate = conversion.NowDateCustom();
            $scope.ngmQuantity = '';
            //$scope.EndDate = conversion.NowDateCustom();
        };
        //**********---- End Clear records ----***************//

        // lc RECEIVE PART Clear info after sucessful transaction
        $scope.Receivedclear = function () {

            //$scope.ListProducts = [];
            $scope.ngModelReceivedCompany = '';
            $("#ddlReceivedCompany").select2("data", { id: '', text: '--Select Company--' });
            $scope.ngModelReceivedProduct = '';
            $("#ddlReceivedProduct").select2("data", { id: '', text: '--Select Product--' });


            $scope.ngmReceivedDate = conversion.NowDateCustom();
            $scope.ngmReceivedQuantity = '';
            //$scope.EndDate = conversion.NowDateCustom();
        };
        //**********----lc RECEIVE PART End Clear records ----***************//

        // save info after sucessful transaction
        $scope.saveOpeningStock = function () {


            debugger
            var companyId = $scope.ngModelCompany;
            if (companyId.length == 0) {
                //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined) {
                Command: toastr["warning"]("Please select Company");
                return;
            }

            var productId = $scope.ngModelProduct;
            if (productId.length == 0) {
                //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined) {
                Command: toastr["warning"]("Please select Product");
                return;
            }

            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            var productId = $scope.ngModelProduct == undefined ? '' : $scope.ngModelProduct;
            var stockDate = $scope.ngmStockDate;
            var quantity = $scope.ngmQuantity;
            if (stockDate.length == 0 || quantity.length == 0) {
                //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            debugger

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            var HedarTokenPostPut = $scope.LV_LIGHT_DRAFT_SURVEY_OID != '' ? $scope.HeaderToken.put : $scope.HeaderToken.post;
            debugger
            var openStockInfo = {
                COMPANYOID: $scope.ngModelCompany,
                PRODUCTOID: $scope.ngModelProduct,
                STOCKDATE: $scope.ngmStockDate == null || "" || undefined ? null : conversion.getStringToDate($scope.ngmStockDate),
                OPENINGQUANTITY: $scope.ngmQuantity,
                OPENINGAMOUNT: 0,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                IsDeleted: false
            };

            var apiRoute = baseUrl + 'SaveOpeningStock/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(openStockInfo) + "]";

            var DocumentCreateUpdate = crudService.GetLists(apiRoute, cmnParam, HedarTokenPostPut);
            DocumentCreateUpdate.then(function (response) {
                debugger
                if (response.data > 0) {
                    Command: toastr["success"]("Opening Stock Save successfully.");
                    $scope.clearOpeningStock();
                }
                else {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                }
            },
                function (error) {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    console.log("Error: " + error);
                });

        };
        //**********---- End save records ----***************//



        // lc RECEIVE save info after sucessful transaction
        $scope.Receivedsave = function () {


            debugger
            var companyId = $scope.ngModelReceivedCompany;
            if (companyId.length == 0) {
                //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined) {
                Command: toastr["warning"]("Please select Company");
                return;
            }

            var productId = $scope.ngModelReceivedProduct;
            if (productId.length == 0) {
                //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined) {
                Command: toastr["warning"]("Please select Product");
                return;
            }

            var companyId = $scope.ngModelReceivedCompany == undefined ? '' : $scope.ngModelReceivedCompany;
            var productId = $scope.ngModelReceivedProduct == undefined ? '' : $scope.ngModelReceivedProduct;
            var receiveDate = $scope.ngmReceivedDate;
            var receiveQuantity = $scope.ngmReceivedQuantity;
            if (receiveDate.length == 0 || receiveQuantity.length == 0) {
                //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined) {
                Command: toastr["warning"]("Please select receive date and quantity");
                return;
            }

            debugger

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            var HedarTokenPostPut = $scope.LV_LIGHT_DRAFT_SURVEY_OID != '' ? $scope.HeaderToken.put : $scope.HeaderToken.post;
            debugger
            var lcReceiveInfo = {
                COMPANYOID: $scope.ngModelReceivedCompany,
                PRODUCTOID: $scope.ngModelReceivedProduct,
                RECEIVEDDATE: $scope.ngmReceivedDate == null || "" || undefined ? null : conversion.getStringToDate($scope.ngmReceivedDate),
                RECEIVEDQUANTITY: $scope.ngmReceivedQuantity,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                IsDeleted: false
            };

            var apiRoute = baseUrl + 'SaveLCReceive/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(lcReceiveInfo) + "]";

            var DocumentCreateUpdate = crudService.GetLists(apiRoute, cmnParam, HedarTokenPostPut);
            DocumentCreateUpdate.then(function (response) {
                debugger
                if (response.data > 0) {
                    Command: toastr["success"]("LC Receive Save successfully.");
                    $scope.clear();
                }
                else {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                }
            },
                function (error) {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    console.log("Error: " + error);
                });

        };
        //**********---- End lc RECEIVE save records ----***************//
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}