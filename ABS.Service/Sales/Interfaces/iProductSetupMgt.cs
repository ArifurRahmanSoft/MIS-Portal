using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System.Collections.Generic;
using System.Data;

namespace CTGroup.Service.Sales.Interfaces
{
    public interface iProductSetupMgt
    {
     /*   string SaveUpdateProductRate(vmProductRate itemMaster, List<vmProductRate> itemDetails, vmCmnParameters objcmnParam);
        IEnumerable<vmIncentiveFormulaSetupMaster> GetIncentiveFormulaSetupMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmBrandSKU> GetBrand(vmCmnParameters objcmnParam);
        IEnumerable<vmDistributor> GetSingleDistributor(vmCmnParameters objcmnParam);*/

        //IEnumerable<object> GetProductDetail(vmCmnParameters productDetailList);

        string SaveUpdateProductDetails(string  jsonDataMstr, string userId);
        string DeleteProduct(string ProductId, string userId);
        
        string SaveUpdateLitem(string jsonDataMstr, string userId);
        object GetProductList(vmCmnParameters objcmnParam);
        object GetProductDetail(string parameter);
        object GetProductDetailById(string parameter);
        object GetLitemDetail(string parameter);
        object GetProductListByPage();


        object GetProductType();
        object GetSconType();
        object GetSpcat();
        object GetProductSerial();
        object GetProductSize();
        object GetProductGroup();
        object GetDoGroup();
        object GetSBrand();
        object GetSmunt();

    }
}
