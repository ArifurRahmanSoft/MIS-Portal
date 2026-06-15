using CTGroup.Data.BaseFactories;
using CTGroup.Data.BaseInterfaces;
using CTGroup.Models;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Service.SystemCommon.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTGroup.Service.AllServiceClasses;
using CTGroup.Utility;
using CTGroup.OracleModel;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using CTGroup.Utility.Common;

namespace CTGroup.Service.SystemCommon.Factories
{    
    public class CmnMenuMgt : iCmnMenuMgt
    {
        OracleCommand cmd = new OracleCommand();
        private iGenericFactory<vmCmnMenu> GenericFactoryFor_Menu = null;
        private iGenericFactory_EF<CmnCompany> GenericFactoryFor_Company = null;
        private iGenericFactory_EF<T_CMNMODULE> GenericFactoryFor_Module = null;

        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        /// 
        public List<T_CMNMODULE> GetModuleOnDemand()
        {
            GenericFactoryFor_Module = new CmnModule_EF();
            List<T_CMNMODULE> objModuleList = null;
            string spQuery = string.Empty;
            try
            {
                //objCustomers = GenericFactoryFor_ProductOutlet.GetAll();
                var module = GenericFactoryFor_Module.GetAll();
                objModuleList = (from olt in module
                                 orderby olt.MODULEID descending
                                 select new
                                 {
                                     ModuleID = olt.MODULEID,
                                     ModuleName = olt.MODULENAME

                                 }).ToList().Select(x => new T_CMNMODULE
                                 {
                                     MODULEID = x.ModuleID,
                                     MODULENAME = x.ModuleName

                                 }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objModuleList.OrderBy(x => x.MODULEID).ToList();
        }

        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        /// 


        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public IEnumerable<vmCmnMenu> GetMenues(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Menu = new vmCmnMenu_GF();
            IEnumerable<vmCmnMenu> objMenues = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);

                spQuery = "[Get_CmnMenu]";
                objMenues = GenericFactoryFor_Menu.ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objMenues;
        }

        public DataTable GetMenueByPage(vmCmnParameters cparam)
        {
            cmd = new OracleCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SETTINGS.Get_CmnMenuByPage";
            cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = cparam.pageNumber;
            cmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = cparam.pageSize;
            cmd.Parameters.Add("IsPaging", OracleDbType.Decimal).Value = cparam.IsPaging;
            cmd.Parameters.Add("SearchValue", OracleDbType.Varchar2).Value = cparam.searchItemName;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(cmd);

            return dt;
        }

        public DataTable GetMenuByID(vmCmnParameters cparam)
        {
            cmd = new OracleCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SETTINGS.Get_CmnMenuById";
            cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("MenuID", OracleDbType.Decimal).Value = cparam.id;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(cmd);

            return dt;
        }

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive single data through a stored procedure</para>
        /// </summary>
        public vmCmnMenu GetMenuByID(int? id)
        {
            GenericFactoryFor_Menu = new vmCmnMenu_GF();
            vmCmnMenu objMenu = new vmCmnMenu();
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("MenuID", id);
                spQuery = "[Get_CmnMenuSingle]";
                objMenu = GenericFactoryFor_Menu.ExecuteQuerySingle(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objMenu;
        }

        /// Static ID Provided
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int SaveMenu(T_CMNMENU model)
        {
            GenericFactoryFor_Menu = new vmCmnMenu_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("MenuID", model.MENUID);
                ht.Add("CustomCode", model.CUSTOMCODE);
                ht.Add("MenuName", model.MENUNAME);
                ht.Add("ModuleID", model.MODULEID);
                ht.Add("MenuPath", model.MENUPATH);
                ht.Add("ReportName", model.REPORTNAME);
                ht.Add("ReportPath", model.REPORTPATH);
                ht.Add("ParentID", model.PARENTID);
                ht.Add("Sequence", model.SEQUENCE);
                ht.Add("MenuTypeID", model.MENUTYPEID);
                ht.Add("StatusID", model.STATUSID);
                ht.Add("MenuIconCss", model.MENUICONCSS);
                ht.Add("CompanyID", model.COMPANYID);
                ht.Add("CreateBy", 1);
                ht.Add("CreateOn", DateTime.Now);
                ht.Add("CreatePc", "1.1");
                ht.Add("UpdateBy", model.UPDATEBY);
                ht.Add("UpdateOn", model.UPDATEON);
                ht.Add("UpdatePc", model.UPDATEPC);
                ht.Add("IsDeleted", model.ISDELETED);
                ht.Add("DeleteBy", model.DELETEBY);
                ht.Add("DeleteOn", model.DELETEON);
                ht.Add("DeletePc", model.DELETEPC);

                string spQuery = "[Set_CmnMenu]";
                result = GenericFactoryFor_Menu.ExecuteCommand(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        /// Static ID Provided
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>
        public int UpdateMenu(T_CMNMENU model)
        {
            GenericFactoryFor_Menu = new vmCmnMenu_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("MenuID", model.MENUID);
                ht.Add("CustomCode", model.CUSTOMCODE);
                ht.Add("MenuName", model.MENUNAME);
                ht.Add("ModuleID", model.MODULEID);
                ht.Add("MenuPath", model.MENUPATH);
                ht.Add("ReportName", model.REPORTNAME);
                ht.Add("ReportPath", model.REPORTPATH);
                ht.Add("ParentID", model.PARENTID);
                ht.Add("Sequence", model.SEQUENCE);
                ht.Add("MenuTypeID", model.MENUTYPEID);
                ht.Add("StatusID", model.STATUSID);
                ht.Add("MenuIconCss", model.MENUICONCSS);
                ht.Add("CompanyID", model.COMPANYID);
                ht.Add("CreateBy", 1);
                ht.Add("CreateOn", DateTime.Now);
                ht.Add("CreatePc", "1.1");
                ht.Add("UpdateBy", model.UPDATEBY);
                ht.Add("UpdateOn", model.UPDATEON);
                ht.Add("UpdatePc", model.UPDATEPC);
                ht.Add("IsDeleted", model.ISDELETED);
                ht.Add("DeleteBy", model.DELETEBY);
                ht.Add("DeleteOn", model.DELETEON);
                ht.Add("DeletePc", model.DELETEPC);
                string spQuery = "[Put_CmnMenu]";
                result = GenericFactoryFor_Menu.ExecuteCommand(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }

        /// <summary>
        /// Update Delete From Database
        /// <para>Use it when delete data through a stored procedure</para>
        /// </summary>
        public int DeleteMenu(int? MenuID)
        {
            GenericFactoryFor_Menu = new vmCmnMenu_GF();
            int result = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("MenuID", MenuID);

                string spQuery = "[Delete_CmnMenu]";
                result = GenericFactoryFor_Menu.ExecuteCommand(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }
    }
}
