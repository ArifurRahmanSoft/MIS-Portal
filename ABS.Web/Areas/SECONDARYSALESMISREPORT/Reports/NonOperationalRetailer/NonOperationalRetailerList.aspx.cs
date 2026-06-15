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

namespace CGSS.Web.Areas.Reports.OrderDeliveryByBrand
{
    public partial class NonOperationalRetailer : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
        protected void Page_Load(object sender, EventArgs e)
        {
            string iid = Request.QueryString["Id"];
            if (!IsPostBack)
            {
                string UserId = Session["UserID"].ToString();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                if ("1" == "1")
                {
                    string[] queryStrings = Request["queryString"].ToString().Split(',');
                    string fromDate = queryStrings[0];
                    string toDate = queryStrings[1];
                    string nationalId = queryStrings[2];
                    string division_Id = queryStrings[3];
                    string region_Id = queryStrings[4];
                    string zone_Id = queryStrings[5];
                    string dist_Id = queryStrings[6];
                    string brand_Id = queryStrings[7];

                    string isBrndGroup = queryStrings[8];
                    string brndGroupId = queryStrings[9];

                    string reportFilter = queryStrings[10];

                    // activity monitoring
                    string reportFilteringData = reportFilter + "-" + brand_Id + "-" + fromDate + "-" + toDate;
                    ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                    string autoId = reportActivityMonitoring.RequestStart("Non Operational Retailer List", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                    if (UserId == "08414")
                        fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

                    string product_Id = "0"; // Request.QueryString["product_Id"];
                    string route_Id = "0"; //Request.QueryString["route_Id"];
                    string retailer_Id = "0"; // Request.QueryString["ret_Id"];
                    string so_Id = "0";//Request.QueryString["so_Id"];                  
                    string arrangeType = "6";//Request.QueryString["arrangeType"];

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

                    if (brand_Id == "")
                        brand_Id = "0";

                    if (brndGroupId == "")
                        brndGroupId = null;

                    string reportMode = "1"; // Request.QueryString["reportMode"];
                    ReportParameter p1 = new ReportParameter();
                    ReportParameter p2 = new ReportParameter();

                    p1.Name = "FromDate";
                    p1.Values.Add(fromDate);
                    p2.Name = "ToDate";
                    p2.Values.Add(toDate);
                    
                    LocalReport LocalReport = new LocalReport();                   

                    if (reportMode == "1")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/NonOperationalRetailer/Report_Unavailable_Retailers.rdlc");
                    }
                    OracleCommand objCmd = new OracleCommand();

                    if (reportMode == "1")
                    {
                        //objCmd.CommandText = "SS_DELIVERY_REPORT.GET_ORD_DLV_BY_BRND_PROD";

                        //objCmd.CommandText = "PKG_REPORT_ORD_DLV_RET_COUNT. NOP_RETAITER_LIST";
                        objCmd.CommandText = "PKG_ORD_DLV_RET_COUNT_NEW.NOP_RETAITER_LIST";

                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = reportMode;

                        objCmd.Parameters.Add("p_s_date", OracleDbType.Varchar2).Value = fromDate;
                        objCmd.Parameters.Add("p_e_date", OracleDbType.Varchar2).Value = toDate;
                        objCmd.Parameters.Add("p_brand_id", OracleDbType.Varchar2).Value = brand_Id == "0" ? null : brand_Id;
                        objCmd.Parameters.Add("p_product_id", OracleDbType.Varchar2).Value = product_Id == "0" ? null : product_Id;
                        objCmd.Parameters.Add("p_division_id", OracleDbType.Varchar2).Value = division_Id == "0" ? null : division_Id;
                        objCmd.Parameters.Add("p_region_id", OracleDbType.Varchar2).Value = region_Id == "0" ? null : region_Id;
                        objCmd.Parameters.Add("p_zone_id", OracleDbType.Varchar2).Value = zone_Id == "0" ? null : zone_Id;
                        objCmd.Parameters.Add("p_distributor_id", OracleDbType.Varchar2).Value = dist_Id == "0" ? null : dist_Id;
                        objCmd.Parameters.Add("p_so_id", OracleDbType.Varchar2).Value = so_Id == "0" ? null : so_Id;
                        objCmd.Parameters.Add("p_route_id", OracleDbType.Varchar2).Value = route_Id == "0" ? null : route_Id;
                        objCmd.Parameters.Add("p_retailer_id", OracleDbType.Varchar2).Value = retailer_Id == "0" ? null : retailer_Id;

                        objCmd.Parameters.Add("isBrndGroup", OracleDbType.Varchar2).Value = isBrndGroup;
                        objCmd.Parameters.Add("brndGroupId", OracleDbType.Varchar2).Value = brndGroupId;
                        objCmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = UserId;
                    }     
                    DataTable dt = classDt.GetSecondaryData(objCmd);

                    LocalReport.EnableHyperlinks = true;
                    LocalReport.DataSources.Clear();

                    LocalReport.SetParameters(p1);
                    LocalReport.SetParameters(p2);
                    ReportDataSource datasource = new ReportDataSource("DS_Order_Delivery_By_Brand", dt);

                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });
                    reportActivityMonitoring.RequestEnd(autoId);
                }
            }
        }
    }
}