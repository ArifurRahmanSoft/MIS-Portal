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

namespace ABS.Web.Areas.MISREPORT.Reports.CustomerWiseDelivery
{
    public partial class CustomerWiseDelivery : System.Web.UI.Page
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

                string customerID = queryStrings[0];
                string productID = queryStrings[1];
                string transportID = queryStrings[2];
                string fromDate = queryStrings[3];
                string toDate = queryStrings[4];
                string requistionNo = queryStrings[5];
                string company = queryStrings[6];
                string loggeduser = queryStrings[7];
                string sardarId = queryStrings[8];
                string locationId = queryStrings[9];
                string fromDateTime = queryStrings[10];
                string toDateTime = queryStrings[11];
                string timeSelection = queryStrings[12];



                // activity monitoring
                string reportFilteringData = customerID + "-" + productID + "-" + transportID + "-" + requistionNo + "-" + company + "-" + sardarId + "-" + locationId
                    + "-" + fromDate + "-" + toDate;
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

                if (customerID == "")
                    customerID = null;

                if (productID == "")
                    productID = null;

                if (transportID == "")
                    transportID = null;

                if (sardarId == "")
                    sardarId = null;

                if (locationId == "")
                    locationId = null;

                if (company == "" || company == "undefined")
                    company = null;

                if (requistionNo == "undefined")
                    requistionNo = null;

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();


                LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/CustomerWiseDelivery/ReportCustomerWiseDelivery.rdlc");

                if (timeSelection == "Yes")
                {
                    objCmd.CommandText = "REPORTS.Customer_Wise_Delivery_Time";
                }
                else
                {
                    objCmd.CommandText = "REPORTS.Customer_Wise_Delivery";
                }


                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("fresult", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                objCmd.Parameters.Add("customerID", OracleDbType.Varchar2).Value = customerID;
                objCmd.Parameters.Add("productID", OracleDbType.Varchar2).Value = productID;
                objCmd.Parameters.Add("transportID", OracleDbType.Varchar2).Value = transportID;
                if (timeSelection == "Yes")
                {
                    objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDateTime;
                    objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDateTime;
                }
                else
                {
                    objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                    objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                }
                objCmd.Parameters.Add("requistionNo", OracleDbType.Varchar2).Value = requistionNo;
                objCmd.Parameters.Add("companyId", OracleDbType.Varchar2).Value = company;
                objCmd.Parameters.Add("loggeduser", OracleDbType.Varchar2).Value = loggeduser;
                objCmd.Parameters.Add("sardarId", OracleDbType.Varchar2).Value = sardarId;
                objCmd.Parameters.Add("locationId", OracleDbType.Varchar2).Value = locationId;



                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSSCustomerWiseDelivery", dt);
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