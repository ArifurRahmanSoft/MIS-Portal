using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Utility;
using CTGroup.Utility.Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CTGroup.Service.MenuMgt
{
    public class MenuMgt : iMenuMgt
    {
        public MenuMgt()
        {

        }
        public List<vmBreadCrums> GetBreadCrums(int? MenuID)
        {
            List<vmBreadCrums> list = new List<vmBreadCrums>();
            try
            {
                List<vmParentChildMenuPermission> obj = new List<vmParentChildMenuPermission>();

                try
                {
                    OracleCommand objCmd = new OracleCommand();
                    objCmd.CommandText = "SETTINGS.Get_BreadCrums";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("MenuIdIn", OracleDbType.Decimal).Value = MenuID;
                    objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                    DataTable dt = classDt.GetData(objCmd);

                    obj = dt.AsEnumerable().Select(dataRow => new vmParentChildMenuPermission
                    {
                        MenuID = dataRow.Field<decimal>("MENUID"),
                        MenuName = dataRow.Field<string>("MENUNAME"),
                        MenuIconCss = dataRow.Field<string>("MENUICONCSS"),
                        ParentID = dataRow.Field<decimal>("PARENTID"),
                        ParentMenuIconCss = dataRow.Field<string>("ParentMenuIconCss"),
                        ParentMenuName = dataRow.Field<string>("ParentMenuName"),
                        ModuleID = dataRow.Field<decimal>("MODULEID"),
                        ModuleName = dataRow.Field<string>("MODULENAME"),
                        MenuPath = Utils.ExtDire + dataRow.Field<string>("MENUPATH"),
                        ParentMenuPath = Utils.ExtDire + dataRow.Field<string>("ParentMenuPath")
                    }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }

                list.Add(new vmBreadCrums { Name = string.IsNullOrEmpty(obj.FirstOrDefault().ModuleName) ? string.Empty : obj.FirstOrDefault().ModuleName.ToLower(), Icon = obj.FirstOrDefault().ParentMenuIconCss, Path = obj.FirstOrDefault().ParentMenuPath });
                list.Add(new vmBreadCrums { Name = string.IsNullOrEmpty(obj.FirstOrDefault().ParentMenuName) ? string.Empty : obj.FirstOrDefault().ParentMenuName.ToLower(), Icon = "", Path = obj.FirstOrDefault().ParentMenuPath });

                String[] menuName;
                var customMenuName = obj.FirstOrDefault().MenuName.ToString();
                if (obj.FirstOrDefault().MenuName.Contains('('))
                {
                    menuName = obj.FirstOrDefault().MenuName.Split('(');
                    customMenuName = menuName[0].ToString().ToLower() + ("(" + menuName[1].ToString().ToUpper());
                }
                list.Add(new vmBreadCrums { Name = customMenuName, Icon = "", Path = obj.FirstOrDefault().MenuPath });
            }
            catch (Exception)
            {
                list = new List<vmBreadCrums>();
                list.Add(new vmBreadCrums { Name = "Na", Icon = "icon_Home", Path = "" });
            }
            return list;
        }
        public int CheckAuthorization(Int64 companyID, Int64 userID, string path)
        {

            return 0;
        }

        public List<vmCmnModule> GetTopMenu(int? companyID, int? loggedUser, int? ModuleID)
        {
            List<vmCmnModule> topMenuDistinct = null;
            string spQuery = string.Empty;
            try
            {
                OracleCommand objCmd = new OracleCommand();

                objCmd.CommandText = "SETTINGS.Get_TopMenu";

                //objCmd.CommandText = "USERROLEMENUPERMISSION.Get_TopMenu";

                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("CompanyIdIn", OracleDbType.Decimal).Value = companyID;
                objCmd.Parameters.Add("LoggedUser", OracleDbType.Decimal).Value = loggedUser;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                DataTable dt = classDt.GetData(objCmd);

                var topMenu = dt.AsEnumerable().Select(dataRow => new vmCmnModule
                {
                    ModuleID = dataRow.Field<decimal>("ModuleID"),
                    ModuleName = dataRow.Field<string>("ModuleName"),
                    ModulePath = Utils.ExtDire + dataRow.Field<string>("ModulePath"),
                    ImageURL = dataRow.Field<string>("ImageURL"),
                    Sequence = dataRow.Field<decimal>("Sequence")
                }).ToList();

                topMenuDistinct = topMenu.Distinct().OrderBy(x => x.Sequence).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return topMenuDistinct;
        }

        public object GetMenuPermission(vmApplicationTokenModel model)
        {
            int loggedUser = model.loggedUserID, companyID = model.loggedCompanyID;
            String moduleName = string.Empty;
            string[] parts = model.MenuPath.Split(new char[] { '/' });
            if (parts.Length > 2)
            {
                moduleName = parts[1];
            }

            List<vmParentChildMenuPermission> parentMenu = null;

            try
            {
                OracleCommand objCmd = new OracleCommand();

                objCmd.CommandText = "SETTINGS.Get_ParentMenuPermission";

                //objCmd.CommandText = "USERROLEMENUPERMISSION.Get_ParentMenuPermission";

                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("CompanyIdIn", OracleDbType.Decimal).Value = companyID;
                objCmd.Parameters.Add("LoggedUser", OracleDbType.Decimal).Value = loggedUser;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                DataTable dt = classDt.GetData(objCmd);

                parentMenu = dt.AsEnumerable().Select(dataRow => new vmParentChildMenuPermission
                {
                    MenuID = dataRow.Field<decimal>("MENUID"),
                    ParentID = dataRow.Field<decimal>("PARENTID"),
                    Sequence = dataRow.Field<decimal>("SEQUENCE"),
                    MenuName = dataRow.Field<string>("MENUNAME"),
                    MenuPath = Utils.ExtDire + dataRow.Field<string>("MENUPATH"),
                    ReportName = dataRow.Field<string>("REPORTNAME"),
                    MenuIconCss = dataRow.Field<string>("MENUICONCSS"),
                    ModuleID = dataRow.Field<decimal>("MODULEID"),
                    EnableView = dataRow.Field<decimal>("ENABLEVIEW"),
                    EnableInsert = dataRow.Field<decimal>("ENABLEINSERT"),
                    EnableUpdate = dataRow.Field<decimal>("ENABLEUPDATE"),
                    EnableDelete = dataRow.Field<decimal>("ENABLEDELETE")

                }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            List<vmParentChildMenuPermission> childMenu = null;

            try
            {
                OracleCommand objCmd = new OracleCommand();

                objCmd.CommandText = "SETTINGS.Get_ChildMenuPermission";

                //objCmd.CommandText = "USERROLEMENUPERMISSION.Get_ChildMenuPermission";

                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("CompanyIdIn", OracleDbType.Decimal).Value = companyID;
                objCmd.Parameters.Add("LoggedUser", OracleDbType.Decimal).Value = loggedUser;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                DataTable dt = classDt.GetData(objCmd);

                childMenu = dt.AsEnumerable().Select(dataRow => new vmParentChildMenuPermission
                {
                    MenuID = dataRow.Field<decimal>("MENUID"),
                    ParentID = dataRow.Field<decimal>("PARENTID"),
                    MenuName = dataRow.Field<string>("MENUNAME"),
                    MenuPath = Utils.ExtDire + dataRow.Field<string>("MENUPATH"),
                    MenuIconCss = dataRow.Field<string>("MENUICONCSS"),
                    ModuleID = dataRow.Field<decimal>("MODULEID"),
                    Sequence = dataRow.Field<decimal>("SEQUENCE"),
                    EnableView = dataRow.Field<decimal>("ENABLEVIEW"),
                    EnableInsert = dataRow.Field<decimal>("ENABLEINSERT"),
                    EnableUpdate = dataRow.Field<decimal>("ENABLEUPDATE"),
                    EnableDelete = dataRow.Field<decimal>("ENABLEDELETE")

                }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            var dba = (
               from tb in parentMenu
               where tb.MenuPath.Contains(moduleName)
               select new
               { //// get parent menu
                   tb.MenuID,
                   tb.ParentID,
                   tb.Sequence,
                   MenuName = tb.MenuName.ToLower(),
                   tb.MenuPath,
                   tb.ReportName,
                   tb.MenuIconCss,
                   tb.ModuleID,
                   tb.EnableView,
                   tb.EnableInsert,
                   tb.EnableUpdate,
                   tb.EnableDelete,

                   ChildMenues = (
                 from cm in childMenu
                 where tb.MenuID == cm.ParentID && cm.MenuPath == model.MenuPath
                 select new
                 { ///// get child menu
                     cm.MenuID,
                     cm.ParentID,
                     cm.MenuName,
                     cm.MenuPath,
                     cm.MenuIconCss,
                     cm.ModuleID,
                     cm.Sequence,
                     cm.EnableView,
                     cm.EnableInsert,
                     cm.EnableUpdate,
                     cm.EnableDelete
                 }).Distinct().OrderBy(x => x.Sequence)
               }).ToList().OrderBy(x => x.Sequence);

            return dba;
        }

        public object GetSideMenu(int? companyID, int? loggedUser, int? ModuleID)
        {
            List<vmParentChildMenuPermission> parentMenu = null;

            try
            {
                OracleCommand objCmd = new OracleCommand();

                objCmd.CommandText = "SETTINGS.Get_ParentMenuPermission";

                //objCmd.CommandText = "USERROLEMENUPERMISSION.Get_ParentMenuPermission";

                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("CompanyIdIn", OracleDbType.Decimal).Value = companyID;
                objCmd.Parameters.Add("LoggedUser", OracleDbType.Decimal).Value = loggedUser;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                DataTable dt = classDt.GetData(objCmd);

                parentMenu = dt.AsEnumerable().Select(dataRow => new vmParentChildMenuPermission
                {
                    MenuID = dataRow.Field<decimal>("MENUID"),
                    ParentID = dataRow.Field<decimal>("PARENTID"),
                    Sequence = dataRow.Field<decimal>("SEQUENCE"),
                    MenuName = dataRow.Field<string>("MENUNAME"),
                    MenuPath = Utils.ExtDire + dataRow.Field<string>("MENUPATH"),
                    ReportName = dataRow.Field<string>("REPORTNAME"),
                    MenuIconCss = dataRow.Field<string>("MENUICONCSS"),
                    ModuleID = dataRow.Field<decimal>("MODULEID"),
                    EnableView = dataRow.Field<decimal>("ENABLEVIEW"),
                    EnableInsert = dataRow.Field<decimal>("ENABLEINSERT"),
                    EnableUpdate = dataRow.Field<decimal>("ENABLEUPDATE"),
                    EnableDelete = dataRow.Field<decimal>("ENABLEDELETE")

                }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            List<vmParentChildMenuPermission> childMenu = null;

            try
            {
                OracleCommand objCmd = new OracleCommand();

                objCmd.CommandText = "SETTINGS.Get_ChildMenuPermission";

                //objCmd.CommandText = "USERROLEMENUPERMISSION.Get_ChildMenuPermission";

                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("CompanyIdIn", OracleDbType.Decimal).Value = companyID;
                objCmd.Parameters.Add("LoggedUser", OracleDbType.Decimal).Value = loggedUser;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                DataTable dt = classDt.GetData(objCmd);

                childMenu = dt.AsEnumerable().Select(dataRow => new vmParentChildMenuPermission
                {
                    MenuID = dataRow.Field<decimal>("MENUID"),
                    ParentID = dataRow.Field<decimal>("PARENTID"),
                    MenuName = dataRow.Field<string>("MENUNAME"),
                    MenuPath = Utils.ExtDire + dataRow.Field<string>("MENUPATH"),
                    MenuIconCss = dataRow.Field<string>("MENUICONCSS"),
                    ModuleID = dataRow.Field<decimal>("MODULEID"),
                    Sequence = dataRow.Field<decimal>("SEQUENCE"),
                    EnableView = dataRow.Field<decimal>("ENABLEVIEW"),
                    EnableInsert = dataRow.Field<decimal>("ENABLEINSERT"),
                    EnableUpdate = dataRow.Field<decimal>("ENABLEUPDATE"),
                    EnableDelete = dataRow.Field<decimal>("ENABLEDELETE")

                }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            var db = (
               from tb in parentMenu
               where tb.ModuleID == ModuleID
               select new
               { //// get parent menu                  
                   tb.MenuID,
                   tb.ParentID,
                   tb.Sequence,
                   MenuName = tb.MenuName.ToLower(),
                   tb.MenuPath,
                   tb.ReportName,
                   tb.MenuIconCss,
                   tb.ModuleID,
                   tb.EnableView,
                   tb.EnableInsert,
                   tb.EnableUpdate,
                   tb.EnableDelete,

                   ChildMenues = (
                 from cm in childMenu
                 where tb.MenuID == cm.ParentID
                 select new
                 { ///// get child menu
                     cm.MenuID,
                     cm.ParentID,
                     cm.MenuName,
                     cm.MenuPath,
                     cm.MenuIconCss,
                     cm.ModuleID,
                     cm.Sequence,
                     cm.EnableView,
                     cm.EnableInsert,
                     cm.EnableUpdate,
                     cm.EnableDelete
                 }).Distinct().OrderBy(x => x.MenuName)
               }).ToList().OrderBy(x => x.MenuName);

            return db;
        }

        public object GetSideMenu(int? companyID, string loggedUser, int? ModuleID)
        {
            //string mainMenuess = string.Empty, childMenuess = string.Empty, subChildMenuess = string.Empty;
            DataTable mainMenues = new DataTable(), childMenues = new DataTable(), subChildMenues = new DataTable();
            //object result = null;
            try
            {
                ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                OracleCommand cmnd = new OracleCommand();                
                cmnd.CommandType = CommandType.StoredProcedure;
                cmnd.Parameters.Add("sresult", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmnd.Parameters.Add("s_UserId", OracleDbType.Varchar2).Value = loggedUser;
                cmnd.Parameters.Add("s_RoleId", OracleDbType.Decimal).Value = null;
                cmnd.Parameters.Add("s_ModuleId", OracleDbType.Decimal).Value = ModuleID;
                cmnd.Parameters.Add("s_ExtDire", OracleDbType.Varchar2).Value = Utils.ExtDire;

                cmnd.CommandText = "SETTINGS.Get_CmnMainMenu";
                mainMenues = classDt.GetData(cmnd);
                cmnd.CommandText = "SETTINGS.Get_CmnChildMenu";
                childMenues = classDt.GetData(cmnd);
                cmnd.CommandText = "SETTINGS.Get_CmnSubChildMenu";
                subChildMenues = classDt.GetData(cmnd);
            }
            catch (Exception ex)
            {
                ex.ToString();
                //Logs.WriteBug(ex);
            }
            return new
            {
                mainMenues,
                childMenues,
                subChildMenues
            };
        }

        //public object GetSideMenu(vmCmnParameters param)
        //{            
        //    //string mainMenuess = string.Empty, childMenuess = string.Empty, subChildMenuess = string.Empty;
        //    DataTable mainMenues = new DataTable(), childMenues=new DataTable(), subChildMenues=new DataTable();
        //    //object result = null;
        //    try
        //    {
        //        OracleCommand cmnd = new OracleCommand();
        //        cmnd.CommandText = "SETTINGS.Get_ChildMenuPermission";
        //        cmnd.CommandType = CommandType.StoredProcedure;
        //        cmnd.Parameters.Add("sresult", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        //        cmnd.Parameters.Add("s_UserId", OracleDbType.Varchar2).Value = param.loggeduser;
        //        cmnd.Parameters.Add("s_RoleId", OracleDbType.Decimal).Value = null;
        //        cmnd.Parameters.Add("s_ModuleId", OracleDbType.Decimal).Value = param.id;
        //        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
        //        //DataTable dt = classDt.GetData(cmnd);

        //        mainMenues = classDt.GetData(cmnd);
        //        childMenues = classDt.GetData(cmnd);
        //        subChildMenues = classDt.GetData(cmnd);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //        //Logs.WriteBug(ex);
        //    }
        //    return new
        //    {
        //        mainMenues,
        //        childMenues,
        //        subChildMenues
        //    };
        //}


        //Without Permisison 
        //public dynamic GetSideMenu(int? companyID, int? loggedUser, int? ModuleID)
        //{
        //    using (ERP_Entities dbContext = new ERP_Entities())
        //    {
        //        var db = (
        //            from tb in dbContext.CmnMenus
        //            where tb.ModuleID == ModuleID &&
        //            (tb.ParentID == 0 || tb.ParentID == null)
        //            select new
        //            {
        //                MenuID = tb.MenuID,
        //                ParentID = tb.ParentID ?? 0,
        //                MenuName = tb.MenuName,
        //                MenuPath = tb.MenuPath,
        //                ReportName = tb.ReportName,
        //                MenuIconCss = tb.MenuIconCss,
        //                ChildMenues =
        //              from master in dbContext.CmnMenus
        //              join details in dbContext.CmnMenus on master.MenuID equals details.ParentID into leftGroup
        //              from lg in leftGroup.DefaultIfEmpty()
        //              where lg.MenuName != null && tb.MenuID == lg.ParentID
        //              select new
        //              {
        //                  lg.MenuID,
        //                  lg.ParentID,
        //                  lg.MenuName,
        //                  lg.MenuPath,
        //                  lg.MenuIconCss
        //              }
        //            });
        //        return db;
        //    }
        //}
        //public List<vmBreadCrums> GetBreadCrums(int? MenuID)
        //{
        //    List<vmBreadCrums> list = new List<vmBreadCrums>();

        //    try
        //    {
        //        using (ERP_Entities dbContext = new ERP_Entities())
        //        {

        //            var obj = (
        //                    from menu in dbContext.CmnMenus
        //                    join mod in dbContext.CmnModules on menu.ModuleID equals mod.ModuleID into ModGrouo
        //                    from modG in ModGrouo.DefaultIfEmpty()
        //                    join pMenu in dbContext.CmnMenus on menu.ParentID equals pMenu.MenuID into parentGroup
        //                    from pm in parentGroup.DefaultIfEmpty()
        //                    where menu.MenuID == MenuID
        //                    select new vmCmnMenu
        //                    {
        //                        MenuID = menu.MenuID,
        //                        MenuName = menu.MenuName,
        //                        MenuIconCss = menu.MenuIconCss,
        //                        ParentID = menu.ParentID ?? 0,
        //                        ParentMenuIconCss = pm.MenuIconCss,
        //                        ParentMenuName = pm.MenuName,
        //                        ModuleID = menu.ModuleID,
        //                        ModuleName = modG.ModuleName,
        //                        MenuPath = menu.MenuPath,
        //                        ParentMenuPath = pm.MenuPath
        //                    }).FirstOrDefault();


        //            list.Add(new vmBreadCrums { Name = obj.ModuleName.ToLower(), Icon = obj.ParentMenuIconCss, Path = obj.ParentMenuPath });
        //            list.Add(new vmBreadCrums { Name = obj.ParentMenuName.ToLower(), Icon = "", Path = obj.ParentMenuPath });

        //            String[] menuName;
        //            var customMenuName = obj.MenuName.ToString();
        //            if (obj.MenuName.Contains('('))
        //            {
        //                menuName = obj.MenuName.Split('(');
        //                customMenuName = menuName[0].ToString().ToLower() + ("(" + menuName[1].ToString().ToUpper());
        //            }


        //            list.Add(new vmBreadCrums { Name = customMenuName, Icon = "", Path = obj.MenuPath });

        //        }
        //    }
        //    catch (Exception)
        //    {
        //        list = new List<vmBreadCrums>();
        //        list.Add(new vmBreadCrums { Name = "Na", Icon = "icon_Home", Path = "" });

        //    }
        //    return list;
        //}


        //public List<vmCmnModule> GetTopMenu(int? companyID, int? loggedUser, int? ModuleID)
        //{
        //    ERP_Entities dbContext = new ERP_Entities();
        //    var usergroupID = dbContext.CmnUsers.Where(x => x.UserID == loggedUser).FirstOrDefault().UserGroupID;
        //    var topMenu =
        //        (from a in dbContext.CmnModules
        //         join b in dbContext.CmnModulePermissions on a.ModuleID equals b.ModuleID
        //         join c in dbContext.CmnMenus on a.ModuleID equals c.ModuleID
        //         join d in dbContext.CmnMenuPermissions on c.MenuID equals d.MenuID
        //         where b.CompanyID == companyID && d.UserID == loggedUser
        //         && a.IsDeleted == false
        //         && b.IsDeleted == false
        //         && c.IsDeleted == false
        //         && d.IsDeleted == false
        //         && d.EnableView == true
        //         select new vmCmnModule
        //         {
        //             ModuleID = a.ModuleID,
        //             ModuleName = a.ModuleName,
        //             ModulePath = a.ModulePath,
        //             ImageURL = a.ImageURL
        //         }).Distinct().OrderBy(x=>x.ModuleID).ToList();

        //    return topMenu;
        //}

        //public object GetMenuPermission(vmApplicationTokenModel model)
        //{
        //    int loggedUser = model.loggedUserID, companyID = model.loggedCompanyID;
        //    String moduleName = string.Empty;
        //    string[] parts = model.MenuPath.Split(new char[] { '/' });
        //    if (parts.Length > 2)
        //    {
        //        moduleName = parts[1];
        //    }
        //    ERP_Entities dbContext = new ERP_Entities();
        //    var usergroupID = dbContext.CmnUsers.Where(x => x.UserID == loggedUser).FirstOrDefault().UserGroupID;

        //    var dba = (
        //        from tb in dbContext.CmnMenus
        //        join permission in dbContext.CmnMenuPermissions on tb.MenuID equals permission.MenuID
        //     //   where tb.CompanyID == companyID &&
        //        where permission.CompanyID == companyID &&
        //        (tb.ParentID == 0 || tb.ParentID == null)
        //        && (permission.UserID == loggedUser)
        //        && tb.MenuPath.Contains(moduleName)
        //        select new
        //        {
        //            MenuID = tb.MenuID,
        //            ParentID = tb.ParentID ?? 0,
        //            Sequencea = tb.Sequence,
        //            MenuName = tb.MenuName.ToLower(),
        //            MenuPath = tb.MenuPath,
        //            ReportName = tb.ReportName ?? "c",
        //            MenuIconCss = tb.MenuIconCss,
        //            tb.ModuleID,
        //            permission.EnableView,
        //            permission.EnableInsert,
        //            permission.EnableUpdate,
        //            permission.EnableDelete,
        //            ChildMenues = (
        //          from master in dbContext.CmnMenus
        //          join details in dbContext.CmnMenus on master.MenuID equals details.ParentID into leftGroup
        //          from lg in leftGroup.DefaultIfEmpty()
        //          where lg.MenuName != null && tb.MenuID == lg.ParentID
        //          join permissions in dbContext.CmnMenuPermissions on lg.MenuID equals permissions.MenuID
        //          join tran in dbContext.CmnTransactionTypes.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on lg.MenuID equals tran.MenuID into leftTranGroup
        //          from ltg in leftTranGroup.DefaultIfEmpty()
        //          //Change Due  To Badru vai
        //          //join userJobContract in dbContext.CmnUserJobContracts.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on loggedUser equals userJobContract.UserID into bgroup
        //          join userJobContract in dbContext.CmnUserJobContracts.Where(x => x.IsActive == true && x.CompanyID == companyID) on loggedUser equals userJobContract.UserID into bgroup

        //          from blj in bgroup.DefaultIfEmpty()
        //          where lg.IsDeleted == false
        //          where lg.MenuName != null && tb.MenuID == lg.ParentID && permissions.EnableView == true
        //          && (permissions.UserID == loggedUser || permissions.UserGroupID == usergroupID)
        //          && permissions.CompanyID == companyID
        //             //&& lg.MenuPath.Contains(model.MenuPath)
        //             && lg.MenuPath==model.MenuPath
        //          select new
        //          {
        //              lg.MenuID,
        //              lg.ParentID,
        //              lg.MenuName,
        //              lg.MenuPath,
        //              lg.MenuIconCss,
        //              lg.ModuleID,
        //              lg.Sequence,
        //              permissions.EnableView,
        //              permissions.EnableInsert,
        //              permissions.EnableUpdate,
        //              permissions.EnableDelete,
        //              TransactionTypeID = (int?)ltg.TransactionTypeID,
        //              TransactionTypeName = ltg.TransactionTypeName,
        //              DepartmentID = blj.DepartmentID
        //          }).Distinct().OrderBy(x => x.Sequence)
        //        }).ToList().OrderBy(x => x.Sequencea); 

        //    return dba;
        //}

        //public object GetSideMenu(int? companyID, int? loggedUser, int? ModuleID)
        //{
        //    ERP_Entities dbContext = new ERP_Entities();
        //    var usergroupID = dbContext.CmnUsers.Where(x => x.UserID == loggedUser).FirstOrDefault().UserGroupID;

        //    var db = (
        //        from tb in dbContext.CmnMenus
        //        join permission in dbContext.CmnMenuPermissions on tb.MenuID equals permission.MenuID
        //        where tb.ModuleID == ModuleID && permission.CompanyID == companyID &&
        //        (tb.ParentID == 0 || tb.ParentID == null)
        //        && (permission.UserID == loggedUser)
        //        select new
        //        {
        //            MenuID = tb.MenuID,
        //            Sequencea = tb.Sequence,
        //            ParentID = tb.ParentID ?? 0,
        //            MenuName = tb.MenuName.ToLower(),
        //            MenuPath = tb.MenuPath,
        //            ReportName = tb.ReportName ?? "c",
        //            MenuIconCss = tb.MenuIconCss,
        //            permission.EnableView,
        //            permission.EnableInsert,
        //            permission.EnableUpdate,
        //            permission.EnableDelete,
        //            ChildMenues = (
        //          from master in dbContext.CmnMenus
        //          join details in dbContext.CmnMenus on master.MenuID equals details.ParentID into leftGroup
        //          from lg in leftGroup.DefaultIfEmpty()
        //          where lg.MenuName != null && tb.MenuID == lg.ParentID
        //          join permissions in dbContext.CmnMenuPermissions on lg.MenuID equals permissions.MenuID
        //          join tran in dbContext.CmnTransactionTypes.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on lg.MenuID equals tran.MenuID into leftTranGroup
        //          from ltg in leftTranGroup.DefaultIfEmpty()
        //          join userJobContract in dbContext.CmnUserJobContracts.Where(x => x.IsActive == true && x.CompanyID == tb.CompanyID) on loggedUser equals userJobContract.UserID into bgroup
        //          from blj in bgroup.DefaultIfEmpty()
        //          where lg.IsDeleted == false
        //          where lg.MenuName != null && tb.MenuID == lg.ParentID && permissions.EnableView==true
        //          && permissions.CompanyID == companyID
        //          && (permissions.UserID == loggedUser || permissions.UserGroupID == usergroupID)
        //          select new 
        //          {
        //              lg.MenuID,
        //              lg.ParentID,
        //              lg.MenuName,
        //              lg.MenuPath,
        //              lg.MenuIconCss,
        //              lg.Sequence,
        //              permissions.EnableView,
        //              permissions.EnableInsert,
        //              permissions.EnableUpdate,
        //              permissions.EnableDelete, 
        //              TransactionTypeID = (int?)ltg.TransactionTypeID,
        //              TransactionTypeName = ltg.TransactionTypeName,
        //              DepartmentID = blj.DepartmentID
        //          }).Distinct().OrderBy(x => x.Sequence)
        //        }).ToList().OrderBy(x=>x.Sequencea);
        //  //  db = null;
        //    return db;
        //}


    }

}