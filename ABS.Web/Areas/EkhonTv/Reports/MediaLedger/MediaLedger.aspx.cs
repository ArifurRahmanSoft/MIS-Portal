using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using CTGroup.Utility.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CTGroup.Web.Areas.Expense.Reports.MediaLedger
{
    public partial class MediaLedger : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserFullName"] == null || Session["UserFullName"] == "undefined")
                {
                    Session.Abandon();
                    Session.Clear();
                    Response.Redirect("~/Account/Login");
                }

                string UserName = Session["UserFullName"].ToString();

                string[] queryStrings = Request["queryString"].ToString().Split(',');
           
                    string CompanyId = queryStrings[0];
                    string AgentID = queryStrings[1];
                    string ClientID = queryStrings[2];
                    string reportType = queryStrings[3];
                    string FromDate = queryStrings[4];
                    string to_date = queryStrings[5];
                    string sel6 = queryStrings[6];

                

                if (string.IsNullOrEmpty(CompanyId) || CompanyId == "undefined")
                    CompanyId = null;

                if (string.IsNullOrEmpty(AgentID) || AgentID == "undefined")
                    AgentID = null;

                if (string.IsNullOrEmpty(ClientID) || ClientID == "undefined")
                    ClientID = null;

                if (string.IsNullOrEmpty(FromDate) || FromDate == "undefined")
                    FromDate = null;

                if (string.IsNullOrEmpty(to_date) || to_date == "undefined")
                    to_date = null;

                if (string.IsNullOrEmpty(sel6) || sel6 == "undefined")
                    sel6 = null;

                //if (string.IsNullOrEmpty(sel7) || sel7 == "undefined")
                //    sel7 = null;

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();

                p1.Name = "UserName";
                p1.Values.Add(UserName);

                p2.Name = "FromDate";
                p2.Values.Add(FromDate == null ? "" : FromDate);

                p3.Name = "ToDate";
                p3.Values.Add(to_date == null ? "" : to_date);

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();
                if (reportType == null || reportType == "Undefined")
                {
                    System.Windows.Forms.MessageBox.Show("Report type is either null or undefined.", "Message");
                }
                if (reportType == "ledger_report_Details")
                { 
                  LocalReport.ReportPath = Server.MapPath("~/Areas/EkhonTv/Reports/MediaLedger/MediaLedgerReportDetails.rdlc");
                }
                if (reportType == "ledger_report_Summary")
                {
                  LocalReport.ReportPath = Server.MapPath("~/Areas/EkhonTv/Reports/MediaLedger/MediaLedgerReportSummary.rdlc");
                }
                OracleCommand objCmd = new OracleCommand();

                objCmd.CommandText = "Rpt_tv_client_ledger.getdata";

                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = CompanyId;
                objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = AgentID; 
                objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = ClientID;

                objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = FromDate ;
                objCmd.Parameters.Add("sel5", OracleDbType.Varchar2).Value = to_date ;

                //objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = FromDate == null ? null : Convert.ToDateTime(FromDate).ToString("dd-MM-yyyyy");
                //objCmd.Parameters.Add("sel5", OracleDbType.Varchar2).Value = to_date == null ? null : Convert.ToDateTime(to_date).ToString("dd-MM-yyyyy");
                objCmd.Parameters.Add("sel6", OracleDbType.Varchar2).Value = null;
                //objCmd.Parameters.Add("sel7", OracleDbType.Varchar2).Value = null;


                DataTable dt = classDt.GetDataEkhon(objCmd);

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();
                ReportDataSource datasource = new ReportDataSource("EkhonTVMediaLedgerDS", dt);

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
            }
        }
    }
}