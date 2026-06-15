app.controller('PartyLedgerLineCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
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
        $scope.IsbtSaveReviseShow = true;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;

        $scope.StartDate = conversion.NowDateCustom();
        $scope.EndDate = conversion.NowDateCustom();

        $scope.btnPISaveText = "Save";
        $scope.btnPIShowText = "Show List";

        $scope.HoldDataModel = '';

        $scope.showDtgrid = 0;
        $scope.listDistributor = [];
        $scope.ngModelDistributor = '';

        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;

        $scope.IsShowReportButtonDisabled = false;

        var commonQueryString = '';


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

        //****************************************************************************Start Mendatory Function*******************************************************************
        $scope.SalesType = 'Consumer';
        $scope.SalesTypeList = [{ TypeName: 'Consumer' }, { TypeName: 'Bulk' }];

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
                        item.COLDISPLAY = item.COLNNAME == 'ISLINE' ? 'Based on Realization (Line)' : item.COLNNAME == 'ISCOMPANY' ? 'Based on Realization (Company)' : item.COLNNAME == 'ISLOCATION' ? 'Based on Realization (Location)' : '';
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

        //$scope.GetPermissionType();

        $scope.setSalesType = function (itemname) {
            debugger;
            $scope.SalesType = itemname;
            angular.forEach($scope.SalesTypeList, function (item) {
                conversion.UnChecked(item.TypeName);
            });

            conversion.Checked(itemname);
            $scope.GetPermissionType();
            //$scope.loadPermissionTypeWiseList();
        }

        $scope.setSalesType('Consumer');

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

            $scope.listSalesCustomer = [];
            $scope.salesCustomerId = '';
            $("#salesCustomerId").select2("data", { id: '', text: '--Select customer--' });

            if ($scope.ReportType == 'LineWise') {
                $scope.GetAllSalesLine('SALES_LINE');
            } else if ($scope.ReportType == 'CompanyWise') {
                $scope.UserWiseCompanyList();
                $scope.GetFilteredCustomer();
            } else if ($scope.ReportType == 'LocationWise') {
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

        $scope.ChangedLine = function (call_name) {
            debugger;
            if ($scope.SalesType == 'Consumer') {
                $scope.GetAllSalesCompany(call_name);
            } else {
                $scope.UserWiseCompanyList();
                $scope.GetFilteredCustomer();
            }
        }

        $scope.ChangedCompany = function () {
            debugger;
            if ($scope.SalesType == 'Consumer' && $scope.ReportType == 'LineWise') {
                $scope.GetAllSalesCustomer();
            } else {
                $scope.GetFilteredCustomer();
            }
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

                var param = [{ fieldNameOne: call_name, fieldNameTদwo: lineModel.SDVNT_SCTYP }];
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
            var fieldTwo = $scope.SalesType == 'Consumer' ? 'SSTYPxxxxxxxxxxxx03,SSTYPxxxxxxxxxxxx09' : 'SSTYPxxxxxxxxxxxx03,SSTYPxxxxxxxxxxxx04,SSTYPxxxxxxxxxxxx09';
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

        //*****************************************************************************Start Date Restriction**************************************************************
        //Date can not be less then 01/11/2023 --Announced by Kamrul Bashar Bhuiyan
        $scope.dtrstrctStr = '';
        $scope.dtrstrct = new Date(conversion.getStringToDate('01/11/2023'));
        $scope.restrictedDate = function (pdate) {
            if ($scope.SalesType == 'Bulk' && $scope.salesLineId != 'SDVNTxxxxxxxxxxx1011') {
                $scope.dtrstrct = new Date(conversion.getStringToDate('01/05/2024'));
            }

            if ($scope.ReportType1 == 'CrateLedgerDetails' || $scope.ReportType1 == 'CrateLedgerSummary') {
                $scope.dtrstrct = new Date(conversion.getStringToDate('04/05/2024'));
            }



            debugger;
            var esdt = $scope[pdate];
            var fdt = new Date(conversion.getStringToDate(esdt));
            if (fdt < $scope.dtrstrct) {
                setTimeout(() => {
                    $scope.$apply(function () {
                        debugger;
                        $scope.dtrstrctStr = ('0' + $scope.dtrstrct.getDate()).slice(-2) + '/' + ('0' + ($scope.dtrstrct.getMonth() + 1)).slice(-2) + '/' + $scope.dtrstrct.getFullYear();
                        $scope[pdate] = $scope.dtrstrctStr;//conversion.getDateToString($scope.dtrstrct);//'01/11/2023';
                    });

                    ShowCustomToastr('warning', "Date can not be less then '" + $scope.dtrstrctStr + "' !!!");
                });
            }
        }
        //******************************************************************************End Date Restriction***************************************************************

        $scope.ReportType1 = 'Details';
        $scope.ShowPartyLedgerLineReport = function () {
            debugger;
            var reportType = $scope.ReportType;
            var reportType1 = $scope.ReportType1;
            //var reportType2 = $scope.ReportType2;

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;

            var salesLine = $scope.salesLineId == undefined ? '' : $scope.salesLineId;
            var salesCompany = $scope.salesCompanyId == undefined ? '' : $scope.salesCompanyId;
            var salesCustomerId = $scope.salesCustomerId == undefined ? '' : $scope.salesCustomerId;


            var queryString = startDate + ',' + endDate + ',' + salesLine + ','
                + salesCompany + ',' + salesCustomerId + ',' + reportType + ',' + reportType1 + ',' + $scope.SalesType;

            var reportPath = repBaseUrl + 'PartyLedgerLineWise/PartyLedgerLineWise.aspx?queryString=' + queryString;

            conversion.reportOpen(reportPath);
        };

        $scope.clear = function () {
            $scope.SalesType = 'Consumer';
            $scope.listSalesLine = [];
            $scope.salesLineId = '';
            $("#ddlLine").select2("data", { id: '', text: '--Select Sales Line--' });

            $scope.listSalesCompany = [];
            $scope.salesCompanyId = '';
            $("#salesCompanyId_Line").select2("data", { id: '', text: '--Select Company--' });
            $("#salesCompanyId_Other").select2("data", { id: '', text: '--Select Company--' });

            $scope.listSalesCustomer = [];
            $scope.salesCustomerId = '';
            $("#salesCustomerId").select2("data", { id: '', text: '--Select customer--' });

            $scope.ReportType = '';
            $scope.ReportType1 = 'Details';

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