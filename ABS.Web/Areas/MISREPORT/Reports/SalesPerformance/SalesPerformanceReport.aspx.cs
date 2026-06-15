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

namespace ABS.Web.Areas.MISREPORT.Reports.SalesPerformance
{
    public partial class SalesPerformanceReport : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
        ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
        string autoId = string.Empty;
        string reportFilteringData = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserFullName"] == null || Session["UserFullName"] == "undefined")
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                                       //Response.Redirect("");
                                       //Server.Transfer("http://localhost:15812/Account/Login");
                                       //Response.Redirect("http://localhost:15812/Account/Login");
                    Response.Redirect("~/Account/Login");
                }

                string UserId = Session["UserID"].ToString();
                string UserName = "";
                UserName = Session["UserFullName"].ToString();

                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string nationalId = queryStrings[2];
                string divisionId = queryStrings[3];
                string regionId = queryStrings[4];
                string zoneId = queryStrings[5];
                string distributorId = queryStrings[6];
                string reportFilter = queryStrings[7];
                string reportType = queryStrings[8];
                string reportOption = queryStrings[9];

                Utils utility = new Utils();
                if (utility.IsNotValidActivity(UserId, nationalId))
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }

                reportFilteringData = reportFilter + "-" + reportType + "-" + reportOption + "-" + fromDate + "-" + toDate;

                if (Session["UserID"].ToString() == "08414")
                    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

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

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                if (reportOption == "mto")
                {
                    if (reportType == "1")   // Summary Report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Summary");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceSummaryMTon.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_SUMMARY";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_SUMMARY";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_SUMMARY";
                        }
                    }
                    else     // detail report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Detail");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceDetailMTon.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_DETAIL";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_DETAIL";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_DETAIL";
                        }
                    }
                }

                if (reportOption == "mtoWithFree")
                {
                    if (reportType == "1")   // Summary Report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Summary");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceSummaryMTonWithFree.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_SUMMARY_FREE_PCS";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_SUMMARY_FREE_PCS";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_SUMMARY_FREE_PCS";
                        }
                    }
                    else     // detail report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Detail");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceDetailMTonWithFree.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_DETAIL_FREE_PCS";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_DETAIL_FREE_PCS";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_DETAIL_FREE_PCS";
                        }
                    }
                }

                if(reportOption== "soPerform")
                {
                    //autoId = WritingReportStartActivity("Sales Performance Detail");
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesOfficersPerformanceDetailMTonWithFree.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_DETAIL_FREE_PCS";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_DETAIL_FREE_PCS";
                    //}
                    //else
                    //{
                        objCmd.CommandText = "SALES_PERFOR_AREA.SALES_OFFICERS_PERFORMANCE";
                    //}
                }

                if (reportOption == "ctn_sack")
                {
                    if (reportType == "1")   // Summary Report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Summary");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceSummaryCtnSack.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_SUMMARY";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_SUMMARY";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_SUMMARY";
                        }
                    }
                    else     // detail report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Detail");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceDetailCtnSack.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_DETAIL";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_DETAIL";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_DETAIL";
                        }
                    }
                }

                if (reportOption == "ctn_sack_With_Free") 
                {
                    if (reportType == "1")   // Summary Report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Summary");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceSummaryCtnSackWithFree.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_SUMMARY_FREE_PCS";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_SUMMARY_FREE_PCS";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_SUMMARY_FREE_PCS";
                        }
                    }
                    else     // detail report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Detail");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceDetailCtnSackWithFree.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_DETAIL_FREE_PCS";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_DETAIL_FREE_PCS";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_DETAIL_FREE_PCS";
                        }
                    }
                }

                if (reportOption == "sku_pcs") 
                {
                    if (reportType == "1")   // Summary Report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Summary");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceSummarySKU_pcs.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_SUMMARY";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_SUMMARY";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_SUMMARY";
                        }
                    }
                    else     // detail report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Detail");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceDetailSKU_pcs.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_DETAIL";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_DETAIL";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_DETAIL";
                        }
                    }
                }

                if (reportOption == "SkuWithFree_pcs")
                {
                    if (reportType == "1")   // Summary Report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Summary");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceSummarySKUfree_pcs.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_SUMMARY_FREE_PCS";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_SUMMARY_FREE_PCS";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_SUMMARY_FREE_PCS";
                        }
                    }
                    else     // detail report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Detail");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceDetailSKUFree_pcs.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_DETAIL_FREE_PCS";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_DETAIL_FREE_PCS";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_DETAIL_FREE_PCS";
                        }
                    }
                }

                if (reportOption == "sku_mton")
                {
                    if (reportType == "1")   // Summary Report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Summary");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceSummarySKU_mton.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_SUMMARY";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_SUMMARY";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_SUMMARY";
                        }
                    }
                    else     // detail report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Detail");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceDetailSKU_mton.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_DETAIL";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_DETAIL";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_DETAIL";
                        }
                    }
                }

                if (reportOption == "SkuWithFree_mton")
                {
                    if (reportType == "1")   // Summary Report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Summary");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceSummarySKUfree_mton.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_SUMMARY_FREE_PCS";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_SUMMARY_FREE_PCS";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_SUMMARY_FREE_PCS";
                        }
                    }
                    else     // detail report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Detail");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceDetailSKUFree_mton.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_DETAIL_FREE_PCS";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_DETAIL_FREE_PCS";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_DETAIL_FREE_PCS";
                        }
                    }
                }

                if (reportOption == "onlyQtyAmount")
                {
                    if (reportType == "1")   // Summary Report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Summary");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceSummaryOnlyQtyAmount.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_SUMMARY";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_SUMMARY";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_SUMMARY";
                        }
                    }
                    else     // detail report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Detail");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/New_SalesPerformanceDetailOnlyQtyAmount.rdlc");
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_BCT.AREA_DETAIL";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA_JMW.AREA_DETAIL";
                        }
                        else
                        {
                            objCmd.CommandText = "SALES_PERFOR_AREA.AREA_DETAIL";
                        }
                    }
                }

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;

                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("New_SalesPerformanceDS", dt);
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });

                //ReportViewer1.ShowPrintButton = false;
                //ReportViewer1.ShowExportControls = false;

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
        private string WritingReportStartActivity(string reportName)
        {
            string autoId = reportActivityMonitoring.RequestStart(reportName, reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());
            return autoId;
        }
    }
}