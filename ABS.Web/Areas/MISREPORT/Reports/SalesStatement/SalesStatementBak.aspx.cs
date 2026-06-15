using ABS.Web.Utility;
using CTGroup.Utility;
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

namespace ABS.Web.Areas.MISREPORT.Reports.SalesStatement
{
    public partial class SalesStatementBak : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string UserName = "";
                string UserId = Session["UserID"].ToString();


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
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string companyId = queryStrings[2];
                string nationalId = queryStrings[3];
                string divisionId = queryStrings[4];
                string regionId = queryStrings[5];
                string zoneId = queryStrings[6];
                string distributorId = queryStrings[7];
                string reportFilter = queryStrings[8];
                string brandId = queryStrings[9];
                string productId = queryStrings[10];
                string reportType = queryStrings[11];

                Utils utility = new Utils();
                if (utility.IsNotValidActivity(UserId, nationalId))
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }

                // activity monitoring
                string reportFilteringData = reportFilter + "-" + companyId + "-" + brandId + "-" + productId 
                                             + "-" + reportType + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Sales Statment", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                //if (Session["UserID"].ToString() == "08414") // rashed sir logic, stopped now, as team member increased
                //    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();
                ReportParameter p4 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);
                p2.Name = "ToDate";
                p2.Values.Add(toDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);

                p4.Name = "ReportFilter";
                p4.Values.Add(reportFilter);

                if (companyId == "")
                    companyId = null;

                if (nationalId == "")
                    nationalId = null;

                if (divisionId == "")
                    divisionId = null;

                if (regionId == "")
                    regionId = null;

                if (zoneId == "")
                    zoneId = null;

                if (distributorId == "")
                    distributorId = null;

                if (brandId == "")
                    brandId = null;

                if (productId == "")
                    productId = null;

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                LocalReport LocalReport = new LocalReport();
                OracleCommand objCmd = new OracleCommand();           

                if (reportType == "details_sales_statement")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatement/DetailsSalesStatementRDLC.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "pack_sales_state2_bct.SALES_STATEMENT";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "pack_sales_state2_jmw.SALES_STATEMENT";
                    }
                    else
                    {
                        objCmd.CommandText = "pack_sales_state2.SALES_STATEMENT";
                    }
                }
                if (reportType == "summary_sales_statement")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatement/SummarySalesStatementRDLC.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "pack_sales_state2_bct.SALES_STATEMENT";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "pack_sales_state2_jmw.SALES_STATEMENT";
                    }
                    else
                    {
                        objCmd.CommandText = "pack_sales_state2.SALES_STATEMENT";
                    }
                }
                if (reportType == "item_wise_summary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatement/ItemWiseSummaryRDLC.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "pack_sales_state2_bct.sales_statement_item_wise_summary";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "pack_sales_state2_jmw.sales_statement_item_wise_summary";
                    }
                    else
                    {
                        objCmd.CommandText = "pack_sales_state2.sales_statement_item_wise_summary";
                    }
                }
                if (reportType == "party_wise_qnty_summary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatement/PartyWiseQntySummaryRDLC.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "pack_sales_state2_bct.SALES_STATEMENT";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "pack_sales_state2_jmw.SALES_STATEMENT";
                    }
                    else
                    {
                        objCmd.CommandText = "pack_sales_state2.SALES_STATEMENT";
                    }
                }
                if (reportType == "Party_wise_Summary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatement/PartyWiseSummaryRDLC.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "pack_sales_state2_bct.SALES_STATEMENT";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "pack_sales_state2_jmw.SALES_STATEMENT";
                    }
                    else
                    {
                        objCmd.CommandText = "pack_sales_state2.SALES_STATEMENT";
                    }
                }
                if (reportType == "Sales_Statement_Ton")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatement/SalesStatementTonRDLC.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "pack_sales_state2_bct.SALES_STATEMENT_WITH_TON";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "pack_sales_state2_jmw.SALES_STATEMENT_WITH_TON";
                    }
                    else
                    {
                        objCmd.CommandText = "pack_sales_state2.SALES_STATEMENT_WITH_TON";
                    }
                }

                objCmd.CommandType = CommandType.StoredProcedure;

                if (reportType == "Sales_Statement_Ton")
                {
                    objCmd.Parameters.Add("fresult", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("comp", OracleDbType.Varchar2).Value = companyId;
                    objCmd.Parameters.Add("cust", OracleDbType.Varchar2).Value = distributorId;
                    objCmd.Parameters.Add("prod", OracleDbType.Varchar2).Value = productId;
                    objCmd.Parameters.Add("dloc", OracleDbType.Varchar2).Value = null;
                    objCmd.Parameters.Add("fdat", OracleDbType.Varchar2).Value = fromDate;
                    objCmd.Parameters.Add("tdat", OracleDbType.Varchar2).Value = toDate;
                    objCmd.Parameters.Add("companyId", OracleDbType.Varchar2).Value = companyId;
                    objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                    objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                    objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                    objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                    objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                    objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                    objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                }
                else
                {
                    objCmd.Parameters.Add("fresult", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("fdat", OracleDbType.Varchar2).Value = fromDate;
                    objCmd.Parameters.Add("tdat", OracleDbType.Varchar2).Value = toDate;
                    objCmd.Parameters.Add("companyId", OracleDbType.Varchar2).Value = companyId;
                    objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                    objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                    objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                    objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                    objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                    objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                    objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                    objCmd.Parameters.Add("dloc", OracleDbType.Varchar2).Value = null;
                }

                DataTable dt = classDt.GetData(objCmd);

                if (reportType == "Sales_Statement_Ton")
                {
                    ReportDataSource datasource = new ReportDataSource("SalesStatementTonDDS", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }
                else
                {
                    ReportDataSource datasource = new ReportDataSource("SalesStatementDDS", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                LocalReport.SetParameters(p4);
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}