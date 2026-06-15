using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.Sales
{
    public class vmIncentiveAchievementRatio
    {
        public decimal INCEN_ID { get; set; }
        public string BRAND_ID { get; set; }
        public decimal ACH_RATIO_ID { get; set; }
        public decimal INCEN_FORMU_ID { get; set; }
        public decimal PRIMARY_PERC { get; set; }
        public decimal SECONDARY_PERC { get; set; }
        public decimal PERCENTAGE_TK { get; set; }
        public string SBRND_NAME { get; set; }
        public bool IsActive { get; set; }
        public int? CREATED_BY { get; set; }
        public string SF_TYPE { get; set; }
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
