using CTGroup.Models;
using CTGroup.Models.ViewModel.Sales;
//using CTGroup.Models.ViewModel.Production;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Service.SystemCommon.Interfaces
{
    public interface iSystemCommonDDL
    {
     
        List<vmLocation> GetLocationList(string userId);
        List<vmMComapnay> GetMasterCompany(string userId);
        object GetChildCompany(vmCmnParameters objcmnParam);
        object GetSubHead(vmCmnParameters objcmnParam); 
        object GetAcHead(vmCmnParameters objcmnParam); 
         List<vmLocation> GetGroupHead(string userId);
        //------------------------
        List<vmShowrooms> GetShowroomList(string userId); 
         List<vmProducts> GetProductList(string userId);
        List<vmProduct> GetRowMetarialsProductList();
        List<SSalesAreaHierarchy> GetSSalesAreaHierarchyList(string mode, string trackNo);
        List<SSalesAreaHierarchy> GetSalesAreaHierarchySecondaryList(string mode, string trackNo);

        List<SSalesAreaHierarchy> GetAreaWiseDistributor(string national, string division, string region, string zone);
        List<SSalesAreaHierarchy> GetAreaWiseDistributorPrimary(string national, string division, string region, string zone);
        List<SSalesAreaHierarchy> GetAreaWiseDistributorSecondary(string national, string division, string region, string zone);
        List<SSalesAreaHierarchy> GetAreaWiseSalesPerson(string national, string division, string region, string zone, string distributor);

        List<vmBrandSKU> GetSKUByBrand(string mode, string brandOID);
        List<vmBrandSKU> GetBulkSKUByBrand(string mode, string brandOID);
        List<vmBrandSKU> GetSalesSKUByBrand(string mode, string brandOID, string sstypId);
        List<vmBrandSKU> GetBrandByCategoryCC(string mode, string brandOID);

        List<vmBrandSKU> GetBrandByCategoryCC(string categoryid, string brndGroupId, string userId);

        List<vmBrandSKU> Get_PROD_CAT_BY_NTN(string mode, string brandOID);
        List<vmBrandSKU> GetSalesTeamBrand(string mode, string nationalOID);
        List<vmSalesReportPermission> GetPermissionForReport(string loggeduser, string userRole);
        IEnumerable<T_CMNMENU> GetParentMenuForDropDown(int? pageNumber, int? pageSize, int? IsPaging, int? ModuleID);
        List<ProductCategory> GetProductCategory();
        List<vmBrandSKU> GetBrandListByNational(string mode, string nationalOID);
        List<vmBrandSKU> GetBrandListByNational(string nationalOid, string brnGroupId, string userId);
        DataTable GetBrandGroupListByUser(string mode, string userId);
        //List<ProductCategory> Get_PROD_CAT_BY_NTN();
        List<ProductType> GetProductType();
        List<SecondaryType> GetSCON_TYP();
        List<ProductTypeSSTYP> GetProductTypeSSTYP();

        List<vmBrandSKU> GetBrandByCategory(string categoryId);
        IEnumerable<vmBrandSKU> GetBrand(vmCmnParameters objcmnParam);
        IEnumerable<vmBrandSKU> GetLocation(vmCmnParameters objcmnParam);
        object GetProductGroupRupshiFood(vmCmnParameters objcmnParam);
        object GetSalesGroupRupshiFood(vmCmnParameters objcmnParam);
        object GetAllProductByBrand(vmCmnParameters objcmnParam);
        object GetAllProductBySSTYP(vmCmnParameters objcmnParam);
        object GetAllSalesGroup(vmCmnParameters objcmnParam);
        object GetAllProductGroup(vmCmnParameters objcmnParam);
        object GetAllBrandGroup(vmCmnParameters objcmnParam);
        object GetAllBrand(vmCmnParameters objcmnParam);
        object GetAllFilteredLocation(vmCmnParameters objcmnParam);
        object GetAllNational(vmCmnParameters objcmnParam);
        IEnumerable<vmBrandSKU> GetFilteredLocation(vmCmnParameters objcmnParam);
        IEnumerable<vmBrandSKU> GetProductGroup(vmCmnParameters objcmnParam);

        IEnumerable<vmBrandSKU> GetSalesLocation(vmCmnParameters objcmnParam);
        IEnumerable<vmEmployee> GetCTGStaff(vmCmnParameters objcmnParam);
        List<SSalesAreaHierarchy> GetSalesAreaHierarchyByUser(vmCmnParameters objcmnParam);
        List<vmEmployee> GetUserForDropDownList(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmCmnModule> GetModuleWithPermission(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging);

        List<vmUnitCompany> GetUnitCompany(vmCmnParameters objcmnParam);
        DataTable GetUserWiseProductCompany(vmCmnParameters objcmnParam);
        DataTable GetUserWiseSSTYP(vmCmnParameters objcmnParam);
        List<vmInventorySupplies> GetInventorySupplier(vmCmnParameters objcmnParam);
        List<vmItemGroup> GetItemGroup(vmCmnParameters objcmnParam);
        List<vmInventoryItem> GetInventoryItem(vmCmnParameters objcmnParam);
        List<vmUserRole> GetAllUser();
        List<vmUserRole> GetAllRole();
        DataTable GetAgentClientDDL(vmCmnParameters objcmnParam);
        DataTable GetAllSalesLine(string callname, string sel1);
        DataTable GetUserMenuModePermission(string loggedUser, int menuId);

        object GetAllUser_SoftV3();
    }
}