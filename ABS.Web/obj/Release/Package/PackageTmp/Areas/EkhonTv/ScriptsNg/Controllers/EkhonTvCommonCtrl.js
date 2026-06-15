//Enable Debug Info for Document Upload
app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);

app.controller('EkhonTvCommonCtrl', ['$scope', '$window', 'reportCommonService', 'crudService', '$http', 'conversion', '$filter', 'uiGridConstants', '$localStorage',
    function ($scope, $window, reportCommonService, crudService, $http, conversion, $filter, uiGridConstants, $localStorage) {

        var objcmnParam = {};

        var sysComDdlUrl = '/SystemCommon/api/SystemCommonDDL/';
        //var sysComDdlUrl = '/Areas/EkhonTv/api/EkhonTvCommonDDL/';
        var baseUrl = '/BuildingManagement/api/FlatTenantTagging/';
        var repBase1Url = '/Areas/BuildingManagement/Reports/';
        var repBase2Url = '/Areas/RentCollection/Reports/';
        var repBase3Url = '/Areas/EkhonTv/Reports/';

        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;
        var UserTypeID = 1;  // user  ;

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.invalid = false;

        $scope.IsUpdateMode = false;
        $scope.listSupplierContact = [];

        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();
        $scope.IsListIcon = true;
        $scope.IsCreateIcon = false;
        $scope.Invalid = true;

        $scope.bool = true;
        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";
        $scope.PageTitle = 'Flat Tenant Tagging';

        //Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsShow = true;
        $scope.IsHiddenDetail = true;
        $scope.IsShowD = false;

        $scope.ShowHide = function () {
            $scope.IsShow = $scope.IsShow ? false : true;
            $scope.IsHidden = $scope.IsHidden == true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden == true) {
                $scope.btnShowText = "Show List";
                $scope.IsShow = true;
                $scope.IsShowD = $scope.ListPODetails.length > 0 ? true : false;
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
            }
            else {
                $scope.btnShowText = "Create";
                $scope.IsShow = false;
                $scope.IsHidden = false;
                $scope.IsShowD = false;
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                loadTenantTagMasterRecords(0);
            }
        }

        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmPurchaseOrder'; DelFunc = 'DeleteUpdateMasterDetail'; DelMsg = 'PurchaseOrder'; EditFunc = 'GetTenantTagByID'; // 'GetPOMasterById, GetPODetailById';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        $scope.EventOnTenantChange = function () {
            $scope.ngmCTGStaffId = "";
            $("#ddlCTGStaffId").select2("data", { id: '', text: '--Select CTG Staff--' });
        }

        $scope.EventOnCTGStaffChange = function () {
            $scope.ngmTenantName = "";
            $("#ddlTenantName").select2("data", { id: '', text: '--Select Tenant--' });
        }

        function LoadBuildingRecord() {
            objcmnParam = {
                pageNumber: 1,
                pageSize: 50,
                IsPaging: 0,
                loggeduser: 1,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: 5,
                tTypeId: 25
            };
            var apiRoute = sysComDdlUrl + 'GetBuildingNameDDL/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listBuilding = crudService.GetList(apiRoute, cmnParam);
            listBuilding.then(function (response) {
                $scope.listBuilding = response.data.listBuilding;

            },
                function (error) {
                    console.warn("Error: " + error);
                });
        }
        //LoadBuildingRecord();

        $scope.StartDate = conversion.NowDateCustom();
        $scope.EndDate = conversion.NowDateCustom();
        $scope.ngmCompany = 'SCOMPxxxxxxxxxxxx111';
        function LoadEkhonTvCompany() {
            objcmnParam = {
                pageNumber: 1,
                pageSize: 50,
                IsPaging: 0,
                loggeduser: 1,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: 5,
                tTypeId: 25,
                id: $scope.ngmCompany
            };
            var apiRoute = sysComDdlUrl + 'GetCompanyNameDDL/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listCompany = crudService.GetList(apiRoute, cmnParam);
            listCompany.then(function (response) {
                $scope.listCompany = response.data.listCompany;
                var com = $scope.listCompany.filter(x => x.OID == $scope.ngmCompany)[0];
                $("#ddlCompany").select2("data", { id: com.OID, text: com.SCOMP_NAME });
            },
                function (error) {
                    console.warn("Error: " + error);
                });
        }
        //LoadEkhonTvCompany();

        $scope.listAgent = [];
        $scope.listClient = [];
        $scope.ngmAgent = '';
        $scope.ngmClient = '';
        function LoadEkhonTvAgentClient() {
            $scope.listAgent = [];
            $scope.listClient = [];
            objcmnParam = {
                pageNumber: 1,
                pageSize: 50,
                IsPaging: 0,
                loggeduser: 1,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: 5,
                tTypeId: 25
            };
            var apiRoute = sysComDdlUrl + 'GetAgentClientDDL/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listClient = crudService.GetList(apiRoute, cmnParam);
            listClient.then(function (response) {
                var listAgent = response.data.listAgent;
                $scope.listAgent = listAgent;
                $scope.listClient = listAgent.filter(x => x.CLIENT_TYPE == 'C');
            },
                function (error) {
                    console.warn("Error: " + error);
                });
        }
        LoadEkhonTvAgentClient();

        function LoadCTGStaff() {
            objcmnParam = {
                //pageNumber: $scope.pagination.pageNumber,
                pageNumber: 1,
                pageSize: 50,
                IsPaging: 0,
                loggeduser: 1,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: 5,
                tTypeId: 25
            };
            var apiRoute = sysComDdlUrl + 'GetCTGStaff/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listCTGStaff = crudService.GetList(apiRoute, cmnParam);
            listCTGStaff.then(function (response) {
                $scope.listCTGStaff = response.data.listCTGStaff;
            },
                function (error) {
                    console.warn("Error: " + error);
                });
        }
        //LoadCTGStaff();

        $scope.LoadTenantByBuilding = function (companyid) {

            $scope.listTenant = null;
            $scope.ngmTenantName = "";
            $("#ddlTenantName").select2("data", { id: '', text: '--Select company--' });

            if (buildingid > 0) {
                objcmnParam = {
                    id: buildingid
                };
                var apiRoute = sysComDdlUrl + 'LoadTenantByBuilding/';
                var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
                var buildingWiseTenant = crudService.GetList(apiRoute, cmnParam);
                buildingWiseTenant.then(function (response) {
                    $scope.listTenant = response.data.objTenant;
                },
                    function (error) {
                        console.warn("Error: " + error);
                    });
            }
        }

        // This executes when entity in table is checked
        $scope.selectSingleFloorFlat = function () {
            // If any entity is not checked, then uncheck the "allItemsSelected" checkbox
            for (var i = 0; i < $scope.listFloorFlat.length; i++) {
                if (!$scope.listFloorFlat[i].isChecked) {
                    $scope.ngmallItemsSelected = false;
                    return;
                }
            }
            //If not the check the "allItemsSelected" checkbox
            $scope.ngmallItemsSelected = true;
        };

        // This executes when checkbox in table header is checked
        $scope.selectAllFloorFlat = function () {
            // Loop through all the entities and set their isChecked property
            for (var i = 0; i < $scope.listFloorFlat.length; i++) {
                $scope.listFloorFlat[i].isChecked = $scope.ngmallItemsSelected;
            }
        };

        $scope.gridOptionslistItemMaster = [];
        $scope.modalSearchItemName = "";

        $scope.deleteRow = function (dataModel, index) {
            $scope.ListPODetails.splice(index, 1);
        }

        $scope.ShowTenantInfoReport = function (dataModel) {

            var building = $scope.ngmBuilding === undefined ? '' : $scope.ngmBuilding;

            var queryString = building + ',' + dataModel;

            var reportPath = repBase1Url + 'TenantInformation/TenantInfo.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open('/EKHON/Areas/BuildingManagement/Reports/TenantInformation/TenantInfo.aspx?queryString=' + queryString);
        };

        $scope.ShowMonthlyDepositReport = function () {
            var reportType = $scope.ReportType;

            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
            else {
                if (reportType == 'single_building' || reportType == 'single_building_dues') {
                    if ($('#ddlBuilding').val() == "" || $('#dtFromMonth').val() == "") {
                        Command: toastr["warning"]("Please select building and a month !");
                        return false;
                    }
                }
                else if (reportType == 'entry-date-wise') {
                    if ($('#dtEntryDateFrom').val() == "" || $('#dtEntryDateTo').val() == "") {
                        Command: toastr["warning"]("Please select entry date range !!!");
                        return false;
                    }
                }
                else {
                    if ($('#dtFromMonth').val() == "") {
                        Command: toastr["warning"]("Please select a month !");
                        return false;
                    }
                }
            }

            var building = $scope.ngmBuilding == undefined ? '' : $scope.ngmBuilding;
            var selectedMonth = $scope.ngmFromMonth != (null || undefined) ? conversion.getStringToDateMMYYFormat($scope.ngmFromMonth) : $scope.ngmFromMonth;

            var entryDateFrom = $scope.EntryDateFrom;
            var entryDateTo = $scope.EntryDateTo;


            var queryString = building + ',' + selectedMonth + ',' + reportType + ',' + entryDateFrom + ',' + entryDateTo;

            var reportPath = repBase2Url + 'MonthlyDeposit/MDCollectionForm.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open('/EKHON/Areas/RentCollection/Reports/MonthlyDeposit/MDCollectionForm.aspx?queryString=' + queryString);
        };

        $scope.ShowRentCollectionSummaryReport = function () {

            if ($('#ddlBuilding').val() == "" || $('#dtFromMonth').val() == "") {
                Command: toastr["warning"]("Please select building and a month !");
                return false;
            }
            var selectedMonth = $scope.ngmFromMonth != (null || undefined) ? conversion.getStringToDateMMYYFormat($scope.ngmFromMonth) : $scope.ngmFromMonth;

            var queryString = selectedMonth;

            var reportPath = repBase2Url + 'RentCollectionSummary.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);

            //$window.open('/EKHON/Areas/RentCollection/Reports/RentCollectionSummary.aspx?queryString=' + queryString);
        };

        $scope.ShowTenantRentSetupReport = function () {
            var reportPath = repBase1Url + 'RentSetupInfo.aspx' + queryString;
            conversion.reportOpen(reportPath);
            //$window.open('/EKHON/Areas/BuildingManagement/Reports/RentSetupInfo.aspx');
        };

        /////// Expense Report Section //////////////

        $scope.ShowMonthwiseExpenseReport = function () {

            if ($('#ddlBuilding').val() == "" || $('#dtFromMonth').val() == "") {
                Command: toastr["warning"]("Please select building and a month !");
                return false;
            }

            var building = $scope.ngmBuilding == undefined ? '' : $scope.ngmBuilding;
            var selectedMonth = $scope.ngmFromMonth != (null || undefined) ? conversion.getStringToDateMMYYFormat($scope.ngmFromMonth) : $scope.ngmFromMonth;

            var queryString = building + ',' + selectedMonth;

            var reportPath = repBase3Url + 'MonthlyExpense.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);

            //$window.open('/EKHON/Areas/Expense/Reports/MonthlyExpense.aspx?queryString=' + queryString);
        };

        $scope.ngmSel6 = '';
        $scope.ngmSel7 = '';
        $scope.ShowMediaLedgerReport = function () {
            var reportType = $scope.ReportType;

            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }
           
            //var building = $scope.ngmBuilding == undefined ? '' : $scope.ngmBuilding;
            var compnay = $scope.ngmCompany == undefined ? '' : $scope.ngmCompany;
            var agent = $scope.ngmAgent == undefined ? '' : $scope.ngmAgent;
            var Client = $scope.ngmClient == undefined ? '' : $scope.ngmClient;


            // var selectedMonth = $scope.ngmFromMonth != (null || undefined) ? conversion.getStringToDateMMYYFormat($scope.ngmFromMonth) : $scope.ngmFromMonth;
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command.toastr["warning"]("Please select From date and to date");
            }

            var sel6 = $scope.ngmSel6 == undefined ? '' : $scope.ngmSel6;
            var sel7 = $scope.ngmSel7 == undefined ? '' : $scope.ngmSel7;

            var queryString = compnay + ',' + agent + ',' + Client + ',' + reportType + ',' + startDate + ',' + endDate + ',' + sel6  ;

            var reportPath = repBase3Url + 'MediaLedger/MediaLedger.aspx?queryString=' + queryString;
            //Areas/EkhonTv/Reports/MediaLedger/MediaLedger.aspx
            conversion.reportOpen(reportPath);

            //$window.open('/EKHON/Areas/Expense/Reports/MonthlyExpense.aspx?queryString=' + queryString);
        };
        $scope.ShowCashTransferReport = function () {
            var reportType = $scope.ReportType;

            if (reportType == '' || reportType == undefined) {
                Command: toastr["warning"]("Please check report type !!!");
                return;
            }

            //var building = $scope.ngmBuilding == undefined ? '' : $scope.ngmBuilding;
            var compnay = $scope.ngmCompany == undefined ? '' : $scope.ngmCompany;
            var agent = $scope.ngmAgent == undefined ? '' : $scope.ngmAgent;
            var Client = $scope.ngmClient == undefined ? '' : $scope.ngmClient;


            // var selectedMonth = $scope.ngmFromMonth != (null || undefined) ? conversion.getStringToDateMMYYFormat($scope.ngmFromMonth) : $scope.ngmFromMonth;
            var startDate = $scope.StartDate;
            var endDate = $scope.EndDate;
            if (startDate.length == 0 || endDate.length == 0) {
                Command.toastr["warning"]("Please select From date and to date");
            }

            var sel6 = $scope.ngmSel6 == undefined ? '' : $scope.ngmSel6;
            var sel7 = $scope.ngmSel7 == undefined ? '' : $scope.ngmSel7;

            var queryString = compnay + ',' + agent + ',' + Client + ',' + reportType + ',' + startDate + ',' + endDate + ',' + sel6;

            var reportPath = repBase3Url + 'CashTransfer/CashTransfer.aspx?queryString=' + queryString;
            //Areas/EkhonTv/Reports/MediaLedger/MediaLedger.aspx
            conversion.reportOpen(reportPath);

            //$window.open('/EKHON/Areas/Expense/Reports/MonthlyExpense.aspx?queryString=' + queryString);
        };

        $scope.ShowSecurityMoneyReport = function () {
            var reportType = $scope.ReportType;
            var building = $scope.ngmBuilding == undefined ? '' : $scope.ngmBuilding;
            var selectedMonth = $scope.ngmFromMonth != (null || undefined) ? conversion.getStringToDateMMYYFormat($scope.ngmFromMonth) : $scope.ngmFromMonth;

            var queryString = building + ',' + selectedMonth + ',' + reportType;

            var reportPath = repBase2Url + 'SecurityMoney/SecurityMoney.aspx?queryString=' + queryString;
            conversion.reportOpen(reportPath);

            //$window.open('/EKHON/Areas/RentCollection/Reports/SecurityMoney/SecurityMoney.aspx?queryString=' + queryString);	
        };

        $scope.clear = function () {

            $scope.invalid = false;
            $("#save").prop("disabled", false);

            $scope.IsUpdateMode = false;
            $scope.btnSaveText = "Save";
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;

            $scope.TAGID = 0;
            $scope.ngmPurposeOfRent = "";

            $('#IfDeedCopyHas').prop('checked', false); // uncheck it
            $('#IfDeedCopyNA').prop('checked', false); // uncheck it

            $scope.TENANTID = "0";

            $scope.IsShow = true;
            $scope.IsShowD = false;
            $scope.IsListIcon = true;
            $scope.IsCreateIcon = false;
            $scope.IsHiddenDetail = true;

            $scope.ngmBuilding = "";
            $("#ddlBuilding").select2("data", { id: '', text: '--Select Building--' });

            $scope.ngmCTGStaffId = "";
            $("#ddlCTGStaffId").select2("data", { id: '', text: '--Select CTG Staff--' });

            $scope.listFloorFlat = [];
            $scope.ngmFloorFlat = "";
            $("#ddlFloorFlat").select2("data", { id: '', text: '--Select Floor/Flat--' });

            $scope.listTenant = [];
            $scope.ngmTenantName = "";
            $("#ddlTenantName").select2("data", { id: '', text: '--Select Tenant--' });

            $scope.ngmallItemsSelected = false;

            $scope.ngmClient = '';
            $("#ddlClient").select2("data", { id: '', text: 'Select Client' });

            $scope.ngmAgent = '';
            $("#ddlAgent").select2("data", { id: '', text: 'Select Agent' });
            //var date = new Date();
            //$scope.ngmDateOfBirth = ('0' + date.getDate()).slice(-2) + '/' + ('0' + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();                       
        };
    }]);


function modal_fadeOutApproved() {
    $("#approveNotificationModal").fadeOut(200, function () {
        $('#approveNotificationModal').modal('hide');
    });
}
function modal_fadeOutDeclained() {
    $("#declainedNotificationModal").fadeOut(200, function () {
        $('#declainedNotificationModal').modal('hide');
    });
}