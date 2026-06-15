app.controller('ItemWisePurchaseCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, $http, $window, crudService, conversion, $filter, $localStorage, uiGridConstants) {

        //**************************************************Start Vairiable Initialize**************************************************      
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        var repBaseUrl = '/Areas/INVENTORY/Reports/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.IsbtSaveReviseShow = true;

        $scope.StartDate = conversion.NowDateCustom();
        $scope.EndDate = conversion.NowDateCustom();
                        
        $scope.btnPISaveText = "Save";
        $scope.btnPIShowText = "Show List";
        $scope.CustomerWiseDeliveryPageTitle = 'Item Wise Purchase History';

        $scope.HoldDataModel = '';

        $scope.ListDistributorTargetDetails = [];
        $scope.listDistTargetMaster = [];
        $scope.showDtgrid = 0;
        $scope.listDistributor = [];
        $scope.ngModelDistributor = '';
        //$scope.ngModelReportType = '';

        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;

        $scope.IsShowReportButtonDisabled = false;        

        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmDistributorTarget'; DelFunc = 'DeleteDistributorTargetMasterDetail'; DelMsg = 'DistributorTargetNo'; EditFunc = 'loadPIMasterDetailsByActivePI';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager);
        $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        
     



        ///// Get all products 
        function loadProductRecords(isPaging) {
            var apiRoute = sysCommonUrl + 'GetInventoryItem/';
            var ListProducts = crudService.getItemWithcmnParam(apiRoute, objcmnParam);
            ListProducts.then(function (response) {
                $scope.ListProducts = response.data.objInventoryItem;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadProductRecords(0);  

        ///// Get all Suppliers 
        function loadSuppliers(isPaging) {

            var apiRoute = sysCommonUrl + 'GetInventorySupplier/';
            var ListSuppliers = crudService.getItemWithcmnParam(apiRoute, objcmnParam);
            ListSuppliers.then(function (response) {
                $scope.ListSuppliers = response.data.objInventorySupplier;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadSuppliers(0);  

        ///// Get all Company/Unit 
        function loadUnit(isPaging) {

            var apiRoute = sysCommonUrl + 'GetUnitCompany/';
            var ListUnists = crudService.getItemWithcmnParam(apiRoute, objcmnParam);
            ListUnists.then(function (response) {
                $scope.ListUnists = response.data.objUnitCompany;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadUnit(0);  

        ///// Get all Item Group
        function loadItemGroup(isPaging) {

            var apiRoute = sysCommonUrl + 'GetItemGroup/';
            var ListItemGrp = crudService.getItemWithcmnParam(apiRoute, objcmnParam);
            ListItemGrp.then(function (response) {
                $scope.ListItemGrp = response.data.objItemGroup;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadItemGroup(0);  


        $scope.ShowItemPurchaseHistory = function () {
            
            if ($scope.ReportType == '' || $scope.ReportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var commonQueryString = $scope.StartDate + ',' + $scope.EndDate + ',' + $scope.ReportType + ',' + $scope.ITEM_ID;
            var reportPath = repBaseUrl + 'ItemPurchaseHistory/ItemPurchaseHistory.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowSupplierLedgerReport = function () {

            if ($scope.ReportType == '' || $scope.ReportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var commonQueryString = $scope.StartDate + ',' + $scope.EndDate + ',' + $scope.ReportType + ',' + $scope.ITEM_ID + ',' + $scope.ngModelSupplier + ',' + $scope.ngModelUnit;

            var reportPath = repBaseUrl + 'SupplierLedgerReport/SupplierLedgerReport.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowPurchaseOrderReport = function () {

            if ($scope.ReportType == '' || $scope.ReportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var commonQueryString = $scope.StartDate + ',' + $scope.EndDate + ',' + $scope.ReportType + ',' + $scope.ITEM_ID + ',' + $scope.ngModelSupplier + ',' + $scope.ngModelUnit + ',' + $scope.ngItemGroup; 

            var reportPath = repBaseUrl + 'PurchaseOrderReport/PurchaseOrderReport.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowItemStoreLedgerReport = function () {

            if ($scope.ReportType == '' || $scope.ReportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var commonQueryString = $scope.StartDate + ',' + $scope.EndDate + ',' + $scope.ReportType + ',' + $scope.ITEM_ID + ',' + $scope.ngModelSupplier + ',' + $scope.ngModelUnit + ',' + $scope.ngItemGroup;

            var reportPath = repBaseUrl + 'ItemStoreLedgerReport/ItemStoreLedgerReport.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };


        $scope.clear = function () {
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Customer--' });

            //$scope.ngModelProduct = '';
            //$("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            $scope.ngModelTransportType = '';
            $("#ddlTransportType").select2("data", { id: '', text: '--Select Transport Type--' });
            
            $scope.ngModelLocation = '';
            $("#ddlLocation").select2("data", { id: '', text: '--Select Location--' });

            $scope.ngModelSardarName = '';
            $("#ddlSardarName").select2("data", { id: '', text: '--Select Sardar Name--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();

            $scope.ngmRequisitionNumber = '';

            $scope.ITEM_ID = '';
            $scope.ITEM_NAME = '';
            $scope.DisplayItemName = '';
            $scope.modalSearchItemName = "";
            $scope.IsCallFromSearch = false;
        };
        
        //**********---- End Clear records ----***************//

        //Item Modal
        $scope.ITEM_ID = '';
        $scope.ITEM_NAME = '';
        $scope.DisplayItemName = '';
        $scope.gridOptionslistItemMaster = [];
        $scope.modalSearchItemName = "";
        $scope.IsCallFromSearch = false;

        $scope.getListItemMaster = function (model) {
            var ItemID = model.ITEM_ID;
            var ItemName = model.ITEM_NAME;
            $scope.ITEM_ID = ItemID;
            $scope.ITEM_NAME = ItemName;
            $scope.DisplayItemName = ItemID + ' - ' + ItemName;

            $scope.modalSearchItemName = "";
            $scope.IsCallFromSearch = false;
            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('hide'); });
        }
        $scope.modalClose = function () {
            $scope.modalSearchItemName = "";
            $scope.IsCallFromSearch = false;
        }
        $scope.SearchItem = function (ev) {
            debugger;
            //if (ev.keyCode == 13) {
            $scope.loadSampleNoModalRecords(0);
            //}
            //$scope.IsCallFromSearch = searchItemName == "" ? false : true;
            //$scope.modalSearchItemName = searchItemName.toString();
            //$scope.paginationItemMaster.pageNumber = 2;
            //$scope.paginationItemMaster.firstPage();
        }
        //**********----Pagination Item Master List popup----***************
        $scope.paginationItemMaster = {
            paginationPageSizes: [10, 20, 30, 50, 70, 100, 500, 1000, "All"],
            ddlpageSize: 30, pageNumber: 1, pageSize: 30, totalItems: 0,

            getTotalPagesItemMaster: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.paginationItemMaster.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                $scope.loadSampleNoModalRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadSampleNoModalRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPagesItemMaster()) {
                    this.pageNumber++;
                    $scope.loadSampleNoModalRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadSampleNoModalRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPagesItemMaster();
                    $scope.loadSampleNoModalRecords(1);
                }
            }
        };
        //**********----Get All Item Record by  select Sample No----***************//
        $scope.loadSampleNoModalRecords = function (isPaging) {
            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });
            $('#ItemModal').modal({ show: true, backdrop: "static" });
            $scope.gridOptionslistItemMaster.enableFiltering = true;
            $scope.gridOptionslistItemMasterenableGridMenu = true;

            // For Loading
            if (isPaging == 0)
                $scope.paginationItemMaster.pageNumber = 1;
            // For Loading
            $scope.loaderMoreItemMaster = true;
            $scope.lblMessageItemMaster = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.paginationItemMaster.pageNumber - 1) * $scope.paginationItemMaster.pageSize),
                pageSize: $scope.paginationItemMaster.pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                //loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                //menuId: $scope.UserCommonEntity.currentMenuID,
                //tTypeId: $scope.UserCommonEntity.currentTransactionTypeID,
                //selectedCompany: $scope.UserCommonEntity.loggedCompnyID,
                searchItemName: $scope.modalSearchItemName
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionslistItemMaster = {
                rowTemplate: '<div ng-dblclick="grid.appScope.getListItemMaster(row.entity)" ng-repeat="col in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ui-grid-cell></div>',
                columnDefs: [
                    { name: "ITEM_ID", displayName: "Item Code", title: "Item Code", visible: true, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ITEM_NAME", displayName: "Item Name", title: "Item Name", visible: true, headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '6%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.EnableUpdate,
                        cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                            '<a href="" title="Select" ng-click="grid.appScope.getListItemMaster(row.entity)">' +
                            '<i class="icon-check" aria-hidden="true"></i> Add' +
                            '</a>' +
                            '</span>'
                    }
                ],

                useExternalPagination: true,
                useExternalSorting: true,
                enableFiltering: true,
                enableRowSelection: true,
                enableSelectAll: true,
                showFooter: true,
                enableGridMenu: true,
                exporterCsvFilename: 'ItemSample.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "ItemSample", style: 'headerStyle' },
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


            var apiRoute = sysCommonUrl + 'GetInventoryItem/';
            var listItemMaster = crudService.getItemWithcmnParam(apiRoute, objcmnParam);
            listItemMaster.then(function (response) {
                debugger;
                if (response.data.objInventoryItem.length > 0) {
                    $scope.paginationItemMaster.totalItems = response.data.objInventoryItem[0].RECORDSTOTAL;
                    $scope.gridOptionslistItemMaster.data = response.data.objInventoryItem;
                    $scope.loaderMoreItemMaster = false;
                }
                else {
                    $scope.paginationItemMaster.totalItems = 0;
                    $scope.gridOptionslistItemMaster.data = [];
                    $scope.loaderMoreItemMaster = false;
                }
            },
                function (error) {
                    console.log("Error: " + error);
                });

        };
        //Item Modal
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}