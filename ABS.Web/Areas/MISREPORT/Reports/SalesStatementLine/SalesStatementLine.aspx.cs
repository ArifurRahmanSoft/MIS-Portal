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

namespace ABS.Web.Areas.MISREPORT.Reports.SalesStatementLine
{
    public partial class SalesStatementLine : System.Web.UI.Page
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
                string brandId = queryStrings[4];
                string productId = queryStrings[5];
                string salesCustomerId = queryStrings[6];
                string reportType = queryStrings[7];
                string reportType1 = queryStrings[8];
                string salesType = queryStrings[9];


                // activity monitoring
                string reportFilteringData = fromDate + "-" + toDate + "-" + salesLine + "-" + salesCompany + "-" + brandId + "-" + productId + "-" + salesCustomerId + "-" + reportType + "-" + salesType;

                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Sales Statment", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

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

                if (string.IsNullOrEmpty(brandId))
                    brandId = null;
                if (string.IsNullOrEmpty(productId))
                    productId = null;
                if (string.IsNullOrEmpty(salesCustomerId))
                    salesCustomerId = null;
                if (string.IsNullOrEmpty(salesCompany))
                    salesCompany = null;


                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                LocalReport LocalReport = new LocalReport();
                OracleCommand objCmd = new OracleCommand();

                if (salesType == "Consumer")
                {
                    if (reportType == "LineWise")
                    {
                        p4.Values.Add("Line Wise");
                        if (reportType1 == "Requisition")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/RequisitionPartyWiseDateWiseDOBase.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata";
                        }
                        if (reportType1 == "Gropwise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesStatementGroupWise.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata";
                        }
                        if (reportType1 == "ReqWise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.req_getdata";
                        }
                        if (reportType1 == "GropwiseMT")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesStatementRequisationWise.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.req_getdata";
                        }
                        if (reportType1 == "Cons_Details")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/ConsumerSalesStatementDetails.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_sales2_line";
                        }
                        if (reportType1 == "Cons_Summary")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/ConsumerSalesStatementSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_sales2_line";
                        }
                        if (reportType1 == "Cons_Item_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/ConsumerSalesItemWiseSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_sales2_line";
                        }
                        if (reportType1 == "Cons_Party_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/ConsumerSalesPartyWiseSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_sales2_line";
                        }
                        if (reportType1 == "Cons_Party_Qnt_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/ConsumerSalesPartyWiseQntySummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_sales2_line";
                        }
                    }
                    if (reportType == "CompanyWise")
                    {
                        p4.Values.Add("Company Wise");

                        if (reportType1 == "Requisition")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/RequisitionPartyWiseDateWiseDOBase.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_comp_wise";
                        }
                        if (reportType1 == "Gropwise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesStatementGroupWise.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_comp_wise";
                        }
                        if (reportType1 == "ReqWise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.req_getdata_comp";
                        }
                        if (reportType1 == "GropwiseMT")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesStatementRequisationWise.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.req_getdata_comp";
                        }
                        if (reportType1 == "Cons_Details")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/ConsumerSalesStatementDetails.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_sales2_comp";
                        }
                        if (reportType1 == "Cons_Summary")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/ConsumerSalesStatementSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_sales2_comp";
                        }
                        if (reportType1 == "Cons_Item_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/ConsumerSalesItemWiseSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_sales2_comp";
                        }
                        if (reportType1 == "Cons_Party_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/ConsumerSalesPartyWiseSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_sales2_comp";
                        }
                        if (reportType1 == "Cons_Party_Qnt_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/ConsumerSalesPartyWiseQntySummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_sales2_comp";
                        }
                    }
                }
                else if (salesType == "Bulk")
                {
                    if (reportType == "LineWise")
                    {
                        p4.Values.Add("Line Wise");
                        if (reportType1 == "Requisition")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/RequisitionPartyWiseDateWiseDOBase.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_bulk";
                        }
                        if (reportType1 == "Gropwise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesStatementGroupWise.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_bulk";
                        }
                        if (reportType1 == "ReqWise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_bulk";
                        }
                        if (reportType1 == "GropwiseMT")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesStatementRequisationWise.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_bulk";
                        }
                        if (reportType1 == "Cons_Details")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/BulkSalesStatementDetails.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_bulk";
                        }
                        if (reportType1 == "Cons_Summary")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/BulkSalesStatementSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_bulk";
                        }
                        if (reportType1 == "Cons_Item_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/BulkSalesItemWiseSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_bulk";
                        }
                        if (reportType1 == "Cons_Party_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/BulkSalesPartyWiseSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_bulk";
                        }
                        if (reportType1 == "Cons_Party_Qnt_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/BulkSalesPartyWiseQntySummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_bulk";
                        }
                    }
                    if (reportType == "CompanyWise")
                    {
                        p4.Values.Add("Company Wise");

                        if (reportType1 == "Requisition")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/RequisitionPartyWiseDateWiseDOBase.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_comp_bulk";
                        }
                        if (reportType1 == "Gropwise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesStatementGroupWise.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_comp_bulk";
                        }
                        if (reportType1 == "ReqWise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_comp_bulk";
                        }
                        if (reportType1 == "GropwiseMT")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/DailySalesStatementRequisationWise.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_comp_bulk";
                        }
                        if (reportType1 == "Cons_Details")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/BulkSalesStatementDetails.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_comp_bulk";
                        }
                        if (reportType1 == "Cons_Summary")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/BulkSalesStatementSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_comp_bulk";
                        }
                        if (reportType1 == "Cons_Item_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/BulkSalesItemWiseSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_comp_bulk";
                        }
                        if (reportType1 == "Cons_Party_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/BulkSalesPartyWiseSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_comp_bulk";
                        }
                        if (reportType1 == "Cons_Party_Qnt_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/BulkSalesPartyWiseQntySummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_comp_bulk";
                        }
                    }
                }
                else if (salesType == "All")
                {
                    if (reportType == "LineWise")
                    {
                        p4.Values.Add("Line Wise");
                        if (reportType1 == "Requisition")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/All_RequisitionPartyWiseDateWiseDOBase.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_all";
                        }
                        if (reportType1 == "Gropwise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/All_DailySalesStatementGroupWise.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_all";
                        }
                        if (reportType1 == "ReqWise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/All_DailySalesSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_all";
                        }
                        if (reportType1 == "GropwiseMT")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/All_DailySalesStatementRequisationWise.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_all";
                        }
                        if (reportType1 == "Cons_Details")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/All_SalesStatementDetails.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_all";
                        }
                        if (reportType1 == "Cons_Summary")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/All_SalesStatementSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_all";
                        }
                        if (reportType1 == "Cons_Item_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/All_SalesItemWiseSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_all";
                        }
                        if (reportType1 == "Cons_Party_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/All_SalesPartyWiseSummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_all";
                        }
                        if (reportType1 == "Cons_Party_Qnt_Summ")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesStatementLine/All_SalesPartyWiseQntySummary.rdlc");
                            objCmd.CommandText = "Repot_pack_sales_statement_summ.getdata_line_all";
                        }
                    }                    
                }

                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = salesLine;
                objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = salesCompany;
                objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = salesCustomerId;
                objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = brandId;
                objCmd.Parameters.Add("sel5", OracleDbType.Varchar2).Value = productId;
                objCmd.Parameters.Add("sel6", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("sel7", OracleDbType.Varchar2).Value = toDate;

                objCmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("SalesLine", dt);
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