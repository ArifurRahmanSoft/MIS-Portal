using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Service.SystemCommon.Interfaces;
using System.Collections.Generic;
using System.Linq;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using CTGroup.Utility.Common;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.OracleModel;
using System.Configuration;
using CTGroup.Models.ViewModel.Sales;

//using CTGroup.Models.ViewModel.Production;

namespace CTGroup.Service.SystemCommon.Factories
{
    public class SystemCommonDDL : iSystemCommonDDL
    {



        //-----------------------------------start-------------------------------------------- 

        public List<vmShowrooms> GetShowroomList(string userId)
        {
            List<vmShowrooms> showromLst = new List<vmShowrooms>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "p_uloc.select_uloc";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("Call_Name", OracleDbType.Varchar2).Value = "SHOWROM_LOC";
            objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = userId;
            objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataCityn(objCmd);


            showromLst = ConvertDataTableToGenericList.BindList<vmShowrooms>(dt);
            return showromLst;
        }



        public List<vmProducts> GetProductList(string userId)
        {
            List<vmProducts> ProductLst = new List<vmProducts>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "p_sprod.select_sprod";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("call_name", OracleDbType.Varchar2).Value = "SHOWROOM_PRODUCT";
            objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataCityn(objCmd);

            ProductLst = ConvertDataTableToGenericList.BindList<vmProducts>(dt);
            return ProductLst;
        }





        //------------------------------------------End-------------------------------------------

        //-------------------------------------------Traial Balance Start-------------------------------
  
        public List<vmLocation> GetLocationList(string userId)
        {
            List<vmLocation> locationLst = new List<vmLocation>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.select_agloc";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("Call_Name", OracleDbType.Varchar2).Value = "COMBO";
            objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = userId;
            objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);


            locationLst = ConvertDataTableToGenericList.BindList<vmLocation>(dt);
            return locationLst;
        }

        public List<vmMComapnay> GetMasterCompany(string userId)
        {
            List<vmMComapnay> mstrComapnyLst = new List<vmMComapnay>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.select_mcom";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("Call_Name", OracleDbType.Varchar2).Value = "COMBO";
            objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = userId;
            objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);


