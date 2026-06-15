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

namespace ABS.Web.Areas.MISREPORT.Reports.SalesComparison
{
    public partial class SalesComparisonByNH : System.Web.UI.Page
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
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string reporttype = queryStrings[2];

                // activity monitoring
                string reportFilteringData = reporttype + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Sales Comparison National Head", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);
                p2.Name = "ToDate";
                p2.Values.Add(toDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);
                               
                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                if (reporttype == "mastergroupbangla")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesComparison/SalesComparisonByNH_MGBangla.rdlc");
                    objCmd.CommandText = "SALES_VARIATIONS.NATIONAL_SALES_COMPARISON";
                }
                if (reporttype == "mastergroup")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesComparison/SalesComparisonByNH_MG.rdlc");
                    objCmd.CommandText = "SALES_VARIATIONS.NATIONAL_SALES_COMPARISON";
                }
                if (reporttype == "productgroup")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesComparison/SalesComparisonByNH_ProdGroup.rdlc");
                    objCmd.CommandText = "SALES_VARIATIONS.NATIONAL_SALES_COMPARISON";
                }
                if (reporttype == "brandwise")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesComparison/SalesComparisonByNH_Brand.rdlc");
                    objCmd.CommandText = "SALES_VARIATIONS.NATIONAL_SALES_COMPARISON";
                }
                if (reporttype == "skuwise")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesComparison/SalesComparisonByNH_SKU.rdlc");
                    objCmd.CommandText = "SALES_VARIATIONS.NATIONAL_SALES_COMPARISON";
                }

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("p_START_DATE", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("p_END_DATE", OracleDbType.Varchar2).Value = toDate;

                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSSalesComparision", dt);
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
        }
    }
}