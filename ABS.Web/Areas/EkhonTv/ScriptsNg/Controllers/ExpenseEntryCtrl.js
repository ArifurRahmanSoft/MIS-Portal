//Enable Debug Info for Document Upload
app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);

app.controller('ExpenseEntryCtrl', ['$scope', 'ExpenseEntryService', 'crudService', '$http', 'conversion', '$filter', 'uiGridConstants', '$localStorage',
    function ($scope, ExpenseEntryService, crudService, $http, conversion, $filter, uiGridConstants, $localStorage) {

        var objcmnParam = {};
        var sysComDdlUrl = '/SystemCommon/api/SystemCommonDDL/';
        var baseUrl = '/EkhonTv/api/ExpenseEntry/';
        var isExisting = 0;
        var page = 0;
        var pageSize = 100;
        var isPaging = 0;
        var totalData = 0;

        $scope.EXPENSEID = 0;

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.IsUpdateMode = false;
        $scope.listSupplierContact = [];

        $scope.CustomCode = "";
        $scope.CustomerOrderID = "0";
        $scope.QuotationContactPersonID = "0";
        $scope.IsListIcon = true;
        $scope.IsCreateIcon = false;
        $scope.Invalid = true;

        var date = new Date();
        $scope.ReceivedDate = ('0' + date.getDate()).slice(-2) + '/' + ('0' + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();

        $scope.FullFormateDate = [];
        $scope.ListCompany = [];
        $scope.bool = true;
        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";
        $scope.PageTitle = 'Expense Entry';
        $scope.ListTitle = 'Expense Information';
        $scope.ListTitleRequisitionMaster = 'Expense Information';
        $scope.ListTitleQuotationMasters = 'Order Information';
        $scope.ListTitleQuotationContact = 'Contact Person';
        $scope.ListTitleSampleNo = 'Product Info';
        $scope.ListTitleQuotationDeatails = 'Listed of Order';

        $scope.listExpenseAreasGrid = [];
        $scope.listExpenseAreasGrid = false;
        $scope.listExpenseAreas = [];
        $scope.AllCostTogether = 0;

        //Ui Grid
        $scope.gridExpenseMaster = [];

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
                $scope.IsShowD = $scope.listExpenseAreasGrid.length > 0 ? true : false;
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
                loadExpenseMasterRecords(0);
            }
        }

        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmDocuments'; DelFunc = 'DeleteUpdateMasterDetail'; DelMsg = 'DocumentNo'; EditFunc = 'GetExpenseByID';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all*************************************************** 

        $scope.LoadFloorFlatByBuilding = function () {

            $scope.listFloorFlat = [];
            $scope.ngmFloorFlat = "";
            $("#ddlFloorFlat").select2("data", { id: '', text: '--Select Floor/Flat--' });

            $scope.ngmExpenseArea = "";
            $("#ddlExpenseArea").select2("data", { id: '', text: '--Expense Area--' });

            $scope.ngmExpenseArea = '';
            $scope.ngmRequisitionBy = '';
            $scope.ngmDetailsOfExpense = '';
            $scope.ngmExpenseDate = '';
            $scope.ngmApprovedBy = '';
            $scope.ngmTotalCost = '';
            $scope.listExpenseAreas = [];

            if ($scope.ngmBuilding > 0) {
                objcmnParam = {
                    id: $scope.ngmBuilding
                };
                var apiRoute = sysComDdlUrl + 'LoadFloorFlatByBuilding/';
                var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
                var floorFlat = crudService.GetList(apiRoute, cmnParam);
                floorFlat.then(function (response) {
                    $scope.listFloorFlat = response.data.objFloorFlat;
                },
                    function (error) {
                        console.warn("Error: " + error);
                    });
            }
        }

        $scope.ExpenseAddGridView = function () {
            debugger

            if ($scope.ngmBuilding == "" || $('#ddlExpenseArea').val() == ""
                || $('#dtExpenseDate').val() == "" || $('#txtTotalCost').val() == "") {
                Command: toastr["warning"]("Please ensure mandatory data input !");
                return false;
            }

            $scope.AllCostTogether = $scope.AllCostTogether + $scope.ngmTotalCost;

            $scope.listExpenseAreasGrid = true;
            debugger
            $scope.listExpenseAreas.push({
                FLOORFLATNO: $scope.ngmFloorFlat == (null || "" || undefined) ? '' : $scope.ngmFloorFlat.FLOORFLATNO,
                EXPENSEAREAHEADID: $scope.ngmExpenseArea.EXPENSEID,
                ExpenseAreaName: $scope.ngmExpenseArea.EXPENSEHEADNAME,
                ExpenseDetail: $scope.ngmDetailsOfExpense,
                ExpenseDateDisplay: $scope.ngmExpenseDate,
                ExpenseDate: $scope.ngmExpenseDate == null || "" || undefined ? null : conversion.getStringToDate($scope.ngmExpenseDate),
                TOTALCOST: $scope.ngmTotalCost,
                RequisitionBy: $scope.ngmRequisitionBy,
                ApprovedBy: $scope.ngmApprovedBy
            });

            $scope.ngmFloorFlat = "";
            $("#ddlFloorFlat").select2("data", { id: '', text: '--Select Floor/Flat--' });

            $scope.ngmExpenseArea = "";
            $("#ddlExpenseArea").select2("data", { id: '', text: '--Expense Area--' });

            $scope.ngmDetailsOfExpense = '',
                $scope.ngmExpenseDate = '',
                $scope.ngmTotalCost = '',
                $scope.ngmRequisitionBy = '',
                $scope.ngmApprovedBy = ''
        };

        $scope.deleteExpenseFromList = function (element, index) {
            var IsConf = confirm('You are about to delete an expense. Are you sure?');
            if (IsConf) {
                $scope.listExpenseAreas.splice(index, 1);
                $scope.AllCostTogether = $scope.AllCostTogether - element.TOTALCOST;
            }
        };


        function LoadBuildingRecord() {
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
        LoadBuildingRecord();

        function LoadExpenseHead() {
            var apiRoute = sysComDdlUrl + 'GetExpenseArea/' + $scope.UserCommonEntity.loggedCompnyID + '/' + $scope.UserCommonEntity.loggedUserID + '/';
            var expenseHead = crudService.getAll(apiRoute, page, pageSize, isPaging);
            expenseHead.then(function (response) {
                $scope.ListExpenseHead = response.data;
            },
                function (error) {
                    console.warn("Error: " + error);
                });
        }
        LoadExpenseHead();

        $scope.pagination = {
            paginationPageSizes: [10, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 10, pageNumber: 1, pageSize: 10, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.pagination.totalItems;
                else
                    this.pageSize = this.ddlpageSize
                this.pageNumber = 1

                loadExpenseMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    loadExpenseMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    loadExpenseMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    loadExpenseMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    loadExpenseMasterRecords(1);
                }
            }
        };

        function loadExpenseMasterRecords(isPaging) {
            // For Paging
            if (isPaging == 0)
                $scope.pagination.pageNumber = 1;

            // For Loading
            $scope.loaderMoreIssueMaster = true;
            $scope.lblMessageForQCMaster = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: (($scope.pagination.pageNumber - 1) * $scope.pagination.pageSize),
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridExpenseMaster = {
                columnDefs: [
                    { name: "EXPENSEID", displayName: "EXPENSEID", title: "EXPENSEID", visible: false, width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BUILDINGID", displayName: "BUILDINGID", title: "BUILDINGID", visible: false, width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "BUILDINGNAME", displayName: "BUILDING NAME", title: "BUILDING NAME", width: '40%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "EXPENSEMONTH", displayName: "EXPENSE MONTH", title: "EXPENSE MONTH", width: '15%', cellFilter: 'date:"MMMM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "TOTALCOST", displayName: "TOTAL COST", title: "TOTAL COST", width: '15%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "CREATEON", displayName: "ENTRY DATE", title: "ENTRY DATE", width: '15%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Action',
                        displayName: "Action",
                        pinnedRight: true,
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        width: '12%',
                        headerCellClass: $scope.highlightFilteredHeader,
                        visible: $scope.UserCommonEntity.visible,

                        cellTemplate: '<span class="label label-success label-mini" >' +
                            '<a href="" title="Edit" ng-click="grid.appScope.GetExpenseByID(row.entity)">' +
                            '<i class="icon-check" aria-hidden="true"></i> Edit' +
                            '</a>' +
                            '</span>' +
                            '<span class="label label-info label-mini" >' +
                            '<a href="" title="Print" ng-click="grid.appScope.PrintExpenseById(row.entity)">' +
                            '<i class="icon-print" aria-hidden="true"></i> Print' +
                            '</a>' +
                            '</span>'
                    }
                ],
                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'TenantTagging.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "Tenant Tagging", style: 'headerStyle' },
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

            var apiRoute = baseUrl + 'GetExpenseMaster/';
            var listTenantTagMaster = ExpenseEntryService.getExpenseMasterList(apiRoute, objcmnParam);
            listTenantTagMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridExpenseMaster.data = response.data.objExpenseMaster;
                $scope.loaderMore = false;
            },
                function (error) {
                    //console.log("Error: " + error);
                });
        }

        loadExpenseMasterRecords(0);

        $scope.GetExpenseByID = function (dataModel) {

            $scope.IsUpdateMode = true;
            $scope.BuildingId = "";
            $scope.cmnParam();
            $scope.btnSaveText = "Update";
            $scope.IsHidden = true;
            $scope.btnShowText = "Show List";
            $scope.IsListIcon = true;
            $scope.IsCreateIcon = false;
            $scope.IsShow = true;

            debugger

            $scope.EXPENSEID = dataModel.EXPENSEID;

            if (dataModel.BUILDINGID > 0) {
                $scope.ngmBuilding = dataModel.BUILDINGID;
                $("#ddlBuilding").select2("data", { id: dataModel.BUILDINGID, text: dataModel.BUILDINGNAME });
            }

            // getting flat data as per builing

            debugger

            $scope.listFloorFlat = [];
            $scope.ngmFloorFlat = "";
            $("#ddlFloorFlat").select2("data", { id: '', text: '--Select Floor/Flat--' });

            if ($scope.ngmBuilding > 0) {
                objcmnParam = {
                    id: $scope.ngmBuilding
                };
                var apiRoute = sysComDdlUrl + 'LoadFloorFlatByBuilding/';
                var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
                var floorFlat = crudService.GetList(apiRoute, cmnParam);
                floorFlat.then(function (response) {
                    $scope.listFloorFlat = response.data.objFloorFlat;
                },
                    function (error) {
                        console.warn("Error: " + error);
                    });
            }



            objcmnParam = {
                pageNumber: 0,
                pageSize: 0,
                IsPaging: 0,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            $scope.listExpenseAreas = [];

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify($scope.EXPENSEID) + "]";

            var apiRoute = baseUrl + 'GetExpenseByID/';

            $scope.AllCostTogether = dataModel.TOTALCOST;

            var ExpenseDetail = ExpenseEntryService.GetList(apiRoute, cmnParam);

            ExpenseDetail.then(function (response) {

                angular.forEach(response.data.ExpenseList, function (exp) {
                    debugger
                    $scope.listExpenseAreas.push({
                        EXPENSEAREAHEADID: exp.EXPENSEAREAHEADID,
                        ExpenseAreaName: exp.EXPENSEHEADNAME,

                        RequisitionBy: exp.REQUISITIONBY == null ? '' : exp.REQUISITIONBY,
                        ApprovedBy: exp.APPROVEDBY == null ? '' : exp.APPROVEDBY,
                        ExpenseDetail: exp.EXPENSEDETAIL == null ? '' : exp.EXPENSEDETAIL,

                        ExpenseDate: exp.EXPENSEDATE,
                        ExpenseDateDisplay: exp.EXPENSEDATE == null || "" || undefined ? '' : conversion.getDateToString(exp.EXPENSEDATE),
                        TOTALCOST: exp.TOTALCOST,
                        FLOORFLATNO: (exp.FLOORNO == null ? '' : exp.FLOORNO) + ' - ' + (exp.FLATNO == null ? '' : exp.FLATNO)

                    })
                });

                $scope.listExpenseAreasGrid = true;
            },
                function (error) {
                    $scope.IsShow = true;
                    $scope.IsShowD = false;
                });
        }

        $scope.Save = function () {

            debugger

            //if ($('#ddlBuilding').val() == "") {
            //    Command: toastr["warning"]("Please ensure mandatory data input !");
            //    return false;
            //}

            if ($scope.listExpenseAreas == 0) {
                Command: toastr["warning"]("Please input expense data !");
                return false;
            }

            objcmnParam = {
                pageNumber: page,
                pageSize: pageSize,
                IsPaging: isPaging,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            var HedarTokenPostPut = $scope.EXPENSEID > 0 ? $scope.HeaderToken.put : $scope.HeaderToken.post;

            debugger

            var ExpenseMaster = {
                BUILDINGID: $scope.ngmBuilding,
                EXPENSEID: $scope.EXPENSEID,
                EXPENSEMONTH: $scope.ngmFromMonth != (null || undefined) ? conversion.getStringToDateMMYYFormat($scope.ngmFromMonth) : $scope.ngmFromMonth,
                CreateBy: $scope.UserCommonEntity.loggedUserID,
                CompanyID: $scope.UserCommonEntity.loggedCompnyID,
                IsDeleted: false
            };

            var apiRoute = baseUrl + 'SaveUpdateExpense/';

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(ExpenseMaster) + ","
                + JSON.stringify($scope.listExpenseAreas) + "]";

            var DocumentCreateUpdate = ExpenseEntryService.GetList(apiRoute, cmnParam, HedarTokenPostPut);
            DocumentCreateUpdate.then(function (response) {
                if (response.data != '') {

                    if (response.data == 'saved') {
                        Command: toastr["success"]("Expense Information saved successfully.");
                        $scope.clear();
                    }
                    else if (response.data == 'updated') {
                        Command: toastr["success"]("Expense Information updated successfully.");
                        $scope.clear();
                    }
                    else if (response.data == -1) {
                        Command: toastr["warning"]("Any type of entry for this selected month is locked !!!");
                    }
                    else {
                        Command: toastr["error"](response.data);
                    }
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

        $scope.clear = function () {

            $scope.invalid = false;
            $("#save").prop("disabled", false);

            $scope.IsUpdateMode = false;
            $scope.btnSaveText = "Save";
            $scope.btnShowText = "Show List";
            $scope.IsHidden = true;

            $scope.EXPENSEID = 0;

            $scope.ngmFromMonth = "";

            $scope.ngmChequeNo = "";
            $scope.ngmBranchName = "";
            $scope.ngmChequeAmount = "";
            $scope.ngmAccountName = "";


            $scope.IsShow = true;
            $scope.IsShowD = false;
            $scope.IsListIcon = true;
            $scope.IsCreateIcon = false;
            $scope.IsHiddenDetail = true;

            $scope.ngmBuilding = "";
            $("#ddlBuilding").select2("data", { id: '', text: '--Select Building--' });

            $scope.listFloorFlat = [];
            $scope.ngmFloorFlat = "";
            $("#ddlFloorFlat").select2("data", { id: '', text: '--Select Floor/Flat--' });

            $scope.listExpenseAreas = [];

        };

        $scope.listExpenseAreasPrint = [];
        $scope.PrintExpenseById = function (dataModel) {
            objcmnParam = {
                pageNumber: 0,
                pageSize: 0,
                IsPaging: 0,
                loggeduser: $scope.UserCommonEntity.loggedUserID,
                loggedCompany: $scope.UserCommonEntity.loggedCompnyID,
                menuId: $scope.UserCommonEntity.currentMenuID,
                tTypeId: $scope.UserCommonEntity.currentTransactionTypeID
            };

            $scope.listExpenseAreasPrint = [];

            var cmnParam = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(dataModel.EXPENSEID) + "]";

            var apiRoute = baseUrl + 'GetExpenseByID/';

            $scope.AllCostTogethers = dataModel.TOTALCOST;

            var ExpenseDetail = ExpenseEntryService.GetList(apiRoute, cmnParam);

            ExpenseDetail.then(function (response) {

                angular.forEach(response.data.ExpenseList, function (exp) {
                    debugger
                    $scope.listExpenseAreasPrint.push({
                        EXPENSEAREAHEADID: exp.EXPENSEAREAHEADID,
                        ExpenseAreaName: exp.EXPENSEHEADNAME,

                        RequisitionBy: exp.REQUISITIONBY == null ? '' : exp.REQUISITIONBY,
                        ApprovedBy: exp.APPROVEDBY == null ? '' : exp.APPROVEDBY,
                        ExpenseDetail: exp.EXPENSEDETAIL == null ? '' : exp.EXPENSEDETAIL,

                        ExpenseDate: exp.EXPENSEDATE,
                        ExpenseDateDisplay: exp.EXPENSEDATE == null || "" || undefined ? '' : conversion.getDateToString(exp.EXPENSEDATE),
                        TOTALCOST: exp.TOTALCOST,
                        FLOORFLATNO: (exp.FLOORNO == null ? '' : exp.FLOORNO) + ' - ' + (exp.FLATNO == null ? '' : exp.FLATNO)

                    });                    
                });

                if ($scope.listExpenseAreasPrint.length > 0) {
                    setTimeout(function () {
                        $scope.printDiv("print");
                    }, 10);
                }
            },
                function (error) {
                });
        }


        $scope.printDiv = function (print) {
            //debugger;
            //$scope.isprint = false;
            var content = document.getElementById(print).innerHTML;
            var mywindow = window.open('', 'Print', 'height=1000,width=2000');
            var is_chrome = Boolean(mywindow.chrome);
            mywindow.document.write('<html><head><title>Expense Area</title>');
            mywindow.document.write('</head><body >');
            mywindow.document.write(content);
            mywindow.document.write('</body> <br/><br/>');
            mywindow.document.write('<footer style="position: fixed;bottom: 0;"> <font color="green">Powered By:</font><font color="blue"> City Group. </font></footer>');
            mywindow.document.write('</html>');

            if (is_chrome) {
                setTimeout(function () { // wait until all resources loaded 
                    mywindow.document.close(); // necessary for IE >= 10
                    mywindow.focus(); // necessary for IE >= 10
                    mywindow.print(); // change window to winPrint
                    mywindow.close(); // change window to winPrint
                }, 100);
            } else {
                mywindow.document.close(); // necessary for IE >= 10
                mywindow.focus(); // necessary for IE >= 10

                mywindow.print();
                mywindow.close();
            }
            return true;
        };
    }]);
