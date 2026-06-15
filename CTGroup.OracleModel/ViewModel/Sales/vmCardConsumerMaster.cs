using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.Sales
{
    public class vmCardConsumerMaster
    {
        public string CHALLAN_NUMBER
        { get; set; }
        public string CHALLAN_DATE
        { get; set; }
        public decimal DISTRIBUTOR_ID
        { get; set; }
    }
}
