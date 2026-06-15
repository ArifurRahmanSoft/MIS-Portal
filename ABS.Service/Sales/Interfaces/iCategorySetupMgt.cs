using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System.Collections.Generic;
using System.Data;

namespace CTGroup.Service.Sales.Interfaces
{
    public interface iCategorySetupMgt
    {
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
