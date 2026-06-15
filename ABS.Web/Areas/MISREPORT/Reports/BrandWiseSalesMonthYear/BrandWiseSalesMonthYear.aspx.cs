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

namespace ABS.Web.Areas.MISREPORT.Reports.BrandWiseSalesMonthYear
{
    public partial class BrandWiseSalesMonthYear : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
        ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
        string autoId = string.Empty;
        string reportFilteringData = string.Empty;

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
                string brandId = queryStrings[7];
                string isBrndGroup = queryStrings[8];
                string brndGroupId = queryStrings[9];
                string reportFilter = queryStrings[10];
                string reportType = queryStrings[11];
                string reportData = queryStrings[12];
                string companyId = queryStrings[13];


                if (Session["UserID"].ToString() == "08414")
                    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

                reportFilteringData = reportFilter + "-" + reportType + "-" + fromDate + "-" + toDate;

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

                if (reportType == "1")
                {
                    if (reportData == "brand")
                    {
                        autoId = WritingReportStartActivity("Brand-Wise Sales Month");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesMonth_Brand.rdlc");
                    }
                    if (reportData == "brand_mton")
                    {
                        autoId = WritingReportStartActivity("Brand-Wise Sales Month");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesMonth_Brand_MTON.rdlc");
                    }
                    else if (reportData == "party")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-Wise Sales Month");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Monthly_BrandPartyDetailMTonWithFree.rdlc");
                    }
                    else if (reportData == "pbs")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-SKU-Wise Sales Month");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Monthly_BrandSKUPartyDetailMTonWithFree.rdlc");
                    }
                    else if (reportData == "pbsam")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-Area-Wise Sales Month");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Monthly_BrandPartyAreaMTonWithFree.rdlc");
                    }
                    else if (reportData == "pbsav")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-Area-Wise Sales Month");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Monthly_BrandPartyAreaValueWithFree.rdlc");
                    }
                    else if (reportData == "sku")
                    {
                        autoId = WritingReportStartActivity("SKU-Wise Sales Month");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesMonth_SKU.rdlc");
                    }
                    else if (reportData == "sku_mton")
                    {
                        autoId = WritingReportStartActivity("SKU-Wise Sales Month");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesMonth_SKU_MTON.rdlc");
                    }
                }
                if (reportType == "2")
                {
                    if (reportData == "brand")
                    {
                        autoId = WritingReportStartActivity("Brand-Wise Sales Year");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesYear_Brand.rdlc");
                    }
                    if (reportData == "brand_mton")
                    {
                        autoId = WritingReportStartActivity("Brand-Wise Sales Year");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesYear_Brand_MTON.rdlc");
                    }
                    else if (reportData == "party")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-Wise Sales Year");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Yearly_BrandPartyDetailMTonWithFree.rdlc");
                    }
                    else if (reportData == "pbs")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-Wise Sales Year");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Yearly_BrandSKUPartyDetailMTonWithFree.rdlc");
                    }
                    else if (reportData == "pbsam")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-Area-Wise Sales Year");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Yearly_BrandPartyAreaMTonWithFree.rdlc");
                    }
                    else if (reportData == "pbsav")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-SKU-Area-Wise Sales Year");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Yearly_BrandPartyAreaValueWithFree.rdlc");
                    }
                    else if (reportData == "sku")
                    {
                        autoId = WritingReportStartActivity("SKU-Wise Sales Year");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesYear_SKU.rdlc");
                    }
                    else if (reportData == "sku_mton")
                    {
                        autoId = WritingReportStartActivity("SKU-Wise Sales Year");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesYear_SKU_MTON.rdlc");
                    }
                }
                if (reportType == "3")
                {
                    if (reportData == "brand")
                    {
                        autoId = WritingReportStartActivity("Brand-Wise Sales Daily");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesDaily_Brand.rdlc");
                    }
                    if (reportData == "brand_mton")
                    {
                        autoId = WritingReportStartActivity("Brand-Wise Sales Daily");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesDaily_Brand_MTON.rdlc");
                    }
                    else if (reportData == "party")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-Wise Sales Daily");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Daily_BrandPartyDetailMTonWithFree.rdlc");
                    }
                    else if (reportData == "pbs")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-SKU-Wise Sales Daily");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Daily_BrandSKUPartyDetailMTonWithFree.rdlc");
                    }
                    else if (reportData == "pbsam")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-Area-Wise Sales Daily");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Daily_BrandPartyAreaMTonWithFree.rdlc");
                    }
                    else if (reportData == "pbsav")
                    {
                        autoId = WritingReportStartActivity("Party-Brand-Area-Wise Sales Daily");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/Daily_BrandPartyAreaValueWithFree.rdlc");
                    }
                    else if (reportData == "sku")
                    {
                        autoId = WritingReportStartActivity("SKU-Wise Sales Daily");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesDaily_SKU.rdlc");
                    }
                    else if (reportData == "sku_mton")
                    {
                        autoId = WritingReportStartActivity("SKU-Wise Sales Daily");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesDaily_SKU_MTON.rdlc");
                    }
                }
                if (reportType == "4")
                {
                    if (reportData == "brand")
                    {
                        autoId = WritingReportStartActivity("Brand-Wise Sales Month");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesMonth_Brand_Plain.rdlc");
                    }
                    else
                    {
                        autoId = WritingReportStartActivity("SKU-Wise Sales Month");
                        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYear/BrandWiseSalesMonth_SKU_Plain.rdlc");
                    }
                }

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

                if (brndGroupId == "")
                    brndGroupId = null;

                if (reportData == "brand" || reportData == "brand_mton" || reportData == "sku" || reportData == "sku_mton")
                {
                    if (nationalId == "" || nationalId == null)
                    {
                        //objCmd.CommandText = "SALES_VARIATIONS.BrandwiseSalesMonthYear_FA";
                        objCmd.CommandText = "SALES_VARIATIONS.BrandwiseSalesMonthYear_New_FA";
                    }
                    else
                    {
                        //objCmd.CommandText = "SALES_VARIATIONS.BrandwiseSalesMonthYear_New";
                        objCmd.CommandText = "SALES_VARIATIONS.BrandwiseSalesMonthYear_New_One";
                    }
                }
                else if (reportData == "party" || reportData == "pbs")
                {
                    if (nationalId == "" || nationalId == null)
                    {
                        //objCmd.CommandText = "SALES_VARIATIONS.BrandwiseSalesMonthYear_FA";
                        objCmd.CommandText = "SALES_VARIATIONS.BrandPartyWiseSalesMonthYear_FA";
                    }
                    else
                    {
                        //objCmd.CommandText = "SALES_VARIATIONS.BrandwiseSalesMonthYear_New";
                        objCmd.CommandText = "SALES_VARIATIONS.BrandPartyWiseSalesMonthYear";
                    }
                }
                else if (reportData == "pbsam" || reportData == "pbsav")
                {
                    objCmd.CommandText = "SALES_VARIATIONS.BrandPartySkuAreaSalesMonthYear";
                }


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

                //New Param
                objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                objCmd.Parameters.Add("isBrndGroup", OracleDbType.Varchar2).Value = isBrndGroup;
                objCmd.Parameters.Add("brndGroupId", OracleDbType.Varchar2).Value = brndGroupId;
                objCmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = UserId;

                DataTable dt = classDt.GetData(objCmd);

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);

                //LocalReport.ReleaseSandboxAppDomain();
                //ReportViewer1.Dispose();

                ReportDataSource datasource = new ReportDataSource("DTBrandWiseSales", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                reportActivityMonitoring.RequestEnd(autoId);
                LocalReport.Dispose();


            }
        }

        private string WritingReportStartActivity(string reportName)
        {
            string autoId = reportActivityMonitoring.RequestStart(reportName, reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());
            return autoId;
        }
    }
}