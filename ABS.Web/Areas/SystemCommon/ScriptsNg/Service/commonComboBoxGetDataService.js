/**
 *  
 */

app.service('commonComboBoxGetDataService', function ($http, hostDire) {
    var urlGet = '';
    //**********----Get All Company Records----***************
    this.GetAll = function (apiRoute, companyID, loggedUser, page, pageSize, isPaging) {
        apiRoute = hostDire + apiRoute;
        urlGet = apiRoute + companyID + '/' + loggedUser + '/' + page + '/' + pageSize + '/' + isPaging;
        return $http.get(urlGet);
    }
   
});