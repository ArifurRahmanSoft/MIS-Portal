app.controller('reportCommonCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
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
        $scope.DailySalesSummaryPageTitle = 'Daily Sales Summary';
        $scope.SalesStatementBulkPageTitle = 'Sales Statement (Bulk)';
        $scope.SalesPerformancePageTitle = 'Sales Performance Report';
        $scope.UndeliveredStatementPageTitle = 'Undelivered Statement';
        $scope.PartyCreditLimitPageTitle = 'Party Credit Limit';
        $scope.ConsumerPartyListPageTitle = 'Consumer Party List';
        $scope.CustomerWiseDeliveryPageTitle = 'Customer Wise Delivery';
        $scope.ListOfActiveSKUPageTitle = 'List of Active SKU';
        $scope.IntercompanyTransferPageTitle = 'Intercompany Transfer';
        $scope.SalesTeamMemberPageTitle = 'Sales Team Member Report';
        $scope.DeliveryStatementPageTitle = 'Delivery Statement';
        $scope.UndeliveredStatementFactoryPageTitle = 'Undelivered Statement Factory';

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





        function loadUserWiseReportPermission() {
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
                        //var mode = '1';
                        //var trackNo = '0';
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






        // good copy backup -- 22-12-2021

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
        //                    $scope.IsDisableCompany = true;

        //                    if ($scope.listPermission[0].NATIONALID != '' && $scope.listPermission[0].NATIONALID != null) {
        //                        $scope.ngModelNational = $scope.listPermission[0].NATIONALID;
        //                        $("#ddlNational").select2("data", { id: $scope.listPermission[0].NATIONALID, text: $scope.listPermission[0].NATIONALNAME });
        //                    }

        //                    if ($scope.listPermission[1].UNDERWHICHNATIONALID != '' && $scope.listPermission[1].UNDERWHICHNATIONALID != null) {
        //                        $scope.ngModelNational = $scope.listPermission[1].UNDERWHICHNATIONALID;
        //                        $("#ddlNational").select2("data", { id: $scope.listPermission[1].UNDERWHICHNATIONALID, text: $scope.listPermission[1].UNDERWHICHNATIONALNAME });
        //                    }
        //                    $scope.loadDivisionRecords();
        //                  //  $scope.loadAreaWiseDistributorRecords();
        //                    $scope.loadProductCategory();
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
        //                   // $scope.loadAreaWiseDistributorRecords();
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
        //                   // $scope.loadAreaWiseDistributorRecords();
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
        //                   // $scope.loadAreaWiseDistributorRecords();
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
        //        });
        //}
        //loadUserWiseReportPermission(0);        




        // Get all division
        $scope.loadDivisionRecords = function () {
            debugger
            $scope.listDivision = [];
            $scope.ngModelDivision = '';
            $("#ddlDivision").select2("data", { id: '', text: '--Select Division--' });

            $scope.listRegion = [];
            $scope.ngModelRegion = '';
            $("#ddlRegion").select2("data", { id: '', text: '--Select Region--' });

            $scope.listZone = [];
            $scope.ngModelZone = '';
            $("#ddlZone").select2("data", { id: '', text: '--Select Zone--' });

            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });


            $scope.listProductCategory = [];
            $scope.ngmProductCategory = '';
            $("#ddlProductCategory").select2("data", { id: '', text: '--Select Category--' });

            $scope.listBrandByCat = [];
            $scope.ngModelBrand = '';
            $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

            $scope.ListSKUBrand = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });



            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';

            var mode = '2';
            var trackNo = $scope.ngModelNational;

            var listDivision = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listDivision.then(function (response) {
                $scope.listDivision = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        // Get all region
        $scope.loadRegionRecords = function () {
            debugger
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

        // Get all zone
        $scope.loadZoneRecords = function () {
            debugger
            $scope.listZone = [];
            $scope.ngModelZone = '';
            $("#ddlZone").select2("data", { id: '', text: '--Select Zone--' });

            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });


            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '4';
            var trackNo = $scope.ngModelRegion;

            var listZone = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listZone.then(function (response) {
                $scope.listZone = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        // Get all distributor by zone   
        $scope.loadDistributorRecords = function () {
            debugger
            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '5';
            var trackNo = $scope.ngModelZone;

            var listDistributor = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listDistributor.then(function (response) {
                $scope.listDistributor = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        // Get area-wise distributor
        $scope.loadAreaWiseDistributorRecords = function () {
            debugger
            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            var nationalId = $scope.ngModelNational == '' || undefined ? 0 : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == '' || undefined ? 0 : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == '' || undefined ? 0 : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == '' || undefined ? 0 : $scope.ngModelZone;

            var apiRoute = sysCommonUrl + 'GetAreaWiseDistributor/';
            var listDistributor = crudService.getAreaWiseDistributor(apiRoute, nationalId, divisionId, regionId, zoneId, page, pageSize, isPaging, $scope.HeaderToken.get);
            listDistributor.then(function (response) {
                $scope.listDistributor = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        // Get all distributor // should get that person who have a global role
        $scope.loadDistributorAll = function () {
            debugger
            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '6';
            var trackNo = 0;

            var listDistributor = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listDistributor.then(function (response) {
                $scope.listDistributor = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        // Get all product type
        //function loadProductType() {
        //    var apiRoute = sysCommonUrl + 'GetProductType/';
        //    var mode = '1';
        //    var trackNo = '0';
        //    var listProductType = crudService.getTwoParameter(apiRoute, mode, trackNo);
        //    listProductType.then(function (response) {
        //        $scope.listProductType = response.data;
        //    },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //}
        //loadProductType();

        // Get all Secondary Type 
        function loadSCON_TYP() {
            var apiRoute = sysCommonUrl + 'GetSCON_TYP/';
            var mode = '1';
            var trackNo = '0';
            var listSCON_TYP = crudService.getTwoParameter(apiRoute, mode, trackNo);
            listSCON_TYP.then(function (response) {
                $scope.listSCON_TYP = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadSCON_TYP();

        // Get all product type
        function loadProductTypeSSTYP() {
            var apiRoute = sysCommonUrl + 'GetProductTypeSSTYP/';
            var mode = '1';
            var trackNo = '0';
            var listSSTYP = crudService.getTwoParameter(apiRoute, mode, trackNo);
            listSSTYP.then(function (response) {
                $scope.listSSTYP = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadProductTypeSSTYP();

        ///// Get all Locations 
        function loadLocationRecords(isPaging) {
            var apiRoute = sysCommonUrl + 'GetLocation/';
            var listLocation = crudService.getItemWithcmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listLocation.then(function (response) {
                $scope.listLocation = response.data.objLocationList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadLocationRecords(0);

        ///// Get all Product Group 
        function loadProductGroup(isPaging) {
            var apiRoute = sysCommonUrl + 'GetProductGroup/';
            var listProdGroup = crudService.getItemWithcmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listProdGroup.then(function (response) {
                $scope.listProdGroup = response.data.objProductGroup;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadProductGroup(0);



        ///// Get all brand on page load
        function loadBrandRecords(isPaging) {
            var apiRoute = sysCommonUrl + 'GetBrand/';
            var listBrand = crudService.getItemWithcmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listBrand.then(function (response) {
                $scope.listBrand = response.data.objBrandList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadBrandRecords(0);

        // Get product by brand 
        $scope.loadSKURecords = function () {
            $scope.ListSKUBrand = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            var apiRoute = sysCommonUrl + 'GetSKUByBrand/';
            var mode = '1';
            var trackNo = $scope.ngModelBrand;
            var listSKU = crudService.getSKUByBrand(apiRoute, mode, trackNo);
            listSKU.then(function (response) {
                debugger
                $scope.ListSKUBrand = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };

        // Get product by brand
        $scope.loadProductBrand = function () {

            $scope.ListSKUBrand = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            var apiRoute = sysCommonUrl + 'GetSKUByBrand/';
            var mode = '1';
            var trackNo = $scope.ngModelBrand;
            var listSKU = crudService.getSKUByBrand(apiRoute, mode, trackNo);
            listSKU.then(function (response) {
                debugger
                $scope.ListSKUBrand = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };

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
            var apiRoute = sysCommonUrl + 'getAllCompanyList/';
            var listModel = crudService.getAllList(apiRoute);
            listModel.then(function (response) {
                $scope.listCompany = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.GetAllCompany();


        // Get City Company list
        //$scope.CityCompanyList = function () {
        //    mCUID = $scope.ngModelCompany;
        //    $http.get(rootUrl + "/SystemCommon/api/SystemCommonDDL/getCityCompanyList").then(function (d) {
        //        $scope.listCityCompany = d.data;
        //    }, function (error) {
        //        alert("Failed");
        //    });
        //};
        //$scope.CityCompanyList();
        $scope.CityCompanyList = function () {
            var apiRoute = sysCommonUrl + 'getCityCompanyList/';
            var listModel = crudService.getAllList(apiRoute);
            listModel.then(function (response) {
                $scope.listCityCompany = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.CityCompanyList();

        //Sales Statement Bulk dropdown
        // Get all distributor
        $scope.loadDistributorAll = function () {
            debugger
            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Customer--' });

            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '11';
            var trackNo = 0;

            var listDistributor = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listDistributor.then(function (response) {
                $scope.listDistributor = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadDistributorAll();

        // Get User-Wise Company list - which has given permission
        $scope.UserWiseCompanyList = function () {
            $scope.cmnParam();
            var param = { loggeduser: objcmnParam.loggeduser };
            var apiRoute = sysCommonUrl + 'getUserWiseCompanyList/';
            var listModel = crudService.getListByParam(apiRoute, param);
            listModel.then(function (response) {
                $scope.listUserWiseCompany = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.UserWiseCompanyList();

        // Get Product By Company
        $scope.GetSKUByCompany = function () {

            $scope.ListProducts = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });


            var param = { selectedCompany: $scope.ngModelCompany };
            var apiRoute = sysCommonUrl + 'GetSKUByCompany/';
            var listModel = crudService.getListByParam(apiRoute, param);
            listModel.then(function (response) {
                $scope.ListProducts = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });



            //$http.get(sysCommonUrl + 'GetSKUByCompany', {
            //    params: { selectedCompany: $scope.ngModelCompany }
            //}).then(function (d) {
            //    $scope.ListProducts = d.data;
            //}, function (error) {
            //    alert("Failed");
            //    });

        };
        //Sales Statement Bulk dropdown



        // Delete from list while input
        $scope.deleteRow = function (index, dataModel) {
            $scope.ListDistributorTargetDetails.splice(index, 1);
            $scope.showDtgrid = $scope.ListDistributorTargetDetails.length;
        };

        // Show Brandwise Sales Statement Report
        $scope.ShowBrandWiseSalesStatement = function (data) {

            debugger

            if (data == 3) {
                if ($scope.ngModelNational == null || $scope.ngModelNational == '' || $scope.ngModelNational == undefined) {
                    Command: toastr["warning"]("Please select national !!!");
                    return;
                }
            }

            var reportFilter = '';

            var selectedNational = '';
            var selectedDivision = '';
            var selectedRegion = '';
            var selectedZone = '';
            var selectedDistributor = '';


            angular.forEach($scope.listNational, function (value) {
                if (value.NTNAL_OID == $scope.ngModelNational) {
                    selectedNational = value.NTNAL_NAME;
                }
            });

            angular.forEach($scope.listDivision, function (value) {
                if (value.DIVISION_ID == $scope.ngModelDivision) {
                    selectedDivision = value.DIVISION_NAME;
                }
            });

            angular.forEach($scope.listRegion, function (value) {
                if (value.REGION_ID == $scope.ngModelRegion) {
                    selectedRegion = value.REGION_NAME;
                }
            });

            angular.forEach($scope.listZone, function (value) {
                if (value.ZONE_ID == $scope.ngModelZone) {
                    selectedZone = value.ZONE_NAME;
                }
            });

            angular.forEach($scope.listDistributor, function (value) {
                if (value.DIST_ID == $scope.ngModelDistributor) {
                    selectedDistributor = value.DIST_NAME;
                }
            });


            if (selectedNational != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedDivision != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedRegion != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ')
                    + '-' + ' Region: ' + selectedRegion.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedZone != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ')
                    + '-' + ' Region: ' + selectedRegion.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Zone: ' + selectedZone.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedDistributor != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ')
                    + '-' + ' Region: ' + selectedRegion.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Zone: ' + selectedZone.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Distributor: ' + selectedDistributor.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            debugger

            reportFilter = reportFilter == "" ? 'Selected Area: All' : reportFilter;

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            var nationalId = $scope.ngModelNational == undefined ? '' : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == undefined ? '' : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == undefined ? '' : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == undefined ? '' : $scope.ngModelZone;
            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var reportOption = data;

            var queryStringBWSS = startDate + ',' + endDate + ',' + nationalId + ','
                + divisionId + ',' + regionId + ',' + zoneId + ',' + distributorId + ',' + reportFilter + ',' + reportOption;

            var reportPath = repBaseUrl + 'BrandWiseSalesStatement/BrandWiseSalesStatement.aspx?queryString=' + queryStringBWSS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };


        // Show Brandwise Sales Summary Monthly and Yearly Report
        $scope.ShowBrandWiseSalesMonthYear = function (data) {

            if ($scope.ReportType == '' || $scope.ReportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            var reportFilter = '';

            var selectedNational = '';
            var selectedDivision = '';
            var selectedRegion = '';
            var selectedZone = '';
            var selectedDistributor = '';


            angular.forEach($scope.listNational, function (value) {
                if (value.NTNAL_OID == $scope.ngModelNational) {
                    selectedNational = value.NTNAL_NAME;
                }
            });

            angular.forEach($scope.listDivision, function (value) {
                if (value.DIVISION_ID == $scope.ngModelDivision) {
                    selectedDivision = value.DIVISION_NAME;
                }
            });

            angular.forEach($scope.listRegion, function (value) {
                if (value.REGION_ID == $scope.ngModelRegion) {
                    selectedRegion = value.REGION_NAME;
                }
            });

            angular.forEach($scope.listZone, function (value) {
                if (value.ZONE_ID == $scope.ngModelZone) {
                    selectedZone = value.ZONE_NAME;
                }
            });

            angular.forEach($scope.listDistributor, function (value) {
                if (value.DIST_ID == $scope.ngModelDistributor) {
                    selectedDistributor = value.DIST_NAME;
                }
            });


            if (selectedNational != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedDivision != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedRegion != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ')
                    + '-' + ' Region: ' + selectedRegion.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedZone != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ')
                    + '-' + ' Region: ' + selectedRegion.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Zone: ' + selectedZone.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedDistributor != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ')
                    + '-' + ' Region: ' + selectedRegion.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Zone: ' + selectedZone.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Distributor: ' + selectedDistributor.replace(/[^a-zA-Z0-9]/g, ' ');
            }



            reportFilter = reportFilter == "" ? 'Selected Area: All' : reportFilter;

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;

            var nationalId = $scope.ngModelNational == undefined ? '' : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == undefined ? '' : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == undefined ? '' : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == undefined ? '' : $scope.ngModelZone;
            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;

            var queryStringBWSS = startDate + ',' + endDate + ',' + nationalId + ',' + divisionId + ',' + regionId + ',' + zoneId + ',' + distributorId + ',' + reportFilter + ',' + data + ',' + $scope.ReportType;

            var reportPath = repBaseUrl + 'BrandWiseSalesMonthYear/BrandWiseSalesMonthYear.aspx?queryString=' + queryStringBWSS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        }


        // Common Query String for Report
        $scope.CommonQueryString = function (data) {
            var reportFilter = '';
            var selectedNational = '';
            var selectedDivision = '';
            var selectedRegion = '';
            var selectedZone = '';
            var selectedDistributor = '';

            angular.forEach($scope.listNational, function (value) {
                if (value.NTNAL_OID == $scope.ngModelNational) {
                    selectedNational = value.NTNAL_NAME;
                }
            });

            angular.forEach($scope.listDivision, function (value) {
                if (value.DIVISION_ID == $scope.ngModelDivision) {
                    selectedDivision = value.DIVISION_NAME;
                }
            });

            angular.forEach($scope.listRegion, function (value) {
                if (value.REGION_ID == $scope.ngModelRegion) {
                    selectedRegion = value.REGION_NAME;
                }
            });

            angular.forEach($scope.listZone, function (value) {
                if (value.ZONE_ID == $scope.ngModelZone) {
                    selectedZone = value.ZONE_NAME;
                }
            });

            angular.forEach($scope.listDistributor, function (value) {
                if (value.DIST_ID == $scope.ngModelDistributor) {
                    selectedDistributor = value.DIST_NAME;
                }
            });

            if (selectedNational != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedDivision != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedRegion != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ')
                    + '-' + ' Region: ' + selectedRegion.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedZone != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ')
                    + '-' + ' Region: ' + selectedRegion.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Zone: ' + selectedZone.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            if (selectedDistributor != '') {
                reportFilter = 'Selected Area: ' + selectedNational.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Division: ' + selectedDivision.replace(/[^a-zA-Z0-9]/g, ' ')
                    + '-' + ' Region: ' + selectedRegion.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Zone: ' + selectedZone.replace(/[^a-zA-Z0-9]/g, ' ') + '-' + ' Distributor: ' + selectedDistributor.replace(/[^a-zA-Z0-9]/g, ' ');
            }

            reportFilter = reportFilter == "" ? 'Selected Area: All' : reportFilter;

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;

            var nationalId = $scope.ngModelNational == undefined ? '' : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == undefined ? '' : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == undefined ? '' : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == undefined ? '' : $scope.ngModelZone;
            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;

            commonQueryString = startDate + ',' + endDate + ',' + nationalId + ','
                + divisionId + ',' + regionId + ',' + zoneId + ',' + distributorId + ',' + reportFilter;
        };

        $scope.ShowSalesStatementBulk = function () {
            var reportType = $scope.ReportType;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            var queryStringDSS = commonQueryString + ',' + reportType;

            var reportPath = repBaseUrl + 'SalesStatementBulk/SalesStatementBulk.aspx?queryString=' + queryStringDSS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowDailySalesSummary = function () {
            var reportType = $scope.ReportType;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            var queryStringDSS = commonQueryString + ',' + reportType;

            var reportPath = repBaseUrl + 'DailySalesSummary/DailySalesSummary.aspx?queryString=' + queryStringDSS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };



        $scope.ShowListOfActiveSKU = function () {
            debugger
            var ProductSCON_TYP = $scope.ngModelProductSCON_TYP;

            //if (ProductSCON_TYP == '' || ProductSCON_TYP == undefined) {
            //    Command: toastr["warning"]("Please check product type !!!");
            //    return;
            //}

            //var queryStringSPS = productTypeSSTYP + ',' + productTypeSSTYP;
            var productType = ProductSCON_TYP;

            var reportPath = repBaseUrl + 'ListOfActiveSKU/ListOfActiveSKU.aspx?productType=' + productType;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };


        //$scope.loadCompanyWiseBrand = function () {
        //    $scope.cmnParam();
        //    debugger
        //    var companyId = $scope.ngModelProductCompany;
        //    $http.get("/MISReport/SystemCommon/api/SystemCommonDDL/getCompanyWiseBrand", {
        //        params: { companyId: companyId }
        //    }).then(function (d) {
        //        $scope.listBrandCompany = d.data;
        //    }, function (error) {
        //        alert("Failed");
        //    });
        //};

        $scope.loadCompanyWiseBrand = function () {
            $scope.cmnParam();
            var param = { companyId: $scope.ngModelProductCompany };
            var apiRoute = sysCommonUrl + 'getCompanyWiseBrand/';
            var listModel = crudService.getListByParam(apiRoute, param);
            listModel.then(function (response) {
                $scope.listBrandCompany = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }



        // Get User-Wise Company list
        //$scope.UserWiseCompanyList = function () {
        //    $scope.cmnParam();
        //    var loggeduser = objcmnParam.loggeduser;
        //    $http.get("/MISReport/SystemCommon/api/SystemCommonDDL/getUserWiseCompanyList", {
        //        params: { loggeduser: loggeduser }
        //    }).then(function (d) {
        //        $scope.listUserWiseCompany = d.data;
        //    }, function (error) {
        //        alert("Failed");
        //    });
        //};
        //$scope.UserWiseCompanyList();

        $scope.UserWiseCompanyList = function () {
            $scope.cmnParam();
            var param = { loggeduser: objcmnParam.loggeduser };
            var apiRoute = sysCommonUrl + 'getUserWiseCompanyList/';
            var listModel = crudService.getListByParam(apiRoute, param);
            listModel.then(function (response) {
                $scope.listUserWiseCompany = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.UserWiseCompanyList();

        $scope.ShowProductionDemand = function () {

            var reportType = $scope.ReportType;
            var salesCompany = $scope.ngModelSalesCompany;
            var productCompany = $scope.ngModelProductCompany;
            var brand = $scope.ngModelBrand;
            var fromDate = $scope.StartDate;
            var toDate = $scope.EndDate;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            var queryStringDSS = reportType + ',' + salesCompany + ',' + productCompany + ',' + brand + ',' + fromDate + ',' + toDate;

            var reportPath = repBaseUrl + 'ProductionDemand/ProductionDemand.aspx?queryString=' + queryStringDSS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.clearListOfActiveSKU = function () {
            $scope.ngModelProductSCON_TYP = '';
            $("#ddlProductSCON_TYP").select2("data", { id: '', text: '--Select Product Type--' });
        }
        $scope.clearPartyCreditLimit = function () {
            $scope.ngModelProductTypeSSTYP = '';
            $("#ddlProductTypeSSTYP").select2("data", { id: '', text: '--Select Product Type--' });
        }
        $scope.ShowReportPartyCreditLimit = function () {
            debugger
            var productTypeSSTYP = $scope.ngModelProductTypeSSTYP;

            if (productTypeSSTYP == '' || productTypeSSTYP == undefined) {
                Command: toastr["warning"]("Please check product type !!!");
                return;
            }

            //var queryStringSPS = productTypeSSTYP + ',' + productTypeSSTYP;
            var productType = productTypeSSTYP;

            var reportPath = repBaseUrl + 'PartyCreditLimit/PartyCreditLimit.aspx?productType=' + productType;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);            
        };
        $scope.clearConsumerPartyList = function () {
            $scope.ngModelProductTypeSSTYP = '';
            $("#ddlProductTypeSSTYP").select2("data", { id: '', text: '--Select Product Type--' });
        }


        $scope.ShowReportConsumerPartyList = function (data) {
            debugger
            var productTypeSSTYP = $scope.ngModelProductTypeSSTYP;

            if ((productTypeSSTYP === '' || productTypeSSTYP === undefined) && data === 2) {
                Command: toastr["warning"]("Please check product type !!!");
                return;
            }
            var productType = productTypeSSTYP + ',' + data;

            var reportPath = repBaseUrl + 'ConsumerPartyList/ConsumerPartyList.aspx?productType=' + productType;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };


        $scope.ShowSalesPerformanceSummary = function (data) {

            if ($scope.listPermission.length == 0 && $scope.listNational.length == 0) {
                Command: toastr["warning"]("No area is found to load report !!!");
                return;
            }

            $scope.cmnParam();
            var loggeduser = objcmnParam.loggeduser;

            if ($scope.ReportType == null || $scope.ReportType == undefined || $scope.ngModelNational == null || $scope.ngModelNational == undefined) {
                Command: toastr["warning"]("Please select your desired national and report type !!!");
                return;
            }

            if ($scope.ngModelNational == 'SNTNLxxxxxxxxxN00005' || $scope.ngModelNational == 'SNTNLxxxxxxxxxN00008') {
                Command: toastr["warning"]("Sales Performance Report is not available for this national !!!");
                return;
            }
            //if ($scope.ngModelNational == 'SNTNLxxxxxxxxxN00003' || $scope.ngModelNational == 'SNTNLxxxxxxxxxN00005' || $scope.ngModelNational == 'SNTNLxxxxxxxxxN00008') {
            //    Command: toastr["warning"]("Sales Performance Report is not available for this national !!!");
            //    return;
            //}

            if (loggeduser == '07198' && ($scope.ngModelNational == 'SNTNLxxxxxxxxxN00001' || $scope.ngModelNational == 'SNTNLxxxxxxxxxN00002')) {
                Command: toastr["warning"]("You are not entitled to view this report !!!"); // NATIONAL - CONSUMER PACK // NATIONAL - FEED PRODUCT
                return;
            }

            var queryStringSPS = commonQueryString + ',' + data + ',' + $scope.ReportType;

            var reportPath = repBaseUrl + 'SalesPerformance/SalesPerformanceReport.aspx?queryString=' + queryStringSPS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowUndeliveredStatement = function () {

            if ($scope.ReportType == '' || $scope.ReportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            var queryStringUDS = commonQueryString + ',' + $scope.ReportType + ',' + $scope.ngModelBrand + ',' + $scope.ngModelProduct;

            var reportPath = repBaseUrl + 'UndeliveredStatement/UndeliveredStatement.aspx?queryString=' + queryStringUDS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };
        // Get all product category according to national fields change
        $scope.loadProductCategory = function () {
            $("#ddlProductCategory").select2("data", { id: '', text: '--Select Category--' });
            var apiRoute = sysCommonUrl + 'Get_PROD_CAT_BY_NTN/';
            var mode = '1';
            var trackNo = $scope.ngModelNational;
            ////var trackNo = 'SNTNLxxxxxxxxxN00001';
            var listProductCategory = crudService.Get_PROD_CAT_BY_NTN(apiRoute, mode, trackNo);
            listProductCategory.then(function (response) {
                debugger
                $scope.listProductCategory = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        // Get all brand by category
        $scope.loadBrandByCategory = function () {
            $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

            var apiRoute = sysCommonUrl + 'GetBrandByCategoryCC/';
            //var apiRoute = sysCommonUrl + 'GetSKUByBrand/';
            var mode = '1';
            var trackNo = $scope.ngmProductCategory;
            var listBrandByCat = crudService.GetBrandByCategoryCC(apiRoute, mode, trackNo);
            listBrandByCat.then(function (response) {
                debugger
                $scope.listBrandByCat = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }


        // Get all brand by National
        $scope.loadBrandByNational = function () {

            $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });
            var apiRoute = sysCommonUrl + 'GetBrandListByNational/';
            var mode = '1';
            var trackNo = $scope.ngModelNational;
            var listBrandListByNational = crudService.GetBrandListByNational(apiRoute, mode, trackNo);
            listBrandListByNational.then(function (response) {
                $scope.listBrandListByNational = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }


        $scope.ShowDeliveryStatement = function () {
            debugger
            if ($scope.ReportType == '' || $scope.ReportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            var queryStringUDS = commonQueryString + ',' + $scope.ngModelBrand + ',' + $scope.ngModelProduct + ',' + $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + $scope.ReportType;
                        
            var reportPath = repBaseUrl + 'DeliveryStatement/DeliveryStatement.aspx?queryString=' + queryStringUDS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };


        // Primary Sales Target
        $scope.ShowPrimarySalesTarget = function () {
            if ($scope.ReportType == null || $scope.ReportType == undefined || $scope.ngModelNational == null || $scope.ngModelNational == undefined) {
                Command: toastr["warning"]("Please select your desired national and report type !!!");
                return;
            }
            var queryStringPT = commonQueryString + ',' + $scope.ngModelBrand + ',' + $scope.ReportType;

            var reportPath = repBaseUrl + 'PrimarySalesTarget/DistributorPrimaryTarget.aspx?queryString=' + queryStringPT;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };



        // Clear info after sucessful transaction
        $scope.clear = function () {
            $scope.frmDistributorTarget.$setPristine();
            $scope.frmDistributorTarget.$setUntouched();

            $scope.IsbtSaveReviseShow = $scope.IsLCCompleted == true ? false : true;

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.DistributorTargetID = '0';
            $scope.DistributorTargetNo = '';
            $scope.showDtgrid = 0;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;
            $scope.btnPIShowText = "Show List";
            $scope.btnPISaveText = "Save";
            $scope.bool = true;

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
        $scope.clearUndeliveredStatement = function () {
            $scope.formUndeliveredStatement.$setPristine();
            $scope.formUndeliveredStatement.$setUntouched();

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;

            //$scope.listCompany = [];
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            //$scope.listNational = [];
            $scope.ngModelNational = '';
            $("#ddlNational").select2("data", { id: '', text: '--Select National--' });


            $scope.listDivision = [];
            $scope.ngModelDivision = '';
            $("#ddlDivision").select2("data", { id: '', text: '--Select Division--' });

            $scope.listRegion = [];
            $scope.ngModelRegion = '';
            $("#ddlRegion").select2("data", { id: '', text: '--Select Region--' });

            $scope.listZone = [];
            $scope.ngModelZone = '';
            $("#ddlZone").select2("data", { id: '', text: '--Select Zone--' });

            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            //$scope.listBrand = [];
            $scope.ngmProductCategory = '';
            $("#ddlProductCategory").select2("data", { id: '', text: '--Select Brand--' });

            //$scope.listBrand = [];
            $scope.ngModelBrand = '';
            $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

            $scope.ListSKUBrand = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();
        };
        $scope.clearUndeliveredFactoryStatement = function () {
            $scope.formUndeliveredStatement.$setPristine();
            $scope.formUndeliveredStatement.$setUntouched();

            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;

            //$scope.listCompany = [];
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            //$scope.listNational = [];
            $scope.ngModelNational = '';
            $("#ddlNational").select2("data", { id: '', text: '--Select National--' });


            $scope.listDivision = [];
            $scope.ngModelDivision = '';
            $("#ddlDivision").select2("data", { id: '', text: '--Select Division--' });

            $scope.listRegion = [];
            $scope.ngModelRegion = '';
            $("#ddlRegion").select2("data", { id: '', text: '--Select Region--' });

            $scope.listZone = [];
            $scope.ngModelZone = '';
            $("#ddlZone").select2("data", { id: '', text: '--Select Zone--' });

            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            //$scope.listBrand = [];
            $scope.ngmProductCategory = '';
            $("#ddlProductCategory").select2("data", { id: '', text: '--Select Brand--' });

            //$scope.listBrand = [];
            $scope.ngModelBrand = '';
            $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

            $scope.ListSKUBrand = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();
        };
        $scope.clearDeliveryStatement = function () {
            //$scope.formUndeliveredStatement.$setPristine();
            //$scope.formUndeliveredStatement.$setUntouched();

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

            //$scope.listNational = [];
            $scope.ngModelNational = '';
            $("#ddlNational").select2("data", { id: '', text: '--Select National--' });


            // $scope.listDivision = [];
            $scope.ngModelDivision = '';
            $("#ddlDivision").select2("data", { id: '', text: '--Select Division--' });

            // $scope.listRegion = [];
            $scope.ngModelRegion = '';
            $("#ddlRegion").select2("data", { id: '', text: '--Select Region--' });

            //  $scope.listZone = [];
            $scope.ngModelZone = '';
            $("#ddlZone").select2("data", { id: '', text: '--Select Zone--' });

            // $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            //$scope.listBrand = [];
            $scope.ngmProductCategory = '';
            $("#ddlProductCategory").select2("data", { id: '', text: '--Select Brand--' });

            //$scope.listBrand = [];
            $scope.ngModelBrand = '';
            $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

            //  $scope.ListSKUBrand = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();
        };

        $scope.clearSalesStatementBulk = function () {
            $scope.ReportType = '';
            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Customer--' });

            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });
        }

        //**********---- End Clear records ----***************//

        $scope.ShowDistributorAutomationReport = function () {
            if ($scope.ReportType == '' || $scope.ReportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            var queryString = commonQueryString + ',' + $scope.ReportType;
            var reportPath = '/Areas/SECONDARYSALESMISREPORT/Reports/DistributorAutomation/DistributorAutomation.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
        };


        $scope.ShowIntercompanyTransfer = function () {
            var selectedLocation = '';
            angular.forEach($scope.listLocation, function (value) {
                if (value.SGLOC_ID == $scope.ngModelLocation) {
                    selectedLocation = value.SGLOC_NAME;
                }
            });
            var reportFilter = selectedLocation == "" ? 'All' : selectedLocation;

            var queryStringIT = $scope.ngModelLocation + ',' + $scope.StartDate + ',' + $scope.EndDate + ','
                + reportFilter + ',' + $scope.ngModelProdGroup + ',' + $scope.ReportType;

            var reportPath = repBaseUrl + 'IntercompanyTransfer/IntercompanyTransferReport.aspx?queryString=' + queryStringIT;
            conversion.reportOpen(reportPath);
        };


        $scope.ShowDepotClosingStock = function () {
            var selectedLocation = '';
            angular.forEach($scope.listLocation, function (value) {
                if (value.SGLOC_ID == $scope.ngModelLocation) {
                    selectedLocation = value.SGLOC_NAME;
                }
            });
            var reportFilter = selectedLocation == "" ? 'All' : selectedLocation;

            var queryStringCS = $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + $scope.ngModelBrand + ',' 
                + $scope.ngModelProduct + ',' + $scope.StartDate + ',' + $scope.EndDate + ',' + reportFilter + ',' + $scope.ReportType;

            var reportPath = repBaseUrl + 'DepotClosingStock/DepotClosingStockReport.aspx?queryString=' + queryStringCS;
            conversion.reportOpen(reportPath);
        };

        $scope.ShowStockByRate = function () {
            if ($scope.ReportType == '' || $scope.ReportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            var selectedLocation = '';
            angular.forEach($scope.listLocation, function (value) {
                if (value.SGLOC_ID == $scope.ngModelLocation) {
                    selectedLocation = value.SGLOC_NAME;
                }
            });
            var reportFilter = selectedLocation == "" ? 'All' : selectedLocation;

            var queryStringCS = $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + $scope.ngModelBrand + ','
                + $scope.ngModelProduct + ',' + $scope.StartDate + ',' + $scope.EndDate + ',' + reportFilter + ',' + $scope.ReportType;

            var reportPath = repBaseUrl + 'DepotClosingStockByRate/DepotClosingStockByRate.aspx?queryString=' + queryStringCS;
            conversion.reportOpen(reportPath);
        };



        $scope.ShowCostingComparison = function () {
            var queryStringUDS = $scope.ngModelCompany + ',' + $scope.StartDate + ',' + $scope.EndDate;

            var reportPath =  '/Areas/COSTING/Reports/CostingComparison/CostingComparison.aspx?queryString=' + queryStringUDS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowSalesTeamMember = function (data) {

            if ($scope.listPermission.length == 0 && $scope.listNational.length == 0) {
                Command: toastr["warning"]("No area is found to load report !!!");
                return;
            }

            var queryStringSPS = commonQueryString;
            var reportPath = repBaseUrl + 'SalesTeamMember/SalesTeamMemberReport.aspx?queryString=' + queryStringSPS;
            conversion.reportOpen(reportPath);
        };

        $scope.clearCostingComparison = function () {
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

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