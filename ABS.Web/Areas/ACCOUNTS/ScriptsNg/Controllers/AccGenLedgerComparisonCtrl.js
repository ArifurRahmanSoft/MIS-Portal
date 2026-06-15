app.controller('AccGenLedgerComparisonCtrl', ['$scope', '$http', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants', 'hostDire',
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
        $scope.ngModelCCompany = '';

        $scope.StartDate = conversion.NowDateCustom();
        $scope.EndDate = conversion.NowDateCustom();

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


        // Get User-Wise Company list - which has given permission
        $scope.UserWiseCompanyList = function () {
      
            $scope.cmnParam();
            var param = { loggeduser: objcmnParam.loggeduser };
            var apiRoute = sysCommonUrl + 'getUserWiseMCompanyList/';
            var listModel = crudService.getListByParam(apiRoute, param);
            listModel.then(function (response) {
                $scope.listUserWiseCompany = response.data;
                console.log(" $scope.listUserWiseCompany", $scope.listUserWiseCompany)
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.UserWiseCompanyList();

        $scope.listUserWiseCCompany = [];

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
        }

        $scope.resetLvl = function () {
            if ($scope.lvlId1 == null) {
                $scope.lvlId2 = null;
                $("#lvlId2").select2("data", { id: '', text: '--Select Level 1--' });
                $scope.listlvl2 = [];

                $scope.lvlId3 = null;
                $("#lvlId3").select2("data", { id: '', text: '--Select Level 2--' });
                $scope.listlvl3 = [];

                $scope.lvlId4 = null;
                $("#lvlId4").select2("data", { id: '', text: '--Select Level 3--' });
                $scope.listlvl4 = [];
            } else if ($scope.lvlId2 == null) {
                $scope.lvlId3 = null;
                $("#lvlId3").select2("data", { id: '', text: '--Select Level 2--' });
                $scope.listlvl3 = [];

                $scope.lvlId4 = null;
                $("#lvlId4").select2("data", { id: '', text: '--Select Level 3--' });
                $scope.listlvl4 = [];
            } else if ($scope.lvlId3 == null) {
                $scope.lvlId4 = null;
                $("#lvlId4").select2("data", { id: '', text: '--Select Level 3--' });
                $scope.listlvl4 = [];
            }
        };

        $scope.lvlId1 = null; $scope.lvlId2 = null; $scope.lvlId3 = null; $scope.lvlId4 = null;
        $scope.listlvl1 = [];
        $scope.listlvl2 = [];
        $scope.listlvl3 = [];
        $scope.listlvl4 = [];
        $scope.loadAllLevel = function (fname) {
            $scope.resetLvl();
            $scope.cmnParam();
            var param = [{ FieldName: fname, strId1: $scope.lvlId1, strId2: $scope.lvlId2, strId3: $scope.lvlId3, strId4: $scope.lvlId4, loggeduser: objcmnParam.loggeduser }];
            var apiRoute = sysCommonUrl + 'getalllevel/';
            var listModel = crudService.postMultipleModel(apiRoute, param, $scope.HeaderToken.get);
            listModel.then(function (response) {
                debugger;
                if (fname == '1') {
                    $scope.listlvl1 = response.data.lvlList;
                } else if (fname == '2') {
                    $scope.listlvl2 = response.data.lvlList;
                } else if (fname == '3') {
                    $scope.listlvl3 = response.data.lvlList;
                } else if (fname == '4') {
                    $scope.listlvl4 = response.data.lvlList;
                }


            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadAllLevel('1',);

        $scope.ShowReport = function () {
            var comId = $scope.ngModelCompany == '' || $scope.ngModelCompany == undefined || $scope.ngModelCompany == null ? '' : $scope.ngModelCompany;
            var ccomId = $scope.ngModelCCompany == '' || $scope.ngModelCCompany == undefined || $scope.ngModelCCompany == null ? '' : $scope.ngModelCCompany;
            //if (comId == '') {
            //    toastr.warning("Please select company!!!!");
            //    return;
            //}

            var lvlId1 = $scope.lvlId1 == null ? '' : $scope.lvlId1;
            var lvlId2 = $scope.lvlId2 == null ? '' : $scope.lvlId2;
            var lvlId3 = $scope.lvlId3 == null ? '' : $scope.lvlId3;
            var lvlId4 = $scope.lvlId4 == null ? '' : $scope.lvlId4;

            var queryStringUDS = comId + ',' + ccomId + ',' + $scope.StartDate + ',' + $scope.EndDate + ',' + $scope.ReportCategory + ',' + $scope.ReportMode + ',' + lvlId1+ ',' + lvlId2 + ',' + lvlId3 + ',' + lvlId4 + ',' + $scope.ReportType;

            var reportPath = '/Areas/ACCOUNTS/Reports/GeneralLedgerComparison/GeneralLedgerComparison.aspx?queryString=' + queryStringUDS;
            conversion.reportOpen(reportPath);
            //$window.open(reportPath);
        };

        $scope.clear = function () {
            $scope.ngModelCompany = '';
            $("#ddlCompany").select2("data", { id: '', text: '--Select Company--' });

            $scope.ngModelCCompany = '';
            $("#ddlCCompany").select2("data", { id: '', text: '--Select Company--' });

            $scope.lvlId1 = null;
            $("#lvlId1").select2("data", { id: '', text: '--Select Level--' });

            $scope.lvlId2 = null;
            $("#lvlId2").select2("data", { id: '', text: '--Select Level 1--' });
            $scope.listlvl2 = [];

            $scope.lvlId3 = null;
            $("#lvlId3").select2("data", { id: '', text: '--Select Level 2--' });
            $scope.listlvl3 = [];

            $scope.lvlId4 = null;
            $("#lvlId4").select2("data", { id: '', text: '--Select Level 3--' });
            $scope.listlvl4 = [];

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