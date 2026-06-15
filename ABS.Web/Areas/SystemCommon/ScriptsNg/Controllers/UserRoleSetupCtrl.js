app.controller('userRoleSetupCtrl', ['$scope', 'crudService', 'uiGridConstants', '$q', '$http', 'conversion', '$filter', '$localStorage',
    function ($scope, crudService, uiGridConstants, $q, $http, conversion, $filter, $localStorage) {
        var baseUrl = '/SystemCommon/api/UserRoleSetup/';
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptions = [];        
        var LoginUserID = $('#hUserID').val();
        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";
        $scope.PageTitle = 'User Role Entry';
        $scope.ListTitle = 'User Role Records';

        $scope.USERROLEID = 0;
        $scope.listUser = [];
        $scope.USERID = '';
        $scope.listRole = [];
        $scope.ROLEID = 0;


        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmUserRole'; DelFunc = 'delete'; DelMsg = 'USERROLE'; EditFunc = 'edit';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        
        
        // Get all role        
        $scope.loadRoleRecords = function () {
            var cmnParam = { id: 0 };
            $scope.listRole = [];
            $("#ROLEID").select2("data", { id: '', text: '--Select Role--' });
            var apiRoute = sysCommonUrl + 'getrole/';
            var roleRecords = crudService.postMultipleModels(apiRoute, cmnParam, $scope.HeaderToken.get);
            roleRecords.then(function (response) {
                $scope.listRole = response.data.role;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadRoleRecords();

        // Get all user        
        $scope.loadUserRecords = function () {
            var cmnParam = { id: 0 };
            $scope.listUser = [];
            $("#USERID").select2("data", { id: '', text: '--Select User--' });
            var apiRoute = sysCommonUrl + 'getusers/';
            var userRecords = crudService.postMultipleModels(apiRoute, cmnParam, $scope.HeaderToken.get);
            userRecords.then(function (response) {
                $scope.listUser = response.data.usr;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadUserRecords();

        $scope.loadUserRoleList = function (isPaging) {
            $scope.cmnParam();
            $scope.gridOptions.enableFiltering = true;

            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";            

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptions = {
                rowTemplate: $scope.UserCommonEntity.rowTemplate,
                columnDefs: [
                    { name: "USERROLEID", displayName: "USERROLEID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ROLENAME", displayName: "ROLE NAME", width: "40%", title: "ROLE NAME", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "USERNAME", displayName: "USER NAME", width: "45%", title: "USER NAME", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Option',
                        displayName: "Option",
                        width: '15%',
                        pinnedRight: true,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.visible,
                        cellTemplate: $scope.UserCommonEntity.cellTemplate
                    }
                ],

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'userRole.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "User Role Target", style: 'headerStyle' },
                exporterPdfFooter: function (currentPage, pageCount) {
                    return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
                },
                exporterPdfCustomFormatter: function (docDefinition) {
                    docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
                    docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
                    return docDefinition;
                },
                exporterPdfOrientation: 'portrait',
                exporterPdfPageSize: 'LETTER',
                exporterPdfMaxGridWidth: 500,
                exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            };
            var apiRoute = baseUrl + 'getbypage/';
            var userRoleRecords = crudService.postMultipleModels(apiRoute, objcmnParam, $scope.HeaderToken.get);
            userRoleRecords.then(function (response) {
                debugger;
                //$scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptions.data = response.data.resdata;                
                $scope.loaderMore = false;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };

        $scope.loadUserRoleList(0);

        $scope.edit = function (dataModel) {
            $scope.btnSaveText = "Update";
            $scope.USERROLEID = dataModel.USERROLEID;
            $scope.USERID = dataModel.USERID;
            $("#USERID").select2("data", { id: dataModel.USERID, text: dataModel.USERNAME });

            $scope.ROLEID = dataModel.ROLEID;
            $("#ROLEID").select2("data", { id: dataModel.ROLEID, text: dataModel.ROLENAME });
        }        

        // Save master detail data
        $scope.Save = function () {
            //$scope.cmnParam();
            var master = {
                USERROLEID: $scope.USERROLEID,
                ROLEID: $scope.ROLEID,
                USERID: $scope.USERID,
                CREATEBY: LoginUserID
            };

            var HeaderTokenPutPost = $scope.USERROLEID == 0 ? $scope.HeaderToken.post : $scope.HeaderToken.put;
            var ModelsArray = [master];
            var apiRoute = baseUrl + 'saveupdate/';
            var DistTargetSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            DistTargetSaveUpdate.then(function (response) {
                if (response != "") {
                    $scope.clear();
                    $scope.loadUserRoleList(0);
                    ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                }
                else if (response == "") {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                }
            },
                function (error) {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                });
        };

        // Delete master detail record
        $scope.delete = function (dataModel) {
            $scope.cmnParam();
            objcmnParam.id = dataModel.USERROLEID;
            var ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'delete/';
            var delDistributorTarget = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delDistributorTarget.then(function (response) {
                if (response.data.result != "") {
                    $scope.loadUserRoleList(0);
                    Command: toastr["success"](dataModel.DistributorTargetNo + " has been Deleted Successfully!!!!");
                }
                else {
                    Command: toastr["warning"](dataModel.DistributorTargetNo + " not Deleted, Please Check and Try Again!");
                }
            },
                function (error) {
                    Command: toastr["warning"](dataModel.DistributorTargetNo + " not Deleted, Please Check and Try Again!");
                    console.log("Error: " + error);
                });
        }

        // Clear info after sucessful transaction
        $scope.clear = function () {
            $scope.frmUserRole.$setPristine();
            $scope.frmUserRole.$setUntouched();

            $scope.USERROLEID = 0;
            $scope.btnSaveText = "Save";
            $scope.ROLEID = 0;
            $("#ROLEID").select2("data", { id: '', text: '--Select Role--' });
                        
            $scope.USERID = '';
            $("#USERID").select2("data", { id: '', text: '--Select User--' });
        };
        //**********---- End Clear records ----***************//
    }
]);