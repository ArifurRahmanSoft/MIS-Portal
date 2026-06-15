using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.SystemCommon
{
    public partial class vmBrandSKU
    {     
        public string BRANDID { get; set; }
        public string BRANDOID { get; set; }
        public string BRANDNAME { get; set; }
        public string SBRND_TEXT { get; set; }
        public string SBRND_NAME { get; set; }
        public string SGLOC_ID { get; set; }
        public string SGLOC_NAME { get; set; }
        public string CATEGORYID { get; set; }
        public string CATEGORYNAME { get; set; }
        public string PRODUCTOID { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCTNAME { get; set; }
        public decimal PACKSIZE { get; set; }
        public decimal DP_PCS { get; set; }
        public decimal TP_PCS { get; set; }
        public decimal MRP_PCS { get; set; }
        public decimal SWP_PCS { get; set; }
        public string SPCAT_ID { get; set; }
        public string SPCAT_NAME { get; set; }
        public DateTime IDAT { get; set; }
        public DateTime EDAT { get; set; }
        public string ENABLE_CHECK { get; set; }
        public string ISUPDATE { get; set; }
        public string PROD_GROUP_OID { get; set; }
        public string PROD_GROUP_NAME { get; set; }
    }
}
