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

namespace ABS.Web.Areas.MISREPORT.Reports.DailyCollection
{
    public partial class DailyCollection : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
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
                string[] queryStrings = Request["queryString"].ToString().Split(',');

                string company = queryStrings[0];
                string customer = queryStrings[1];
                string location = queryStrings[2];
                string tranmode = queryStrings[3];
                string postmode = queryStrings[4];
                string sstype = queryStrings[5];
                string fromDate = queryStrings[6];
                string toDate = queryStrings[7];

                // activity monitoring
                string reportFilteringData = company + "-" + customer + "-" + location + "-" + tranmode + "-" + postmode + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Daily Collection", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                if (Session["UserID"].ToString() == "08414")
                    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);
                p2.Name = "ToDate";
                p2.Values.Add(toDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);


                if (company == "" || company == "undefined")
                    company = null;

                if (customer == "" || customer == "undefined")
                    customer = null;

                if (location == "" || location == "undefined")
                    location = null;

                if (tranmode == "" || tranmode == "undefined")
                    tranmode = null;

                if (postmode == "" || postmode == "undefined")
                    postmode = null;


                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();


                LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DailyCollection/DailyCollectionReport.rdlc");
                objCmd.CommandText = "pack_MR.get_daily_mr";


                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                objCmd.Parameters.Add("p_company", OracleDbType.Varchar2).Value = company;
                objCmd.Parameters.Add("p_customer", OracleDbType.Varchar2).Value = customer;
                objCmd.Parameters.Add("p_location", OracleDbType.Varchar2).Value = location;
                objCmd.Parameters.Add("p_tranmode", OracleDbType.Varchar2).Value = tranmode;
                objCmd.Parameters.Add("p_postmode", OracleDbType.Varchar2).Value = postmode;
                objCmd.Parameters.Add("p_sstype", OracleDbType.Varchar2).Value = sstype;

                objCmd.Parameters.Add("p_fromDate", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("p_toDate", OracleDbType.Varchar2).Value = toDate;

                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSDailyCollection", dt);
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}