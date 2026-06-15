using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.SystemCommon
{
    public class vmParentChildMenuPermission
    {
        public decimal MenuID { get; set; }
        public string MenuName { get; set; }
        public string ParentMenuName { get; set; }
        public decimal Sequence { get; set; }
        public decimal ModuleID { get; set; }
        public string ModuleName { get; set; }
        public decimal MenuPermissionID { get; set; }
        public string MenuIconCss { get; set; }
        public string ParentMenuIconCss { get; set; }
        public decimal UserID { get; set; }
        public decimal EnableView { get; set; }
        public decimal EnableInsert { get; set; }
        public decimal EnableUpdate { get; set; }
        public decimal EnableDelete { get; set; }
        public string MenuPath { get; set; }
        public string ParentMenuPath { get; set; }
        public string ReportName { get; set; }
        public string ReportPath { get; set; }
        public decimal ParentID { get; set; }
    }
}
