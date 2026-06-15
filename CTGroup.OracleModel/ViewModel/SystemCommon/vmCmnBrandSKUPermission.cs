using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.SystemCommon
{
    public partial class vmCmnBrandSKUPermission
    {
        public string NATIONALOID { get; set; }
        public string BRANDOID { get; set; }
        public string PRODUCTOID { get; set; }
        public string LOGGEDUSERID { get; set; }
        public bool ISUPDATE { get; set; }
        public bool ENABLE_CHECK { get; set; }
        public string CREATEBY { get; set; }
        public DateTime? CREATEON { get; set; }
        public string CREATEPC { get; set; }
    }
}
