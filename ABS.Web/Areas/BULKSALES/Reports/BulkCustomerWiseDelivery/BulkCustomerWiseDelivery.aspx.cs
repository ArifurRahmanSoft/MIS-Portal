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

namespace ABS.Web.Areas.BULKSALES.Reports.BulkCustomerWiseDelivery
{
    public partial class BulkCustomerWiseDelivery : System.Web.UI.Page
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
                string reportType = queryStrings[10];

                // activity monitoring
                string reportFilteringData = customerID + "-" + productID + "-" + transportID + "-" + requistionNo + "-" + company + "-" + sardarId + "-" + locationId
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

                if (reportType == "partydetails")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkPartyWiseDeliveryDetails.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }
                if (reportType == "partysummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkCustomerWisePartySummary.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }
                if (reportType == "details")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkCustomerWiseDelivery.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }
                if (reportType == "summary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkCustomerWiseDeliverySummary.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }
                if (reportType == "perioditem")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkDeleveryItemWisePeriod.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }
                if (reportType == "periodsummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkDeleveryItemWisePeriodSummary.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }
                if (reportType == "categoryitem")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkDeliveryCategoryItem.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }
                if (reportType == "categorysummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkDeliveryCategorySummary.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }
                if (reportType == "pointitem")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkDeleveryPointWiseItem.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }
                if (reportType == "pointsummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkDeleveryPointWiseSummary.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }
                if (reportType == "dchargeitem")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkDeleveryWithDeliveryChargeItem.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }
                if (reportType == "dcharge")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkCustomerWiseDelivery/ReportBulkDeleveryWithDeliveryCharge.rdlc");
                    objCmd.CommandText = "TEST_SUMON.Bulk_Customer_Wise_Delivery";
                }

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("customerID", OracleDbType.Varchar2).Value = customerID;
                objCmd.Parameters.Add("productID", OracleDbType.Varchar2).Value = productID;
                objCmd.Parameters.Add("transportID", OracleDbType.Varchar2).Value = transportID;
                objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("companyId", OracleDbType.Varchar2).Value = company;
                objCmd.Parameters.Add("locationId", OracleDbType.Varchar2).Value = locationId;
                objCmd.Parameters.Add("reportType", OracleDbType.Varchar2).Value = reportType;
                objCmd.Parameters.Add("loggedUser", OracleDbType.Varchar2).Value = loggeduser;


                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSSCustomerWiseDelivery", dt);
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