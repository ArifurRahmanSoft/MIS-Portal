/// <reference path="../Service/CrudService.js" />
/// <reference path="../Service/CrudService.js" />
app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(false);
}]);
app.controller('TopNavigationBarCtrl', ['$scope', 'hostDire', '$localStorage', 'sideNavService', '$rootScope', '$window',
    function ($scope, hostDire, $localStorage, sideNavService, $rootScope, $window) {
        //***************** ComapnyDropDownNotify *****************************

        $scope.ListUserWiseCompany = [];
        $scope.sessionUserCompanyID = $('#hCompanyID').val();
        $scope.sessionLoggedUserID = $('#hUserID').val();
        $scope.sessionCompanyShortName = $('#hCompanyShortName').val();
        $scope.ModuleNameFont = " font-size: 13px !important;";
        if ($scope.sessionLoggedUserID == 1) {
            $scope.ModuleNameFont = " font-size: 9px !important;";
        }

        $scope.sessionUserCompanyName = "";
        $scope.ShowCompanyList = true;
        $scope.ShowCompanyChange = false;
        //$rootScope.CompanyFullName = "ABC";
        function SetCompanyName(ComapnyName) {
            var s = ComapnyName,
                a = s.split(' '),
                l = a.length,
                i = 0,
                result = "";
            for (; i < l; ++i) {
                result += a[i].charAt(0);
            }
            //$scope.sessionUserCompanyName = result;

            $scope.sessionUserCompanyName = $('#hCompanyShortName').val();
        }

        $scope.ChangeCompany = function (data) {
            var CompanyID = data.CompanyID;
            var CompanyName = data.CompanyName;
            var apiRoute = '/SystemCommon/Dashboard/' + 'ModifyCompanySession/' + CompanyID;
            var porcessMasterDetails = sideNavService.getByID(apiRoute);
            porcessMasterDetails.then(function (response) {
                SetCompanyName(data.CompanyName);
                $rootScope.CompanyFullName = data.CompanyName;
                $window.location.reload();
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }


        //********** Get TopMenus
        $scope.TopMenues = [];
        $scope.BaseName = "";
        $scope.BaseUrl = "";
        var requestedUrl = window.location.pathname;
        function loadRecords_Top(dataModuleID) {
            $.ajax({
                method: "get",
                url: hostDire + '/SystemCommon/api/SystemCommonLayout/GetTopMenu/' + $scope.sessionUserCompanyID + '/' + $scope.sessionLoggedUserID + '/' + $scope.sessionUserCompanyID,

                dataType: "json",
                contentType: "application/json",
                success: function (data) {
                    $scope.TopMenues = data;
                    angular.forEach($scope.TopMenues, function (value, key) {
                        if (value.ModulePath.toLowerCase() == requestedUrl.toLowerCase()) {
                            $scope.BaseName = value.ModuleName;
                            $scope.BaseUrl = value.ModulePath;
                            $rootScope.TopNavigationBarModuleID = value.ModuleID;
                        }
                    });
                },
                async: false,
                headers: '1'
            });
        }
        loadRecords_Top(0);
    }]);
