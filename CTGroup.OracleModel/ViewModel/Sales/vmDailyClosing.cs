using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.Sales
{
    public class vmDailyClosing
    {
        public string OID { get; set; }
        public decimal? BULKDAILYCLOSINGBALANCE { get; set; }
        public decimal? CLOSINGBALANCE { get; set; }
        public DateTime ENTRYTIME { get; set; }
        public DateTime SALEDATE { get; set; }
        public string ENTRYUSER { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public string UPDATEBY { get; set; }
        public DateTime UPDATEDATE { get; set; }
        public string USERTYPE { get; set; }
        public string SALETYPE { get; set; }
        public string LOCATIONOID { get; set; }
    }
}
