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

namespace ABS.Web.Areas.MISREPORT.Reports.DeliveryStatementConsumerBulk
{
    public partial class DeliveryStatementConsumerBulk : System.Web.UI.Page
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
                string UserId = Session["UserID"].ToString();

                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string reportType = queryStrings[2];

                Utils utility = new Utils();
                //if (utility.IsNotValidActivity(UserId))
                //{
                //    Session.Abandon(); // Does nothing
                //    Session.Clear();   // Removes the data contained in the session
                //    Response.Redirect("~/Account/Login");
                //}

                // activity monitoring
                string reportFilteringData = fromDate + "-" + toDate + "-" + reportType;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();

                if (Session["UserID"].ToString() == "08414")
                    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

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
                p4.Values.Add("All");

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                if (reportType == "1")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementConsumerBulk/ReportDeliveryStatementConBulkMTon_SKU.rdlc");
                }
                else
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementConsumerBulk/ReportDeliveryStatementConBulkMTonSummary.rdlc");
                }
                objCmd.CommandText = "pack_challanDetail_new.Get_ConsumerBulkDeliveryMTon";

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                //New Param
                objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("isBrndGroup", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("brndGroupId", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = UserId;
                //New Param

                string autoId = reportActivityMonitoring.RequestStart("Delivery Statement Consumer Bulk", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());
                DataTable dt = classDt.GetData(objCmd);
                reportActivityMonitoring.RequestEnd(autoId);

                ReportDataSource datasource = new ReportDataSource("dsDeleveryStatementConBulk", dt);
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                LocalReport.SetParameters(p4);
            }
        }
    }
}