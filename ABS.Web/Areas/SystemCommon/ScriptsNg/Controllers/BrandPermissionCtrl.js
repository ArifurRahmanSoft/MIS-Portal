//const { forEach } = require("angular");

app.controller('BrandPermissionCtrl', function ($scope, crudService, menuService, $filter, conversion) {

    var baseUrl = '/SystemCommon/api/SystemCommonDDL/';

    var isExisting = 0;
    var page = 1;
    var pageSize = 1000;
    var isPaging = 0;
    var LoginUserID = $('#hUserID').val();

    $scope.btnSaveUpdateText = "Save";
    $scope.PageTitle = 'Create Brand Permission';
    $scope.ListTitle = 'Selected Brand for Permission';
    $scope.UserPermissionID = 0;
    $scope.ListUser = [];
    $scope.ListModuleForPermission = [];
    $scope.ListCompany = [];
    $scope.ListStatus = [];
    $scope.ListBrandPermission = [];
    $scope.ListPermissionNew = [];
    $scope.ListSKUNew = [];
    $scope.IsDeleteSku = false;

    $scope.Params = {
        ModuleID: 0,
        CompanyID: 0,
        UserID: 0
    };

    //$scope.EnableAllView = false;

    // Get all product category
    function loadProductCategory() {
        var apiRoute = baseUrl + 'GetProductCategory/';
        var mode = '1';
        var trackNo = '0';
        var listProductCategory = crudService.getMultipleParameter(apiRoute, mode, trackNo);
        listProductCategory.then(function (response) {
            $scope.listProductCategory = response.data;
        },
            function (error) {
                console.log("Error: " + error);
            });
    }
    loadProductCategory();


    // Get all brand by category
    $scope.loadBrandByCategory = function () {
        $scope.listBrand = [];
        $scope.ngmProductBrand = '';
        $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });

        var trackNo = 0;
        var apiRoute = baseUrl + 'GetBrandByCategory/';
        var categoryId = $scope.ngmProductCategory;
        var listBrand = crudService.getMultipleParameter(apiRoute, categoryId, trackNo);
        listBrand.then(function (response) {
            $scope.listBrand = response.data;
        },
            function (error) {
                console.log("Error: " + error);
            });
    }

    function loadSalesTeam() {
        var apiRoute = baseUrl + 'GetSSalesAreaHierarchyList/';
        var mode = '1';
        var trackNo = '0';
        var listNational = crudService.getMultipleParameter(apiRoute, mode, trackNo);
        listNational.then(function (response) {
            $scope.listNational = response.data;
        },
            function (error) {
                console.log("Error: " + error);
            });
    }
    loadSalesTeam();

    //Working with File Delete
    $scope.deleteBrand = function (element, index) {
        var IsConf = confirm('You are about to delete ' + element.BRANDNAME + '. Are you sure?');
        if (IsConf) {
            $scope.IsDeleteSku = true;
            $scope.loadSKURecords(element);
            $scope.ListBrandPermission.splice(index, 1);
        };
    };

    $scope.ResetBrandPermission = function () {
        $scope.ListSKUNew = [];
        $scope.ListBrandPermission = [];

        // get existing brand for this sales team.

        var apiRoute = baseUrl + 'GetSalesTeamBrand/';
        var mode = '1';
        var trackNo = $scope.ngModelNational;
        var listBrand = crudService.getMultipleParameter(apiRoute, mode, trackNo);
        listBrand.then(function (response) {
            $scope.ListBrandPermission = response.data;
        },
            function (error) {
                console.log("Error: " + error);
            });
    }

    $scope.loadSKURecords = function (model) {
        debugger;
        if (!$scope.IsDeleteSku) {
            $("#ItemModal").fadeIn(200, function () { $('#ItemModal').modal('show'); });
            conversion.UniformUnChecked('uniform-chkEnableAllChecked');
        }
        debugger

        var apiRoute = baseUrl + 'GetSKUByBrand/';
        var mode = $scope.ngModelNational;
        var trackNo = model.BRANDOID;
        $scope.BrandTitle = model.BRANDNAME;
        var listSKU = crudService.getMultipleParameter(apiRoute, mode, trackNo);
        listSKU.then(function (response) {
            $scope.ListSKUBrand = response.data;

            angular.forEach($scope.ListSKUBrand, function (item) {
                item.ENABLE_CHECK = item.ENABLE_CHECK == '1' ? true : false;
                item.ISUPDATE = item.ISUPDATE == '1' ? true : false;

                if ($scope.ListSKUNew.length > 0) {
                    var exModel = $scope.ListSKUNew.filter(x => x.NATIONALOID == $scope.ngModelNational && x.BRANDOID == item.BRANDOID && x.PRODUCTOID == item.PRODUCTOID)[0];
                    if (exModel != undefined) {
                        item.ENABLE_CHECK = exModel.ENABLE_CHECK;
                    }
                }

                if (item.ENABLE_CHECK) {
                    var chkModel = $scope.ListSKUNew.filter(x => x.NATIONALOID == $scope.ngModelNational && x.BRANDOID == item.BRANDOID && x.PRODUCTOID == item.PRODUCTOID)[0];
                    if (chkModel == undefined) {
                        $scope.setSKUNewList(item.ENABLE_CHECK, item);
                    }
                }
            });

            $scope.EnableAllCheck = $scope.ListSKUBrand.length > 0 && ($scope.ListSKUBrand.filter(x => x.ENABLE_CHECK == false)).length > 0 ? false : true;
            if ($scope.EnableAllCheck == true) {
                $scope.checkedCount = 1;
                $scope.IsDeleteSku ? null :
                    conversion.UniformChecked('uniform-chkEnableAllChecked');
            }
            else {
                if ($scope.checkedCount == 1) {
                    $scope.IsDeleteSku ? null :
                        conversion.UniformUnChecked('uniform-chkEnableAllChecked');
                }

                $scope.checkedCount = 0;
            }

            debugger
            $scope.IsDeleteSku = false;
            //if ($scope.ListSKUNew.length > 0) {
            //$scope.ListSKUNew;
            //var dsfsaf = $scope.ListSKUNew[index].PRODUCTID;
            //if ($scope.ListSKUBrand[index].PRODUCTOID = dsfsaf) {
            //    var sadfasfafaf = dsfsaf;
            //}
            //else {
            //    var sad1 = dsfsaf;
            //}
            //angular.forEach($scope.ListSKUNew[index], function (value, key) {
            //        $scope.ListSKUNew;
            //        var aaa = value.PRODUCTID;
            //        var aaafff = key.PRODUCTID;
            //        if (value.PRODUCTID == $scope.ListSKUBrand[index].PRODUCTOID) {
            //            value.ENABLE_CHECK = true;

            //            return false;
            //        }
            //    });
            //}



            //$scope.ListSKUBrand = response.data;
            //$scope.ListSKUNew;
            //var productid=$scope.ListSKUBrand[index].PRODUCTOID;

            //// omor code check list 
            //var selectedBrandName = '';
            //var selectedCategory = '';



            //debugger
            //angular.forEach($scope.ListSKUNew, function (value, key)  {
            //    var aaa = value.PRODUCTID;
            //    var aaafff = key.PRODUCTID;
            //    var ddd=$scope.ListSKUBrand[index].PRODUCTOID;
            //    if (value.PRODUCTID == $scope.ListSKUBrand[index].PRODUCTOID) {
            //        var aaa=value.PRODUCTID;
            //        value.ENABLE_CHECK = true;
            //    }
            //});
        },
            function (error) {
                console.log("Error: " + error);
            });


        //if ($scope.ListSKUNew.length > 0) {
        //    angular.forEach($scope.ListSKUNew, function (value) {
        //        if (value.PRODUCTID == $scope.ListSKUBrand[index].PRODUCTOID) {
        //            value.ENABLE_CHECK = true;

        //            return false;
        //        }
        //    });
        //}


    }

    $scope.AddToList = function () {
        debugger

        if ($('#ddlProductBrand').val() == "" || $('#ddlProductBrand').val() == "") {
            Command: toastr["warning"]("Please select a brand first !");
            return false;
        }

        var ifExists = 0;

        if ($scope.ListBrandPermission.length > 0) {

            angular.forEach($scope.ListBrandPermission, function (value) {
                if (value.BRANDOID == $scope.ngmProductBrand) {
                    Command: toastr["warning"]("This brand is already exist. Please select a different brand !");
                    ifExists = 1
                    return false;
                }
            });

            if (ifExists == 0) {
                var selectedBrandName = '';
                var selectedCategory = '';
                angular.forEach($scope.listBrand, function (value) {
                    if (value.BRANDID == $scope.ngmProductBrand) {
                        selectedBrandName = value.SBRND_NAME;
                    }
                });

                angular.forEach($scope.listProductCategory, function (value) {
                    if (value.CATEGORYID == $scope.ngmProductCategory) {
                        selectedCategory = value.CATEGORYNAME;
                    }
                });

                $scope.ListBrandPermission.push({
                    CATEGORYNAME: selectedCategory,
                    BRANDOID: $scope.ngmProductBrand,
                    BRANDNAME: selectedBrandName
                });
                Command: toastr["success"]("This brand has successfully added to list. !!!");
            }
        }
        else {
            debugger
            var selectedBrandName = '';
            var selectedCategory = '';
            angular.forEach($scope.listBrand, function (value) {
                if (value.BRANDID == $scope.ngmProductBrand) {
                    selectedBrandName = value.SBRND_NAME;
                }
            });
            angular.forEach($scope.listProductCategory, function (value) {
                if (value.CATEGORYID == $scope.ngmProductCategory) {
                    selectedCategory = value.CATEGORYNAME;
                }
            });

            $scope.ListBrandPermission.push({
                CATEGORYNAME: selectedCategory,
                BRANDOID: $scope.ngmProductBrand,
                BRANDNAME: selectedBrandName
            });


            Command: toastr["success"]("This brand has successfully added to list. !!!");
        }
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

    $scope.Save = function (dataModel) {

        if (dataModel.length == 0) {
            Command: toastr["warning"]("Please select at least one brand first !");
            return false;
        }

        //  angular.forEach($scope.ListBrandPermission, function (value) {
        angular.forEach(dataModel, function (value) {
            debugger
            value.NATIONALTEAMOID = $scope.ngModelNational;
            value.LOGGEDUSERID = LoginUserID;
            value.ListSKUmodel = $scope.ListSKUNew;
        });
        debugger
        var apiRoute = '/SystemCommon/api/BrandPermission/SaveBrandPermission/';
        //var BrandPermissionCreate = crudService.post(apiRoute, dataModel);
        var BrandPermissionCreate = crudService.post(apiRoute, dataModel);
        BrandPermissionCreate.then(function (response) {
            ShowCustomToastrMessage(response);
            $scope.clear();
        },
            function (error) {
                console.log("Error: " + error);
            });
        //if ($scope.ListSKUNew.length > 0) {
        //    // save sku list in separate table
        //    debugger
        //    var apiRoute = '/SystemCommon/api/BrandPermission/SaveBrandSKU/';
        //    var BrandSKUPermissionCreate = crudService.post(apiRoute, ListSKUNew);
        //    BrandSKUPermissionCreate.then(function (response) {
        //        ShowCustomToastrMessage(response);
        //        $scope.clear();
        //    },
        //        function (error) {
        //            console.log("Error: " + error);
        //        });
        //}
    };

    $scope.checkedCount = 0;
    $scope.gridViewCheckBoxClicked = function (model) {
        debugger
        // start for product/sku selection
        if (model.ENABLE_CHECK) {
            //ENABLE_CHECK = true;
            //debugger
            $scope.setSKUNewList(model.ENABLE_CHECK, model);

            $scope.EnableAllCheck = $scope.ListSKUBrand.length > 0 && ($scope.ListSKUBrand.filter(x => x.ENABLE_CHECK == false)).length > 0 ? false : true;
            if ($scope.EnableAllCheck == true) {
                $scope.checkedCount = 1;
                conversion.UniformChecked('uniform-chkEnableAllChecked');
            }
            else {
                if ($scope.checkedCount == 1) {
                    conversion.UniformUnChecked('uniform-chkEnableAllChecked');
                }

                $scope.checkedCount = 0;
            }
            //$scope.ListSKUNew;
            Command: toastr["success"]("New Product has successfully added to list. !!!");
        }
        else {
            $scope.setSKUNewList(model.ENABLE_CHECK, model);

            $scope.EnableAllCheck = $scope.ListSKUBrand.length > 0 && ($scope.ListSKUBrand.filter(x => x.ENABLE_CHECK == false)).length > 0 ? false : true;

            if ($scope.EnableAllCheck == true) {
                $scope.checkedCount = 1;
                conversion.UniformChecked('uniform-chkEnableAllChecked');
            }
            else {

                if ($scope.checkedCount == 1) {
                    conversion.UniformUnChecked('uniform-chkEnableAllChecked');
                }

                $scope.checkedCount = 0;
            }

            Command: toastr["success"]("Existing Product has successfully removed to list. !!!");
        }
        // end for product/sku selection
    }

    $scope.setSKUNewList = function (isTrue, model) {
        if (isTrue) {
            var SKUModel = $scope.ListSKUNew.filter(x => x.NATIONALOID == $scope.ngModelNational && x.BRANDOID == model.BRANDOID && x.PRODUCTOID == model.PRODUCTOID)[0];
            if (SKUModel != undefined) {
                SKUModel.ENABLE_CHECK = model.ENABLE_CHECK;
            } else {
                $scope.ListSKUNew.push({
                    NATIONALOID: $scope.ngModelNational,
                    BRANDOID: model.BRANDOID,
                    PRODUCTOID: model.PRODUCTOID,
                    LOGGEDUSERID: LoginUserID,
                    ISUPDATE: model.ISUPDATE,
                    ENABLE_CHECK: $scope.IsDeleteSku ? false : model.ENABLE_CHECK
                });
            }
        }
        else {
            var SKUModel = $scope.ListSKUNew.filter(x => x.NATIONALOID == $scope.ngModelNational && x.BRANDOID == model.BRANDOID && x.PRODUCTOID == model.PRODUCTOID)[0];
            if (SKUModel != undefined) {
                if (model.ISUPDATE) {
                    SKUModel.ENABLE_CHECK = $scope.IsDeleteSku ? false : model.ENABLE_CHECK;
                }
                else {
                    $scope.ListSKUNew.splice($scope.ListSKUNew.indexOf(SKUModel), 1);
                }
            }
        }
    }

    $scope.clear = function () {

        $scope.ListBrandPermission = [];
        $scope.ListSKUNew = [];
        $scope.ngModelNational = '';
        $("#ddlNational").select2("data", { id: '', text: '--Select Sales Team--' });

        $scope.ngmProductCategory = '';
        $("#ddlProductCategory").select2("data", { id: '', text: '--Select Category--' });

        $scope.listBrand = [];
        $scope.ngmProductBrand = '';
        $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });
    };

    $scope.AllCheckClicked = function (EnableAllCheck) {
        //debugger
        angular.forEach($scope.ListSKUBrand, function (value) {
            //debugger
            if (EnableAllCheck) {
                value.ENABLE_CHECK = true;
                $scope.checkedCount = 1;
                $scope.setSKUNewList(EnableAllCheck, value);

                $scope.EnableAllCheck = $scope.ListSKUBrand.length > 0 && ($scope.ListSKUBrand.filter(x => x.ENABLE_CHECK == false)).length > 0 ? false : true;

                $('#chkEnableSingleCheck').find('span').addClass('checked');
            }
            else {
                value.ENABLE_CHECK = false;
                $scope.checkedCount = 0;
                $scope.setSKUNewList(EnableAllCheck, value);
                $scope.EnableAllCheck = $scope.ListSKUBrand.length > 0 && ($scope.ListSKUBrand.filter(x => x.ENABLE_CHECK == false)).length > 0 ? false : true;
                $('#chkEnableSingleCheck').find('span').removeClass('checked');
            }
        });
    }

    //function SetAllGridCheckBox(permission) {
    //    debugger
    //    var InsertView = true;
    //    var UpdateView = true;
    //    var DeleteView = true;
    //    var OnlyView = true;

    //    angular.forEach($scope.ListBrandPermission, function (value) {

    //        debugger
    //        if (value.ENABLEVIEW == 0) {
    //            OnlyView = false;
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

    //}

    //******************view CheckBox*********************************

});




