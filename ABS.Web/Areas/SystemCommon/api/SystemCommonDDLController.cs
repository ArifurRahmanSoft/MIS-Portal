using CTGroup.Models;
using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using CTGroup.Service.SystemCommon.Factories;
using CTGroup.Service.SystemCommon.Interfaces;
using CTGroup.Utility.Common;
using CTGroup.Web.Attributes;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace CTGroup.Web.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/SystemCommonDDL")]
    public class SystemCommonDDLController : ApiController
    {
        private iSystemCommonDDL objDDLService = null;
        public SystemCommonDDLController()
        {
            this.objDDLService = new SystemCommonDDL();
        }



        //----------------------------Start-----------------------------------------



        [Route("GetShowroom/{userId}/")]
        public IEnumerable<vmShowrooms> GetShowroom(string userId)
        {
            IEnumerable<vmShowrooms> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetShowroomList(userId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }


        [Route("GetProduct/{userId}/")]
        public IEnumerable<vmProducts> GetProduct(string userId)
        {
            IEnumerable<vmProducts> objProductList = null;
            try
            {
                objProductList = objDDLService.GetProductList(userId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objProductList;
        }


        //---------------------------------------End---------------------------------------

        //----------------------------------------Trial Balance Start-----------------------
 

        [Route("GetLocation/{userId}/")]
        public IEnumerable<vmLocation> GetLocation(string userId)
        {
            IEnumerable<vmLocation> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetLocationList(userId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }



        [Route("GetMasterCompany/{userId}/")]
        public IEnumerable<vmMComapnay> GetMasterCompany(string userId)
        {
            IEnumerable<vmMComapnay> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetMasterCompany(userId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }



                [HttpPost, BasicAuthorization]
                public IHttpActionResult GetChildCompany(object[] data)
                {
                    object objChildCompanyList = null;
                    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                    try
                    {
                       objChildCompanyList = objDDLService.GetChildCompany(objcmnParam);
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }
                    return Json(new
                    {
                        objChildCompanyList
                    });
                }

        
            [HttpPost, BasicAuthorization]
            public IHttpActionResult GetSubHead(object[] data)
             {
            object objSubHeadList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objSubHeadList = objDDLService.GetSubHead(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objSubHeadList
            });
        }




        
        [Route("GetGroupHead/{userId}/")]
        public IEnumerable<vmLocation> GetGroupHead(string userId)
        {
            IEnumerable<vmLocation> grpHeadListList = null;
            try
            {
                grpHeadListList = objDDLService.GetGroupHead(userId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return grpHeadListList;
        }

        
        [HttpPost, BasicAuthorization]
         public IHttpActionResult GetAcHead(object[] data)
        {
            object objAcHeadList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objAcHeadList = objDDLService.GetAcHead(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objAcHeadList
            });
        }





        //----------------------------------------Trial Balance End-----------------------




        //[Route("GetSKUByBrand/{mode}/{trackNo}/")]
        public IEnumerable<vmProduct> GetRowMetarialsProductList()
        {
            IEnumerable<vmProduct> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetRowMetarialsProductList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetPermissionForReport/{loggeduser}/{userrole}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(vmSalesReportPermission)), BasicAuthorization]
        public IEnumerable<vmSalesReportPermission> GetPermissionForReport(string loggeduser, string userRole, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmSalesReportPermission> objPermissionList = null;
            try
            {
                objPermissionList = objDDLService.GetPermissionForReport(loggeduser, userRole);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objPermissionList;
        }

        [Route("GetBrandListByNational/{mode}/{trackNo}/")]
        public IEnumerable<vmBrandSKU> GetBrandListByNational(string mode, string trackNo)
        {
            IEnumerable<vmBrandSKU> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetBrandListByNational(mode, trackNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetBrandListByNational/{nationalOid}/{brndGroupId}/{userId}/")]
        public IEnumerable<vmBrandSKU> GetBrandListByNational(string nationalOid, string brndGroupId, string userId)
        {
            IEnumerable<vmBrandSKU> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetBrandListByNational(nationalOid, brndGroupId, userId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetBrandGroupListByUser/{mode}/{loggedUserId}/")]
        public DataTable GetBrandGroupListByUser(string mode, string loggedUserId)
        {
            DataTable objSalesHierList = new DataTable();
            try
            {
                objSalesHierList = objDDLService.GetBrandGroupListByUser(mode, loggedUserId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetAreaWiseDistributor/{national}/{division}/{region}/{zone}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(SSalesAreaHierarchy)), BasicAuthorization]
        public IEnumerable<SSalesAreaHierarchy> GetAreaWiseDistributor(string national, string division, string region, string zone, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<SSalesAreaHierarchy> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetAreaWiseDistributor(national, division, region, zone);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetAreaWiseDistributorPrimary/{national}/{division}/{region}/{zone}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(SSalesAreaHierarchy)), BasicAuthorization]
        public IEnumerable<SSalesAreaHierarchy> GetAreaWiseDistributorPrimary(string national, string division, string region, string zone, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<SSalesAreaHierarchy> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetAreaWiseDistributorPrimary(national, division, region, zone);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetProductCategory/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(ProductCategory)), BasicAuthorization]
        public IEnumerable<ProductCategory> GetProductCategory(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<ProductCategory> objProductCategory = null;
            try
            {
                objProductCategory = objDDLService.GetProductCategory();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objProductCategory;
        }

        [Route("GetSSalesAreaHierarchyList/{mode}/{trackNo}/")]
        public IEnumerable<SSalesAreaHierarchy> GetSSalesAreaHierarchyList(string mode, string trackNo)
        {
            IEnumerable<SSalesAreaHierarchy> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetSSalesAreaHierarchyList(mode, trackNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetSSalesAreaHierarchyList/{mode}/{trackNo}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(SSalesAreaHierarchy))]
        public IEnumerable<SSalesAreaHierarchy> GetSSalesAreaHierarchyList(string mode, string trackNo, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<SSalesAreaHierarchy> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetSSalesAreaHierarchyList(mode, trackNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }



        /// <summary>
        /// ///////// handling secondary data issue //////////////
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="trackNo"></param>
        /// <returns></returns>

        [Route("GetSalesAreaHierarchySecondaryList/{mode}/{trackNo}/")]
        public IEnumerable<SSalesAreaHierarchy> GetSalesAreaHierarchySecondaryList(string mode, string trackNo)
        {
            IEnumerable<SSalesAreaHierarchy> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetSalesAreaHierarchySecondaryList(mode, trackNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetSalesAreaHierarchySecondaryList/{mode}/{trackNo}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(SSalesAreaHierarchy)), BasicAuthorization]
        public IEnumerable<SSalesAreaHierarchy> GetSalesAreaHierarchySecondaryList(string mode, string trackNo, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<SSalesAreaHierarchy> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetSalesAreaHierarchySecondaryList(mode, trackNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [HttpPost]
        public IHttpActionResult GetCTGStaff(object[] data)
        {
            List<vmEmployee> listCTGStaff = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                listCTGStaff = objDDLService.GetCTGStaff(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                listCTGStaff
            });
        }


        [Route("GetAreaWiseDistributorSecondary/{national}/{division}/{region}/{zone}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(SSalesAreaHierarchy)), BasicAuthorization]
        public IEnumerable<SSalesAreaHierarchy> GetAreaWiseDistributorSecondary(string national, string division, string region, string zone, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<SSalesAreaHierarchy> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetAreaWiseDistributorSecondary(national, division, region, zone);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetAreaWiseSalesPerson/{national}/{division}/{region}/{zone}/{distributor}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(SSalesAreaHierarchy)), BasicAuthorization]
        public IEnumerable<SSalesAreaHierarchy> GetAreaWiseSalesPerson(string national, string division, string region, string zone, string distributor, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<SSalesAreaHierarchy> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetAreaWiseSalesPerson(national, division, region, zone, distributor);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBrand(object[] data)
        {
            IEnumerable<vmBrandSKU> objBrandList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objBrandList = objDDLService.GetBrand(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objBrandList
            });
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLocation(object[] data)
        {
            IEnumerable<vmBrandSKU> objLocationList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objLocationList = objDDLService.GetLocation(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objLocationList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetProductGroupRupshiFood(object[] data)
        {
            object objProdGroupList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objProdGroupList = objDDLService.GetProductGroupRupshiFood(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objProdGroupList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSalesGroupRupshiFood(object[] data)
        {
            object objSalesGroupList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objSalesGroupList = objDDLService.GetSalesGroupRupshiFood(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objSalesGroupList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllProductByBrand(object[] data)
        {
            object skuList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                skuList = objDDLService.GetAllProductByBrand(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                skuList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllProductBySSTYP(object[] data)
        {
            object productList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                productList = objDDLService.GetAllProductBySSTYP(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                productList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllSalesGroup(object[] data)
        {
            object slsGroupList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                slsGroupList = objDDLService.GetAllSalesGroup(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                slsGroupList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllProductGroup(object[] data)
        {
            object prdGroupList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                prdGroupList = objDDLService.GetAllProductGroup(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                prdGroupList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllBrandGroup(object[] data)
        {
            object brndGroupList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                brndGroupList = objDDLService.GetAllBrandGroup(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                brndGroupList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllBrand(object[] data)
        {
            object brandList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                brandList = objDDLService.GetAllBrand(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                brandList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllFilteredLocation(object[] data)
        {
            object locationList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                locationList = objDDLService.GetAllFilteredLocation(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                locationList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetFilteredLocation(object[] data)
        {
            IEnumerable<vmBrandSKU> objLocationList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objLocationList = objDDLService.GetFilteredLocation(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objLocationList
            });
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetProductGroup(object[] data)
        {
            IEnumerable<vmBrandSKU> objProductGroup = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objProductGroup = objDDLService.GetProductGroup(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objProductGroup
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSalesLocation(object[] data)
        {
            IEnumerable<vmBrandSKU> objLocationList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objLocationList = objDDLService.GetSalesLocation(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objLocationList
            });
        }

        [Route("GetBulkSKUByBrand/{mode}/{trackNo}/")]
        public IEnumerable<vmBrandSKU> GetBulkSKUByBrand(string mode, string trackNo)
        {
            IEnumerable<vmBrandSKU> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetBulkSKUByBrand(mode, trackNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetSalesSKUByBrand/{mode}/{trackNo}/{sstypId}")]
        public IEnumerable<vmBrandSKU> GetSalesSKUByBrand(string mode, string trackNo, string sstypId)
        {
            IEnumerable<vmBrandSKU> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetSalesSKUByBrand(mode, trackNo, sstypId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetSKUByBrand/{mode}/{trackNo}/")]
        public IEnumerable<vmBrandSKU> GetSKUByBrand(string mode, string trackNo)
        {
            IEnumerable<vmBrandSKU> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetSKUByBrand(mode, trackNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }



        [Route("GetRouteByDistributor/{mode}/{trackNo}/")]
        public List<SSalesAreaHierarchy> GetRouteByDistributor(string mode, string trackNo)
        {
            List<SSalesAreaHierarchy> modeList = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.GET_ROUTE_BY_DISTRIBUTOR";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_distributoId", OracleDbType.Varchar2).Value = trackNo;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetSecondaryBasic(objCmd);
            modeList = ConvertDataTableToGenericList.BindList<SSalesAreaHierarchy>(dt);
            return modeList;
        }


        [Route("GetBrandByCategoryCC/{mode}/{trackNo}/")]
        public IEnumerable<vmBrandSKU> GetBrandByCategoryCC(string mode, string trackNo)
        {
            IEnumerable<vmBrandSKU> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetBrandByCategoryCC(mode, trackNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetBrandByCategoryCC/{categoryid}/{brndGroupId}/{userId}/")]
        public IEnumerable<vmBrandSKU> GetBrandByCategoryCC(string categoryid, string brndGroupId, string userId)
        {
            IEnumerable<vmBrandSKU> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetBrandByCategoryCC(categoryid, brndGroupId, userId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("Get_PROD_CAT_BY_NTN/{mode}/{trackNo}/")]
        public IEnumerable<vmBrandSKU> Get_PROD_CAT_BY_NTN(string mode, string trackNo)
        {
            IEnumerable<vmBrandSKU> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.Get_PROD_CAT_BY_NTN(mode, trackNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }

        [Route("GetSalesTeamBrand/{mode}/{trackNo}/")]
        public IEnumerable<vmBrandSKU> GetSalesTeamBrand(string mode, string trackNo)
        {
            IEnumerable<vmBrandSKU> objSalesHierList = null;
            try
            {
                objSalesHierList = objDDLService.GetSalesTeamBrand(mode, trackNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSalesHierList;
        }



        [Route("GetProductCategory/{mode}/{trackNo}/")]
        public IEnumerable<ProductCategory> GetProductCategory()
        {
            IEnumerable<ProductCategory> objProductCategory = null;
            try
            {
                objProductCategory = objDDLService.GetProductCategory();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objProductCategory;
        }

        //[Route("Get_PROD_CAT_BY_NTN/{mode}/{trackNo}/")]
        //public IEnumerable<ProductCategory> Get_PROD_CAT_BY_NTN()
        //{
        //    IEnumerable<ProductCategory> objProductCategory = null;
        //    try
        //    {
        //        objProductCategory = objDDLService.Get_PROD_CAT_BY_NTN();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objProductCategory;
        //}

        [Route("GetProductType/{mode}/{trackNo}/")]
        public IEnumerable<ProductType> GetProductType()
        {
            IEnumerable<ProductType> objProductType = null;
            try
            {
                objProductType = objDDLService.GetProductType();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objProductType;
        }
        [Route("GetSCON_TYP/{mode}/{trackNo}/")]
        public IEnumerable<SecondaryType> GetSCON_TYP()
        {
            IEnumerable<SecondaryType> objProductType = null;
            try
            {
                objProductType = objDDLService.GetSCON_TYP();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objProductType;
        }

        [Route("GetProductTypeSSTYP/{mode}/{trackNo}/")]
        public IEnumerable<ProductTypeSSTYP> GetProductTypeSSTYP()
        {
            IEnumerable<ProductTypeSSTYP> objProductTypeSSTYP = null;
            try
            {
                objProductTypeSSTYP = objDDLService.GetProductTypeSSTYP();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objProductTypeSSTYP;
        }

        [Route("GetBrandByCategory/{mode}/{trackNo}/")]
        public IEnumerable<vmBrandSKU> GetBrandByCategory(string mode, string trackNo)
        {
            IEnumerable<vmBrandSKU> objBrand = null;
            try
            {
                objBrand = objDDLService.GetBrandByCategory(mode);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBrand;
        }




        [Route("GetBrandByCategory/{categoryId}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(vmBrandSKU)), BasicAuthorization]
        public IEnumerable<vmBrandSKU> GetBrandByCategory(string categoryId, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmBrandSKU> objBrand = null;
            try
            {
                objBrand = objDDLService.GetBrandByCategory(categoryId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBrand;
        }



        [HttpPost]
        public IHttpActionResult GetSalesAreaHierarchyByUser(object[] data)
        {
            IEnumerable<SSalesAreaHierarchy> objSalesHierList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objSalesHierList = objDDLService.GetSalesAreaHierarchyByUser(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objSalesHierList
            });
        }

        [HttpPost]
        public IHttpActionResult GetAllNational(object[] data)
        {
            object nationalList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                nationalList = objDDLService.GetAllNational(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                nationalList
            });
        }

        [Route("GetUser/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnCompany))]
        [HttpGet]
        public List<vmEmployee> GetUser(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmEmployee> obj = null;
            try
            {
                obj = objDDLService.GetUserForDropDownList(companyID, loggedUser, pageNumber, pageSize, IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return obj;
        }

        // GET: GetCustomers/0/10/0
        [Route("GetModuleWithPermission/{companyID:int}/{userID:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(T_CMNMODULE))]
        [HttpGet]
        public IEnumerable<vmCmnModule> GetModuleWithPermission(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmCmnModule> objListModules = null;
            try
            {
                objListModules = objDDLService.GetModuleWithPermission(companyID, userID, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListModules;
        }


        [Route("GetParentMenuForDropDown/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ModuleID:int}")]
        [ResponseType(typeof(T_CMNMODULE))]
        [HttpGet]
        public IEnumerable<T_CMNMENU> GetParentMenuForDropDown(int? pageNumber, int? pageSize, int? IsPaging, int? ModuleID)
        {
            IEnumerable<T_CMNMENU> listMenues = null;
            try
            {
                listMenues = objDDLService.GetParentMenuForDropDown(pageNumber, pageSize, IsPaging, ModuleID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listMenues;
        }

        public List<vmCompany> getAllCompanyList()
        {
            List<vmCompany> _list = new List<vmCompany>();

            vmCompany num1 = new vmCompany("SCOMPxxxxxxxxxxxxx94", "RUPSHI FEED MILLS LTD");
            vmCompany num2 = new vmCompany("SCOMPxxxxxxxxxxxxx13", "CITY FEED PRODUCTS LTD");
            vmCompany num3 = new vmCompany("SCOMPxxxxxxxxxxxxx32", "M/S. M HASAN & CO");
            _list.Add(num1);
            _list.Add(num2);
            _list.Add(num3);

            return _list;
        }

        public List<vmCityAllCompany> getCityCompanyList()
        {
            List<vmCityAllCompany> mCompanyList = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_Company";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            mCompanyList = ConvertDataTableToGenericList.BindList<vmCityAllCompany>(dt);
            return mCompanyList;
        }

        public List<vmCityAllCompany> GetExportCompanyList()
        {
            List<vmCityAllCompany> mCompanyList = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_Company_Export";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = null;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            mCompanyList = ConvertDataTableToGenericList.BindList<vmCityAllCompany>(dt);
            return mCompanyList;
        }

        public List<vmTranMode> getTransactionMode()
        {
            List<vmTranMode> modeList = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_Tran_Mode";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            modeList = ConvertDataTableToGenericList.BindList<vmTranMode>(dt);
            return modeList;
        }

        public List<vmTranMode> getPostMode()
        {
            List<vmTranMode> postMode = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_Post_Mode";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            postMode = ConvertDataTableToGenericList.BindList<vmTranMode>(dt);
            return postMode;
        }



        public List<vmCityAllCompany> getUserWiseCompanyList(string loggeduser)
        {
            List<vmCityAllCompany> mCompanyList = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_User_Wise_Company";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("logeedUser", OracleDbType.Varchar2).Value = loggeduser;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            mCompanyList = ConvertDataTableToGenericList.BindList<vmCityAllCompany>(dt);
            return mCompanyList;
        }


        public string getUserRoleID(string loggeduser)
        {
            string vmUser = string.Empty;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.GETUSERROLEID";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("P_LOGEEDUSER", OracleDbType.Varchar2).Value = loggeduser;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            vmUser = dt.Rows[0]["ROLEID"].ToString();
            return vmUser;
        }


        public List<vmProduct> GetSKUByCompany(string selectedCompany)
        {
            List<vmProduct> productList = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_SKU_By_Company";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_selectedcompany", OracleDbType.Varchar2).Value = selectedCompany;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            productList = ConvertDataTableToGenericList.BindList<vmProduct>(dt);
            return productList;
        }

        public List<vmProduct> GetExportSKUByCompanyBrand(string companyId, string brandId)
        {
            List<vmProduct> productList = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_Export_SKU_By_Company_Brand";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_company_Id", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(companyId) ? null : companyId;
            objCmd.Parameters.Add("p_brand_Id", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(brandId) ? null : brandId;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            productList = ConvertDataTableToGenericList.BindList<vmProduct>(dt);
            return productList;
        }

        public List<vmBrandSKU> getExportBrand(string companyId)
        {
            List<vmBrandSKU> mBrandList = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_Export_Brand";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_company_Id", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(companyId) ? null : companyId;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            mBrandList = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return mBrandList;
        }

        [HttpPost]
        public IHttpActionResult GetUnitCompany(object[] data)
        {
            IEnumerable<vmUnitCompany> objUnitCompany = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objUnitCompany = objDDLService.GetUnitCompany(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objUnitCompany
            });
        }

        [HttpPost]
        public IHttpActionResult GetUserWiseProductCompany(object[] data)
        {
            DataTable prodComList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                prodComList = objDDLService.GetUserWiseProductCompany(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                prodComList
            });
        }

        [HttpPost]
        public IHttpActionResult GetUserWiseSSTYP(object[] data)
        {
            DataTable sstypList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                sstypList = objDDLService.GetUserWiseSSTYP(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                sstypList
            });
        }


        [HttpPost]
        public IHttpActionResult GetInventorySupplier(object[] data)
        {
            IEnumerable<vmInventorySupplies> objInventorySupplier = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objInventorySupplier = objDDLService.GetInventorySupplier(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objInventorySupplier
            });
        }

        [HttpPost]
        public IHttpActionResult GetItemGroup(object[] data)
        {
            IEnumerable<vmItemGroup> objItemGroup = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

            try
            {
                objItemGroup = objDDLService.GetItemGroup(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objItemGroup
            });
        }


        [HttpPost]
        public IHttpActionResult GetInventoryItem(object[] data)
        {
            IEnumerable<vmInventoryItem> objInventoryItem = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objInventoryItem = objDDLService.GetInventoryItem(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objInventoryItem
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult getusers(object[] data)
        {
            object usr = null;
            try
            {
                usr = objDDLService.GetAllUser();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                usr
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult getalluser_softv3(object[] data)
        {
            object usrV3 = null;
            try
            {
                usrV3 = objDDLService.GetAllUser_SoftV3();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                usrV3
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult getrole(object[] data)
        {
            object role = null;
            try
            {
                role = objDDLService.GetAllRole();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                role
            });
        }

        public List<vmBrandSKU> getCompanyWiseBrand(string companyId)
        {
            List<vmBrandSKU> mCompanyList = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PRODUCTION_DEMAND.BRAND_BY_COMPANY";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("productCompanyId", OracleDbType.Varchar2).Value = companyId;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            mCompanyList = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return mCompanyList;
        }

        [HttpPost]
        public IHttpActionResult GetAgentClientDDL(object[] data)
        {
            DataTable listAgent = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                listAgent = objDDLService.GetAgentClientDDL(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                listAgent
            });
        }

        [HttpPost]
        public IHttpActionResult GetAllSalesLine(object[] data)
        {
            //List<vmCitySalesLineList> mCompanyList = null;
            DataTable mCompanyList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

            try
            {
                mCompanyList = objDDLService.GetAllSalesLine(objcmnParam.FieldNameOne, objcmnParam.FieldNameTwo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                mCompanyList
            });
        }

        [HttpPost]
        public IHttpActionResult getUserMenuModePermission(object[] data)
        {
            //List<vmCitySalesLineList> mCompanyList = null;
            DataTable modePermission = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

            try
            {
                modePermission = objDDLService.GetUserMenuModePermission(objcmnParam.loggeduser, objcmnParam.menuId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                modePermission
            });
        }
    }
}
