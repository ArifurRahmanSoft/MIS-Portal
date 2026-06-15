
app.filter('timeAgo', ['$interval', function ($interval) {
    // trigger digest every 60 seconds
    $interval(function () { }, 60000);

    function fromNowFilter(time) {
        return moment(time).fromNow();
    }

    fromNowFilter.$stateful = true;
    return fromNowFilter;
}]);
app.controller('TopNavigationNotificationCtrl', ['$scope', '$localStorage', 'sideNavService', function ($scope, $localStorage, sideNavService) {
    $scope.processNotificationList = [];
    $scope.processNotificationList = $localStorage.ProcessListfromLocalStrorage;
    $scope.topNavigationNotificationList = [];
    $scope.sessionUserCompanyID = $('#hCompanyID').val();
    $scope.sessionLoggedUserID = $('#hUserID').val();
}]);