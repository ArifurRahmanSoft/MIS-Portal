using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.SystemCommon
{
    public class vmDataMapping
    {
        public string COMPANY_OID { get; set; }
        //public string COMPANY_NAME { get; set; }
        public string SSTYP_OID { get; set; }
        //public string SSTYP_NAME { get; set; }

        public string SDVNT_OID { get; set; }
        //public string SDVNT_NAME { get; set; }
        public string SPROG_OID { get; set; }
        //public string SPROG_NAME { get; set; }

        public string GROUP_OID { get; set; }
        //public string GROUP_NAME { get; set; }
        public string USER_OID { get; set; }
        //public string USER_TEXT { get; set; }
        public string BRAND_OID { get; set; }
        //public string BRAND_NAME { get; set; }
        public string SKU_OID { get; set; }
        //public string SKU_NAME { get; set; }

        public string ROLE_OID { get; set; }
        //public string ROLE_NAME { get; set; }
        public string NTNAL_OID { get; set; }
        //public string NTNAL_NAME { get; set; }
        public bool IS_ENABLE { get; set; }

        public string TRAN_TYPE_ID { get; set; }
        public string CMN_OID { get; set; }
        public string USER_FULLNAME { get; set; }
        public string USER_PASS { get; set; }
        public string LOGGED_USER { get; set; }
        public bool IS_UPDATE { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string JSON_DATA { get; set; }
    }
}
