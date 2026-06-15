using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.SystemCommon
{
    public class vmSprodCostRate
    {
        public string OID { get; set; }
        public string COST_TRNO { get; set; }
        public DateTime COST_TRDT { get; set; }
        public string COST_SCOMP { get; set; }
        public string COST_SPROD { get; set; }
        public decimal COST_RATE { get; set; }
        public string COST_STATUS { get; set; }
        public string IUSER { get; set; }
        public string EUSER { get; set; }
        public DateTime IDAT { get; set; }
        public DateTime EDAT { get; set; }

    }
}
