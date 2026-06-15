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

namespace ABS.Web.Areas.MISREPORT.Reports.ListOfActiveSKU
{
    public partial class ListOfActiveSKU : System.Web.UI.Page
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
                                       //Response.Redirect("http://localhost:15812/Account/Login");
                    Response.Redirect("~/Account/Login");
                }

                UserName = Session["UserFullName"].ToString();
                //string[] queryStrings = Request["queryString"].ToString().Split(',');
                //string productType = queryStrings[0];
                //string[] queryStrings = Request["queryString"].ToString().Split(',');
                string productType = Request["productType"];


                // activity monitoring
                string reportFilteringData = productType;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("List Of Active SKU", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());
                

                ReportParameter p1 = new ReportParameter();
                //ReportParameter p2 = new ReportParameter();


                p1.Name = "UserName";
                p1.Values.Add(UserName);
                //  p2.Name = "ReportFilter";
                //   p2.Values.Add(reportFilter);



                if (productType == "" || productType == "undefined")
                    productType = null;



                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();


                LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ListOfActiveSKU/ReportListOfActiveSKU.rdlc");
                objCmd.CommandText = "REPORTS.List_Of_Active_SKU";


                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("productType", OracleDbType.Varchar2).Value = productType;

                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSSListOfActiveSKU", dt);
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1 });


                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}