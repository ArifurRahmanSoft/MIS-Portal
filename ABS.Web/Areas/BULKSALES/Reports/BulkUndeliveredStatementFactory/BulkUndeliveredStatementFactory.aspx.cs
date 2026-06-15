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

namespace ABS.Web.Areas.BULKSALES.Reports.BulkUndeliveredStatementFactory
{
    public partial class BulkUndeliveredStatementFactory : System.Web.UI.Page
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
                string isExport = queryStrings[11];

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

                if (isExport == "true" && (reportType == "itemsummary" || reportType == "unsummary"))
                {
                    objCmd.CommandText = "rpt_bulk_undelSum_Loc_Exp.getdata";
                }
                if (isExport == "true" && (reportType == "itemdetails" || reportType == "unpdetails"))
                {
                    objCmd.CommandText = "rpt_bulk_undel_dtl_Loc_Exp.getdata";
                }


                if (isExport == "false" && (reportType == "itemsummary" || reportType == "unsummary"))
                {
                    objCmd.CommandText = "rpt_bulk_undeliverdSum.getdata";
                }
                if (isExport == "false" && (reportType == "itemdetails" || reportType == "unpdetails"))
                {
                    objCmd.CommandText = "rpt_bulk_undeliverd_dtl.getdata";
                }


            
                if (reportType == "unpdetails")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkUndeliveredStatementFactory/ReportBulkUndeliveryStatementFactoryDetailPartyWise.rdlc");
                }
                if (reportType == "unsummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkUndeliveredStatementFactory/ReportBulkUndeliveryStatementFactorySummary.rdlc");
                }
                if (reportType == "itemdetails")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkUndeliveredStatementFactory/ReportBulkUndeliveryStatemenFactoryItemWiseDetails.rdlc");
                }
                if (reportType == "itemsummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkUndeliveredStatementFactory/ReportBulkUndeliveryStatementFactoryItemWiseSummary.rdlc");
                }
                


                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("p_company_id", OracleDbType.Varchar2).Value = company;
                objCmd.Parameters.Add("p_customer_id", OracleDbType.Varchar2).Value = customerID;
                objCmd.Parameters.Add("p_location_id", OracleDbType.Varchar2).Value = locationId;
                objCmd.Parameters.Add("p_product_id", OracleDbType.Varchar2).Value = productID;
               //objCmd.Parameters.Add("transportID", OracleDbType.Varchar2).Value = transportID;
                objCmd.Parameters.Add("p_from_date", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("p_to_date", OracleDbType.Varchar2).Value = toDate;       
                objCmd.Parameters.Add("p_prod_group_id", OracleDbType.Varchar2).Value = null;
                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSSUndeliveredStatement", dt);
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