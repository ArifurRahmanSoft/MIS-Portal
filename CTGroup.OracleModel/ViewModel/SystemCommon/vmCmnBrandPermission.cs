using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.SystemCommon
{
    public partial class vmCmnBrandPermission
    {
        public string BRANDOID { get; set; }
        public string NATIONALTEAMOID { get; set; }
        public string MENUNAME { get; set; }
        public string MODULENAME { get; set; }
        public string LOGGEDUSERID { get; set; }
        public decimal BRANDPERMISSIONID { get; set; }
        public decimal ENABLEVIEW { get; set; }
        public bool ENABLE_VIEW { get; set; }
        public int BrandPermissionID { get; set; }
        public List<vmCmnBrandSKUPermission> ListSKUmodel { get; set; }
        public int UserID { get; set; }
        public bool EnableView { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateOn { get; set; }
        public string CreatePc { get; set; }
    }
}
