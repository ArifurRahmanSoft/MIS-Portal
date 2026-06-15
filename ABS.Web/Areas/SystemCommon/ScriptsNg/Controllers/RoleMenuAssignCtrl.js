app.controller('roleMenuAssignCtrl', ['$scope', 'crudService', 'uiGridConstants', '$q', '$http', 'conversion', '$filter', '$localStorage',
    function ($scope, crudService, uiGridConstants, $q, $http, conversion, $filter, $localStorage) {
        var baseUrl = '/SystemCommon/api/RoleMenuAssign/';
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptions = [];
        var LoginUserID = $('#hUserID').val();
        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";
        $scope.PageTitle = 'Role Menu Entry';
        $scope.ListTitle = 'Role Menu Records';

        $scope.listRole = [];
        $scope.ROLEID = null;

        $scope.roleName = '';

        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmRoleMenu'; DelFunc = ''; DelMsg = ''; EditFunc = '';
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

        $scope.loadRoleMenuList = function (isPaging) {
            $scope.cmnParam();
            objcmnParam.id = $scope.ROLEID;
            objcmnParam.loggeduser = LoginUserID;
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
                    { name: "RMID", displayName: "RMID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MENUNAME", displayName: "Menu Name", width: "20%", title: "Menu Name", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "MODULENAME", displayName: "Module Name", width: "20%", title: "Module Name", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: "ENABLEVIEW", displayName: "Is View", cellClass: 'text-center', type: 'boolean', width: "10%", headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<input type="checkbox" ng-model="row.entity.ENABLEVIEW" ng-disabled="row.entity.PARENTID==0" ng-click="grid.appScope.SetChecked(row.entity, \'ENABLEVIEW\')">',
                    },
                    {
                        name: "ENABLEINSERT", displayName: "Is Insert", cellClass: 'text-center', type: 'boolean', width: "10%", headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<input type="checkbox" ng-model="row.entity.ENABLEINSERT" ng-disabled="row.entity.PARENTID==0">',
                    },
                    {
                        name: "ENABLEUPDATE", displayName: "Is Update", cellClass: 'text-center', type: 'boolean', width: "10%", headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<input type="checkbox" ng-model="row.entity.ENABLEUPDATE" ng-disabled="row.entity.PARENTID==0">',
                    },
                    {
                        name: "ENABLEDELETE", displayName: "Is Delete", cellClass: 'text-center', type: 'boolean', width: "10%", headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<input type="checkbox" ng-model="row.entity.ENABLEDELETE" ng-disabled="row.entity.PARENTID==0">',
                    },
                    {
                        name: "ISACTIVE", displayName: "Is Active", cellClass: 'text-center', type: 'boolean', width: "5%", headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<input type="checkbox" ng-model="row.entity.ISACTIVE">',
                    },
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
                exporterCsvFilename: 'roleMenu.csv',
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
                var gridList = response.data.resdata.length > 0 ? $scope.setIntToBool(response.data.resdata) : [];
                $scope.gridOptions.data = gridList;
                $scope.loaderMore = false;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };

        $scope.loadRoleMenuList(0);

        $scope.setIntToBool = function (gridList) {

            angular.forEach(gridList, function (item) {
                item.ENABLEVIEW = item.ENABLEVIEW == 1 ? true : false;
                item.ENABLEINSERT = item.ENABLEINSERT == 1 ? true : false;
                item.ENABLEUPDATE = item.ENABLEUPDATE == 1 ? true : false;
                item.ENABLEDELETE = item.ENABLEDELETE == 1 ? true : false;
                item.ISACTIVE = item.ISACTIVE == '1' ? true : false
            });

            return gridList;
        }

        $scope.SetChecked = function (row, entity) {
            debugger;
            var prows = $scope.gridOptions.data.filter(x => x.MENUID == row.PARENTID)[0];
            var crows = $scope.gridOptions.data.filter(x => x[entity] == true && x.PARENTID == row.PARENTID)[0];
            prows[entity] = crows == undefined ? false : true;

        }

        // Save master detail data
        $scope.Save = function () {
            //$scope.cmnParam();            

            var HeaderTokenPutPost = $scope.HeaderToken.post;
            var ModelsArray = [$scope.gridOptions.data];
            var apiRoute = baseUrl + 'saveupdate/';
            var SaveUpdateRoleMenu = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SaveUpdateRoleMenu.then(function (response) {
                if (response != "") {
                    $scope.clear();
                    $scope.loadRoleMenuList(0);
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
            objcmnParam.id = dataModel.RMID;
            var ModelsArray = [objcmnParam];
            var apiRoute = baseUrl + 'delete/';
            var delDistributorTarget = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
            delDistributorTarget.then(function (response) {
                if (response.data.result != "") {
                    $scope.loadRoleMenuList(0);
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
            $scope.frmRoleMenu.$setPristine();
            $scope.frmRoleMenu.$setUntouched();

            $scope.ROLEID = null;
            $("#ROLEID").select2("data", { id: '', text: '--Select Role--' });
        };
        //**********---- End Clear records ----***************//

        $scope.saveRole = function () {
            var HeaderTokenPutPost = $scope.HeaderToken.post;
            var param = { roleName: $scope.roleName.trim(), loggedUserId: LoginUserID };
            var ModelsArray = [param];
            var apiRoute = baseUrl + 'saverole/';
            var SaveUpdateRole = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
            SaveUpdateRole.then(function (response) {
                if (response.resdata == "1") {
                    $scope.roleName = '';
                    $scope.loadRoleRecords();
                    ShowCustomToastr("success", "Role Saved Successfully!!!");
                }
                else if (response.resdata == "0") {
                    ShowCustomToastr("info", "Role already exists!!!");
                }
                else if (response.resdata == "") {
                    ShowCustomToastr("warning", "Something went wrong, Please check and try again later!!!");
                }
            },
                function (error) {
                    ShowCustomToastr("error", "Error occured, Please check and try again later!!!");
                });
        }
    }
]);