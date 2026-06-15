using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.Sales
{
    public class SODIST
    {
        public string DIST_CODE { get; set; }
        public string SO_CODE { get; set; }
    }

    public class SOPRODUCT
    {
        public string PROD_LINE { get; set; }
        public string DIST_CODE { get; set; }
        public string SO_CODE { get; set; }
        public string SKU_CODE { get; set; }
        public decimal CTN_QTY { get; set; }
    }

    public class SOPRODCOL
    {
        public string COLNAME { get; set; }
    }
}
