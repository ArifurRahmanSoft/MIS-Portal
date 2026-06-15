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

namespace ABS.Web.Areas.MISREPORT.Reports.ProductionDemand
{
    public partial class ProductionDemand : System.Web.UI.Page
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
                string reportType = queryStrings[0];
                string salesCompanyId = queryStrings[1];
                string productCompanyId = queryStrings[2];
                string brandId = queryStrings[3];
                string fromDate = queryStrings[4];
                string toDate = queryStrings[5];
               
 
                // activity monitoring
               // string reportFilteringData = reportFilter + "-" + reportType+ "-" + fromDate + "-" + toDate;
               // ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
               // string autoId = reportActivityMonitoring.RequestStart("Daily Sales Summary", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                //if (Session["UserID"].ToString() == "08414")
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
                //p4.Name = "ReportFilter";
                //p4.Values.Add(reportFilter);

                if (salesCompanyId == "" || salesCompanyId == "undefined")
                    salesCompanyId = null;

                if (brandId == "" || brandId == "undefined")
                    brandId = null;

                if (productCompanyId == "" || productCompanyId == "undefined")
                    productCompanyId = null;
                
                

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

       
                if (reportType == "undelivered_statement_details")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ProductionDemand/UndeliveredStatementDetails.rdlc");
                    objCmd.CommandText = "PRODUCTION_DEMAND.UNDELIVERY_SUMMARY_DETAIL";
                }
                if (reportType == "undelivered_statement_summary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ProductionDemand/UndeliveredStatementSummary.rdlc");
                    objCmd.CommandText = "PRODUCTION_DEMAND.UNDELIVERY_SUMMARY_DETAIL";
                }
            

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("fresult", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("fdat", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("tdat", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("p_salesCompany", OracleDbType.Varchar2).Value = salesCompanyId;
                objCmd.Parameters.Add("p_productCompany", OracleDbType.Varchar2).Value = productCompanyId;
                objCmd.Parameters.Add("p_brandId", OracleDbType.Varchar2).Value = brandId;
          

                DataTable dt = classDt.GetData(objCmd);

        
                if (reportType == "undelivered_statement_details")
                {
                    ReportDataSource datasource = new ReportDataSource("DTUndeldProductionDemand", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }
                if (reportType == "undelivered_statement_summary")
                {
                    ReportDataSource datasource = new ReportDataSource("DTUndeldProductionDemand", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);

                //reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}