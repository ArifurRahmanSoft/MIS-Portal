app.controller('CustomerWiseDeliveryCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
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

        $scope.ListDistributorTargetDetails = [];
        $scope.listDistTargetMaster = [];
        $scope.showDtgrid = 0;
        $scope.listDistributor = [];
        $scope.ngModelDistributor = '';

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
            $scope.listDistributor = [];
            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Customer--' });

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
        $scope.loadDistributorAll();

        // Get all Product
        $scope.loadProductAll = function () {
            debugger
            $scope.ListProducts = [];
            $scope.ngModelProduct = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });

            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '7';
            var trackNo = 0;

            var ListProducts = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            ListProducts.then(function (response) {
                $scope.ListProducts = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadProductAll();


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



        // Get all Transport
        $scope.loadTransportAll = function () {
            debugger
            $scope.listTransportType = [];
            $scope.ngModelTransportType = '';
            $("#ddlTransportType").select2("data", { id: '', text: '--Select Transport--' });

            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '8';
            var trackNo = 0;

            var listTransportType = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listTransportType.then(function (response) {
                $scope.listTransportType = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadTransportAll();



        // Get all Sardar Name
        $scope.loadSardarAll = function () {
            debugger
            $scope.listSardarName = [];
            $scope.ngModelSardarName = '';
            $("#ddlSardarName").select2("data", { id: '', text: '--Select Sardar Name--' });

            var apiRoute = sysCommonUrl + 'GetSSalesAreaHierarchyList/';
            var mode = '10';
            var trackNo = 0;

            var listTransportType = crudService.getMultipleParameter(apiRoute, mode, trackNo, page, pageSize, isPaging, $scope.HeaderToken.get);
            listTransportType.then(function (response) {
                $scope.listSardarName = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadSardarAll();



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
                $scope.listCompany = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.CityCompanyList();


        // Get User-Wise Company list  
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


        $scope.clearConsumerPartyList = function () {
            $scope.ngModelProductTypeSSTYP = '';
            $("#ddlProductTypeSSTYP").select2("data", { id: '', text: '--Select Product Type--' });
        };

        $scope.ShowCustomerWiseDelivery = function () {

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
            var loggeduser = objcmnParam.loggeduser;
            var requistionNo = $scope.ngmRequisitionNumber;

            debugger

            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;

            if ($scope.StartDateTime != undefined && $scope.StartDateTime != '' && $scope.StartDateTime != null
                && $scope.EndDateTime != undefined && $scope.EndDateTime != '' && $scope.EndDateTime != null) {
                var startDateTime = $scope.getDateTimeToString($scope.StartDateTime);
                var endDateTime = $scope.getDateTimeToString($scope.EndDateTime);
            }


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
            debugger
            var timeordate = $scope.IsTime == true ? 'Yes' : 'No';


            var commonQueryString = distributorId + ','
                + productId + ',' + transportId + ',' + startDate + ',' + endDate + ',' + requistionNo + ','
                + companyId + ',' + loggeduser + ',' + sardarId + ',' + locationId + ',' + startDateTime + ',' + endDateTime + ',' + timeordate;

            var reportPath = repBaseUrl + 'CustomerWiseDelivery/CustomerWiseDelivery.aspx?queryString=' + commonQueryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath)
        };

        $scope.getDateTimeToString = function (nDate) {
            var date = nDate;
            return Nowdate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear() + ' ' + date.getHours() + ':' + date.getMinutes();
            //+ ':' + ('0' + date.getSeconds()).slice(-2)
        }

        $scope.IsTime = false;
        $scope.TimeSelect = function () {
            debugger;
            $scope.IsTime = $scope.IsTime ? false : true;
        }

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


            $scope.StartDateTime = '';
            $scope.EndDateTime = '';

            $scope.ngmRequisitionNumber = '';
        };

        //**********---- End Clear records ----***************//
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}