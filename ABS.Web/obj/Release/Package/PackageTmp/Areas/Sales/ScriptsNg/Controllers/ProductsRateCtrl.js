app.controller('ProductsRateCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************

        var baseUrl = '/Sales/api/ProductsRate/';
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsDTML = [];
        $scope.gridOptionslistItemMaster = [];

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;

        $scope.ngActiveDate = conversion.NowDateCustom();
        $scope.ngCloseDate = conversion.NowDateCustom();

        $scope.bool = true;

        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";
        $scope.PageTitle = 'Product Rate Setup';
        $scope.ListTitle = 'Product Rate Records';
        $scope.ListTitlePIDeatails = 'Listed Item of Product Rate Details';


        $scope.ListIncentiveFormulaSetupDetails = [];
        $scope.listDistTargetMaster = [];
        $scope.listIncentiveFormulaSetupMaster = [];
        $scope.ListIncentiveDistRatio = [];
        $scope.ListAchievementRatioDetail = [];
        $scope.showDtgrid = 0;
        $scope.listDistributor = [];
        $scope.ngModelDistributor = '';

        $scope.ListBrandSkuAdded = [];

        $scope.IsCheckedDistributor = false;
        $scope.IsDisableDistributor = true;
        $scope.IsReadOnlyDistributor = true;
        debugger
        $scope.DistributorCheck = function () {
            if ($scope.IsCheckedDistributor === true) {
                $scope.IsDisableDistributor = false;
                $scope.IsReadOnlyDistributor = false;
            } else {
                $scope.ngModelDistributor = '';
                $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

                $scope.IsDisableDistributor = true;
                $scope.IsReadOnlyDistributor = true;
            }
        }
        $scope.DistributorCheck();


        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmProductRate'; DelFunc = 'DeleteIncentiveFormulaSetupMasterDetail'; DelMsg = 'IncentiveFormulaSetupNo'; EditFunc = 'loadPIMasterDetailsByActivePI';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        

        //Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsHiddenDetail = true;
        $scope.IsShow = true;
        $scope.IsFormHidden = false;
        $scope.IsHiddenBrandSku = false;
        $scope.IsHiddenRateDetail = false;

        $scope.ShowHide = function () {
            $scope.IsShow = $scope.IsShow ? false : true;
            $scope.IsHidden = $scope.IsHidden === true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden === true) {
                $scope.btnShowText = "Show List";
                $scope.IsShow = true;
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
                $scope.IsHiddenBrandSku = false;
                $scope.IsHiddenRateDetail = false;
            }
            else {
                $scope.btnShowText = "Create";
                $scope.IsShow = false;
                $scope.IsHidden = false;
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                loadMotherVesselMasterRecords(0);
                $scope.IsHiddenBrandSku = true;
                $scope.IsHiddenRateDetail = true;
            }
        };

        $scope.sstypId = '';
        $scope.listSSTYP = [];
        $scope.GetUserWiseSSTYPList = function () {
            $scope.cmnParam();
            var param = { loggeduser: objcmnParam.loggeduser };
            var apiRoute = sysCommonUrl + 'GetUserWiseSSTYP/';
            var listModel = crudService.getItemWithcmnParam(apiRoute, param);
            listModel.then(function (response) {
                $scope.listSSTYP = response.data.sstypList;
                if ($scope.listSSTYP.length > 0) {
                    $scope.sstypId = $scope.listSSTYP[0].SSTYP_OID;
                    $("#sstypId").select2("data", { id: $scope.sstypId, text: $scope.listSSTYP[0].SSTYP_NAME });

                    $scope.loadBrandRecords();
                }
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.GetUserWiseSSTYPList();

        ///// Get all brand 
        $scope.loadBrandRecords = function () {
            debugger;
            $scope.ListProductByBrand = []; // make brand list blank first
            $scope.ListBrandSkuAdded = []; // make the bottom list blank

            $scope.ngModelBrand = '';
            $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });
            $scope.listBrand = [];
            $scope.cmnParam();
            var param = { parameter: $scope.sstypId };
            var apiRoute = baseUrl + 'GetBrand/';
            var listBrand = crudService.getItemWithcmnParam(apiRoute, param, $scope.HeaderToken.get);
            listBrand.then(function (response) {
                $scope.listBrand = response.data.objBrandList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        //loadBrandRecords(0);



        // Get all distributor // should get that person who have a global role
        function loadSingleDistributor(isPaging) {
            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            var apiRoute = baseUrl + 'GetSingleDistributor/';
            var listBrand = crudService.getItemWithcmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listBrand.then(function (response) {
                $scope.listDistributor = response.data.objDistributor;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadSingleDistributor(0);





        // Save master detail data
        $scope.Save = function () {

            if ($scope.ngRefNumber === '' || $scope.ngRefNumber === undefined
                || $scope.ngActiveDate === '' || $scope.ngActiveDate === undefined
                || $scope.ngCloseDate === '' || $scope.ngCloseDate === undefined
                || $scope.ListBrandSkuAdded.length === 0 || $scope.ListBrandSkuAdded === undefined
            ) {
                Command: toastr["warning"]("Please ensure mandatory data with product list !!!");
                return;
            }

            $scope.cmnParam();
            objcmnParam.loggedCompany = 1;

            var rateMaster = {
                REF_NUMBER: $scope.ngRefNumber,
                ACTIVE_DATE: conversion.getStringToDate($scope.ngActiveDate),
                CLOSE_DATE: conversion.getStringToDate($scope.ngCloseDate),
                DIST_OID: $scope.ngModelDistributor
            };

            var apiRoute = baseUrl + 'SaveUpdateProductRate/';
            var ProductRateSaveUpdate = crudService.postModelRate(apiRoute, rateMaster, $scope.ListBrandSkuAdded, objcmnParam);
            ProductRateSaveUpdate.then(function (response) {

                if (response !== "") {
                    debugger
                    $scope.clear();
                    $scope.IncentiveFormulaSetupNo = response;
                    ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                }
                else if (response === "") {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                }
            },
                function (error) {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                });
        };


        $scope.AddBrandToDetail = function () {
            $scope.ListProductByBrand = []; // make brand list blank first
            //$scope.ListBrandSkuAdded = []; // make the bottom list blank
            var apiRoute = sysCommonUrl + 'GetSalesSKUByBrand/';

            var mode = '1';
            var trackNo = $scope.ngModelBrand;
            var listBrand = crudService.getMultipleParameter2(apiRoute, mode, trackNo, $scope.sstypId);
            listBrand.then(function (response) {
                $scope.ListProductByBrand = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
            $scope.IsHiddenDetail = false;
            $scope.showDtgrid = $scope.ListAchievementRatioDetail.length;
        };

        $scope.AddToList = function () {
            debugger;
            if ($scope.ngModelBrand == '' || $scope.ngModelBrand == undefined) {
                Command: toastr["warning"]("Please select a brand first !");
                return false;
            }
            if ($scope.ListProductByBrand.length > 0) {

                //$scope.ListBrandSkuAdded = [];

                angular.forEach($scope.ListProductByBrand, function (value) {

                    //if (value.DP_PCS > 0 && value.TP_PCS > 0 && value.MRP_PCS > 0) {//Previous Code
                    if (value.DP_PCS > 0) {
                        $scope.ListBrandSkuAdded.push({
                            BRANDOID: value.BRANDOID,
                            PACKSIZE: value.PACKSIZE,
                            PRODUCTOID: value.PRODUCTOID,
                            PRODUCT_CODE: value.PRODUCT_CODE,
                            SBRND_NAME: value.SBRND_NAME,
                            BRAND_ID: value.BRAND_ID,
                            PRODUCTNAME: value.PRODUCTNAME,
                            DP_PCS: value.DP_PCS,
                            DP_CTN: value.DP_PCS * value.PACKSIZE,
                            TP_PCS: value.TP_PCS,
                            TP_CTN: value.TP_PCS * value.PACKSIZE,
                            MRP_PCS: value.MRP_PCS,
                            MRP_CTN: value.MRP_PCS * value.PACKSIZE,
                            SWP_PCS: value.SWP_PCS,
                            SWP_CTN: value.SWP_PCS * value.PACKSIZE
                        });
                    }
                });
            }
        };

        // Delete from list while input
        $scope.deleteItem = function (element, index) {
            var IsConf = confirm('You are about to delete ' + element.PRODUCTNAME + '. Are you sure?');
            if (IsConf) {
                $scope.ListBrandSkuAdded.splice(index, 1);
            }
        };

        // Clear info after sucessful transaction
        $scope.clear = function () {
            $scope.frmProductRate.$setPristine();
            $scope.frmProductRate.$setUntouched();

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.showDtgrid = 0;

            $scope.invalid = false;
            $("#save").prop("disabled", false);

            $scope.IsUpdateMode = false;
            $scope.btnSaveText = "Save";
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;

            $scope.IsShow = true;
            $scope.IsShowD = false;
            $scope.IsListIcon = true;
            $scope.IsCreateIcon = false;
            $scope.IsHiddenDetail = true;

            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });
            $scope.IsCheckedDistributor = false;

            conversion.UniformUnChecked('chkBDistributor');


            $scope.ListProductByBrand = []; // make brand list blank first
            $scope.ListBrandSkuAdded = []; // make the bottom list blank

            $scope.ngRefNumber = '';

            $scope.ngModelBrand = '';
            $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

            $scope.sstypId = '';
            $("#sstypId").select2("data", { id: '', text: '--Select Sales Type--' });

            $scope.ngActiveDate = conversion.NowDateCustom();
            $scope.ngCloseDate = conversion.NowDateCustom();

            $scope.GetUserWiseSSTYPList();
        };
        //**********---- End Clear records ----***************//
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}




        //$scope.pagination = {
        //    paginationPageSizes: [10, 25, 50, 75, 100, 500, 1000, "All"],
        //    ddlpageSize: 10, pageNumber: 1, pageSize: 10, totalItems: 0,

        //    getTotalPages: function () {
        //        return Math.ceil(this.totalItems / this.pageSize);
        //    },
        //    pageSizeChange: function () {
        //        if (this.ddlpageSize == "All")
        //            this.pageSize = $scope.pagination.totalItems;
        //        else
        //            this.pageSize = this.ddlpageSize
        //        this.pageNumber = 1

        //        loadMotherVesselMasterRecords(1);
        //    },
        //    firstPage: function () {
        //        if (this.pageNumber > 1) {
        //            this.pageNumber = 1
        //            loadMotherVesselMasterRecords(1);
        //        }
        //    },
        //    nextPage: function () {
        //        if (this.pageNumber < this.getTotalPages()) {
        //            this.pageNumber++;
        //            loadMotherVesselMasterRecords(1);
        //        }
        //    },
        //    previousPage: function () {
        //        if (this.pageNumber > 1) {
        //            this.pageNumber--;
        //            loadMotherVesselMasterRecords(1);
        //        }
        //    },
        //    lastPage: function () {
        //        if (this.pageNumber >= 1) {
        //            this.pageNumber = this.getTotalPages();
        //            loadMotherVesselMasterRecords(1);
        //        }
        //    }
        //};
        //function loadMotherVesselMasterRecords(isPaging) {
        //    // For Paging
        //    if (isPaging == 0)
        //        $scope.pagination.pageNumber = 1;

        //    // For Loading
        //    $scope.loaderMoreIssueMaster = true;
        //    $scope.lblMessageForQCMaster = 'loading please wait....!';
        //    $scope.result = "color-red";

        //    //Ui Grid
        //    objcmnParam = {
        //        pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
        //        pageSize: $scope.pagination.pageSize,
        //        IsPaging: isPaging,
        //        loggeduser: $scope.UserCommonEntity.loggedUserID,
        //        loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
        //        menuId: $scope.UserCommonEntity.currentMenuID,
        //        tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
        //    };

        //    $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
        //        if (col.filters[0].term) {
        //            return 'header-filtered';
        //        } else {
        //            return '';
        //        }
        //    };

        //    $scope.gridOptionsPRML = {
        //        columnDefs: [
        //            { name: "RECEIVEOID", displayName: "RECEIVEOID", title: "RECEIVEOID", visible: false, width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "MVOID", displayName: "MVOID", title: "MVOID", visible: false, width: '10%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "PRODUCTOID", displayName: "PRODUCTOID", title: "PRODUCTOID", visible: false, width: '10%', headerCellClass: $scope.highlightFilteredHeader },

        //            { name: "REF_NUMBER", displayName: "REF_NUMBER", title: "REF_NUMBER", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "ACTIVE_DATE", displayName: "ACTIVE_DATE", title: "ACTIVE_DATE", width: '25%', headerCellClass: $scope.highlightFilteredHeader },
        //            { name: "CLOSE_DATE", displayName: "CLOSE_DATE", title: "CLOSE_DATE", width: '15%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
        //            {
        //                name: 'Action',
        //                displayName: "Action",
        //                pinnedRight: true,
        //                enableColumnResizing: false,
        //                enableFiltering: false,
        //                enableSorting: false,
        //                width: '12%',
        //                headerCellClass: $scope.highlightFilteredHeader,
        //                visible: $scope.UserCommonEntity.visible,

        //                cellTemplate: '<span class="label label-success label-mini" >' +
        //                    '<a href="" title="Edit" ng-click="grid.appScope.GET_MV_RECEIVE_INFO_BY_ID(row.entity)">' +
        //                    '<i class="icon-check" aria-hidden="true"></i> Edit' +
        //                    '</a>' +
        //                    '</span>'
        //            }
        //        ],
        //        enableFiltering: true,
        //        enableGridMenu: true,
        //        enableSelectAll: true,
        //        exporterCsvFilename: 'MotherVesselReceive.csv',
        //        exporterPdfDefaultStyle: { fontSize: 9 },
        //        exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
        //        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        //        exporterPdfHeader: { text: "Tenant Tagging", style: 'headerStyle' },
        //        exporterPdfFooter: function (currentPage, pageCount) {
        //            return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
        //        },
        //        exporterPdfCustomFormatter: function (docDefinition) {
        //            docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
        //            docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
        //            return docDefinition;
        //        },
        //        exporterPdfOrientation: 'portrait',
        //        exporterPdfPageSize: 'LETTER',
        //        exporterPdfMaxGridWidth: 500,
        //        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        //    };

        //    var apiRoute = baseUrl + 'GET_MV_RECEIVE_INFO/';
        //    var listTenantTagMaster = crudService.getMVMasterList(apiRoute, objcmnParam);
        //    listTenantTagMaster.then(function (response) {
        //        $scope.pagination.totalItems = response.data.recordsTotal;
        //        $scope.gridOptionsPRML.data = response.data.objMVMaster;
        //        $scope.loaderMore = false;
        //    },
        //        function (error) {
        //            //console.log("Error: " + error);
        //        });
        //}
        //loadMotherVesselMasterRecords(0);