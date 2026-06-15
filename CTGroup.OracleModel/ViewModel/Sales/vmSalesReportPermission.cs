using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.Sales
{
    public class vmSalesReportPermission
    {
        public string ENABLENATIONAL { get; set; }
        public string ENABLEDIVISION { get; set; }
        public string ENABLEREGION { get; set; }
        public string ENABLEZONE { get; set; }
        public string ENABLEDISTRIBUTOR { get; set; }
        public string NATIONALID { get; set; }
        public string NATIONALNAME { get; set; }
        public string DIVISIONID { get; set; }
        public string DIVISIONNAME { get; set; }
        public string REGIONID { get; set; }
        public string REGIONNAME { get; set; }
        public string ZONEID { get; set; }
        public string ZONENAME { get; set; }
        public string DISTRIBUTORID { get; set; }
        public string DISTRIBUTORNAME { get; set; }

        public string UNDERWHICHDISTRIBUTORID { get; set; }
        public string UNDERWHICHDISTRIBUTORNAME { get; set; }

        public string UNDERWHICHZONEID { get; set; }
        public string UNDERWHICHZONENAME { get; set; }

        public string UNDERWHICHREGIONID { get; set; }
        public string UNDERWHICHREGIONNAME { get; set; }

        public string UNDERWHICHDIVISIONID { get; set; }
        public string UNDERWHICHDIVISIONNAME { get; set; }

        public string UNDERWHICHNATIONALID { get; set; }
        public string UNDERWHICHNATIONALNAME { get; set; }
    }
}
