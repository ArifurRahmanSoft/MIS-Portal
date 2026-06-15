using CTGroup.Models;
using CTGroup.Models.ViewModel.Sales;
using CTGroup.Data.BaseFactories;
using CTGroup.Models.ViewModel.SystemCommon;
using System;
using CTGroup.OracleModel;

namespace CTGroup.Service.AllServiceClasses
{
    #region Sales Entities
    public class CmnUserUserAuthentication_EF : GenericFactory_EF<Entities, T_CMNUSERAUTHENTICATION> { }
    public class vmCmnMenu_GF : GenericFactory<Entities, vmCmnMenu> { }

    public class CmnModule_EF : GenericFactory_EF<Entities, T_CMNMODULE> { }

    public partial class vmCmnBrandPermission_GF : GenericFactory<Entities, vmCmnBrandPermission> { }
    public partial class vmCmnMenuPermission_GF : GenericFactory<Entities, vmCmnMenuPermission> { }
    public class CmnMenuPermission_EF : GenericFactory_EF<Entities, T_CMNMENUPERMISSION> { }

    #endregion



    #region db Sample Entities
    //public class tbl_Sales_EF : GenericFactory_EF<dbSampleEntities, tbl_Sales> { }
    #endregion

    #region Commented Entities
    //public class PIBuyerGFactory_EF : GenericFactory_EF<OracleModelEntity, T_CMNUSER> { }
    //public class PICompanyGFactory_EF : GenericFactory_EF<OracleModelEntity, CmnCompany> { }
    //public class ItemColorGFactory_EF : GenericFactory_EF<OracleModelEntity, CmnItemColor> { }
    //public class PISampleNoGFactory_EF : GenericFactory_EF<OracleModelEntity, CmnItemMaster> { }
    //public class PIComboGFactory_EF : GenericFactory_EF<OracleModelEntity, T_CMNCOMBO> { }
    //public class CompanyFactory_EF : GenericFactory_EF<OracleModelEntity, CmnCompany> { }
    //public class BuyerFactory_EF : GenericFactory_EF<OracleModelEntity, T_CMNUSER> { }
    //public class BankFactory_EF : GenericFactory_EF<OracleModelEntity, CmnBank> { }
    //public class BankBranchFactory_EF : GenericFactory_EF<OracleModelEntity, CmnBankBranch> { }
    //public class MenuPermissionMgtGFactory1 : GenericFactory<OracleModelEntity, vmCmnMenuPermission> { }
    //public class CompanyTemp : GenericFactory_EF<OracleModelEntity, CmnCompany> { }
    //public class ModuleDDL_GF : GenericFactory<OracleModelEntity, vmCmnModule> { }
    //public class Company : GenericFactory_EF<OracleModelEntity, CmnCompany> { }
    //public class ModulePMgtGFactory_EF : GenericFactory_EF<OracleModelEntity, T_CMNMODULE> { }
    //public class CompanyDDL_EF : GenericFactory_EF<OracleModelEntity, CmnCompany> { }
    //public class Organogram_GF : GenericFactory<OracleModelEntity, vmCmnOrganogram> { }
    //public class CmnUOMFactory_EF : GenericFactory_EF<OracleModelEntity, CmnUOM> { }
    //public class CmnColorFactory_EF : GenericFactory_EF<OracleModelEntity, CmnItemColor> { }
    //public class CmnSizeFactory_EF : GenericFactory_EF<OracleModelEntity, CmnItemSize> { }
    //public class ItemGroupFF_ddl : GenericFactory_EF<OracleModelEntity, CmnItemGroup> { }
    //public partial class CmnItemMaster_EF : GenericFactory_EF<OracleModelEntity, CmnItemMaster> { }
    #endregion
}
