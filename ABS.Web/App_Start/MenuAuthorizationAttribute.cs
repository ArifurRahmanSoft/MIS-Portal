using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CTGroup.Models;
using CTGroup.OracleModel;

namespace CTGroup.Web
{
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class MenuAuthorizationAttribute : ActionFilterAttribute
    {
        private decimal MenuID;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;


            decimal isOk = CheckAccessRight(actionName, controllerName);
            if (isOk == 1)
            {
                filterContext.RouteData.Values.Add("menuId", MenuID);
                return;
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Accounting/Dashboard");
                //HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
                //var path = UrlHelper.GenerateContentUrl("~/Inventory/InventoryHome/Index", httpContext);
                //filterContext.Result = new JsonResult
                //{

                //    Data = new
                //    {
                //        // put whatever data you want which will be sent
                //        // to the client
                //        url = path
                //    },
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                //};

            }

            base.OnActionExecuting(filterContext);

        }

        public decimal GetMenuId(string controllerName, string actionName)
        {
            //var isSysAdmin = Convert.ToBoolean(Session["IsSysAdmin"]);
            decimal menuId = 0;
            var builder = new StringBuilder();

            try
            {
                using (var db = new Entities())
                {
                    builder.Append("/Accounting").Append("/").Append(controllerName).Append("/").Append("Create");
                    var pathCreate = builder.ToString();
                    menuId = db.T_CMNMENU.First(r => r.MENUPATH == pathCreate).MENUID;
                    if (menuId == 0)
                    {
                        builder.Clear();
                        builder.Append("/Accounting").Append("/").Append(controllerName).Append("/").Append(actionName);
                        pathCreate = builder.ToString();
                        menuId = db.T_CMNMENU.First(r => r.MENUPATH == pathCreate).MENUID;
                    }
                }
            }
            catch (Exception e)
            {
                using (var db = new Entities())
                {
                    builder.Clear();
                    builder.Append("/Accounting").Append("/").Append(controllerName);
                    var pathIndexAndEdit = builder.ToString();
                    menuId = db.T_CMNMENU.First(r => r.MENUPATH == pathIndexAndEdit).MENUID;

                }


            }
            return menuId;

        }
        public T_CMNMENUPERMISSION UserPrivilege(decimal menuId, int userId)
        {
            T_CMNMENUPERMISSION userPrivilege = null;
            using (var db = new Entities())
            {
                userPrivilege = db.T_CMNMENUPERMISSION.FirstOrDefault(r => r.USERID == userId && r.MENUID == menuId);
            }
            return userPrivilege;

        }
        private decimal CheckAccessRight(string action, string controller)
        {
            if (HttpContext.Current.Session["UserId"] != null)
            {
                var userId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
                var isSysAdmin = Convert.ToBoolean(HttpContext.Current.Session["IsSysAdmin"]);
                if (isSysAdmin)
                {
                    return 1;
                }
                var menuId = GetMenuId(controller, action);
                MenuID = menuId;
                var userPrevlg = UserPrivilege(menuId, userId);
                switch (action)
                {
                    case "Create":
                        return userPrevlg.ENABLEINSERT;

                        break;
                    case "Edit":
                        return userPrevlg.ENABLEUPDATE;
                        break;
                    case "Delete":
                        return userPrevlg.ENABLEDELETE;
                        break;
                    case "Index":
                        return userPrevlg.ENABLEVIEW;
                        break;
                    default:
                        return userPrevlg.ENABLEINSERT;
                        break;
                }

            }
            else
            {
                return 0;
            }
        }
    }
}