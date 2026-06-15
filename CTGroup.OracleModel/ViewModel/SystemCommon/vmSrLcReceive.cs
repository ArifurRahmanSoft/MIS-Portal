using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.SystemCommon
{
    public class vmSrLcReceive
    {
        public string SR_LC_RECEIVE_OID { get; set; }
        public string COMPANYOID { get; set; }
        public string PRODUCTOID { get; set; }
        public DateTime RECEIVEDDATE { get; set; }
        public decimal RECEIVEDQUANTITY { get; set; }
    }
}
