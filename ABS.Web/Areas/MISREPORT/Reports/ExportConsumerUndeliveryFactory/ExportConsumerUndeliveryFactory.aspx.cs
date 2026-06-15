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

namespace ABS.Web.Areas.MISREPORT.Reports.ExportConsumerUndeliveryFactory
{
    public partial class ExportConsumerUndeliveryFactory : System.Web.UI.Page
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
          
                string distributorId = queryStrings[0];
                string productID = queryStrings[1];
                string transportID = queryStrings[2];
                string fromDate = queryStrings[3];
                string toDate = queryStrings[4];
                string requistionNo = queryStrings[5];
                string company = queryStrings[6];
                string loggeduser = queryStrings[7];
                string sardarId = queryStrings[8];
                string locationId = queryStrings[9];
                string reportType = queryStrings[10];

                // activity monitoring
                string reportFilteringData = distributorId + "-" + productID + "-" + transportID + "-" + requistionNo + "-" + company + "-" + loggeduser + "-" + sardarId + "-" + locationId
                    + "-" + fromDate + "-" + toDate + "-" + reportType;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Customer-wise Delivery", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

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

     
                if (distributorId == "" || distributorId == "undefined")
                    distributorId = null;

                if (productID == "" || productID == "undefined")
                    productID = null;

     
                if (locationId == "" || locationId == "undefined")
                    locationId = null;

                if (company == "" || company == "undefined")
                    company = null;


                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                if (reportType == "details")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ExportConsumerUndeliveryFactory/ReportUSDetailsFactory.rdlc");
                    objCmd.CommandText = "PKG_RPT_EXPORT.EXPORT_CONSU_UNDELI";
                }
                if (reportType == "summary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ExportConsumerUndeliveryFactory/ReportUndeliveredStatementSummaryFactory.rdlc");
                    objCmd.CommandText = "PKG_RPT_EXPORT.EXPORT_CONSU_UNDELI";
                }

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;

                objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("productID", OracleDbType.Varchar2).Value = productID;
          //    objCmd.Parameters.Add("transportID", OracleDbType.Varchar2).Value = transportID;
                
                objCmd.Parameters.Add("companyId", OracleDbType.Varchar2).Value = company;
                objCmd.Parameters.Add("locationId", OracleDbType.Varchar2).Value = locationId;
            //  objCmd.Parameters.Add("reportType", OracleDbType.Varchar2).Value = reportType;
                objCmd.Parameters.Add("loggeduser", OracleDbType.Varchar2).Value = loggeduser;

                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("UndeliveryAreaWise", dt);
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3});

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