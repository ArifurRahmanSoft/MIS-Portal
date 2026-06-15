
/**
 * CTGroup_App_v1.js
 */
var app;
(function () {
    'use strict';
    //app = angular.module("CTGroup_App", ["ngStorage"]);
    //app = angular.module('CTGroup_App_Sc', ['ngStorage', 'ngAnimate', 'ngTouch', 'ui.grid', 'ui.grid.pagination', 'ui.grid.grouping', 'ui.grid.resizeColumns', 'ui.grid.moveColumns', 'ui.grid.pinning', 'ui.grid.selection', 'ui.grid.autoResize', 'ui.grid.exporter']);
    app = angular.module('CTGroup_App_Sc', ['ngStorage', 'ngAnimate', 'ngTouch', 'ui.grid', 'ui.grid.pagination', 'ui.grid.grouping', 'ui.grid.resizeColumns', 'ui.grid.moveColumns', 'ui.grid.pinning', 'ui.grid.selection', 'ui.grid.autoResize', 'ui.grid.exporter', 'angularTreeview']);

    //for live version
    //app.value("hostDire", "/MISReport");

    //for development version
    app.value("hostDire", "");

})();