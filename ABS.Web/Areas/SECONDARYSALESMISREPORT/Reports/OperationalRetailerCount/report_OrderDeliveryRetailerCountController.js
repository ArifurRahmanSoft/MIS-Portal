
angular.module('cgssApp.controllers')
   
    .controller('report_OrderDeliveryRetailerCountController', ['$scope', '$window', '$timeout', '$location', 'ngTableParams', 'exDialog', '$route', 'Auth', 'getDistributorByDistributorCode', 'getRouteByDistAndSO',  'getSSalesAreaHierarchyList', 'getProductInfoList',
        function ($scope, $window, $timeout, $location, ngTableParams, exDialog, $route, Auth, getDistributorByDistributorCode, getRouteByDistAndSO,  getSSalesAreaHierarchyList, getProductInfoList) {


            // AuthenticationService.validateRequest();
            $scope.model = {};
            $scope.model.orderList = {};
            $scope.search = {};
            $scope.model.SelDistributor = {};
            $scope.model.SelSO = {};
            $scope.model.SelRoute = {};
            $scope.model.SelRetailer = {};
            $scope.model.SelProductBrand = {};
            $scope.model.SelProduct = {};


            //Datepicker.
            $scope.openFrom = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();
                $scope.openedFrom = true;
                $scope.openedTo = false;
            };
            $scope.openTo = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();
                $scope.openedTo = true;
                $scope.openedFrom = false;
            };


            $scope.dateOptions = {
                formatYear: 'yyyy',
                startingDay: 1,
                showWeeks: 'false'
            };
            $scope.format = 'dd-MMM-yyyy';

            //Set default object for checkboxes in table including items array.
            $scope.checkboxes = {
                'topChecked': false,
                items: []
            };


            $scope.init = function () {
                if (Auth.userHasPermission(["report_OrderDeliveryRetailerCount"])) {
                    // some evil logic here
                    var userName = Auth.currentUser().name;
                    $scope.user_role = Auth.currentUser().USER_ROLE;
                    if ($scope.user_role == 'ADMIN') {
                        //$scope.model.SelDistributor.DIST_ID = '0';
                    }
                    else {
                        $scope.model.SelDistributor.DIST_ID = Auth.currentUser().LOGIN_ID;
                    }

                }

            };

            $scope.init();

            $scope.model.DeliveryFromDate = new Date();
            $scope.model.DeliveryToDate = new Date();

            $scope.model.AreaList = [{ Id: 1, AreaName: 'By National' }, { Id: 2, AreaName: 'By Division' }, { Id: 3, AreaName: 'By Region' }, { Id: 4, AreaName: 'By Zone' }, { Id: 5, AreaName: 'By Distributor' }, { Id: 6, AreaName: 'By SO' }, { Id: 7, AreaName: 'By Route' }, { Id: 8, AreaName: 'By Retailer' }];


            //$scope.GetDistributorByCode = function (dist_code) {
            //    getDistributorByDistributorCode.query({ modeNo: '9', trackingNo: dist_code }, function (DistData) {
            //        getRouteListByDistributorFromOrder.query({ mode: '1', DistId: dist_code}, function (RouteData) {

            //           // $scope.model.SelDistributor = DistData;
            //            $scope.model.RouteList = RouteData;
            //            if (RouteData.length>0) {
            //                $scope.showRouteList = true;
            //            }
                       


            //    });
            //    });

            //}

            $scope.ChangeRoute = function () {
                $scope.model.SelRetailer = {};
                $scope.ArrangeType = '';
                //getRetailerListByDistributorAndRouteFromOrder.query({ mode: '2', DistId: $scope.model.SelDistributor.DIST_NO, RouteId: $scope.model.SelRoute.ROUTE_ID }, function (RetailerData) {

                //    $scope.model.RetailerList = RetailerData;
                //});

            };

            $scope.model.SelDivision = {};
            $scope.model.SelRegion = {};
            $scope.model.SelZone = {};
           // $scope.model.SelOffer = {};
           // $scope.model.SelDistributor = {};

            $scope.model.DivisionList = [];
            $scope.model.RegionList = [];
            $scope.model.ZoneList = [];
            $scope.model.BrandList = [];
            $scope.model.ProductList = [];
           // $scope.model.DistributorList = [];

            var LoadList = function () {

                $scope.ArrangeType = '';

                getSSalesAreaHierarchyList.query({ mode: '2', trackNo: '0' }, function (DivisionData) {
                    getProductInfoList.query({ modeNo: '1', trackingNo: '0' }, function (brandData) {

                        $scope.model.BrandList = brandData;
                        $scope.model.DivisionList = DivisionData;

                    });

                   

                });

            };

            LoadList();

            $scope.BrandChange = function (brandId) {

                getProductInfoList.query({ modeNo: '2', trackingNo: brandId }, function (productData) {
                    $scope.model.ProductList = [];

                    $scope.model.ProductList = productData;

                });


            };

            $scope.ChangeDivision = function () {
                $scope.model.SelRegion = {};
                $scope.model.SelZone = {};
                $scope.model.SelDistributor = {};
                $scope.model.SelSO = {};
                $scope.model.SelRoute = {};
                $scope.model.SelRetailer = {};
                $scope.ArrangeType = '';

                $scope.model.RegionList = [];

                getSSalesAreaHierarchyList.query({ mode: '3', trackNo: $scope.model.SelDivision.DIVISION_ID }, function (Data) {
                    $scope.model.RegionList = Data;

                });

            }

            $scope.ChangeRegion = function () {
                $scope.model.SelZone = {};
                $scope.model.SelDistributor = {};
                $scope.model.SelSO = {};
                $scope.model.SelRoute = {};
                $scope.model.SelRetailer = {};
                $scope.ArrangeType = '';

                $scope.model.ZoneList = [];

                getSSalesAreaHierarchyList.query({ mode: '4', trackNo: $scope.model.SelRegion.REGION_ID }, function (Data) {
                    $scope.model.ZoneList = Data;
                });

            }

            $scope.ChangeZone = function () {
                $scope.model.SelDistributor = {};
                $scope.model.SelSO = {};
                $scope.model.SelRoute = {};
                $scope.model.SelRetailer = {};
                $scope.ArrangeType = '';
                $scope.model.DistributorList = [];
                getSSalesAreaHierarchyList.query({ mode: '5', trackNo: $scope.model.SelZone.ZONE_ID }, function (Data) {
                    $scope.model.DistributorList = Data;
                });

            }

            $scope.ChangeDistributor = function (dist_id) {
               
                //$scope.model.SelRetailer = {};
                //$scope.ArrangeType = '';
                //$scope.DIST_CODE = '';
                //$scope.DIST_CODE = $scope.model.SelDistributor.DIST_NO;
                //$scope.GetDistributorByCode($scope.DIST_CODE);

                $scope.model.SelSO = {};
                $scope.model.SelRoute = {};
                $scope.model.SelRetailer = {};
                $scope.ArrangeType = '';
                $scope.model.SOList = [];
                getSSalesAreaHierarchyList.query({ mode: '10', trackNo: dist_id }, function (Data) {
                    $scope.model.SOList = Data;
                });

            }

            $scope.ChangeSO = function (so_Id) {


                getRouteByDistAndSO.query({ mode: '1', distId: $scope.model.SelDistributor.DIST_ID, soId: so_Id }, function (Data) {
                    $scope.model.RouteList = Data;
                });
            };

            //$scope.ChangeRetailer = function () {
            //    $scope.ArrangeType = '';

            //}
            

           //// getRouteList();

           // $scope.ChangeRoute = function (RouteId) {

           //     $scope.RetailerList = [];

           //     getSSalesAreaHierarchyList.query({ mode: '8', trackNo: RouteId }, function (DataRetailer) {

           //         $scope.RetailerList = DataRetailer;
           //     });
           // };

            $scope.ShowReport = function (fromDate, toDate) {


                var brand_Id = '';


                var division_Id = '';
                var region_Id = '';
                var zone_Id = '';
                var dist_Id = '';
                var route_Id = '';
                var retailer_Id = '';
                var product_Id = '';
                var so_Id = '';
                

                if ($scope.model.SelDivision.DIVISION_ID == '' || $scope.model.SelDivision.DIVISION_ID == undefined || $scope.model.SelDivision.DIVISION_ID  == null) {
                    division_Id = '0';
                }
                else {
                    division_Id = $scope.model.SelDivision.DIVISION_ID;

                    //if ($scope.AreaType >= 2) {
                    //    division_Id = $scope.model.SelDivision.DIVISION_ID;
                    //}
                    //else {
                    //    division_Id = '0';
                    //}
                    
                }

                if ($scope.model.SelRegion.REGION_ID == '' || $scope.model.SelRegion.REGION_ID == undefined || $scope.model.SelRegion.REGION_ID == null) {
                    region_Id = '0';
                }
                else {
                    region_Id = $scope.model.SelRegion.REGION_ID;
                    //if ($scope.AreaType >= 3) {
                    //    region_Id = $scope.model.SelRegion.REGION_ID;
                    //}
                    //else {
                    //    region_Id = '0';
                    //}
                }

                if ($scope.model.SelZone.ZONE_ID == '' || $scope.model.SelZone.ZONE_ID == undefined || $scope.model.SelZone.ZONE_ID == null) {
                    zone_Id = '0';
                }
                else {
                    zone_Id = $scope.model.SelZone.ZONE_ID;
                    //if ($scope.AreaType >= 4) {
                    //    zone_Id = $scope.model.SelZone.ZONE_ID;
                    //}
                    //else {
                    //    zone_Id = '0';
                    //}
                }

                if ($scope.model.SelDistributor.DIST_ID == '' || $scope.model.SelDistributor.DIST_ID == undefined || $scope.model.SelDistributor.DIST_ID == null) {
                    dist_Id = '0';
                }
                else {
                   
                    if ($scope.AreaType >= 5) {
                        dist_Id = $scope.model.SelDistributor.DIST_ID;
                    }
                    else {
                        dist_Id = '0';
                    }
                }

                if ($scope.model.SelSO.SO_ID == '' || $scope.model.SelSO.SO_ID == undefined || $scope.model.SelSO.SO_ID== null) {
                    so_Id = '0';
                }
                else {

                    if ($scope.AreaType >= 6) {
                        so_Id = $scope.model.SelSO.SO_ID;
                    }
                    else {
                        so_Id = '0';
                    }
                }


                if ($scope.model.SelRoute.ROUTE_ID == '' || $scope.model.SelRoute.ROUTE_ID == undefined || $scope.model.SelRoute.ROUTE_ID == null) {
                    route_Id = '0';
                }
                else {
                   
                    if ($scope.AreaType >= 7) {
                        route_Id = $scope.model.SelRoute.ROUTE_ID;
                    }
                    else {
                        route_Id = '0';
                    }
                }


                if ($scope.model.SelRetailer.RETAILER_ID == '' || $scope.model.SelRetailer.RETAILER_ID == undefined || $scope.model.SelRetailer.RETAILER_ID == null) {
                    retailer_Id = '0';
                }
                else {
                   
                    if ($scope.AreaType >=8) {
                        retailer_Id = $scope.model.SelRetailer.RETAILER_ID;
                    }
                    else {
                        retailer_Id = '0';
                    }
                }

                if ($scope.model.SelProduct == null) {
                    product_Id = '0';
                }
                else {
                    if ($scope.model.SelProduct.PRODUCT_ID == '' || $scope.model.SelProduct.PRODUCT_ID == undefined || $scope.model.SelProduct.PRODUCT_ID == null) {
                        product_Id = '0';
                    }
                    else {
                        product_Id = $scope.model.SelProduct.PRODUCT_ID;
                    }
                }

                if ($scope.model.SelProductBrand == null) {
                    brand_Id = '0';
                }
                else {
                    if ($scope.model.SelProductBrand.BRAND_ID == '' || $scope.model.SelProductBrand.BRAND_ID == undefined || $scope.model.SelProductBrand.BRAND_ID == null) {
                        brand_Id = '0';
                    }
                    else {
                        brand_Id = $scope.model.SelProductBrand.BRAND_ID ;
                    }
                }
               

               

                var f = new Date(fromDate);
                var t = new Date(toDate);

                var f_Date = f.getDate() + "/" + (f.getMonth() + 1) + "/" + f.getFullYear();
                var t_Date = t.getDate() + "/" + (t.getMonth() + 1) + "/" + t.getFullYear();

                //if (type=='2') {
                // $window.open('/'+VirtualDirectory+'/ViewReport/Report1.aspx?Id=9&fromDate=' + f_Date + '&toDate=' + t_Date);
                if ($scope.ReportMode == '1') {

                

                $window.open('/' + VirtualDirectory + '/Areas/Reports/OrderDeliveryRetailerCount/ReportOrderDeliveryRetailerCount.aspx?Id=1&fromDate=' + f_Date + '&toDate=' + t_Date +
                    '&brand_Id=' + brand_Id +
                    '&product_Id=' + product_Id +
                    '&division_Id=' + division_Id +
                    '&region_Id=' + region_Id +
                    '&zone_Id=' + zone_Id +
                    '&dist_Id=' + dist_Id +
                    '&so_Id=' + so_Id +
                    '&route_Id=' + route_Id +
                    '&retailer_Id=' + retailer_Id +
                    '&arrangeType=' + $scope.ArrangeType +
                    '&reportMode=' + $scope.ReportMode 
                    );

                }

                if ($scope.ReportMode == '2') {

                    $window.open('/' + VirtualDirectory + '/Areas/Reports/OrderDeliveryByBrand/ReportOrderDeliveryByBrand.aspx?Id=1&fromDate=' + f_Date + '&toDate=' + t_Date +
                        '&brand_Id=' + brand_Id +
                        '&product_Id=' + product_Id +
                        '&division_Id=' + division_Id +
                        '&region_Id=' + region_Id +
                        '&zone_Id=' + zone_Id +
                        '&dist_Id=' + dist_Id +
                        '&so_Id=' + so_Id +
                        '&route_Id=' + route_Id +
                        '&retailer_Id=' + retailer_Id +
                        '&arrangeType=' + $scope.ArrangeType +
                        '&reportMode=' + $scope.ReportMode
                    );
                }


               // }

                //if (type == '2') {
                //    // $window.open('/'+VirtualDirectory+'/ViewReport/Report1.aspx?Id=9&fromDate=' + f_Date + '&toDate=' + t_Date);
                //    $window.open('/' + VirtualDirectory + '/ViewReport/Sales_Delivery_Report/SalesDeliveryReport.aspx?Id=8&fromDate=' + f_Date + '&toDate=' + t_Date + '&dist_Id=' + $scope.model.SelDistributor.DIST_ID);
                //}

                //if (type == '3') {
                //    $window.open('/' + VirtualDirectory + '/ViewReport/Report1.aspx?Id=11&fromDate=' + f_Date + '&toDate=' + t_Date + '&dist_Id=' + $scope.model.SelDistributor.DIST_ID);
                //}


            };
            

            $scope.ReportTypeChange = function (reportType) {

                $scope.model.SelDistributor.DIST_ID = '';
            }

            $scope.AreaTypeChange = function (Type) {
                //$scope.model.SelDivision = {};
                //$scope.model.SelRegion = {};
                //$scope.model.SelZone = {};
                //$scope.model.SelDistributor = {};
                //$scope.model.SelSO = {};
                //$scope.model.SelRoute = {};
                //$scope.model.SelRetailer = {};
               
            }

            $scope.ShowArrangeType = function () {
                if ($scope.AreaType == 1) {
                    return true;
                }
                if ($scope.AreaType == 2 && $scope.model.SelDivision.DIVISION_ID) {
                    return true;
                }
                if ($scope.AreaType == 3 && $scope.model.SelRegion.REGION_ID) {
                    return true;
                }
                if ($scope.AreaType == 4 && $scope.model.SelZone.ZONE_ID) {
                    return true;
                }
                if ($scope.AreaType == 5 && $scope.model.SelDistributor.DIST_ID) {
                    return true;
                }
                if ($scope.AreaType == 6 && $scope.model.SelSO.SO_ID) {
                    return true;
                }
                if ($scope.AreaType == 7 && $scope.model.SelRoute.ROUTE_ID ) {
                    return true;
                }
                if ($scope.AreaType == 8 && $scope.model.SelRetailer.RETAILER_ID) {
                    return true;
                }


            };


        }])

    //.directive('expand', function () {
    //    function link(scope, element, attrs) {
    //        scope.$on('onExpandAll', function (event, args) {
    //            scope.expanded = args.expanded;
    //        });
    //    }
    //    return {
    //        link: link
    //    };
    //})

.directive('expand', function () {
    function link(scope, element, attrs) {
        scope.$on('onExpandAll', function (event, args) {           
            scope.expandedDelivery = args.expandedDelivery;
            scope.expandedDelivery = args.expandedDelivery;
        });
    }
    return {
        link: link
    };
});

   

    ;



