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

namespace ABS.Web.Areas.MISREPORT.Reports.ShowroomLedger
{
    public partial class ShowroomLedger : System.Web.UI.Page
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
                string showroomId = queryStrings[2];
                string productId = queryStrings[3];
               
                string reportType = queryStrings[4];
               

          
              
                string reportFilteringData =  reportType + "-" + productId + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Undelivered Statment", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                if (Session["UserID"].ToString() == "08414")
                    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();
                //ReportParameter p4 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);
                p2.Name = "ToDate";
                p2.Values.Add(toDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);
            
               




                if (productId == "" || productId == "undefined")
                    productId = null;

                if (showroomId == "" || showroomId == "undefined")
                    showroomId = null;

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                if (reportType == "comTranDetails")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ShowroomLedger/ReportCompanyWiseTranDetails.rdlc");
                    objCmd.CommandText = "Rpt_sroom_sale_value_sum_amnt.get_comp_sales";
                }

                if (reportType == "comTranSummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ShowroomLedger/ReportCompanyWiseTranSummary.rdlc");
                    objCmd.CommandText = "Rpt_sroom_sale_value_sum_amnt.get_comp_sales";
                }

                if (reportType == "showrmLedgerDetails")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ShowroomLedger/ReportShowroomLedgerDetails.rdlc");
                    objCmd.CommandText = "cityn.Rpt_sroom_sale_value_sum_amnt.getdata_ledger_new";
                   
                }

                if (reportType == "showrmLedgerSummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ShowroomLedger/ReportShowroomLedgerSummary.rdlc");
                    objCmd.CommandText = "cityn.Rpt_sroom_sale_value_sum_amnt.srom_ledger_summ_new";
                }


                if (reportType == "dailyStatementWithAmntDetails")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ShowroomLedger/ReportShowroomDailyStockDetailsWithAmmount.rdlc");
                    objCmd.CommandText = "cityn.rpt_sroom_stock_summary.getdata_new";
                }


                if (reportType == "dailyStatementWithAmntSummary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ShowroomLedger/ReportShowroomDailyStockDetailsWithAmmount.rdlc");
                    objCmd.CommandText = "cityn.rpt_sroom_stock_summary.getdata_new";
                }


                if (reportType == "dailyStatemntWithSalesVlueDtls")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ShowroomLedger/ReportDailyStatementWithSalesValue.rdlc");
                    objCmd.CommandText = "cityn.RPT_SROOM_SALE_VALUE_SUM_AMNT.getdata_new";
                }

                if (reportType == "dailyStatemntWithSalesVlueSummry")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ShowroomLedger/ReportDailyStatementWithSalesValue.rdlc");
                    objCmd.CommandText = "cityn.RPT_SROOM_SALE_VALUE_SUM_AMNT.getdata_new";
                }




                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                objCmd.Parameters.Add("location", OracleDbType.Varchar2).Value = showroomId;
                objCmd.Parameters.Add("company", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("product", OracleDbType.Varchar2).Value = productId;
                objCmd.Parameters.Add("fdate", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("tdate", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("sel6", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("sel7", OracleDbType.Varchar2).Value = null;


                DataTable dt = classDt.GetDataCityn(objCmd);


                //
                ReportDataSource datasource = new ReportDataSource("DTShowroomLedger", dt);//  DSShowroomLedger
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
                //ReportViewer1.LocalReport.SetParameters(new ReportParameter[] {  });



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