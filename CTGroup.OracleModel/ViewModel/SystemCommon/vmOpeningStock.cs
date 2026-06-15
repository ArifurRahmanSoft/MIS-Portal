using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.SystemCommon
{
    public class vmOpeningStock
    {
        public string OPENING_STOCK_OID { get; set; }
        public string COMPANYOID { get; set; }
        public string PRODUCTOID { get; set; }
        public DateTime STOCKDATE { get; set; }
        public decimal OPENINGQUANTITY { get; set; }
        public decimal OPENINGAMOUNT { get; set; }
    }
}
