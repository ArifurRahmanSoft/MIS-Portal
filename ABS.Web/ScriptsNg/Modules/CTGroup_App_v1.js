
/**
 * CTGroup_App_v1.js
 */

var app;
(function () {
    'use strict'; //Defines that JavaScript code should be executed in "strict mode"
    app = angular.module("CTGroup_App_Lay", ['ngStorage']);

    //for live version
    //app.value("hostDire", "/MISReport");

    //for development version
    app.value("hostDire", "");

})();