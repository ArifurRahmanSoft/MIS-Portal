app.controller('PrimarySalesGrowthMonitoringCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
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

        $scope.PrevStartDate = conversion.NowDateCustom();
        $scope.PrevEndDate = conversion.NowDateCustom();
        $scope.PostStartDate = conversion.NowDateCustom();
        $scope.PostEndDate = conversion.NowDateCustom();

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

        // Get permission as per user
        // $scope.loadUserWiseReportPermission = function (isPaging) {

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
                    if (userrole == 16) { // 16 is the sales top role who have access to get any distributor data
                        $scope.loadDistributorAll();
                    }
                    var apiRoute = sysCommonUrl + 'GetPermissionForReport/';
                    var listPermission = crudService.getMultipleParameter(apiRoute, objcmnParam.loggeduser, userrole, page, pageSize, isPaging, $scope.HeaderToken.get);
                    listPermission.then(function (response) {
                        $scope.listPermission = response.data;
                        // load national data
                        var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
                        var mode = '1';
                        var trackNo = '0';
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
                            //  $scope.loadAreaWiseDistributorRecords();
                            // $scope.loadProductCategory();
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
                            // $scope.loadAreaWiseDistributorRecords();

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
                            //   $scope.loadAreaWiseDistributorRecords();                          
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
                            //  $scope.loadAreaWiseDistributorRecords();
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



        // Get all division
        $scope.loadDivisionRecords = function () {

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


            //$scope.listProductCategory = [];
            //$scope.ngmProductCategory = '';
            //$("#ddlProductCategory").select2("data", { id: '', text: '--Select Category--' });

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

                $scope.loadAreaWiseDistributorRecords();//New
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

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

                $scope.loadAreaWiseDistributorRecords();//New
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        // Get all zone
        $scope.loadZoneRecords = function () {

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

                $scope.loadAreaWiseDistributorRecords();//New
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        // Get all distributor by zone   
        $scope.loadDistributorRecords = function () {

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

            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

            var nationalId = $scope.ngModelNational == '' || undefined ? 0 : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == '' || undefined ? 0 : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == '' || undefined ? 0 : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == '' || undefined ? 0 : $scope.ngModelZone;

            var apiRoute = sysCommonUrl + 'GetAreaWiseDistributorPrimary/';
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

            $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

            var apiRoute = sysCommonUrl + 'GetBrandListByNational/';
            //var apiRoute = sysCommonUrl + 'GetSKUByBrand/';
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

        $scope.setMode = function () {
            var mode = '0';
            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            var nationalId = $scope.ngModelNational == undefined ? '' : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == undefined ? '' : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == undefined ? '' : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == undefined ? '' : $scope.ngModelZone;
            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;

            if (distributorId != '') {
                mode = '1';
            }
            else if (distributorId == '' && zoneId != '') {
                mode = '2';
            }
            else if (distributorId == '' && zoneId == '' && regionId != '') {
                mode = '3';
            }
            else if (distributorId == '' && zoneId == '' && regionId == '' && divisionId != '') {
                mode = '4';
            }
            else if (distributorId == '' && zoneId == '' && regionId == '' && divisionId == '' && nationalId != '') {
                mode = '5';
            }

            return mode;
        }


        $scope.ShowReport = function () {
            debugger

            if ($scope.listPermission.length == 0 && $scope.listNational.length == 0) {
                Command: toastr["warning"]("No area is found to load report !!!");
                return;
            }

            var reportType = $scope.ReportType;
            var mode = $scope.setMode();

            if (mode === '0') {
                Command: toastr["warning"]("Please select a dropdown from national to distributor !!!");
                return;
            }

            var reportName = reportType == 'AreaBrand' ? mode == '1' ? 'Distributor' : mode == '2' ? 'Zone' : mode == '3' ? 'Region' : mode == '4' ? 'Division' : mode == '5' ? 'National' : '' :'Distributor';

            if (reportType === '' || reportType === undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            //var brandId = $scope.ngModelBrand;

            //if (reportType === 'Sales_Statement_Ton') {
            //    if (brandId === '' || brandId === undefined) {
            //        Command: toastr["warning"]("Please check brand name !!!");
            //        return;
            //    }
            //}

            var reportFilter = '';
            var selectedCompany = '';
            var selectedNational = '';
            var selectedDivision = '';
            var selectedRegion = '';
            var selectedZone = '';
            var selectedDistributor = '';


            angular.forEach($scope.listCompany, function (value) {
                if (value.SCOMP_OID == $scope.ngModelCompany) {
                    selectedCompany = value.SCOMP_NAME;
                }
            });

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

            if (selectedCompany != '') {
                reportFilter = 'Selected Area: ';
            }

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
                    + '-' + ' Region: ' + selectedRegion + '-' + ' Zone: ' + selectedZone + '-' + ' Distributor: ' + selectedDistributor.replace(/[&]/g, '');
            }

            reportFilter = reportFilter == "" ? 'Selected Area: All' : reportFilter;

            var prevStartDate = $scope.PrevStartDate;
            var prevEndDate = $scope.PrevEndDate;

            var postStartDate = $scope.PostStartDate;
            var postEndDate = $scope.PostEndDate;

            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            var nationalId = $scope.ngModelNational == undefined ? '' : $scope.ngModelNational;
            var divisionId = $scope.ngModelDivision == undefined ? '' : $scope.ngModelDivision;
            var regionId = $scope.ngModelRegion == undefined ? '' : $scope.ngModelRegion;
            var zoneId = $scope.ngModelZone == undefined ? '' : $scope.ngModelZone;
            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            debugger
            var brandId = $scope.ngModelBrand === undefined ? '' : $scope.ngModelBrand;
            var productId = $scope.ngModelProduct === undefined ? '' : $scope.ngModelProduct;
            var queryString = prevStartDate + ',' + prevEndDate + ',' + postStartDate + ',' + postEndDate + ',' + companyId + ','
                + nationalId + ',' + divisionId + ',' + regionId + ',' + zoneId + ',' + distributorId + ',' + reportFilter
                + ',' + brandId + ',' + productId + ',' + reportType + ',' + mode + ',' + reportName;

            //var queryString = commonQueryString + ',' + brandId + ',' + productId + ',' + reportType;
            //var queryString = commonQueryString + ',' + reportType;
            var reportPath = repBaseUrl + 'PrimarySalesGrowthMonitoring/PrimarySalesGrowthMonitoring.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);

        };

        $scope.clear = function () {
            //$scope.frmDistributorTarget.$setPristine();
            //$scope.frmDistributorTarget.$setUntouched();

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


            $scope.listCompany = [];
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });


            $scope.listBrandByCat = [];
            $scope.ngModelBrand = '';
            $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

            $scope.ListSKUBrand = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });


            $scope.listNational = [];
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

            $scope.ngModelDistributor = '';
            $scope.lstSampleNoList = '';
            $("#ddlItemGroup").select2("data", { id: '', text: '--Select Brand--' });

            $scope.PrevStartDate = conversion.NowDateCustom();
            $scope.PrevEndDate = conversion.NowDateCustom();

            $scope.PostStartDate = conversion.NowDateCustom();
            $scope.PostEndDate = conversion.NowDateCustom();
        };
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}