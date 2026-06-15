app.controller('DeliveryCommonCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
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
        $scope.CustomerWiseDeliveryPageTitle = 'Customer Wise Delivery';

        $scope.HoldDataModel = '';

        $scope.listDistTargetMaster = [];
        $scope.showDtgrid = 0;
        $scope.listDistributor = [];
        $scope.listDistributorExport = [];
        $scope.ngModelDistributor = '';

        $scope.ListProducts = [];
        $scope.ngModelProduct = '';
        $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });


        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;

        $scope.IsShowReportButtonDisabled = false;

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
            $scope.listDistributors = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Customer--' });

            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '6';
            var trackNo = 0;

            var listDistributors = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listDistributors.then(function (response) {
                $scope.listDistributors = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadDistributorAll();

        $scope.loadDistributorExport = function () {
            debugger
            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Customer--' });

            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '14';
            var trackNo = 0;

            var listDistributor = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listDistributor.then(function (response) {
                $scope.listDistributor = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadDistributorExport();

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

        $scope.ShowFactoryDelivery = function () {

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
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType;
            var reportPath = repBaseUrl + 'DeliveryStatementFactory/DeliveryStatementFactory.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowExportSales = function () {

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

            var locationId = $scope.ngModelLocation;
            //if (locationId == '' || locationId == undefined) {
            //    Command: toastr["warning"]("Please select location!");
            //    return;
            // }

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
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType;
            var reportPath = repBaseUrl + 'ExportConsumerSales/ExportConsumerSales.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowExportSalesFactory = function () {

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

            var locationId = $scope.ngModelLocation;
            //if (locationId == '' || locationId == undefined) {
            //    Command: toastr["warning"]("Please select location!");
            //    return;
            // }

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
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType;
            var reportPath = repBaseUrl + 'ExportConsumerSalesFactory/ExportConsumerSalesFactory.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowExportDelivery = function () {

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

            var locationId = $scope.ngModelLocation;
            //if (locationId == '' || locationId == undefined) {
            //    Command: toastr["warning"]("Please select location!");
            //    return;
            // }

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
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType;
            var reportPath = repBaseUrl + 'ExportConsumerDelivery/ExportConsumerDelivery.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowExportDeliveryFactory = function () {

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

            var locationId = $scope.ngModelLocation;
            //if (locationId == '' || locationId == undefined) {
            //    Command: toastr["warning"]("Please select location!");
            //    return;
            // }

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
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType;
            var reportPath = repBaseUrl + 'ExportConsumerDeliveryFactory/ExportConsumerDeliveryFactory.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowExportUndelivery = function () {

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

            var locationId = $scope.ngModelLocation;
            //if (locationId == '' || locationId == undefined) {
            //    Command: toastr["warning"]("Please select location!");
            //    return;
            // }

            var loggeduser = objcmnParam.loggeduser;
            var requistionNo = $scope.ngmRequisitionNumber;
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            //var brandId = $scope.ngModelbrandId == undefined ? '' : $scope.ngModelbrandId;
            var productId = $scope.ngModelProduct == undefined ? '' : $scope.ngModelProduct;
            var transportId = $scope.ngModelTransportType == undefined ? '' : $scope.ngModelTransportType;
            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            var sardarId = $scope.ngModelSardarName == undefined ? '' : $scope.ngModelSardarName;
            //var locationId = $scope.ngModelLocation == undefined ? '' : $scope.ngModelLocation;



            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType;
            var reportPath = repBaseUrl + 'ExportConsumerUndelivery/ExportConsumerUndelivery.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.ShowExportUndeliveryFactory = function () {

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

            var locationId = $scope.ngModelLocation;
            //if (locationId == '' || locationId == undefined) {
            //    Command: toastr["warning"]("Please select location!");
            //    return;
            // }

            var loggeduser = objcmnParam.loggeduser;
            var requistionNo = $scope.ngmRequisitionNumber;
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command: toastr["warning"]("Please select From date and To date");
                return;
            }

            var distributorId = $scope.ngModelDistributor == undefined ? '' : $scope.ngModelDistributor;
            //var brandId = $scope.ngModelbrandId == undefined ? '' : $scope.ngModelbrandId;
            var productId = $scope.ngModelProduct == undefined ? '' : $scope.ngModelProduct;
            var transportId = $scope.ngModelTransportType == undefined ? '' : $scope.ngModelTransportType;
            var companyId = $scope.ngModelCompany == undefined ? '' : $scope.ngModelCompany;
            var sardarId = $scope.ngModelSardarName == undefined ? '' : $scope.ngModelSardarName;
            //var locationId = $scope.ngModelLocation == undefined ? '' : $scope.ngModelLocation;



            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + reportType;
            var reportPath = repBaseUrl + 'ExportConsumerUndeliveryFactory/ExportConsumerUndeliveryFactory.aspx?queryString=' + commonQueryString;
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