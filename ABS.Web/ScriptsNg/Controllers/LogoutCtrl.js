/**
 * LogoutCtrl.js
 */

app.controller('LogoutController', ['$scope', 'hostDire', '$http', '$window',
    function ($scope, hostDire, $http, $window) {
        $scope.LogOut = function () {
            debugger

            $http({
                method: 'POST',
                url: hostDire + '/Account/LogOut',
            }).

                success(function (data, status, headers, config) {
                    debugger
                    if (data.status === 1) {
                        $window.location = hostDire + '/Account/Login';
                    }
                }).
                error(function (data, status, headers, config) {

                });
        };
    }])
app.config(function ($locationProvider) {
    $locationProvider.html5Mode(true);
});
