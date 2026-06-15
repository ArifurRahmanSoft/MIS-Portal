app.controller('BreadcrumbCtrl', ['$scope', 'hostDire', '$localStorage', '$rootScope', '$http',
    function ($scope, hostDire, $localStorage, $rootScope, $http) {
    //************ Load User Common Entity **********************
   // 
    var IsDashboard = $rootScope.IsDashboard;
    $scope.BreadcrumbsList = [];
    function loadUserCommonEntity(num) {
        debugger
        var pagedata = $scope.menuManager.LoadPageMenu(window.location.pathname);
        console.clear();
        $scope.UserCommonEntity = {}
        $scope.UserCommonEntity = pagedata;

        debugger

        if (IsDashboard != 0) {
            var apiRoute = hostDire+'/SystemCommon/api/SystemCommonLayout/GetBreadCrums/' + $scope.UserCommonEntity.currentMenuID;
            var listBreadCrums = $http.get(apiRoute);
            listBreadCrums.then(function (response) {
                $scope.BreadcrumbsList = response.data;
            },
            function (error) {
                console.log("Error: " + error);
            });
        }
        else {
            $scope.BreadcrumbsList = [{ Name: "Dashboard", Icon: "icon-home" }];
        }
    }
    loadUserCommonEntity(0);
}]);

