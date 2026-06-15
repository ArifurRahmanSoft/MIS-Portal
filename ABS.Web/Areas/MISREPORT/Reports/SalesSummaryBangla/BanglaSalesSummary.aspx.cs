using ABS.Web.Utility;
using CTGroup.Utility;
using CTGroup.Utility.Common;
using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABS.Web.Areas.MISREPORT.Reports.SalesSummaryBangla
{
    public partial class BanglaSalesSummary : System.Web.UI.Page
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
                string company = queryStrings[0];
                string location = queryStrings[1];
                string fromDate = queryStrings[2];
                string toDate = queryStrings[3];
                string reporttype = queryStrings[4];

                // activity monitoring
                string reportFilteringData = company + "-" + location + "-" + reporttype + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Product Group-wise Sales", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                if (Session["UserID"].ToString() == "08414")
                    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);
                p2.Name = "ToDate";
                p2.Values.Add(toDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);


                try
                {
                    if (company == "" || company == "undefined")
                        company = null;

                    if (location == "" || location == "undefined")
                        location = null;


                    ReportViewer1.ProcessingMode = ProcessingMode.Local;

                    LocalReport LocalReport = new LocalReport();

                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesSummaryBangla/SalesSummaryBanglaReport.rdlc");

                    DateTime newFromDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime newToDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    
                    DataTable finalDT = new DataTable();
                    DataTable dt = new DataTable();
                    
                    while (newFromDate <= newToDate)
                    {
                        OracleCommand objCmd = new OracleCommand();
                        objCmd.CommandText = "SALES_SUMMARY_BANGLA_TESTING.SelectedItemSales";
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("p_COMPANY", OracleDbType.Varchar2).Value = company;
                        objCmd.Parameters.Add("p_LOCATION", OracleDbType.Varchar2).Value = location;
                        objCmd.Parameters.Add("p_START_DATE", OracleDbType.Varchar2).Value = newFromDate.ToString("dd/MM/yyyy");
                        objCmd.Parameters.Add("p_END_DATE", OracleDbType.Varchar2).Value = newFromDate.ToString("dd/MM/yyyy");
                        dt = classDt.GetData(objCmd);
                        finalDT.Merge(dt);
                        newFromDate = newFromDate.AddDays(1);
                    }



                    ReportDataSource datasource = new ReportDataSource("DTSalesSummaryBangla", finalDT);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });

                    LocalReport.EnableHyperlinks = true;
                    LocalReport.DataSources.Clear();

                    LocalReport.SetParameters(p1);
                    LocalReport.SetParameters(p2);
                    LocalReport.SetParameters(p3);
                    reportActivityMonitoring.RequestEnd(autoId);
                }
                catch(Exception ex)
                {
                    var frame = new StackTrace(true).GetFrame(0);
                    var filename = frame.GetFileName();
                    var line = frame.GetFileLineNumber();

                    Utils u = new Utils();
                    u.LogWrite(ex, filename, line);
                }
            }
        }
    }
}