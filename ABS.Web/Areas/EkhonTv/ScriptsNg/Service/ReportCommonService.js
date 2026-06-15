/**
 * QuotationSrvc.js
 */

app.service('reportCommonService', function ($http, hostDire) {

    //**********----Get All Record----***************
    var urlGet = '';

    this.GetList = function (apiRoute, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
    this.getAllUsers = function (apiRoute, page, pageSize, isPaging, UserTypeID, LoginCompanyID) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging + '/' + UserTypeID + '/' + LoginCompanyID;
        return $http.get(urlGet);
    }
    //**********----Get Single Record----***************
    this.getItemByID = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
        return $http.get(apiRoute);
    }
 

    this.getByQuotationID = function (apiRoute, QuotationId, CompanyID, headerToken) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + QuotationId + '/' + CompanyID;
        var request = $http({
            method: "get",
            url: urlGet,
            headers: headerToken
        });
        return request;

    }

    this.GetFrList = function (apiRoute, cmnParam, headerToken) {
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

   


    this.getItemDetailsByRequisitionID = function (apiRoute, RequisitionID, CompanyId) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + RequisitionID + '/' + CompanyId;
        var request = $http({
            method: "get",
            url: urlGet
        });
        return request;
    }

    this.getModel = function (apiRoute, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }

    this.getTaggingMasterList = function (apiRoute, objcmnParam) {
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

    this.getDynamicGrid = function (apiRoute, objcmnParam) {
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

    this.GetloadSPRNo = function (apiRoute, objcmnParam) {
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
    this.getModelByID = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
        return $http.get(apiRoute);
    }

    this.getPostservice = function (apiRoute, objcmnParam, headerToken) {
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(objcmnParam) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    //this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, menuID,headerToken) {
    //    // //debugger
    //    var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(menuID) + "]";

    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
    //        dataType: "json",
    //        contentType: "application/json",
    //        headers: headerToken
    //    });
    //    //console.log(request);
    //    return request;
    //}

    this.postMasterDetail = function (apiRoute, modelMaster, modelDetails, LoadingAddressRows, menuID, headerToken) {
        debugger
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(LoadingAddressRows) + "," + JSON.stringify(menuID) + "]";
        //console.log(strFinal);
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

    this.postMasterDetail = function (apiRoute, modelMaster
        , modelDetails
        , ContactPerson
        , WarrantyRows
        , TermsRows
        , TermsDetailsRows
        , LoadingAddressRows
        , DischargeAddressRows
        , PaymentRows
        , DocumentRows
        , SummaryRows
        , menuID
        , headerToken) {
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) +
            "," + JSON.stringify(ContactPerson) +
            "," + JSON.stringify(WarrantyRows) +
            "," + JSON.stringify(TermsRows) +
            "," + JSON.stringify(TermsDetailsRows) +
            "," + JSON.stringify(LoadingAddressRows) +
            "," + JSON.stringify(DischargeAddressRows) +
            "," + JSON.stringify(PaymentRows) +
            "," + JSON.stringify(DocumentRows) +
            "," + JSON.stringify(SummaryRows) +
            "," + JSON.stringify(menuID) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            dataType: "json",
            contentType: "application/json",
            headers: headerToken
        });
        return request;
    }

    //this.saveLC = function (apiRoute, modelMaster, modelDetails, fileInfo) {
    //    // //debugger
    //    var strFinal = "[" + JSON.stringify(modelMaster) + "," + JSON.stringify(modelDetails) + "," + JSON.stringify(fileInfo) + "]";

    //    var request = $http({
    //        method: "post",
    //        url: apiRoute,
    //        data: strFinal, //{ modelMaster: modelMaster, modelDetails: modelDetails }
    //        dataType: "json",
    //        contentType: "application/json"
    //    });
    //    //console.log(request);
    //    return request;
    //}



    this.getDeptByCompanyID = function (apiRoute, objcmnParam, companyID) {
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(companyID) + "]";
        var request = $http({
            method: "post",
            url: apiRoute,
            data: strFinal,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getModelByCompanyID = function (apiRoute, id, companyID) {
        apiRoute = hostDire + apiRoute;
        var strFinal = "[" + JSON.stringify(id) + "," + JSON.stringify(companyID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            //data: JSON.stringify({ id: id, companyID: companyID }),
            data: strFinal,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }

    this.getBranchByBankID = function (apiRoute, bankID) {
        apiRoute = hostDire + apiRoute;
        var request = $http({
            method: "post",
            url: apiRoute,
            data: bankID,
            dataType: "json",
            contentType: "application/json"
        });
        return request;
    }


    this.getItemMasterByGroup = function (apiRoute, objcmnParam, groupID) {
        apiRoute = hostDire + apiRoute;
        var cmnParamWitGroupID = "[" + JSON.stringify(objcmnParam) + "," + JSON.stringify(groupID) + "]";

        var request = $http({
            method: "post",
            url: apiRoute,
            data: cmnParamWitGroupID,
            dataType: "json",
            contentType: "application/json"
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


    this.GetLocation = function (apiRoute, objcmnParam) {
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
    this.getPIDetailsByActivePIID = function (apiRoute, activePI) {
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

    this.getUserWiseCompany = function (apiRoute, objcmnParam) {
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

    this.getLCMaster = function (apiRoute, objcmnParam) {
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
        // //debugger
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
    this.postList = function (apiRoute, modelDetails) {
        // //debugger
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
});