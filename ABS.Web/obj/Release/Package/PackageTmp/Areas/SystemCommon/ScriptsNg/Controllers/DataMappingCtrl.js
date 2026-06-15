app.controller('DataMappingCtrl', ['$scope', 'crudService', 'uiGridConstants', '$q', '$http', 'conversion', '$filter', '$localStorage',
    function ($scope, crudService, uiGridConstants, $q, $http, conversion, $filter, $localStorage) {
        var baseUrl = '/SystemCommon/api/DataMapping/';
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptions = [];
        var LoginUserID = $('#hUserID').val();
        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";
        $scope.PageTitle = 'Data Mapping Entry';
        $scope.ListTitle = 'Data Mapping Records';

        $scope.COMPANY_OID = '';
        $scope.SSTYP_OID = '';
        $scope.SDVNT_OID = '';
        $scope.SPROG_OID = '';
        $scope.GROUP_OID = '';
        $scope.USER_OID = '';
        $scope.BRAND_OID = '';
        $scope.SKU_OID = '';
        $scope.ROLE_OID = '';
        $scope.NTNAL_OID = '';
        $scope.IS_ENABLE = false;
        $scope.IS_UPDATE = false;
        $scope.TRAN_TYPE_ID = '';
        $scope.CMN_OID = '';
        $scope.USER_FULLNAME = '';
        $scope.USER_PASS = '';

        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmDataMapping'; DelFunc = 'delete'; DelMsg = 'DataMapping'; EditFunc = 'edit';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        

        $scope.mappingTypeList = [
            { TRAN_TYPE_ID: '1', TRAN_TYPE_NAME: 'Company And SSTyp Setup (Many to many)' },
            { TRAN_TYPE_ID: '2', TRAN_TYPE_NAME: 'SDVNT Wise Product Group Setup-for product group' },
            { TRAN_TYPE_ID: '3', TRAN_TYPE_NAME: 'User wise Group and Brand Setup-for sales group (Many to many)' },
            { TRAN_TYPE_ID: '4', TRAN_TYPE_NAME: 'User wise Group, Brand and SKU Setup (Many to many)' },
            { TRAN_TYPE_ID: '5', TRAN_TYPE_NAME: 'None Sales Person Permission' },
            { TRAN_TYPE_ID: '6', TRAN_TYPE_NAME: 'Temporary User for MIS Portal' },
            { TRAN_TYPE_ID: '7', TRAN_TYPE_NAME: 'User Menu Mode Permission' },
            { TRAN_TYPE_ID: '8', TRAN_TYPE_NAME: 'Set User wise Sales Group (UsLine-Cityn)' },
            { TRAN_TYPE_ID: '9', TRAN_TYPE_NAME: 'Set User for Sales Soft V3 (TUser-Cityn)' },
            { TRAN_TYPE_ID: '10', TRAN_TYPE_NAME: 'Set Sales Group wise Product (Line Prod-Cityn)' },
            { TRAN_TYPE_ID: '11', TRAN_TYPE_NAME: 'Open New Product (Product-Cityn)' },
        ];

        $scope.roleList = [{ ROLE_OID: '16', ROLE_NAME: 'All National' }, { ROLE_OID: '15', ROLE_NAME: 'Single National' }]

        $scope.MappingTypeChange = function () {
            debugger;
            $scope.COMPANY_OID = '';
            $scope.SSTYP_OID = '';
            $scope.SDVNT_OID = '';
            $scope.SPROG_OID = '';
            $scope.GROUP_OID = '';
            $scope.USER_OID = '';
            $scope.BRAND_OID = '';
            $scope.SKU_OID = '';
            $scope.ROLE_OID = '';
            $scope.NTNAL_OID = '';
            $scope.IS_ENABLE = false;
            $scope.IS_UPDATE = false;
            $scope.CMN_OID = '';
            $scope.USER_FULLNAME = '';
            $scope.USER_PASS = '';
            if ($scope.TRAN_TYPE_ID == '1') {
                $scope.loadAllSSTypList();
                $scope.loadAllCompanyList();
            } else if ($scope.TRAN_TYPE_ID == '2') {
                $scope.loadAllSdvntList();
                $scope.loadAllPrdGroupList();
            } else if ($scope.TRAN_TYPE_ID == '3' || $scope.TRAN_TYPE_ID == '4') {
                $scope.loadAllStaffList();
                $scope.loadAllBrndGroupList();
                $scope.loadAllBrandList();
            } else if ($scope.TRAN_TYPE_ID == '5') {
                $scope.loadAllStaffList();
                $scope.loadNationalRecords();
            } else if ($scope.TRAN_TYPE_ID == '7') {
                $scope.loadAllStaffList();
                $scope.LoadModule();
            } else if ($scope.TRAN_TYPE_ID == '8') {
                $scope.IS_ENABLE = true;
                $scope.loadAllUserSoftV3();
                $scope.loadAllSdvntList();
            } else if ($scope.TRAN_TYPE_ID == '10') {
                $scope.IS_ENABLE = true;
                $scope.loadAllSdvntList();
                //$scope.loadFilteredProduct();
            }

            $scope.isShowList = true;

            if ($scope.TRAN_TYPE_ID != '7') {
                $scope.loadDataMappingList(0);
            }
        }

        $scope.isShowList = false;
        $scope.loadList = function () {
            debugger;
            if ($scope.TRAN_TYPE_ID != '7') {
                $scope.isShowList = true;
                $scope.loadDataMappingList();
            } else if ($scope.TRAN_TYPE_ID == '7') {
                if ($scope.CMN_OID != '' && $scope.USER_OID != '') {
                    $scope.isShowList = true;
                    $scope.loadDataMappingList();
                }
            }
        }

        //*********************************************************************START TRAN_TYPE_ID=='1'*******************************************************************************
        // Get all SSTyp
        $scope.ssTypList = [];
        $scope.loadAllSSTypList = function () {
            $scope.ssTypList = [];
            $scope.SSTYP_OID = '';
            $("#SSTYP_OID").select2("data", { id: '', text: '--Select Sales Type--' });
            var apiRoute = sysCommonUrl + 'GetProductTypeSSTYP/';
            var ssTypeRecords = crudService.getMultipleParameter(apiRoute, '0', '0');
            ssTypeRecords.then(function (response) {
                $scope.ssTypList = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        // Get all SSTyp

        // Get all company
        $scope.companyList = [];
        $scope.loadAllCompanyList = function () {
            $scope.companyList = [];
            $scope.COMPANY_OID = '';
            $("#COMPANY_OID").select2("data", { id: '', text: '--Select Company--' });
            var apiRoute = sysCommonUrl + 'getCityCompanyList/';
            var compRecords = crudService.getAllList(apiRoute);
            compRecords.then(function (response) {
                $scope.companyList = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        // Get all company
        //*********************************************************************END TRAN_TYPE_ID=='1'*******************************************************************************

        //********************************************************************START TRAN_TYPE_ID=='2'******************************************************************************
        // Get all Sales Group
        $scope.sdvntList = [];
        $scope.loadAllSdvntList = function () {
            var cmnParam = { id: 0 };
            $scope.SDVNT_OID = '';
            $scope.sdvntList = [];
            $("#SDVNT_OID").select2("data", { id: '', text: '--Select Sales Group--' });
            var apiRoute = sysCommonUrl + 'GetAllSalesGroup/';
            var sdvntRecords = crudService.postMultipleModels(apiRoute, cmnParam, $scope.HeaderToken.get);
            sdvntRecords.then(function (response) {
                $scope.sdvntList = response.data.slsGroupList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        // Get all Sales Group

        // Get all Product Group
        $scope.prdGroupList = [];
        $scope.loadAllPrdGroupList = function () {
            var cmnParam = { id: 0 };
            $scope.SPROG_OID = '';
            $scope.prdGroupList = [];
            $("#SPROG_OID").select2("data", { id: '', text: '--Select Product Group--' });
            var apiRoute = sysCommonUrl + 'GetAllProductGroup/';
            var prdGroupRecords = crudService.postMultipleModels(apiRoute, cmnParam, $scope.HeaderToken.get);
            prdGroupRecords.then(function (response) {
                $scope.prdGroupList = response.data.prdGroupList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        // Get Product Group
        //*********************************************************************END TRAN_TYPE_ID=='2'******************************************************************************

        //********************************************************************START TRAN_TYPE_ID=='3'*****************************************************************************
        // Get all Staff
        $scope.staffList = [];
        $scope.loadAllStaffList = function () {
            $scope.USER_OID = '';
            $scope.staffList = [];
            $("#USER_OID").select2("data", { id: '', text: '--Select Staff--' });
            var apiRoute = sysCommonUrl + 'GetUser/';
            var staffRecords = crudService.getAllIncludingCompanyuser(apiRoute, 0, 0, 0, 0, 0);
            staffRecords.then(function (response) {
                $scope.staffList = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        // Get all Staff

        // Get all Brand Group
        $scope.brndGroupList = [];
        $scope.loadAllBrndGroupList = function () {
            var cmnParam = { id: 0 };
            $scope.GROUP_OID = '';
            $scope.brndGroupList = [];
            $("#GROUP_OID").select2("data", { id: '', text: '--Select Brand Group--' });
            var apiRoute = sysCommonUrl + 'GetAllBrandGroup/';
            var brndGroupRecords = crudService.postMultipleModels(apiRoute, cmnParam, $scope.HeaderToken.get);
            brndGroupRecords.then(function (response) {
                $scope.brndGroupList = response.data.brndGroupList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        // Get Brand Group

        // Get all Brand Group
        $scope.brandList = [];
        $scope.loadAllBrandList = function () {
            var cmnParam = { id: 0 };
            $scope.BRAND_OID = '';
            $scope.brandList = [];
            $("#BRAND_OID").select2("data", { id: '', text: '--Select Brand--' });
            var apiRoute = sysCommonUrl + 'GetBrand/';
            var brndGroupRecords = crudService.postMultipleModels(apiRoute, cmnParam, $scope.HeaderToken.get);
            brndGroupRecords.then(function (response) {
                $scope.brandList = response.data.objBrandList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        // Get Brand Group
        //********************************************************************END TRAN_TYPE_ID=='3'*****************************************************************************

        //*******************************************************************START TRAN_TYPE_ID=='4'****************************************************************************
        // Get all Staff
        // Get all Staff

        // Get all Brand Group
        // Get all Brand Group

        // Get all Brand        
        // Get all Brand

        //Get all sku
        $scope.listSKU = [];
        $scope.loadSKURecords = function () {
            var cmnParam = { strId: $scope.BRAND_OID };
            $scope.listSKU = [];
            $scope.SKU_OID = '';
            $("#SKU_OID").select2("data", { id: '', text: '--Select Product--' });
            var apiRoute = sysCommonUrl + 'GetAllProductByBrand/';
            var skuRecords = crudService.postMultipleModels(apiRoute, cmnParam, $scope.HeaderToken.get);
            skuRecords.then(function (response) {
                $scope.listSKU = response.data.skuList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        //Get all sku
        //*******************************************************************END TRAN_TYPE_ID=='4'****************************************************************************

        //*******************************************************************START TRAN_TYPE_ID=='5'****************************************************************************
        // Get all Staff
        // Get all Staff

        // Get all Brand Group
        // Get all Brand Group

        // Get all Brand        
        // Get all Brand

        //Get all sku
        $scope.listNational = [];
        $scope.loadNationalRecords = function () {
            $scope.listNational = [];
            $scope.NTNAL_OID = '';
            $("#NTNAL_OID").select2("data", { id: '', text: '--Select National--' });
            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var ntnalRecords = crudService.getMultipleParameter(apiRoute, '16', null, $scope.HeaderToken.get);
            ntnalRecords.then(function (response) {
                $scope.listNational = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        //Get all sku
        //*******************************************************************END TRAN_TYPE_ID=='4'****************************************************************************

        //*******************************************************************START TRAN_TYPE_ID=='7'****************************************************************************
        // Get all Staff
        // Get all Staff

        //Get All Module
        $scope.LoadModule = function () {
            $scope.CMN_OID = '';
            $scope.ListModule = [];
            $("#moduleId").select2("data", { id: 0, text: '--Select Module--' });
            var apiRoute = sysCommonUrl + 'GetModuleWithPermission/';
            var listModuleProcess = crudService.getAllIncludingCompanyLog(apiRoute, 1, 1, 0, 0, 0);
            listModuleProcess.then(function (response) {
                $scope.ListModule = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        //$scope.LoadModule(0);
        //Get All Module
        //*******************************************************************END TRAN_TYPE_ID=='7'****************************************************************************

        //*******************************************************************START TRAN_TYPE_ID=='8'****************************************************************************
        // Get all Staff Soft V3
        $scope.loadAllUserSoftV3 = function () {
            var cmnParam = { id: 0 };
            $scope.USER_OID = '';
            $scope.staffList = [];
            $("#USER_OID_8").select2("data", { id: '', text: '--Select Staff--' });
            var apiRoute = sysCommonUrl + 'getalluser_softv3/';
            var usrRecords = crudService.postMultipleModels(apiRoute, cmnParam, $scope.HeaderToken.get);
            usrRecords.then(function (response) {
                $scope.staffList = response.data.usrV3;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        // Get all Staff Soft V3

        // Get all Sales Group
        //*******************************************************************END TRAN_TYPE_ID=='8'****************************************************************************

        //*******************************************************************START TRAN_TYPE_ID=='10'****************************************************************************        
        //load all sdvnt
        //load all sdvnt
        // Get all sstyp wise product
        $scope.loadFilteredProduct = function () {
            debugger;
            var sdvntModel = $scope.sdvntList.filter(x => x.SDVNT_OID == $scope.SDVNT_OID)[0];
            var cmnParam = { strId: sdvntModel.SDVNT_SCOMP, strId2: sdvntModel.SDVNT_SSTYP, strId3: sdvntModel.SDVNT_OID };
            $scope.listSKU = [];
            $scope.SKU_OID = '';
            $("#SKU_OID_10").select2("data", { id: '', text: '--Select Product--' });
            var apiRoute = sysCommonUrl + 'GetAllProductBySSTYP/';
            var skuRecords = crudService.postMultipleModels(apiRoute, cmnParam, $scope.HeaderToken.get);
            skuRecords.then(function (response) {
                $scope.listSKU = response.data.productList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        // Get all sstyp wise product

        // Get all Sales Group
        //*******************************************************************END TRAN_TYPE_ID=='10'****************************************************************************

        $scope.loadDataMappingList = function (isPaging) {
            //$scope.cmnParam();
            var params = {
                COMPANY_OID: $scope.COMPANY_OID,
                SSTYP_OID: $scope.SSTYP_OID,
                SDVNT_OID: $scope.SDVNT_OID,
                SPROG_OID: $scope.SPROG_OID,
                GROUP_OID: $scope.GROUP_OID,
                USER_OID: $scope.USER_OID,
                BRAND_OID: $scope.BRAND_OID,
                SKU_OID: $scope.SKU_OID,
                ROLE_OID: $scope.ROLE_OID,
                NTNAL_OID: $scope.NTNAL_OID,
                IS_ENABLE: $scope.IS_ENABLE,
                IS_UPDATE: $scope.IS_UPDATE,
                TRAN_TYPE_ID: $scope.TRAN_TYPE_ID,
                CMN_OID: $scope.CMN_OID,
                USER_FULLNAME: $scope.USER_FULLNAME,
                USER_PASS: $scope.USER_PASS,
                pageNumber: 0,
                pageSize: 0,
            };
            $scope.gridOptions.enableFiltering = true;

            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            if ($scope.TRAN_TYPE_ID == '1') {
                $scope.gridOptions = {
                    rowTemplate: $scope.UserCommonEntity.rowTemplate,
                    columnDefs: [
                        { name: "COMPANY_OID", displayName: "COMPANY_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "COMPANY_NAME", displayName: "COMPANY", width: "45%", title: "Company", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SSTYP_OID", displayName: "SSTYP_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SSTYP_NAME", displayName: "SSTYP", width: "45%", title: "Sales Type", headerCellClass: $scope.highlightFilteredHeader },
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
                            cellTemplate: '<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                                '<a href="" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                '<i class="icon-check" aria-hidden="true"></i> Delete </a></span>'
                        }
                    ]
                };
            } else if ($scope.TRAN_TYPE_ID == '2') {
                $scope.gridOptions = {
                    rowTemplate: $scope.UserCommonEntity.rowTemplate,
                    columnDefs: [
                        { name: "SDVNT_OID", displayName: "SDVNT_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SDVNT_NAME", displayName: "Sales Group", width: "45%", title: "Sales Group", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SPROG_OID", displayName: "SPROG_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SPROG_NAME", displayName: "Product Group", width: "45%", title: "Product Group", headerCellClass: $scope.highlightFilteredHeader },
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
                            cellTemplate: '<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                                '<a href="" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                '<i class="icon-check" aria-hidden="true"></i> Delete </a></span>'
                        }
                    ]
                };
            } else if ($scope.TRAN_TYPE_ID == '3') {
                $scope.gridOptions = {
                    rowTemplate: $scope.UserCommonEntity.rowTemplate,
                    columnDefs: [
                        { name: "USER_OID", displayName: "USER_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "USER_NAME", displayName: "Staff", width: "30%", title: "Staff", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "GROUP_OID", displayName: "GROUP_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "GROUP_NAME", displayName: "Group", width: "30%", title: "Group", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "BRND_OID", displayName: "BRND_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "BRAND_NAME", displayName: "Brand", width: "30%", title: "Brand", headerCellClass: $scope.highlightFilteredHeader },
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
                            cellTemplate: '<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                                '<a href="" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                '<i class="icon-check" aria-hidden="true"></i> Delete </a></span>'
                        }
                    ]
                };
            } else if ($scope.TRAN_TYPE_ID == '4') {
                $scope.gridOptions = {
                    rowTemplate: $scope.UserCommonEntity.rowTemplate,
                    columnDefs: [
                        { name: "USER_OID", displayName: "USER_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "USER_NAME", displayName: "Staff", width: "35%", title: "Staff", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "GROUP_OID", displayName: "GROUP_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "GROUP_NAME", displayName: "Group", width: "10%", title: "Group", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "BRND_OID", displayName: "BRND_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "BRAND_NAME", displayName: "Brand", width: "10%", title: "Brand", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SKU_OID", displayName: "SKU_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SKU_NAME", displayName: "Product", width: "35%", title: "Product", headerCellClass: $scope.highlightFilteredHeader },
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
                            cellTemplate: '<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                                '<a href="" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                '<i class="icon-check" aria-hidden="true"></i> Delete </a></span>'
                        }
                    ]
                };
            } else if ($scope.TRAN_TYPE_ID == '5') {
                $scope.gridOptions = {
                    rowTemplate: $scope.UserCommonEntity.rowTemplate,
                    columnDefs: [
                        { name: "USER_OID", displayName: "USER_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "USER_NAME", displayName: "Staff", width: "45%", title: "Staff", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "NTNAL_OID", displayName: "NTNAL_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "NTNAL_NAME", displayName: "National", width: "25%", title: "National", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "ROLE_OID", displayName: "ROLE_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "ROLE_NAME", displayName: "Role", width: "10%", title: "Role", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SKU_OID", displayName: "SKU_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "IS_ENABLE", displayName: "Is Enabled", width: "10%", title: "Is Enabled", headerCellClass: $scope.highlightFilteredHeader },
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
                            cellTemplate: '<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                                '<a href="" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                '<i class="icon-check" aria-hidden="true"></i> Delete </a></span>'
                        }
                    ]
                };
            } else if ($scope.TRAN_TYPE_ID == '6' || $scope.TRAN_TYPE_ID == '9') {
                $scope.gridOptions = {
                    rowTemplate: $scope.UserCommonEntity.rowTemplate,
                    columnDefs: [
                        { name: "USER_OID", displayName: "Staff ID", width: "20%", title: "Staff ID", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "USER_FULLNAME", displayName: "Staff Name", width: "60%", title: "Staff Name", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "IS_ENABLE", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "ACTIVE_STATUS", displayName: "Is Active", width: "10%", title: "Is Active", headerCellClass: $scope.highlightFilteredHeader },
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
                            cellTemplate: '<span class="label label-success label-mini" style="text-align:center !important;">' +
                                '<a href="" title="Delete" ng-click="grid.appScope.edit(row.entity)">' +
                                '<i class="icon-check" aria-hidden="true"></i> Edit </a></span>' +
                                '<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                                '<a href="" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                '<i class="icon-trash" aria-hidden="true"></i> Delete </a></span>'
                        }
                    ]
                };
            } else if ($scope.TRAN_TYPE_ID == '7') {
                $scope.gridOptions = {
                    rowTemplate: $scope.UserCommonEntity.rowTemplate,
                    columnDefs: [
                        { name: "MENUNAME", displayName: "Menu Name", width: "55%", title: "Menu Name", headerCellClass: $scope.highlightFilteredHeader },
                        {
                            name: "ISLINE", displayName: "Is Line", width: '10%', cellClass: 'text-center', type: 'boolean', headerCellClass: 'center',
                            cellTemplate: '<input type="checkbox" ng-model="row.entity.ISLINE">', enableCellEdit: false, enableFiltering: false,
                        },
                        {
                            name: "ISCOMPANY", displayName: "Is Company", width: '10%', cellClass: 'text-center', type: 'boolean', headerCellClass: 'center',
                            cellTemplate: '<input type="checkbox" ng-model="row.entity.ISCOMPANY">', enableCellEdit: false, enableFiltering: false,
                        },
                        {
                            name: "ISLOCATION", displayName: "Is Location", width: '10%', cellClass: 'text-center', type: 'boolean', headerCellClass: 'center',
                            cellTemplate: '<input type="checkbox" ng-model="row.entity.ISLOCATION">', enableCellEdit: false, enableFiltering: false,
                        },
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
                            cellTemplate: '<span class="label label-success label-mini" style="text-align:center !important;">' +
                                '<a href="" title="Delete" ng-click="grid.appScope.edit(row.entity)">' +
                                '<i class="icon-check" aria-hidden="true"></i> Edit </a></span>' +
                                '<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                                '<a href="" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                '<i class="icon-trash" aria-hidden="true"></i> Delete </a></span>'
                        }
                    ]
                };
            } else if ($scope.TRAN_TYPE_ID == '8') {
                $scope.gridOptions = {
                    rowTemplate: $scope.UserCommonEntity.rowTemplate,
                    columnDefs: [
                        { name: "USLINE_OID", displayName: "USLINE OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "USER_OID", displayName: "USER_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "USER_NAME", displayName: "Staff", width: "40%", title: "Staff", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SDVNT_OID", displayName: "SDVNT_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SDVNT_NAME", displayName: "Sales Group", width: "40%", title: "Sales Group", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "ISACTIVE", displayName: "Is Active", width: "10%", title: "Is Active", headerCellClass: $scope.highlightFilteredHeader },
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
                            cellTemplate: '<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                                '<a href="" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                '<i class="icon-check" aria-hidden="true"></i> Delete </a></span>'
                        }
                    ]
                };
            } else if ($scope.TRAN_TYPE_ID == '10') {
                $scope.gridOptions = {
                    rowTemplate: $scope.UserCommonEntity.rowTemplate,
                    columnDefs: [
                        { name: "SDVNT_OID", displayName: "SDVNT_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SDVNT_NAME", displayName: "Sales Group", width: "45%", title: "Sales Group", headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SKU_OID", displayName: "SKU_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                        { name: "SPROD_NAME", displayName: "Product", width: "45%", title: "Product", headerCellClass: $scope.highlightFilteredHeader },
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
                            cellTemplate: '<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                                '<a href="" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                                '<i class="icon-check" aria-hidden="true"></i> Cancel </a></span>'
                        }
                    ]
                };
            }


            var apiRoute = baseUrl + 'getbypage/';
            var dataMappingRecords = crudService.postMultipleModels(apiRoute, params, $scope.HeaderToken.get);
            dataMappingRecords.then(function (response) {
                debugger;
                //$scope.pagination.totalItems = response.data.recordsTotal;
                if ($scope.TRAN_TYPE_ID == '7') {
                    angular.forEach(response.data.resdata, function (item) {
                        item.ISLINE = item.ISLINE == 0 ? false : true;
                        item.ISCOMPANY = item.ISCOMPANY == 0 ? false : true;
                        item.ISLOCATION = item.ISLOCATION == 0 ? false : true;
                        item.ISMODE = item.ISMODE == '0' ? false : true;
                    })
                }

                $scope.gridOptions.data = response.data.resdata;
                $scope.loaderMore = false;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };

        //$scope.loadDataMappingList(0);

        $scope.edit = function (dataModel) {
            if ($scope.TRAN_TYPE_ID == '6' || $scope.TRAN_TYPE_ID == '9') {
                debugger;
                $scope.USER_OID = $scope.TRAN_TYPE_ID == '9' ? dataModel.USER_TEXT : dataModel.USER_OID;
                $scope.USER_FULLNAME = dataModel.USER_FULLNAME;
                $scope.USER_PASS = dataModel.USER_PASSWORD;
                $scope.IS_ENABLE = dataModel.IS_ENABLE == 1 ? true : false;
                $scope.IS_UPDATE = true;
            }
        }

        $scope.delete = function (entity) {
            var IsConf = confirm('You are about to delete. Are you sure?');
            if (IsConf) {
                entity.TRAN_TYPE_ID = $scope.TRAN_TYPE_ID;
                var apiRoute = baseUrl + 'delete/';
                var delRecords = crudService.postMultipleModels(apiRoute, entity, $scope.HeaderToken.delete);
                delRecords.then(function (response) {
                    debugger
                    if (response != "") {
                        if (response.data.resdata == '1') {
                            $scope.clear();
                            $scope.loadDataMappingList(0);
                            ShowCustomToastr('success', 'Data deleted successfully !!!');
                        } else {
                            ShowCustomToastr('warning', 'Data not deleted, Please try again later !!!');
                        }
                    }
                    else if (response == "") {
                        ShowCustomToastr('warning', 'Data not deleted, Please try again later !!!');
                    }
                },
                    function (error) {
                        ShowCustomToastr('warning', 'Data not deleted, Please try again later !!!');
                    });
            }
        }

        // Save master detail data
        $scope.Save = function () {
            debugger;
            if ($scope.TRAN_TYPE_ID == '10' && $scope.SDVNT_OID != '' && $scope.SDVNT_OID != null && $scope.SDVNT_OID != undefined) {
                var sdvntModel = $scope.sdvntList.filter(x => x.SDVNT_OID == $scope.SDVNT_OID)[0];
                if (sdvntModel != undefined) {
                    $scope.CMN_OID = sdvntModel.SDVNT_SCTYP;
                    $scope.COMPANY_OID = sdvntModel.SDVNT_SCOMP;
                }
            }

            var master = {
                COMPANY_OID: $scope.COMPANY_OID,
                SSTYP_OID: $scope.SSTYP_OID,
                SDVNT_OID: $scope.SDVNT_OID,
                SPROG_OID: $scope.SPROG_OID,
                GROUP_OID: $scope.GROUP_OID,
                USER_OID: $scope.USER_OID,
                BRAND_OID: $scope.BRAND_OID,
                SKU_OID: $scope.SKU_OID,
                ROLE_OID: $scope.ROLE_OID,
                NTNAL_OID: $scope.NTNAL_OID,
                IS_ENABLE: $scope.IS_ENABLE,
                IS_UPDATE: $scope.IS_UPDATE,
                TRAN_TYPE_ID: $scope.TRAN_TYPE_ID,
                CMN_OID: $scope.CMN_OID,
                USER_FULLNAME: $scope.USER_FULLNAME,
                USER_PASS: $scope.USER_PASS,
                LOGGED_USER: LoginUserID,
                pageNumber: 0,
                pageSize: 0,
                JSON_DATA: ''
            };

            if ($scope.TRAN_TYPE_ID == '1' && (master.COMPANY_OID == '' || master.SSTYP_OID == '')) {
                ShowCustomToastr('warning', 'Please check and input valid information!!!');
                return;
            } else if ($scope.TRAN_TYPE_ID == '2' && (master.SDVNT_OID == '' || master.SPROG_OID == '')) {
                ShowCustomToastr('warning', 'Please check and input valid information!!!');
                return;
            } else if ($scope.TRAN_TYPE_ID == '3' && (master.GROUP_OID == '' || master.USER_OID == '' || master.BRAND_OID == '')) {
                ShowCustomToastr('warning', 'Please check and input valid information!!!');
                return;
            } else if ($scope.TRAN_TYPE_ID == '4' && (master.GROUP_OID == '' || master.USER_OID == '' || master.BRAND_OID == '' || master.SKU_OID == '')) {
                ShowCustomToastr('warning', 'Please check and input valid information!!!');
                return;
            } else if ($scope.TRAN_TYPE_ID == '5' && (master.USER_OID == '' || master.ROLE_OID == '')) {
                ShowCustomToastr('warning', 'Please check and input valid information!!!');
                return;
            } else if (($scope.TRAN_TYPE_ID == '6' || $scope.TRAN_TYPE_ID == '9') && (master.USER_OID == '' || master.USER_FULLNAME == '' || master.USER_PASS == '')) {
                ShowCustomToastr('warning', 'Please check and input valid information!!!');
                return;
            } else if ($scope.TRAN_TYPE_ID == '7') {
                debugger;
                var glist = $scope.gridOptions.data.filter(x => x.ISLINE || x.ISCOMPANY || x.ISLOCATION || x.ISMODE);
                if (glist.length == 0) {
                    ShowCustomToastr('warning', 'Please tick on any checkbox in list!!!');
                    return;
                } else {
                    master.JSON_DATA = JSON.stringify(glist);
                }
            } else if ($scope.TRAN_TYPE_ID == '8' && (master.SDVNT_OID == '' || master.USER_OID == '')) {
                ShowCustomToastr('warning', 'Please check and input valid information!!!');
                return;
            } else if ($scope.TRAN_TYPE_ID == '10' && (master.SDVNT_OID == '' || master.CMN_OID == '' || master.SKU_OID == '')) {
                ShowCustomToastr('warning', 'Please check and input valid information!!!');
                return;
            }

            var HeaderTokenPutPost = $scope.HeaderToken.post;
            var ModelsArray = [master];
            var apiRoute = baseUrl + 'saveupdate/';
            var DataMappingSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            DataMappingSaveUpdate.then(function (response) {
                if (response != "") {
                    if (response.resdata == '1') {
                        $scope.clear();
                        $scope.loadDataMappingList(0);
                        ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                    } else {
                        ShowCustomToastr('warning', 'Data already exists');
                    }
                }
                else if (response == "") {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                }
            },
                function (error) {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                });
        };

        // Delete master detail record
        //$scope.delete = function (dataModel) {
        //    $scope.cmnParam();
        //    objcmnParam.id = dataModel.USERROLEID;
        //    var ModelsArray = [objcmnParam];
        //    var apiRoute = baseUrl + 'delete/';
        //    var delDistributorTarget = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
        //    delDistributorTarget.then(function (response) {
        //        if (response.data.result != "") {
        //            $scope.loadDataMappingList(0);
        //            Command: toastr["success"](dataModel.DistributorTargetNo + " has been Deleted Successfully!!!!");
        //        }
        //        else {
        //            Command: toastr["warning"](dataModel.DistributorTargetNo + " not Deleted, Please Check and Try Again!");
        //        }
        //    },
        //        function (error) {
        //            Command: toastr["warning"](dataModel.DistributorTargetNo + " not Deleted, Please Check and Try Again!");
        //            console.log("Error: " + error);
        //        });
        //}

        // Clear info after sucessful transaction
        $scope.clear = function () {
            $scope.frmDataMapping.$setPristine();
            $scope.frmDataMapping.$setUntouched();

            $scope.COMPANY_OID = '';
            $("#COMPANY_OID_1").select2("data", { id: '', text: '--Select Company--' });
            $scope.SSTYP_OID = '';
            $("#SSTYP_OID_1").select2("data", { id: '', text: '--Select Sales Type--' });

            $scope.SDVNT_OID = '';
            $("#SDVNT_OID_2").select2("data", { id: '', text: '--Select Sales Group--' });
            $("#SDVNT_OID_8").select2("data", { id: '', text: '--Select Sales Group--' });
            $scope.SPROG_OID = '';
            $("#SPROG_OID_2").select2("data", { id: '', text: '--Select Product Group--' });


            $scope.USER_OID = '';
            $("#USER_OID_3").select2("data", { id: '', text: '--Select Staff--' });
            $("#USER_OID_4").select2("data", { id: '', text: '--Select Staff--' });
            $("#USER_OID_5").select2("data", { id: '', text: '--Select Staff--' });
            $("#USER_OID_7").select2("data", { id: '', text: '--Select Staff--' });
            $("#USER_OID_8").select2("data", { id: '', text: '--Select Staff--' });
            $scope.GROUP_OID = '';
            $("#GROUP_OID_3").select2("data", { id: '', text: '--Select Brand Group--' });
            $("#GROUP_OID_4").select2("data", { id: '', text: '--Select Brand Group--' });
            $scope.BRAND_OID = '';
            $("#BRAND_OID_3").select2("data", { id: '', text: '--Select Brand--' });
            $("#BRAND_OID_4").select2("data", { id: '', text: '--Select Brand--' });
            $scope.skuList = [];
            $scope.SKU_OID = '';
            $("#SKU_OID_4").select2("data", { id: '', text: '--Select Product--' });
            $("#SKU_OID_10").select2("data", { id: '', text: '--Select Product--' });


            $scope.ROLE_OID = '';
            $("#ROLE_OID_5").select2("data", { id: '', text: '--Select Role--' });
            $scope.NTNAL_OID = '';
            $("#NTNAL_OID_5").select2("data", { id: '', text: '--Select National--' });

            $scope.CMN_OID = '';
            $("#MODULEID_7").select2("data", { id: '', text: '--Select Module--' });

            $scope.IS_ENABLE = false;
            $scope.IS_UPDATE = false;

            $scope.USER_FULLNAME = '';
            $scope.USER_PASS = '';
        };
        //**********---- End Clear records ----***************//


        $scope.printDiv = function printElem(print) {
            var content = document.getElementById(print).innerHTML;
            var mywindow = window.open('', 'Print', 'height=1000,width=2000');

            mywindow.document.write('<html><head><title></title>');
            mywindow.document.write('</head><body >');
            mywindow.document.write(content);
            mywindow.document.write('</body> <br/><br/>');
            mywindow.document.write('<footer> <font color="green">Powered By:</font><font color="blue"> onAir International Ltd. </font></footer>');
            mywindow.document.write('</html>');


            mywindow.document.close();
            mywindow.focus();
            mywindow.print();
            mywindow.close();
            return true;
        };

        $scope.exportToExcel = function () {
            var blob = new Blob([document.getElementById('print').innerHTML], {
                type: 'application/vnd.ms-excel;charset=charset=utf-8'
            });
            var url = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.download = 'DailyFeesCollection.xls';
            a.href = url;
            a.textContent = 'DailyFeesCollection.xls';
            a.click();
            //saveAs(blob, 'DailyFeesCollection.xls');
        };
    }
]);