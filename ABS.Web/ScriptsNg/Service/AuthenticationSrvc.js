/**
 * UserSrvc.js
 */

app.service('userService', function ($http, hostDire) {

    //**********----Create New Record----***************
    this.post = function (apiRoute, model) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: model
        });
        return request;
    }

    this.postExternal = function (apiRoute, model) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: model
        });
        return request;
    }
});