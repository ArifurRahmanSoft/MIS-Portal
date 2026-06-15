using CTGroup.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description; 
using System.IO;
using System.Web;
using System.Dynamic;
using CTGroup.Utility.Common;
using System.Data.OleDb;
using System.Data;
using CTGroup.Service.Expense.Interfaces;
using CTGroup.Service.Expense.Factories;
using CTGroup.OracleModel.ViewModel.Expense;
using System.Web.Configuration;
using CTGroup.Models.ViewModel.SystemCommon;

namespace CTGroup.Web.Areas.EkhonTv.api 
{
    [RoutePrefix("EkhonTv/api/ExpenseEntry")]
    public class ExpenseEntryController : ApiController
    {
        private iExpenseEntryMgt objExpenseService = null;

        public ExpenseEntryController()
        {
            objExpenseService = new ExpenseEntryMgt();
        }

        [HttpPost]
        public IHttpActionResult GetExpenseMaster(object[] data)
        {
            long recordsTotal = 0;
            IEnumerable<vmExpenseEntry> objExpenseMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objExpenseMaster = objExpenseService.GetExpenseMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objExpenseMaster
            });
        }

        [HttpPost]
        public IHttpActionResult GetExpenseByID(object[] data)
        {
            object[] ExpenseList = null;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                Int64 EXPENSEID = Convert.ToInt64(data[1]);
                ExpenseList = objExpenseService.GetExpenseByID(objcmnParam, EXPENSEID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                ExpenseList
            });
        }

        [HttpPost]
        public HttpResponseMessage SaveUpdateExpense(object[] data)
        {
            string result = "";
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                vmExpenseEntry ExpenseMaster = JsonConvert.DeserializeObject<vmExpenseEntry>(data[1].ToString());
                List<vmExpenseEntry> ExpenseDetail = JsonConvert.DeserializeObject<List<vmExpenseEntry>>(data[2].ToString());
                
                if (ModelState.IsValid)
                {
                    result = objExpenseService.SaveUpdateExpense(objcmnParam, ExpenseMaster, ExpenseDetail);
                }
                else
                {
                    result = "";
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}

