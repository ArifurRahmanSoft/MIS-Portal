/**
* UserCtrl.js
*/

app.controller('loginCtrl', ['$scope', 'userService', '$window', 'hostDire',
    function ($scope, userService, $window, hostDire) {
        $scope.IsVisible = true; $scope.IsLogged = false; $scope.success = false; $scope.error = false;
        //**********----Create New Record----***************

        $scope.submitLogin = function () {

            // checking browser
            //var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
            //if (!isChrome) {
            //    alert('Please use google chrome browser to login this software !!!')
            //    return;
            //}
            debugger

            if ($scope.UserLogin === '' || $scope.UserLogin === undefined || $scope.Password === '' || $scope.Password === undefined) {
                $scope.IsVisible = true;
                $scope.IsLogged = true;
                $scope.error = true;
                if (($scope.UserLogin === '' || $scope.UserLogin === undefined) && ($scope.Password === '' || $scope.Password === undefined)) {
                    $scope.lblrmessage = 'Please Enter Staff Id and Password....';
                    return;
                }

                if ($scope.UserLogin === '' || $scope.UserLogin === undefined) {
                    $scope.lblrmessage = 'Please Enter Staff Id....';
                    return;
                }

                if ($scope.Password === '' || $scope.Password === undefined) {
                    $scope.lblrmessage = 'Please Enter Password....';
                    return;
                }
            }

            else {

                var AuthenticateUser = {
                    EmpID: $scope.UserLogin,
                    UserPassw: $scope.Password
                };
                $scope.lblrmessage = 'Authenticating....';

                if (AuthenticateUser != null) {

                    //var urlLogin = 'https://cssap.citygroupbd.com/acmsapi/api/usersetup/verifyuser';
                    //var LoginUser = userService.postExternal(urlLogin, AuthenticateUser);
                    //LoginUser.then(function (response) {
                    //if (response.data.charAt(0) === '1') {
                    var urlLogin = '/Account/Login/';
                    var LoginUser = userService.post(urlLogin, AuthenticateUser);
                    LoginUser.then(function (response) {
                        debugger
                        if (response.data.status > 0) {
                            $scope.error = false;
                            $scope.IsVisible = false;
                            $scope.IsLogged = true;
                            $scope.success = true;
                            $scope.lblrmessage = 'Login Successful. Redirecting....';
                            if (response.data.TargetUrl === null) {
                                $window.location = hostDire + '/Home';
                            }
                            else {
                                $window.location = hostDire + response.data.TargetUrl
                            }
                        }
                        else {
                            Command: toastr["error"]("Sorry, You are not authorized for this application !!!");
                        }
                    })
                    //}


                    //else if ($scope.UserLogin == '00999' || $scope.UserLogin == '00888') /////// special user based on boss request
                    //{
                    //    var urlLogin = '/Account/Login/';
                    //    var LoginUser = userService.post(urlLogin, AuthenticateUser);
                    //    LoginUser.then(function (response) {
                    //        debugger
                    //        if (response.data.status > 0) {
                    //            $scope.error = false;
                    //            $scope.IsVisible = false;
                    //            $scope.IsLogged = true;
                    //            $scope.success = true;
                    //            $scope.lblrmessage = 'Login Successful. Redirecting....';
                    //            if (response.data.TargetUrl === null) {
                    //                $window.location = hostDire + '/Home';
                    //            }
                    //            else {
                    //                $window.location = hostDire + response.data.TargetUrl
                    //            }
                    //        }
                    //        else {
                    //            Command: toastr["error"]("Sorry, You are not authorized for this application !!!");
                    //        }
                    //    })
                    //}



                    //else {
                    //    Command: toastr["error"](response.data);
                    //}
                    //},
                    //    function (error) {
                    //        console.log("Error: " + error);
                    //    });
                }


                //if (AuthenticateUser != null) {
                //    debugger
                //    var urlLogin = '/Account/Login/';
                //    var LoginUser = userService.post(urlLogin, AuthenticateUser);
                //    LoginUser.then(function (response) {
                //        debugger;
                //        if (response.data.status === 1) {
                //            $scope.error = false;
                //            $scope.IsVisible = false;
                //            $scope.IsLogged = true;
                //            $scope.success = true;
                //            $scope.lblrmessage = 'Login Successful. Redirecting....';
                //            if (response.data.TargetUrl == null) {
                //                $window.location = '/Home';
                //            }
                //            else {
                //                $window.location = response.data.TargetUrl
                //            }
                //            //Command: toastr["info"]("Login Successful, Redirecting please wait....!");
                //        }
                //        else if (response.data.status === 0) {
                //            $scope.IsVisible = true;
                //            $scope.IsLogged = true;
                //            $scope.error = true;
                //            $scope.lblrmessage = 'The login is invalid! Please re-enter....';
                //            Command: toastr["warning"]("The login is invalid! Please re-enter....");
                //        }
                //        else {
                //            Command: toastr["error"]("Account Not Found!");
                //        }
                //    },
                //        function (error) {
                //            console.log("Error: " + error);
                //        });
                //}
            }
        };

        $scope.ClearMessage = function () {
            $scope.IsVisible = true;
            $scope.IsLogged = false;
            $scope.error = false;
            $scope.success = false;
            $scope.lblrmessage = '';
        }

        //**********----Create New Record----***************
        $scope.submitRecovery = function () {
            debugger;
            var RecoverUser = {
                RecoverEmail: $scope.RecEmail
            };

            if (RecoverUser != null) {
                var url = '/Account/Recover/';
                var Recovery = userService.post(url, RecoverUser);
                Recovery.then(function (response) {
                    if (response.data.status === 1) {

                        Command: toastr["success"]("Great. We have sent you an email!");
                    }
                    else if (response.data.status === 0) {

                        Command: toastr["error"]("Account Not Found!");
                    }
                    else {
                        Command: toastr["error"]("Account Not Found!");
                    }
                },
                    function (error) {
                        console.log("Error: " + error);
                    });
            }
        }
    }]);


