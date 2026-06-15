using ABS.Web.Utility;
using CTGroup.Utility.Common;
using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABS.Web.Areas.MISREPORT.Reports.SaleTopSheet
{
    public partial class SaleTopSheetExcel : System.Web.UI.Page
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
                string location = queryStrings[1];
                string fromDate = queryStrings[2];
                string toDate = queryStrings[3];

                // activity monitoring
                string reportFilteringData = company + "-" + location + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Sales Top Sheet", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

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

                if (location == "" || location == "undefined")
                    location = null;

                /// collection section
                OracleCommand objCmdCol = new OracleCommand();
                objCmdCol.CommandText = "Rpt_sales_deposit.daily_collection";
                objCmdCol.CommandType = CommandType.StoredProcedure;
                objCmdCol.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmdCol.Parameters.Add("sel1", OracleDbType.Varchar2).Value = location; // location
                objCmdCol.Parameters.Add("sel2", OracleDbType.Varchar2).Value = company; // company
                objCmdCol.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
                objCmdCol.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
                objCmdCol.Parameters.Add("sel5", OracleDbType.Varchar2).Value = fromDate; // from date
                objCmdCol.Parameters.Add("sel6", OracleDbType.Varchar2).Value = toDate; // to date
                objCmdCol.Parameters.Add("sel7", OracleDbType.Varchar2).Value = null;
                DataTable dtCol = classDt.GetData(objCmdCol);
                ReportDataSource collection = new ReportDataSource("Collection", dtCol);


                /// do sale and other sale section
                OracleCommand objCmdDOSale = new OracleCommand();
                objCmdDOSale.CommandText = "Rpt_sales_deposit.sales_summ_noadv"; // ";
                objCmdDOSale.CommandType = CommandType.StoredProcedure;
                objCmdDOSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmdDOSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = location; // location
                objCmdDOSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = company; // company
                objCmdDOSale.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
                objCmdDOSale.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
                objCmdDOSale.Parameters.Add("sel5", OracleDbType.Varchar2).Value = fromDate; // from date
                objCmdDOSale.Parameters.Add("sel6", OracleDbType.Varchar2).Value = toDate; // to date
                objCmdDOSale.Parameters.Add("sel7", OracleDbType.Varchar2).Value = null;
                DataTable dtDOSale = classDt.GetData(objCmdDOSale);



                /// advanced sale
                OracleCommand objCmdAdvanceSale = new OracleCommand();
                objCmdAdvanceSale.CommandText = "Rpt_sales_deposit.sales_advsalse";
                objCmdAdvanceSale.CommandType = CommandType.StoredProcedure;
                objCmdAdvanceSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmdAdvanceSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = location; // location
                objCmdAdvanceSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = company; // company
                objCmdAdvanceSale.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
                objCmdAdvanceSale.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
                objCmdAdvanceSale.Parameters.Add("sel5", OracleDbType.Varchar2).Value = fromDate; // from date
                objCmdAdvanceSale.Parameters.Add("sel6", OracleDbType.Varchar2).Value = toDate; // to date
                objCmdAdvanceSale.Parameters.Add("sel7", OracleDbType.Varchar2).Value = null;
                DataTable dtAdvanceSale = classDt.GetData(objCmdAdvanceSale);
                ReportDataSource advancesale = new ReportDataSource("AdvanceSale", dtAdvanceSale);


                /// cheque in hand
                OracleCommand objCmdChequeInHand = new OracleCommand();
                objCmdChequeInHand.CommandText = "Rpt_sales_deposit.cheque_inhand";
                objCmdChequeInHand.CommandType = CommandType.StoredProcedure;
                objCmdChequeInHand.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmdChequeInHand.Parameters.Add("t_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmdChequeInHand.Parameters.Add("call_name", OracleDbType.Varchar2).Value = null;
                objCmdChequeInHand.Parameters.Add("sel1", OracleDbType.Varchar2).Value = null;
                objCmdChequeInHand.Parameters.Add("sel2", OracleDbType.Varchar2).Value = null; // 
                objCmdChequeInHand.Parameters.Add("sel3", OracleDbType.Varchar2).Value = fromDate; // from date;
                objCmdChequeInHand.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
                objCmdChequeInHand.Parameters.Add("sel5", OracleDbType.Varchar2).Value = null;
                DataTable dtChequeInHand = classDt.GetData(objCmdChequeInHand);
                ReportDataSource chequeInHand = new ReportDataSource("ChequeInHand", dtChequeInHand);


                /// market rate
                OracleCommand objCmdMarketRate = new OracleCommand();
                objCmdMarketRate.CommandText = "Rpt_sales_deposit.prod_market_rate";
                objCmdMarketRate.CommandType = CommandType.StoredProcedure;
                objCmdMarketRate.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmdMarketRate.Parameters.Add("sel1", OracleDbType.Varchar2).Value = null; // location
                objCmdMarketRate.Parameters.Add("sel2", OracleDbType.Varchar2).Value = null; // company
                objCmdMarketRate.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
                objCmdMarketRate.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
                objCmdMarketRate.Parameters.Add("sel5", OracleDbType.Varchar2).Value = fromDate; // from date
                objCmdMarketRate.Parameters.Add("sel6", OracleDbType.Varchar2).Value = toDate; // to date
                objCmdMarketRate.Parameters.Add("sel7", OracleDbType.Varchar2).Value = null;
                DataTable dtMarketRate = classDt.GetData(objCmdMarketRate);
                ReportDataSource marketRate = new ReportDataSource("MarketRate", dtMarketRate);


                /// auto rice sale
                OracleCommand objAutRice = new OracleCommand();
                objAutRice.CommandText = "Rpt_sales_deposit.auto_rice_sale";
                objAutRice.CommandType = CommandType.StoredProcedure;
                objAutRice.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objAutRice.Parameters.Add("sel1", OracleDbType.Varchar2).Value = fromDate; // from date
                objAutRice.Parameters.Add("sel2", OracleDbType.Varchar2).Value = toDate; // to date
                DataTable dtAR = classDt.GetData(objAutRice);


                /// till date do sale 
                OracleCommand objTillDateDOSale = new OracleCommand();
                objTillDateDOSale.CommandText = "Rpt_sales_deposit.TillDateTotalSale";
                objTillDateDOSale.CommandType = CommandType.StoredProcedure;
                objTillDateDOSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objTillDateDOSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = fromDate; // from date
                objTillDateDOSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = toDate; // to date
                DataTable dtTillDateDOSale = classDt.GetData(objTillDateDOSale);

                DataTable tilldateTotSale = new DataTable();
                tilldateTotSale = dtTillDateDOSale.Copy();
                tilldateTotSale.Merge(dtAdvanceSale);

                ReportDataSource TilldateTotSale = new ReportDataSource("TillDateTotalSale", tilldateTotSale);

                //******************************************************************
                //******************************************************************
                //************///////calculating data for total sale of last month//////**********
                /// last month calculation 
                ///                 
                DateTime newFromDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddMonths(-1);
                var firstDayOfLastMonth = new DateTime(newFromDate.Year, newFromDate.Month, 1);
                var lastDayOfLastMonth = firstDayOfLastMonth.AddMonths(1).AddDays(-1);
                DateTime todayOfLastMonth = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddMonths(-1);
                int lastMonthFirstToTodayDuration = (todayOfLastMonth - firstDayOfLastMonth).Days + 1;

                ReportParameter lastMonthFirstToTodayDurationNumber = new ReportParameter();
                lastMonthFirstToTodayDurationNumber.Name = "LastMonthFirstToTodayDurationNumber";
                lastMonthFirstToTodayDurationNumber.Values.Add(lastMonthFirstToTodayDuration.ToString());

                ReportParameter fDLM = new ReportParameter();
                fDLM.Name = "FirstDayOfLastMonth";
                fDLM.Values.Add(firstDayOfLastMonth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));

                ReportParameter tDLM = new ReportParameter();
                tDLM.Name = "TodayOfLastMonth";
                tDLM.Values.Add(todayOfLastMonth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));

                DateTime selectedDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime todayOfThisMonth = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var firstDateOfThisMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                int thisMonthFirstToTodayDuration = (todayOfThisMonth - firstDateOfThisMonth).Days + 1;

                ReportParameter fDTT = new ReportParameter();
                fDTT.Name = "FirstDateOfThisMonth";
                fDTT.Values.Add(firstDateOfThisMonth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));

                ReportParameter thisMonthFirstToTodayDurationNumber = new ReportParameter();
                thisMonthFirstToTodayDurationNumber.Name = "ThisMonthFirstToTodayDurationNumber";
                thisMonthFirstToTodayDurationNumber.Values.Add(thisMonthFirstToTodayDuration.ToString());


                //////////////////////
                ///


                OracleCommand objLastMonthDOSale = new OracleCommand();
                objLastMonthDOSale.CommandText = "RPT_SALES_DEPOSIT_PRE_SALE.PreviousDateTotalSale";
                objLastMonthDOSale.CommandType = CommandType.StoredProcedure;
                objLastMonthDOSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objLastMonthDOSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = firstDayOfLastMonth.Date.ToString("dd/MM/yyyy"); // from date
                objLastMonthDOSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = lastDayOfLastMonth.Date.ToString("dd/MM/yyyy"); // to date
                DataTable dtLastMonthDOSale = classDt.GetData(objLastMonthDOSale);


                OracleCommand objCmdLastMonthAdvanceSale = new OracleCommand();
                objCmdLastMonthAdvanceSale.CommandText = "Rpt_sales_deposit.sales_advsalse";
                objCmdLastMonthAdvanceSale.CommandType = CommandType.StoredProcedure;
                objCmdLastMonthAdvanceSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmdLastMonthAdvanceSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = location; // location
                objCmdLastMonthAdvanceSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = company; // company
                objCmdLastMonthAdvanceSale.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
                objCmdLastMonthAdvanceSale.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
                objCmdLastMonthAdvanceSale.Parameters.Add("sel5", OracleDbType.Varchar2).Value = firstDayOfLastMonth.Date.ToString("dd/MM/yyyy"); // from date
                objCmdLastMonthAdvanceSale.Parameters.Add("sel6", OracleDbType.Varchar2).Value = lastDayOfLastMonth.Date.ToString("dd/MM/yyyy"); // to date
                objCmdLastMonthAdvanceSale.Parameters.Add("sel7", OracleDbType.Varchar2).Value = null;
                DataTable dtLastMonthAdvanceSale = classDt.GetData(objCmdLastMonthAdvanceSale);

                DataTable lastMonthTotalSale = new DataTable();
                lastMonthTotalSale = dtLastMonthDOSale.Copy();
                lastMonthTotalSale.Merge(dtLastMonthAdvanceSale);

                ReportDataSource LastMonthTotalSale = new ReportDataSource("LastMonthTotalSale", lastMonthTotalSale);

                //*************////// end last month data calculation/////**********
                //******************************************************************
                //******************************************************************



                //******************************************************************
                //******************************************************************
                /// last month till today calculation 
                ///              

                OracleCommand objLastMonthTTDOSale = new OracleCommand();
                objLastMonthTTDOSale.CommandText = "RPT_SALES_DEPOSIT_PRE_SALE.PreviousDateTotalSale";
                objLastMonthTTDOSale.CommandType = CommandType.StoredProcedure;
                objLastMonthTTDOSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objLastMonthTTDOSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = firstDayOfLastMonth.Date.ToString("dd/MM/yyyy"); // from date
                objLastMonthTTDOSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = todayOfLastMonth.Date.ToString("dd/MM/yyyy"); // to date
                DataTable dtLastMonthTTDOSale = classDt.GetData(objLastMonthTTDOSale);


                OracleCommand objCmdLastMonthTillTodayAdvanceSale = new OracleCommand();
                objCmdLastMonthTillTodayAdvanceSale.CommandText = "Rpt_sales_deposit.sales_advsalse";
                objCmdLastMonthTillTodayAdvanceSale.CommandType = CommandType.StoredProcedure;
                objCmdLastMonthTillTodayAdvanceSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmdLastMonthTillTodayAdvanceSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = location; // location
                objCmdLastMonthTillTodayAdvanceSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = company; // company
                objCmdLastMonthTillTodayAdvanceSale.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
                objCmdLastMonthTillTodayAdvanceSale.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
                objCmdLastMonthTillTodayAdvanceSale.Parameters.Add("sel5", OracleDbType.Varchar2).Value = firstDayOfLastMonth.Date.ToString("dd/MM/yyyy"); // from date
                objCmdLastMonthTillTodayAdvanceSale.Parameters.Add("sel6", OracleDbType.Varchar2).Value = todayOfLastMonth.Date.ToString("dd/MM/yyyy"); // to date
                objCmdLastMonthTillTodayAdvanceSale.Parameters.Add("sel7", OracleDbType.Varchar2).Value = null;
                DataTable dtLastMonthTTAdvanceSale = classDt.GetData(objCmdLastMonthTillTodayAdvanceSale);

                DataTable lastMonthTTTotalSale = new DataTable();
                lastMonthTTTotalSale = dtLastMonthTTDOSale.Copy();
                lastMonthTTTotalSale.Merge(dtLastMonthTTAdvanceSale);

                ReportDataSource LastMonthTTTotalSale = new ReportDataSource("LastMonthTTTotalSale", lastMonthTTTotalSale);

                //*************////// end last month till today data calculation/////**********
                //******************************************************************
                //******************************************************************

                /// ton VS vehicle
                OracleCommand objTonVSVehicle = new OracleCommand();
                objTonVSVehicle.CommandText = "Rpt_sales_deposit.TonVSVehicle";
                objTonVSVehicle.CommandType = CommandType.StoredProcedure;
                objTonVSVehicle.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objTonVSVehicle.Parameters.Add("sel1", OracleDbType.Varchar2).Value = fromDate; // from date
                objTonVSVehicle.Parameters.Add("sel2", OracleDbType.Varchar2).Value = toDate; // to date
                DataTable dtTV = classDt.GetData(objTonVSVehicle);
                ReportDataSource TonVehicle = new ReportDataSource("TonVSVehicle", dtTV);


                DataTable dtAll = new DataTable();
                dtAll = dtDOSale.Copy();
                dtAll.Merge(dtAdvanceSale);
                dtAll.Merge(dtAR);
                ReportDataSource DOPlusAdvance = new ReportDataSource("DOPlusAdvance", dtAll);


                /// do sale with auto rice merge
                DataTable DOWithAR = new DataTable();
                DOWithAR = dtDOSale.Copy();
                DOWithAR.Merge(dtAR);
                ReportDataSource dosale = new ReportDataSource("DOSale", DOWithAR);


                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                LocalReport LocalReport = new LocalReport();
                LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SaleTopSheet/SaleDepositTS.rdlc");


                ReportViewer1.LocalReport.DataSources.Add(collection);
                ReportViewer1.LocalReport.DataSources.Add(dosale);
                ReportViewer1.LocalReport.DataSources.Add(advancesale);
                ReportViewer1.LocalReport.DataSources.Add(chequeInHand);
                ReportViewer1.LocalReport.DataSources.Add(DOPlusAdvance);
                ReportViewer1.LocalReport.DataSources.Add(marketRate);
                ReportViewer1.LocalReport.DataSources.Add(TonVehicle);
                ReportViewer1.LocalReport.DataSources.Add(TilldateTotSale);
                ReportViewer1.LocalReport.DataSources.Add(LastMonthTotalSale);
                ReportViewer1.LocalReport.DataSources.Add(LastMonthTTTotalSale);



                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, fDTT, fDLM, tDLM,
                    lastMonthFirstToTodayDurationNumber, thisMonthFirstToTodayDurationNumber });

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);

                // writing db for report process end time
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}