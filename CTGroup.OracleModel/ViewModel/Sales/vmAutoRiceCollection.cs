using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.Sales
{
    public class vmAutoRiceCollection
    {
        public string OID { get; set; }
        public decimal? CASHCOLLECTION { get; set; }
        public decimal? CHEQUECOLLECTION { get; set; }
        public decimal? TTCOLLECTION { get; set; }
        public decimal? LCCOLLECTION { get; set; }

        public decimal? OTHERCOLLECTION { get; set; }
        public DateTime ENTRYTIME { get; set; }
        public DateTime SALEDATE { get; set; }
        public string ENTRYUSER { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public string UPDATEBY { get; set; }
        public DateTime UPDATEDATE { get; set; }
        public string USERTYPE { get; set; }
        public string SALETYPE { get; set; }
        public string LOCATIONOID { get; set; }
        public string SGLOC_NAME { get; set; }
    }
}
