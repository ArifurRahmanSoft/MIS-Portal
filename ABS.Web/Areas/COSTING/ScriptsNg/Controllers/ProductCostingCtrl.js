app.controller('ProductCostingCtrl', ['$scope', '$http', 'crudService', 'conversion', '$filter', '$localStorage', 'uiGridConstants', 'hostDire',
    function ($scope, $http, crudService, conversion, $filter, $localStorage, uiGridConstants, hostDire) {
        //**************************************************Start Vairiable Initialize**************************************************
        var baseUrl = '/COSTING/api/ProductConsting/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsDTML = [];
        $scope.gridOptionslistItemMaster = [];

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;
        $scope.IsbtSaveReviseShow = true;
        $scope.IsDOCompleted = "";
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        $scope.StartDate = conversion.NowDateCustom();
        $scope.EndDate = conversion.NowDateCustom();

        var objcmnParam = {};
        objcmnParam = {};

        $scope.bool = true;
        $scope.IncentiveFormulaSetupID = "0";

        $scope.btnPISaveText = "Save";
        $scope.btnPIShowText = "Show List";
        $scope.btnPIReviseText = "Update";
        $scope.PageTitle = 'Product Costing Entry';
        $scope.ListTitle = 'Product Costing Records';
        $scope.ListTitleActivePIMasters = 'Product Costing Information';
        $scope.ListTitleSampleNo = 'Product Costing';
        $scope.ListTitlePIDeatails = 'Listed Item of Product Costing';

        $scope.HoldDataModel = '';

        $scope.ListIncentiveFormulaSetupDetails = [];
        $scope.listDistTargetMaster = [];
        $scope.listIncentiveFormulaSetupMaster = [];
        $scope.ListIncentiveDistRatio = [];
        $scope.ListAchievementRatioDetail = [];
        $scope.showDtgrid = 0;
        $scope.listDistributor = [];
        $scope.ngModelDistributor = '';

        $scope.IsHidden = true;
        $scope.IsShow = true;

        $scope.IsHiddenDetail = false;
        $scope.SCOMP_SACOM = '';


        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmIncentiveFormulaSetup'; DelFunc = 'DeleteIncentiveFormulaSetupMasterDetail'; DelMsg = 'IncentiveFormulaSetupNo'; EditFunc = 'loadPIMasterDetailsByActivePI';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        


        // Get All Company list
        $scope.GetAllCompany = function () {
            mCUID = $scope.ngModelCompany;
            $http.get(hostDire + "/COSTING/api/ProductConsting/getAllCompanyList").then(function (d) {
                $scope.listCompany = d.data;
            }, function (error) {
                alert("Failed");
            })
        };
        $scope.GetAllCompany();

        $scope.LoadAllProduct = function () {

            $scope.listProduct = [];


            angular.forEach($scope.listCompany, function (value, key) {
                debugger

                if (value.SCOMP_OID == $scope.ngModelCompany) {
                    $scope.SCOMP_SACOM = value.SCOMP_SACOM;
                }
            });

            debugger
            objcmnParam = {
                pageNumber: 1,
                pageSize: 50,
                IsPaging: 0,
                loggeduser: 1,
                loggedCompany: 1,
                menuId: 5,
                tTypeId: 25,
                parameter: $scope.ngModelCompany,
                parameter1: $scope.SCOMP_SACOM
            };
            var apiRoute = baseUrl + 'LoadAllProduct/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listProduct = crudService.GetList(apiRoute, cmnParam);
            listProduct.then(function (response) {
                $scope.listProduct = response.data.listProduct;
            },
                function (error) {
                    console.warn("Error: " + error);
                });
        }
        $scope.EditAction = function (event,index, dataModel) {
            debugger
            if (event.keyCode == 13) {   // '13' is the key code for enter
                // do what you want to do when 'enter' is pressed :)
                //Command: toastr["success"]("asdfgasdfasdf");
                $scope.Edit(index, dataModel);
            }
        
        };

        $scope.Edit = function (index, dataModel) {

            debugger
           
                objcmnParam = {
                    pageNumber: page,
                    pageSize: pageSize,
                    IsPaging: isPaging,
                    loggeduser: $scope.UserCommonEntity.loggedUserID,
                    loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                    menuId: $scope.UserCommonEntity.currentMenuID,
                    tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
                };
                
            var HedarTokenPostPut = $scope.LV_LIGHT_DRAFT_SURVEY_OID != '' ? $scope.HeaderToken.put : $scope.HeaderToken.post;
            debugger
                var costingdata = {
                    COST_SCOMP: $scope.ngModelCompany,
                    COST_SPROD: dataModel.OID,
                    COST_RATE: dataModel.SPROD_COST_RATE,
                    CreateBy: $scope.UserCommonEntity.loggedUserID,                  
                    IsDeleted: false
            };
            
            var apiRoute = baseUrl + 'SaveProductCostingRate/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(costingdata) + "]";

                var DocumentCreateUpdate = crudService.GetList(apiRoute, cmnParam, HedarTokenPostPut);
                DocumentCreateUpdate.then(function (response) {
                    debugger
                    if (response.data > 0) {
                            Command: toastr["success"]("Product Costing Information Save successfully.");
                        $scope.clear();
                    }
                    else {
                        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    }
                },
                    function (error) {
                        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                        console.log("Error: " + error);
                    });
            
        };

        // Delete from list while input
        $scope.deleteRow = function (index, dataModel) {
            $scope.ListAchievementRatioDetail.splice(index, 1);
            $scope.showDtgrid = $scope.ListAchievementRatioDetail.length;
        };
        
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}