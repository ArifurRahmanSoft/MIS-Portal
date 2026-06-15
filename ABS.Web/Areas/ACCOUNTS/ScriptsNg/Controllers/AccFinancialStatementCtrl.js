//const { debug } = require("../../../../Scripts/export-ui-grid/csv");

app.controller('AccFinancialStatementCtrl', ['$scope', '$http', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants', 'hostDire',
    function ($scope, $http, crudService, conversion, $filter, $localStorage, uiGridConstants, hostDire) {
        //**************************************************Start Vairiable Initialize**************************************************
        //var baseUrl = '/ACCOUNTS/api/ProductConsting/';

        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsDTML = [];
        $scope.gridOptionslistItemMaster = [];

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.IsbtSaveReviseShow = true;

        $scope.StartDate = conversion.NowDateCustom();
        $scope.EndDate = conversion.NowDateCustom();

        

        var objcmnParam = {};
        objcmnParam = {};

        $scope.bool = true;
        $scope.IncentiveFormulaSetupID = "0";

        $scope.btnPISaveText = "Save";
        $scope.btnPIShowText = "Show List";
        $scope.btnPIReviseText = "Update";
        $scope.PageTitle = 'General Ledger Comparison';
        $scope.ListTitle = 'General Ledger Comparison List';


        $scope.showDtgrid = 0;
        $scope.listDistributor = [];
        $scope.ngModelDistributor = '';

        $scope.IsHidden = true;
        $scope.IsShow = true;

        $scope.IsHiddenDetail = false;
        $scope.ngModelCompany = '';
       // $scope.ngModelCCompany = '';

        $scope.StartDate = conversion.NowDateCustom();
        $scope.EndDate = conversion.NowDateCustom();
        $scope.ngModelFromYear = '';
       

        $scope.ReportMode = 'monthly';
        $scope.ReportCategory = 'general';
        $scope.ReportType = 'level1';

        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmAccGenLedgerComparison'; DelFunc = 'DeleteAccGenLedgerComparison'; DelMsg = 'IncentiveFormulaSetupNo'; EditFunc = 'loadPIMasterDetailsByActivePI';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        


      

    
        $scope.currentYear = conversion.loadCurrentYear();
       

        
        



        $scope.cYear = new Date().getFullYear();
        //Load Current Month
        $scope.loadCurrentMonth = function (year) {
            $scope.currentMonths = conversion.loadCurrentMonth(year);
        }
        $scope.loadCurrentMonth($scope.cYear)

        //Load End Month
        $scope.loadEndMonth = function (year) {
            $scope.endMonths = conversion.loadEndMonth(year);
        }
        $scope.loadEndMonth($scope.cYear)
       
       


        // Get User-Wise Company list - which has given permission
   /*     $scope.UserWiseCompanyList = function () {
           
            $scope.cmnParam();
            var param = { loggeduser: objcmnParam.loggeduser };
            var apiRoute = sysCommonUrl + 'getUserWiseMCompanyList/';
            var listModel = crudService.getListByParam(apiRoute, param);
            listModel.then(function (response) {
                $scope.listUserWiseCompany = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.UserWiseCompanyList();
*/
    /*    $scope.listUserWiseCCompany = [];

        $scope.UserWiseCCompanyList = function () {
            if ($scope.ngModelCompany != null && $scope.ngModelCompany != undefined && $scope.ngModelCompany != '') {
                $scope.cmnParam();
                var param = { loggeduser: objcmnParam.loggeduser, mcom_oid: $scope.ngModelCompany };
                var apiRoute = sysCommonUrl + 'getUserWiseCCompanyList/';
                var listModel = crudService.getListByParam(apiRoute, param);
                listModel.then(function (response) {
                    $scope.listUserWiseCCompany = response.data;
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
        }*/


        // Get User-Wise Company list - which has given permission
        $scope.loadLocation = function () {
            $scope.cmnParam();
            debugger
            $scope.ListLocation = [];
            $scope.ngModelocation = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });
            var apiRoute = sysCommonUrl + 'GetLocation/';
            var userId = objcmnParam.loggeduser;
            var LocationList = crudService.getObjList(apiRoute, userId);
            LocationList.then(function (response) {
                debugger
                $scope.ListLocation = response.data;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        $scope.loadLocation();


        $scope.loadMasterCompany = function () {
            $scope.ListMstrCompany = [];
            $scope.ngModelocation = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });
            var apiRoute = sysCommonUrl + 'GetMasterCompany/';
            var userId = objcmnParam.loggeduser;
            var MCompanyList = crudService.getObjList(apiRoute, userId);
            MCompanyList.then(function (response) {
                $scope.ListMstrCompany = response.data;
                console.log("Child Company is ", $scope.ListMstrCompany)
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        $scope.loadMasterCompany();


        $scope.loadChildCompany = function () {
            $scope.ListChildCompany = [];
            $scope.ngModelocation = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });
            var apiRoute = sysCommonUrl + 'GetChildCompany/';
            //var userId = objcmnParam.loggeduser;
            var param = { loggeduser: objcmnParam.loggeduser, strId: $scope.ngModelMCompany  };
            var headerToken = { }
            var cCompanyList = crudService.getItemWithcmnParam(apiRoute, param, headerToken);
            cCompanyList.then(function (response) {
                debugger
                $scope.ListChildCompany = response.data.objChildCompanyList;
                console.log("Child Company is ", $scope.ListChildCompany)
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };

        $scope.loadSubHead = function () {
            debugger
            $scope.ListSubHead = [];
            $scope.ngModelocation = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });
            var apiRoute = sysCommonUrl + 'GetSubHead/';
            //var userId = objcmnParam.loggeduser;
            var param = { loggeduser: objcmnParam.loggeduser, strId: $scope.ngModelMCompany };
            var headerToken = {}
            var subHeadList = crudService.getItemWithcmnParam(apiRoute, param, headerToken);
            subHeadList.then(function (response) {
                debugger
                console.log("Sub Head Company is ", response)
                $scope.ListSubHead = response.data.objSubHeadList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
       // $scope.loadChildCompany();
        
        $scope.GetGroupHead = function () {
            $scope.ListGrpHead = [];
            $scope.ngModelocation = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });
            var apiRoute = sysCommonUrl + 'GetGroupHead/';
            var userId = objcmnParam.loggeduser;
            var GroupHeadList = crudService.getObjList(apiRoute, userId);
            GroupHeadList.then(function (response) {
                $scope.ListGrpHead = response.data;
                console.log("Sub  $scope.ListGrpHead is ", $scope.ListGrpHead)
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        $scope.GetGroupHead();


        $scope.loadAcountHead = function () {
            debugger
            $scope.ListAcHead = [];
            $scope.ngModelocation = '';
            $("#ddlProduct").select2("data", { id: '', text: '--Select Product--' });
            var apiRoute = sysCommonUrl + 'GetAcHead/';
            //var userId = objcmnParam.loggeduser;
            var param = {
                loggeduser: objcmnParam.loggeduser,
                strId: $scope.ngModelCCompany,
                strId2: $scope.ngModelSubHead,
                strId3: $scope.ngModelAccGrpHead
            };
            var headerToken = {}
            var AcHeadList = crudService.getItemWithcmnParam(apiRoute, param, headerToken);
            AcHeadList.then(function (response) {
                debugger
                
                $scope.ListAcHead = response.data.objAcHeadList;
                console.log("Child  $scope.ListAcHead is ", $scope.ListAcHead)
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };




        $scope.ShowReport = function () {
            debugger
            if ($scope.ReportType == 'level1') {
                Command: toastr["warning"]("Please select Report Type !!!");
                return;
            }

            var locationId = $scope.ngModelLocation == '' || $scope.ngModelLocation == undefined || $scope.ngModelLocation == null ? '' : $scope.ngModelLocation;
            var mCompany = $scope.ngModelMCompany == '' || $scope.ngModelMCompany == undefined || $scope.ngModelMCompany == null ? '' : $scope.ngModelMCompany;
            var fromYear = $scope.ngModelFromYear == '' || $scope.ngModelFromYear == undefined || $scope.ngModelFromYear == null ? '' : $scope.ngModelFromYear;
            var toYear = $scope.ngModelToYear == '' || $scope.ngModelToYear == undefined || $scope.ngModelToYear == null ? '' : $scope.ngModelToYear;
            var fromMonth = $scope.ngModelFromMonth == '' || $scope.ngModelFromMonth == undefined || $scope.ngModelFromMonth == null ? '' : $scope.ngModelFromMonth;
            var toMonth = $scope.ngModeltoMonth == '' || $scope.ngModeltoMonth == undefined || $scope.ngModeltoMonth == null ? '' : $scope.ngModeltoMonth;
            var fromDateStr = fromMonth + '/' + fromYear; 
            var toDateStr = toMonth + '/' + toYear;       

            function parseDate(dateStr) {
                var parts = dateStr.split('/');
                return new Date(parts[1] + '/' + parts[0] + '/' + parts[2]); 
            }

            var fromDate = parseDate(fromDateStr);
            var toDate = parseDate(toDateStr); 

            if (!$scope.ngModelMCompany || !$scope.ngModelFromYear || !$scope.ngModelToYear || !$scope.ngModelFromMonth || !$scope.ngModeltoMonth) {
                Command: toastr["warning"]("pleaese Select All Required Field !!!");
                return;
            }
            if (fromDate > toDate) {
                Command: toastr["warning"]("From Date must not be greater than To Date !!!");
                return;
            }
            var queryStringUDS = locationId + ',' + mCompany + ',' + fromYear + ',' + toYear + ',' + fromMonth + ',' + toMonth + ',' + $scope.ReportType;
            var reportPath = '/Areas/ACCOUNTS/Reports/FinancialStatement/FinancialStatement.aspx?queryString=' + queryStringUDS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };



        $scope.clear = function () {
          
            $scope.ngModelLocation = '';
            $("#ddlLocation").select2("data", { id: '', text: '--Select Location--' });

            $scope.ngModelMCompany = '';
            $("#ddlMCompany").select2("data", { id: '', text: '--Select Account Group--' });

            $scope.ngModelCCompany = '';
            $("#ddlCCompany").select2("data", { id: '', text: '--Select Company--' });

            $scope.ngModelSubHead = '';
            $("#ddlLocProject").select2("data", { id: '', text: '--Select Loc/Project--' });
            //$scope.listlvl2 = [];

            $scope.ngModelAccGrpHead = '';
            $("#ddlAcHead").select2("data", { id: '', text: '--Select Group Head--' });
            //$scope.listlvl3 = [];

            $scope.ngModelAccHead = '';
            $("#ddlAcCode").select2("data", { id: '', text: '--Select A/C Head--' });
            

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