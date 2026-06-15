
app.service('sideNavService', function ($http, hostDire) {

    var urlGet = '';
    //*****************  method For Layout Side Menu ********************
    this.GetSideMenu = function (apiRoute, companyID, loggedUser, ModuleID) {
        // debugger
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + ModuleID;
        return $http.get(urlGet);
    }
    this.GetProcess = function (apiRoute, UserID, CompanyID, DepartmentID, moduleName, controllerName, controllerActionName) {
        // debugger
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + UserID + '/' + CompanyID + '/' + DepartmentID + '/' + moduleName + '/' + controllerName + '/' + controllerActionName;
        return $http.get(urlGet);
    }
    //Get----get single data from server
    this.getByID = function (apiRoute) {
        apiRoute = hostDire + apiRoute;
        return $http.get(apiRoute);
    }
     

});