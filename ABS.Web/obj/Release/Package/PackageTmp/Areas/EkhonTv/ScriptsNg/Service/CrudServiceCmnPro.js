/**
 * CustomerSrvc.js
 */

app.service('crudService', function ($http, hostDire, conversion, $q) {

    //**********----Get All Record----***************
    var urlGet = '';

    this.GetList = function (apiRoute, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getAll = function (apiRoute, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getAll = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute;
        return $http.get(urlGet);
    }

    //**********----Get Single Record----***************
    this.getByID = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
        return $http.get(apiRoute);
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


        //var request = $.ajax({
        //    method: "post",
        //    url: apiRoute,
        //    data: cmnParam,
        //    async: false,
        //    dataType: "json",
        //    contentType: "application/json",
        //    headers: headerToken
        //});
        //return request;

    }





    this.GetLists = function (apiRoute, cmnParam, headerToken) {
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

    this.PostLists = function (apiRoute, cmnParam, headerToken) {
        apiRoute = hostDire + apiRoute;
        var request = $.ajax({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    this.getModelDc = function (apiRoute, objcmnParam, headerToken) {
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        debugger
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

    //**********----Get Single Record----***************
    this.getItemByID = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
        return $http.get(apiRoute);
    }

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

    this.getDynamicGrid = function (apiRoute, objcmnParam, headerToken) {
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

    this.saveLC = function (apiRoute, modelMaster, modelDetails, fileInfo, DocList, objcmnParam, headerToken) {
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(fileInfo) + "," + JSON.stringify(DocList) + "," + JSON.stringify(objcmnParam) + "]";

        var request = $.ajax({
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

    this.postModels = function (apiRoute, modelFirst, modelSecond) {
        apiRoute = hostDire + apiRoute;
        var param = "[" + JSON.stringify(modelFirst) + "," + JSON.stringify(modelSecond) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: param, //{ modelFirst: modelFirst, modelSecond: modelSecond }
            dataType: "json",
            contentType: "application/json"
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

    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, menuID) {
        // debugger
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(menuID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json"
        });
        //console.log(request);
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
    this.postMultipleMasterDetail = function (apiRoute, model1, model2, model3, model4, model5) {
        debugger
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(model1) + "," + JSON.stringify(model2) + "," + JSON.stringify(model3) + "," + JSON.stringify(model4) + "," + JSON.stringify(model5) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json"
        });
        //console.log(request);
        return request;
    }

    this.postMultipleMasterDetailExt = function (apiRoute, model1, model2, model3, model4, model5, model6) {
        debugger
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(model1) + "," + JSON.stringify(model2) + "," + JSON.stringify(model3) + ","
            + JSON.stringify(model4) + "," + JSON.stringify(model5) + "," + JSON.stringify(model6) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json"
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
    //this.GetChecmicalByID = function (apiRoute, id) {
    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: id,
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    return request;
    //}


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

    this.getPIMasterByActivePIID = function (apiRoute, activePI) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: activePI,
            dataType: "json",
            contentType: "application/json"
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
        debugger
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

    this.getPIMasterListByPIActive = function (apiRoute, objcmnParam, headerToken) {
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        debugger
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

    this.postDefectInfo = function (apiRoute, modelMaster) {
        // debugger
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(modelMaster) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json"
        });
        //console.log(request);
        return request;
    }

    this.getModelDefectType = function (apiRoute, objcmnParam) {
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        debugger
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }
    this.postList = function (apiRoute, modelDetails) {
        // debugger
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(modelDetails) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            dataType: "json",
            contentType: "application/json"
        });
        //console.log(request);
        return request;
    }

    this.getModelOutputUnit = function (apiRoute, objcmnParam) {
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        debugger
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.postMultipleModel = function (apiRoute, model, headerToken) {
        debugger
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

        //$('#WaitModal').modal('hide');

        return request;
    }
    this.postMultipleModelWithoutDynamicConversion = function (apiRoute, FactoryDeliveryOrdersMaster, FDODetailsList,DetailsPackingList,DeleteListHDOInfoDetails,objcmnParam, headerToken) {
        debugger
        apiRoute = hostDire + apiRoute;
       // var strFinal = conversion.getDynamicModels(model);
        var strFinal = "[" + JSON.stringify(FactoryDeliveryOrdersMaster) + "," + JSON.stringify(FDODetailsList) + "," + JSON.stringify(DetailsPackingList) + "," + JSON.stringify(DeleteListHDOInfoDetails) + "," + JSON.stringify(objcmnParam) + "]";
        var request = $.ajax({
            method: "post",
            url: apiRoute,
            data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
            async: false,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
        //var RequestType = strFinal.Token.post.AuthorizedToken == headerToken.AuthorizedToken || strFinal.Token.put.AuthorizedToken == headerToken.AuthorizedToken ? $.ajax : $http;
        //var request = RequestType({
        //    method: "post",
        //    url: apiRoute,
        //    data: strFinal,
        //    async: false,
        //    dataType: "json",
        //    contentType: "application/json",
        //    headers: headerToken
        //});

        ////$('#WaitModal').modal('hide');

        //return request;
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


    this.getDailyFabricQualityTests = function (apiRoute, objcmnParam) {
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
    this.getMultipleModel = function (apiRoute, model, headerToken) {
        debugger
        apiRoute = hostDire + apiRoute;
       // var cmnParam = "[" + JSON.stringify(model) + "]";
        var strFinal = conversion.getDynamicModels(model);
        var request = $.ajax({
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

    this.getModelAjax = function (apiRoute, model, headerToken) {
        debugger
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(model) + "]";
        var request = $.ajax({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            async: false,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });

        //$('#WaitModal').modal('hide');

        return request;
    }
});