/**
 * DashboardCtrl.js
 */
app.controller('dashboardCtrl', ['$scope', 'dashboardService', '$rootScope', '$localStorage','$http',
function ($scope, dashboardService, $rootScope, $localStorage, $http) {
     
        $scope.totalUser = '';
        var LoggedUserID = $('#hUserID').val();
        var LoggedCompanyID = $('#hCompanyID').val();

        $rootScope.IsDashboard = 0;
    }]);

