using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.SystemCommon
{
    public class vmRoleMenu
    {
        public decimal RMID { get; set; }
        public string MENUNAME { get; set; }
        public decimal MENUID { get; set; }
        public decimal PARENTID { get; set; }
        public decimal MODULEID { get; set; }
        public string MODULENAME { get; set; }
        public decimal ROLEID { get; set; }
        public decimal ENABLEVIEW { get; set; }
        public decimal ENABLEINSERT { get; set; }
        public decimal ENABLEUPDATE { get; set; }
        public decimal ENABLEDELETE { get; set; }
        public string ISACTIVE { get; set; }
        public string LOGGEDUSERID { get; set; }
    }
}
