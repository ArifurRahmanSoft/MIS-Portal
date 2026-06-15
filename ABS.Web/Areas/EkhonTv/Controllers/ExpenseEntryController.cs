using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.Expense.Controllers 
{
    public class ExpenseEntryController : Controller
    {
        // GET: EkhonTv/ExpenseEntry/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
	}
}