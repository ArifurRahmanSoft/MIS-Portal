/**
 *  
 */

app.service('customCodeService', function ($http, hostDire) {
    var urlGet = '';

    //**********----Get All Record----***************
    
    this.getAll = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging) {
        // debugger
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    //************ Get Details  By ID  *************************
    this.getDetailsByID = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging,DetailsID) {
        // debugger
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging + '/' + DetailsID;
        return $http.get(urlGet);
    }
    //**********----Get Single Record----***************


    this.getByID = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
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

    //************** Post Master Details  ************
    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails) {
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }
});