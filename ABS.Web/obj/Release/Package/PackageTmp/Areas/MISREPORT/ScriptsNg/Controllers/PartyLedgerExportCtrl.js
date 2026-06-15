app.controller('PartyLedgerExportCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
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
        $scope.PageTitle = 'Consumer DO Print';

        $scope.DeliveryStatementPageTitle = 'Consumer DO Print';

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
        // Get permission as per user
        // $scope.loadUserWiseReportPermission = function (isPaging) {

        //function loadUserWiseReportPermission() {
        //    $scope.cmnParam();
        //    var loggeduser = objcmnParam.loggeduser;
        //    var userrole = '';

        //    var url = 'https://cssap.citygroupbd.com/acmsapi/api/userrole/getconsumersalesuserroles/' + '' + loggeduser + '';


        //    $http.get(url).then(function (response) {

        //        if (response.data.length > 0) {
        //            userrole = response.data[0].roleID;

        //            if (userrole == 16) { // 16 is the sales top role who have access to get any distributor data
        //                $scope.loadDistributorAll();
        //            }
        //            var apiRoute = sysCommonUrl + 'GetPermissionForReport/';
        //            var listPermission = crudService.getMultipleParameter(apiRoute, loggeduser, userrole, page, pageSize, isPaging, $scope.HeaderToken.get);
        //            listPermission.then(function (response) {
        //                $scope.listPermission = response.data;
        //                // load national data
        //                var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
        //                var mode = '1';
        //                var trackNo = '0';
        //                var listNational = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
        //                listNational.then(function (response) {
        //                    $scope.listNational = response.data;
        //                },
        //                    function (error) {
        //                        console.log("Error: " + error);
        //                    });
        //                debugger
        //                if ($scope.listPermission[0].ENABLENATIONAL == '1') {
        //                    $scope.IsDisableNational = false;
        //                    $scope.IsReadOnlyNational = false;
        //                    $scope.IsDisableCompany = false;
        //                }
        //                else {
        //                    $scope.IsDisableNational = true;
        //                    $scope.IsReadOnlyNational = true;

        //                    if ($scope.listPermission[0].NATIONALID != '' && $scope.listPermission[0].NATIONALID != null) {
        //                        $scope.ngModelNational = $scope.listPermission[0].NATIONALID;
        //                        $("#ddlNational").select2("data", { id: $scope.listPermission[0].NATIONALID, text: $scope.listPermission[0].NATIONALNAME });
        //                    }

        //                    if ($scope.listPermission[1].UNDERWHICHNATIONALID != '' && $scope.listPermission[1].UNDERWHICHNATIONALID != null) {
        //                        $scope.ngModelNational = $scope.listPermission[1].UNDERWHICHNATIONALID;
        //                        $("#ddlNational").select2("data", { id: $scope.listPermission[1].UNDERWHICHNATIONALID, text: $scope.listPermission[1].UNDERWHICHNATIONALNAME });
        //                    }
        //                    $scope.loadAreaWiseDistributorRecords();
        //                }

        //                if ($scope.listPermission[0].ENABLEDIVISION == '1') {
        //                    $scope.IsDisableDivision = false;
        //                    $scope.IsReadOnlyDivision = false;
        //                }
        //                else {
        //                    $scope.IsDisableDivision = true;
        //                    $scope.IsReadOnlyDivision = true;

        //                    if ($scope.listPermission[0].DIVISIONID != '' && $scope.listPermission[0].DIVISIONID != null) {
        //                        $scope.ngModelDivision = $scope.listPermission[0].DIVISIONID;
        //                        $("#ddlDivision").select2("data", { id: $scope.listPermission[0].DIVISIONID, text: $scope.listPermission[0].DIVISIONNAME });
        //                    }

        //                    if ($scope.listPermission[1].UNDERWHICHDIVISIONID != '' && $scope.listPermission[1].UNDERWHICHDIVISIONID != null) {
        //                        $scope.ngModelDivision = $scope.listPermission[1].UNDERWHICHDIVISIONID;
        //                        $("#ddlDivision").select2("data", { id: $scope.listPermission[1].UNDERWHICHDIVISIONID, text: $scope.listPermission[1].UNDERWHICHDIVISIONNAME });
        //                    }
        //                    // $scope.loadAreaWiseDistributorRecords();
        //                    $scope.loadRegionRecords();
        //                }

        //                if ($scope.listPermission[0].ENABLEREGION == '1') {
        //                    $scope.IsDisableRegion = false;
        //                    $scope.IsReadOnlyRegion = false;
        //                }
        //                else {
        //                    $scope.IsDisableRegion = true;
        //                    $scope.IsReadOnlyRegion = true;
        //                    if ($scope.listPermission[0].REGIONID != '' && $scope.listPermission[0].REGIONID != null) {
        //                        $scope.ngModelRegion = $scope.listPermission[0].REGIONID;
        //                        $("#ddlRegion").select2("data", { id: $scope.listPermission[0].REGIONID, text: $scope.listPermission[0].REGIONNAME });
        //                    }
        //                    if ($scope.listPermission[1].UNDERWHICHREGIONID != '' && $scope.listPermission[1].UNDERWHICHREGIONID != null) {
        //                        $scope.ngModelRegion = $scope.listPermission[1].UNDERWHICHREGIONID;
        //                        $("#ddlRegion").select2("data", { id: $scope.listPermission[1].UNDERWHICHREGIONID, text: $scope.listPermission[1].UNDERWHICHREGIONNAME });
        //                    }
        //                    // $scope.loadAreaWiseDistributorRecords();
        //                    $scope.loadZoneRecords();
        //                }

        //                if ($scope.listPermission[0].ENABLEZONE == '1') {
        //                    $scope.IsDisableZone = false;
        //                    $scope.IsReadOnlyZone = false;
        //                }
        //                else {
        //                    $scope.IsDisableZone = true;
        //                    $scope.IsReadOnlyZone = true;
        //                    if ($scope.listPermission[0].ZONEID != '' && $scope.listPermission[0].ZONEID != null) {
        //                        $scope.ngModelZone = $scope.listPermission[0].ZONEID;
        //                        $("#ddlZone").select2("data", { id: $scope.listPermission[0].ZONEID, text: $scope.listPermission[0].ZONENAME });
        //                    }
        //                    if ($scope.listPermission[1].UNDERWHICHZONEID != '' && $scope.listPermission[1].UNDERWHICHZONEID != null) {
        //                        $scope.ngModelZone = $scope.listPermission[1].UNDERWHICHZONEID;
        //                        $("#ddlZone").select2("data", { id: $scope.listPermission[1].UNDERWHICHZONEID, text: $scope.listPermission[1].UNDERWHICHZONENAME });
        //                    }
        //                    // $scope.loadAreaWiseDistributorRecords();
        //                }

        //                if ($scope.listPermission[0].ENABLEDISTRIBUTOR == '1') {
        //                    $scope.IsDisableDistributor = false;
        //                    $scope.IsReadOnlyDistributor = false;
        //                }
        //                else {
        //                    $scope.IsDisableDistributor = true;
        //                    $scope.IsReadOnlyDistributor = true;
        //                    if ($scope.listPermission[0].DISTRIBUTORID != '' && $scope.listPermission[0].DISTRIBUTORID != null) {
        //                        $scope.ngModelDistributor = $scope.listPermission[0].DISTRIBUTORID;
        //                        $("#ddlDistributor").select2("data", { id: $scope.listPermission[0].DISTRIBUTORID, text: $scope.listPermission[0].DISTRIBUTORNAME });
        //                    }
        //                    if ($scope.listPermission[1].UNDERWHICHDISTRIBUTORID != '' && $scope.listPermission[1].UNDERWHICHDISTRIBUTORID != null) {
        //                        $scope.ngModelDistributor = $scope.listPermission[1].UNDERWHICHDISTRIBUTORID;
        //                        $("#ddlDistributor").select2("data", { id: $scope.listPermission[1].UNDERWHICHDISTRIBUTORID, text: $scope.listPermission[1].UNDERWHICHDISTRIBUTORNAME });
        //                    }
        //                }
        //            },
        //                function (error) {
        //                    console.log("Error: " + error);
        //                });
        //        }
        //        else {

        //            debugger
        //            Command: toastr["warning"]("You have no permission to view this report");
        //            $scope.IsShowReportButtonDisabled = true;
        //        }
        //    });
        //}
        //loadUserWiseReportPermission(0);



        //function loadUserWiseReportPermission() {
        //    debugger;
        //    $scope.cmnParam();
        //    var userrole = '';
        //    var param = { loggeduser: objcmnParam.loggeduser };
        //    var apiRoute = sysCommonUrl + 'getUserRoleID/';

        //    var listModel = crudService.getListByParam(apiRoute, param);
        //    listModel.then(function (response) {
        //        userrole = response.data;

        //        if (userrole.length > 0) {
        //            if (userrole == '16') { // 16 is the sales top role who have access to get any distributor data
        //                $scope.loadDistributorAll();
        //            }
        //            var apiRoute = sysCommonUrl + 'GetPermissionForReport/';
        //            var listPermission = crudService.getMultipleParameter(apiRoute, objcmnParam.loggeduser, userrole, page, pageSize, isPaging, $scope.HeaderToken.get);
        //            listPermission.then(function (response) {
        //                $scope.listPermission = response.data;
        //                // load national data
        //                var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
        //                var mode = '1';
        //                var trackNo = '0';
        //                var listNational = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
        //                listNational.then(function (response) {
        //                    $scope.listNational = response.data;
        //                },
        //                    function (error) {
        //                        console.log("Error: " + error);
        //                    });
        //                debugger
        //                if ($scope.listPermission[0].ENABLENATIONAL == '1') {
        //                    $scope.IsDisableNational = false;
        //                    $scope.IsReadOnlyNational = false;
        //                    $scope.IsDisableCompany = false;
        //                }
        //                else {
        //                    $scope.IsDisableNational = true;
        //                    $scope.IsReadOnlyNational = true;
        //                    $scope.IsDisableCompany = true;

        //                    if ($scope.listPermission[0].NATIONALID != '' && $scope.listPermission[0].NATIONALID != null) {
        //                        $scope.ngModelNational = $scope.listPermission[0].NATIONALID;
        //                        $("#ddlNational").select2("data", { id: $scope.listPermission[0].NATIONALID, text: $scope.listPermission[0].NATIONALNAME });
        //                    }

        //                    if ($scope.listPermission[1].UNDERWHICHNATIONALID != '' && $scope.listPermission[1].UNDERWHICHNATIONALID != null) {
        //                        $scope.ngModelNational = $scope.listPermission[1].UNDERWHICHNATIONALID;
        //                        $("#ddlNational").select2("data", { id: $scope.listPermission[1].UNDERWHICHNATIONALID, text: $scope.listPermission[1].UNDERWHICHNATIONALNAME });
        //                    }
        //                    $scope.loadAreaWiseDistributorRecords();
        //                }

        //                if ($scope.listPermission[0].ENABLEDISTRIBUTOR == '1') {
        //                    $scope.IsDisableDistributor = false;
        //                    $scope.IsReadOnlyDistributor = false;
        //                }
        //                else {
        //                    $scope.IsDisableDistributor = true;
        //                    $scope.IsReadOnlyDistributor = true;
        //                    if ($scope.listPermission[0].DISTRIBUTORID != '' && $scope.listPermission[0].DISTRIBUTORID != null) {
        //                        $scope.ngModelDistributor = $scope.listPermission[0].DISTRIBUTORID;
        //                        $("#ddlDistributor").select2("data", { id: $scope.listPermission[0].DISTRIBUTORID, text: $scope.listPermission[0].DISTRIBUTORNAME });
        //                    }
        //                    if ($scope.listPermission[1].UNDERWHICHDISTRIBUTORID != '' && $scope.listPermission[1].UNDERWHICHDISTRIBUTORID != null) {
        //                        $scope.ngModelDistributor = $scope.listPermission[1].UNDERWHICHDISTRIBUTORID;
        //                        $("#ddlDistributor").select2("data", { id: $scope.listPermission[1].UNDERWHICHDISTRIBUTORID, text: $scope.listPermission[1].UNDERWHICHDISTRIBUTORNAME });
        //                    }
        //                }

        //                debugger
        //                ////// pushing distributors as per area wise permission
        //                $scope.loadAreaWiseDistributorRecords();
        //            },
        //                function (error) {
        //                    console.log("Error: " + error);
        //                });
        //        }
        //        else {
        //            debugger
        //            Command: toastr["warning"]("You have no permission to view this report");
        //            $scope.IsShowReportButtonDisabled = true;
        //        }

        //    }, function (error) {
        //        alert("failed");
        //    });
        //}
        //loadUserWiseReportPermission(0);




        /////// Get all Locations 
        //function loadLocationRecords(isPaging) {

        //    var apiRoute = sysCommonUrl + 'GetLocation/';
        //    var listLocation = crudService.getItemWithcmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
        //    listLocation.then(function (response) {
        //        $scope.listLocation = response.data.objLocationList;
        //    },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //}
        //loadLocationRecords(0);

        // Get All Company list
        //$scope.GetAllCompany = function () {
        //    mCUID = $scope.ngModelCompany;
        //    $http.get(rootUrl + "/SystemCommon/api/SystemCommonDDL/getAllCompanyList").then(function (d) {
        //        $scope.listCompany = d.data;
        //    }, function (error) {
        //        alert("Failed");
        //    });
        //};
        //$scope.GetAllCompany();

        $scope.GetAllCompany = function () {
            var apiRoute = sysCommonUrl + 'GetExportCompanyList/';
            var listModel = crudService.getAllList(apiRoute);
            listModel.then(function (response) {
                $scope.listCompany = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.GetAllCompany();

        // Get All institutional distributor 
        //$scope.loadDistributorAll = function () {
        //    $scope.listDistributor = [];
        //    $scope.ngModelDistributor = '';
        //    $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

        //    var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
        //    var mode = '9';  // GET ALL Institutional DISTRIBUTOR
        //    var trackNo = 0;

        //    var listDistributor = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
        //    listDistributor.then(function (response) {
        //        $scope.listDistributor = response.data;
        //    },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //};
        //$scope.loadDistributorAll();


        // Get all distributor
        $scope.loadDistributorAll = function () {
            $scope.listAllDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Customer--' });

            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '13';
            var trackNo = 0;
            var listAllDistributor = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listAllDistributor.then(function (response) {
                $scope.listAllDistributor = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        $scope.loadDistributorAll();

        // Get area-wise distributor
        //$scope.loadAreaWiseDistributorRecords = function () {
        //    debugger
        //    $scope.listDistributor = [];
        //    $scope.ngModelDistributor = '';
        //    $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

        //    var nationalId = $scope.ngModelNational == '' || undefined ? 0 : $scope.ngModelNational;
        //    var divisionId = 0;
        //    var regionId = 0;
        //    var zoneId = 0;

        //    var apiRoute = sysCommonUrl + 'GetAreaWiseDistributor/';
        //    var listAreaDistributor = crudService.getAreaWiseDistributor(apiRoute, nationalId, divisionId, regionId, zoneId, page, pageSize, isPaging, $scope.HeaderToken.get);
        //    listAreaDistributor.then(function (response) {
        //        $scope.listAreaDistributor = response.data;
        //    },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //}

        $scope.ShowConsumerDOPrint = function () {

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;

            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + startDate + ',' + endDate;
            var reportPath = repBaseUrl + 'ConsumerDOPrint/ConsumerDOPrint.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
        };


        $scope.ShowSalesVsCollectionReport = function () {
            var reporttype = $scope.ReportType;
            if (reporttype == '' || reporttype == undefined || reporttype == null) {
                Command: toastr["warning"]("Please select report type first !");
                return;
            }
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + startDate + ',' + endDate + ',' + reporttype;
            var reportPath = repBaseUrl + 'SaleVsCollection/SalesCollection.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
        };

        $scope.clearSalesVsCollection = function () {

            $scope.ngModelLocation = '';
            $("#ddlLocation").select2("data", { id: '', text: '--Select Location--' });

            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();
        };



        $scope.ShowPartyLedger = function (reporttype) {

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            // $scope.ngModelLocation

            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelDistributor + ',' + startDate + ',' + endDate + ',' + reporttype;

            var reportPath = repBaseUrl + 'PartyLedger/PartyLedger.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowPartyLedgerExport = function (reporttype) {

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            // $scope.ngModelLocation

            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelDistributor + ',' + startDate + ',' + endDate + ',' + reporttype;

            var reportPath = repBaseUrl + 'PartyLedgerExport/PartyLedgerExport.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowDOVSDeliveryTime = function (type) {
            debugger
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            var reportType = type;
            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelDistributor + ',' + startDate + ',' + endDate + ',' + reportType;

            var reportPath = repBaseUrl + 'DOvsDelivery/DOvsDeliveryView.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };
        $scope.ReqVSDOReport = function () {
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + startDate + ',' + endDate;
            var reportPath = repBaseUrl + 'RequisitionVsDO/RequisitionVsDO.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.EmployeeProductConsumption = function () {
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            //var queryString = $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + startDate + ',' + endDate;
            var queryString = startDate + ',' + endDate;
            var reportPath = repBaseUrl + 'EmployeeProductConsumption/EmployeeProductConsumption.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ProductGroupWiseSale = function () {
            var reporttype = $scope.ReportType;
            if (reporttype == '' || reporttype == undefined || reporttype == null) {
                Command: toastr["warning"]("Please select report type first !");
                return;
            }
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;

            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + startDate + ',' + endDate + ',' + reporttype;

            var reportPath = repBaseUrl + 'ProductGroupWiseSale/ProductGroupWiseSale.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.SalesComparisonAnalysis = function () {
            var reporttype = $scope.ReportType;
            if (reporttype == '' || reporttype == undefined || reporttype == null) {
                Command: toastr["warning"]("Please select report type first !");
                return;
            }
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            var queryString = startDate + ',' + endDate + ',' + reporttype;
            var reportPath = repBaseUrl + 'SalesComparison/SalesComparisonByNH.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.SalesSummaryBangla = function () {

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            var reporttype = $scope.ReportType;

            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + startDate + ',' + endDate + ',' + reporttype;

            var reportPath = repBaseUrl + 'SalesSummaryBangla/BanglaSalesSummary.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.clearConsumerDOPrint = function () {
            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;


            //$scope.listLocation = [];
            $scope.ngModelLocation = '';
            $("#ddlLocation").select2("data", { id: '', text: '--Select Location--' });



            //$scope.listCompany = [];
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();
        };
        $scope.clearReqVSDO = function () {
            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;

            $scope.ngModelLocation = '';
            $("#ddlLocation").select2("data", { id: '', text: '--Select Location--' });

            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();
        };
        $scope.clearPartyLedger = function () {
            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;


            //$scope.listLocation = [];
            $scope.ngModelLocation = '';
            $("#ddlLocation").select2("data", { id: '', text: '--Select Location--' });



            //$scope.listCompany = [];
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            //$scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();
        };
        $scope.clearDOVSDelivery = function () {
            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;

            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();
        };

        $scope.clearProductGroupWiseSale = function () {
            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;

            //$scope.listLocation = [];
            $scope.ngModelLocation = '';
            $("#ddlLocation").select2("data", { id: '', text: '--Select Location--' });

            //$scope.listCompany = [];
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

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