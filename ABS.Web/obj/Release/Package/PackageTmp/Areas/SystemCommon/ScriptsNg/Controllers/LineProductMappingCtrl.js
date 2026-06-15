app.controller('LineProductMappingCtrl', ['$scope', 'crudService', 'uiGridConstants', '$q', '$http', 'conversion', '$filter', '$localStorage',
    function ($scope, crudService, uiGridConstants, $q, $http, conversion, $filter, $localStorage) {
        var baseUrl = '/SystemCommon/api/DataMapping/';
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptions = [];
        var LoginUserID = $('#hUserID').val();
        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";
        $scope.PageTitle = 'Line wise product mapping entry';
        $scope.ListTitle = 'Line wise product records';

        $scope.COMPANY_OID = '';
        $scope.SSTYP_OID = '';
        $scope.SDVNT_OID = '';
        $scope.SPROG_OID = '';
        $scope.GROUP_OID = '';
        $scope.USER_OID = '';
        $scope.BRAND_OID = '';
        $scope.SKU_OID = '';
        $scope.ROLE_OID = '';
        $scope.NTNAL_OID = '';
        $scope.IS_ENABLE = false;
        $scope.IS_UPDATE = false;
        $scope.TRAN_TYPE_ID = '10';
        $scope.CMN_OID = '';
        $scope.USER_FULLNAME = '';
        $scope.USER_PASS = '';

        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmLineProductMapping'; DelFunc = 'delete'; DelMsg = 'LineProductMapping'; EditFunc = 'edit';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        

        //$scope.mappingTypeList = [
        //    { TRAN_TYPE_ID: '1', TRAN_TYPE_NAME: 'Company And SSTyp Setup (Many to many)' },
        //    { TRAN_TYPE_ID: '2', TRAN_TYPE_NAME: 'SDVNT Wise Product Group Setup-for product group' },
        //    { TRAN_TYPE_ID: '3', TRAN_TYPE_NAME: 'User wise Group and Brand Setup-for sales group (Many to many)' },
        //    { TRAN_TYPE_ID: '4', TRAN_TYPE_NAME: 'User wise Group, Brand and SKU Setup (Many to many)' },
        //    { TRAN_TYPE_ID: '5', TRAN_TYPE_NAME: 'None Sales Person Permission' },
        //    { TRAN_TYPE_ID: '6', TRAN_TYPE_NAME: 'Temporary User for MIS Portal' },
        //    { TRAN_TYPE_ID: '7', TRAN_TYPE_NAME: 'User Menu Mode Permission' },
        //    { TRAN_TYPE_ID: '8', TRAN_TYPE_NAME: 'Set User wise Sales Group (UsLine-Cityn)' },
        //    { TRAN_TYPE_ID: '9', TRAN_TYPE_NAME: 'Set User for Sales Soft V3 (TUser-Cityn)' },
        //    { TRAN_TYPE_ID: '10', TRAN_TYPE_NAME: 'Set Sales Group wise Product (Line Prod-Cityn)' },
        //    { TRAN_TYPE_ID: '11', TRAN_TYPE_NAME: 'Open New Product (Product-Cityn)' },
        //];

        /*$scope.roleList = [{ ROLE_OID: '16', ROLE_NAME: 'All National' }, { ROLE_OID: '15', ROLE_NAME: 'Single National' }]*/

        $scope.MappingTypeChange = function () {
            debugger;
            $scope.COMPANY_OID = '';
            $scope.SSTYP_OID = '';
            $scope.SDVNT_OID = '';
            $scope.SPROG_OID = '';
            $scope.GROUP_OID = '';
            $scope.USER_OID = '';
            $scope.BRAND_OID = '';
            $scope.SKU_OID = '';
            $scope.ROLE_OID = '';
            $scope.NTNAL_OID = '';
            $scope.IS_ENABLE = false;
            $scope.IS_UPDATE = false;
            $scope.CMN_OID = '';
            $scope.USER_FULLNAME = '';
            $scope.USER_PASS = '';

            $scope.IS_ENABLE = true;

            $scope.isShowList = true;
        }
        $scope.MappingTypeChange();

        $scope.isShowList = false;
        $scope.loadList = function () {
            debugger;
            $scope.isShowList = $scope.isShowList ? false : true;
            if ($scope.isShowList) {
                $scope.loadDataMappingList();
            } else {
                $scope.loadFilteredProduct();
            }
        }

        //********************************************************************START TRAN_TYPE_ID=='10'******************************************************************************
        // Get all Sales Group
        $scope.sdvntList = [];
        $scope.loadAllSdvntList = function () {
            var cmnParam = { id: 0 };
            $scope.SDVNT_OID = '';
            $scope.sdvntList = [];
            $("#SDVNT_OID").select2("data", { id: '', text: '--Select Sales Group--' });
            var apiRoute = sysCommonUrl + 'GetAllSalesGroup/';
            var sdvntRecords = crudService.postMultipleModels(apiRoute, cmnParam, $scope.HeaderToken.get);
            sdvntRecords.then(function (response) {
                $scope.sdvntList = response.data.slsGroupList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.loadAllSdvntList();
        // Get all Sales Group   

        $scope.onChangeSdvnt = function () {
            debugger;
            if ($scope.isShowList) {
                $scope.loadDataMappingList();
            } else {
                $scope.loadFilteredProduct();
            }
        }

        // Get all sstyp wise product
        $scope.loadFilteredProduct = function () {
            debugger;
            $scope.ListLineProduct = [];
            $scope.listSKU = [];
            var sdvntModel = $scope.sdvntList.filter(x => x.SDVNT_OID == $scope.SDVNT_OID)[0];
            if (sdvntModel != undefined) {
                var cmnParam = { strId: sdvntModel.SDVNT_SCOMP, strId2: sdvntModel.SDVNT_SSTYP, strId3: sdvntModel.SDVNT_OID };
                $("#SKU_OID").select2("data", { id: '', text: '--Select Product--' });
                var apiRoute = sysCommonUrl + 'GetAllProductBySSTYP/';
                var skuRecords = crudService.postMultipleModels(apiRoute, cmnParam, $scope.HeaderToken.get);
                skuRecords.then(function (response) {
                    $scope.listSKU = response.data.productList;
                    $scope.SetSalesGroupProduct();
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
        }

        $scope.ListLineProduct = [];
        $scope.SetSalesGroupProduct = function () {
            if ($scope.SDVNT_OID != '' && $scope.SDVNT_OID != null && $scope.SDVNT_OID != undefined) {
                var sdvntModel = $scope.sdvntList.filter(x => x.SDVNT_OID == $scope.SDVNT_OID)[0];
                if (sdvntModel != undefined) {
                    $scope.CMN_OID = sdvntModel.SDVNT_SCTYP;
                    $scope.COMPANY_OID = sdvntModel.SDVNT_SCOMP;

                    angular.forEach($scope.listSKU, function (item) {
                        $scope.ListLineProduct.push({
                            CMN_OID: sdvntModel.SDVNT_SCTYP,
                            COMPANY_OID: sdvntModel.SDVNT_SCOMP,
                            SDVNT_OID: sdvntModel.SDVNT_OID,
                            SDVNT_NAME: sdvntModel.SDVNT_NAME,
                            SKU_OID: item.PROD_OID,
                            PROD_CODE: item.PROD_CODE,
                            PROD_NAME: item.PROD_NAME,
                            BRAND_OID: item.BRAND_OID,
                            BRAND_NAME: item.BRAND_NAME,
                            IS_ENABLE: true,
                            IS_CHECKED: item.IS_EXIST == 0 ? false : true,
                            IS_EXIST: item.IS_EXIST == 0 ? false : true
                        });
                    });
                }
            }
        }

        $scope.gridCheckBoxClicked = function (index, status) {
            debugger
            if (status) {
                $scope.ListLineProduct[index].IS_CHECKED = true;
            }

            if (!status) {
                $scope.ListLineProduct[index].IS_CHECKED = false;
            }
        }
        // Get all sstyp wise product

        // Get all Sales Group
        //*******************************************************************END TRAN_TYPE_ID=='10'****************************************************************************

        $scope.loadDataMappingList = function (isPaging) {
            //$scope.cmnParam();
            var params = {
                COMPANY_OID: $scope.COMPANY_OID,
                SSTYP_OID: $scope.SSTYP_OID,
                SDVNT_OID: $scope.SDVNT_OID,
                SPROG_OID: $scope.SPROG_OID,
                GROUP_OID: $scope.GROUP_OID,
                USER_OID: $scope.USER_OID,
                BRAND_OID: $scope.BRAND_OID,
                SKU_OID: $scope.SKU_OID,
                ROLE_OID: $scope.ROLE_OID,
                NTNAL_OID: $scope.NTNAL_OID,
                IS_ENABLE: $scope.IS_ENABLE,
                IS_UPDATE: $scope.IS_UPDATE,
                TRAN_TYPE_ID: $scope.TRAN_TYPE_ID,
                CMN_OID: $scope.CMN_OID,
                USER_FULLNAME: $scope.USER_FULLNAME,
                USER_PASS: $scope.USER_PASS,
                pageNumber: 0,
                pageSize: 0,
            };
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
                    { name: "SDVNT_OID", displayName: "SDVNT_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SDVNT_NAME", displayName: "Sales Group", width: "50%", title: "Sales Group", headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SKU_OID", displayName: "SKU_OID", visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SPROD_NAME", displayName: "Product", width: "50%", title: "Product", headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Option',
                        displayName: "Option",
                        width: '10%',
                        pinnedRight: true,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.visible,
                        cellTemplate: '<span class="label label-danger label-mini" style="text-align:center !important; background-color:brown">' +
                            '<a href="" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                            '<i class="icon-check" aria-hidden="true"></i> Cancel </a></span>'
                    }
                ]
            };


            var apiRoute = baseUrl + 'getbypage/';
            var dataMappingRecords = crudService.postMultipleModels(apiRoute, params, $scope.HeaderToken.get);
            dataMappingRecords.then(function (response) {
                debugger;
                $scope.gridOptions.data = response.data.resdata;
                $scope.loaderMore = false;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        $scope.loadDataMappingList(0);

        $scope.edit = function (dataModel) {
            if ($scope.TRAN_TYPE_ID == '6' || $scope.TRAN_TYPE_ID == '9') {
                debugger;
                $scope.USER_OID = $scope.TRAN_TYPE_ID == '9' ? dataModel.USER_TEXT : dataModel.USER_OID;
                $scope.USER_FULLNAME = dataModel.USER_FULLNAME;
                $scope.USER_PASS = dataModel.USER_PASSWORD;
                $scope.IS_ENABLE = dataModel.IS_ENABLE == 1 ? true : false;
                $scope.IS_UPDATE = true;
            }
        }

        $scope.delete = function (entity) {
            var IsConf = confirm('You are about to cancel. Are you sure?');
            if (IsConf) {
                entity.TRAN_TYPE_ID = $scope.TRAN_TYPE_ID;
                var apiRoute = baseUrl + 'delete/';
                var delRecords = crudService.postMultipleModels(apiRoute, entity, $scope.HeaderToken.delete);
                delRecords.then(function (response) {
                    debugger
                    if (response != "") {
                        if (response.data.resdata == '1') {
                            $scope.clear();
                            $scope.loadDataMappingList(0);
                            ShowCustomToastr('success', 'Data deleted successfully !!!');
                        } else {
                            ShowCustomToastr('warning', 'Data not deleted, Please try again later !!!');
                        }
                    }
                    else if (response == "") {
                        ShowCustomToastr('warning', 'Data not deleted, Please try again later !!!');
                    }
                },
                    function (error) {
                        ShowCustomToastr('warning', 'Data not deleted, Please try again later !!!');
                    });
            }
        }

        // Save master detail data
        $scope.Save = function () {
            debugger;
            var lineProdeList = [];

            lineProdeList = $scope.ListLineProduct.filter(x => x.IS_CHECKED && !x.IS_EXIST);
            if (lineProdeList.length > 0) {

                var HeaderTokenPutPost = $scope.HeaderToken.post;
                var ModelsArray = [lineProdeList];
                var apiRoute = baseUrl + 'saveupdatelineproduct/';
                var DataMappingSaveUpdate = crudService.postMultipleModel(apiRoute, ModelsArray, HeaderTokenPutPost);
                DataMappingSaveUpdate.then(function (response) {
                    debugger;
                    if (response.resdata != "") {
                        if (response.resdata != '') {
                            ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                            ShowCustomToastr('info', response.resdata);
                        } else {
                            ShowCustomToastr('warning', 'Data already exists');
                        }
                    }
                    else if (response == "") {
                        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    }
                },
                    function (error) {
                        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    });
            } else {
                ShowCustomToastr('warning', 'Please select at least one item from list!!!!');
            }
        };

        // Delete master detail record
        //$scope.delete = function (dataModel) {
        //    $scope.cmnParam();
        //    objcmnParam.id = dataModel.USERROLEID;
        //    var ModelsArray = [objcmnParam];
        //    var apiRoute = baseUrl + 'delete/';
        //    var delDistributorTarget = crudService.postMultipleModel(apiRoute, ModelsArray, $scope.HeaderToken.delete);
        //    delDistributorTarget.then(function (response) {
        //        if (response.data.result != "") {
        //            $scope.loadDataMappingList(0);
        //            Command: toastr["success"](dataModel.DistributorTargetNo + " has been Deleted Successfully!!!!");
        //        }
        //        else {
        //            Command: toastr["warning"](dataModel.DistributorTargetNo + " not Deleted, Please Check and Try Again!");
        //        }
        //    },
        //        function (error) {
        //            Command: toastr["warning"](dataModel.DistributorTargetNo + " not Deleted, Please Check and Try Again!");
        //            console.log("Error: " + error);
        //        });
        //}

        // Clear info after sucessful transaction
        $scope.clear = function () {
            $scope.frmDataMapping.$setPristine();
            $scope.frmDataMapping.$setUntouched();

            $scope.COMPANY_OID = '';
            $scope.SDVNT_OID = '';
            $("#SDVNT_OID").select2("data", { id: '', text: '--Select Sales Group--' });

            $scope.skuList = [];
            $scope.SKU_OID = '';
            $("#SKU_OID").select2("data", { id: '', text: '--Select Product--' });

            $scope.CMN_OID = '';
            $scope.IS_ENABLE = false;
        };
        //**********---- End Clear records ----***************//


        $scope.printDiv = function printElem(print) {
            var content = document.getElementById(print).innerHTML;
            var mywindow = window.open('', 'Print', 'height=1000,width=2000');

            mywindow.document.write('<html><head><title></title>');
            mywindow.document.write('</head><body >');
            mywindow.document.write(content);
            mywindow.document.write('</body> <br/><br/>');
            mywindow.document.write('<footer> <font color="green">Powered By:</font><font color="blue"> onAir International Ltd. </font></footer>');
            mywindow.document.write('</html>');


            mywindow.document.close();
            mywindow.focus();
            mywindow.print();
            mywindow.close();
            return true;
        };

        $scope.exportToExcel = function () {
            var blob = new Blob([document.getElementById('print').innerHTML], {
                type: 'application/vnd.ms-excel;charset=charset=utf-8'
            });
            var url = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.download = 'DailyFeesCollection.xls';
            a.href = url;
            a.textContent = 'DailyFeesCollection.xls';
            a.click();
            //saveAs(blob, 'DailyFeesCollection.xls');
        };
    }
]);