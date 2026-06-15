app.controller('BulkSalesCommonCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, $http, $window, crudService, conversion, $filter, $localStorage, uiGridConstants) {

        //**************************************************Start Vairiable Initialize**************************************************      
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        var repBaseUrl = '/Areas/SHOWROOM/Reports/';

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
        $scope.CustomerWiseDeliveryPageTitle = 'Customer Wise Delivery';

        $scope.HoldDataModel = '';

        $scope.ListDistributorTargetDetails = [];
        $scope.listDistTargetMaster = [];
        $scope.showDtgrid = 0;
        $scope.listDistributor = [];
        $scope.ngModelDistributor = '';

        $scope.ListProducts = [];
        $scope.ngModelProduct = '';
        $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });


        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;
        $scope.IsShowReportButtonDisabled = false;

        $scope.ngChkBoxExport = false;

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

        ///// Get all Locations 
        function loadLocationRecords(isPaging) {
            $scope.cmnParam();
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


        //// Get all locations with checkbox
        $scope.LoadAllLocationsCB = function (isPaging) {
            $scope.listLocation = null;
            $scope.ngmAllLocationSelected = "";

            $scope.cmnParam();
            var apiRoute = sysCommonUrl + 'GetFilteredLocation/';
            var listLocationCB = crudService.getItemWithcmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listLocationCB.then(function (response) {
                $scope.listLocationCB = response.data.objLocationList;
                debugger;
                angular.forEach($scope.listLocationCB, function (item) {
                    item.isChecked = true;
                })
                debugger;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.LoadAllLocationsCB(0);

        $scope.ngmAllLocationSelected = true;
        $scope.selectAllLocations = function () {
            for (var i = 0; i < $scope.listLocationCB.length; i++) {
                $scope.listLocationCB[i].isChecked = $scope.ngmAllLocationSelected;
            }
        };

        $scope.selectSingleLocation = function () {
            for (var i = 0; i < $scope.listLocationCB.length; i++) {
                if (!$scope.listLocationCB[i].isChecked) {
                    $scope.ngmAllLocationSelected = false;
                    return;
                }
            }
            $scope.ngmAllLocationSelected = true;
        };



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
        $scope.ShowSalesVsCollectionsReport = function () {
            var reporttype = $scope.ReportType;
            if (reporttype == '' || reporttype == undefined || reporttype == null) {
                Command: toastr["warning"]("Please select report type first !");
                return;
            }
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + startDate + ',' + endDate + ',' + reporttype;
            var reportPath = repBaseUrl + 'SaleVsCollectionS/SalesCollectionS.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
        };

        $scope.ShowBulkCustomerWiseDelivery = function () {

            // date issue as per user
            if (objcmnParam.loggeduser === '05624' || objcmnParam.loggeduser === '07648'
                || objcmnParam.loggeduser === '05276' || objcmnParam.loggeduser === '01310') {
                $scope.BackTo3Days = conversion.BackTo3Days();

                var firstDate = conversion.getStringToDate($scope.StartDate);
                var secondDate = conversion.getStringToDate($scope.BackTo3Days);
                var isExportIncluded = $scope.ngChkBoxExport;

                firstDate = new Date(firstDate);
                secondDate = new Date(secondDate);

                if (firstDate < secondDate) {
                    Command: toastr["warning"]("You are not authorized to view report more than 3 days !!!");
                    return;
                }
            }
            debugger
            var reportType = $scope.ReportType;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }



            var loggeduser = objcmnParam.loggeduser;
            var requistionNo = $scope.ngmRequisitionNumber;
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var productId = $scope.ngModelProduct == undefined ? '' : $scope.ngModelProduct;
            var transportId = $scope.ngModelTransportType == undefined ? '' : $scope.ngModelTransportType;
            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            var sardarId = $scope.ngModelSardarName == undefined ? '' : $scope.ngModelSardarName;
            var locationId = $scope.ngModelLocation == undefined ? '' : $scope.ngModelLocation;

            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType;
            var reportPath = repBaseUrl + 'BulkCustomerWiseDelivery/BulkCustomerWiseDelivery.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowBulkPartyLedger = function () {
            debugger;
            var locationId = "";
            $scope.SelectedLocations = [];
            if ($scope.listLocationCB.length > 0) {
                var checkedLocation = $scope.listLocationCB.filter(x => x.isChecked);
                if (checkedLocation.length > 0) {
                    angular.forEach(checkedLocation, function (item) {
                        $scope.SelectedLocations.push(item.SGLOC_ID);
                    });

                    locationId = $scope.SelectedLocations.join(',');
                }
            }

            if (locationId === "") {
                Command: toastr["warning"]("Please select a location !!!");
                return;
            }

            //if ($scope.SelectedFloorFlat.length === 0) {
            //    Command: toastr["warning"]("Please select at least one flat for tagging !!!");
            //    return false;
            //}

            debugger
            var reportType = $scope.ReportType;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            var loggeduser = objcmnParam.loggeduser;
            var requistionNo = $scope.ngmRequisitionNumber;
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var productId = $scope.ngModelProduct == undefined ? '' : $scope.ngModelProduct;
            //var transportId = $scope.ngModelTransportType == undefined ? '' : $scope.ngModelTransportType;
            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            //var sardarId = $scope.ngModelSardarName == undefined ? '' : $scope.ngModelSardarName;
            //var locationId = $scope.ngModelLocation == undefined ? '' : $scope.ngModelLocation;

            var commonQueryString = distributorId + '~'
                + productId + '~' + startDate + '~' + endDate + '~'
                + companyId + '~' + loggeduser + '~' + locationId + '~' + reportType;
            var reportPath = repBaseUrl + 'BulkPartyLedger/BulkPartyLedger.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
        };

        $scope.ShowUndeliveredStatement = function () {
            var isExportIncluded = $scope.ngChkBoxExport;
            var reportType = $scope.ReportType;
            if (reportType === '' || reportType === undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            var loggeduser = objcmnParam.loggeduser;
            var requistionNo = $scope.ngmRequisitionNumber;
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var productId = $scope.ngModelProduct == undefined ? '' : $scope.ngModelProduct;
            var transportId = $scope.ngModelTransportType == undefined ? '' : $scope.ngModelTransportType;
            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            var sardarId = $scope.ngModelSardarName == undefined ? '' : $scope.ngModelSardarName;
            var locationId = $scope.ngModelLocation == undefined ? '' : $scope.ngModelLocation;

            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType + ',' + isExportIncluded;
            var reportPath = repBaseUrl + 'BulkUndeliveredStatement/BulkUndeliveredStatement.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
        };


        //-------------------------------------------Start----------------------------------------------------
        //-------------------------------------------Start----------------------------------------------------
        $scope.ShowShowRoomStatememt = function () {
            var isExportIncluded = $scope.ngChkBoxExport;
            var reportType = $scope.ReportType;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            var loggeduser = objcmnParam.loggeduser;
            var requistionNo = $scope.ngmRequisitionNumber;
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var productId = $scope.ngModelProduct == undefined ? '' : $scope.ngModelProduct;
            var transportId = $scope.ngModelTransportType == undefined ? '' : $scope.ngModelTransportType;
            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            var sardarId = $scope.ngModelSardarName == undefined ? '' : $scope.ngModelSardarName;
            var locationId = $scope.ngModelLocation == undefined ? '' : $scope.ngModelLocation;

            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType + ',' + isExportIncluded;
            var reportPath = repBaseUrl + 'ShowroomSalesStatement/ShowroomSalesStatement.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
        };








             //-------------------------------------------End----------------------------------------------------
        //-------------------------------------------End----------------------------------------------------


        $scope.ShowBulkSalesStatement = function () {

            // date issue as per user
            if (objcmnParam.loggeduser === '05624' || objcmnParam.loggeduser === '07648'
                || objcmnParam.loggeduser === '05276' || objcmnParam.loggeduser === '01310') {
                $scope.BackTo3Days = conversion.BackTo3Days();

                var firstDate = conversion.getStringToDate($scope.StartDate);
                var secondDate = conversion.getStringToDate($scope.BackTo3Days);

                firstDate = new Date(firstDate);
                secondDate = new Date(secondDate);

                if (firstDate < secondDate) {
                    Command: toastr["warning"]("You are not authorized to view report more than 3 days !!!");
                    return;
                }
            }
            debugger
            var isExportIncluded = $scope.ngChkBoxExport;
            var reportType = $scope.ReportType;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            var loggeduser = objcmnParam.loggeduser;
            var requistionNo = $scope.ngmRequisitionNumber;
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var productId = $scope.ngModelProduct == undefined ? '' : $scope.ngModelProduct;
            var transportId = $scope.ngModelTransportType == undefined ? '' : $scope.ngModelTransportType;
            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            var sardarId = $scope.ngModelSardarName == undefined ? '' : $scope.ngModelSardarName;
            var locationId = $scope.ngModelLocation == undefined ? '' : $scope.ngModelLocation;

            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType + ',' + isExportIncluded;
            var reportPath = repBaseUrl + 'BulkSalesStatement/BulkSalesStatement.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.clear = function () {
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Customer--' });

            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            $scope.ngModelTransportType = '';
            $("#ddlTransportType").select2("data", { id: '', text: '--Select Transport Type--' });

            $scope.ngModelLocation = '';
            $("#ddlLocation").select2("data", { id: '', text: '--Select Location--' });

            $scope.ngModelSardarName = '';
            $("#ddlSardarName").select2("data", { id: '', text: '--Select Sardar Name--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();

            $scope.ngmRequisitionNumber = '';
        };



        ///////////////------------------------- Below for Factory Version --------------------------
        $scope.ShowFactoryBulkDelivery = function () {

            debugger
            var reportType = $scope.ReportType;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            var locationId = $scope.ngModelLocation;
            if (locationId == '' || locationId == undefined) {
                Command: toastr["warning"]("Please select location!");
                return;
            }

            var loggeduser = objcmnParam.loggeduser;
            var requistionNo = $scope.ngmRequisitionNumber;
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var productId = $scope.ngModelProduct == undefined ? '' : $scope.ngModelProduct;
            var transportId = $scope.ngModelTransportType == undefined ? '' : $scope.ngModelTransportType;
            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            var sardarId = $scope.ngModelSardarName == undefined ? '' : $scope.ngModelSardarName;
            //var locationId = $scope.ngModelLocation == undefined ? '' : $scope.ngModelLocation;



            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType + ',' + $scope.ngChkBoxExport;
            var reportPath = repBaseUrl + 'BulkDeliveryFactory/BulkDeliveryCityFactory.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
        };

        $scope.ShowFactorySalesStatement = function () {

            // date issue as per user
            if (objcmnParam.loggeduser === '05624' || objcmnParam.loggeduser === '07648'
                || objcmnParam.loggeduser === '05276' || objcmnParam.loggeduser === '01310') {
                $scope.BackTo3Days = conversion.BackTo3Days();

                var firstDate = conversion.getStringToDate($scope.StartDate);
                var secondDate = conversion.getStringToDate($scope.BackTo3Days);

                firstDate = new Date(firstDate);
                secondDate = new Date(secondDate);

                if (firstDate < secondDate) {
                    Command: toastr["warning"]("You are not authorized to view report more than 3 days !!!");
                    return;
                }
            }
            debugger
            var isExportIncluded = $scope.ngChkBoxExport;
            var reportType = $scope.ReportType;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            var loggeduser = objcmnParam.loggeduser;
            var requistionNo = $scope.ngmRequisitionNumber;
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                //if ($scope.ngModelDistributor == '' || $scope.ngModelDistributor == undefined) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            var productId = $scope.ngModelProduct == undefined ? '' : $scope.ngModelProduct;
            var transportId = $scope.ngModelTransportType == undefined ? '' : $scope.ngModelTransportType;
            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            var sardarId = $scope.ngModelSardarName == undefined ? '' : $scope.ngModelSardarName;
            var locationId = $scope.ngModelLocation == undefined ? '' : $scope.ngModelLocation;

            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType + ',' + isExportIncluded;
            var reportPath = repBaseUrl + 'BulkSalesFactory/BulkSalesCityFactory.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        //**********---- End Clear records ----***************//
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}