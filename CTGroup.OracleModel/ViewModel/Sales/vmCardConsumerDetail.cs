using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.Sales
{
    public class vmCardConsumerDetail
    {
        public string PRODUCT_ID { get; set; }
        public string CHALLAN_QTY_CTN { get; set; }
        public decimal CHALLAN_QTY_PCS { get; set; }
        public string MRP_CTN { get; set; }
        public DateTime MRP_PCS { get; set; }
        public DateTime DP_CTN { get; set; }
        public string DP_PCS { get; set; }
        public decimal TP_CTN { get; set; }
        public string TP_PCS { get; set; }
    }
}