            mstrComapnyLst = ConvertDataTableToGenericList.BindList<vmMComapnay>(dt);
            return mstrComapnyLst;
        }



        public object GetChildCompany(vmCmnParameters objcmnParam)
        {
            DataTable objlistCcompany = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.select_ucom";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("Call_Name", OracleDbType.Varchar2).Value = "COMBO";
            objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = objcmnParam.strId;
            objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;
            objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            objlistCcompany = classDt.GetData(objCmd);
            return objlistCcompany;
        }


        public object GetSubHead(vmCmnParameters objcmnParam)
        {
            DataTable objlistSubHead = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.select_cshd";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("Call_Name", OracleDbType.Varchar2).Value = "COMBO";
            objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = objcmnParam.strId;
            objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            objlistSubHead = classDt.GetData(objCmd);
            return objlistSubHead;
        }



        public List<vmLocation> GetGroupHead(string userId)
        {
            List<vmLocation> grpHead = new List<vmLocation>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.select_rptm";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("Call_Name", OracleDbType.Varchar2).Value = "ACGP";
            objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);


            grpHead = ConvertDataTableToGenericList.BindList<vmLocation>(dt);
            return grpHead;
        }


        public object GetAcHead(vmCmnParameters objcmnParam)
        {
            DataTable objlistAcHead = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.select_rptm";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("Call_Name", OracleDbType.Varchar2).Value = "ACCDTEXT";
            objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = objcmnParam.strId;
            objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = objcmnParam.strId2;
            objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = objcmnParam.strId3;
            objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            objlistAcHead = classDt.GetData(objCmd);
            return objlistAcHead;
        }











        //-------------------------------------------Traial Balance End-------------------------------

        public List<vmProduct> GetRowMetarialsProductList()
        {
            List<vmProduct> sSalesAreas = new List<vmProduct>();
            OracleCommand objCmd = new OracleCommand();
            //objCmd.CommandText = "PKG_PRODUCT_DETAILS_PER_LC.Get_ROW_METARIALS_PRODUCT_LIST";
            objCmd.CommandText = "PKG_PRODUCT_DETAILS_PER_LC.GET_PRODUCT_LIST";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmProduct>(dt);
            return sSalesAreas;
        }
        public List<vmBrandSKU> GetBrandListByNational(string mode, string nationalOID)
        {
            List<vmBrandSKU> sSalesAreas = new List<vmBrandSKU>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.GET_BRAND_LIST_BY_NATIONAL";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_nationalOID", OracleDbType.Varchar2).Value = nationalOID == "null" ? null : nationalOID;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return sSalesAreas;
        }

        public List<vmBrandSKU> GetBrandListByNational(string nationalOid, string brndGroupId, string userId)
        {
            List<vmBrandSKU> sSalesAreas = new List<vmBrandSKU>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.GET_BRAND_LIST_BY_NATIONAL_BRND_GROUP";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_nationalOID", OracleDbType.Varchar2).Value = nationalOid == "null" ? null : nationalOid;
            objCmd.Parameters.Add("p_brndGroupId", OracleDbType.Varchar2).Value = brndGroupId == "null" ? null : brndGroupId;
            objCmd.Parameters.Add("p_userId", OracleDbType.Varchar2).Value = userId == "null" ? null : userId;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return sSalesAreas;
        }

        public DataTable GetBrandGroupListByUser(string mode, string loggedUserId)
        {
            DataTable dtBrandGroupList = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.GET_BRAND_GROUP_LIST_BY_USER";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_loggedUserId", OracleDbType.Varchar2).Value = loggedUserId == "null" ? null : loggedUserId;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            dtBrandGroupList = classDt.GetDataBasic(objCmd);
            return dtBrandGroupList;
        }

        public List<vmSalesReportPermission> GetPermissionForReport(string loggeduser, string userRoleId)
        {
            List<vmSalesReportPermission> permission = new List<vmSalesReportPermission>();

            OracleCommand objCmd = new OracleCommand();

            objCmd.CommandText = "REPORTS.Get_Permission_For_Report";

            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_loggeduser", OracleDbType.Varchar2).Value = loggeduser;
            objCmd.Parameters.Add("p_userroleid", OracleDbType.Varchar2).Value = userRoleId;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

            DataTable dt = classDt.GetDataBasic(objCmd);
            permission = ConvertDataTableToGenericList.BindList<vmSalesReportPermission>(dt);

            return permission;
        }

        public List<SSalesAreaHierarchy> GetSSalesAreaHierarchyList(string mode, string trackNo)
        {
            if (mode == "1")
            {
                trackNo = null;
            }
            List<SSalesAreaHierarchy> sSalesAreas = new List<SSalesAreaHierarchy>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_Sales_Area_Hierarchy";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = mode;
            objCmd.Parameters.Add("p_track_no", OracleDbType.Varchar2).Value = trackNo;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<SSalesAreaHierarchy>(dt);
            return sSalesAreas;
        }

        public List<SSalesAreaHierarchy> GetSalesAreaHierarchyByUser(vmCmnParameters objcmnParam)
        {
            List<SSalesAreaHierarchy> sSalesAreas = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_SALES_HIERARCHY.Get_Sales_Area_Hierarchy_By_User";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_user", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;
            objCmd.Parameters.Add("p_saleshierarchy", OracleDbType.Varchar2).Value = objcmnParam.saleshierarchy;
            objCmd.Parameters.Add("p_area_id", OracleDbType.Varchar2).Value = "";
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<SSalesAreaHierarchy>(dt);
            return sSalesAreas;
        }
        public List<SSalesAreaHierarchy> GetSalesAreaHierarchySecondaryList(string mode, string trackNo)
        {
            if (mode == "1")
            {
                trackNo = null;
            }
            List<SSalesAreaHierarchy> sSalesAreas = new List<SSalesAreaHierarchy>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_Sales_Area_Hierarchy";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = mode;
            objCmd.Parameters.Add("p_track_no", OracleDbType.Varchar2).Value = trackNo;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetSecondaryBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<SSalesAreaHierarchy>(dt);
            return sSalesAreas;
        }

        public List<SSalesAreaHierarchy> GetAreaWiseDistributor(string national, string division, string region, string zone)
        {
            if (national == "0")
                national = null;

            if (division == "0")
                division = null;

            if (region == "0")
                region = null;

            if (zone == "0")
                zone = null;

            List<SSalesAreaHierarchy> sSalesAreas = new List<SSalesAreaHierarchy>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_AreaWise_Distributor_info";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = national;
            objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = division;
            objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = region;
            objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zone;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<SSalesAreaHierarchy>(dt);
            return sSalesAreas;
        }

        public List<SSalesAreaHierarchy> GetAreaWiseDistributorPrimary(string national, string division, string region, string zone)
        {
            if (national == "0")
                national = null;

            if (division == "0")
                division = null;

            if (region == "0")
                region = null;

            if (zone == "0")
                zone = null;

            List<SSalesAreaHierarchy> sSalesAreas = new List<SSalesAreaHierarchy>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_AreaWise_Primary_Distributor";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = national;
            objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = division;
            objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = region;
            objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zone;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<SSalesAreaHierarchy>(dt);
            return sSalesAreas;
        }

        public List<SSalesAreaHierarchy> GetAreaWiseDistributorSecondary(string national, string division, string region, string zone)
        {
            if (national == "0")
                national = null;

            if (division == "0")
                division = null;

            if (region == "0")
                region = null;

            if (zone == "0")
                zone = null;

            List<SSalesAreaHierarchy> sSalesAreas = new List<SSalesAreaHierarchy>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_AreaWise_Distributor_info";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = national;
            objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = division;
            objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = region;
            objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zone;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetSecondaryBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<SSalesAreaHierarchy>(dt);
            return sSalesAreas;
        }

        public List<SSalesAreaHierarchy> GetAreaWiseSalesPerson(string national, string division, string region, string zone, string distributor)
        {
            if (national == "0")
                national = null;

            if (division == "0")
                division = null;

            if (region == "0")
                region = null;

            if (zone == "0")
                zone = null;

            if (distributor == "0")
                distributor = null;

            List<SSalesAreaHierarchy> sSalesAreas = new List<SSalesAreaHierarchy>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_AreaWise_Sales_Person";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = national;
            objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = division;
            objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = region;
            objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zone;
            objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributor;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetSecondaryBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<SSalesAreaHierarchy>(dt);
            return sSalesAreas;
        }

        public List<vmBrandSKU> GetSKUByBrand(string mode, string brandOID)
        {
            List<vmBrandSKU> sSalesAreas = new List<vmBrandSKU>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "TestSKUMapping.Get_SKUByBrand";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_brandOID", OracleDbType.Varchar2).Value = brandOID;
            objCmd.Parameters.Add("p_nationalOID", OracleDbType.Varchar2).Value = mode;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return sSalesAreas;
        }

       


        public List<vmBrandSKU> GetBulkSKUByBrand(string mode, string brandOID)
        {
            List<vmBrandSKU> sSalesAreas = new List<vmBrandSKU>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_BulkSKUByBrand";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_brandOID", OracleDbType.Varchar2).Value = brandOID;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return sSalesAreas;
        }

        public List<vmBrandSKU> GetSalesSKUByBrand(string mode, string brandOID, string sstypId)
        {
            List<vmBrandSKU> sSalesAreas = new List<vmBrandSKU>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_SalesSKUByBrand";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_brandOID", OracleDbType.Varchar2).Value = brandOID;
            objCmd.Parameters.Add("p_sstypId", OracleDbType.Varchar2).Value = sstypId;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return sSalesAreas;
        }

        public List<vmBrandSKU> GetBrandByCategoryCC(string mode, string brandOID)
        {
            List<vmBrandSKU> sSalesAreas = new List<vmBrandSKU>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_BrandByPCAT";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_brandOID", OracleDbType.Varchar2).Value = brandOID;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return sSalesAreas;
        }

        public List<vmBrandSKU> GetBrandByCategoryCC(string categoryId, string brndGroupId, string userId)
        {
            List<vmBrandSKU> sSalesAreas = new List<vmBrandSKU>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_BrandByCategoryGroup";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_categoryId", OracleDbType.Varchar2).Value = categoryId == "null" ? null : categoryId;
            objCmd.Parameters.Add("p_brndGroupId", OracleDbType.Varchar2).Value = brndGroupId == "null" ? null : brndGroupId;
            objCmd.Parameters.Add("p_userId", OracleDbType.Varchar2).Value = userId == "null" ? null : userId;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return sSalesAreas;
        }

        public List<vmBrandSKU> Get_PROD_CAT_BY_NTN(string mode, string brandOID)
        {
            List<vmBrandSKU> sSalesAreas = new List<vmBrandSKU>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_PROD_CAT_BY_NTN";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_nationalID", OracleDbType.Varchar2).Value = brandOID;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return sSalesAreas;
        }

        public List<vmBrandSKU> GetSalesTeamBrand(string mode, string nationalOID)
        {
            List<vmBrandSKU> brandList = new List<vmBrandSKU>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "TestSKUMapping.Get_SalesTeamBrand";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("P_NATIONALOID", OracleDbType.Varchar2).Value = nationalOID;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            brandList = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return brandList;
        }

        public IEnumerable<vmEmployee> GetCTGStaff(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmEmployee> objCTGStaff = null;

            OracleCommand objCmd = new OracleCommand();

            objCmd.CommandText = " SELECT AEMP_TEXT STAFFID, CONCAT( CONCAT( AEMP_TEXT, ' - ' ), AEMP_ENAM) STAFFNAME FROM T_AEMP@HR_EMP_TABLE WHERE AEMP_ACTV = 'Y' ";

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            objCTGStaff = ConvertDataTableToGenericList.BindList<vmEmployee>(dt);
            return objCTGStaff;
        }

        public List<ProductCategory> GetProductCategory()
        {
            List<ProductCategory> objProductCategory = new List<ProductCategory>();


            OracleCommand objCmd = new OracleCommand();

            objCmd.CommandText = "PKG_BASIC_INFO.Get_ProductCategory";

            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

            DataTable dt = classDt.GetDataBasic(objCmd);
            objProductCategory = ConvertDataTableToGenericList.BindList<ProductCategory>(dt);


            return objProductCategory;
        }

        //public List<ProductCategory> Get_PROD_CAT_BY_NTN()
        //{
        //    List<ProductCategory> objProductCategory = new List<ProductCategory>();


        //    OracleCommand objCmd = new OracleCommand();

        //    objCmd.CommandText = "PKG_BASIC_INFO.Get_PROD_CAT_BY_NTN";

        //    objCmd.CommandType = CommandType.StoredProcedure;

        //    objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        //    ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

        //    DataTable dt = classDt.GetDataBasic(objCmd);
        //    objProductCategory = ConvertDataTableToGenericList.BindList<ProductCategory>(dt);


        //    return objProductCategory;
        //}

        public List<ProductType> GetProductType()
        {
            List<ProductType> objProductType = new List<ProductType>();

            OracleCommand objCmd = new OracleCommand();

            objCmd.CommandText = "PKG_BASIC_INFO.Get_ProductType";

            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

            DataTable dt = classDt.GetDataBasic(objCmd);
            objProductType = ConvertDataTableToGenericList.BindList<ProductType>(dt);


            return objProductType;
        }
        public List<SecondaryType> GetSCON_TYP()
        {
            List<SecondaryType> objProductType = new List<SecondaryType>();

            OracleCommand objCmd = new OracleCommand();

            objCmd.CommandText = "PKG_BASIC_INFO.Get_SCON_TYP";

            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

            DataTable dt = classDt.GetDataBasic(objCmd);
            objProductType = ConvertDataTableToGenericList.BindList<SecondaryType>(dt);


            return objProductType;
        }
        public List<ProductTypeSSTYP> GetProductTypeSSTYP()
        {
            List<ProductTypeSSTYP> objProductType = new List<ProductTypeSSTYP>();

            OracleCommand objCmd = new OracleCommand();

            objCmd.CommandText = "PKG_BASIC_INFO.Get_ProductTypeSSTYP";

            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

            DataTable dt = classDt.GetDataBasic(objCmd);
            objProductType = ConvertDataTableToGenericList.BindList<ProductTypeSSTYP>(dt);


            return objProductType;
        }



        public List<vmBrandSKU> GetBrandByCategory(string categoryId)
        {
            List<vmBrandSKU> objProductCategory = new List<vmBrandSKU>();


            OracleCommand objCmd = new OracleCommand();

            objCmd.CommandText = "PKG_BASIC_INFO.Get_BrandByCategory";

            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("P_CategoryId", OracleDbType.Varchar2).Value = categoryId;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

            DataTable dt = classDt.GetDataBasic(objCmd);
            objProductCategory = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);


            return objProductCategory;
        }
        public IEnumerable<vmBrandSKU> GetLocation(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmBrandSKU> objLocation = null;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            //string query = "SELECT OID AS SGLOC_ID, SGLOC_NAME FROM CITYN.T_SGLOC ORDER BY SGLOC_NAME";

            string query = "SELECT LOC.OID AS SGLOC_ID, LOC.SGLOC_NAME FROM CITYN.T_SGLOC LOC " +
                "INNER JOIN CITYN.T_ULOC UL ON LOC.SGLOC_TEXT = UL.ULOC_SGLOC " +
                "WHERE  UL.ULOC_USER = '" + objcmnParam.loggeduser + "' AND UL.ULOC_PRPT = 1 " +
                "ORDER BY SGLOC_NAME";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objLocation = t1.AsEnumerable().Select(dataRow => new vmBrandSKU
            {
                SGLOC_ID = dataRow.Field<string>("SGLOC_ID"),
                SGLOC_NAME = dataRow.Field<string>("SGLOC_NAME")
            }).ToList();

            return objLocation;
        }

        public object GetProductGroupRupshiFood(vmCmnParameters objcmnParam)
        {
            DataTable objlistProdGroup = new DataTable();

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            //string query = "SELECT OID AS SGLOC_ID, SGLOC_NAME FROM CITYN.T_SGLOC ORDER BY SGLOC_NAME";

            string query = "select oid SPROG_ID, sprog_name,  " +
                "CASE WHEN oid='SPROGxxxxxxxxxxxxx85' THEN 'SDVNTxxxxxxxxxxx1008' " +
                "WHEN oid = 'SPROGxxxxxxxxxxxxx86' THEN 'SDVNTxxxxxxxxxxx1009' " +
                "WHEN oid = 'SPROGxxxxxxxxxxxxx87' THEN 'SDVNTxxxxxxxxxxx1010' " +
                "ELSE '' END SDVNT_ID from cityn.t_sprog where oid in ('SPROGxxxxxxxxxxxxx85','SPROGxxxxxxxxxxxxx86','SPROGxxxxxxxxxxxxx87')";

            OracleCommand cmd = new OracleCommand(query, con);

            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(objlistProdGroup);
            }
            con.Close();

            return objlistProdGroup;
        }

        public object GetSalesGroupRupshiFoods(vmCmnParameters objcmnParam)
        {
            DataTable objlistSalesGroup = new DataTable();

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            //string query = "SELECT OID AS SGLOC_ID, SGLOC_NAME FROM CITYN.T_SGLOC ORDER BY SGLOC_NAME";

            string query = "SELECT OID SDVNT_ID, SDVNT_NAME || '('||SDVNT_BRND||')' AS SDVNT_NAME FROM CITYN.T_SDVNT WHERE OID IN ('SDVNTxxxxxxxxxxx1008','SDVNTxxxxxxxxxxx1009','SDVNTxxxxxxxxxxx1010') ORDER BY SDVNT_NAME";

            OracleCommand cmd = new OracleCommand(query, con);

            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(objlistSalesGroup);
            }
            con.Close();

            return objlistSalesGroup;
        }

        public object GetSalesGroupRupshiFood(vmCmnParameters objcmnParam)
        {
            DataTable objlistSalesGroup = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_Sales_Group_By_User";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_userId", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            objlistSalesGroup = classDt.GetDataBasic(objCmd);
            //objProductGroup = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return objlistSalesGroup;
        }

        public object GetAllNational(vmCmnParameters objcmnParam)
        {
            DataTable listNational = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_National";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = objcmnParam.strId;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            listNational = classDt.GetDataBasic(objCmd);
            return listNational;
        }

        public object GetAllProductByBrand(vmCmnParameters objcmnParam)
        {
            DataTable listProdList = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_Product_BY_Brand";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_brand_OID", OracleDbType.Varchar2).Value = objcmnParam.strId;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            listProdList = classDt.GetDataBasic(objCmd);
            //objProductGroup = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return listProdList;
        }

        public object GetAllProductBySSTYP(vmCmnParameters objcmnParam)
        {
            DataTable listProdList = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_Product_BY_SSTYP";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_SComp", OracleDbType.Varchar2).Value = objcmnParam.strId;
            objCmd.Parameters.Add("p_SSType", OracleDbType.Varchar2).Value = objcmnParam.strId2;
            objCmd.Parameters.Add("p_SDVNT", OracleDbType.Varchar2).Value = objcmnParam.strId3;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            listProdList = classDt.GetDataBasic(objCmd);
            //objProductGroup = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return listProdList;
        }

        public object GetAllSalesGroup(vmCmnParameters objcmnParam)
        {
            DataTable objlistSalesGroup = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_Sales_Group";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            objlistSalesGroup = classDt.GetDataBasic(objCmd);
            //objProductGroup = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return objlistSalesGroup;
        }

        public object GetAllProductGroup(vmCmnParameters objcmnParam)
        {
            DataTable objlistSalesGroup = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_Product_Group";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            objlistSalesGroup = classDt.GetDataBasic(objCmd);
            //objProductGroup = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return objlistSalesGroup;
        }

        public object GetAllBrandGroup(vmCmnParameters objcmnParam)
        {
            DataTable objlistSalesGroup = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_Brand_Group";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            objlistSalesGroup = classDt.GetDataBasic(objCmd);
            //objProductGroup = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return objlistSalesGroup;
        }

        public object GetAllBrand(vmCmnParameters objcmnParam)
        {
            DataTable brandList = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_Brand";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            brandList = classDt.GetDataBasic(objCmd);
            //objProductGroup = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return brandList;
        }

        public object GetAllFilteredLocation(vmCmnParameters objcmnParam)
        {
            DataTable locationList = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_Filtered_Location";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_userId", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;
            objCmd.Parameters.Add("loc_ids", OracleDbType.Varchar2).Value = objcmnParam.InputString;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            locationList = classDt.GetDataBasic(objCmd);
            return locationList;
        }

        public IEnumerable<vmBrandSKU> GetFilteredLocation(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmBrandSKU> objLocation = null;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            //string query = "SELECT OID AS SGLOC_ID, SGLOC_NAME FROM CITYN.T_SGLOC ORDER BY SGLOC_NAME";

            string query = "SELECT LOC.OID AS SGLOC_ID, LOC.SGLOC_NAME FROM CITYN.T_SGLOC LOC " +
                "INNER JOIN CITYN.T_ULOC UL ON LOC.SGLOC_TEXT = UL.ULOC_SGLOC " +
                "WHERE LOC.SGLOC_SPNT='1' AND UL.ULOC_USER = '" + objcmnParam.loggeduser + "' AND UL.ULOC_PRPT = 1 " +
                "ORDER BY SGLOC_NAME";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objLocation = t1.AsEnumerable().Select(dataRow => new vmBrandSKU
            {
                SGLOC_ID = dataRow.Field<string>("SGLOC_ID"),
                SGLOC_NAME = dataRow.Field<string>("SGLOC_NAME")
            }).ToList();

            return objLocation;
        }

        public IEnumerable<vmBrandSKU> GetProductGroup(vmCmnParameters objcmnParam)
        {
            //IEnumerable<vmBrandSKU> objProductGroup = null;

            //OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            //con.Open();
            //string query = "SELECT OID PROD_GROUP_OID, SPROG_NAME PROD_GROUP_NAME FROM CITYN.T_SPROG WHERE SPROG_ACTV = '1' ORDER BY SPROG_NAME";

            //OracleCommand cmd = new OracleCommand(query, con);

            //DataTable t1 = new DataTable();
            //using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            //{
            //    a.Fill(t1);
            //}
            //con.Close();

            //objProductGroup = t1.AsEnumerable().Select(dataRow => new vmBrandSKU
            //{
            //    PROD_GROUP_OID = dataRow.Field<string>("PROD_GROUP_OID"),
            //    PROD_GROUP_NAME = dataRow.Field<string>("PROD_GROUP_NAME")
            //}).ToList();

            //return objProductGroup;




            List<vmBrandSKU> objProductGroup = new List<vmBrandSKU>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_PROD_GROUP_BY_NTNAL";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_nationalID", OracleDbType.Varchar2).Value = objcmnParam.nationalId;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            objProductGroup = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return objProductGroup;
        }

        public IEnumerable<vmBrandSKU> GetSalesLocation(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmBrandSKU> objLocation = null;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT OID AS SGLOC_ID, SGLOC_NAME FROM CITYN.T_SGLOC where oid in " +
                           "('SGLOCxxxxxxxxxxxxx03', 'SGLOCxxxxxxxxxxxxx02', 'SGLOCxxxxxxxxxxxxx06', 'SGLOCxxxxxxxxxxxxx05') " +
                           "ORDER BY SGLOC_NAME";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objLocation = t1.AsEnumerable().Select(dataRow => new vmBrandSKU
            {
                SGLOC_ID = dataRow.Field<string>("SGLOC_ID"),
                SGLOC_NAME = dataRow.Field<string>("SGLOC_NAME")
            }).ToList();

            return objLocation;
        }



        public IEnumerable<vmBrandSKU> GetBrand(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmBrandSKU> objBrand = null;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT OID AS BRANDID, SBRND_NAME FROM CITYN.T_SBRND";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objBrand = t1.AsEnumerable().Select(dataRow => new vmBrandSKU
            {
                BRANDID = dataRow.Field<string>("BRANDID"),
                SBRND_NAME = dataRow.Field<string>("SBRND_NAME")
            }).ToList();

            return objBrand;
        }

        public List<vmEmployee> GetUserForDropDownList(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmEmployee> objUserList = null;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "SETTINGS.Get_ApplicationUser";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            objUserList = ConvertDataTableToGenericList.BindList<vmEmployee>(dt);

            return objUserList.ToList().OrderBy(x => x.UserID).ToList();
        }

        public IEnumerable<vmCmnModule> GetModuleWithPermission(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmCmnModule> listModule = new List<vmCmnModule>();

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "SETTINGS.Get_Module";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            listModule = ConvertDataTableToGenericList.BindList<vmCmnModule>(dt);

            return listModule.ToList().OrderBy(x => x.MODULEID).ToList();
        }

        public IEnumerable<T_CMNMENU> GetParentMenuForDropDown(int? pageNumber, int? pageSize, int? IsPaging, int? ModuleID)
        {
            List<T_CMNMENU> listMenues = new List<T_CMNMENU>();
            return listMenues.OrderBy(x => x.MENUNAME).ToList();
        }


        public List<vmUnitCompany> GetUnitCompany(vmCmnParameters objcmnParam)
        {
            List<vmUnitCompany> objUnitCompany = null;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbSCM"].ConnectionString);

            con.Open();
            string query = "SELECT CAST(CUNIT_ID AS Varchar2(10)) CUNIT_ID, CUNIT_NAME  FROM T_CON_UNITS";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objUnitCompany = t1.AsEnumerable().Select(dataRow => new vmUnitCompany
            {
                CUNIT_ID = dataRow.Field<string>("CUNIT_ID"),
                CUNIT_NAME = dataRow.Field<string>("CUNIT_NAME")
            }).ToList();

            return objUnitCompany;
        }

        public DataTable GetUserWiseProductCompany(vmCmnParameters objcmnParam)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT DISTINCT COM.OID SCOMP_OID" +
                            ", COM.SCOMP_NAME " +
                            " FROM CITYN.T_SCOMP COM " +
                            " INNER JOIN CITYN.T_SDOGP GP ON COM.OID = GP.SDOGP_PKCOM " +
                            " LEFT JOIN T_USER_WISE_PRODUCT_COMPANY PCOM ON CASE WHEN PCOM.COMP_ID = '0' THEN 1 " +
                            " WHEN COM.OID = PCOM.COMP_ID THEN 1 ELSE 0 END = 1 " +
                            " WHERE PCOM.USER_ID='" + objcmnParam.loggeduser + "'" +
                            " ORDER BY COM.SCOMP_NAME";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable prodComList = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(prodComList);
            }
            con.Close();
            return prodComList;
        }

        public DataTable GetUserWiseSSTYP(vmCmnParameters objcmnParam)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT DISTINCT STYP.OID SSTYP_OID" +
                            ", STYP.SSTYP_NAME " +
                            " FROM CITYN.T_SSTYP STYP " +
                            " LEFT JOIN T_USER_WISE_SSTYP USTYP ON CASE WHEN USTYP.SSTYP_ID='0' THEN 1 " +
                            " WHEN STYP.OID=USTYP.SSTYP_ID THEN 1 ELSE 0 END=1 " +
                            " WHERE STYP.SSTYP_ACTV='1' AND USTYP.USER_ID='" + objcmnParam.loggeduser + "'" +
                            " ORDER BY STYP.SSTYP_NAME";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable sstypList = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(sstypList);
            }
            con.Close();
            return sstypList;
        }


        public List<vmInventorySupplies> GetInventorySupplier(vmCmnParameters objcmnParam)
        {
            List<vmInventorySupplies> objInventorySupplier = null;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbSCM"].ConnectionString);

            con.Open();
            string query = "SELECT SUPP_ID, SUPP_NAME  FROM T_SUPPLIERS";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objInventorySupplier = t1.AsEnumerable().Select(dataRow => new vmInventorySupplies
            {
                SUPP_ID = dataRow.Field<string>("SUPP_ID"),
                SUPP_NAME = dataRow.Field<string>("SUPP_NAME")
            }).ToList();

            return objInventorySupplier;
        }

        public List<vmItemGroup> GetItemGroup(vmCmnParameters objcmnParam)
        {
            List<vmItemGroup> objItemGroup = null;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbSCM"].ConnectionString);

            con.Open();
            string query = "SELECT CAST(ITMGRP_ID AS Varchar2(10)) ITMGRP_ID, GRP_NAME  FROM T_ITMGRP";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objItemGroup = t1.AsEnumerable().Select(dataRow => new vmItemGroup
            {
                ITMGRP_ID = dataRow.Field<string>("ITMGRP_ID"),
                GRP_NAME = dataRow.Field<string>("GRP_NAME")
            }).ToList();

            return objItemGroup;
        }

        public List<vmInventoryItem> GetInventoryItem(vmCmnParameters cparam)
        {
            List<vmInventoryItem> objInventoryItem = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PURCHASEITEM.Get_Item_Master";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = cparam.pageNumber;
            objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = cparam.pageSize;
            objCmd.Parameters.Add("SearchParam", OracleDbType.Varchar2).Value = cparam.searchItemName;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetSCMData(objCmd);
            objInventoryItem = ConvertDataTableToGenericList.BindList<vmInventoryItem>(dt);

            return objInventoryItem;
        }

        public List<vmUserRole> GetAllUser()
        {
            List<vmUserRole> objUserList = null;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "USERROLEMENUPERMISSION.Get_User";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            objUserList = ConvertDataTableToGenericList.BindList<vmUserRole>(dt);

            return objUserList.ToList().OrderBy(x => x.USERID).ToList();
        }

        public List<vmUserRole> GetAllRole()
        {
            List<vmUserRole> objUserList = null;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "USERROLEMENUPERMISSION.Get_Role";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            objUserList = ConvertDataTableToGenericList.BindList<vmUserRole>(dt);

            return objUserList.ToList().OrderBy(x => x.USERID).ToList();
        }

        public DataTable GetAgentClientDDL(vmCmnParameters objcmnParam)
        {
            OracleCommand objCmd = new OracleCommand();

            objCmd.CommandText = "SELECT OID, CLIENT_TEXT||'-'||CLIENT_NAME CLIENT_NAME, CLIENT_TYPE FROM TV_CLIENT";

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataEkhon(objCmd);
            //objCompany = ConvertDataTableToGenericList.BindList<vmEkhonTv>(dt);
            return dt;
        }

        public DataTable GetAllSalesLine(string callname, string sel1)
        {
            //List<vmCitySalesLineList> mCompanyList = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "p_sales_dropdown.select_sales_dropdown";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("t_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("call_name", OracleDbType.Varchar2).Value = callname;
            objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = sel1;
            objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
            objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            //mCompanyList = ConvertDataTableToGenericList.BindList<vmCitySalesLineList>(dt);
            return dt;
        }

        public DataTable GetUserMenuModePermission(string loggedUser, int menuId)
        {
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "p_sales_dropdown.Get_UserMenuModePermission";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("t_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("User_Id", OracleDbType.Varchar2).Value = loggedUser;
            objCmd.Parameters.Add("Menu_Id", OracleDbType.Decimal).Value = menuId;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(objCmd);
            return dt;
        }

        public object GetAllUser_SoftV3()
        {
            DataTable users = new DataTable();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_User_SoftV3";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            users = classDt.GetDataBasic(objCmd);
            //objProductGroup = ConvertDataTableToGenericList.BindList<vmBrandSKU>(dt);
            return users;
        }



        







    }
}