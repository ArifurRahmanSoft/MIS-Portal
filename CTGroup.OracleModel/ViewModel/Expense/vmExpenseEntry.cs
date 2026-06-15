using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.Expense
{
    public class vmExpenseEntry
    {
        public string REQUISITIONBY { get; set; }
        public string CTGSTAFFID { get; set; }
        public decimal? TOTALCOST { get; set; }
        public string APPROVEDBY { get; set; }
        public decimal? BUILDINGID { get; set; }
        public string BUILDINGNAME { get; set; }
        public string FLOORFLATNO { get; set; }
        public decimal? FLOORNO { get; set; }
        public string FLATNO { get; set; }
        public decimal? TENANTID { get; set; }
        public decimal? EXPENSEID { get; set; }
        public string EXPENSEDETAIL { get; set; }
        public string DETAILOFEXPENSE { get; set; }
        public string ACCOUNTNAME { get; set; }
        public string CHEQUENO { get; set; }
        public string BRANCHNAME { get; set; }
        public decimal? EXPENSEAREAHEADID { get; set; }
        public string EXPENSEHEADNAME { get; set; }
        public DateTime EXPENSEDATE { get; set; }
        public DateTime? EXPENSEMONTH { get; set; }
        public string ISACTIVE { get; set; }
        public string CREATEBY { get; set; }
        public DateTime CREATEON { get; set; }
        public string CREATEIP { get; set; }
    }
}
