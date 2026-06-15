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

namespace ABS.Web.Areas.MISREPORT.Reports.BrandWiseSalesMonthYearLine
{
    public partial class BrandWiseSalesMonthYearLine : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string UserName = "";
                //  string UserId = Session["UserID"].ToString();


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
                string salesLine = queryStrings[2];
                string salesCompany = queryStrings[3];
                string salesCustomerId = queryStrings[4];
                string reportType = queryStrings[5];
                string reportType0 = queryStrings[6];
                string reportType1 = queryStrings[7];



                // activity monitoring
                string reportFilteringData = fromDate + "-" + toDate + "-" + salesLine + "-" + salesCompany + "-" + salesCustomerId + "-" + reportType;

                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Brand Wise Sales Month Year Line", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                //if (Session["UserID"].ToString() == "08414") // rashed sir logic, stopped now, as team member increased
                //    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

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

                //if (salesCompany == "")
                //    salesCompany = null;

                //if (salesCustomerId == "")
                //    salesCustomerId = null;

                if (string.IsNullOrEmpty(salesCustomerId))
                    salesCustomerId = null;

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                LocalReport LocalReport = new LocalReport();
                OracleCommand objCmd = new OracleCommand();

                if (reportType == "LineWise")
                {
                    p4.Values.Add("Line Wise");
                    if (reportType1 == "brand")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesDaily_Brand.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesMonth_Brand.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesYear_Brand.rdlc");
                        }
                    } 
                    else if (reportType1 == "brand_mton")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesDaily_Brand_MTON.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesMonth_Brand_MTON.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesYear_Brand_MTON.rdlc");
                        }
                    }
                    else if (reportType1 == "category_mton")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesDaily_Category_MTON.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesMonth_Category_MTON.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesYear_Category_MTON.rdlc");
                        }
                    }
                    else if (reportType1 == "sku")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesDaily_SKU.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesMonth_SKU.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesYear_SKU.rdlc");
                        }
                    }
                    else if (reportType1 == "sku_mton")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesDaily_SKU_MTON.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesMonth_SKU_MTON.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesYear_SKU_MTON.rdlc");
                        }
                    }
                    else if (reportType1 == "partybrand")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Daily_BrandPartyDetailMTonWithFree.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Monthly_BrandPartyDetailMTonWithFree.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Yearly_BrandPartyDetailMTonWithFree.rdlc");
                        }
                    }
                    else if (reportType1 == "partybrandaream")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Daily_BrandPartyAreaMTonWithFree.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Monthly_BrandPartyAreaMTonWithFree.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Yearly_BrandPartyAreaMTonWithFree.rdlc");
                        }
                    }
                    else if (reportType1 == "partybrandareav")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Daily_BrandPartyAreaValueWithFree.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Monthly_BrandPartyAreaValueWithFree.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Yearly_BrandPartyAreaValueWithFree.rdlc");
                        }
                    }
                    else if (reportType1 == "partybrandsku")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Daily_BrandSKUPartyDetailMTonWithFree.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Monthly_BrandSKUPartyDetailMTonWithFree.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Yearly_BrandSKUPartyDetailMTonWithFree.rdlc");
                        }
                    }

                    if (reportType1 == "brand" || reportType1 == "brand_mton" || reportType1 == "sku" || reportType1 == "sku_mton")
                    {
                        objCmd.CommandText = "SALES_STATEMENT_LINE.BrandWiseSalesMonthYearLine";
                    }
                    else if (reportType1 == "partybrand" || reportType1== "partybrandsku")
                    {
                        objCmd.CommandText = "SALES_STATEMENT_LINE.BrandPartySkuWiseSalesMonthYearLine";
                    }
                    else if (reportType1 == "category_mton")
                    {
                        objCmd.CommandText = "SALES_STATEMENT_LINE.CategoryWiseSalesMonthYearLine";
                    }
                    else if (reportType1 == "partybrandaream" || reportType1 == "partybrandareav")
                    {
                        objCmd.CommandText = "SALES_STATEMENT_LINE.BrandPartySkuAreaSalesMonthYearLine";
                    }
                }
                if (reportType == "CompanyWise")
                {
                    p4.Values.Add("Company Wise");
                    if (reportType1 == "brand")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesDaily_Brand.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesMonth_Brand.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesYear_Brand.rdlc");
                        }
                    }
                    else if (reportType1 == "brand_mton")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesDaily_Brand_MTON.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesMonth_Brand_MTON.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesYear_Brand_MTON.rdlc");
                        }
                    }
                    else if (reportType1 == "category_mton")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesDaily_Category_MTON.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesMonth_Category_MTON.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesYear_Category_MTON.rdlc");
                        }
                    }
                    else if (reportType1 == "sku")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesDaily_SKU.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesMonth_SKU.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesYear_SKU.rdlc");
                        }
                    }
                    else if (reportType1 == "sku_mton")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesDaily_SKU_MTON.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesMonth_SKU_MTON.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/BrandWiseSalesYear_SKU_MTON.rdlc");
                        }
                    }
                    else if (reportType1 == "partybrand")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Daily_BrandPartyDetailMTonWithFree.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Monthly_BrandPartyDetailMTonWithFree.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Yearly_BrandPartyDetailMTonWithFree.rdlc");
                        }
                    }
                    //else if (reportType1 == "partybrandarea")
                    //{
                    //    if (reportType0 == "daily")
                    //    {
                    //        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Daily_BrandPartyAreaMTonWithFree.rdlc");
                    //    }
                    //    else if (reportType0 == "monthly")
                    //    {
                    //        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Monthly_BrandPartyAreaMTonWithFree.rdlc");
                    //    }
                    //    else if (reportType0 == "yearly")
                    //    {
                    //        LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Yearly_BrandPartyAreaMTonWithFree.rdlc");
                    //    }
                    //}
                    else if (reportType1 == "partybrandsku")
                    {
                        if (reportType0 == "daily")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Daily_BrandSKUPartyDetailMTonWithFree.rdlc");
                        }
                        else if (reportType0 == "monthly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Monthly_BrandSKUPartyDetailMTonWithFree.rdlc");
                        }
                        else if (reportType0 == "yearly")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/BrandWiseSalesMonthYearLine/Yearly_BrandSKUPartyDetailMTonWithFree.rdlc");
                        }
                    }

                    if (reportType1 == "brand" || reportType1 == "brand_mton" || reportType1 == "sku" || reportType1 == "sku_mton")
                    {
                        objCmd.CommandText = "SALES_STATEMENT_LINE.BrandWiseSalesMonthYearCom";
                    }
                    else if (reportType1 == "partybrand" || reportType1 == "partybrandsku")
                    {
                        objCmd.CommandText = "SALES_STATEMENT_LINE.BrandPartySkuWiseSalesMonthYearCom";
                    }
                    else if (reportType1 == "category_mton")
                    {
                        objCmd.CommandText = "SALES_STATEMENT_LINE.CategoryWiseSalesMonthYearCom";
                    }
                    //else if (reportType1 == "partybrandarea")
                    //{
                    //    objCmd.CommandText = "SALES_STATEMENT_LINE.BrandPartySkuAreaSalesMonthYearCom";
                    //}
                }

                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = salesLine;
                objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = salesCompany;
                objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = salesCustomerId;
                objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("sel5", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("sel6", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("sel7", OracleDbType.Varchar2).Value = null;

                objCmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DTBrandWiseSalesLine", dt);
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });

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