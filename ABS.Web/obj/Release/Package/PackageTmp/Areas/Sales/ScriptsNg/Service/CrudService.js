/**
 * CustomerSrvc.js
 */

app.service('crudService', function ($http, hostDire, conversion, $q) {

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

    this.GetList = function (apiRoute, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }


    this.getMVMasterList = function (apiRoute, objcmnParam) {
        apiRoute = hostDire + apiRoute;
        var cmnParam = "[" + JSON.stringify(objcmnParam) + "]";
        //debugger
        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParam,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getAll = function (apiRoute, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getItemWithcmnParam = function (apiRoute, objcmnParam, headerToken) {
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
    };

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
    };


    this.postModels = function (apiRoute, modelFirst, modelSecond) {
        apiRoute = hostDire + apiRoute;
        var param = "[" + JSON.stringify(modelFirst) + "," + JSON.stringify(modelSecond) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: param,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    };

    this.postModelRate = function (apiRoute, modelFirst, modelSecond, modelThird) {
        apiRoute = hostDire + apiRoute;
        var param = "[" + JSON.stringify(modelFirst) + "," + JSON.stringify(modelSecond) + "," + JSON.stringify(modelThird) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: param,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    };

    this.postMultiModels = function (apiRoute, modelFirst, modelSecond, modelThird, modelFourth, modelFifth) {
        apiRoute = hostDire + apiRoute;
        var param = "[" + JSON.stringify(modelFirst) + "," + JSON.stringify(modelSecond) + "," + JSON.stringify(modelThird) + "," + JSON.stringify(modelFourth) + "," + JSON.stringify(modelFifth) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: param,
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


    this.getItemMasterByGroup = function (apiRoute, objcmnParam, headerToken) {
        apiRoute = hostDire + apiRoute;
        var cmnParamWitGroupID = "[" + JSON.stringify(objcmnParam) + "]";

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

    this.getAPIRequestWithCmnParam = function (apiRoute, objcmnParam, headerToken) {
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

    this.getMultipleParameter = function (apiRoute, mode, trackNo, page, pageSize, isPaging, headerToken) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + mode + '/' + trackNo + '/' + page + '/' + pageSize + '/' + isPaging;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;
    };


    this.getMultipleParameter = function (apiRoute, mode, trackNo) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + mode + '/' + trackNo + '/';
        var request = $http({
            method: "get",
            url: urlGet
        });
        return request;
    };

    this.getMultipleParameter2 = function (apiRoute, mode, trackNo, sstypId) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + mode + '/' + trackNo + '/' + sstypId + '/';
        var request = $http({
            method: "get",
            url: urlGet
        });
        return request;
    };

    this.getIncenCal = function (apiRoute, mode, distributorId, trackNo, page, pageSize, isPaging, headerToken) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + mode + '/' + distributorId + '/' + trackNo + '/' + page + '/' + pageSize + '/' + isPaging;
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
    this.postList = function (apiRoute, modelDetails) {
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

    this.postMultipleModelWithoutDynamicConversion = function (apiRoute, FactoryDeliveryOrdersMaster, FDODetailsList, DetailsPackingList, DeleteListHDOInfoDetails, objcmnParam, headerToken) {
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