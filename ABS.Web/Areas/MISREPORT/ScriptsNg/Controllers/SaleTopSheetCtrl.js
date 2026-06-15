app.controller('SaleTopSheetCtrl', ['$scope', '$http', '$window', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, $http, $window, crudService, conversion, $filter, $localStorage, uiGridConstants) {

        //**************************************************Start Vairiable Initialize**************************************************
        var repBaseUrl = '/Areas/MISREPORT/Reports/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.IsbtSaveReviseShow = true;

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

        $scope.HideMYComparison = true; // default hide thakbe
        debugger
        if ($scope.UserCommonEntity.loggedUserID == "02343" || $scope.UserCommonEntity.loggedUserID == "08635" || $scope.UserCommonEntity.loggedUserID == "01227" || $scope.UserCommonEntity.loggedUserID == "01770") {
            $scope.HideMYComparison = false;
        }                   

        $scope.SaleDepositTopSheet = function (data) {
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + startDate + ',' + endDate + ',' + data;

            var reportPath = repBaseUrl + 'SaleTopSheet/SaleTopSheet.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };  

        $scope.SaleDepositTopSheetExcel = function () {
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            var queryString = $scope.ngModelCompany + ',' + $scope.ngModelLocation + ',' + startDate + ',' + endDate;

            var reportPath = repBaseUrl + 'SaleTopSheet/SaleTopSheetExcel.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };  


        $scope.clearSaleDepositTopSheet = function () {
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
       
        
        //**********---- End Clear records ----***************//
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}