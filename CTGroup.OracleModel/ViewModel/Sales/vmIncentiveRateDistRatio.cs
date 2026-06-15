using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.Sales
{
    public class vmIncentiveRateDistRatio
    {
        public decimal RATE_ID { get; set; }
        public string BRAND_ID { get; set; }
        public decimal? PER_QUANTITY { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
        public string PER_UOM { get; set; }
        public decimal TAKA_AMOUNT { get; set; }
        public string SF_TYPE { get; set; }
        public decimal EACH_SF_TK_AMOUNT { get; set; }
        public int CompanyID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }        
        //public string BRANDID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public string CreatePc { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public Nullable<int> DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteOn { get; set; }
        public string DeletePc { get; set; }

    }
}
