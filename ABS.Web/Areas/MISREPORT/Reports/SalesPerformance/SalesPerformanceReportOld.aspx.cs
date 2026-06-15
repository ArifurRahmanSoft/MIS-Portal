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
    public partial class SalesPerformanceReportOld : System.Web.UI.Page
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

                //if(productType == "SDVNTxxxxxxxxxxx1001") // option for pack data
                
                // sales performance report MTO, CTN/SACK, SKU
                if (reportOption == "mto")
                    {
                    if (nationalId == "SNTNLxxxxxxxxxN00001") // option for pack data
                    {
                        if (reportType == "1")   // Summary Report
                        {
                            autoId = WritingReportStartActivity("Sales Performance Summary for CONSUMER PACK");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceSummary.rdlc");
                            objCmd.CommandText = "sales_performance.get_sales_performance";
                        }
                        else     // detail report
                        {
                            autoId = WritingReportStartActivity("Sales Performance Detail for CONSUMER PACK");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceDetail.rdlc");
                            objCmd.CommandText = "sales_performance.get_sales_performance";
                        }
                    }

                    if (nationalId == "SNTNLxxxxxxxxxN00002")  // option for poultry feed
                    {
                        if (reportType == "1")    // Summary Report
                        {
                            autoId = WritingReportStartActivity("Sales Performance Summary for poultry feed");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformancePFSummary.rdlc");
                            objCmd.CommandText = "sales_performance.get_poultry_sales_performance";
                        }
                        else      // detail report
                        {
                            autoId = WritingReportStartActivity("Sales Performance detail for poultry feed");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformancePFDetail.rdlc");
                            objCmd.CommandText = "sales_performance.get_poultry_sales_performance";
                        }
                    }

                    if (nationalId == "SNTNLxxxxxxxxxN00007")  // option for sun
                    {
                        if (reportType == "1")    // Summary Report
                        {
                            autoId = WritingReportStartActivity("Sales Performance Summary for sun national");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceSUNSummary.rdlc");
                            objCmd.CommandText = "sales_performance.get_SUN_sales_performance";
                        }
                        else      // detail report
                        {
                            autoId = WritingReportStartActivity("Sales Performance Detail for sun national");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceSUNDetail.rdlc");
                            objCmd.CommandText = "sales_performance.get_SUN_sales_performance";
                        }
                    }

                    if (nationalId == "SNTNLxxxxxxxxxN00006")  // option for Bulk
                    {
                        if (reportType == "1")    // Summary Report
                        {
                            autoId = WritingReportStartActivity("Sales Performance Summary for bulk national");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceBULKSummary.rdlc");
                            objCmd.CommandText = "sales_performance.get_BULK_sales_performance";
                        }
                        else      // detail report
                        {
                            autoId = WritingReportStartActivity("Sales Performance Detail for bulk national");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceBULKDetail.rdlc");
                            objCmd.CommandText = "sales_performance.get_BULK_sales_performance";
                        }
                    }
                    if (nationalId == "SNTNLxxxxxxxxxN00009")  // option for Modern Trade Sales
                    {
                        if (reportType == "1")    // Summary Report
                        {
                            autoId = WritingReportStartActivity("Sales Performance Summary for modern trade national");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceMTSSummary.rdlc");
                            objCmd.CommandText = "sales_performance.get_modern_trade_sales";
                        }
                        else      // detail report
                        {
                            autoId = WritingReportStartActivity("Sales Performance Detail for modern trade national");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceMTSDetail.rdlc");
                            objCmd.CommandText = "sales_performance.get_modern_trade_sales";
                        }
                    }
                    if (nationalId == "SNTNLxxxxxxxxxN00004")  // option for Corporate National Sales
                    {
                        if (reportType == "1")    // Summary Report
                        {
                            autoId = WritingReportStartActivity("Sales Performance Summary for corportae national");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceCNSSummary.rdlc");
                            objCmd.CommandText = "sales_performance.get_corporate_national_sales";
                        }
                        else      // detail report
                        {
                            autoId = WritingReportStartActivity("Sales Performance Detail for corportae national");
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceCNSDetail.rdlc");
                            objCmd.CommandText = "sales_performance.get_corporate_national_sales";
                        }
                    }

                }
       
                if (reportOption == "ctn_sack") // Summary Report
                {
                    if (reportType == "1")   // Summary Report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Summary");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceCTNSackSummary.rdlc");
                        objCmd.CommandText = "sales_performance.get_sales_performance_ctn_sku";
                    }
                    else     // detail report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Detail");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceCTNSackDetail.rdlc");
                        objCmd.CommandText = "sales_performance.get_sales_performance_ctn_sku";
                    }
                }

                if (reportOption == "sku") // Summary Report
                {
                    if (reportType == "1")   // Summary Report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Summary");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceSKUSummary.rdlc");
                        objCmd.CommandText = "sales_performance.get_sales_performance_ctn_sku";
                    }
                    else     // detail report
                    {
                        autoId = WritingReportStartActivity("Sales Performance Detail");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesPerformance/SalesPerformanceSKUDetail.rdlc");
                        objCmd.CommandText = "sales_performance.get_sales_performance_ctn_sku";
                    }                    
                }

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("fresult", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("fdat", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("tdat", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;

                DataTable dt = classDt.GetData(objCmd);

                //if (productType == "SDVNTxxxxxxxxxxx1001") // report for pack
                if (nationalId == "SNTNLxxxxxxxxxN00001") // option for pack data
                {
                    ReportDataSource datasource = new ReportDataSource("SalesPerformanceDT", dt);
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                }

                // if (productType == "SDVNTxxxxxxxxxxx1002") // report for poultry feed
                if (nationalId == "SNTNLxxxxxxxxxN00002")  // option for poultry feed
                {
                    ReportDataSource datasource = new ReportDataSource("DSTPoultryFeed", dt);
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                }

                //   if (productType == "SDVNTxxxxxxxxxxx1005") // report for sun
                if (nationalId == "SNTNLxxxxxxxxxN00007")  // option for sun
                {
                    ReportDataSource datasource = new ReportDataSource("SalesPerformanceSUNDT", dt);
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                }
                // if (productType == "SDVNTxxxxxxxxxxx1004") // report for BULK
                if (nationalId == "SNTNLxxxxxxxxxN00006")  // option for Bulk
                {
                    ReportDataSource datasource = new ReportDataSource("DSSSalesPerformanceBULK", dt);
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                }

                if (nationalId == "SNTNLxxxxxxxxxN00009")  // option for Modern Trade Sales
                {
                    ReportDataSource datasource = new ReportDataSource("DSSNationalModernTradeSales", dt);
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                }
                if (nationalId == "SNTNLxxxxxxxxxN00004")  // option for Corporate  Sales
                {
                    ReportDataSource datasource = new ReportDataSource("DSSNationalModernTradeSales", dt);
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                }

                // sales performance report MTO, CTN/SACK, SKU
                if (reportOption == "mto")
                {
                    ReportDataSource datasource = new ReportDataSource("DSSSalesPerformanceBULK", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportOption == "ctn_sack")
                {
                    ReportDataSource datasource = new ReportDataSource("DSSSalesPerformanceBULK", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportOption == "sku")
                {
                    ReportDataSource datasource = new ReportDataSource("DSSSalesPerformanceBULK", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });

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