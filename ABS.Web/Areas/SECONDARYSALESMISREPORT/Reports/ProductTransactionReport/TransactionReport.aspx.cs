using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using Microsoft.Reporting.WebForms;
using CTGroup.Utility.Common;
using ABS.Web.Utility;
using CTGroup.Utility;

namespace ABS.Web.Areas.SECONDARYSALESMISREPORT.Reports.ProductTransactionReport
{
    public partial class TransactionReport : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string UserName = "";
                string UserId = Session["UserID"].ToString();

                if (Session["UserFullName"] == null || Session["UserFullName"] == "undefined")
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }

                UserName = Session["UserFullName"].ToString();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string nationalId = queryStrings[2];
                string division_Id = queryStrings[3];
                string region_Id = queryStrings[4];
                string zone_Id = queryStrings[5];
                string dist_Id = queryStrings[6];
                string reportFilter = queryStrings[7];
                string brand_Id = queryStrings[8];
                string product_Id = queryStrings[9];

                string isBrndGroup = queryStrings[10];
                string brndGroupId = queryStrings[11];

                string report_Type = queryStrings[12];
                string report_category = queryStrings[13];

                Utils utility = new Utils();
                if (utility.IsNotValidActivity(UserId, nationalId))
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }
                // activity monitoring
                string reportFilteringData = reportFilter + "-" + brand_Id + "-" + product_Id + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Transaction Report", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                if (Session["UserID"].ToString() == "08414")
                    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

                if (nationalId == "")
                    nationalId = "0";

                if (division_Id == "")
                    division_Id = "0";

                if (region_Id == "")
                    region_Id = "0";

                if (zone_Id == "")
                    zone_Id = "0";

                if (dist_Id == "")
                    dist_Id = "0";

                if (brand_Id == "" || brand_Id == "undefined")
                    brand_Id = "0";

                if (product_Id == "" || product_Id == "undefined")
                    product_Id = "0";

                if (brndGroupId == "")
                    brndGroupId = null;

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
                p4.Name = "ReportFilter";
                p4.Values.Add(reportFilter);


                /// current report. we will keep this report also
                //LocalReport LocalReport = new LocalReport();
                //LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductTransactionReport/Product_Brand_Transaction_Report/ProductBrandTransactionReportRetailer.rdlc");


                LocalReport LocalReport = new LocalReport();
                if (report_category == "BrandSku")
                {
                    if (report_Type == "ROUTE")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductTransactionReport/SalesVSDel_Route_SO_Dist_Area.rdlc");
                    }
                    if (report_Type == "RETAILER")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductTransactionReport/SalesVSDel_Ret_Route_SO_Dist_Area.rdlc");
                    }
                    if (report_Type == "DISTRIBUTOR")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductTransactionReport/SalesVSDel_Dist_National_Area.rdlc");
                    }
                    if (report_Type == "COUNTER_SALES")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductTransactionReport/SalesVSDel_Counter_National_Area.rdlc");
                    }
                }

                else
                {
                    //  LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductTransactionReport/SalesVSDel_Brand_Dist_National_Area.rdlc");
                    if (report_Type == "ROUTE")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductTransactionReport/SalesVSDel_Brand_Route_SO_Dist_Area.rdlc");
                    }
                    if (report_Type == "RETAILER")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductTransactionReport/SalesVSDel_Brand_Ret_Route_SO_Dist_Area.rdlc");
                    }
                    if (report_Type == "DISTRIBUTOR")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductTransactionReport/SalesVSDel_Brand_Dist_National_Area.rdlc");
                    }
                    if (report_Type == "COUNTER_SALES")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductTransactionReport/SalesVSDel_Brand_Counter_National_Area.rdlc");
                    }
                }


                OracleCommand objCmd = new OracleCommand();
                //objCmd.CommandText = "PKG_SS_SALES_VS_DEL.GET_SALES_VS_DEL";
                objCmd.CommandText = "PKG_SS_SALES_VS_DEL_NEW.GET_SALES_VS_DEL";
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("p_s_date", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("p_e_date", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("p_brand_id", OracleDbType.Varchar2).Value = brand_Id == "0" ? null : brand_Id;
                objCmd.Parameters.Add("p_product_id", OracleDbType.Varchar2).Value = product_Id == "0" ? null : product_Id;
                objCmd.Parameters.Add("p_division_id", OracleDbType.Varchar2).Value = division_Id == "0" ? null : division_Id;
                objCmd.Parameters.Add("p_region_id", OracleDbType.Varchar2).Value = region_Id == "0" ? null : region_Id;
                objCmd.Parameters.Add("p_zone_id", OracleDbType.Varchar2).Value = zone_Id == "0" ? null : zone_Id;
                objCmd.Parameters.Add("p_distributor_id", OracleDbType.Varchar2).Value = dist_Id == "0" ? null : dist_Id;
                objCmd.Parameters.Add("p_so_id", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("p_route_id", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("p_retailer_id", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("p_report_type", OracleDbType.Varchar2).Value = report_Type;

                objCmd.Parameters.Add("isBrndGroup", OracleDbType.Varchar2).Value = isBrndGroup;
                objCmd.Parameters.Add("brndGroupId", OracleDbType.Varchar2).Value = brndGroupId;
                objCmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = UserId;

                DataTable dt = classDt.GetSecondaryData(objCmd);

                //ReportDataSource datasource = new ReportDataSource();
                //datasource.Value = dt;
                //datasource.Name = "SalesOrderSummaryDS";

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();
                //LocalReport.DataSources.Add(datasource);
                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);

                /// current report. we will keep this report also
                //ReportDataSource datasource = new ReportDataSource("ProductBrandTransactionDS", dt);


                ReportDataSource datasource = new ReportDataSource("DSSalesVSDelRoutetoArea", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });

                reportActivityMonitoring.RequestEnd(autoId);

                //        localReports.Add(LocalReport);

                //        string deviceInfo =
                //     @"<DeviceInfo>
                //    <OutputFormat>PDF</OutputFormat>
                //    <PageWidth>16in</PageWidth>
                //    <PageHeight>8.5in</PageHeight>
                //    <MarginTop>0.50in</MarginTop>
                //    <MarginLeft>0.25in</MarginLeft>
                //    <MarginRight>0.25in</MarginRight>
                //    <MarginBottom>0.50in</MarginBottom>
                //</DeviceInfo>";

                //        byte[] file = LocalReport.Render("pdf", deviceInfo);

                //        Response.ContentType = "application/pdf";
                //        Response.AddHeader("content-disposition", "inline;filename=DC-" + iid + ".pdf");
                //        Response.Buffer = true;
                //        Response.Clear();
                //        Response.BinaryWrite(file);

                //        Response.End();




                //if (reportMode == "1")
                //{
                //    if (arrangeType == "2")
                //    {
                //        LocalReport.ReportPath = Server.MapPath("~/ViewReport/Transaction_Report/Product_Brand_Transaction_Report/ProductBrandTransactionReportDivision.rdlc");
                //    }
                //    if (arrangeType == "3")
                //    {
                //        LocalReport.ReportPath = Server.MapPath("~/ViewReport/Transaction_Report/Product_Brand_Transaction_Report/ProductBrandTransactionReportRegion.rdlc");
                //    }
                //    if (arrangeType == "4")
                //    {
                //        LocalReport.ReportPath = Server.MapPath("~/ViewReport/Transaction_Report/Product_Brand_Transaction_Report/ProductBrandTransactionReportZone.rdlc");
                //    }
                //    if (arrangeType == "5")
                //    {
                //        LocalReport.ReportPath = Server.MapPath("~/ViewReport/Transaction_Report/Product_Brand_Transaction_Report/ProductBrandTransactionReportDistributor.rdlc");
                //    }
                //    if (arrangeType == "6")
                //    {
                //        LocalReport.ReportPath = Server.MapPath("~/ViewReport/Transaction_Report/Product_Brand_Transaction_Report/ProductBrandTransactionReportRoute.rdlc");
                //    }
                //    if (arrangeType == "7")
                //    {
                //        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductTransactionReport/Product_Brand_Transaction_Report/ProductBrandTransactionReportRetailer.rdlc");
                //    }
                //}

                //if (reportMode == "2")
                //{
                //    LocalReport.ReportPath = Server.MapPath("~/ViewReport/Transaction_Report/Product_Brand_Transaction_Report/Brand_Product_Ord_Delv_Report.rdlc");
                //}


            }
        }
    }
}