using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.Sales
{
    public class vmIncentiveCalculationDetail
    {
        public long DISTRIBUTORTARGETID { get; set; }
        public long DISTRIBUTORTARGETDETAILID { get; set; }
        public decimal? QUANTITY { get; set; }
        public int CompanyID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }        
        public string BRANDID { get; set; }
        public int UOMID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CREATEDON { get; set; }
        public string CreatePc { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public Nullable<int> DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteOn { get; set; }
        public string DeletePc { get; set; }

    }
}
