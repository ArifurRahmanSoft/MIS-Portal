using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.Sales
{
    public class vmIncentiveFormulaSetupMaster
    {
        public decimal INCEN_ID { get; set; }
        public decimal P_SALE_PERCENT { get; set; }
        public decimal S_SALE_PERCENT { get; set; }
        public decimal INCEN_PERCEN { get; set; }
        public string BRAND_ID { get; set; }
        public bool IsActive { get; set; }     
        public int? CREATED_BY { get; set; }
        public decimal MINACHNEXTMONTHCONS { get; set; }
        public string SALES_FORCE_TYPE_ID { get; set; }
        public System.DateTime START_DATE { get; set; }
        public System.DateTime END_DATE { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public string CREATED_IP { get; set; }
        public Nullable<int> UPDATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_ON { get; set; }
        public string UPDATED_IP { get; set; }
        public bool IS_DELETED { get; set; }
        public bool IS_ACTIVE { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> DELETED_BY { get; set; }
        public Nullable<System.DateTime> DELETED_ON { get; set; }
        public string DELETED_IP { get; set; }
    }
}
