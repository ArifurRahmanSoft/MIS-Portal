app.controller('UndeliveredStatementLineCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, $http, $window, crudService, conversion, $filter, $localStorage, uiGridConstants) {

        //**************************************************Start Vairiable Initialize**************************************************
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        var repBaseUrl = '/Areas/MISREPORT/Reports/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;

        $scope.StartDate = conversion.NowDateCustom();
        $scope.EndDate = conversion.NowDateCustom();

        $scope.btnPISaveText = "Save";
        $scope.btnPIShowText = "Show List";

        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmUndeliveredStatementLine'; DelFunc = 'DeleteDistributorTargetMasterDetail'; DelMsg = 'DistributorTargetNo'; EditFunc = 'loadPIMasterDetailsByActivePI';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        

        //****************************************************************************Start Mendatory Function*******************************************************************
        $scope.userMenuModePermissionList = [];
        $scope.ReportType = '';
        $scope.GetPermissionType = function () {
            debugger;
            $scope.cmnParam();
            var param = [{ loggeduser: objcmnParam.loggeduser, menuId: $localStorage.currentMenuID }];
            var apiRoute = sysCommonUrl + 'getUserMenuModePermission/';

            var listModel = crudService.postMultipleModel(apiRoute, param, $scope.HeaderToken.get);
            listModel.then(function (response) {
                var dataList = response.data.modePermission;
                debugger;
                var listData = dataList.filter(x => x.COLVALUE == 1)
                if (listData.length > 0) {

                    angular.forEach(listData, function (item) {
                        item.COLID = item.COLNNAME == 'ISLINE' ? 'LineWise' : item.COLNNAME == 'ISCOMPANY' ? 'CompanyWise' : item.COLNNAME == 'ISLOCATION' ? 'LocationWise' : '';
                        item.COLDISPLAY = item.COLNNAME == 'ISLINE' ? 'Line Wise' : item.COLNNAME == 'ISCOMPANY' ? 'Company Wise' : item.COLNNAME == 'ISLOCATION' ? 'Location Wise' : '';
                    });

                    $scope.userMenuModePermissionList = listData;
                    $scope.setPermissionMode($scope.userMenuModePermissionList[0].COLID);

                    debugger;
                    $scope.loadPermissionTypeWiseList();
                }
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.GetPermissionType();

        $scope.setPermissionMode = function (itemname) {
            debugger;
            $scope.ReportType = itemname;
            angular.forEach($scope.userMenuModePermissionList, function (item) {
                conversion.UnChecked(item.COLID);
            });

            conversion.Checked(itemname);

            $scope.loadPermissionTypeWiseList();
        }

        $scope.loadPermissionTypeWiseList = function () {
            $scope.listSalesLine = [];
            $scope.salesLineId = '';
            $("#ddlLine").select2("data", { id: '', text: '--Select Sales Line--' });

            $scope.listSalesCompany = [];
            $scope.salesCompanyId = '';
            $("#salesCompanyId_Line").select2("data", { id: '', text: '--Select Company--' });
            $("#salesCompanyId_Other").select2("data", { id: '', text: '--Select Company--' });

            $scope.listLocation = [];
            $scope.locationId = '';
            $("#locationId").select2("data", { id: '', text: '--Select Location--' });

            $scope.listSalesCustomer = [];
            $scope.salesCustomerId = '';
            $("#salesCustomerId").select2("data", { id: '', text: '--Select customer--' });

            if ($scope.ReportType == 'LineWise') {
                $scope.GetAllSalesLine('SALES_LINE');
            } else if ($scope.ReportType == 'CompanyWise') {
                $scope.UserWiseCompanyList();
                $scope.GetFilteredCustomer();
            } else if ($scope.ReportType == 'LocationWise') {
                $scope.UserWiseCompanyList();
                $scope.GetFilteredLocation();
                $scope.GetFilteredCustomer();
            }
        }
        //*****************************************************************************End Mendatory Function**************************************************************

        //********************************************************************************Start Line Wise******************************************************************
        $scope.salesLineId = '';
        $scope.listSalesLine = [];
        $scope.salesCompanyId = '';
        $scope.listSalesCompany = [];
        $scope.salesCustomerId = '';
        $scope.listSalesCustomer = [];
        $scope.GetAllSalesLine = function (call_name) {
            debugger;
            $scope.cmnParam();
            var param = [{ fieldNameOne: call_name, fieldNameTwo: objcmnParam.loggeduser }];
            var apiRoute = sysCommonUrl + 'GetAllSalesLine/';
            var listModel = crudService.postMultipleModel(apiRoute, param, $scope.HeaderToken.get);
            listModel.then(function (response) {
                debugger;
                $scope.listSalesLine = response.data.mCompanyList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.GetAllSalesCompany = function (call_name) {
            $scope.listSalesCompany = [];
            $scope.salesCompanyId = '';
            $("#salesCompanyId_Line").select2("data", { id: '', text: '--Select Company--' });
            debugger;
            var param = [{ fieldNameOne: call_name, fieldNameTwo: $scope.salesLineId }];
            var apiRoute = sysCommonUrl + 'GetAllSalesLine/';
            var listModel = crudService.postMultipleModel(apiRoute, param, $scope.HeaderToken.get);
            listModel.then(function (response) {
                debugger
                $scope.listSalesCompany = response.data.mCompanyList;

                debugger;
                if ($scope.listSalesCompany.length > 1) {
                    $scope.salesCompanyId = $scope.listSalesCompany[1].OID;
                    $("#salesCompanyId_Line").select2("data", { id: $scope.listSalesCompany[1].OID, text: $scope.listSalesCompany[1].SCOMP_NAME });
                    $scope.GetAllSalesCustomer();
                }
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.GetAllSalesCustomer = function () {
            debugger;
            $scope.listSalesCustomer = [];
            $scope.salesCustomerId = '';
            $("#salesCustomerId").select2("data", { id: '', text: '--Select customer--' });
            var lineModel = $scope.listSalesLine.filter(x => x.OID == $scope.salesLineId)[0];
            if (lineModel != undefined && $scope.salesCompanyId != undefined) {

                var call_name = $scope.salesCompanyId == 'SCOMPxxxxxxxxxxxxx32' || $scope.salesCompanyId == 'SCOMPxxxxxxxxxxxxx94' ? 'CUSTOMER_MHASAN_FEED' : 'LINE_CUSTOMER';

                var param = [{ fieldNameOne: call_name, fieldNameTwo: lineModel.SDVNT_SCTYP }];
                var apiRoute = sysCommonUrl + 'GetAllSalesLine/';
                var listModel = crudService.postMultipleModel(apiRoute, param, $scope.HeaderToken.get);
                listModel.then(function (response) {
                    debugger;
                    $scope.listSalesCustomer = response.data.mCompanyList;
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
        }
        //********************************************************************************End Line Wise********************************************************************

        //*******************************************************************************Start Company Wise****************************************************************
        $scope.UserWiseCompanyList = function () {
            $scope.cmnParam();
            var param = { loggeduser: objcmnParam.loggeduser };
            var apiRoute = sysCommonUrl + 'getUserWiseCompanyList/';
            var listModel = crudService.getListByParam(apiRoute, param);
            listModel.then(function (response) {
                $scope.listSalesCompany = [];
                var itemlist = response.data;
                angular.forEach(itemlist, function (item) {
                    $scope.listSalesCompany.push({
                        OID: item.SCOMP_OID,
                        SCOMP_TEXT: item.SCOMP_TEXT,
                        SCOMP_NAME: item.SCOMP_NAME
                    });
                });

                debugger;
                if ($scope.listSalesCompany.length > 0) {
                    $scope.salesCompanyId = $scope.listSalesCompany[0].OID;
                    $("#salesCompanyId_Other").select2("data", { id: $scope.listSalesCompany[0].OID, text: $scope.listSalesCompany[0].SCOMP_NAME });
                }
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.GetFilteredCustomer = function () {
            debugger;
            $scope.listSalesCustomer = [];
            $scope.salesCustomerId = '';
            $("#salesCustomerId").select2("data", { id: '', text: '--Select customer--' });
            //if ($scope.salesCompanyId != undefined) {
            var call_name = 'SSTYP_03_09';
            var fieldTwo = 'SSTYPxxxxxxxxxxxx03,SSTYPxxxxxxxxxxxx09';
            var param = [{ fieldNameOne: call_name, fieldNameTwo: fieldTwo }];
            var apiRoute = sysCommonUrl + 'GetAllSalesLine/';
            var listModel = crudService.postMultipleModel(apiRoute, param, $scope.HeaderToken.get);
            listModel.then(function (response) {
                debugger;
                $scope.listSalesCustomer = response.data.mCompanyList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
            //}
        }
        //********************************************************************************End Company Wise*****************************************************************     

        //*******************************************************************************Start Location Wise***************************************************************  
        $scope.GetFilteredLocation = function () {
            debugger;
            $scope.listLocation = [];
            $scope.locationId = '';
            $("#locationId").select2("data", { id: '', text: '--Select Location--' });
            $scope.cmnParam();
            var loggeduser = objcmnParam.loggeduser;
            var inputString = 'SGLOCxxxxxxxxxxxxx01,SGLOCxxxxxxxxxxxxx03,SGLOCxxxxxxxxxxxxx04,SGLOCxxxxxxxxxxxxx10,SGLOCxxxxxxxxxxxxx11,SGLOCxxxxxxxxxxxxx19,SGLOCxxxxxxxxxxxxx25,SGLOCxxxxxxxxxxxxx12,SGLOCxxxxxxxxxxxxx13,SGLOCxxxxxxxxxxxxx15,SGLOCxxxxxxxxxxxxx21,SGLOCxxxxxxxxxxxxx22,SGLOCxxxxxxxxxxxxx30,SGLOCxxxxxxxxxxxxx33,SGLOCxxxxxxxxxxxxx34';
            var param = [{ loggeduser: loggeduser, InputString: inputString }];
            var apiRoute = sysCommonUrl + 'GetAllFilteredLocation/';
            var listModel = crudService.postMultipleModel(apiRoute, param, $scope.HeaderToken.get);
            listModel.then(function (response) {
                debugger;
                $scope.listLocation = response.data.locationList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        //********************************************************************************End Location Wise****************************************************************

        //*****************************************************************************Start Date Restriction**************************************************************
        //Date can not be less then 01/11/2023 --Announced by Kamrul Bashar Bhuiyan
        $scope.dtrstrct = new Date(conversion.getStringToDate('01/11/2023'));
        $scope.restrictedDate = function (pdate) {
            debugger;
            var esdt = $scope[pdate];
            var fdt = new Date(conversion.getStringToDate(esdt));
            if (fdt < $scope.dtrstrct) {
                setTimeout(() => {
                    $scope.$apply(function () {
                        debugger;
                        $scope[pdate] = '01/11/2023';
                    });
                });

                ShowCustomToastr('warning', "Date can not be less then '01/11/2023' !!!");
            }
        }
        //******************************************************************************End Date Restriction***************************************************************

        //******************************************************************************Start Common Dropdown**************************************************************
        // Get all Brand
        $scope.brandId = '';
        $scope.brandList = [];
        $scope.loadAllBrandList = function () {
            debugger;
            var cmnParam = [{ id: 0 }];
            $scope.brandId = '';
            $scope.brandList = [];
            $("#brandId").select2("data", { id: '', text: '--Select Brand--' });
            var apiRoute = sysCommonUrl + 'GetAllBrand/';
            var brndGroupRecords = crudService.postMultipleModel(apiRoute, cmnParam, $scope.HeaderToken.get);
            brndGroupRecords.then(function (response) {
                debugger;
                $scope.brandList = response.data.brandList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadAllBrandList();
        // Get all Brand
        // Get all product by brand
        $scope.productId = '';
        $scope.productList = [];
        $scope.loadProductsByBrand = function () {
            var cmnParam = [{ strId: $scope.brandId }];
            $scope.productList = [];
            $scope.ProductId = '';
            $("#productId").select2("data", { id: '', text: '--Select Product--' });
            var apiRoute = sysCommonUrl + 'GetAllProductByBrand/';
            var prod = crudService.postMultipleModel(apiRoute, cmnParam, $scope.HeaderToken.get);
            prod.then(function (response) {
                $scope.productList = response.data.skuList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        // Get all product by brand

        //********************************************************************************End Common Dropdown**************************************************************

        $scope.ReportType0 = 'details';
        $scope.ReportType1 = 'Requisition';
        $scope.ShowUDSalesStatementLineReport = function () {
            debugger;
            var salesLine = $scope.salesLineId == undefined ? '' : $scope.salesLineId;
            var salesCompany = $scope.salesCompanyId == undefined ? '' : $scope.salesCompanyId;
            var locationId = $scope.locationId == undefined ? '' : $scope.locationId;
            var brandId = $scope.brandId == undefined ? '' : $scope.brandId;
            var productId = $scope.productId == undefined ? '' : $scope.productId;
            var salesCustomerId = $scope.salesCustomerId == undefined ? '' : $scope.salesCustomerId;

            var queryString = $scope.StartDate + ',' + $scope.EndDate + ',' + salesLine + ',' + salesCompany + ',' + locationId + ','
                + brandId + ',' + productId + ',' + salesCustomerId + ',' + $scope.ReportType + ',' + $scope.ReportType0 + ',' + $scope.ReportType1;

            var reportPath = repBaseUrl + 'UndeliveredStatementLineWise/UndeliveredStatementLineWise.aspx?queryString=' + queryString;

            conversion.reportOpen(reportPath);
        };

        $scope.clear = function () {
            $scope.listSalesLine = [];
            $scope.salesLineId = '';
            $("#ddlLine").select2("data", { id: '', text: '--Select Sales Line--' });

            $scope.listSalesCompany = [];
            $scope.salesCompanyId = '';
            $("#salesCompanyId_Line").select2("data", { id: '', text: '--Select Company--' });
            $("#salesCompanyId_Other").select2("data", { id: '', text: '--Select Company--' });

            $scope.listLocation = [];
            $scope.locationId = '';
            $("#locationId").select2("data", { id: '', text: '--Select Location--' });

            $scope.brandList = [];
            $scope.brandId = '';
            $("#brandId").select2("data", { id: '', text: '--Select Brand--' });

            $scope.productList = [];
            $scope.productId = '';
            $("#productId").select2("data", { id: '', text: '--Select Product--' });

            $scope.listSalesCustomer = [];
            $scope.salesCustomerId = '';
            $("#salesCustomerId").select2("data", { id: '', text: '--Select customer--' });

            $scope.ReportType = '';
            $scope.ReportType0 = 'details';
            $scope.ReportType1 = 'Requisition';

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();

            $scope.setPermissionMode($scope.userMenuModePermissionList[0].COLID);
            $scope.loadPermissionTypeWiseList();
        };
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}