
app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);


app.controller('TentativeSaleCtrl', ['$scope', 'crudService', 'uiGridConstants', '$q', '$http', 'conversion', '$filter', '$localStorage',
    function ($scope, crudService, uiGridConstants, $q, $http, conversion, $filter, $localStorage) {

        var baseUrl = "/Sales/api/TentativeSale/";
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';

        $scope.gridOptionATRS = [];
        var objcmnParam = {};

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};

        $scope.bulkhide = true;
        $scope.marketinghide = true;
        $scope.saletype = '';

        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";

        $scope.PageTitle = 'Tentative Sales';
        $scope.ListTitle = 'Tentative Sales';

        $scope.PageTitle = 'Tentative Sales';
        $scope.ListTitle = 'Tentative Sales';
        $scope.ListTitleMrrMasters = 'Tentative Sales';

        $scope.CollectorId = 0;

        $scope.listUser = [];
        var User = {};
        
        $scope.IsListIcon = true;
        $scope.IsCreateIcon = false;
        $scope.Invalid = true;

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
                loadMotherVesselMasterRecords(0);
            }
        };

        function loadUserCommonEntity(num) {
            // debugger
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = $scope.menuManager.LoadPageMenu(window.location.pathname);
            //  console.clear();
            //  debugger
            //Coming from SideNavCrl  
            $scope.permissionPageVisibility = true;
            $scope.generateSecurityParam = {};
            $scope.generateSecurityParam.MenuID = $scope.UserCommonEntity.currentMenuID;
            $scope.generateSecurityParam.CompanyID = $scope.UserCommonEntity.loggedCompnyID;

            $scope.HeaderToken = {};
            $scope.generateSecurityParam.methodtype = 'get';
            $scope.HeaderToken.get = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };
            $scope.generateSecurityParam.methodtype = 'put';
            $scope.HeaderToken.put = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };
            $scope.generateSecurityParam.methodtype = 'post';
            $scope.HeaderToken.post = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };
            $scope.generateSecurityParam.methodtype = 'delete';
            $scope.HeaderToken.delete = { 'AuthorizedToken': $scope.tokenManager.generateSecurityToken($scope.generateSecurityParam) };

        }
        loadUserCommonEntity(0);

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

                loadMotherVesselMasterRecords(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    loadMotherVesselMasterRecords(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    loadMotherVesselMasterRecords(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    loadMotherVesselMasterRecords(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    loadMotherVesselMasterRecords(1);
                }
            }
        };
        function loadMotherVesselMasterRecords(isPaging) {
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

            $scope.gridOptionATRS = {
                columnDefs: [
                    { name: "OID", displayName: "OID", title: "OID", visible: false, width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "LOCATION", displayName: "LOCATION", title: "LOCATION", width: '28%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "AMOUNT", displayName: "AMOUNT", title: "AMOUNT", width: '28%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "SALEDATE", displayName: "SALE DATE", title: "SALE DATE", width: '25%', cellFilter: 'date:"dd-MM-yyyy"', headerCellClass: $scope.highlightFilteredHeader },
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
                            '<a href="" title="Edit" ng-click="grid.appScope.GetDetail(row.entity)">' +
                            '<i class="icon-check" aria-hidden="true"></i> Edit' +
                            '</a>' +
                            '</span>'
                    }
                ],
                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'MotherVesselReceive.csv',
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

            var apiRoute = baseUrl + 'GetTentativeSale/';
            var listTenantTagMaster = crudService.getMVMasterList(apiRoute, objcmnParam);
            listTenantTagMaster.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionATRS.data = response.data.listUsers;
                
                $scope.loaderMore = false;
            },
                function (error) {
                    //console.log("Error: " + error);
                });
        }
        loadMotherVesselMasterRecords(0);


        ///// Get all Locations 
        function loadLocationRecords(isPaging) {

            var apiRoute = sysCommonUrl + 'GetSalesLocation/';
            var listLocation = crudService.getItemWithcmnParam(apiRoute, objcmnParam, $scope.HeaderToken.get);
            listLocation.then(function (response) {
                $scope.listLocation = response.data.objLocationList;
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        loadLocationRecords(0);     

        $scope.GetDetail = function (datamodel) {

            $scope.IsUpdateMode = true;
            $scope.BuildingId = "";
            $scope.btnSaveText = "Update";
            $scope.IsHidden = true;
            $scope.btnShowText = "Show List";
            $scope.IsListIcon = true;
            $scope.IsCreateIcon = false;
            $scope.IsShow = true;

            $scope.OID = datamodel.OID,
                $scope.AMOUNT = datamodel.AMOUNT;
            $scope.ngModelLocation = datamodel.LOCATIONOID;
            $("#ddlLocation").select2("data", { id: datamodel.LOCATIONOID, text: datamodel.LOCATION });
        };

        $scope.Save = function () {

            Collector =
                {
                    ENTRYUSER: $scope.UserCommonEntity.loggedUserID,
                    OID: $scope.OID,
                    AMOUNT: $scope.AMOUNT,
                    LOCATIONOID: $scope.ngModelLocation
                };
            var apiRoute = baseUrl + 'SaveTentativeSale/';
            var UserCreate = crudService.postList(apiRoute, Collector);
            UserCreate.then(function (response) {
                debugger
                if (response.data == "duplicate") {
                    Command: toastr["warning"]("Tentative sale already exist for this location !!!");
                    modal_fadeOut();
                }
                else if (response.data != "") {
                    Command: toastr["info"]("Data Saved Successfully!");
                    modal_fadeOut();
                    $scope.clear();
                }
                else {
                    Command: toastr["error"](response.data);
                }
            },
                function (error) {
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

            $scope.IsShow = true;
            $scope.IsShowD = false;
            $scope.IsListIcon = true;
            $scope.IsCreateIcon = false;
            $scope.IsHiddenDetail = true;

            $scope.OID = null;
            $scope.AMOUNT = '';
            $scope.ngModelLocation = '';
            $("#ddlLocation").select2("data", { id: '', text: '--Select Location--' });
        };

    }]);

function modal_fadeOut() {
    $("#userModal").fadeOut(200, function () {
        $('#userModal').modal('hide');
    });
}