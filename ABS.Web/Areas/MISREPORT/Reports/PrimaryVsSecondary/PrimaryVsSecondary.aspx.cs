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

namespace ABS.Web.Areas.MISREPORT.Reports.PrimaryVsSecondary
{
    public partial class PrimaryVsSecondary : System.Web.UI.Page
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

              //  commonQueryString = startDate + ',' + endDate + ',' + nationalId + ','
              //+ divisionId + ',' + regionId + ',' + zoneId + ',' + distributorId + ',' + reportFilter;


                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string nationalId = queryStrings[2];
                string divisionId = queryStrings[3];
                string regionId = queryStrings[4];
                string zoneId = queryStrings[5];
                string distributorId = queryStrings[6];
                string reportFilter = queryStrings[7];


                string brandId = queryStrings[8];
                string productId = queryStrings[9];
                string reportType = queryStrings[10];

                // activity monitoring
                string reportFilteringData = reportFilter + "-" + brandId + "-" + productId + "-" + fromDate + "-" + toDate + "-" + reportType;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Primary VS Secondary", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                if (Session["UserID"].ToString() == "08414")
                    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

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


                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();
                OracleCommand objCmd = new OracleCommand();

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

                if (reportType == "areadetailcarton")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PrimaryVsSecondary/PrimaryVsSecondaryAreaCartonDetail.rdlc");
                    objCmd.CommandText = "PKG_REPORT_PRI_VS_SECON.RPI_TAR_LIFT_SEC_ORD_DEL_CTN";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                    objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                    objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                    objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                    objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                    objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                    objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                    objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                    objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                }
                if (reportType == "areasummarycarton")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PrimaryVsSecondary/PrimaryVsSecondaryAreaCartonSummary.rdlc");
                    objCmd.CommandText = "PKG_REPORT_PRI_VS_SECON.RPI_TAR_LIFT_SEC_ORD_DEL_CTN";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                    objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                    objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                    objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                    objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                    objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                    objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                    objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                    objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                }

                if (reportType == "areadetail")
                { 
                LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PrimaryVsSecondary/PrimaryVsSecondaryAreaDetail.rdlc");
                    objCmd.CommandText = "PKG_REPORT_PRI_VS_SECON.RPI_TAR_LIFT_SEC_ORD_DEL";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                    objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                    objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                    objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                    objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                    objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                    objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                    objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                    objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                }
                if (reportType == "areasummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PrimaryVsSecondary/PrimaryVsSecondaryAreaSummary.rdlc");
                    objCmd.CommandText = "PKG_REPORT_PRI_VS_SECON.RPI_TAR_LIFT_SEC_ORD_DEL";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                    objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                    objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                    objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                    objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                    objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                    objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                    objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                    objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                }
                

                DataTable dt = classDt.GetData(objCmd);

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);

                ReportDataSource datasource = new ReportDataSource("DSPrimaryVsSecondary", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}