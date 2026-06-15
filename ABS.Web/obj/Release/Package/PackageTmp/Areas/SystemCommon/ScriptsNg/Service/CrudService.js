/**
 * ng_CrudService.js
 */

app.service('crudService', function ($http, hostDire, conversion) {
    //**********----Get All Record----***************
    var urlGet = '';

    this.getAllList = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
        return $http.get(apiRoute);
    }

    this.getListByParam = function (apiRoute, param) {
        apiRoute = hostDire + apiRoute;
        return $http.get(apiRoute, { params: param });
    }

    this.getModel = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute;
        var request = $http({
            method: "get",
            url: urlGet
        });
        return request;
    }
    
    this.getMultipleParameter = function (apiRoute, mode, trackNo) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + mode + '/' + trackNo + '/';
        var request = $http({
            method: "get",
            url: urlGet
        });
        return request;
    }

    this.getAllIncludingCompanyLog = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.getAllIncludingCompanyuser = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.getAllWithCompnayID = function (apiRoute, page, pageSize, isPaging, ComapnyID) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + ComapnyID;
        return $http.get(urlGet);
    }
  this.getUser = function (apiRoute, companyID, loggedUser, userTypeID, page, pageSize, isPaging) {
      apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + '/' + userTypeID + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.getAll = function (apiRoute, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getAllUsers = function (apiRoute, objcmnParam) {
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getAllUsersType = function (apiRoute, objcmnParam) {
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getAllUsersGroup = function (apiRoute, objcmnParam) {
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getAllByParam = function (apiRoute, loggedUser, page, pageSize, isPaging, pModuleID, pUserID) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging + '/' + pModuleID + '/' + pUserID;
        return $http.get(urlGet);
    }
    //**********----Get Single Record----***************
    this.getByID = function (apiRoute) {
        return $http.get(apiRoute);
    }


    //**********----Create New Record----***************
    this.post = function (apiRoute, Model) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: Model
        });
        return request;
    }
    this.postList = function (apiRoute, User, PmCompany) {
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(User) + "," + JSON.stringify(PmCompany) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            contentType: "application/json"
        });
        //console.log(request);
        return request;
    }



    this.GetList = function (apiRoute, cmnParam) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }
    this.GetList = function (apiRoute, cmnParam, headerToken) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    //**********----Update the Record----***************
    this.put = function (apiRoute, Model) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "put",
            url: apiRoute,
            data: Model
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

    this.getMultipleModels = function (apiRoute, model, headerToken) {
        apiRoute = hostDire + apiRoute;
        var strFinal = conversion.JsonStringify(model);
        var RequestType = strFinal.Token.post.AuthorizedToken == headerToken.AuthorizedToken || strFinal.Token.put.AuthorizedToken == headerToken.AuthorizedToken ? $.ajax : $http;
        var request = RequestType({
            method: "get",
            url: apiRoute,
            data: strFinal.model,
            async: false,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });

        //$('#WaitModal').modal('hide');

        return request;
    }

    this.postMultipleModels = function (apiRoute, model, headerToken) {
        apiRoute = hostDire + apiRoute;
        var strFinal = conversion.JsonStringify(model);
        var RequestType = strFinal.Token.post.AuthorizedToken == headerToken.AuthorizedToken || strFinal.Token.put.AuthorizedToken == headerToken.AuthorizedToken ? $.ajax : $http;
        var request = RequestType({
            method: "post",
            url: apiRoute,
            data: strFinal.model,
            async: false,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });

        //$('#WaitModal').modal('hide');

        return request;
    }

    this.postMultipleModel = function (apiRoute, model, headerToken) {
        apiRoute = hostDire + apiRoute;
        var strFinal = conversion.getDynamicModels(model);
        var RequestType = strFinal.Token.post.AuthorizedToken == headerToken.AuthorizedToken || strFinal.Token.put.AuthorizedToken == headerToken.AuthorizedToken ? $.ajax : $http;
        var request = RequestType({
            method: "post",
            url: apiRoute,
            data: strFinal.model,
            async: false,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.checkedUserLoginID = function (apiRoute, Model) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: Model
        });
        return request;
    }
});