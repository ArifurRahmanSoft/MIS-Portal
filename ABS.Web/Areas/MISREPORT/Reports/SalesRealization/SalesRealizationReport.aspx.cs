using CTGroup.Utility.Common;
using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Web.Utility;

namespace ABS.Web.Areas.MISREPORT.Reports.SalesRealization
{
    public partial class SalesRealizationReport : System.Web.UI.Page
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
                string customer = queryStrings[1];
                string fromDate = queryStrings[2];
                string toDate = queryStrings[3];

                // activity monitoring
                string reportFilteringData = company + "-" + customer + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Sales Realization Report", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

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


                if (company == "" || company == "undefined")
                    company = null;

                if (customer == "" || company == "undefined")
                    company = null;

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();


                LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesRealization/SalesRealizationReport.rdlc");
                objCmd.CommandText = "pack_salesrealiz.getdata";


                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("p_COMPANY", OracleDbType.Varchar2).Value = company;
                objCmd.Parameters.Add("p_CUSTOMER", OracleDbType.Varchar2).Value = customer;
                objCmd.Parameters.Add("p_START_DATE", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("p_END_DATE", OracleDbType.Varchar2).Value = toDate;

                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSSalesRealizationReport", dt);
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