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

namespace ABS.Web.Areas.ACCOUNTS.Reports.GeneralLedgerSubHeadWise
{
    public partial class GeneralLedgerSubHeadWise : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string UserName = "";
                string UserID = "";
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
                UserID= Session["UserID"].ToString();

                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string company = queryStrings[0];
                string company1 = queryStrings[1];
                string fromDate = queryStrings[2];
                string toDate = queryStrings[3];
                string rptCate = queryStrings[4];
                string rptMode = queryStrings[5];
                string lvl0 = queryStrings[6];
                string lvl1 = queryStrings[7];
                string lvl2 = queryStrings[8];
                string lvl3 = queryStrings[9];
                string rptType = queryStrings[10];

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter(); 

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);
                p2.Name = "ToDate";
                p2.Values.Add(toDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);

                if(company== "undefined" || company == "")
                {
                    company = null;
                }
                if (company1 == "undefined" || company1 == "")
                {
                    company1 = null;
                }

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();
                string proc = "";
                if (string.IsNullOrEmpty(company))
                {
                    if (rptCate == "general")
                    {
                        proc = "GET_GENERAL_LEDGER_SUBHEADWISE";
                        if (rptMode == "monthly")
                        {
                            if (rptType == "level0")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Monthly/GeneralLedgerSubHeadWise_Monthly_l0.rdlc");
                            }
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Monthly/GeneralLedgerSubHeadWise_Monthly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Monthly/GeneralLedgerSubHeadWise_Monthly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Monthly/GeneralLedgerSubHeadWise_Monthly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Monthly/GeneralLedgerSubHeadWise_Monthly_l4.rdlc");
                            }
                        }
                        if (rptMode == "quarterly")
                        {
                            if (rptType == "level0")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l0.rdlc");
                            }
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l4.rdlc");
                            }
                        }
                        if (rptMode == "halfyearly")
                        {
                            if (rptType == "level0")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l0.rdlc");
                            }
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l4.rdlc");
                            }
                        }
                        if (rptMode == "yearly")
                        {
                            if (rptType == "level0")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Yearly/GeneralLedgerSubHeadWise_Yearly_l0.rdlc");
                            }
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Yearly/GeneralLedgerSubHeadWise_Yearly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Yearly/GeneralLedgerSubHeadWise_Yearly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Yearly/GeneralLedgerSubHeadWise_Yearly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/AllCompany/General/Yearly/GeneralLedgerSubHeadWise_Yearly_l4.rdlc");
                            }
                        }
                    }
                }
                else
                {
                    if (rptCate == "general")
                    {
                        proc = "GET_GENERAL_LEDGER_SUBHEADWISE";
                        if (rptMode == "monthly")
                        {
                            if (rptType == "level0")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Monthly/GeneralLedgerSubHeadWise_Monthly_l0.rdlc");
                            }
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Monthly/GeneralLedgerSubHeadWise_Monthly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Monthly/GeneralLedgerSubHeadWise_Monthly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Monthly/GeneralLedgerSubHeadWise_Monthly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Monthly/GeneralLedgerSubHeadWise_Monthly_l4.rdlc");
                            }
                        }
                        if (rptMode == "quarterly")
                        {
                            if (rptType == "level0")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l0.rdlc");
                            }
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l4.rdlc");
                            }
                        }
                        if (rptMode == "halfyearly")
                        {
                            if (rptType == "level0")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l0.rdlc");
                            }
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l4.rdlc");
                            }
                        }
                        if (rptMode == "yearly")
                        {
                            if (rptType == "level0")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Yearly/GeneralLedgerSubHeadWise_Yearly_l0.rdlc");
                            }
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Yearly/GeneralLedgerSubHeadWise_Yearly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Yearly/GeneralLedgerSubHeadWise_Yearly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Yearly/GeneralLedgerSubHeadWise_Yearly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/General/Yearly/GeneralLedgerSubHeadWise_Yearly_l4.rdlc");
                            }
                        }
                    }

                    if (rptCate == "comparison")
                    {
                        proc = "GET_GENERAL_LEDGER_SUBHEADWISE2";
                        if (rptMode == "monthly")
                        {
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Monthly/GeneralLedgerSubHeadWise_Monthly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Monthly/GeneralLedgerSubHeadWise_Monthly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Monthly/GeneralLedgerSubHeadWise_Monthly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Monthly/GeneralLedgerSubHeadWise_Monthly_l4.rdlc");
                            }
                        }
                        if (rptMode == "quarterly")
                        {
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Quarterly/GeneralLedgerSubHeadWise_Quarterly_l4.rdlc");
                            }
                        }
                        if (rptMode == "halfyearly")
                        {
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/HalfYearly/GeneralLedgerSubHeadWise_HalfYearly_l4.rdlc");
                            }
                        }
                        if (rptMode == "yearly")
                        {
                            if (rptType == "level1")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Yearly/GeneralLedgerSubHeadWise_Yearly_l1.rdlc");
                            }
                            if (rptType == "level2")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Yearly/GeneralLedgerSubHeadWise_Yearly_l2.rdlc");
                            }
                            if (rptType == "level3")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Yearly/GeneralLedgerSubHeadWise_Yearly_l3.rdlc");
                            }
                            if (rptType == "level4")
                            {
                                LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/GeneralLedgerSubHeadWise/CompanyWise/Comparison/Yearly/GeneralLedgerSubHeadWise_Yearly_l4.rdlc");
                            }
                        }
                    }
                }

                OracleCommand objCmd = new OracleCommand();

                objCmd.CommandText = "PKG_ACC_GENERAL_LEDGER."+ proc;
             
                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("t_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("P_MCOM_OID", OracleDbType.Varchar2).Value = company;
                objCmd.Parameters.Add("P_CCOM_OID", OracleDbType.Varchar2).Value = company1;
                objCmd.Parameters.Add("p_startdate", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("p_enddate", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("lvl0", OracleDbType.Varchar2).Value = lvl0;
                objCmd.Parameters.Add("lvl1", OracleDbType.Varchar2).Value = lvl1;
                objCmd.Parameters.Add("lvl2", OracleDbType.Varchar2).Value = lvl2;
                objCmd.Parameters.Add("lvl3", OracleDbType.Varchar2).Value = lvl3;
                objCmd.Parameters.Add("p_userId", OracleDbType.Varchar2).Value = UserID;

                DataTable dt = classDt.GetData(objCmd);

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);

                ReportDataSource datasource = new ReportDataSource("ds_GenLedgCom", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3});
            }
        }
    }
}