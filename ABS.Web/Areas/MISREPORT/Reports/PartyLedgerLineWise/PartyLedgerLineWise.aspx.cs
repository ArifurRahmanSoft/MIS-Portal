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

namespace ABS.Web.Areas.MISREPORT.Reports.PartyLedgerLineWise
{
    public partial class PartyLedgerLineWise : System.Web.UI.Page
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
                string reportType1 = queryStrings[6];
                string salesType = queryStrings[7];
                //string ssTypeId = queryStrings[8];



                // activity monitoring
                string reportFilteringData = fromDate + "-" + toDate + "-" + salesLine + "-" + salesCompany + "-" + salesCustomerId + "-" + reportType + "-" + salesType;// + "-" + ssTypeId;

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

                if (string.IsNullOrEmpty(salesLine))
                    salesLine = null;

                if (string.IsNullOrEmpty(salesCompany))
                    salesCompany = null;

                if (string.IsNullOrEmpty(salesCustomerId))
                    salesCustomerId = null;

                //if (string.IsNullOrEmpty(ssTypeId))
                //    ssTypeId = null;

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                LocalReport LocalReport = new LocalReport();
                OracleCommand objCmd = new OracleCommand();

                if (salesType == "Consumer")
                {
                    if (reportType == "LineWise")
                    {
                        p4.Values.Add("Line Wise");
                        if (reportType1 == "Details")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/PartyLedgerRealDetailsLineWise.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_line_wise";
                        }
                        if (reportType1 == "Summary")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/PartyLedgerRealSummaryLineWise.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_line_wise";
                        }
                        if (reportType1 == "SumDueADV")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/PartyLedgerRealDueAdvLineWise.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.adv_due_line";
                        }
                        if (reportType1 == "SumDueADVAmo")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/PartyLedgerRealDueAmountLineWise.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.due_baln_line";
                        }

                        if (reportType1 == "CrateLedgerSummary")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/CrateLedger_Summary_Party.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.caret_ledger_line_wise";
                        }
                        if (reportType1 == "CrateLedgerDetails")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/CrateLedger_Details_Party.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.caret_ledger_line_wise";
                        }
                    }
                    if (reportType == "CompanyWise")
                    {
                        p4.Values.Add("CompanyWise");

                        if (reportType1 == "Details")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/PartyLedgerMoneyDetailsLineWise.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_comp_wise";
                        }
                        if (reportType1 == "Summary")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/PartyLedgerMoneySummaryLineWise.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_comp_wise";
                        }
                        if (reportType1 == "SumDueADV")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/PartyLedgerMoneyDueAdvanceLineWise.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.adv_due_comp";
                        }
                        if (reportType1 == "SumDueADVAmo")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/PartyLedgerMoneyDueAmountLineWise.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.due_baln_comp";
                        }
                    }
                }
                else if (salesType == "Bulk")
                {
                    if (reportType == "LineWise")
                    {
                        p4.Values.Add("Line Wise");
                        if (reportType1 == "SummaryParty")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/Bulk_PartyLedger_Summary_Party.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_line_bulk";
                        }
                        if (reportType1 == "DetailsParty")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/Bulk_PartyLedger_Details_Party.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_line_bulk";
                        }
                        if (reportType1 == "SummaryItem")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/Bulk_PartyLedger_Summary_Item.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_line_bulk";
                        }
                        if (reportType1 == "DetailsItem")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/Bulk_PartyLedger_Details_Item.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_line_bulk";
                        }
                    }
                    if (reportType == "CompanyWise")
                    {
                        p4.Values.Add("CompanyWise");
                        if (reportType1 == "SummaryParty")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/Bulk_PartyLedger_Summary_Party.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_comp_bulk";
                        }
                        if (reportType1 == "DetailsParty")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/Bulk_PartyLedger_Details_Party.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_comp_bulk";
                        }
                        if (reportType1 == "SummaryItem")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/Bulk_PartyLedger_Summary_Item.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_comp_bulk";
                        }
                        if (reportType1 == "DetailsItem")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/PartyLedgerLineWise/Bulk_PartyLedger_Details_Item.rdlc");
                            objCmd.CommandText = "Repot_pack_party_ledger.ledger_comp_bulk";
                        }
                    }
                }

                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = salesLine;
                objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = salesCompany;
                objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = salesCustomerId;
                objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;//ssTypeId;
                objCmd.Parameters.Add("sel5", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("sel6", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("sel7", OracleDbType.Varchar2).Value = null;

                objCmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSPartyLedgerLine", dt);
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