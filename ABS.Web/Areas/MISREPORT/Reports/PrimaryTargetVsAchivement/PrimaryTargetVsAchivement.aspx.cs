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

namespace ABS.Web.Areas.MISREPORT.Reports.PrimaryTargetVsAchivement
{
    public partial class PrimaryTargetVsAchivement : System.Web.UI.Page
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
                string companyId = queryStrings[2];
                string nationalId = queryStrings[3];
                string divisionId = queryStrings[4];
                string regionId = queryStrings[5];
                string zoneId = queryStrings[6];
                string distributorId = queryStrings[7];
                string reportFilter = queryStrings[8];
                string brandId = queryStrings[9];
                string productId = queryStrings[10];
                string reportType = queryStrings[11];
                string mode = queryStrings[12];

                Utils utility = new Utils();
                if (utility.IsNotValidActivity(UserId, nationalId))
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }

                // activity monitoring
                string reportFilteringData = reportFilter + "-" + companyId + "-" + brandId + "-" + productId
                                             + "-" + reportType + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Primary Target Vs Achievement", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                //if (Session["UserID"].ToString() == "08414") // rashed sir logic, stopped now, as team member increased
                //    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

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
                p5.Name = "ReportType";
                p5.Values.Add(reportType);

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


                LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PrimaryTargetVsAchivement/rpt_Primary_Target_Vs_Sales.rdlc");

                if (mode == "1")
                {
                    objCmd.CommandText = "PRIMARY_TARGET_VS_ACHIEVEMENT.GET_DIST_PRIMARY_TARGET_VS_SALES";
                }
                else if (mode == "2")
                {
                    objCmd.CommandText = "PRIMARY_TARGET_VS_ACHIEVEMENT.GET_DIST_PRIMARY_TARGET_VS_SALES_ZONE";
                }
                else if (mode == "3")
                {
                    objCmd.CommandText = "PRIMARY_TARGET_VS_ACHIEVEMENT.GET_DIST_PRIMARY_TARGET_VS_SALES_REGN";
                }
                else if (mode == "4")
                {
                    objCmd.CommandText = "PRIMARY_TARGET_VS_ACHIEVEMENT.GET_DIST_PRIMARY_TARGET_VS_SALES_DVSN";
                }
                else if (mode == "5")
                {
                    objCmd.CommandText = "PRIMARY_TARGET_VS_ACHIEVEMENT.GET_DIST_PRIMARY_TARGET_VS_SALES_NTNAL";
                }

                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("fresult", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("fdat", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("tdat", OracleDbType.Varchar2).Value = toDate;
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


                ReportDataSource datasource = new ReportDataSource("PrimarTargetVsAchievement", dt);
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5 });

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