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

namespace ABS.Web.Areas.SECONDARYSALESMISREPORT.Reports.RetailerInfo
{
    public partial class RetailerInfo : System.Web.UI.Page
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
                string UserId = Session["UserID"].ToString();

                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string nationalId = queryStrings[2];
                string division_Id = queryStrings[3];
                string region_Id = queryStrings[4];
                string zone_Id = queryStrings[5];
                string dist_Id = queryStrings[6];
                string reportFilter = queryStrings[7];
                //string brand_Id = queryStrings[8];
                //string product_Id = queryStrings[9];

                //string isBrndGroup = queryStrings[10];
                //string brndGroupId = queryStrings[11];

                //string report_Type = queryStrings[12];

                // activity monitoring
                string reportFilteringData = fromDate + "-" + toDate;

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

                //if (brand_Id == "" || brand_Id == "undefined")
                //    brand_Id = "0";

                //if (product_Id == "" || product_Id == "undefined")
                //    product_Id = "0";

                //if (brndGroupId == "")
                //    brndGroupId = null;

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();
                ReportParameter p4 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);
                p2.Name = "ToDate";
                p2.Values.Add(toDate);
                //p3.Name = "UserName";
                //p3.Values.Add(UserName);
                //p4.Name = "ReportFilter";
                //p4.Values.Add(reportFilter);

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                LocalReport LocalReport = new LocalReport();

                OracleCommand objCmd = new OracleCommand();

                objCmd.CommandText = "PKG_REPORT_MASTER_DATA.GET_MASTER_DATA";
                LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/RetailerInformation/Master_Dist_Retailer_Report.rdlc");

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = "DB";

                objCmd.Parameters.Add("p_tracking_no", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("p_stype", OracleDbType.Varchar2).Value = null;
                
                //objCmd.Parameters.Add("p_brand_id", OracleDbType.Varchar2).Value = brand_Id == "0" ? null : brand_Id;
                //objCmd.Parameters.Add("p_product_id", OracleDbType.Varchar2).Value = product_Id == "0" ? null : product_Id;
                objCmd.Parameters.Add("p_national", OracleDbType.Varchar2).Value = nationalId;
                objCmd.Parameters.Add("p_division", OracleDbType.Varchar2).Value = division_Id;
                objCmd.Parameters.Add("p_region", OracleDbType.Varchar2).Value = region_Id;
                objCmd.Parameters.Add("p_zone", OracleDbType.Varchar2).Value = zone_Id;
                objCmd.Parameters.Add("p_dist", OracleDbType.Varchar2).Value = dist_Id;

                objCmd.Parameters.Add("p_so", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("p_route", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("p_retailer", OracleDbType.Varchar2).Value = null;

                objCmd.Parameters.Add("p_s_date", OracleDbType.Varchar2).Value = null;//fromDate;
                objCmd.Parameters.Add("p_e_date", OracleDbType.Varchar2).Value = null; //toDate;

                //objCmd.Parameters.Add("isBrndGroup", OracleDbType.Varchar2).Value = isBrndGroup;
                //objCmd.Parameters.Add("brndGroupId", OracleDbType.Varchar2).Value = brndGroupId;
                //objCmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = UserId;

                DataTable dt = classDt.GetSecondaryData(objCmd);
                
                ReportViewer1.ShowPrintButton = false;
                ReportViewer1.ShowExportControls = false;                

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();
                //LocalReport.ReleaseSandboxAppDomain();
                LocalReport.Dispose();

                //LocalReport.SetParameters(p1);
                //LocalReport.SetParameters(p2);
                //LocalReport.SetParameters(p3);
                //LocalReport.SetParameters(p4);


                ReportDataSource datasource = new ReportDataSource("DSRetailerInfo", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                //ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });
            }
        }
    }
}