using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.Sales
{
    public class vmCmnDocument
    {
        public decimal DOCUMENTID { get; set; }
        public string DOCUMENTNAME { get; set; }
        public string AREA_NATIONAL { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
        public string VIRTUALPATH { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
    }
}
