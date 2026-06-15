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

namespace ABS.Web.Areas.ACCOUNTS.Reports.TrialBalance
{
    public partial class TrialBalance : System.Web.UI.Page
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
                string cCompany = queryStrings[2];
                string subHead = queryStrings[3];//locProject
                string accGrpHead = queryStrings[4];
                string accHead = queryStrings[5];
                string fromDate = queryStrings[6];
                string toDate = queryStrings[7];
                string rptType = queryStrings[8];

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter(); 

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);
                p2.Name = "ToDate";
                p2.Values.Add(toDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);

                if(location == "undefined" || location == "")
                {
                    location = null;
                }
                if (mCompany == "undefined" || mCompany == "")
                {
                    mCompany = null;
                }
                if (cCompany == "undefined" || cCompany == "")
                {
                    cCompany = null;
                }
                if (subHead == "undefined" || subHead == "")
                {
                    subHead = null;
                }
                if (accGrpHead == "undefined" || accGrpHead == "")
                {
                    accGrpHead = null;
                }

                if (accHead == "undefined" || accHead == "")
                {
                    accHead = null;
                }

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();
                string proc = "";
                OracleCommand objCmd = new OracleCommand();

                if (rptType == "inComTranverification")
                {
                    objCmd.CommandText = "Repot_trail_balance.getdata";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/TrialBalance/inComTranverification.rdlc");
                }
                if (rptType == "TrialBlnce")
                {
                    objCmd.CommandText = "Repot_trail_balance.getdata";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/ACCOUNTS/Reports/TrialBalance/TrialBalance.rdlc");
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

                // objCmd.CommandText = "Repot_trail_balance." + proc;
                

                objCmd.CommandType = CommandType.StoredProcedure;
    
                objCmd.Parameters.Add("t_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("ccom_mcom", OracleDbType.Varchar2).Value = mCompany;
                objCmd.Parameters.Add("tsrm_ccom", OracleDbType.Varchar2).Value = cCompany;
                objCmd.Parameters.Add("accd_cshd", OracleDbType.Varchar2).Value = subHead;
                objCmd.Parameters.Add("ahdgp_acgp", OracleDbType.Varchar2).Value = accGrpHead;
                objCmd.Parameters.Add("tsrd_accd", OracleDbType.Varchar2).Value = accHead;
                objCmd.Parameters.Add("tsrm_date", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("sel7", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("sel8", OracleDbType.Varchar2).Value = null;
                //objCmd.Parameters.Add("p_userId", OracleDbType.Varchar2).Value = UserID;

                DataTable dt = classDt.GetData(objCmd);

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);

                ReportDataSource datasource = new ReportDataSource("DtTrialBalance", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3});
            }
        }
    }
}