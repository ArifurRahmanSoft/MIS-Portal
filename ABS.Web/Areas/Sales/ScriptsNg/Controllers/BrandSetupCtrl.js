//const { debug } = require("node:util");

//const { localstorage } = require("modernizr");

app.controller('BrandSetupCtrl', ['$scope', 'crudService','conversion', '$filter', '$localStorage', 'uiGridConstants',
    function ($scope, crudService, conversion, $filter, $localStorage, uiGridConstants) {
        //**************************************************Start Vairiable Initialize**************************************************

        var baseUrl = '/Sales/api/ProductSetup/';
        var sysCommonUrl = '/SystemCommon/api/SystemCommonDDL/';

        $scope.permissionPageVisibility = true;
        $scope.UserCommonEntity = {};
        $scope.HeaderToken = {};
        objcmnParam = {};
        $scope.gridOptionsDTML = [];
        $scope.gridOptionslistItemMaster = [];

        $scope.IsCreateIcon = false;
        $scope.IsListIcon = true;

        $scope.ngActiveDate = conversion.NowDateCustom();
        $scope.ngCloseDate = conversion.NowDateCustom();

        $scope.bool = true;

        $scope.btnSaveText = "Save";
        $scope.btnShowText = "Show List";
        $scope.PageTitle = 'Product List';
        $scope.ListTitle = 'Product Rate Records';
        $scope.ListTitlePIDeatails = 'Listed Item of Product  Details';


        $scope.ListIncentiveFormulaSetupDetails = [];
        $scope.listDistTargetMaster = [];
        $scope.listIncentiveFormulaSetupMaster = [];
        $scope.ListIncentiveDistRatio = [];
        $scope.ListAchievementRatioDetail = [];
        $scope.showDtgrid = 0;
        $scope.listDistributor = [];
        $scope.ngModelDistributor = '';

        $scope.ListBrandSkuAdded = [];
        $scope.listProductDetail = [];
        $scope.ProductDetail = [];//ex

        $scope.IsCheckedDistributor = false;
        $scope.IsDisableDistributor = true;
        $scope.IsReadOnlyDistributor = true;
        debugger
        $scope.DistributorCheck = function () {
            if ($scope.IsCheckedDistributor === true) {
                $scope.IsDisableDistributor = false;
                $scope.IsReadOnlyDistributor = false;
            } else {
                $scope.ngModelDistributor = '';
                $("#ddlDistributor").select2("data", { id: '', text: '--Select Distributor--' });

                $scope.IsDisableDistributor = true;
                $scope.IsReadOnlyDistributor = true;
                $scope.CurrentDate = new Date();
            }
        }
        $scope.DistributorCheck();


        //***************************************************End Vairiable Initialize***************************************************
        //$scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { $scope.cmnbtnShowHideEnDisable(num = (toast = $('.toast-warning').attr("style")) == undefined ? $scope.CmnEntity.num : 7); } }
        //***************************************************Start Common Task for all**************************************************
        frmName = 'frmProductSetup'; DelFunc = 'DeleteIncentiveFormulaSetupMasterDetail'; DelMsg = 'IncentiveFormulaSetupNo'; EditFunc = 'loadPIMasterDetailsByActivePI';
        $scope.UserCommonEntity = conversion.UserCmnEntity($scope.menuManager.LoadPageMenu(window.location.pathname), frmName, DelFunc, DelMsg, EditFunc);
        $scope.HeaderToken = conversion.Tokens($scope.tokenManager); $scope.DelParam = {};
        $scope.cmnParam = function () { objcmnParam = conversion.cmnParams(); }
        $scope.CmnMethod = function (FuncEntity, CmnNum) { $scope.CmnEntity = {}; $scope.CmnEntity = conversion.ExecuteCmnFunc(FuncEntity, CmnNum); if (CmnNum != 0 && CmnNum != 2 && CmnNum != 6) { $scope[$scope.CmnEntity.MethodName]($scope.CmnEntity.rowEntity); } if (CmnNum == 2) { for (var i = 0; i < $scope.CmnEntity.MethodName.length; i++) { $scope[$scope.CmnEntity.MethodName[i]]($scope.CmnEntity.rowEntity); } } if (CmnNum == 3) { conversion.SaveUpdatBehave($scope.CmnEntity.num); } }
        $scope.cmnbtnShowHideEnDisable = function (num) { $scope.UserCommonEntity = conversion.btnBehave(num, $scope.UserCommonEntity.IsbtnSaveDisable); }
        $scope.cmnbtnShowHideEnDisable(0);//0=default/reset, 1=Create, 2=Update, 3=Unchange on Update mode btn text, 4=only disable save button, 5=only enable save button
        //****************************************************End Common Task for all***************************************************        

        //Show and Hide Order---**********//
        $scope.IsHidden = true;
        $scope.IsHiddenDetail = true;
        $scope.IsShow = true;
        $scope.IsFormHidden = false;
        $scope.IsHiddenBrandSku = false;
        $scope.IsHiddenRateDetail = false;

        $scope.ShowHide = function () {
            debugger
            $scope.IsShow = $scope.IsShow ? false : true;
            $scope.IsHidden = $scope.IsHidden === true ? false : true;
            $scope.IsHiddenDetail = true;
            if ($scope.IsHidden === true) {
                $scope.btnShowText = "Show List";
                $scope.IsShow = true;
                $scope.IsCreateIcon = false;
                $scope.IsListIcon = true;
                $scope.IsHiddenBrandSku = false;
                $scope.IsHiddenRateDetail = false;
                $scope.productsList = [];//extra
                $scope.ProductDetail = [];//extra
            }
            else {
                $scope.btnShowText = "Create";
                $scope.IsShow = false;
                $scope.IsHidden = false;
                $scope.IsCreateIcon = true;
                $scope.IsListIcon = false;
                //loadMotherVesselMasterRecords(0);
                $scope.IsHiddenBrandSku = true;
                $scope.IsHiddenRateDetail = true;
          
                $scope.ProductDetail = [];//extra
                $scope.productDetailByPage();
            }
        };

   /*     $scope.sstypId = '';
        $scope.listSSTYP = [];
        $scope.GetUserWiseSSTYPList = function () {
            $scope.cmnParam();
            var param = { loggeduser: objcmnParam.loggeduser };
            var apiRoute = sysCommonUrl + 'GetUserWiseSSTYP/';
            var listModel = crudService.getItemWithcmnParam(apiRoute, param);
            listModel.then(function (response) {
                $scope.listSSTYP = response.data.sstypList;
                if ($scope.listSSTYP.length > 0) {
                    $scope.sstypId = $scope.listSSTYP[0].SSTYP_OID;
                    $("#sstypId").select2("data", { id: $scope.sstypId, text: $scope.listSSTYP[0].SSTYP_NAME });

                    $scope.loadBrandRecords();
                }
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }
        $scope.GetUserWiseSSTYPList();
*/
  





  //-----------------------------------------------------Start--------------------------------------
       //GET PRODUCT LIST
        $scope.GetProductDetails = function () {
            debugger
            $scope.listProductDetail = [];
          

            if ($scope.productName === '' && $scope.productCode === ''
                
            ) {
                Command: toastr["warning"]("Please ensure mandatory data with product list !!!");
                return;
            }

        

            var rateMaster = {
                parameter: $scope.productName,
                parameter1: $scope.productCode,
              
            };
 
            var apiRoute = baseUrl + 'GetProductList/';
            console.log("rateMaster", rateMaster, apiRoute)
            var ProductRateSaveUpdate = crudService.getListByParam(apiRoute, rateMaster);
            ProductRateSaveUpdate.then(function (response) {
                if (response !== "") {
                    $scope.listProductDetail = JSON.parse(response.data.resdata);
                    console.log("response of output", $scope.listProductDetail);
                }
                else if (response === "") {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                }
            },
                function (error) {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                });
        };

  


        //GET PRODUCT DETAIL BY CODE 
        $scope.GetDetailsByProductCode = function (product) {
            debugger;
              // $scope.ProductDetail = [];
            let existingProduct = null;
            if ($scope.ProductDetail.length>0) {
                existingProduct = $scope.ProductDetail.find(p => p.sprod_text.slice(0, 3) === product.sprod_text.slice(0, 3));
                if (existingProduct !=null ) {
                    ShowCustomToastrMessageResult(-1);
                } 
            }
          
            if (existingProduct === undefined || existingProduct === null) {
            var proCode = { parameter: product.sprod_text };
            var apiRoute = baseUrl + 'GetProductDetail/';
            var ProductDetail = crudService.getListByParam(apiRoute, proCode);
       
            ProductDetail.then(function (response) {
                if (response !== "") {
                    let details = JSON.parse(response.data.resdata);
                    console.log("product detail-- ", details)

                    details.forEach(item => {
                        $scope.ProductDetail.push({
                            sprodOid:'',
                            sprod_text: item.sprod_text,
                            sprod_name: item.sprod_name,
                            //sprod_opdt: item.sprod_opdt,
                            sprod_opdt: conversion.NowDateCustom(),
                            sprod_bnam: item.sprod_bnam,
                            sprod_snam: item.sprod_snam,
                            sprod_sstyp: item.sprod_sstyp,
                            sprod_scon_typ: item.sprod_scon_typ,
                            sprod_pack: item.sprod_pack,
                            sprod_spcat: item.sprod_spcat,
                            sprod_punt: item.sprod_punt,
                            sprod_funt: item.sprod_funt,
                            sprod_wunt: item.sprod_wunt,
                            sprod_nppk: item.sprod_nppk,//item.sprod_nppk
                            sprod_npwt: item.sprod_npwt,//item.sprod_npwt
                            sprod_npaw: item.sprod_npaw,//item.sprod_npaw
                            sprod_sbrnd: item.sprod_sbrnd,
                            sprod_sdogp: item.sprod_sdogp,
                            sprod_rate: 0,//item.sprod_rate
                            sprod_lfac: item.sprod_lfac,
                            sprod_crat: item.sprod_crat,
                            sprod_mfac: item.sprod_mfac,
                            sprod_dcnt: item.sprod_dcnt,
                            sprod_dchg: item.sprod_dchg,
                            sprod_mchg: item.sprod_mchg,
                            sprod_gchg: item.sprod_gchg,
                            sprod_actv: item.sprod_actv,
                            iuser: item.iuser,
                            euser: item.euser,
                            //idat: item.idat,
                            idat: conversion.NowDateCustom(),
                            //edat: item.edat,
                            edat: conversion.NowDateCustom(),
                            ver: item.ver,
                            //udt: item.udt,
                            udt: conversion.NowDateCustom(),
                            sprod_sprog: item.sprod_sprog,
                            sprod_dlkg: item.sprod_dlkg,
                            sprod_cstyp: item.sprod_cstyp,
                            sprod_pkname: item.sprod_pkname,
                            sprod_pkbnam: item.sprod_pkbnam,
                            sprod_pkrate: item.sprod_pkrate,
                            sprod_pdogp: item.sprod_pdogp,
                            sprod_pntf: item.sprod_pntf,
                            sprod_seqn: item.sprod_seqn,
                            sprod_p8kg: item.sprod_p8kg,
                            sprod_psize: item.sprod_psize,
                            sprod_rnam: item.sprod_rnam,
                            sprod_pnts: item.sprod_pnts,
                            sprod_trgf: item.sprod_trgf,
                            sprod_srom: item.sprod_srom,
                            sprod_wstf: item.sprod_wstf,
                            sprod_fqnt: item.sprod_fqnt,
                            //sprod_litem: item.sprod_litem,
                            sprod_litem:'',// item.sprod_litem,
                            sprod_vtflg: item.sprod_vtflg,
                            sprod_ahad: item.sprod_ahad,
                            sprod_chlkg: item.sprod_chlkg,//item.sprod_chlkg
                            sprod_smscod: item.sprod_smscod,
                            sprod_apsname: item.sprod_apsname,
                            sprod_ltr: item.sprod_ltr,// item.sprod_ltr
                            sprod_packwet: item.sprod_packwet,//item.sprod_packwet
                            sprod_swrate: 0,//item.sprod_swrate
                            sprod_tp_ctn_rate: 0,//item.sprod_tp_ctn_rate
                            sprod_mrp_ctn_rate: 0,//item.sprod_mrp_ctn_rate
                            sprod_swrname: item.sprod_swrname,
                            sprod_swbrcod: item.sprod_swbrcod,
                            sprod_rmfg: item.sprod_rmfg,
                            sprod_block: item.sprod_block, //item.sprod_block
                            sprod_seril: item.sprod_seril,
                            sprod_cost_rate: 0,//item.sprod_cost_rate
                            sprod_dopaper_oid:'',// item.sprod_dopaper_oid,
                            sprod_target_id: item.sprod_target_id,
                            sprod_ltrtokg: item.sprod_ltrtokg,
                            sprod_scm_itmoid: item.sprod_scm_itmoid,
                            sprod_shtbang: item.sprod_shtbang,
                            sprod_barcode: '' //item.sprod_barcode
                         

                        });
                    });
                
                  
                    console.log("response of output of Details", $scope.ProductDetail);
                   
                  
                }
         
            }); 
            }
            };
       
        $scope.addProduct = function () {
            debugger
            if ($scope.ProductDetail.length === 0) {
                return; 
            }
            //let len = $scope.ProductDetail.length;
            let n = $scope.ProductDetail.length - 1;
            let spNum = parseInt($scope.ProductDetail[n].sprod_text)+1;
            let sprodText = spNum.toString().padStart(6, '0');
           

            var newProduct = {
                sprodOid:'',
                sprod_text: sprodText,
                sprod_name: $scope.ProductDetail[n].sprod_name,
                sprod_opdt: $scope.ProductDetail[n].sprod_opdt,
                sprod_bnam: $scope.ProductDetail[n].sprod_bnam,
                sprod_snam: $scope.ProductDetail[n].sprod_snam,
                sprod_sstyp: $scope.ProductDetail[n].sprod_sstyp,
                sprod_scon_typ: $scope.ProductDetail[n].sprod_scon_typ,
                sprod_pack: $scope.ProductDetail[n].sprod_pack,
                sprod_spcat: $scope.ProductDetail[n].sprod_spcat,
                sprod_punt: $scope.ProductDetail[n].sprod_punt,
                sprod_funt: $scope.ProductDetail[n].sprod_funt,
                sprod_wunt: $scope.ProductDetail[n].sprod_wunt,
                sprod_nppk: $scope.ProductDetail[n].sprod_nppk,
                sprod_npwt: $scope.ProductDetail[n].sprod_npwt,
                sprod_npaw: $scope.ProductDetail[n].sprod_npaw,
                sprod_sbrnd: $scope.ProductDetail[n].sprod_sbrnd,
                sprod_sdogp: $scope.ProductDetail[n].sprod_sdogp,
                sprod_rate: $scope.ProductDetail[n].sprod_rate,
                sprod_lfac: $scope.ProductDetail[n].sprod_lfac,
                sprod_crat: $scope.ProductDetail[n].sprod_crat,
                sprod_mfac: $scope.ProductDetail[n].sprod_mfac,
                sprod_dcnt: $scope.ProductDetail[n].sprod_dcnt,
                sprod_dchg: $scope.ProductDetail[n].sprod_dchg,
                sprod_mchg: $scope.ProductDetail[n].sprod_mchg,
                sprod_gchg: $scope.ProductDetail[n].sprod_gchg,
                sprod_actv: $scope.ProductDetail[n].sprod_actv,
                iuser: $scope.ProductDetail[n].iuser,
                euser: $scope.ProductDetail[n].euser,
                idat: $scope.ProductDetail[n].idat,
                edat: $scope.ProductDetail[n].edat,
                ver: $scope.ProductDetail[n].ver,
                udt: $scope.ProductDetail[n].udt,
                sprod_sprog: $scope.ProductDetail[n].sprod_sprog,
                sprod_dlkg: $scope.ProductDetail[n].sprod_dlkg,
                sprod_cstyp: $scope.ProductDetail[n].sprod_cstyp,
                sprod_pkname: $scope.ProductDetail[n].sprod_pkname,
                sprod_pkbnam: $scope.ProductDetail[n].sprod_pkbnam,
                sprod_pkrate: $scope.ProductDetail[n].sprod_pkrate,
                sprod_pdogp: $scope.ProductDetail[n].sprod_pdogp,
                sprod_pntf: $scope.ProductDetail[n].sprod_pntf,
                sprod_seqn: $scope.ProductDetail[n].sprod_seqn,
                sprod_p8kg: $scope.ProductDetail[n].sprod_p8kg,
                sprod_psize: $scope.ProductDetail[n].sprod_psize,
                sprod_rnam: $scope.ProductDetail[n].sprod_rnam,
                sprod_pnts: $scope.ProductDetail[n].sprod_pnts,
                sprod_trgf: $scope.ProductDetail[n].sprod_trgf,
                sprod_srom: $scope.ProductDetail[n].sprod_srom,
                sprod_wstf: $scope.ProductDetail[n].sprod_wstf,
                sprod_fqnt: $scope.ProductDetail[n].sprod_fqnt,
                sprod_litem: $scope.ProductDetail[n].sprod_litem,
                sprod_vtflg: $scope.ProductDetail[n].sprod_vtflg,
                sprod_ahad: $scope.ProductDetail[n].sprod_ahad,
                sprod_chlkg: $scope.ProductDetail[n].sprod_chlkg,
                sprod_smscod: $scope.ProductDetail[n].sprod_smscod,
                sprod_apsname: $scope.ProductDetail[n].sprod_apsname,
                sprod_ltr: $scope.ProductDetail[n].sprod_ltr,
                sprod_packwet: $scope.ProductDetail[n].sprod_packwet,
                sprod_swrate: $scope.ProductDetail[n].sprod_swrate,
                sprod_tp_ctn_rate: $scope.ProductDetail[n].sprod_tp_ctn_rate,
                sprod_mrp_ctn_rate: $scope.ProductDetail[n].sprod_mrp_ctn_rate,
                sprod_swrname: $scope.ProductDetail[n].sprod_swrname,
                sprod_swbrcod: $scope.ProductDetail[n].sprod_swbrcod,
                sprod_rmfg: $scope.ProductDetail[n].sprod_rmfg,
                sprod_block: $scope.ProductDetail[n].sprod_block,
                sprod_seril: $scope.ProductDetail[n].sprod_seril,
                sprod_cost_rate: $scope.ProductDetail[n].sprod_cost_rate,
                sprod_dopaper_oid: $scope.ProductDetail[n].sprod_dopaper_oid,
                sprod_target_id: $scope.ProductDetail[n].sprod_target_id,
                sprod_ltrtokg: $scope.ProductDetail[n].sprod_ltrtokg,
                sprod_scm_itmoid: $scope.ProductDetail[n].sprod_scm_itmoid,
                sprod_shtbang: $scope.ProductDetail[n].sprod_shtbang,
                sprod_barcode: $scope.ProductDetail[n].sprod_barcode
               


            };
            $scope.ProductDetail.push(newProduct);
        };

        //EDIT PRODUCT
        $scope.editProduct = function (product) {
            debugger
            $scope.productsList = [];
            var apiRoute = baseUrl + 'GetProductDetailById/';
            var proCode = { parameter: product.id };
            var prodDetail = crudService.getListByParam(apiRoute, proCode);
            prodDetail.then(function (response) {
                let prodDtl = JSON.parse(response.data.resdata);
                console.log("Product Detail by id", prodDtl)
                prodDtl.forEach(item => {
                    $scope.ProductDetail.push({
                        sprodOid: item.oid,
                        sprod_text: item.sprod_text,
                        sprod_name: item.sprod_name,
                        sprod_opdt: item.sprod_opdt,
                        //sprod_opdt: conversion.NowDateCustom(),
                        sprod_bnam: item.sprod_bnam,
                        sprod_snam: item.sprod_snam,
                        sprod_sstyp: item.sprod_sstyp,
                        sprod_scon_typ: item.sprod_scon_typ,
                        sprod_pack: item.sprod_pack,
                        sprod_spcat: item.sprod_spcat,
                        sprod_punt: item.sprod_punt,
                        sprod_funt: item.sprod_funt,
                        sprod_wunt: item.sprod_wunt,
                        sprod_nppk: item.sprod_nppk,//item.sprod_nppk
                        sprod_npwt: item.sprod_npwt,//item.sprod_npwt
                        sprod_npaw: item.sprod_npaw,//item.sprod_npaw
                        sprod_sbrnd: item.sprod_sbrnd,
                        sprod_sdogp: item.sprod_sdogp,
                        sprod_rate: item.sprod_rate,//item.sprod_rate
                        sprod_lfac: item.sprod_lfac,
                        sprod_crat: item.sprod_crat,
                        sprod_mfac: item.sprod_mfac,
                        sprod_dcnt: item.sprod_dcnt,
                        sprod_dchg: item.sprod_dchg,
                        sprod_mchg: item.sprod_mchg,
                        sprod_gchg: item.sprod_gchg,
                        sprod_actv: item.sprod_actv,
                        iuser: item.iuser,
                        euser: item.euser,
                        idat: item.idat,
                        //idat: conversion.NowDateCustom(),
                        edat: item.edat,
                        ver: item.ver,
                        udt: item.udt,
                        sprod_sprog: item.sprod_sprog,
                        sprod_dlkg: item.sprod_dlkg,
                        sprod_cstyp: item.sprod_cstyp,
                        sprod_pkname: item.sprod_pkname,
                        sprod_pkbnam: item.sprod_pkbnam,
                        sprod_pkrate: item.sprod_pkrate,
                        sprod_pdogp: item.sprod_pdogp,
                        sprod_pntf: item.sprod_pntf,
                        sprod_seqn: item.sprod_seqn,
                        sprod_p8kg: item.sprod_p8kg,
                        sprod_psize: item.sprod_psize,
                        sprod_rnam: item.sprod_rnam,
                        sprod_pnts: item.sprod_pnts,
                        sprod_trgf: item.sprod_trgf,
                        sprod_srom: item.sprod_srom,
                        sprod_wstf: item.sprod_wstf,
                        sprod_fqnt: item.sprod_fqnt,
                        //sprod_litem: item.sprod_litem,
                        sprod_litem: item.sprod_litem,// item.sprod_litem,
                        sprod_vtflg: item.sprod_vtflg,
                        sprod_ahad: item.sprod_ahad,
                        sprod_chlkg: item.sprod_chlkg,//item.sprod_chlkg
                        sprod_smscod: item.sprod_smscod,
                        sprod_apsname: item.sprod_apsname,
                        sprod_ltr: item.sprod_ltr,// item.sprod_ltr
                        sprod_packwet: item.sprod_packwet,//item.sprod_packwet
                        sprod_swrate: item.sprod_swrate,//item.sprod_swrate
                        sprod_tp_ctn_rate: item.sprod_tp_ctn_rate,//item.sprod_tp_ctn_rate
                        sprod_mrp_ctn_rate: item.sprod_mrp_ctn_rate,//item.sprod_mrp_ctn_rate
                        sprod_swrname: item.sprod_swrname,
                        sprod_swbrcod: item.sprod_swbrcod,
                        sprod_rmfg: item.sprod_rmfg,
                        sprod_block: item.sprod_block, //item.sprod_block
                        sprod_seril: item.sprod_seril,
                        sprod_cost_rate: item.sprod_cost_rate,//item.sprod_cost_rate
                        sprod_dopaper_oid: item.sprod_dopaper_oid,
                        sprod_target_id: item.sprod_target_id,
                        sprod_ltrtokg: item.sprod_ltrtokg,
                        sprod_scm_itmoid: item.sprod_scm_itmoid,
                        sprod_shtbang: item.sprod_shtbang,
                        sprod_barcode: item.sprod_barcode //item.sprod_barcode

                    })
                })
                
                

            }
            
            )};
 

        $scope.removeProduct = function (product) {
            debugger
          
            var IsConf = confirm('Are you sure You Want to Delete ' + product.sprod_name +'?');
            if (IsConf) {
                let index = $scope.ProductDetail.indexOf(product);
                let IsDelete = $scope.ProductDetail[index].sprod_litem || '';
                console.log("IsDelete value is ", IsDelete);
                if (IsDelete !== '') {
                    confirm('You Can Not Remove  ' + product.sprod_name +'Litem already Exist');
                    return;
                }
                if (index !== -1 && !IsDelete) {
                    $scope.ProductDetail.splice(index, 1);
                }
            }
          
        };

        //SAVE UPDATE PRODUCT DETIALS 

        $scope.SaveProductDetail = function () {
            debugger
            var id = $localStorage.loggedUserID
            console.log("id", id)
            var productDetailList = angular.copy($scope.ProductDetail);
            var productList = [productDetailList, id];
            var apiRoute = baseUrl + 'SaveUpdateProductDetails/';
            var ProductRateSaveUpdate = crudService.post(apiRoute, productList);
            ProductRateSaveUpdate.then(function (response) {
                if (response.data == "Success") {
                    ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                    $scope.ProductDetail = [];
                    window.location.reload();
                }

                else if (response === "") {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                }
            },
                function (error) {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                });
        };


        //DELETE PRODUCT BY ID
        $scope.DeleteProdById = function (product) {
            debugger
            var IsConf = confirm('Are you sure You Want to Delete ' + product.name + '?');
            if (IsConf) {
                var userId = $localStorage.loggedUserID
                var prodId = product.id;
                var deleteProduct = [prodId, userId];
                var apiRoute = baseUrl + 'DeleteProduct/';
                var ProductRateSaveUpdate = crudService.post(apiRoute, deleteProduct);
                ProductRateSaveUpdate.then(function (response) {
                    if (response.data == "Success") {
                        ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                        window.location.reload();
                    }

                    else if (response === "") {
                        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    }
                },
                    function (error) {
                        ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    });
            }
          
 
        };

        $scope.productDetailByPage = function () {
            debugger;
            //$scope.ProductDetail = [];//add here for remove
            var apiRoute = baseUrl + 'GetProductListByPage/';
            var listProduct = crudService.getAllList(apiRoute);
            listProduct.then(function (response) {
                let type = JSON.parse(response.data.resdata);
                $scope.productsList = [];

                // Push formatted data into productType
                type.forEach(item => {
                    $scope.productsList.push({
                        id: item.oid,
                        text: item.prodText,
                        name: item.prodName,
                        bngName: item.prodBnName,
                        shortName: item.prodShrtName,
                        rate: item.prodRate,
                        litemId: item.lItem
                    });
                });

               
                console.log("Formatted productsList:", $scope.productsList);
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.productDetailByPage();

        //LOAD PRODUCT TYPE


        $scope.loadProductType = function () {
            debugger;
          /*  $("#ddlProductType").select2("data", { id: '', text: '--Select Product Type--' });*/
        
            var param = { parameter: $scope.sstypId };
            var apiRoute = baseUrl + 'GetProductType/';
            var listProductType = crudService.getAllList(apiRoute);
            listProductType.then(function (response) {
                let list = JSON.parse(response.data.resdata);
                $scope.productType = [];

                // Push formatted data into productType
                list.forEach(item => {
                    $scope.productType.push({
                        id: item.sstypId,
                        text: item.sstypName
                    });
                });

                console.log("Formatted productType:", $scope.productType);
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.loadProductType();




        $scope.loadSconType = function () {
            debugger;
         /*   $("#ddlSconType").select2("data", { id: '', text: '--Select Scon Type--' });*/

            var param = { parameter: $scope.sstypId };
            var apiRoute = baseUrl + 'GetSconType/';
            var listSconType = crudService.getAllList(apiRoute);
            listSconType.then(function (response) {
                let type = JSON.parse(response.data.resdata);
                $scope.sconType = [];

                // Push formatted data into productType
                type.forEach(item => {
                    $scope.sconType.push({
                        id: item.sconTypeId,
                        text: item.sconName
                    });
                });

                console.log("Formatted productType:", $scope.sconType);
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.loadSconType();

      
        $scope.loadProductCat = function () {
            debugger;
            $("#ddlProductCat").select2("data", { id: '', text: '--Select Product Catagory--' });

            var param = { parameter: $scope.sstypId };
            var apiRoute = baseUrl + 'GetSpcat/';
            var listProductCat = crudService.getAllList(apiRoute);
            listProductCat.then(function (response) {
                let type = JSON.parse(response.data.resdata);
                $scope.ProductCat = [];

                // Push formatted data into productType
                type.forEach(item => {
                    $scope.ProductCat.push({
                        id: item.spcatId,
                        text: item.spcatName
                    });
                });

                console.log("Formatted loadProductCat:", $scope.ProductCat);
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.loadProductCat();



        
        $scope.loadProductSerial = function () {
            debugger;
            $("#ddlProductSerial").select2("data", { id: '', text: '--Select Product Serial--' });
            var apiRoute = baseUrl + 'GetProductSerial/';
            var listProductSerial = crudService.getAllList(apiRoute);
            listProductSerial.then(function (response) {
                let type = JSON.parse(response.data.resdata);
                $scope.ProductSerial = [];

                type.forEach(item => {
                    $scope.ProductSerial.push({
                        id: item.sprodSerilId,
                        text: item.sprodSerilName
                    });
                });
                console.log("Formatted ProductSerial:", $scope.ProductSerial);
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.loadProductSerial();

      
        $scope.loadProductSize = function () {
            debugger;
         /*   $("#ddlProductSize").select2("data", { id: '', text: '--Select Product Size--' });*/
            var apiRoute = baseUrl + 'GetProductSize/';
            var listProductSize = crudService.getAllList(apiRoute);
            listProductSize.then(function (response) {
                let list = JSON.parse(response.data.resdata);
                $scope.ProductSize = [];
                list.forEach(item => {
                    $scope.ProductSize.push({
                        id: item.psizeId,
                        text: item.psizeName
                    });
                });
                console.log("Formatted ProductSize:", $scope.ProductSize);
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.loadProductSize();

        
        $scope.loadProductGroup = function () {
            debugger;
       /*     $("#ddlProductGrp").select2("data", { id: '', text: '--Select Product Group--' });*/
            var apiRoute = baseUrl + 'GetProductGroup/';
            var listProductGroup = crudService.getAllList(apiRoute);
            listProductGroup.then(function (response) {
                let list = JSON.parse(response.data.resdata);
                $scope.ProductGroup = [];
                list.forEach(item => {
                    $scope.ProductGroup.push({
                        id: item.sprogId,
                        text: item.sprogName
                    });
                });
                console.log("Formatted ProductGroup:", $scope.ProductGroup);
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.loadProductGroup();


       
        $scope.loadDoGroup = function () {
            debugger;
        /*    $("#ddlDoGroup").select2("data", { id: '', text: '--Select DO Group--' });*/
            var apiRoute = baseUrl + 'GetDoGroup/';
            var listDoGroup = crudService.getAllList(apiRoute);
            listDoGroup.then(function (response) {
                let list = JSON.parse(response.data.resdata);
                $scope.DoGroup = [];
                list.forEach(item => {
                    $scope.DoGroup.push({
                        id: item.sdogpId,
                        text: item.sdogpName
                    });
                });
                console.log("Formatted productType:", $scope.DoGroup);
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.loadDoGroup();

       
        $scope.loadProductBrand = function () {
            debugger;
       /*     $("#ddlProductBrand").select2("data", { id: '', text: '--Select Brand--' });*/
            var apiRoute = baseUrl + 'GetSBrand/';
            var listProductBrand = crudService.getAllList(apiRoute);
            listProductBrand.then(function (response) {
                let list = JSON.parse(response.data.resdata);
                $scope.ProductBrand = [];
                list.forEach(item => {
                    $scope.ProductBrand.push({
                        id: item.sbrandId,
                        text: item.sbrandName
                    });
                });
                console.log("Formatted ProductBrand:", $scope.ProductBrand);
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.loadProductBrand();


        
        $scope.loadSmunt = function () {
            debugger;
           /* $("#ddlSmunt").select2("data", { id: '', text: '--Select loadSmunt--' });*/
            var apiRoute = baseUrl + 'GetSmunt/';
            var listSmunt = crudService.getAllList(apiRoute);
            listSmunt.then(function (response) {
                let list = JSON.parse(response.data.resdata);
                $scope.Smunt = [];
                list.forEach(item => {
                    $scope.Smunt.push({
                        id: item.smuntId,
                        text: item.smuntName
                    });
                });
                console.log("Formatted ProductBrand:", $scope.Smunt);
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.loadSmunt(); 


        //get L item data -----------------------------------
        $scope.GetDetailsByLitemCode = function (prod) {
            debugger
            $scope.index = $scope.ProductDetail.indexOf(prod);
            $("#lItemModal").modal("show");
            var sprodOid = "SPRODxxxxxxxxx" + prod.sprod_text;
           
         
            $scope.LitemProduct = [];
            $scope.LitemProduct.push({
                lItemOid:'',
                prodName: prod.sprod_name,
                prodBnName: prod.sprod_bnam,
                sprodId: sprodOid,
                isActv: 'Y',
                prodType: 'T',
                itemType: 'C',
                prodGroup: '',
                subGroup: '',
                rateType: '1',
                itemSign: 1,
                itemFlag: 1,
                itemGenId: '',
                itemInvId:''
            });
            $scope.lItem = $scope.LitemProduct[0];
            $scope.toggleIsActive = function () {
                debugger
                $scope.lItem.isActv = $scope.lItem.isActv === 'Y' ? 'N' : 'Y';
            };
        };


        //get litem code form database for edit
        $scope.GetLitemById = function (prod) {
            debugger
            $("#lItemModal").modal("show");
            var apiRoute = baseUrl + 'GetLitemDetail/';
            var sprodOid = "SPRODxxxxxxxxx" + prod.sprod_text;
            var proCode = { parameter: sprodOid };
    
            var litemDetail = crudService.getListByParam(apiRoute, proCode);
            litemDetail.then(function (response) {
                let lItemLst = JSON.parse(response.data.resdata);
                let litem = lItemLst[0]
                console.log("lItem", litem)





                $scope.LitemProduct = [];
                    $scope.LitemProduct.push({
                        lItemOid: litem.oid,
                        prodName: litem.litem_name,
                        prodBnName: litem.litem_bnam,
                        sprodId: litem.litem_sprod,
                        isActv: litem.litem_actv,
                        prodType: 'T',
                        itemType: litem.litem_type,
                        prodGroup: litem.litem_group,
                        subGroup: litem.litem_sub_grp,
                        rateType: litem.litem_rate_type,
                        itemSign: litem.litem_sign,
                        itemFlag: litem.litem_flag,
                        itemGenId: '',
                        itemInvId: ''
                    });
                $scope.lItem = $scope.LitemProduct[0];
                console.log("For Update $scope.lItem", $scope.lItem)
                    $scope.toggleIsActive = function () {
                        debugger
                        $scope.lItem.isActv = $scope.lItem.isActv === 'Y' ? 'N' : 'Y';
                    };

            });
        };

        

        $scope.SavelItemDetails = function () {
            debugger
            var id = $localStorage.loggedUserID;
            var prodLitem = angular.copy($scope.lItem);
            var productLitem = [prodLitem, id];
            var apiRoute = baseUrl + 'SaveUpdateLitem/';
            var lItemSaveUpdate = crudService.post(apiRoute, productLitem);
            lItemSaveUpdate.then(function (response) {
                console.log("Liyem outpur is", response)
                if (response.data !== "Error") {
                    $scope.ProductDetail[$scope.index].sprod_litem = response.data;
                    console.log("Update  $scope.ProductDetail", $scope.ProductDetail)
                    ShowCustomToastrMessageSaveUpdate(1, $scope.UserCommonEntity);
                    $scope.productLitem = [];
                    $scope.lItem = [];
                }

                else if (response === "") {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                    $scope.lItem = [];
                }
            },
                function (error) {
                    ShowCustomToastrMessageSaveUpdate(0, $scope.UserCommonEntity);
                });


        }

        //-----------------------------------------------------End--------------------------------------





        $scope.clears = function () {
            $scope.productName = '';
            $scope.productCode = '';
        }

      
    }
]);


function modal_fadeOut() {
    $("#PIModal").fadeOut(200, function () {
        $('#PIModal').modal('hide');
    });
}



