var onSuccessForm = function (result) {
    //alert(result);
    if (result.url) {
        // if the server returned a JSON object containing an url
        // property we redirect the browser to that url
        //window.location.href = result.url;
        //var path = '@Url.Content("~/Login/UserLogin")';

        window.location.href = result.url;
    }
    //console.log(result)

}
var onSuccess = function onSuccess(response) {

    if (response.url) {

        window.location.href = response.url;
    }
    else if (response.success == 1) {
        
        Command: toastr["success"](response.message);
        $('#form0 input,select,textarea').not('[readonly],[disabled],:button,:hidden,:checkbox').val("");

        $('select').select2("val", "");

    } else {
        Command: toastr["warning"](response.message);

    }

}
var onFailure = function onFailure(response) {
    if (response.url) {

        window.location.href = response.url;
    }
    Command: toastr["error"](response.message);
}