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

namespace ABS.Web.Areas.MISREPORT.Reports.UndeliveredStatement
{
    public partial class UndeliveredStatementBak : System.Web.UI.Page
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
                string divisionId = queryStrings[3];
                string regionId = queryStrings[4];
                string zoneId = queryStrings[5];
                string distributorId = queryStrings[6];
                string reportFilter = queryStrings[7];
                string reportType = queryStrings[8];
                string brandId = queryStrings[9];
                string productId = queryStrings[10];

                Utils utility = new Utils();
                if (utility.IsNotValidActivity(UserId, nationalId))
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }
                // activity monitoring
                string reportFilteringData = reportFilter + "-" + reportType + "-" + brandId + "-" + productId +  "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Undelivered Statment", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

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

                //if (company == "" || company == "undefined")
                //    company = null;

                if (brandId == "" || brandId == "undefined")
                    brandId = null;

                if (productId == "" || productId == "undefined")
                    productId = null;

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                if (reportType == "detail")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/UndeliveredStatement/ReportUSDetails.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_BCT.summary_detail";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_JMW.summary_detail";
                    }
                    else
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING.summary_detail";
                    }
                }
                if (reportType == "summary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/UndeliveredStatement/ReportUndeliveredStatement.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_BCT.summary_detail";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_JMW.summary_detail";
                    }
                    else
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING.summary_detail";
                    }
                }
                if (reportType == "partyxproduct")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/UndeliveredStatement/ReportUndeliveredStatementXParty.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_BCT.PARTY_X_PRODUCT";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_JMW.PARTY_X_PRODUCT";
                    }
                    else
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING.PARTY_X_PRODUCT";
                    }
                }
                if (reportType == "partyxproductxbrand")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/UndeliveredStatement/ReportUndeliveredStatementXPartyByBrand.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_BCT.PARTY_X_PRODUCT";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_JMW.PARTY_X_PRODUCT";
                    }
                    else
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING.PARTY_X_PRODUCT";
                    }
                }
                if (reportType == "areasummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/UndeliveredStatement/ReportUndeliveredStatementAreaWise.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_BCT.UNDELI_AREA_WIESE";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_JMW.UNDELI_AREA_WIESE";
                    }
                    else
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING.UNDELI_AREA_WIESE";
                    }
                }
                if (reportType == "areadetail")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/UndeliveredStatement/ReportUndeliveredStatementAreaWiseDetail.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_BCT.UNDELI_AREA_WIESE";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_JMW.UNDELI_AREA_WIESE";
                    }
                    else
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING.UNDELI_AREA_WIESE";
                    }
                }
                if (reportType == "areadetailnew")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/UndeliveredStatement/ReportUndeliveredStatementAreaWiseDetail_New.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_BCT.UNDELI_AREA_WIESE";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_JMW.UNDELI_AREA_WIESE";
                    }
                    else
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING.UNDELI_AREA_WIESE_NEW";
                    }
                }
                if (reportType == "summaryton")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/UndeliveredStatement/ReportUndeliveredStatementAreaWiseTon.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_BCT.UNDELI_AREA_WIESE";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_JMW.UNDELI_AREA_WIESE";
                    }
                    else
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING.UNDELI_AREA_WIESE";
                    }
                }
                if (reportType == "detailton")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/UndeliveredStatement/ReportUndeliveredStatementAreaWiseDetailTon.rdlc");
                    if (UserId == "07686") // tea - Iqbal Chowdhury
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_BCT.UNDELI_AREA_WIESE";
                    }
                    else if (UserId == "06443") // water - Ahmmed Ali
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING_JMW.UNDELI_AREA_WIESE";
                    }
                    else
                    {
                        objCmd.CommandText = "MONIR_UND_TESTING.UNDELI_AREA_WIESE";
                    }
                }

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

                DataTable dt = classDt.GetData(objCmd);


                if (reportType == "detail")
                {
                    ReportDataSource datasource = new ReportDataSource("DDS_US_DETAILS", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "summary")
                {
                    ReportDataSource datasource = new ReportDataSource("DDSUndeliveredStatement", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "partyxproduct")
                {
                    ReportDataSource datasource = new ReportDataSource("DDSUndeliveredStatementXParty", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }
                if (reportType == "partyxproductxbrand")
                {
                    ReportDataSource datasource = new ReportDataSource("DDSUndeliveredStatementXParty", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }
                if (reportType == "areasummary")
                {
                    ReportDataSource datasource = new ReportDataSource("UndeliveryAreaWise", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }
                if (reportType == "areadetail")
                {
                    ReportDataSource datasource = new ReportDataSource("UndeliveryAreaWise", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }
                if (reportType == "areadetailnew")
                {
                    ReportDataSource datasource = new ReportDataSource("UndeliveryAreaWise", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "summaryton")
                {
                    ReportDataSource datasource = new ReportDataSource("DSUndelAreaBrandWiseTon", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "detailton")
                {
                    ReportDataSource datasource = new ReportDataSource("DSUndelAreaBrandWiseTon", dt);
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