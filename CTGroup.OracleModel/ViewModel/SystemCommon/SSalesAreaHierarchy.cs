using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.SystemCommon
{
    public class SSalesAreaHierarchy
    {
        public string NTNAL_OID { get; set; }
        public string NTNAL_NAME { get; set; }

        public string DIVISION_ID { get; set; }
        public string DIVISION_NAME { get; set; }

        public string REGION_ID { get; set; }
        public string REGION_NAME { get; set; }

        public string ZONE_ID { get; set; }
        public string ZONE_NAME { get; set; }

        public string DIST_ID { get; set; }
        public string DIST_NAME { get; set; }
        public string DIST_CODE { get; set; }
        public string ADDRESS { get; set; }

        public string ROUTE_ID { get; set; }
        public string ROUTE_NAME { get; set; }

        public string RETAILER_ID { get; set; }
        public string RETAILER_NAME { get; set; }
        public string RETAILER_ADDRESS { get; set; }

        public string TRANSPORT_ID { get; set; }
        public string TRANSPORT_NAME { get; set; }

        public string PRODUCTOID { get; set; }
        public string PRODUCTNAME { get; set; }

        public string SO_ID { get; set; }
        public string SO_NAME { get; set; }
        public string SO_TEXT { get; set; }
        public string SO_IP_PHONE { get; set; }

        public string SARDAR_OID { get; set; }
        public string SARDAR_NAME { get; set; }
        public string DESIGNATION { get; set; }
    }
}
