app.filter('timeAgo', ['$interval', function ($interval) {
    // trigger digest every 60 seconds
    $interval(function (){}, 60000);

    function fromNowFilter(time){
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
    
    $scope.targetNotifiedID = function (model, MasterID) {
       
        $localStorage.notificationStorageModel = model;
        $localStorage.notificationStorageMenuID = model.MenuID;
        
        $localStorage.currentMenuID = model.MenuID;
        $localStorage.notificationStorageMasterID = model.TransactionID;
        $localStorage.notificationStorageIsApproved = true;
        $localStorage.notificationStorageIsDeclained = true; 
       
        var apiRoute = '/SystemCommon/Dashboard/' + 'ModifyCompanySession/' + model.NotificationCompanyID;
        var porcessMasterDetails = sideNavService.getByID(apiRoute);
        porcessMasterDetails.then(function (response) {
            debugger
            var ab = response;
            
        },
       function (error) {
           console.log("Error: " + error);
       });
    };
    
}]);