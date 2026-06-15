using ABS.Web.Utility;
using CTGroup.Utility.Common;
using CTGroup.Utility;
using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json;
using CTGroup.OracleModel.ViewModel.Sales;

namespace ABS.Web.Areas.MISREPORT.Reports.DailySalesSummary
{
    public partial class DailySalesSummary : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserFullName"] == null || Session["UserFullName"] == "undefined")
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }
                string UserName = "";
                string UserId = Session["UserID"].ToString();
                UserName = Session["UserFullName"].ToString();

                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string nationalId = queryStrings[2];
                string divisionId = queryStrings[3];
                string regionId = queryStrings[4];
                string zoneId = queryStrings[5];
                string distributorId = queryStrings[6];

                string brandId = queryStrings[7];
                string isBrndGroup = queryStrings[8];
                string brndGroupId = queryStrings[9];

                string reportFilter = queryStrings[10];
                string reportType = queryStrings[11];

                Utils utility = new Utils();
                if (utility.IsNotValidActivity(UserId, nationalId))
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }

                //string spath = HostingEnvironment.MapPath("~/Content/xml/");
                ////string workingDirectory = Environment.CurrentDirectory;
                ////string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
                ////string projectDirectorys = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                //string fileName = "DeniedDesignation.xml";
                //string path = Path.Combine(spath, fileName);
                //DataSet deniedDesigDS = new DataSet();
                //deniedDesigDS.ReadXml(path);
                //DataTable dtDeniedDesig = deniedDesigDS.Tables[0];
                //dynamic str = JsonConvert.SerializeObject(dtDeniedDesig);
                //List<vmDeniedDesignation> listDndDesig = JsonConvert.DeserializeObject<List<vmDeniedDesignation>>(str);
                //var str = Conversion.GetJsonFromDataTable(dtDeniedDesig);

                // activity monitoring
                string reportFilteringData = reportFilter + "-" + reportType + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Daily Sales Summary", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

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

                if (brndGroupId == "")
                    brndGroupId = null;

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                LocalReport LocalReport = new LocalReport();

                string loggedDesig = string.Empty;
                DataTable BlockedDesig = new DataTable();
                DataTable UserDesig = new DataTable();
                OracleCommand objCmd = new OracleCommand();
                //START GET LOGGED USER SHORT DESIGNATION
                OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
                con.Open();
                string query = "select  SUBSTR(SP.SPRSN_SDESG,19,2) DESIGNATION from CITYN.T_SPRSN SP WHERE  " +
                               "SPRSN_TEXT= '" + Session["UserID"].ToString() + "' and sprsn_actv = 'Y'";
                OracleCommand cmd = new OracleCommand(query, con);
                using (OracleDataAdapter a = new OracleDataAdapter(cmd))
                {
                    a.Fill(UserDesig);
                }
                con.Close();
                //END GET LOGGED USER SHORT DESIGNATION

                //START GET BLOCKED SHORT DESIGNATION
                if (UserDesig.Rows.Count > 0)
                {
                    loggedDesig = UserDesig.Rows[0]["DESIGNATION"].ToString();
                    OracleConnection conB = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
                    conB.Open();
                    string queryB = "SELECT SHORT_DESIG BDESIG from T_BLOCKED_DESIG WHERE ISBLOCK_DESIG='1' AND SHORT_DESIG='" + loggedDesig + "'";
                    OracleCommand cmdB = new OracleCommand(queryB, conB);
                    using (OracleDataAdapter b = new OracleDataAdapter(cmdB))
                    {
                        b.Fill(BlockedDesig);
                    }
                    conB.Close();
                }
                //END GET BLOCKED SHORT DESIGNATION

                if (reportType == "reqbased")
                {
                    ///// ZO validation block
                    if (UserDesig.Rows.Count > 0)
                    {
                        if (BlockedDesig.Rows.Count > 0)
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummaryReqBased.rdlc");
                        }
                        else
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummaryReqBased_DistInvoice.rdlc");
                        }
                    }
                    else
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummaryReqBased_DistInvoice.rdlc");
                    }

                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_BCT.DAILY_SALES_SUMMARY_REQ_BASED";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_JMW.DAILY_SALES_SUMMARY_REQ_BASED";
                    //}
                    //else
                    //{
                    objCmd.CommandText = "DAILY_SALES_SUMMARY_NEW.DAILY_SALES_SUMMARY_REQ_BASED";
                    //}
                }

                if (reportType == "reqbased_bulk")
                {
                    ///// ZO validation block
                    if (UserDesig.Rows.Count > 0)
                    {
                        if (BlockedDesig.Rows.Count > 0)
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummaryReqBased.rdlc");
                        }
                        else
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummaryReqBased_DistInvoice.rdlc");
                        }
                    }
                    else
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummaryReqBased_DistInvoice.rdlc");
                    }
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_BCT.DAILY_SALES_SUMMARY_REQ_BASED";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_JMW.DAILY_SALES_SUMMARY_REQ_BASED";
                    //}
                    //else
                    //{
                    objCmd.CommandText = "DAILY_SALES_SUMMARY_NEW.DAILY_SALES_SUMMARY_REQ_BASED_BULK";
                    //}
                }

                if (reportType == "dobased")
                {
                    ///// ZO validation block
                    if (UserDesig.Rows.Count > 0)
                    {
                        if (BlockedDesig.Rows.Count > 0)
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummary.rdlc");
                        }
                        else
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummary_DistInvoice.rdlc");
                        }
                    }
                    else
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummary_DistInvoice.rdlc");
                    }

                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_BCT.DAILY_SALES_SUMMARY_DO_BASED";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_JMW.DAILY_SALES_SUMMARY_DO_BASED";
                    //}
                    //else
                    //{
                    objCmd.CommandText = "DAILY_SALES_SUMMARY_NEW.DAILY_SALES_SUMMARY_DO_BASED";
                    //}
                }

                if (reportType == "dobased_bulk")
                {
                    ///// ZO validation block //was fixed for zo
                    if (UserDesig.Rows.Count > 0)
                    {
                        if (BlockedDesig.Rows.Count > 0)
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummary.rdlc");
                        }
                        else
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummary_DistInvoice.rdlc");
                        }
                    }
                    else
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/DailySalesSummary_DistInvoice.rdlc");
                    }

                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_BCT.DAILY_SALES_SUMMARY_DO_BASED";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_JMW.DAILY_SALES_SUMMARY_DO_BASED";
                    //}
                    //else
                    //{
                    objCmd.CommandText = "DAILY_SALES_SUMMARY_NEW.DAILY_SALES_SUMMARY_DO_BASED_BULK";
                    //}
                }

                if (reportType == "product_group_dobased")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/PGDOBasedReport.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_BCT.DAILY_SALES_SUMMARY_DO_BASED";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_JMW.DAILY_SALES_SUMMARY_DO_BASED";
                    //}
                    //else
                    //{
                    objCmd.CommandText = "DAILY_SALES_SUMMARY_NEW.DAILY_SALES_SUMMARY_DO_BASED";
                    //}
                }
                if (reportType == "product_group_reqbased")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/PGReqBasedReport.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_BCT.DAILY_SALES_SUMMARY_REQ_BASED";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_JMW.DAILY_SALES_SUMMARY_REQ_BASED";
                    //}
                    //else
                    //{
                    objCmd.CommandText = "DAILY_SALES_SUMMARY_NEW.DAILY_SALES_SUMMARY_REQ_BASED";
                    //}
                }
                if (reportType == "pack_size_wise_daily_sels")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/PACK_SIZE_WISE_DAILY_SALS_Report.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_BCT.PACK_SIZE_WISE_DAILY_SALES";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_JMW.PACK_SIZE_WISE_DAILY_SALES";
                    //}
                    //else
                    //{
                    objCmd.CommandText = "DAILY_SALES_SUMMARY_NEW.PACK_SIZE_WISE_DAILY_SALES";
                    //}
                }
                if (reportType == "daily_sales_consumer_with_bulk")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailySalesSummary/Daily_Sales_Consumer_With_Bulk_Report.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_BCT.cash_daily_sales";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "DAILY_SALES_SUMMARY_JMW.cash_daily_sales";
                    //}
                    //else
                    //{
                    objCmd.CommandText = "DAILY_SALES_SUMMARY_NEW.cash_daily_sales";
                    //}
                }

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("fresult", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("desig", OracleDbType.Varchar2).Value = loggedDesig;
                objCmd.Parameters.Add("fdat", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("tdat", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;

                //New Param
                objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                objCmd.Parameters.Add("isBrndGroup", OracleDbType.Varchar2).Value = isBrndGroup;
                objCmd.Parameters.Add("brndGroupId", OracleDbType.Varchar2).Value = brndGroupId;
                objCmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = UserId;

                DataTable dt = classDt.GetData(objCmd);

                if (reportType == "reqbased")
                {
                    ReportDataSource datasource = new ReportDataSource("DataSetDailySalesSummaryReqBased", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "reqbased_bulk")
                {
                    ReportDataSource datasource = new ReportDataSource("DataSetDailySalesSummaryReqBased", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "dobased")
                {
                    ReportDataSource datasource = new ReportDataSource("DataSetDailySalesSummary", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }
                if (reportType == "dobased_bulk")
                {
                    ReportDataSource datasource = new ReportDataSource("DataSetDailySalesSummary", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "product_group_dobased")
                {
                    ReportDataSource datasource = new ReportDataSource("DSSPGDOBased", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }
                if (reportType == "product_group_reqbased")
                {
                    ReportDataSource datasource = new ReportDataSource("DSSPGReqBased", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "pack_size_wise_daily_sels")
                {
                    ReportDataSource datasource = new ReportDataSource("DSS_PACK_SIZE_WISE_DAILY_SALS", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }
                if (reportType == "daily_sales_consumer_with_bulk")
                {
                    ReportDataSource datasource = new ReportDataSource("DSS_DAILY_SALES_CONSUMER_WITH_BULK", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

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