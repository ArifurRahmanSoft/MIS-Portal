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

namespace ABS.Web.Areas.SECONDARYSALESMISREPORT.Reports.SRPerformance
{
    public partial class SRPerformanceMonitoring : System.Web.UI.Page
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
                                       //Response.Redirect("");
                                       //Server.Transfer("http://localhost:15812/Account/Login");
                                       //Response.Redirect("http://localhost:15812/Account/Login");
                    Response.Redirect("~/Account/Login");
                }

                UserName = Session["UserFullName"].ToString();

                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string nationalId = queryStrings[2];
                string division_Id = queryStrings[3];
                string region_Id = queryStrings[4];
                string zone_Id = queryStrings[5];
                string dist_Id = queryStrings[6];
                string reportFilter = queryStrings[7];
                string sales_Person_Id = queryStrings[8];
                string brand_Id = queryStrings[9];
                string product_Id = queryStrings[10];

                string isBrndGroup = queryStrings[11];
                string brndGroupId = queryStrings[12];

                string reporttype = queryStrings[13];

                // activity monitoring
                string reportFilteringData = reportFilter + "-" + brand_Id + "-" + product_Id + "-" + reporttype
                                             + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("SO Performance", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

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

                if (sales_Person_Id == "")
                    sales_Person_Id = "0";

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
                ReportParameter p5 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);
                p2.Name = "ToDate";
                p2.Values.Add(toDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);
                p4.Name = "ReportFilter";
                p4.Values.Add(reportFilter);
                p5.Name = "ReportName";

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                LocalReport LocalReport = new LocalReport();

                OracleCommand objCmd = new OracleCommand();
                DataTable dt = new DataTable();
                string pkgName = "PKG_Report_Sales_Officer_Performance_New";
                string spName = "";

                if (reporttype == "Summary" || reporttype == "Detail")
                {
                    p5.Values.Add("Value");
                    spName = "GET_SO_PERFORMANCE";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/SRPerformance/SOPerformanceMonitoring.rdlc");
                }
                else if (reporttype == "SR_Delivery_Summary")
                {
                    p5.Values.Add("Delivery-Summary");
                    spName = "GET_SR_PERFORMANCE_DELIVERY_SUMMARY";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/SRPerformance/SRPerformanceMonitoringSummary.rdlc");
                }
                else if (reporttype == "SR_Delivery_Detail")
                {
                    p5.Values.Add("Delivery-Detail");
                    spName = "GET_SR_PERFORMANCE_DELIVERY_DETAIL";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/SRPerformance/SRPerformanceMonitoringDetail.rdlc");
                }
                else if (reporttype == "SR_Delivery_Detail_Sku")
                {
                    p5.Values.Add("Delivery-SKU-Wise");
                    spName = "GET_SR_PERFORMANCE_DELIVERY_SKU_WISE";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/SRPerformance/SRPerformanceMonitoringDetailSkuWise.rdlc");
                }
                else if (reporttype == "SR_Delivery_Detail_Dist")
                {
                    p5.Values.Add("Delivery-Dist-Wise");
                    //spName = "GET_SR_PERFORMANCE_DELIVERY_DIST_WISE";
                    spName = "GET_SR_PERFORMANCE_DELIVERY_DIST_WISE_NEW";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/SRPerformance/SRPerformanceMonitoringDetailDistWise.rdlc");
                }
                else if (reporttype == "SR_Order_Summary")
                {
                    p5.Values.Add("Order-Summary");
                    spName = "GET_SR_PERFORMANCE_ORDER_SUMMARY";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/SRPerformance/SRPerformanceMonitoringSummary.rdlc");
                }
                else if (reporttype == "SR_Order_Detail")
                {
                    p5.Values.Add("Order-Detail");
                    spName = "GET_SR_PERFORMANCE_ORDER_DETAIL";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/SRPerformance/SRPerformanceMonitoringDetail.rdlc");
                }
                else if (reporttype == "SR_Order_Detail_Sku")
                {
                    p5.Values.Add("Order-SKU-Wise");
                    spName = "GET_SR_PERFORMANCE_ORDER_SKU_WISE";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/SRPerformance/SRPerformanceMonitoringDetailSkuWise.rdlc");
                }
                else if (reporttype == "SR_Order_Detail_Dist")
                {
                    p5.Values.Add("Order-Dist-Wise");
                    spName = "GET_SR_PERFORMANCE_ORDER_DIST_WISE";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/SRPerformance/SRPerformanceMonitoringDetailDistWise.rdlc");
                }

                if (!string.IsNullOrEmpty(spName))
                {
                    objCmd.CommandText = pkgName + "." + spName;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = reporttype;
                    objCmd.Parameters.Add("p_s_date", OracleDbType.Varchar2).Value = fromDate;
                    objCmd.Parameters.Add("p_e_date", OracleDbType.Varchar2).Value = toDate;
                    objCmd.Parameters.Add("p_brand_id", OracleDbType.Varchar2).Value = brand_Id == "0" ? null : brand_Id;
                    objCmd.Parameters.Add("p_product_id", OracleDbType.Varchar2).Value = product_Id == "0" ? null : product_Id;
                    objCmd.Parameters.Add("p_national_id", OracleDbType.Varchar2).Value = nationalId == "0" ? null : nationalId;
                    objCmd.Parameters.Add("p_division_id", OracleDbType.Varchar2).Value = division_Id == "0" ? null : division_Id;
                    objCmd.Parameters.Add("p_region_id", OracleDbType.Varchar2).Value = region_Id == "0" ? null : region_Id;
                    objCmd.Parameters.Add("p_zone_id", OracleDbType.Varchar2).Value = zone_Id == "0" ? null : zone_Id;
                    objCmd.Parameters.Add("p_distributor_id", OracleDbType.Varchar2).Value = dist_Id == "0" ? null : dist_Id;
                    objCmd.Parameters.Add("p_sales_person_id", OracleDbType.Varchar2).Value = sales_Person_Id == "0" ? null : sales_Person_Id;

                    objCmd.Parameters.Add("isBrndGroup", OracleDbType.Varchar2).Value = isBrndGroup;
                    objCmd.Parameters.Add("brndGroupId", OracleDbType.Varchar2).Value = brndGroupId;
                    objCmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = UserId;

                    //string st = DateTime.Now.ToLongTimeString();                    
                    dt = classDt.GetSecondaryData(objCmd);
                    //string et = DateTime.Now.ToLongTimeString();
                }

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();
                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                LocalReport.SetParameters(p4);
                LocalReport.SetParameters(p5);

                ReportDataSource datasource = new ReportDataSource("DTSRPerformance", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5 });
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}