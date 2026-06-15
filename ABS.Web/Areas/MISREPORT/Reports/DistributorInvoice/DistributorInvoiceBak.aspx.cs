using ABS.Web.Utility;
using CTGroup.Utility.Common;
using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABS.Web.Areas.MISREPORT.Reports.DistributorInvoice
{
    public partial class DistributorInvoiceBak : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
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
                String UserId = Session["UserID"].ToString();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                string requisitionID = Request.QueryString["requisitionID"];

                // activity monitoring
                string reportFilteringData = requisitionID;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Distributor Invoice", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                //START GET LOGGED USER SHORT DESIGNATION
                string loggedDesig = string.Empty;
                DataTable UserDesig = new DataTable();
                OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
                con.Open();
                string query = "select  SUBSTR(SP.SPRSN_SDESG,19,2) DESIGNATION from CITYN.T_SPRSN SP WHERE  " +
                               "SPRSN_TEXT= '" + Session["UserID"].ToString() + "' and sprsn_actv = 'Y'";
                OracleCommand cmd = new OracleCommand(query, con);
                using (OracleDataAdapter a = new OracleDataAdapter(cmd))
                {
                    a.Fill(UserDesig);
                }
                con.Close();

                if (UserDesig.Rows.Count > 0)
                {
                    loggedDesig = UserDesig.Rows[0]["DESIGNATION"].ToString();
                }
                //END GET LOGGED USER SHORT DESIGNATION

                LocalReport LocalReport = new LocalReport();

                LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DistributorInvoice/ReportDistributorInvoice.rdlc");

                //if (UserId == "01646" || UserId == "01395" )
                //{
                //}
                //else
                //{
                //    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DistributorInvoice/ReportDistributorInvoiceWithoutValue.rdlc");
                //}

                OracleCommand objCmd = new OracleCommand();
                objCmd.CommandText = "REPORTS.Distributor_Invoice";
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("desig", OracleDbType.Varchar2).Value = loggedDesig;
                objCmd.Parameters.Add("requisitionID", OracleDbType.Varchar2).Value = requisitionID;
                //objCmd.Parameters.Add("p_date1", OracleDbType.Varchar2).Value = fromDate;
                //objCmd.Parameters.Add("p_date2", OracleDbType.Varchar2).Value = toDate;

                DataTable dt = classDt.GetData(objCmd);

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                ReportDataSource datasource = new ReportDataSource("DDSDistributorInvoice", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                //refresh
                ReportViewer1.LocalReport.Refresh();
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}