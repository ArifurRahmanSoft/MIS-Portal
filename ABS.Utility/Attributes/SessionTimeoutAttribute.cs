using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Utility.Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public SessionTimeoutAttribute()
        {
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                HttpContext ctx = HttpContext.Current;
                string path = Utils.ExtDire + "/Home"; // filterContext.HttpContext.Request.FilePath.ToString();
                int length = path.ToString().Length;
                if (HttpContext.Current.Session["UserID"] == null)
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                    return;
                }
                if (HttpContext.Current.Session["UserID"] != null && HttpContext.Current.Session["CompanyID"] != null)
                {
                    Int64 companyID = Convert.ToInt64(HttpContext.Current.Session["CompanyID"]);
                    Int64 userID = Convert.ToInt64(HttpContext.Current.Session["UserID"]);
                    if (length > 1)
                    {
                        List<vmCmnMenu> menuList = null;
                        string spQuery = string.Empty;
                        try
                        {
                            OracleCommand objCmd = new OracleCommand();
                            objCmd.CommandText = "SETTINGS.Get_MenuList";
                            //objCmd.CommandText = "USERROLEMENUPERMISSION.Get_MenuList";
                            objCmd.CommandType = CommandType.StoredProcedure;

                            objCmd.Parameters.Add("CompanyIdIn", OracleDbType.Decimal).Value = companyID;
                            objCmd.Parameters.Add("LoggedUser", OracleDbType.Decimal).Value = userID;
                            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                            DataTable dt = classDt.GetData(objCmd);

                            menuList = dt.AsEnumerable().Select(dataRow => new vmCmnMenu
                            {
                                MenuID = dataRow.Field<decimal>("MenuID"),
                                MenuName = dataRow.Field<string>("MenuName"),
                                MenuPath = dataRow.Field<string>("MenuPath")
                            }).ToList();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }

                        var menuPermission = (from menu in menuList
                                              where menu.MenuPath.Contains(path)
                                              select menu).ToList();

                        if (menuPermission.Count() == 0 && path != Utils.ExtDire + "/Home" && !path.Contains("/SystemCommon"))
                        {
                            HttpContext.Current.Session["UserID"] = null;
                            HttpContext.Current.Session["CompanyID"] = null;
                            filterContext.Result = new RedirectResult("~/Account/Login");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();

                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                Utils u = new Utils();
                u.LogWrite(ex, filename, line);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
