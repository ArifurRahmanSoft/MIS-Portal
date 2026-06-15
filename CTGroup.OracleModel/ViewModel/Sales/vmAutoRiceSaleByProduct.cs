using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.Sales
{
    public class vmAutoRiceSaleByProduct
    {
        public string OID { get; set; }
        public decimal? CINIGURASALE_BULK { get; set; }
        public decimal? CINIGURASALE_CONSUMER { get; set; }
        public decimal? MASURDALSALE_BULK { get; set; }
        public decimal? MASURDALSALE_CONSUMER { get; set; }
        public decimal? RICESALE_BULK { get; set; }
        public decimal? RICESALE_CONSUMER { get; set; }
        public DateTime ENTRYTIME { get; set; }
        public DateTime SALEDATE { get; set; }
        public string ENTRYUSER { get; set; }
        public DateTime ENTRYDATE { get; set; }
        public string UPDATEBY { get; set; }
        public DateTime UPDATEDATE { get; set; }
        public string SGLOC_NAME { get; set; }
    }
}
