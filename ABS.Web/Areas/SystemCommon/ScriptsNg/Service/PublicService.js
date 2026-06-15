app.service('PublicService', function ($http, hostDire) {
    //**********----Get All Record----***************
    var urlGet = '';
    this.getItemMasterService = function (apiRoute, objcmnParam) {
        apiRoute = hostDire + apiRoute;
        var cmnParamWitGroupID = "[" + JSON.stringify(objcmnParam) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWitGroupID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

});