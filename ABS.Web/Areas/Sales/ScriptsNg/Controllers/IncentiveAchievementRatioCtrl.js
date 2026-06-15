app.controller('IncentiveAchievementRatioCtrl', ['$scope', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************

        var baseUrl = '/Sales/api/IncentiveFormulaSetup/';
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsDTML = [];
        $scope.gridOptionslistItemMaster = [];

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.IsbtSaveReviseShow = true;
        $scope.IsDOCompleted = "";
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        $scope.StartDate = conversion.NowDateCustom();
        $scope.EndDate = conversion.NowDateCustom();
        
        $scope.bool = true;
        $scope.IncentiveFormulaSetupID = "0";

        $scope.btnPISaveText = "Save";
        $scope.btnPIShowText = "Show List";
        $scope.btnPIReviseText = "Update";
        $scope.PageTitle = 'Incentive Achievement Ratio';
        $scope.ListTitle = 'Incentive Formula Records';
        $scope.ListTitleActivePIMasters = 'Incentive Formula Information';
        $scope.ListTitleSampleNo = 'Brand Info';
        $scope.ListTitlePIDeatails = 'Listed Item of Incentive Achievement Ratio';

        $scope.HoldDataModel = '';

        $scope.ListIncentiveFormulaSetupDetails = [];
        $scope.listDistTargetMaster = [];
        $scope.listIncentiveFormulaSetupMaster = [];
        $scope.ListIncentiveDistRatio = [];
        $scope.ListAchievementRatioDetail = [];
        $scope.showDtgrid = 0;
        $scope.listDistributor = [];
        $scope.ngModelDistributor = '';

        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;

      
        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmIncentiveFormulaSetup'; DelFunc = 'DeleteIncentiveFormulaSetupMasterDetail'; DelMsg = 'IncentiveFormulaSetupNo'; EditFunc = 'loadPIMasterDetailsByActivePI';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        
     
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
            if ($scope.IsHidden == true) {
                $scope.clear();
            }
            else {
                $scope.RefreshMasterList();
            }
        }

        // Get all division
        function loadDivisionRecords(isPaging) {
            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '2';
            var trackNo = '0';
            var listDivision = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listDivision.then(function (response) {
                $scope.listDivision = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadDivisionRecords(0);

         // Get all region
        $scope.loadRegionRecords = function () {

            $scope.listRegion = [];
            $scope.ngModelRegion = '';
            $("#ddlRegion").select2("data", { id: '', text: '--Select Region--' });

            $scope.listZone = [];
            $scope.ngModelZone = '';
            $("#ddlZone").select2("data", { id: '', text: '--Select Zone--' });

            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '3';
            var trackNo = $scope.ngModelDivision;

            var listRegion = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listRegion.then(function (response) {
                $scope.listRegion = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }        

         // Get all sales force
        function loadSalesForce(isPaging) {
            var sflist = [
                { "value": 'S.O', "text": 'S.O' },
                { "value": 'D.S.R', "text": 'D.S.R' },
                { "value": 'Z.O', "text": 'Z.O' },
                { "value": 'R.S.M', "text": 'R.S.M' },
                { "value": 'D.S.M', "text": 'D.S.M' },
                { "value": 'D.G.M', "text": 'D.G.M' }
            ];
            $scope.listSalesForce = sflist;
        }
        loadSalesForce(0);

        ///// Get all brand 
        function loadBrandRecords(isPaging) {

            var apiRoute = baseUrl + 'GetBrand/';
            var listBrand = crudService.getItemWithcmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listBrand.then(function (response) {
                $scope.listBrand = response.data.objBrandList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadBrandRecords(0);


        //UI Grid Distributor Target Master List
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
                $scope.loadDistTargetMList(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadDistTargetMList(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadDistTargetMList(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadDistTargetMList(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadDistTargetMList(1);
                }
            }
        };        
        $scope.loadDistTargetMList = function (isPaging) {

            $scope.gridOptionsDTML.enableFiltering = true;
          
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
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

            $scope.gridOptionsDTML = {
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "DIST_TARGET_ID", displayName: "DIST_TARGET_ID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "DISTRIBUTOR_NAME", displayName: "DISTRIBUTOR NAME", width: "20%", title: "DISTRIBUTOR NAME", headerCellClass: $scope.highlightFilteredHeader },                    
                    { name: "START_DATE", displayName: "START DATE ", width: "35%", title: "START DATE ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "END_DATE", displayName: "END DATE ", width: "20%", title: "END DATE ", cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
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
                        cellTemplate: $scope.UserCommonEntity.cellTemplate
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'IncentiveFormulaSetup.csv',
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
            var apiRoute = baseUrl + 'GetIncentiveFormulaSetupMaster/';
            var listDistTargetMaster = crudService.getAPIRequestWithCmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listDistTargetMaster.then(function (response) {

                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsDTML.data = response.data.objVmPIMaster;
                $scope.loaderMoreDTML = false;
                $scope.loaderMore = false;
            },
            function (error) {
                console.log("Error: " + error);
            });
        };
        //End UI Grid Distributor Target Master List


        $scope.RefreshMasterList = function () {
            $scope.pagination.pageNumber = 1;
            $scope.loadDistTargetMList(0);
            $scope.IsHiddenDetail = true;
        }
        $scope.RefreshMasterList();


        // table for sales force incentive ratio setup
        $scope.getListDistRatio = function () {
            $scope.IsHiddenDetail = false;          
            angular.forEach($scope.listSalesForce, function (value) {

                $scope.ListIncentiveDistRatio.push({
                    SF_TYPE: value.value, SF_TYPE: value.value,
                    EACH_SF_TK_AMOUNT: 0.00
                });

            });
            $scope.showDtgrid = $scope.ListIncentiveDistRatio.length;
        }
        $scope.getListDistRatio(0);


        $scope.loadPIMasterDetailsByActivePI = function (dataModel) {
            modal_fadeOut();

            $scope.IsbtSaveReviseShow = dataModel.IsLCCompleted == false ? true : false;

            $scope.IsShow = true;
            $scope.IsHiddenDetail = false;
            //
            $scope.btnPIShowText = "Show List";
            $scope.IsHidden = true;

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;
            //

            $scope.btnPISaveText = "Update";
            $scope.listIncentiveFormulaSetupMaster = [];
            var IncentiveFormulaSetupID = dataModel.IncentiveFormulaSetupID;
            $scope.IncentiveFormulaSetupID = dataModel.IncentiveFormulaSetupID;
            $scope.TransactionTypeID = dataModel.TransactionTypeID;
            $scope.IncentiveFormulaSetupNo = dataModel.IncentiveFormulaSetupNo;
            $scope.Description = dataModel.Description;

            $scope.ngModelDistributor = dataModel.BuyerID;
            $("#ddlBuyer").select2("data", { id: dataModel.BuyerID, text: dataModel.BuyerFullName });

            $scope.lstBuyerReferenceList = dataModel.BuyerRefID;
            $("#ddlBuyerReference").select2("data", { id: dataModel.BuyerRefID, text: dataModel.BuyerReferenceFullName });

            $scope.bool = false;

            var apiRoute = baseUrl + 'GetIncentiveFormulaSetupDetail/';
            var ListIncentiveFormulaSetupDetails = crudService.getPIDetailsByActivePIID(apiRoute, $scope.IncentiveFormulaSetupID, $scope.HeaderToken.get);
            ListIncentiveFormulaSetupDetails.then(function (response) {
                $scope.ListIncentiveFormulaSetupDetails = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        

        // Save master detail data
        $scope.Save = function () {
            $scope.cmnParam();
            objcmnParam.loggedCompany = 1;

            //Incentive Achievement Ratio
            var IncenAchRatio = {
                ACH_RATIO_ID: $scope.ACH_RATIO_ID,
                INCEN_FORMU_ID: $scope.ngIncentiveFormula,
                SF_TYPE: $scope.ngSalesForce,
                START_DATE: conversion.getStringToDate($scope.StartDate),
                END_DATE: conversion.getStringToDate($scope.EndDate)
            };


            // var HeaderTokenPutPost = $scope.INCEN_ID == 0 ? $scope.HeaderToken.post : $scope.HeaderToken.put;

            // ModelsArray = [itemMaster, incentiveRate, objcmnParam];

            var apiRoute = baseUrl + 'SaveUpdateIncentiveAchievementRatio/';
            var IncentiveFormulaSaveUpdate = crudService.postModels(apiRoute, IncenAchRatio, $scope.ListAchievementRatioDetail);
            IncentiveFormulaSaveUpdate.then(function (response) {
                var result = 0;
                //message = $scope.IncentiveFormulaSetupID == 0 ? 'Saved' : 'Updated';
                if (response != "") {
                    $scope.clear();
                    $scope.IncentiveFormulaSetupNo = response;
                    ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                }
                else if (response == "") {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    //$("#save").prop("disabled", false);
                }
            },
                function (error) {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                });
        };


        ///// Get all brand UI grid
        $scope.paginationItemMaster = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

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
        $scope.loadSampleNoModalRecords = function (isPaging) {

            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });

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
            $scope.cmnParam();
            objcmnParam.pageNumber = (($scope.paginationItemMaster.pageNumber - 1) * $scope.paginationItemMaster.pageSize);
            objcmnParam.pageSize = $scope.paginationItemMaster.pageSize;
            objcmnParam.IsPaging = isPaging;

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionslistItemMaster = {
                columnDefs: [
                    { name: "BRANDID", displayName: "BRANDID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SBRND_TEXT", displayName: "SBRND TEXT", title: "SBRND TEXT", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SBRND_NAME", displayName: "SBRND NAME", title: "SBRND NAME", width: '30%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "IDAT", displayName: "I DAT", title: "I DAT", width: '20%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "EDAT", displayName: "E DAT", title: "E DAT", width: '20%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Action',
                        displayName: "Action",
                        width: '20%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        // visible: $scope.UserCommonEntity.EnableUpdate,
                        cellTemplate: '<span class="label label-warning label-mini" style="text-align:center !important;">' +
                            '<a href="" title="Select" ng-click="grid.appScope.AddBrandToDetail(row.entity)">' +
                            '<i class="icon-check" aria-hidden="true"></i> Add' +
                            '</a>' +
                            '</span>'
                    }
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                },
                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'BrandData.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "BrandData", style: 'headerStyle' },
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

            var apiRoute = baseUrl + 'GetBrandPopUp/';
            var listItemMaster = crudService.getItemWithcmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listItemMaster.then(function (response) {
                $scope.paginationItemMaster.totalItems = response.data.recordsTotal;
                $scope.gridOptionslistItemMaster.data = response.data.objBrandList;
                $scope.loaderMoreItemMaster = false;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        ///// End Get all brand UI grid


        ///// Get all current incentive 
        function loadIncentiveFormula(isPaging) {

            var apiRoute = baseUrl + 'GetIncentiveFormula/';
            var listIncentiveFormula = crudService.getItemWithcmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listIncentiveFormula.then(function (response) {
                $scope.listIncentiveFormula = response.data.objIncenFormula;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadIncentiveFormula(0);

        $scope.AddBrandToDetail = function (dataModel) {

            $scope.IsHiddenDetail = false;
            var existItem = dataModel.BRANDID;
            var duplicateItem = 0;
            angular.forEach($scope.ListAchievementRatioDetail, function (item) {
                if (existItem == item.BRANDID) {
                    duplicateItem = 1;
                    return false;
                }
            });

            if (duplicateItem === 0) {
                $scope.ListAchievementRatioDetail.push({
                    ID: 0, ACH_RATIO_ID: 0, BRAND_ID: dataModel.BRANDID, SBRND_NAME: dataModel.SBRND_NAME,
                    PRIMARY_PERC: 0.00, SECONDARY_PERC: 0.00, PERCENTAGE_TK: 0.00
                }
                );
                Command: toastr["info"]("Brand Successfully Added.");
            }
            else if (duplicateItem === 1) {
                Command: toastr["warning"]("Brand Already Exists!!!!");
            }
            $scope.showDtgrid = $scope.ListAchievementRatioDetail.length;
        }

        // Delete from list while input
        $scope.deleteRow = function (index, dataModel) {
            $scope.ListAchievementRatioDetail.splice(index, 1);
            $scope.showDtgrid = $scope.ListAchievementRatioDetail.length;
        };


        // Delete master detail record
        $scope.DeleteIncentiveFormulaSetupMasterDetail = function (dataModel) { 
            $scope.cmnParam();
            objcmnParam.id = dataModel.IncentiveFormulaSetupID;
            ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'DeleteUpdateIncentiveFormulaSetupMasterDetails/';
            var delIncentiveFormulaSetup = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delIncentiveFormulaSetup.then(function (response) {
                if (response.data.result != "") {
                    $scope.RefreshMasterList();
                    Command: toastr["success"](dataModel.IncentiveFormulaSetupNo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](dataModel.IncentiveFormulaSetupNo + " not Deleted, Please Check and Try Again!");
                }
            },
            function (error) {
                Command: toastr["warning"](dataModel.IncentiveFormulaSetupNo + " not Deleted, Please Check and Try Again!");
                console.log("Error: " + error);
            });
        }

        // Clear info after sucessful transaction
        $scope.clear = function () {
            $scope.frmIncentiveFormulaSetup.$setPristine();
            $scope.frmIncentiveFormulaSetup.$setUntouched();

            $scope.IsbtSaveReviseShow = $scope.IsLCCompleted == true ? false : true;

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.IncentiveFormulaSetupID = '0';
            $scope.IncentiveFormulaSetupNo = '';
            $scope.showDtgrid = 0;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;
            $scope.btnPIShowText = "Show List";
            $scope.btnPISaveText = "Save";
            $scope.listIncentiveFormulaSetupMaster = [];
            $scope.ListIncentiveDistRatio = [];
            $scope.bool = true;
            $scope.listDistributor = [];

            $scope.Description = '';


            $scope.listRegion = [];
            $scope.ngModelRegion = '';
            $("#ddlRegion").select2("data", { id: '', text: '--Select Region--' });

            $scope.listZone = [];
            $scope.ngModelZone = '';
            $("#ddlZone").select2("data", { id: '', text: '--Select Zone--' });

            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });


            $scope.ngModelDistributor = '';
            $scope.lstSampleNoList = '';
            $("#ddlItemGroup").select2("data", { id: '', text: '--Select Brand--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();
        };
        //**********---- End Clear records ----***************//
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}