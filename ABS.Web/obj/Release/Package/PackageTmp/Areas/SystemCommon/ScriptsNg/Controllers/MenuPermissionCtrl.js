app.controller('MenuPermissionCtrl', function ($scope, crudService, conversion, menuService, $filter) {
    var baseUrl = '/SystemCommon/api/MenuPermission/';
    var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';

    var isExisting = 0;
    var page = 1;
    var pageSize = 1000;
    var isPaging = 0;
    var totalData = 0;
    var companyID = $('#hCompanyID').val();
    var loggedUser = 0;
    var LoginUserID = $('#hUserID').val();
    var LoginCompanyID = $('#hCompanyID').val();
    $scope.UserCommonEntity = {};
    $scope.btnSaveUpdateText = "Save";
    $scope.PageTitle = 'Create User Permission';
    $scope.ListTitle = 'Permission Records';
    $scope.UserPermissionID = 0;
    $scope.ListUser = [];
    $scope.ListModuleForPermission = [];
    $scope.ListCompany = [];
    $scope.ListStatus = [];
    $scope.ListPermission = [];
    $scope.ListPermissionNew = [];

    $scope.Params = {
        ModuleID: 0,
        CompanyID: 0,
        UserID: 0
    };
    var pModuleID = 0;
    var pUserID = 0;

    //$scope.EnableAllView = false;
    //$scope.EnableAllInsert = false;
    //$scope.EnableAllUpdate = false;
    //$scope.EnableAllDelete = false;


    //***************************************************Start Common Task for all**************************************************
    frmName = 'frmDocumentUpload'; DelFunc = 'DeleteBuildingMasterRecord'; DelMsg = 'DocName'; EditFunc = 'getDocumentListByID';
    $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
    $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
    $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
    $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
    $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
    $scope.cmnbtnShowHideEnDisable(0);

        //****************************************************End Common Task for all*************************************************** 


    
    function loadRecords_User(isPaging) {
        var apiRoute = sysCommonUrl + 'GetUser/';
        var processListUser = crudService.getAllIncludingCompanyLog(apiRoute, companyID, LoginUserID, page, pageSize, isPaging);
        processListUser.then(function (response) {
            $scope.ListUser = response.data;
            $("#userDropDown").select2("data", { id: 0, text: '--Select User--' });
        },
            function (error) {
                console.log("Error: " + error);
            });
    }
    loadRecords_User(0);

    $scope.LoadModule = function loadRecords_Module(isPaging) {
        var apiRoute = sysCommonUrl + 'GetModuleWithPermission/';
        var listModuleProcess = menuService.GetModules(apiRoute, LoginCompanyID, LoginUserID, page, pageSize, isPaging);
        listModuleProcess.then(function (response) {
         
            $scope.ListModuleForPermission = response.data
            $("#moduleDropDown").select2("data", { id: 0, text: '--Select Module--' });
        },
            function (error) {
                console.log("Error: " + error);
            });
    };
    $scope.LoadModule(0);

    $scope.Generate = function (ddlModule, ListModuleForPermission) {
        debugger
        var selectedID = ddlModule;
        var SeletedName = "";
        try {
            for (i = 0; i < ListModuleForPermission.length; i++) {
                if (ddlModule == ListModuleForPermission[i].ModuleID) {
                    SeletedName = ListModuleForPermission[i].ModuleName;
                    break;
                }
            }
        } catch (e) {

        }
        if (selectedID >= 1) {
            var bbd = $scope.Params;
            var mod = $scope.Params.ModuleID;
            $scope.Params.ModuleID = selectedID;
        }
        $scope.BindPermissionGrid();
    };

    $scope.ModuleChanged = function (ddlModule, moduleList) {
        ////debugger
        var selectedID = ddlModule;
        var SeletedName = "";
        for (i = 0; i < moduleList.length; i++) {
            if (ddlModule == moduleList[i].ModuleID) {
                SeletedName = moduleList[i].ModuleName;
                break;
            }
        }
        if (selectedID >= 1) {
            var bbd = $scope.Params;
            var mod = $scope.Params.ModuleID;
            $scope.Params.ModuleID = selectedID;
        }
        $scope.BindPermissionGrid();
    };

    $scope.SetInputModule = function (selectedItem) {
       
        if (selectedItem == null) {
            selectedItem = 0;
        }
        var selectedID = selectedItem;
        $scope.Params.ModuleID = selectedID;
    };
    
    $scope.BindPermissionGrid = function () {
        
        pModuleID = $scope.Params.ModuleID || 0;
        pUserID = $scope.ddlUserMaster || 0;
        companyID = $('#hCompanyID').val();
        pModule = $scope.Params.ModuleID || 0;

        var apiRoute = baseUrl + 'GetMenuPermissionByParam/';
        var listPermissionParam = crudService.getAllByParam(apiRoute, LoginUserID, page, pageSize, isPaging, pModuleID, pUserID);
        listPermissionParam.then(function (response) {
            debugger
            $scope.ListPermission = response.data

            for (i = 0; i < response.data.length; i++) {

                if ($scope.ListPermission[i].ENABLEVIEW == 1) {
                    $scope.ListPermission[i].ENABLE_VIEW = true;
                }
                else {
                    $scope.ListPermission[i].ENABLE_VIEW = false;
                }


                if ($scope.ListPermission[i].ENABLEINSERT == 1) {
                    $scope.ListPermission[i].ENABLE_INSERT = true;
                }
                else {
                    $scope.ListPermission[i].ENABLE_INSERT = false;
                }


                if ($scope.ListPermission[i].ENABLEUPDATE == 1) {
                    $scope.ListPermission[i].ENABLE_UPDATE = true;
                }
                else {
                    $scope.ListPermission[i].ENABLE_UPDATE = false;
                }


                if ($scope.ListPermission[i].ENABLEDELETE == 1) {
                    $scope.ListPermission[i].ENABLE_DELETE = true;
                }
                else {
                    $scope.ListPermission[i].ENABLE_DELETE = false;
                }
            }

            //SetAllGridCheckBox($scope.ListPermission);
        },
            function (error) {
                console.log("Error: " + error);
            });
    };

    $scope.Save = function (dataModel) {

        debugger

      //  angular.forEach($scope.ListPermission, function (value) {
        angular.forEach(dataModel, function (value) {
            debugger
            value.MODULEID = $scope.Params.ModuleID;
            value.USERID = $scope.ddlUserMaster;
            value.STATUSID = 1;
            value.COMPANYID = $('#hCompanyID').val();
        });

        var apiRoute = baseUrl + 'SaveMenuPermission/';
        var MenuPermissionCreate = crudService.post(apiRoute, dataModel);
        MenuPermissionCreate.then(function (response) {
            ShowCustomToastrMessage(response);
            $scope.BindPermissionGrid();
        },
            function (error) {
                console.log("Error: " + error);
            });
    };
     

    $scope.gridViewCheckBoxClicked = function (index, status) {
        debugger
        if (status) {
            $scope.ListPermission[index].ENABLEVIEW = 1;
        }

        if (!status) {
            $scope.ListPermission[index].ENABLEVIEW = 0;
        }          
    }

    $scope.gridInsertCheckBoxClicked = function (index, status) {
        debugger
        if (status) {
            $scope.ListPermission[index].ENABLEINSERT = 1;
        }

        if (!status) {
            $scope.ListPermission[index].ENABLEINSERT = 0;
        }         
    }
   
    $scope.gridUpdateCheckBoxClicked = function (index, status) {
        debugger
        if (status) {
            $scope.ListPermission[index].ENABLEUPDATE = 1;
        }

        if (!status) {
            $scope.ListPermission[index].ENABLEUPDATE = 0;
        }        
        //SetAllGridCheckBox(1);
    }

    $scope.gridDeleteCheckBoxClicked = function (index, status) {
        debugger

        if (status) {
            $scope.ListPermission[index].ENABLEDELETE = 1;
        }

        if (!status) {
            $scope.ListPermission[index].ENABLEDELETE = 0;
        }                       

        //SetAllGridCheckBox(1);
    }


    $scope.clear = function () {
        $scope.ListPermission = [];
        $("#userDropDown").select2("data", { id: 0, text: '--Select User--' });
        $("#moduleDropDown").select2("data", { id: 0, text: '--Select Module--' });
    };

    //$scope.EnableAllViewClicked = function (EnableAllView) {
    //    debugger
    //    angular.forEach($scope.ListPermission, function (value) {
    //        debugger
    //        if (EnableAllView) {
    //            value.ENABLEVIEW = 1;
    //            $scope.EnableAllView = true;
    //            $('#chkEnableAllView').find('span').addClass('checked');
    //        }
    //        else {
    //            value.ENABLEVIEW = 0;
    //            $scope.EnableAllView = false;
    //            $('#chkEnableAllView').find('span').removeClass('checked');
    //        }
    //    });
    //}

    //$scope.EnableAllInsertClicked = function (EnableAllInsert) {
    //    debugger
    //    angular.forEach($scope.ListPermission, function (value) {
    //        debugger
    //        if (EnableAllInsert) {
    //            value.ENABLEINSERT = 1;
    //            $scope.EnableAllInsert = true;
    //        }
    //        else {
    //            value.ENABLEINSERT = 0;
    //            $scope.EnableAllInsert = false;
    //        }
    //    });
    //}

    //$scope.EnableAllUpdateClicked = function (EnableAllUpdate) {
    //    debugger
    //    angular.forEach($scope.ListPermission, function (value) {
    //        if (EnableAllUpdate) {
    //            value.ENABLEUPDATE = 1;
    //            $scope.EnableAllUpdate = true;
    //        }
    //        else {
    //            value.ENABLEUPDATE = 0;
    //            $scope.EnableAllUpdate = false;
    //        }
    //    });
    //}

    //$scope.EnableAllDeleteClicked = function (EnableAllDelete) {
    //    debugger
    //    angular.forEach($scope.ListPermission, function (value) {
    //        if (EnableAllDelete) {
    //            value.ENABLEDELETE = 1;
    //            $scope.EnableAllDelete = true;
    //        }
    //        else {
    //            value.ENABLEDELETE = 0;
    //            $scope.EnableAllDelete = false;
    //        }
    //    });
    //}




    //function SetAllGridCheckBox(permission) {
    //    debugger
    //    var InsertView = true;
    //    var UpdateView = true;
    //    var DeleteView = true;
    //    var OnlyView = true;

    //    angular.forEach($scope.ListPermission, function (value) {

    //        debugger
    //        if (value.ENABLEVIEW == 0) {
    //            OnlyView = false;
    //        }
    //        if (value.ENABLEINSERT == 0) {
    //            InsertView = false;
    //        }
    //        if (value.ENABLEUPDATE == 0) {
    //            UpdateView = false;
    //        }
    //        if (value.ENABLEDELETE == 0) {
    //            DeleteView = false;
    //        }
    //    });


    //    if (OnlyView)
    //    {
    //        $scope.EnableAllView = true;
    //        $('#chkEnableAllView').find('span').addClass('checked');
    //    }
    //    else
    //    {
    //        $scope.EnableAllView = false;
    //        $('#chkEnableAllView').find('span').removeClass('checked');
    //    }
    //    if (InsertView)
    //    {
    //        $scope.EnableAllInsert = true;
    //        $('#chkEnableAllInsert').find('span').addClass('checked');
    //    }
    //    else
    //    {
    //        $scope.EnableAllInsert = false;
    //        $('#chkEnableAllInsert').find('span').removeClass('checked');
    //    }
    //    if (UpdateView)
    //    {
    //        $scope.EnableAllUpdate = true;
    //        $('#chkEnableAllUpdate').find('span').addClass('checked');
    //    }
    //    else
    //    {
    //        $scope.EnableAllUpdate = false;
    //        $('#chkEnableAllUpdate').find('span').removeClass('checked');
    //    }
    //    if (DeleteView)
    //    {
    //        $scope.EnableAllDelete = true;
    //        $('#chkEnableAllDelete').find('span').addClass('checked');
    //    }
    //    else
    //    {
    //        $scope.EnableAllDelete = false;
    //        $('#chkEnableAllDelete').find('span').removeClass('checked');
    //    }
    //}

    //******************view CheckBox*********************************

});




