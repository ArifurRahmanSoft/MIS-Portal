/**
 * ng_CrudService.js
 */

app.service('moduleService', function ($http, hostDire) {
    //**********----Get All Record----***************

    var urlGet = '';
    this.GetModules = function (apiRoute, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.GetModule = function (apiRoute, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute  + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.GetModulesNew = function (apiRoute, LCompanyID, LUserID, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + '/' + LCompanyID + '/' + LUserID + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    //**********----Get Single Record----***************
    this.getModuleByID = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
        return $http.get(apiRoute);
    }

    //**********----Create New Record----***************
    this.post = function (apiRoute, Customer) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: Customer
        });
        return request;
    }

    //**********----Update the Record----***************
    this.put = function (apiRoute, Customer) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "put",
            url: apiRoute,
            data: Customer
        });
        return request;
    }

    //**********----Delete the Record----***************
    this.delete = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "delete",
            url: apiRoute
        });
        return request;
    }
});