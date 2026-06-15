using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.SystemCommon
{
   public class vmCompany
    {
        public Int64 id { get; set; }
        public string label { get; set; }
        public bool IsDefult { set; get; }
        public string SCOMP_OID { get; set; }
        public string SCOMP_NAME { get; set; }

        public vmCompany(string SCOMP_OID, string SCOMP_NAME)
        {
            this.SCOMP_OID = SCOMP_OID;
            this.SCOMP_NAME = SCOMP_NAME;
        }
    }
}
