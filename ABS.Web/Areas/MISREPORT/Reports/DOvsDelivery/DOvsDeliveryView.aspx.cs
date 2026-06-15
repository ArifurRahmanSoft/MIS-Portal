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

namespace ABS.Web.Areas.MISREPORT.Reports.DOvsDelivery
{
    public partial class DOvsDeliveryView : System.Web.UI.Page
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
                string distributor = queryStrings[1];
                string fromDate = queryStrings[2];
                string toDate = queryStrings[3];
                string reportType = queryStrings[4];

                // activity monitoring
                string reportFilteringData = reportType + "-" + company + "-" + distributor + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("DO VS Delivery", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

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

                if (distributor == "" || distributor == "undefined")
                    distributor = null;

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();
                LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DOvsDelivery/DOVSDelivery.rdlc");

                if (reportType == "1")
                    objCmd.CommandText = "DOvsDelivery.getdataDOBased";

                if (reportType == "2")
                    objCmd.CommandText = "DOvsDelivery.getdatachallanBased";

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("company", OracleDbType.Varchar2).Value = company;// ; // 
                objCmd.Parameters.Add("distributor", OracleDbType.Varchar2).Value = distributor;//; // distributor
                objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate; // from date

                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSDOVSDelivery", dt);
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