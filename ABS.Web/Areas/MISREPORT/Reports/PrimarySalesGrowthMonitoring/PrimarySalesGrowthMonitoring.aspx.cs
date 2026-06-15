using ABS.Web.Utility;
using CTGroup.Utility;
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

namespace ABS.Web.Areas.MISREPORT.Reports.PrimarySalesGrowthMonitoring
{
    public partial class PrimarySalesGrowthMonitoring : System.Web.UI.Page
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
                string fromDatePrev = queryStrings[0];
                string toDatePrev = queryStrings[1];
                string fromDatePost = queryStrings[2];
                string toDatePost = queryStrings[3];
                string companyId = queryStrings[4];
                string nationalId = queryStrings[5];
                string divisionId = queryStrings[6];
                string regionId = queryStrings[7];
                string zoneId = queryStrings[8];
                string distributorId = queryStrings[9];
                string reportFilter = queryStrings[10];
                string brandId = queryStrings[11];
                string productId = queryStrings[12];
                string reportType = queryStrings[13];
                string mode = queryStrings[14];
                string reportName = queryStrings[15];

                Utils utility = new Utils();
                if (utility.IsNotValidActivity(UserId, nationalId))
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }

                // activity monitoring
                string reportFilteringData = reportFilter + "-" + companyId + "-" + brandId + "-" + productId
                                             + "-" + reportType + "-" + fromDatePrev + "-" + toDatePrev + "-" + fromDatePost + "-" + toDatePost;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Primary Sales Growth Monitoring", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                //if (Session["UserID"].ToString() == "08414") // rashed sir logic, stopped now, as team member increased
                //    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();
                ReportParameter p4 = new ReportParameter();
                ReportParameter p5 = new ReportParameter();
                ReportParameter p6 = new ReportParameter();
                ReportParameter p7 = new ReportParameter();

                p1.Name = "PrevFromDate";
                p1.Values.Add(fromDatePrev);
                p2.Name = "PrevToDate";
                p2.Values.Add(toDatePrev);
                p3.Name = "PostFromDate";
                p3.Values.Add(fromDatePost);
                p4.Name = "PostToDate";
                p4.Values.Add(toDatePost);
                p5.Name = "UserName";
                p5.Values.Add(UserName);
                p6.Name = "ReportFilter";
                p6.Values.Add(reportFilter);
                p7.Name = "ReportType";
                p7.Values.Add(reportName);

                if (companyId == "")
                    companyId = null;

                if (nationalId == "")
                    nationalId = null;

                if (divisionId == "")
                    divisionId = null;

                if (regionId == "")
                    regionId = null;

                if (zoneId == "")
                    zoneId = null;

                if (distributorId == "")
                    distributorId = null;

                if (brandId == "")
                    brandId = null;

                if (productId == "")
                    productId = null;

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                LocalReport LocalReport = new LocalReport();
                OracleCommand objCmd = new OracleCommand();

                if (reportType == "AreaDBBrand")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PrimarySalesGrowthMonitoring/rpt_Primary_Sales_Growth_Monitoring_DB_BRND.rdlc");
                    objCmd.CommandText = "PRIMARY_SALES_GROWTH_MONITORING.Get_Primary_Sales_Growth";
                }
                else if (reportType == "AreaBrand")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PrimarySalesGrowthMonitoring/rpt_Primary_Sales_Growth_Monitoring.rdlc");

                    if (mode == "1")
                    {
                        objCmd.CommandText = "PRIMARY_SALES_GROWTH_MONITORING.GET_PRIMARY_SALES_GROWTH_DIST";
                    }
                    else if (mode == "2")
                    {
                        objCmd.CommandText = "PRIMARY_SALES_GROWTH_MONITORING.GET_PRIMARY_SALES_GROWTH_ZONE";
                    }
                    else if (mode == "3")
                    {
                        objCmd.CommandText = "PRIMARY_SALES_GROWTH_MONITORING.GET_PRIMARY_SALES_GROWTH_REGN";
                    }
                    else if (mode == "4")
                    {
                        objCmd.CommandText = "PRIMARY_SALES_GROWTH_MONITORING.GET_PRIMARY_SALES_GROWTH_DVSN";
                    }
                    else if (mode == "5")
                    {
                        objCmd.CommandText = "PRIMARY_SALES_GROWTH_MONITORING.GET_PRIMARY_SALES_GROWTH_NTNAL";
                    }
                }
                else if (reportType == "AreaBrandDtl")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PrimarySalesGrowthMonitoring/rpt_Primary_Sales_Growth_Monitoring_DB_Area_BRND.rdlc");
                    objCmd.CommandText = "PRIMARY_SALES_GROWTH_MONITORING.GET_PRIMARY_SALES_GROWTH_ONLY";
                }

                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("fresult", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("fdatPrev", OracleDbType.Varchar2).Value = fromDatePrev;
                objCmd.Parameters.Add("tdatPrev", OracleDbType.Varchar2).Value = toDatePrev;
                objCmd.Parameters.Add("fdatPost", OracleDbType.Varchar2).Value = fromDatePost;
                objCmd.Parameters.Add("tdatPost", OracleDbType.Varchar2).Value = toDatePost;
                objCmd.Parameters.Add("companyId", OracleDbType.Varchar2).Value = companyId;
                objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                objCmd.Parameters.Add("dloc", OracleDbType.Varchar2).Value = null;

                DataTable dt = classDt.GetData(objCmd);


                ReportDataSource datasource = new ReportDataSource("PriSalGrowthMonitoring", dt);
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7 });

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                LocalReport.SetParameters(p4);
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}