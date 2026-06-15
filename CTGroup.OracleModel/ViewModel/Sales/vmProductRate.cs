using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.Sales
{
    public class vmProductRate
    {
        public string DIST_OID { get; set; }
        public string REF_NUMBER { get; set; }
        public decimal DP_CTN { get; set; }
        public decimal DP_PCS { get; set; }
        public decimal TP_CTN { get; set; }
        public decimal TP_PCS { get; set; }
        public decimal MRP_CTN { get; set; }
        public decimal MRP_PCS { get; set; }
        public decimal SWP_CTN { get; set; }
        public decimal SWP_PCS { get; set; }
        public string REMARKS { get; set; }
        public string BRANDOID { get; set; }
        public string SBRND_NAME { get; set; }
        public string PRODUCTOID { get; set; }
        public string PRODUCTNAME { get; set; }
        public string PACKSIZE { get; set; }       
        public string VERIFIED_BY { get; set; }
        public string APPROVED_BY { get; set; }
        public string ENTRY_BY { get; set; }
        public string UPDATE_BY { get; set; }
        public string DEVICE_ID { get; set; }
        public int VERSION { get; set; }
        public string LOC_ID { get; set; }
        public string COMP_ID { get; set; }
        public DateTime APPROVED_DATE { get; set; }
        public DateTime ENTRY_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public DateTime DT { get; set; }
        public DateTime ACTIVE_DATE { get; set; }
        public DateTime CLOSE_DATE { get; set; }
    }
}
