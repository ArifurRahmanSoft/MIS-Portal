/**
 * dashboardService.js
 */

app.service('dashboardService', function ($http, hostDire) {
    //**********----Get Record Total----***************
    this.getTotal = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
        return $http.get(apiRoute);
    }

    var urlGet = '';
    this.getModel = function (apiRoute, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

});