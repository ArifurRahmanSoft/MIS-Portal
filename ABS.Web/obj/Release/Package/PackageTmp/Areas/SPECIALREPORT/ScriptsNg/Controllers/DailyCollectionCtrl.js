app.controller('DailyCollectionCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, $http, $window, crudService, conversion, $filter, $localStorage, uiGridConstants) {

        //*************************************************Start Vairiable Initialize**************************************************
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        var costingUrl = '/COSTING/api/ProductConsting/';
        var repBaseUrl = '/Areas/SPECIALREPORT/Reports/';

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
        
        // Get All Company list
        //$scope.GetAllCompany = function () {
        //    $http.get(rootUrl + "/COSTING/api/ProductConsting/getAllCompanyList").then(function (d) {
        //        $scope.listCompany = d.data;
        //    }, function (error) {
        //    })
        //};
        //$scope.GetAllCompany();
        $scope.GetAllCompany = function () {
            var apiRoute = costingUrl + 'getAllCompanyList/';
            var listModel = crudService.getAllList(apiRoute);
            listModel.then(function (response) {
                $scope.listCompany = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.GetAllCompany();



        // Get Transaction Mode
        //$scope.TransactionModeList = function () {
        //    $http.get(rootUrl + "/SystemCommon/api/SystemCommonDDL/getTransactionMode").then(function (d) {
        //        $scope.listTranMode = d.data;
        //    }, function (error) {
        //    });
        //};
        //$scope.TransactionModeList();

        $scope.TransactionModeList = function () {
            var apiRoute = sysCommonUrl + 'getTransactionMode/';
            var listModel = crudService.getAllList(apiRoute);
            listModel.then(function (response) {
                $scope.listTranMode = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.TransactionModeList();



        // Get Post Mode
        //$scope.PostModeList = function () {
        //    $http.get(rootUrl + "/SystemCommon/api/SystemCommonDDL/getPostMode").then(function (d) {
        //        $scope.listPostMode = d.data;
        //    }, function (error) {
        //    });
        //};
        //$scope.PostModeList();

        $scope.PostModeList = function () {
            var apiRoute = sysCommonUrl + 'getPostMode/';
            var listModel = crudService.getAllList(apiRoute);
            listModel.then(function (response) {
                $scope.listPostMode = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.PostModeList();




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


        
        $scope.ShowDailyCollection = function () {    

            if ($scope.ngModelTranMode === '' || $scope.ngModelTranMode === undefined) {
                Command: toastr["warning"]("Please transaction mode !!!");
                return;
            }

            if ($scope.ngModelPostMode === '' || $scope.ngModelPostMode === undefined) {
                Command: toastr["warning"]("Please select Post Mode !!!");
                return;
            }
            
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;

            var company = $scope.ngModelCompany;
            var customer = $scope.ngModelDistributor;
            var location = $scope.ngModelLocation;
            var tranmode = $scope.ngModelTranMode;
            var postmode = $scope.ngModelPostMode;
            debugger

            var queryString = company + ',' + customer + ',' + location + ',' + tranmode + ',' + postmode + ',' + startDate + ',' + endDate;

            var reportPath = repBaseUrl + 'DailyCollection/DailyCollection.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };          

        $scope.ClearDailyCollection = function () {
            $scope.IsCreateIcon = false;
            $scope.IsListIcon = true;

            $scope.IsHidden = true;
            $scope.IsShow = true;
            $scope.IsHiddenDetail = true;

            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            $scope.ngModelDistributor = '';
            $("#ddlDistributor").select2("data", { id: '', text: '--Select Customer--' });

            $scope.ngModelLocation = '';
            $("#ddlLocation").select2("data", { id: '', text: '--Select Location--' });

            $scope.ngModelTranMode = '';
            $("#ddlTranMode").select2("data", { id: '', text: '--Select Mode--' });

            $scope.ngModelPostMode = '';
            $("#ddlPostMode").select2("data", { id: '', text: '--Post Confirm?--' });                       

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