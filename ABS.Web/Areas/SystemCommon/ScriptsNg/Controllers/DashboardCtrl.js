
app.controller('dashboardCtrl', ['$scope', 'dashboardService', '$rootScope', '$localStorage','$http',
function ($scope, dashboardService, $rootScope, $localStorage,$http) {
        $scope.totalUser = '';
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

    //PageLoad
        function loadUserCommonEntity(num) {
            var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
            console.clear();
            $scope.UserCommonEntity = {}
            $scope.UserCommonEntity = pagedata;
            console.log($scope.UserCommonEntity);
        }
        loadUserCommonEntity(0);

        $rootScope.IsDashboard = 0;
    }]);

