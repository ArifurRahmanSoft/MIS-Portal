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

namespace ABS.Web.Areas.MISREPORT.Reports.BrandWiseSalesStatement
{
    public partial class BrandWiseSalesStatement : System.Web.UI.Page
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
                                       //Response.Redirect("");
                                       //Server.Transfer("http://localhost:15812/Account/Login");
                                       //Response.Redirect("http://localhost:15812/Account/Login");
                    Response.Redirect("~/Account/Login");
                }

                string UserName = "";
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
                string reportOption = queryStrings[8];
                string brandId = queryStrings[9];
                string productId = queryStrings[10];
                string groupId = queryStrings[11];
                string isBrndGroup = queryStrings[12];
                string brndGroupId = queryStrings[13];
                string companyId = queryStrings[14];

                Utils utility = new Utils();
                if (utility.IsNotValidActivity(UserId, nationalId))
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }

                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();

                //if (Session["UserID"].ToString() == "08414") // rashed sir logic, stopped now, as team member increased
                //    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

                // activity monitoring
                string reportFilteringData = reportFilter + "-" + fromDate + "-" + toDate;
                string autoId = reportActivityMonitoring.RequestStart("Brand Wise Sales Statment", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());


                UserName = Session["UserFullName"].ToString();

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

                if (groupId == "")
                    groupId = null;

                if (brndGroupId == "")
                    brndGroupId = null;

                if (reportOption == "1") // detail report
                {
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "PKG_SALES_BRANDWISE_BCT.SALES_AREA_DETAIL";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "PKG_SALES_BRANDWISE_JMW.SALES_AREA_DETAIL";
                    //}
                    //else
                    //{
                    //objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_AREA_DETAIL";
                    objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_AREA_DETAIL_NEW";
                    //}
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesStatement/BrandWiseSalesStatementSegregation.rdlc");
                }

                else if (reportOption == "3") // primary vs secondary
                {
                    if (nationalId == null) // without any national selection
                    {
                        //if (UserId == "07686") // tea - Iqbal Chowdhury
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_BCT.SALES_FA_PRI_SEC";
                        //}
                        //else if (UserId == "06443") // water - Ahmmed Ali
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_JMW.SALES_FA_PRI_SEC";
                        //}
                        //else
                        //{
                        objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_FA_PRI_SEC_NEW"; // combining all national data
                        //}
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesStatement/BRANDWISESALES_AREA_PRI_SECON_NEW.rdlc");
                    }
                    else
                    {
                        //if (UserId == "07686") // tea - Iqbal Chowdhury
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_BCT.SALES_AREA_PRI_SEC";
                        //}
                        //else if (UserId == "06443") // water - Ahmmed Ali
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_JMW.SALES_AREA_PRI_SEC";
                        //}
                        //else
                        //{
                        //objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_AREA_PRI_SEC_NEW";  // data for selected national
                        //objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_AREA_PRIMARY_SECOND_NEW";  // data for selected national
                        objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_AREA_PRIMARY_SECOND_NEW";  // data for selected national
                        //}
                        //LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesStatement/BRANDWISESALES_AREA_PRI_SECON.rdlc");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesStatement/BRANDWISESALES_AREA_PRI_SECON_NEW.rdlc");
                    }
                }

                else if (reportOption == "4") //sku wise report
                {
                    if (nationalId == null)
                    {
                        //if (UserId == "07686") // tea - Iqbal Chowdhury
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_BCT.SALES_FA";
                        //}
                        //else if (UserId == "06443") // water - Ahmmed Ali
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_JMW.SALES_FA";
                        //}
                        //else
                        //{
                        //objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_FA"; // combining all national data
                        objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_FA_NEW"; // combining all national data
                        //}
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesStatement/BrandWiseSalesStatementSku.rdlc");
                    }
                    else
                    {
                        //if (UserId == "07686") // tea - Iqbal Chowdhury
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_BCT.SALES_AREA";
                        //}
                        //else if (UserId == "06443") // water - Ahmmed Ali
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_JMW.SALES_AREA";
                        //}
                        //else
                        //{
                        //objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_AREA";  // data for selected national
                        objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_AREA_NEW";  // data for selected national
                        //}
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesStatement/BrandWiseSalesStatementSku.rdlc");
                    }
                }

                else   // summary report
                {
                    if (nationalId == null)
                    {
                        //if (UserId == "07686") // tea - Iqbal Chowdhury
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_BCT.SALES_FA";
                        //}
                        //else if (UserId == "06443") // water - Ahmmed Ali
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_JMW.SALES_FA";
                        //}
                        //else
                        //{
                        //objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_FA"; // combining all national data
                        objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_FA_NEW"; // combining all national data
                        //}
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesStatement/BrandWiseSalesStatementSummary.rdlc");
                    }
                    else
                    {
                        //if (UserId == "07686") // tea - Iqbal Chowdhury
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_BCT.SALES_AREA";
                        //}
                        //else if (UserId == "06443") // water - Ahmmed Ali
                        //{
                        //    objCmd.CommandText = "PKG_SALES_BRANDWISE_JMW.SALES_AREA";
                        //}
                        //else
                        //{
                        //objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_AREA";  // data for selected national
                        objCmd.CommandText = "PKG_SALES_BRANDWISE_DEVELOPMENT.SALES_AREA_NEW";  // data for selected national
                        //}
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesStatement/BrandWiseSalesStatementSummary.rdlc");
                    }
                }
                // SKU Wise Report



                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = companyId;
                objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;


                objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                objCmd.Parameters.Add("groupId", OracleDbType.Varchar2).Value = groupId;

                //New Param
                objCmd.Parameters.Add("isBrndGroup", OracleDbType.Varchar2).Value = isBrndGroup;
                objCmd.Parameters.Add("brndGroupId", OracleDbType.Varchar2).Value = brndGroupId;
                objCmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = UserId;

                DataTable dt = classDt.GetData(objCmd);

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                LocalReport.SetParameters(p4);


                ReportDataSource datasource = new ReportDataSource("BrandWiseSales", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}