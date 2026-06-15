app.service('crdMyProfileSerive', function ($http, hostDire) {
    var urlGet = '';
    this.getCurrentUserPassword = function (apiRoute, companyID, loggedUser) {
        // debugger
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + companyID + '/' + loggedUser;
        return $http.get(urlGet);
    }

    this.post = function (apiRoute, Model) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: Model
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