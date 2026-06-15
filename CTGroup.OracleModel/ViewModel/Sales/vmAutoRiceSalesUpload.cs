using System;

namespace CTGroup.OracleModel.ViewModel.Sales
{
    public class vmAutoRiceSalesUpload
    {
        public string SALES_ORG { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public DateTime SALES_DATE { get; set; }
        public string SALES_ORDER { get; set; }
        public string MATERIAL_NAME { get; set; }
        public string MATERIAL_DESCRIPTION { get; set; }
        public double QNT_IN_BAG_CTN { get; set; }
        public double QNT_IN_KG_PCS { get; set; }
        public double QNT_IN_TON { get; set; }
        public double NET_WEIGHT { get; set; }
        public double SALES_VALUE { get; set; }
        public string SALES_TYPE { get; set; }

    }
}
