/**
 * CustomerSrvc.js
 */

app.service('crudService', function ($http, hostDire, conversion) {

    //**********----Get All Record----***************
    var urlGet = '';
    this.getModel = function (apiRoute, page, pageSize, isPaging, headerToken) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }
    this.getModelSampleNo = function (apiRoute, companyID, page, pageSize, isPaging, headerToken) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + companyID + '/' + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }
    this.getModelByBuyer = function (apiRoute, buyerID, companyID, page, pageSize, isPaging, headerToken) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + buyerID + '/' + companyID + '/' + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    }

    this.getModelHDO = function (apiRoute, objcmnParam, headerToken) {
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        
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

    this.postModels = function (apiRoute, modelFirst, modelSecond, headerToken) {
        apiRoute = hostDire + apiRoute;
        var param = "[" + JSON.stringify(modelFirst) + "," + JSON.stringify(modelSecond) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: param, //{ modelFirst: modelFirst, modelSecond: modelSecond }
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    //**********----Get Single Record----***************
    this.getModelByID = function (apiRoute, headerToken) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "get",
            url: apiRoute,
            headers: headerToken
        });
        return request;
    }

    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, menuID, headerToken) {
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(menuID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        //console.log(request);
        return request;
    }

    this.saveLC = function (apiRoute, modelMaster, modelDetails, fileInfo, objcmnParam, headerToken) {
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(fileInfo) + "," + JSON.stringify(objcmnParam) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        //console.log(request);
        return request;
    }

    this.getBankAdvisingByCompanyID = function (apiRoute, companyID, headerToken) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: companyID,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getModelByCompanyID = function (apiRoute, id, companyID, headerToken) {
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(id) + "," + JSON.stringify(companyID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            //data: JSON.stringify({ id: id, companyID: companyID }),
            data: strFinal,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getBranchByBankID = function (apiRoute, bankID, headerToken) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: bankID,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }


    this.getItemMasterByGroup = function (apiRoute, objcmnParam, groupID, headerToken) {
        apiRoute = hostDire + apiRoute;
        var cmnParamWitGroupID = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(groupID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWitGroupID,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getPIMasterByActivePIID = function (apiRoute, activePI, headerToken) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: activePI,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getPIDetailsByActivePIID = function (apiRoute, activePI, headerToken) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: activePI,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }
    this.getUserWiseCompany = function (apiRoute, LoginUserID, headerToken) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: LoginUserID,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }
    this.getLCMaster = function (apiRoute, objcmnParam, headerToken) {
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        
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

    this.getPIMasterListByPIActive = function (apiRoute, objcmnParam, headerToken) {
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        
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


    this.post = function (apiRoute, model, headerToken) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: model,
            headers: headerToken
        });
        return request;
    }

    this.postNotification = function (apiRoute, Customer) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: Customer
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

    this.postMultipleModel = function (apiRoute, model, headerToken) {
        apiRoute = hostDire + apiRoute;
        var strFinal = conversion.getDynamicModels(model);

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        //console.log(request);
        return request;
    }
});