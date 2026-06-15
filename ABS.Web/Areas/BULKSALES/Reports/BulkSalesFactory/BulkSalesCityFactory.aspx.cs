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

namespace ABS.Web.Areas.BULKSALES.Reports.BulkSalesFactory
{
    public partial class BulkSalesCityFactory : System.Web.UI.Page
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
                string isExportIncluded = queryStrings[11]; 

                // activity monitoring
                string reportFilteringData = customerID + "-" + productID + "-" + transportID + "-" + requistionNo + "-" + company + "-" + sardarId + "-" + locationId
                    + "-" + fromDate + "-" + toDate + "-" + reportType;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Bulk Sales Statement", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

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

                if(isExportIncluded == "true")  //////// local + export
                {
                    if (reportType == "partydetails")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementDetails.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.ConsolidatedSalesExport";
                    }
                    if (reportType == "partysummary")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementSummary.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.ConsolidatedSalesExport";
                    }
                    if (reportType == "partytotal")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementSummaryDetails.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.ConsolidatedSalesExport";
                    }
                    if (reportType == "Proddetails")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkProductWiseSalesStatementDetails.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.ConsolidatedSalesExport";
                    }
                    if (reportType == "Prodsummary")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkProductWiseSalesStatementSummary.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.ConsolidatedSalesExport";
                    }
                    if (reportType == "WithoutItem")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementItemsWithoutParty.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.ConsolidatedSalesExport";
                    }
                    if (reportType == "WithoutSummary")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementSummaryWithoutParty.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.ConsolidatedSalesExport";
                    }
                    if (reportType == "preioditem")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementItemWiseByPreiod.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.ConsolidatedSalesExport";
                    }
                    if (reportType == "preiodsummary")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatemenSummaryWiseByPreiod.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.ConsolidatedSalesExport";
                    }
                    if (reportType == "categoryitem")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkCategoryWiseSalesStatementDetails.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.getsalesstateCategory";
                    }
                    if (reportType == "partywiseitem")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementDetailsTon.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.ConsolidatedSalesExport";
                    }
                }
                else /////////// local sales
                {
                    if (reportType == "partydetails")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementDetails.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.Bulk_Sales_Statement_Itemwise";
                    }
                    if (reportType == "partysummary")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementSummary.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.Bulk_Sales_Statement_Itemwise";
                    }
                    if (reportType == "partytotal")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementSummaryDetails.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.Bulk_Sales_Statement_Itemwise";
                    }
                    if (reportType == "Proddetails")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkProductWiseSalesStatementDetails.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.Bulk_Sales_Statement_Itemwise";
                    }
                    if (reportType == "Prodsummary")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkProductWiseSalesStatementSummary.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.Bulk_Sales_Statement_Itemwise";
                    }
                    if (reportType == "WithoutItem")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementItemsWithoutParty.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.Bulk_Sales_Statement_Itemwise";
                    }
                    if (reportType == "WithoutSummary")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementSummaryWithoutParty.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.Bulk_Sales_Statement_Itemwise";
                    }
                    if (reportType == "preioditem")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementItemWiseByPreiod.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.Bulk_Sales_Statement_Itemwise";
                    }
                    if (reportType == "preiodsummary")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatemenSummaryWiseByPreiod.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.Bulk_Sales_Statement_Itemwise";
                    }
                    if (reportType == "categoryitem")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkCategoryWiseSalesStatementDetails.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.getsalesstateCategory";
                    }
                    if (reportType == "partywiseitem")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/BULKSALES/Reports/BulkSalesFactory/ReportBulkPartyWiseSalesStatementDetailsTon.rdlc");
                        objCmd.CommandText = "PKG_BULK_SALES_RPT.getsalesstateCategory";
                    }
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

                ReportDataSource datasource = new ReportDataSource("DSFactoryBulkSales", dt);
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