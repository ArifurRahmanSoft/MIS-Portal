
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
    
    $scope.topNavigationNotificationList = [];
    $scope.sessionUserCompanyID = $('#hCompanyID').val();
    $scope.sessionLoggedUserID = $('#hUserID').val();

    $scope.targetNotifiedID = function (model, MasterID) {
        $localStorage.notificationStorageModel = model;
        $localStorage.notificationStorageMenuID = model.MenuID;
        $localStorage.currentMenuID = model.MenuID;
       
        $localStorage.notificationStorageMasterID = model.TransactionID;
        $localStorage.notificationStorageIsApproved = true;
        $localStorage.notificationStorageIsDeclained = true;
    };

}]);