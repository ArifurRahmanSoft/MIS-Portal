app.controller('BakersCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
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

            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Customer--' });

            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '12';
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

        function loadProductGroupRecords(isPaging) {

            var apiRoute = sysCommonUrl + 'GetProductGroupRupshiFood/';
            var listProductGroup = crudService.getItemWithcmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listProductGroup.then(function (response) {
                debugger;
                $scope.listProductGroup = response.data.objProdGroupList;
                var prodGroup = $scope.listProductGroup.filter(x => x.SPROG_ID == 'SPROGxxxxxxxxxxxxx86')[0];
                $scope.ngModelProductGroup = prodGroup.SPROG_ID;
                $("#ddlProductGroup").select2("data", { id: prodGroup.SPROG_ID, text: prodGroup.SPROG_NAME });
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadProductGroupRecords(0);

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

            $scope.ngModelProductGroup = 'SPROGxxxxxxxxxxxxx86';
            $("#ddlProductGroup").select2("data", { id: 'SPROGxxxxxxxxxxxxx86', text: 'Bakery' });

            $scope.ngModelSardarName = '';
            $("#ddlSardarName").select2("data", { id: '', text: '--Select Sardar Name--' });

            $scope.StartDate = conversion.NowDateCustom();
            $scope.EndDate = conversion.NowDateCustom();
        };

        $scope.ShowRupshiFoodsDelivery = function () {

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
            var sprogId = $scope.ngModelProductGroup == undefined ? '' : $scope.ngModelProductGroup;
            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + sprogId + ',' + reportType;
            var reportPath = repBaseUrl + 'RupshiFoodsDelivery/RupshiFoodsDelivery.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
        };

        $scope.ShowRupshiFoods = function () {

            var reportType = $scope.ReportType;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            var locationId = $scope.ngModelLocation;

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
            var sprogId = $scope.ngModelProductGroup == undefined ? '' : $scope.ngModelProductGroup;

            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + sprogId + ',' + reportType;
            var reportPath = repBaseUrl + 'BakersSales/BakersSales.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
        };

        $scope.ShowRupFoodPartyLedger = function (reporttype) {
            debugger
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;

            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelDistributor + ',' + startDate + ',' + endDate + ',' + reporttype;

            var reportPath = repBaseUrl + 'RupshiFoodsPartyLedger/RupshiFoodsPartyLedger.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
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

        $scope.ShowRupshiFoodsUndelivery = function () {

            var reportType = $scope.ReportType;
            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            var locationId = $scope.ngModelLocation;
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
            var sprogId = $scope.ngModelProductGroup == undefined ? '' : $scope.ngModelProductGroup;

            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + sprogId + ',' + reportType;
            var reportPath = repBaseUrl + 'RupshiFoodsUndelivery/RupshiFoodsUndelivery.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}