using ABS.Web.Utility;
using CTGroup.Utility.Common;
using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABS.Web.Areas.INVENTORY.Reports.ItemStoreLedgerReport
{
    public partial class ItemStoreLedgerReport : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string UserName = "";

                if (Session["UserFullName"] == null || Session["UserFullName"] == "undefined")
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                                       //Response.Redirect("");
                                       //Server.Transfer("http://localhost:15812/Account/Login");
                                       //Response.Redirect("http://localhost:15812/Account/Login");
                    Response.Redirect("~/Account/Login");
                }

                UserName = Session["UserFullName"].ToString();

                string[] queryStrings = Request["queryString"].ToString().Split(',');

                string FromDate    = queryStrings[0];
                string ToDate      = queryStrings[1];
                string reportType  = queryStrings[2];
                string productID   = queryStrings[3];
                string SupplierID  = queryStrings[4];
                string CompanyUnit = queryStrings[5];
                string ItemGroup   = queryStrings[6];

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(FromDate);
                p2.Name = "ToDate";
                p2.Values.Add(ToDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);

                if (productID == "" || productID == "Undefine")
                    productID = null;

                if (CompanyUnit == "" || CompanyUnit == "Undefine")
                    CompanyUnit = null;

                if (ItemGroup == "" || ItemGroup == "Undefine")
                    ItemGroup = null;

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                if (reportType == "consummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/ItemStoreLedgerReport/ItemStoreLedgerUnitSummary.rdlc");
                    objCmd.CommandText = "p_reports.get_item_ledger_summary_new";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_wh_id", OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_cunit_id", OracleDbType.Varchar2).Value = CompanyUnit;
                    objCmd.Parameters.Add("p_itm_id", OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_grp_id", OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_frm_dt", OracleDbType.Varchar2).Value = FromDate;
                    objCmd.Parameters.Add("p_to_dt", OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_user", OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_user_id", OracleDbType.Varchar2).Value = "";
                }
                if (reportType == "condetails")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/ItemStoreLedgerReport/ItemStoreLedgerUnitDetails.rdlc");
                    objCmd.CommandText = "p_reports.get_item_ledger_new2_test";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur",      OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_wh_id",    OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_cunit_id", OracleDbType.Varchar2).Value = CompanyUnit;
                    objCmd.Parameters.Add("p_itm_id",   OracleDbType.Varchar2).Value = productID;
                    objCmd.Parameters.Add("p_grp_id",   OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_frm_dt",   OracleDbType.Varchar2).Value = FromDate;
                    objCmd.Parameters.Add("p_to_dt",    OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_user",     OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_user_id",  OracleDbType.Varchar2).Value = "";
                }
                if (reportType == "itemsummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/ItemStoreLedgerReport/ItemStoreLedgerItemWiseSummary.rdlc");
                    objCmd.CommandText = "p_reports.get_item_ledger_summary_new";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur",      OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_wh_id",    OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_cunit_id", OracleDbType.Varchar2).Value = CompanyUnit;
                    objCmd.Parameters.Add("p_itm_id",   OracleDbType.Varchar2).Value = productID;
                    objCmd.Parameters.Add("p_grp_id",   OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_frm_dt",   OracleDbType.Varchar2).Value = FromDate;
                    objCmd.Parameters.Add("p_to_dt",    OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_user",     OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_user_id",  OracleDbType.Varchar2).Value = "";
                }
                if (reportType == "itemdetails")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/ItemStoreLedgerReport/ItemStoreLedgerItemWiseDetails.rdlc");
                    objCmd.CommandText = "p_reports.get_item_ledger_new2_test";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur",      OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_wh_id",    OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_cunit_id", OracleDbType.Varchar2).Value = CompanyUnit;
                    objCmd.Parameters.Add("p_itm_id",   OracleDbType.Varchar2).Value = productID;
                    objCmd.Parameters.Add("p_grp_id",   OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_frm_dt",   OracleDbType.Varchar2).Value = FromDate;
                    objCmd.Parameters.Add("p_to_dt",    OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_user",     OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_user_id",  OracleDbType.Varchar2).Value = "";
                }

                if (reportType == "itemcusummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/ItemStoreLedgerReport/ItemStoreLedgerAllCUsummary.rdlc");
                    objCmd.CommandText = "p_reports.get_item_ledger_new2_test";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur",      OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_wh_id",    OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_cunit_id", OracleDbType.Varchar2).Value = CompanyUnit;
                    objCmd.Parameters.Add("p_itm_id",   OracleDbType.Varchar2).Value = productID;
                    objCmd.Parameters.Add("p_grp_id",   OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_frm_dt",   OracleDbType.Varchar2).Value = FromDate;
                    objCmd.Parameters.Add("p_to_dt",    OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_user",     OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_user_id",  OracleDbType.Varchar2).Value = "";
                }

                if (reportType == "itemdcudetails")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/ItemStoreLedgerReport/ItemStoreLedgerAllCUDetails.rdlc");
                    objCmd.CommandText = "p_reports.get_item_ledger_new2_test";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur",      OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_wh_id",    OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_cunit_id", OracleDbType.Varchar2).Value = CompanyUnit;
                    objCmd.Parameters.Add("p_itm_id",   OracleDbType.Varchar2).Value = productID;
                    objCmd.Parameters.Add("p_grp_id",   OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_frm_dt",   OracleDbType.Varchar2).Value = FromDate;
                    objCmd.Parameters.Add("p_to_dt",    OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_user",     OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_user_id",  OracleDbType.Varchar2).Value = "";
                }
                if (reportType == "itemstocksummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/ItemStoreLedgerReport/ItemStoreLedgerItemStockSummary.rdlc");
                    objCmd.CommandText = "p_reports.get_item_ledger_stock_summ";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur",      OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_callname", OracleDbType.Varchar2).Value = "summ";
                    objCmd.Parameters.Add("p_grp_id",   OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_frm_dt",   OracleDbType.Varchar2).Value = FromDate;
                    objCmd.Parameters.Add("p_to_dt",    OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_user",     OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_user_id",  OracleDbType.Varchar2).Value = "";
                }
                if (reportType == "itemstockDetails")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/ItemStoreLedgerReport/ItemStoreLedgerItemStockDetails.rdlc");
                    objCmd.CommandText = "p_reports.get_item_ledger_stock_summ";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur",      OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_callname", OracleDbType.Varchar2).Value = "details";
                    objCmd.Parameters.Add("p_grp_id",   OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_frm_dt",   OracleDbType.Varchar2).Value = FromDate;
                    objCmd.Parameters.Add("p_to_dt",    OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_user",     OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_user_id",  OracleDbType.Varchar2).Value = ""; 
                }
                if (reportType == "combinedItem")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/ItemStoreLedgerReport/ItemStoreCombineStockItem.rdlc");
                    objCmd.CommandText = "p_stock_balance.get_item_stock_crosstab";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur",          OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("trn_dt",         OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_wh_id",        OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_cu_id",        OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_itmgrp_id",    OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_item_id",      OracleDbType.Varchar2).Value = productID;
                    objCmd.Parameters.Add("p_loc_id",       OracleDbType.Int16).Value = 12;
                    objCmd.Parameters.Add("p_user",         OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_itmname_like", OracleDbType.Varchar2).Value = "";  
                }
                if (reportType == "combinedIssue")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/ItemStoreLedgerReport/ItemStoreCombineIssueItem.rdlc");
                    objCmd.CommandText = "p_stock_balance.get_item_crosstab_report";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur",          OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("callname",       OracleDbType.Varchar2).Value = "issue_rpt";
                    objCmd.Parameters.Add("frm_dt",         OracleDbType.Varchar2).Value = FromDate;
                    objCmd.Parameters.Add("to_dt",          OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_wh_id",        OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_cu_id",        OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_itmgrp_id",    OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_item_id",      OracleDbType.Varchar2).Value = productID;
                    objCmd.Parameters.Add("p_loc_id",       OracleDbType.Int16).Value = 12;
                    objCmd.Parameters.Add("p_user",         OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_itmname_like", OracleDbType.Varchar2).Value = "";
                }


                DataTable dt = classDt.GetSCMData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSItemStoreLedger", dt);
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
            }
        }
    }
}