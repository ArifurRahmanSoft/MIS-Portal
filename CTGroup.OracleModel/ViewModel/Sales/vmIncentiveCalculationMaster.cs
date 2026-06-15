using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.Sales
{
    public class vmIncentiveCalculationMaster
    {
        public decimal DIST_TARGET_ID { get; set; }
        public string BRANDID { get; set; }
        public string SBRND_NAME { get; set; }
        public string DISTRIBUTORID { get; set; }
        public string SALES_FORCE_TYPE_ID { get; set; }
        public decimal TOTALQTY { get; set; }
        public decimal TARGETQUANTITY { get; set; }
        public decimal PRIMARYSALEQUANTITY { get; set; }
        public decimal SECONDARYSALEQUANTITY { get; set; }
        public decimal TOTALQTYINTON { get; set; }
        public decimal ACTUALINCENTIVE { get; set; }
        public string IncentiveCalculationNo { get; set; }
        public int? PITypeID { get; set; }
        public DateTime IncentiveCalculationDate { get; set; }           
        public int CompanyID { get; set; }
        public int? TransactionTypeID { get; set; }
        public bool IsActive { get; set; }     
        public int? Created_By { get; set; }        
    }
}
