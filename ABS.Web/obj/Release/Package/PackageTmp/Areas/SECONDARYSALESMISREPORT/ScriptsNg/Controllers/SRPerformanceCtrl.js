app.controller('srPerformanceCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, $http, $window, crudService, conversion, $filter, $localStorage, uiGridConstants) {

        //**************************************************Start Vairiable Initialize**************************************************
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        var repBaseUrl = '/Areas/SECONDARYSALESMISREPORT/Reports/';

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
                if (response.data.length > 0) {
                    userrole = response.data;
                    var apiRoute = sysCommonUrl + 'GetPermissionForReport/';
                    var listPermission = crudService.getMultipleParameter(apiRoute, objcmnParam.loggeduser, userrole, page, pageSize, isPaging, $scope.HeaderToken.get);
                    listPermission.then(function (response) {
                        $scope.listPermission = response.data;
                        // load national data
                        var apiRoute = sysCommonUrl + 'GetSalesAreaHierarchySecondaryList/';
                        var mode = '15';
                        var trackNo = objcmnParam.loggeduser;
                        var listNational = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
                        listNational.then(function (response) {
                            $scope.listNational = response.data;
                        },
                            function (error) {
                                console.log("Error: " + error);
                            });

                        if ($scope.listPermission[0].ENABLENATIONAL == '1') {
                            $scope.loadDistributorAll();
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
                            $scope.loadBrandByNational();
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
                        }

                        if ($scope.listPermission[0].ENABLEDISTRIBUTOR == '1') {
                            $scope.IsDisableDistributor = false;
                            $scope.IsReadOnlyDistributor = false;
                        }
                        else {
                            debugger
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

                        ////// pushing sales officer as per area wise permission
                        $scope.loadAreaWiseSalesPerson();
                    },
                        function (error) {
                            console.log("Error: " + error);
                        });


                }
                else {
                    Command: toastr["warning"]("You have no permission to view this report");
                    $scope.IsShowReportButtonDisabled = true;
                }
            }, function (error) {
                alert("failed");
            });
        }
        loadUserWiseReportPermission(0);

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

            $scope.listRoute = [];
            $scope.ngModelRoute = '';
            $("#ddlRoute").select2("data", { id: '', text: '-- Select Route --' });

            //$scope.listBrandByCat = [];
            //$scope.ngModelBrand = '';
            //$("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

            if (!$scope.isBrandGroup) {
                $scope.listBrandGroup = [];
                $scope.brandGroupId = '';
                $("#ddlBrandGroup").select2("data", { id: '', text: '--Select Brand Group--' });

                $scope.listBrandListByNational = [];
                $scope.ngModelBrand = '';
                $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });
            }

            $scope.ListSKUBrand = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            var apiRoute = sysCommonUrl + 'GetSalesAreaHierarchySecondaryList/';

            var mode = '2';
            var trackNo = $scope.ngModelNational;

            var listDivision = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listDivision.then(function (response) {
                $scope.listDivision = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });

            // loading distributor as per national selection
            $scope.loadAreaWiseDistributorRecords();

            ////// pushing sales officer as per national selection
            $scope.loadAreaWiseSalesPerson();
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

            $scope.listRoute = [];
            $scope.ngModelRoute = '';
            $("#ddlRoute").select2("data", { id: '', text: '-- Select Route --' });

            var apiRoute = sysCommonUrl + 'GetSalesAreaHierarchySecondaryList/';
            var mode = '3';
            var trackNo = $scope.ngModelDivision;

            var listRegion = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listRegion.then(function (response) {
                $scope.listRegion = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });


            // loading distributor as per division selection
            $scope.loadAreaWiseDistributorRecords();

            ////// pushing sales officer as per division selection
            $scope.loadAreaWiseSalesPerson();
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

            $scope.listRoute = [];
            $scope.ngModelRoute = '';
            $("#ddlRoute").select2("data", { id: '', text: '-- Select Route --' });


            var apiRoute = sysCommonUrl + 'GetSalesAreaHierarchySecondaryList/';
            var mode = '4';
            var trackNo = $scope.ngModelRegion;

            var listZone = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listZone.then(function (response) {
                $scope.listZone = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });

            // loading distributor as per region selection
            $scope.loadAreaWiseDistributorRecords();

            ////// pushing sales officer as per region selection
            $scope.loadAreaWiseSalesPerson();
        }

        // Get all distributor by zone   
        $scope.loadDistributorRecords = function () {
            debugger
            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            $scope.listRoute = [];
            $scope.ngModelRoute = '';
            $("#ddlRoute").select2("data", { id: '', text: '-- Select Route --' });

            var apiRoute = sysCommonUrl + 'GetSalesAreaHierarchySecondaryList/';
            var mode = '5';
            var trackNo = $scope.ngModelZone;

            var listDistributor = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listDistributor.then(function (response) {
                $scope.listDistributor = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });

            ////// pushing sales officer as per zone selection
            $scope.loadAreaWiseSalesPerson();
        }


        // Get all distributor by zone as Per Primary Sales  
        $scope.loadDistributorPrimary = function () {
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

            $scope.listRoute = [];
            $scope.ngModelRoute = '';
            $("#ddlRoute").select2("data", { id: '', text: '-- Select Route --' });

            var nationalId = $scope.ngModelNational == '' || undefined ? 0 : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == '' || undefined ? 0 : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == '' || undefined ? 0 : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == '' || undefined ? 0 : $scope.ngModelZone;

            var apiRoute = sysCommonUrl + 'GetAreaWiseDistributorSecondary/';
            var listDistributor = crudService.getAreaWiseDistributor(apiRoute, nationalId, divisionId, regionId, zoneId, page, pageSize, isPaging, $scope.HeaderToken.get);
            listDistributor.then(function (response) {
                $scope.listDistributor = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }


        // Get area-wise sales person
        $scope.loadAreaWiseSalesPerson = function () {
            debugger;
            $scope.listSalesPerson = [];
            $scope.ngModelSalesPerson = '';
            $("#ddlSalesPerson").select2("data", { id: '', text: '--Select Sales Officer--' });

            var nationalId = $scope.ngModelNational == '' || undefined ? 0 : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == '' || undefined ? 0 : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == '' || undefined ? 0 : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == '' || undefined ? 0 : $scope.ngModelZone;
            var distributorId = $scope.ngModelDistributor == '' || undefined ? 0 : $scope.ngModelDistributor;

            var apiRoute = sysCommonUrl + 'GetAreaWiseSalesPerson/';
            var listSalesPerson = crudService.getAreaWiseSalesPerson(apiRoute, nationalId, divisionId, regionId, zoneId, distributorId, page,
                pageSize, isPaging, $scope.HeaderToken.get);

            listSalesPerson.then(function (response) {
                $scope.listSalesPerson = response.data;
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

            $scope.listRoute = [];
            $scope.ngModelRoute = '';
            $("#ddlRoute").select2("data", { id: '', text: '-- Select Route --' });

            var apiRoute = sysCommonUrl + 'GetSalesAreaHierarchySecondaryList/';
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

        // Get Route by distributor 
        $scope.loadRouteByDistributor = function () {
            debugger;
            var distCode = '';
            var distModel = $scope.listDistributor.filter(x => x.DIST_ID == $scope.ngModelDistributor)[0];
            if (distModel != undefined) {
                distCode = distModel.DIST_CODE;
            }
            $scope.listRoute = [];
            $scope.ngModelRoute = '';
            $("#ddlRoute").select2("data", { id: '', text: '-- Select Route --' });

            var apiRoute = sysCommonUrl + 'GetRouteByDistributor/';
            var mode = '1';
            var distributorId = distCode;
            var listRoute = crudService.getTwoParameter(apiRoute, mode, distributorId); // this name used just to get a mapping getSKUByBrand
            listRoute.then(function (response) {
                $scope.listRoute = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }


        // Get all brand group by userId
        $scope.isBrandGroup = false;
        $scope.brandGroupId = "";
        $scope.listBrandGroup = [];
        $scope.loadBrandGroupListByUser = function () {

            debugger
            $scope.listBrandGroup = [];
            $scope.brandGroupId = '';
            $("#ddlBrandGroup").select2("data", { id: '', text: '--Select Brand Group--' });

            var apiRoute = sysCommonUrl + 'GetBrandGroupListByUser/';
            var mode = '1';
            var trackNo = objcmnParam.loggeduser;
            var listBrandGroupByUser = crudService.getTwoParameter(apiRoute, mode, trackNo);
            listBrandGroupByUser.then(function (response) {
                $scope.listBrandGroup = response.data;
                if ($scope.listBrandGroup.length > 0) {
                    $scope.isBrandGroup = true;
                    $scope.loadBrandByNationalBrandGroup();
                } else {
                    $scope.isBrandGroup = false;
                    $scope.loadBrandByNational();
                }
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadBrandGroupListByUser();

        // Get all brand by National and brand group
        $scope.loadBrandByNationalBrandGroup = function () {

            debugger
            $scope.listBrandListByNational = [];
            $scope.ngModelBrand = '';
            $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

            var apiRoute = sysCommonUrl + 'GetBrandListByNational/';
            //var mode = '1';
            var nationalOid = $scope.ngModelNational == "" || $scope.ngModelNational == undefined ? null : $scope.ngModelNational;
            var brndGroupId = $scope.brandGroupId == "" || $scope.brandGroupId == undefined ? null : $scope.brandGroupId;
            var userId = objcmnParam.loggeduser;
            var listBrandListByNational = crudService.GetList(apiRoute, nationalOid, brndGroupId, userId);
            listBrandListByNational.then(function (response) {
                $scope.listBrandListByNational = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }


        // Get all product type
        function loadProductType() {
            var apiRoute = sysCommonUrl + 'GetProductType/';
            var mode = '1';
            var trackNo = '0';
            var listProductType = crudService.getTwoParameter(apiRoute, mode, trackNo);
            listProductType.then(function (response) {
                $scope.listProductType = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadProductType();

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

        ///// Get all brand 
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

        // Get all brand by National
        $scope.loadBrandByNational = function () {
            debugger
            if (!$scope.isBrandGroup) {
                $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

                var apiRoute = sysCommonUrl + 'GetBrandListByNational/';
                //var apiRoute = sysCommonUrl + 'GetSKUByBrand/';
                var mode = '1';
                var trackNo = $scope.ngModelNational;
                var listBrandListByNational = crudService.GetBrandListByNational(apiRoute, mode, trackNo);
                listBrandListByNational.then(function (response) {
                    debugger
                    $scope.listBrandListByNational = response.data;
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
        }

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
        }

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
        }

        // Get All Company list
        //$scope.GetAllCompany = function () {
        //    mCUID = $scope.ngModelCompany;
        //    $http.get(rootUrl + "/SystemCommon/api/SystemCommonDDL/getAllCompanyList").then(function (d) {
        //        $scope.listCompany = d.data;
        //    }, function (error) {
        //        alert("Failed");
        //    })
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

        // Delete from list while input
        $scope.deleteRow = function (index, dataModel) {
            $scope.ListDistributorTargetDetails.splice(index, 1);
            $scope.showDtgrid = $scope.ListDistributorTargetDetails.length;
        };


        ////////////////////******************** REPORT     ***********************/////////////////////////    
        ////////////////////******************** SECTION     ***********************/////////////////////////
        ////////////////////******************** STARTS     ***********************/////////////////////////
        ////////////////////******************** BELOW     ***********************/////////////////////////

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
                reportFilter = 'Selected Area: ' + selectedNational;
            }

            if (selectedDivision != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision;
            }

            if (selectedRegion != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion;
            }

            if (selectedZone != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone;
            }

            if (selectedDistributor != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone + '-' + ' Distributor: ' + selectedDistributor.replace(/[&]/g, '');;
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

        $scope.ShowActivityReport = function () {

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
                reportFilter = 'Selected Area: ' + selectedNational;
            }

            if (selectedDivision != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision;
            }

            if (selectedRegion != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion;
            }

            if (selectedZone != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone;
            }

            if (selectedDistributor != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone + '-' + ' Distributor: ' + selectedDistributor;
            }

            debugger

            reportFilter = reportFilter == "" ? 'Selected Area: All' : reportFilter;

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            debugger
            var nationalId = $scope.ngModelNational == undefined ? '' : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == undefined ? '' : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == undefined ? '' : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == undefined ? '' : $scope.ngModelZone;
            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;

            var queryString = startDate + ',' + endDate + ',' + nationalId + ','
                + divisionId + ',' + regionId + ',' + zoneId + ',' + distributorId + ',' + reportFilter;

            var reportPath = repBaseUrl + 'ActivityReport/ActivityReport.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);

        };

        $scope.ShowSoActivityReport = function () {

            if ($scope.listPermission.length == 0 && $scope.listNational.length == 0) {
                Command: toastr["warning"]("No area is found to load report !!!");
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
                reportFilter = 'Selected Area: ' + selectedNational;
            }

            if (selectedDivision != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision;
            }

            if (selectedRegion != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion;
            }

            if (selectedZone != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone;
            }

            if (selectedDistributor != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone + '-' + ' Distributor: ' + selectedDistributor;
            }

            debugger

            reportFilter = reportFilter == "" ? 'Selected Area: All' : reportFilter;

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            debugger
            var nationalId = $scope.ngModelNational == undefined ? '' : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == undefined ? '' : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == undefined ? '' : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == undefined ? '' : $scope.ngModelZone;
            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;

            var queryString = startDate + ',' + endDate + ',' + nationalId + ','
                + divisionId + ',' + regionId + ',' + zoneId + ',' + distributorId + ',' + reportFilter;

            var reportPath = repBaseUrl + 'SoActivityReport/SoActivityReport.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);

        };


        $scope.ShowDeviceLogReport = function () {
            if ($scope.ReportType == '' || $scope.ReportType == undefined || $scope.ReportType == null) {
                Command: toastr["warning"]("Please select your desired Report Option !!!");
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
                reportFilter = 'Selected Area: ' + selectedNational;
            }

            if (selectedDivision != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision;
            }

            if (selectedRegion != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion;
            }

            if (selectedZone != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone;
            }

            if (selectedDistributor != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone + '-' + ' Distributor: ' + selectedDistributor;
            }
            reportFilter = reportFilter == "" ? 'Selected Area: All' : reportFilter;
            var nationalId = $scope.ngModelNational == undefined ? '' : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == undefined ? '' : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == undefined ? '' : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == undefined ? '' : $scope.ngModelZone;
            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var reportOption = $scope.ReportType;

            var queryString = nationalId + ',' + divisionId + ',' + regionId + ',' + zoneId + ',' + distributorId + ',' + reportFilter + ',' + reportOption;

            var reportPath = repBaseUrl + 'SSalesDeviceLogReport/DeviceLogReport.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };


        $scope.OperationalRetailerCount = function (data) {

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
                if (value.DIST_CODE == $scope.ngModelDistributor) {
                    selectedDistributor = value.DIST_NAME;
                }
            });


            if (selectedNational != '') {
                reportFilter = 'Selected Area: ' + selectedNational;
            }

            if (selectedDivision != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision;
            }

            if (selectedRegion != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion;
            }

            if (selectedZone != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone;
            }

            if (selectedDistributor != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone + '-' + ' Distributor: ' + selectedDistributor;
            }

            debugger

            reportFilter = reportFilter == "" ? 'Selected Area: All' : reportFilter;

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            debugger
            var nationalId = $scope.ngModelNational == undefined ? '' : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == undefined ? '' : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == undefined ? '' : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == undefined ? '' : $scope.ngModelZone;
            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var routeId = $scope.ngModelRoute == undefined ? '' : $scope.ngModelRoute;
            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var salesPersonId = $scope.ngModelSalesPerson == undefined ? '' : $scope.ngModelSalesPerson;

            var brandId = $scope.ngModelBrand == undefined ? '' : $scope.ngModelBrand;
            var brandGroupId = $scope.brandGroupId == undefined ? '' : $scope.brandGroupId;
            var isBrandGroup = $scope.isBrandGroup ? '1' : '0';

            var queryString = startDate + ',' + endDate + ',' + nationalId + ',' + divisionId + ',' + regionId + ',' + zoneId + ',' + distributorId + ',' + salesPersonId + ','
                + routeId + ',' + brandId + ',' + isBrandGroup + ',' + brandGroupId + ',' + reportFilter + ',' + data + ',' + $scope.ReportType;

            var reportPath = repBaseUrl + 'OperationalRetailerCount/ReportOrderDeliveryRetailerCount.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.NonOperationalRetailer = function () {
            debugger
            //if ($scope.ngModelBrand == '' || $scope.ngModelBrand == undefined || $scope.ngModelBrand == null) {
            //    Command: toastr["warning"]("Please select your desired Brand to view this report !!!");
            //    return;
            //}
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
                reportFilter = 'Selected Area: ' + selectedNational;
            }

            if (selectedDivision != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision;
            }

            if (selectedRegion != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion;
            }

            if (selectedZone != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone;
            }

            if (selectedDistributor != '') {
                reportFilter = 'Selected Area: ' + selectedNational + '-' + ' Division: ' + selectedDivision
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone + '-' + ' Distributor: ' + selectedDistributor;
            }

            reportFilter = reportFilter == "" ? 'Selected Area: All' : reportFilter;

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;

            var nationalId = $scope.ngModelNational == undefined ? '' : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == undefined ? '' : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == undefined ? '' : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == undefined ? '' : $scope.ngModelZone;
            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var brandId = $scope.ngModelBrand == undefined ? '' : $scope.ngModelBrand;

            var brandGroupId = $scope.brandGroupId == undefined ? '' : $scope.brandGroupId;
            var isBrandGroup = $scope.isBrandGroup ? '1' : '0';

            var queryString = startDate + ',' + endDate + ',' + nationalId + ','
                + divisionId + ',' + regionId + ',' + zoneId + ',' + distributorId + ','
                + brandId + ',' + isBrandGroup + ',' + brandGroupId + ',' + reportFilter;

            var reportPath = repBaseUrl + 'NonOperationalRetailer/NonOperationalRetailerList.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);            
        };

        $scope.ShowProductTransactionReport = function (reportType) {
            if (reportType == "RETAILER") {
                if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined || $scope.ngModelDistributor == null) {
                    Command: toastr["warning"]("Please select your desired distributor to view this retailer-wiser report !!!");
                    return;
                }
            }

            var brandGroupId = $scope.brandGroupId == undefined ? '' : $scope.brandGroupId;
            var isBrandGroup = $scope.isBrandGroup ? '1' : '0';

            var queryStringPTR = commonQueryString + ',' + $scope.ngModelBrand + ',' + $scope.ngModelProduct + ',' + isBrandGroup + ',' + brandGroupId + ',' + reportType + ',' + $scope.reportcategory;

            var reportPath = repBaseUrl + 'ProductTransactionReport/TransactionReport.aspx?queryString=' + queryStringPTR;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowStockOrderDeliveryReport = function () {

            if ($scope.ngModelNational == '' || $scope.ngModelNational == undefined || $scope.ngModelNational == null) {
                Command: toastr["warning"]("Please select your desired national to view this report !!!");
                return;
            }

            if ($scope.ReportType == '' || $scope.ReportType == undefined || $scope.ReportType == null) {
                Command: toastr["warning"]("Please select your desired report type to view this report !!!");
                return;
            }

            var reporttype = $scope.ReportType;
            var queryString = commonQueryString + ',' + $scope.ngModelBrand + ',' + $scope.ngModelProduct + ',' + reporttype;

            var reportPath = repBaseUrl + 'StockOrderDelivery/StockOrderDelivery.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowProductAuditReport = function () {
            if ($scope.ngModelNational == '' || $scope.ngModelNational == undefined || $scope.ngModelNational == null) {
                Command: toastr["warning"]("Please select your desired national to view this report !!!");
                return;
            }
            if ($scope.ReportType == '' || $scope.ReportType == undefined || $scope.ReportType == null) {
                Command: toastr["warning"]("Please select your desired report type to view this report !!!");
                return;
            }
            var reporttype = $scope.ReportType;
            var queryString = commonQueryString + ',' + $scope.ngModelBrand + ',' + $scope.ngModelProduct + ',' + reporttype + ',' + $scope.ngModelRoute;

            var reportPath = repBaseUrl + 'ProductAudit/ProductAudit.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);            
        };

        $scope.ShowDeliveryCountingStatus = function () {
            var reporttype = $scope.ReportType;
            var queryString = commonQueryString + ',' + $scope.ngModelBrand + ',' + $scope.ngModelProduct + ',' + reporttype;

            var reportPath = repBaseUrl + 'DeliveryCountingStatus/DeliveryCountingStatus.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.MemoWiseRetailerDelivery = function () {
            if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined || $scope.ngModelDistributor == null) {
                Command: toastr["warning"]("Please select your desired distributor to view this report !!!");
                return;
            }
            var queryString = commonQueryString + ',' + $scope.ngModelBrand + ',' + $scope.ngModelProduct + ',' + $scope.ReportType;

            var reportPath = repBaseUrl + 'MemoWiseRetailerDelivery/MemoWiseDelivery.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowTargetVSAchievementSOWise = function () {

            if ($scope.ngModelNational == '' || $scope.ngModelNational == undefined || $scope.ngModelNational == null) {
                Command: toastr["warning"]("Please select your desired national to view this report !!!");
                return;
            }

            if ($scope.ReportType == '' || $scope.ReportType == undefined || $scope.ReportType == null) {
                Command: toastr["warning"]("Please select your desired report type to view this report !!!");
                return;
            }

            var reporttype = $scope.ReportType;
            var queryString = commonQueryString + ',' + $scope.ngModelSalesPerson + ',' + $scope.ngModelBrand + ',' + $scope.ngModelProduct + ',' + reporttype;

            var reportPath = repBaseUrl + 'TargetVSAchievementSOWise/TargetVSAchievement.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowSRPerformance = function () {

            if ($scope.ngModelNational == '' || $scope.ngModelNational == undefined || $scope.ngModelNational == null) {
                Command: toastr["warning"]("Please select your desired national to view this report !!!");
                return;
            }

            if ($scope.ReportType == '' || $scope.ReportType == undefined || $scope.ReportType == null) {
                Command: toastr["warning"]("Please select your desired report type to view this report !!!");
                return;
            }

            var brandId = $scope.ngModelBrand == undefined ? '' : $scope.ngModelBrand;
            var brandGroupId = $scope.brandGroupId == undefined ? '' : $scope.brandGroupId;
            var isBrandGroup = $scope.isBrandGroup ? '1' : '0';

            var reporttype = $scope.ReportType;
            var queryString = commonQueryString + ',' + $scope.ngModelSalesPerson + ',' + brandId + ',' + $scope.ngModelProduct + ',' + isBrandGroup + ',' + brandGroupId + ',' + reporttype;

            var reportPath = repBaseUrl + 'SRPerformance/SRPerformanceMonitoring.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.DistributorProfit = function () {
            //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined || $scope.ngModelDistributor == null) {
            //    Command: toastr["warning"]("Please select your desired distributor to view this report !!!");
            //    return;
            //}
            var queryString = commonQueryString + ',' + $scope.ngModelBrand + ',' + $scope.ngModelProduct;

            var reportPath = repBaseUrl + 'DistributorProfit/DistributorProfit.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.DistributorStockDayEndDelivery = function () {
            if (($scope.ReportType == '' || $scope.ReportType == undefined || $scope.ReportType == null)
                || ($scope.ngModelNational == '' || $scope.ngModelNational == undefined || $scope.ngModelNational == null)) {
                Command: toastr["warning"]("Please select your desired report type and a national to view the report !!!");
                return;
            }

            var brandGroupId = $scope.brandGroupId == undefined ? '' : $scope.brandGroupId;
            var isBrandGroup = $scope.isBrandGroup ? '1' : '0';

            var queryString = commonQueryString + ',' + $scope.ngModelBrand + ',' + $scope.ngModelProduct + ',' + isBrandGroup + ',' + brandGroupId + ',' + $scope.ReportType;
            var reportPath = repBaseUrl + 'DistributorStock/DistStockDayEndEntry.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
        };

        $scope.ReportType = 'InPcsParty';
        // Clear info after sucessful transaction
        $scope.clear = function () {

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

        //**********---- End Clear records ----***************//
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}