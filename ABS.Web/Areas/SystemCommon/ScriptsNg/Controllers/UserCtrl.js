
//app.controller('userCtrl', function ($scope, $http, crudService, $filter) {
app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}]);


app.controller('userCtrl', ['$scope', 'crudService', 'uiGridConstants', '$q', '$http', 'conversion',
    function ($scope, crudService, uiGridConstants, $q, $http, conversion) {
        var baseUrl = '/SystemCommon/api/User/';
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';
        $scope.gridOptionsUsers = [];
        var objcmnParam = {};

        $scope.onlyNumbers = /^\d+$/;
        var isExisting = 0;
        var page = 1;
        var pageSize = 20;
        var isPaging = 0;
        var inCallback = false;
        var totalData = 0;

        var LUserID = $('#hUserID').val();
        var LCompanyID = $('#hCompanyID').val();
        $scope.EditUserID = 0;
        $scope.IsOnlineAccount = false;
        $scope.PanelTitle = 'New User';
        $scope.DataPanelTitle = 'Existing User';
        $scope.listUser = [];
        var User = {};
        var OnlineAccount = 0;

        $scope.ngHPassword = false;
        $scope.ngHLoginID = false;
        $scope.loaderMore = false;
        $scope.loaderUpload = false;
        $scope.scrollended = false;

        //-------------Tab Name----------------
        $scope.ngComPerMission = true;
        $scope.ngJobContract = true;
        $scope.ngLedger = true;
        $scope.ngPresentAddress = true;
        $scope.ngPermanentAddress = true;
        $scope.ngPersonalInfo = true;
        $scope.ngAccountInfo = true;
        $scope.TabActive = "";
        // for Personal Info
        $scope.ngPersonalInfoforEmployeeDesign = true;
        $scope.ngPersonalInfoforSupplier = false;
        $scope.ngPersonalInfoforBuyer = false;
        $scope.ngPersonalInfoforBuyerReff = false;
        $scope.ngPersonalInfoforPartyContactPersonDesign = false;
        $scope.ngPersonalInfoforSupplierContactPersonDesign = false;

        $scope.TabManageByUserTyeID = function () {

            if ($scope.UserType == 1) { // For User
                $scope.ActiveAccountInfo = "active";
                $scope.ACTypeID = 4;
            }
        }
        $scope.TabManageByUserTyeID();

        function LoadCTGStaff() {
            objcmnParam = {
                //pageNumber: $scope.pagination.pageNumber,
                pageNumber: 1,
                pageSize: 50,
                IsPaging: 0,
                loggeduser: 1,
                loggedCompany: 1,
                menuId: 5,
                tTypeId: 25
            };
            var apiRoute = sysCommonUrl + 'GetCTGStaff/';
            var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
            var listCTGStaff = crudService.GetList(apiRoute, cmnParam);
            listCTGStaff.then(function (response) {
                $scope.listCTGStaff = response.data.listCTGStaff;
            },
                function (error) {
                    console.warn("Error: " + error);
                });
        }
        LoadCTGStaff();


        $scope.ChangeOnStaffId = function () {
            $scope.LoginIDEmp = $scope.StaffId;
        }


        //Pagination
        $scope.pagination = {
            paginationPageSizes: [15, 25, 50, 75, 100, 500, 1000, "All"],
            ddlpageSize: 15, pageNumber: 1, pageSize: 15, totalItems: 0,

            getTotalPages: function () {
                return Math.ceil(this.totalItems / this.pageSize);
            },
            pageSizeChange: function () {
                if (this.ddlpageSize == "All")
                    this.pageSize = $scope.pagination.totalItems;
                else
                    this.pageSize = this.ddlpageSize

                this.pageNumber = 1
                $scope.loadRecordsUser(1);
            },
            firstPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber = 1
                    $scope.loadRecordsUser(1);
                }
            },
            nextPage: function () {
                if (this.pageNumber < this.getTotalPages()) {
                    this.pageNumber++;
                    $scope.loadRecordsUser(1);
                }
            },
            previousPage: function () {
                if (this.pageNumber > 1) {
                    this.pageNumber--;
                    $scope.loadRecordsUser(1);
                }
            },
            lastPage: function () {
                if (this.pageNumber >= 1) {
                    this.pageNumber = this.getTotalPages();
                    $scope.loadRecordsUser(1);
                }
            }
        };

        //**********----Get All Record----***************
        $scope.loadRecordsUser = function (isPaging) {

            // For Loading
            $scope.loaderMore = true;
            $scope.lblMessage = 'loading please wait....!';
            $scope.result = "color-red";

            //Ui Grid
            objcmnParam = {
                pageNumber: $scope.pagination.pageNumber,
                pageSize: $scope.pagination.pageSize,
                IsPaging: isPaging,
                loggeduser: LUserID,
                loggedCompany: LCompanyID
            };

            $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                if (col.filters[0].term) {
                    return 'header-filtered';
                } else {
                    return '';
                }
            };

            $scope.gridOptionsUsers = {
                columnDefs: [
                    { name: "USERID", displayName: "Login ID", width: '10%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "USERFULLNAME", displayName: "Full Name", width: '20%', headerCellClass: $scope.highlightFilteredHeader },
                    { name: "ENCRYPTPASSWORD", displayName: "Password", width: '10%', visible: false, headerCellClass: $scope.highlightFilteredHeader },
                    { name: "AEMP_OMAIL", displayName: "Staff Email", width: '30%', headerCellClass: $scope.highlightFilteredHeader },
                    {
                        name: 'Edit',
                        displayName: "Edit",
                        width: '7%',
                        enableColumnResizing: false,
                        enableFiltering: false,
                        enableSorting: false,
                        headerCellClass: $scope.highlightFilteredHeader,
                        cellTemplate: '<span class="label label-warning label-mini">' +
                            '<a ng-href="#userModal" data-toggle="modal" class="bs-tooltip" title="Edit Info">' +
                            '<i class="icon-pencil" ng-click="grid.appScope.getUser(row.entity)"></i>' +
                            '</a>' +
                            '</span>'
                        //+
                        //            '<span class="label label-danger label-mini">' +
                        //                '<a href="javascript:void(0);" class="bs-tooltip" title="Delete" ng-click="grid.appScope.delete(row.entity)">' +
                        //                    '<i class="icon-trash"></i>' +
                        //                '</a>' +
                        //            '</span>'
                    }

                ]
                ,

                enableFiltering: true,
                enableGridMenu: true,
                enableSelectAll: true,
                exporterCsvFilename: 'Users.csv',
                exporterPdfDefaultStyle: { fontSize: 9 },
                exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
                exporterPdfHeader: { text: "User", style: 'headerStyle' },
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

            var apiRoute = baseUrl + 'GetUser/';
            var listAllUser = crudService.getAllUsers(apiRoute, objcmnParam);
            listAllUser.then(function (response) {
                $scope.pagination.totalItems = response.data.recordsTotal;
                $scope.gridOptionsUsers.data = response.data.listUsers;
                $scope.loaderMore = false;
                //$scope.listUser = response.data
                $scope.ManageValidation();
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };
        $scope.loadRecordsUser(0);
        //----------Start Manage Validation and Grid Hide Show--------------------------
        $scope.ManageValidation = function () {
            if ($scope.UserType == 1) { // For User

                $scope.IsLoginIDRequired = false;
                $scope.IsRequiredFirstName = true;
                $scope.IsJCDepartmentRequired = true;
                $scope.IsDefultCompanyRequired = true;
                $scope.IsEmailRequired = true;
                $scope.IsRequiredLastName = true;
                $scope.IsRequiredJobTitle = true;
                $scope.IsJCDesignationRequired = true;
                $scope.gridOptionsUsers.columnDefs[0].visible = false;
                $scope.gridOptionsUsers.columnDefs[1].visible = false;
                $scope.gridOptionsUsers.columnDefs[2].visible = false;
                $scope.gridOptionsUsers.columnDefs[3].visible = true;
                $scope.gridOptionsUsers.columnDefs[4].visible = false;
                $scope.gridOptionsUsers.columnDefs[5].visible = true;
                $scope.gridOptionsUsers.columnDefs[6].visible = false;
                $scope.gridOptionsUsers.columnDefs[7].visible = false;
                $scope.gridOptionsUsers.columnDefs[8].visible = false;
                $scope.gridOptionsUsers.columnDefs[9].visible = true;
                $scope.gridOptionsUsers.columnDefs[10].visible = true;
                $scope.gridOptionsUsers.columnDefs[11].visible = true;
                $scope.gridOptionsUsers.columnDefs[12].visible = false;
                $scope.gridOptionsUsers.columnDefs[13].visible = false;
                $scope.Star = '*';
            }
        }
        $scope.ManageValidation();
        //----------Start Manage Validation and Grid Hide Show--------------------------

        $scope.fillupLoginIDnPassword = function () {
            if ($scope.EmailEmp != "") {
                $scope.LoginIDEmp = $scope.EmailEmp.split('@')[0];
                $scope.passwordEmp = $scope.EmailEmp.split('@')[0];
            }
        }

        //**********----Create New Record----***************
        $scope.save = function () {
            debugger
            var shwMsgAfterSaveUserWise = "";

            if ($scope.EditUserID > 0) {
                shwMsgAfterSaveUserWise = "User Updated";
            }
            else {
                shwMsgAfterSaveUserWise = "User Saved";
            }

            User = {
                CompanyID: LCompanyID,
                LoggedUser: LUserID,
                UserID: $scope.EditUserID,
                LoginID: $scope.LoginIDEmp,
                Password: $scope.passwordEmp == undefined ? '' : $scope.passwordEmp
            };

            var apiRoute = baseUrl + 'SaveUser/';
            var UserCreate = crudService.postList(apiRoute, User);
            UserCreate.then(function (response) {
                if (response.data === 1) {
                    Command: toastr["info"](" " + shwMsgAfterSaveUserWise + " successfully. ");
                    modal_fadeOut();
                    $scope.loadRecordsUser(0);
                    $scope.clear();
                }
                else if (response.data === 0) {
                    Command: toastr["error"]("Error while " + shwMsgAfterSaveUserWise + "!");
                }
                else {
                    Command: toastr["error"]("Error Undefined!");
                }
            },
                function (error) {
                    console.log("Error: " + error);
                });
        };

        $scope.getUser = function (dataModel) {
            debugger
            $scope.EditUserID = dataModel.USERID;
            $scope.LoginIDEmp = dataModel.USERID;
            $scope.passwordEmp == dataModel.ENCRYPTPASSWORD;
        };

        $scope.delete = function (dataModel) {
            var IsConf = confirm('You are about to delete ' + dataModel.UserFullName + '. Are you sure?');
            if (IsConf) {
                var apiRoute = baseUrl + 'DeleteUser/' + dataModel.UserID + '/' + LCompanyID + '/' + LUserID;
                var UserDelete = crudService.delete(apiRoute);
                UserDelete.then(function (response) {
                    if (response.data === 1) { Command: toastr["info"]("" + dataModel.UserType + " Deleted successfully!"); }
                    else { Command: toastr["error"]("Error while Deleting!"); }
                    $scope.loadRecordsUser(0);
                    $scope.ManageValidation();
                }, function (error) {
                    console.log("Error: " + error);
                });
            }
        }

        $scope.clear = function () {
            $scope.LoginIDEmp = "";
            $scope.passwordEmp = "";

            $scope.EmployeeIDEmp = "";

            $scope.EditUserID = 0;
            $scope.frmUser.$setPristine();
            $scope.frmUser.$setUntouched();

            $scope.PanelTitle = 'New User';

            $scope.password = "";

            //-----------END-------------

            $scope.LoginID = '';
            $scope.FirstName = '';
            $scope.MiddleName = '';
            $scope.LastName = '';
            $scope.Email = '';
            $scope.Phone = '';
            $scope.emMobile = "";

            $scope.EmployeeID = "";
            $scope.Telephone = "";
            $scope.emMobile = "";

            $scope.Appointment = "";
            $scope.Remarks = "";
            $scope.ContractEmailID = "";

            $scope.TabManageByUserTyeID();
            $scope.ManageValidation();
        };

        $scope.CheckLoginID = function () {
            if ($scope.UserType == 1) {
                CmnUserAuthentication = {
                    LoginID: $scope.LoginID,
                };
                var apiRoute = baseUrl + 'CheckLoginID/';
                var itemGroup = crudService.checkedUserLoginID(apiRoute, CmnUserAuthentication);
                itemGroup.then(function (response) {
                    if (response.data === 1) {
                        Command: toastr["warning"]($scope.LoginID + " " + "This Login ID already existing");
                        $scope.LoginID = "";
                    }
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
        }

        $scope.CreateAccount = function () {
            if ($scope.IsOnlineAccount == true) {
                $scope.LoginID = '';
                $scope.password = '';
                $scope.ngHPassword = true;
                $scope.ngHLoginID = true;
                $scope.IsLoginIDRequired = true;
            } else {
                $scope.LoginID = '';
                $scope.password = '';
                $scope.ngHPassword = false;
                $scope.ngHLoginID = false;
                $scope.IsLoginIDRequired = false;
            }
        }

    }]);

function modal_fadeOut() {
    $("#userModal").fadeOut(200, function () {
        $('#userModal').modal('hide');
    });
}