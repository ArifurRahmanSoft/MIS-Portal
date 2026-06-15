using System;

namespace CTGroup.OracleModel.ViewModel.Sales
{
    public class vmCardConsumerDataUpload
    {
        public string CHALLAN_NUMBER { get; set; }
        public DateTime CHALLAN_DATE { get; set; }
        public string DISTRIBUTOR_ID { get; set; }
        public string DISTRIBUTOR_NAME { get; set; }
        public string DIST_ID { get; set; }
        public string PRODUCT_ID { get; set; }
        public string SHORT_NAME { get; set; }
        public string PRODUCT_NAME { get; set; }
        public decimal CHALLAN_QTY_CTN { get; set; }
        public decimal QTY_CTN { get; set; }
        public decimal QTY_PCS { get; set; }
        public decimal CHALLAN_QTY_PCS { get; set; }
        public decimal DP_CTN { get; set; }
        public decimal DP_PCS { get; set; }
        public string ENTRY_BY { get; set; }
        public string UPLOAD_BY { get; set; }
       public string CHALLAN_ID { get; set; }
       // public string CHALLAN_DATE { get; set; }
    }
}
