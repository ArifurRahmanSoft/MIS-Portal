app.controller('showroomLedgerCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
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
        $scope.PageTitle = 'Brand Wise Sales Statement';
        $scope.showroomLedgerPageTitle = 'Showroom Sales Ledger';
  

        $scope.HoldDataModel = '';

        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;

        $scope.IsShowReportButtonDisabled = false;

        var commonQueryString = '';

        $scope.listNational = [];

        $scope.listPermission = [];
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





        /*function loadUserWiseReportPermission() {
            debugger;
            $scope.cmnParam();
            var userrole = '';            
            var param = { loggeduser: objcmnParam.loggeduser };
            var apiRoute = sysCommonUrl + 'getUserRoleID/';

            var listModel = crudService.getListByParam(apiRoute, param);
            listModel.then(function (response) {
                userrole = response.data;

                if (userrole.length > 0) {
                    if (userrole == '16') { // 16 is the sales top role who have access to get any distributor data
                        $scope.loadDistributorAll();
                    }
                    var apiRoute = sysCommonUrl + 'GetPermissionForReport/';
                    var listPermission = crudService.getMultipleParameter(apiRoute, objcmnParam.loggeduser, userrole, page, pageSize, isPaging, $scope.HeaderToken.get);
                    listPermission.then(function (response) {
                        $scope.listPermission = response.data;
                        // load national data
                        var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
                        var mode = '15';
                        var trackNo = objcmnParam.loggeduser;
                        var listNational = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
                        listNational.then(function (response) {
                            $scope.listNational = response.data;
                        },
                            function (error) {
                                console.log("Error: " + error);
                            });
                        debugger
                        if ($scope.listPermission[0].ENABLENATIONAL == '1') {
                            $scope.IsDisableNational = false;
                            $scope.IsReadOnlyNational = false;
                            $scope.IsDisableCompany = false;
                        }
                        else {
                            $scope.IsDisableNational = true;
                            $scope.IsReadOnlyNational = true;
                            $scope.IsDisableCompany = true;

                            if ($scope.listPermission[0].NATIONALID != '' && $scope.listPermission[0].NATIONALID != null) {
                                $scope.ngModelNational = $scope.listPermission[0].NATIONALID;
                                $("#ddlNational").select2("data", { id: $scope.listPermission[0].NATIONALID, text: $scope.listPermission[0].NATIONALNAME });
                            }

                            if ($scope.listPermission[1].UNDERWHICHNATIONALID != '' && $scope.listPermission[1].UNDERWHICHNATIONALID != null) {
                                $scope.ngModelNational = $scope.listPermission[1].UNDERWHICHNATIONALID;
                                $("#ddlNational").select2("data", { id: $scope.listPermission[1].UNDERWHICHNATIONALID, text: $scope.listPermission[1].UNDERWHICHNATIONALNAME });
                            }
                            $scope.loadDivisionRecords();
                            //  $scope.loadAreaWiseDistributorRecords();
                            $scope.loadProductCategory();
                        }

                        if ($scope.listPermission[0].ENABLEDIVISION == '1') {
                            $scope.IsDisableDivision = false;
                            $scope.IsReadOnlyDivision = false;
                        }
                        else {
                            $scope.IsDisableDivision = true;
                            $scope.IsReadOnlyDivision = true;

                            if ($scope.listPermission[0].DIVISIONID != '' && $scope.listPermission[0].DIVISIONID != null) {
                                $scope.ngModelDivision = $scope.listPermission[0].DIVISIONID;
                                $("#ddlDivision").select2("data", { id: $scope.listPermission[0].DIVISIONID, text: $scope.listPermission[0].DIVISIONNAME });
                            }

                            if ($scope.listPermission[1].UNDERWHICHDIVISIONID != '' && $scope.listPermission[1].UNDERWHICHDIVISIONID != null) {
                                $scope.ngModelDivision = $scope.listPermission[1].UNDERWHICHDIVISIONID;
                                $("#ddlDivision").select2("data", { id: $scope.listPermission[1].UNDERWHICHDIVISIONID, text: $scope.listPermission[1].UNDERWHICHDIVISIONNAME });
                            }
                            // $scope.loadAreaWiseDistributorRecords();
                            $scope.loadRegionRecords();
                        }

                        if ($scope.listPermission[0].ENABLEREGION == '1') {
                            $scope.IsDisableRegion = false;
                            $scope.IsReadOnlyRegion = false;
                        }
                        else {
                            $scope.IsDisableRegion = true;
                            $scope.IsReadOnlyRegion = true;
                            if ($scope.listPermission[0].REGIONID != '' && $scope.listPermission[0].REGIONID != null) {
                                $scope.ngModelRegion = $scope.listPermission[0].REGIONID;
                                $("#ddlRegion").select2("data", { id: $scope.listPermission[0].REGIONID, text: $scope.listPermission[0].REGIONNAME });
                            }
                            if ($scope.listPermission[1].UNDERWHICHREGIONID != '' && $scope.listPermission[1].UNDERWHICHREGIONID != null) {
                                $scope.ngModelRegion = $scope.listPermission[1].UNDERWHICHREGIONID;
                                $("#ddlRegion").select2("data", { id: $scope.listPermission[1].UNDERWHICHREGIONID, text: $scope.listPermission[1].UNDERWHICHREGIONNAME });
                            }
                            // $scope.loadAreaWiseDistributorRecords();
                            $scope.loadZoneRecords();
                        }

                        if ($scope.listPermission[0].ENABLEZONE == '1') {
                            $scope.IsDisableZone = false;
                            $scope.IsReadOnlyZone = false;
                        }
                        else {
                            $scope.IsDisableZone = true;
                            $scope.IsReadOnlyZone = true;
                            if ($scope.listPermission[0].ZONEID != '' && $scope.listPermission[0].ZONEID != null) {
                                $scope.ngModelZone = $scope.listPermission[0].ZONEID;
                                $("#ddlZone").select2("data", { id: $scope.listPermission[0].ZONEID, text: $scope.listPermission[0].ZONENAME });
                            }
                            if ($scope.listPermission[1].UNDERWHICHZONEID != '' && $scope.listPermission[1].UNDERWHICHZONEID != null) {
                                $scope.ngModelZone = $scope.listPermission[1].UNDERWHICHZONEID;
                                $("#ddlZone").select2("data", { id: $scope.listPermission[1].UNDERWHICHZONEID, text: $scope.listPermission[1].UNDERWHICHZONENAME });
                            }
                            // $scope.loadAreaWiseDistributorRecords();
                        }

                        if ($scope.listPermission[0].ENABLEDISTRIBUTOR == '1') {
                            $scope.IsDisableDistributor = false;
                            $scope.IsReadOnlyDistributor = false;
                        }
                        else {
                            $scope.IsDisableDistributor = true;
                            $scope.IsReadOnlyDistributor = true;
                            if ($scope.listPermission[0].DISTRIBUTORID != '' && $scope.listPermission[0].DISTRIBUTORID != null) {
                                $scope.ngModelDistributor = $scope.listPermission[0].DISTRIBUTORID;
                                $("#ddlDistributor").select2("data", { id: $scope.listPermission[0].DISTRIBUTORID, text: $scope.listPermission[0].DISTRIBUTORNAME });
                            }
                            if ($scope.listPermission[1].UNDERWHICHDISTRIBUTORID != '' && $scope.listPermission[1].UNDERWHICHDISTRIBUTORID != null) {
                                $scope.ngModelDistributor = $scope.listPermission[1].UNDERWHICHDISTRIBUTORID;
                                $("#ddlDistributor").select2("data", { id: $scope.listPermission[1].UNDERWHICHDISTRIBUTORID, text: $scope.listPermission[1].UNDERWHICHDISTRIBUTORNAME });
                            }
                        }

                        debugger
                        ////// pushing distributors as per area wise permission
                        $scope.loadAreaWiseDistributorRecords();
                    },
                        function (error) {
                            console.log("Error: " + error);
                        });
                }
                else {
                    debugger
                    Command: toastr["warning"]("You have no permission to view this report");
                    $scope.IsShowReportButtonDisabled = true;
                }

            }, function (error) {
                alert("failed");
            });
        }
        loadUserWiseReportPermission(0);
*/





        







           //-----------------------START FORM HERE ----------------------------------------------------
        // Common Query String for Report
        $scope.CommonQueryString = function (data) {
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;

            var showroomId = $scope.ngModelShowroom == undefined ? '' : $scope.ngModelShowroom;
            var productId = $scope.ngModelProduct == undefined ? '' : $scope.ngModelProduct;
            commonQueryString = startDate + ',' + endDate + ',' + showroomId + ',' + productId;
        };

   

     
        $scope.ShowShowroomLedger = function () {
            debugger
            var reportType = $scope.ReportType;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            var queryStringDSS = commonQueryString + ',' + reportType;

            var reportPath = repBaseUrl + 'ShowroomLedger/ShowroomLedger.aspx?queryString=' + queryStringDSS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };



        $scope.loadShowroom = function () {
            debugger
            $scope.ListShowroom = [];
            $scope.ngModelShowroom = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            var apiRoute = sysCommonUrl + 'GetShowroom/';
            var userId = objcmnParam.loggeduser;
            var showroomList = crudService.getObjList(apiRoute, userId);
            showroomList.then(function (response) {
                debugger
                $scope.ListShowroom = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        $scope.loadShowroom();



        $scope.loadProduct = function () {
            debugger
            $scope.loadProduct = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            var apiRoute = sysCommonUrl + 'GetProduct/';
            var userId = objcmnParam.loggeduser;
            var productList = crudService.getObjList(apiRoute, userId);
            productList.then(function (response) {
                debugger
                $scope.loadProduct = response.data;
                console.log("Load Product Is", $scope.loadProduct)
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        $scope.loadProduct();

        $scope.clear = function () {
            debugger
            $scope.loadProduct = [];
            $scope.ListShowroom = [];
            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();

        };
       

     
      
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}