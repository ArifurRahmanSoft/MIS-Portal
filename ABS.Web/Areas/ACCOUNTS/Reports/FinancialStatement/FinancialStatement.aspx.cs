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

namespace ABS.Web.Areas.ACCOUNTS.Reports.FinancialStatement
{
    public partial class FinancialStatement : System.Web.UI.Page
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
                UserID= Session["UserID"].ToString();//queryStringUDS

                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string location = queryStrings[0];
                string mCompany = queryStrings[1];
                string fromYear = queryStrings[2];
                string toYear = queryStrings[3];//locProject
                string fromMonth = queryStrings[4];
                string toMonth = queryStrings[5];
               // string fromDate = queryStrings[6];
                //string toDate = queryStrings[7];
                string rptType = queryStrings[6];

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();

                string fromDate = fromMonth + "/" + fromYear;
                string toDate = toMonth + "/" + toYear;

                 p1.Name = "FromDate";
                 p1.Values.Add(fromDate);
                 p2.Name = "ToDate";
                 p2.Values.Add(toDate);
                 p3.Name = "UserName";
                 p3.Values.Add(UserName);

                if (location == "undefined" || location == "")
                {
                    location = null;
                }
                if (mCompany == "undefined" || mCompany == "")
                {
                    mCompany = null;
                }
                if (fromYear == "undefined" || fromYear == "")
                {
                    fromYear = null;
                }
                if (toYear == "undefined" || toYear == "")
                {
                    toYear = null;
                }
                if (fromMonth == "undefined" || fromMonth == "")
                {
                    fromMonth = null;
                }

                if (toMonth == "undefined" || toMonth == "")
                {
                    toMonth = null;
                }


                

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();
                string proc = "";
                OracleCommand objCmd = new OracleCommand();


                if (rptType == "BalanceSheet")
                {
                    objCmd.CommandText = "REPOT_FINANCIAL_STATEMENT.Get_Balance_Sheet";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/FinancialStatement/Balance_Sheet.rdlc");
                }

              /*  if (rptType == "inComTranverification")
                {
                    objCmd.CommandText = "Repot_trail_balance.getdata";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/TrialBalance/inComTranverification.rdlc");
                }*/
                if (rptType == "IncomeStatement")
                {
                    objCmd.CommandText = "Repot_income_statement_half_year.getdata_new";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/FinancialStatement/IncomeStatement.rdlc");
                }
                if (rptType == "TrialBlnceAcHead")
                {
                    objCmd.CommandText = "Repot_trail_balance.getdata";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/TrialBalance/TrialBalance _AC_Head_Wise.rdlc");
                }
                if (rptType == "TrialBlnceGlHead")
                {
                    objCmd.CommandText = "Repot_trail_balance.getdata_grp_trail";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/TrialBalance/TrialBalance_GL_Head_Wise.rdlc");
                }
               



                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = mCompany;
               
                objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = location;
                objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("sel5", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("sel6", OracleDbType.Varchar2).Value = fromDate;// "01-01-2024";//
                objCmd.Parameters.Add("sel7", OracleDbType.Varchar2).Value = toDate;// "01-11-2025"; //
                objCmd.Parameters.Add("sel8", OracleDbType.Varchar2).Value = null;
                //objCmd.Parameters.Add("p_userId", OracleDbType.Varchar2).Value = UserID;

                DataTable dt = classDt.GetData(objCmd);

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);

               // ReportDataSource datasource = new ReportDataSource("DtFinancialStatement", dt);//DataSet1
                ReportDataSource datasource = new ReportDataSource("DataSet1", dt);//

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3});
            }
        }
    }
}