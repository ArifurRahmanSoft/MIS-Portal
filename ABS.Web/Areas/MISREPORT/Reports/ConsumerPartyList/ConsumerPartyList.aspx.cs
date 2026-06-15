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

namespace ABS.Web.Areas.MISREPORT.Reports.ConsumerPartyList
{
    public partial class ConsumerPartyList : System.Web.UI.Page
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
                string[] queryStrings = Request["productType"].ToString().Split(',');
                //string productType = queryStrings[0];
                //string[] queryStrings = Request["queryString"].ToString().Split(',');
             //   string productType = Request["productType"];
                string productType = queryStrings[0];
                string reportType = queryStrings[1];

                // activity monitoring
                string reportFilteringData = productType;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Consumer Party List", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());
                
                ReportParameter p1 = new ReportParameter();
                //ReportParameter p2 = new ReportParameter();
                
                p1.Name = "UserName";
                p1.Values.Add(UserName);
                //  p2.Name = "ReportFilter";
                //   p2.Values.Add(reportFilter);



                if (productType == "")
                    productType = null;



                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                if(reportType == "2")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ConsumerPartyList/ReportConsumerPartyList.rdlc");
                    objCmd.CommandText = "REPORTS.Consumer_Party_List";
                }
                else
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ConsumerPartyList/ReportConsumerDoLocatioinPartyList.rdlc");
                    objCmd.CommandText = "REPORTS.DO_Location_Base_Party_List";
                }



                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("productType", OracleDbType.Varchar2).Value = productType;

                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSSConsumerPartyList", dt);
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