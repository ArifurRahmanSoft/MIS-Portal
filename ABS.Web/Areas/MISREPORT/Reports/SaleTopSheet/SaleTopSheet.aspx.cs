using ABS.Web.Utility;
using CTGroup.Utility;
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
    public partial class SaleTopSheet : System.Web.UI.Page
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
                string reportType = queryStrings[4];

                DateTime fDate = Conversion.StringToFormattedDate(fromDate);

                string FstDateMonth = Conversion.FstDateOfMonth(fDate);
                string LstDateMonth = Conversion.LstDateOfMonth(fDate);

                string FstDateOfLastMonth = Conversion.FirstOfMonth(fDate, -1, 0);
                string LstDateOfLastMonth = Conversion.LastOfMonth(fDate, 0, -1);

                string curToDate = DateTime.Now.ToString("dd/MM/yyyy");

                if (reportType == "TopSheet")
                {
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


                    /// Rupshi Foods Sales Daily
                    /// 

                    OracleCommand objRupSaleDaily = new OracleCommand();
                    objRupSaleDaily.CommandText = "Rpt_sales_deposit.RupshiSales";
                    objRupSaleDaily.CommandType = CommandType.StoredProcedure;
                    objRupSaleDaily.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objRupSaleDaily.Parameters.Add("sel1", OracleDbType.Varchar2).Value = fromDate; // from date
                    objRupSaleDaily.Parameters.Add("sel2", OracleDbType.Varchar2).Value = toDate; // to date
                    objRupSaleDaily.Parameters.Add("rtype", OracleDbType.Varchar2).Value = "daily"; // sales type
                    DataTable dtRupSale = classDt.GetData(objRupSaleDaily);
                    //ReportDataSource RupSaleDaily = new ReportDataSource("RupshiSale", dtRupSaleDaily);

                    /// Rupshi Foods Sales Monthly
                    OracleCommand objRupSaleMonthly = new OracleCommand();
                    objRupSaleMonthly.CommandText = "Rpt_sales_deposit.RupshiSales";
                    objRupSaleMonthly.CommandType = CommandType.StoredProcedure;
                    objRupSaleMonthly.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objRupSaleMonthly.Parameters.Add("sel1", OracleDbType.Varchar2).Value = FstDateMonth; // from date
                    objRupSaleMonthly.Parameters.Add("sel2", OracleDbType.Varchar2).Value = LstDateMonth; // to date
                    objRupSaleMonthly.Parameters.Add("rtype", OracleDbType.Varchar2).Value = "monthly"; // sales type
                    DataTable dtRupSaleMonthly = classDt.GetData(objRupSaleMonthly);
                    dtRupSale.Merge(dtRupSaleMonthly);
                    //ReportDataSource RupSaleMonthly = new ReportDataSource("RupshiSaleMonthly", dtRupSaleMonthly);

                    /// Rupshi Foods Sales Monthly
                    OracleCommand objRupSaleLastMonth = new OracleCommand();
                    objRupSaleLastMonth.CommandText = "Rpt_sales_deposit.RupshiSales";
                    objRupSaleLastMonth.CommandType = CommandType.StoredProcedure;
                    objRupSaleLastMonth.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objRupSaleLastMonth.Parameters.Add("sel1", OracleDbType.Varchar2).Value = FstDateOfLastMonth; // from date
                    objRupSaleLastMonth.Parameters.Add("sel2", OracleDbType.Varchar2).Value = LstDateOfLastMonth; // to date
                    objRupSaleLastMonth.Parameters.Add("rtype", OracleDbType.Varchar2).Value = "lastMonth"; // sales type
                    DataTable dtRupSaleLastMonth = classDt.GetData(objRupSaleLastMonth);
                    dtRupSale.Merge(dtRupSaleLastMonth);
                    ReportDataSource RupSale = new ReportDataSource("RupshiSale", dtRupSale);

                    ///// Rupshi Foods Sales Till Date
                    //OracleCommand objRupSaleTillDate = new OracleCommand();
                    //objRupSaleTillDate.CommandText = "Rpt_sales_deposit.RupshiSales";
                    //objRupSaleTillDate.CommandType = CommandType.StoredProcedure;
                    //objRupSaleTillDate.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    //objRupSaleTillDate.Parameters.Add("sel1", OracleDbType.Varchar2).Value = ""; // from date
                    //objRupSaleTillDate.Parameters.Add("sel2", OracleDbType.Varchar2).Value = LstDateMonth; // to date
                    //objRupSaleTillDate.Parameters.Add("rtype", OracleDbType.Varchar2).Value = "tillday"; // sales type
                    //DataTable dtRupSaleTillDate = classDt.GetData(objRupSaleTillDate);
                    //dtRupSale.Merge(dtRupSaleTillDate);
                    ////ReportDataSource RupSaleTillDate = new ReportDataSource("RupshiSaleTillDate", dtRupSaleTillDate);
                    //ReportDataSource RupSale = new ReportDataSource("RupshiSale", dtRupSale);

                    ///// Rupshi Foods Sales Daily
                    //OracleCommand objRupSale = new OracleCommand();
                    //objRupSale.CommandText = "Rpt_sales_deposit.RupshiSales";
                    //objRupSale.CommandType = CommandType.StoredProcedure;
                    //objRupSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    //objRupSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = fromDate; // from date
                    //objRupSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = toDate; // to date
                    //DataTable dtRupSale = classDt.GetData(objRupSale);
                    //ReportDataSource RupSale = new ReportDataSource("RupshiSale", dtRupSale);

                    ///// Rupshi Foods Sales Monthly
                    //OracleCommand objRupSaleMonthly = new OracleCommand();
                    //objRupSaleMonthly.CommandText = "Rpt_sales_deposit.RupshiSales";
                    //objRupSaleMonthly.CommandType = CommandType.StoredProcedure;
                    //objRupSaleMonthly.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    //objRupSaleMonthly.Parameters.Add("sel1", OracleDbType.Varchar2).Value = FstDateMonth; // from date
                    //objRupSaleMonthly.Parameters.Add("sel2", OracleDbType.Varchar2).Value = LstDateMonth; // to date
                    //DataTable dtRupSaleMonthly = classDt.GetData(objRupSaleMonthly);
                    //ReportDataSource RupSaleMonthly = new ReportDataSource("RupshiSaleMonthly", dtRupSaleMonthly);

                    ///// Rupshi Foods Sales Till Date
                    //OracleCommand objRupSaleTillDate = new OracleCommand();
                    //objRupSaleTillDate.CommandText = "Rpt_sales_deposit.RupshiSales";
                    //objRupSaleTillDate.CommandType = CommandType.StoredProcedure;
                    //objRupSaleTillDate.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    //objRupSaleTillDate.Parameters.Add("sel1", OracleDbType.Varchar2).Value = ""; // from date
                    //objRupSaleTillDate.Parameters.Add("sel2", OracleDbType.Varchar2).Value = LstDateMonth; // to date
                    //DataTable dtRupSaleTillDate = classDt.GetData(objRupSaleTillDate);
                    //ReportDataSource RupSaleTillDate = new ReportDataSource("RupshiSaleTillDate", dtRupSaleTillDate);


                    ///// Rupshi Foods Collection`
                    //OracleCommand objRupColl = new OracleCommand();
                    //objRupColl.CommandText = "Rpt_sales_deposit.RupshiCollection";
                    //objRupColl.CommandType = CommandType.StoredProcedure;
                    //objRupColl.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    //objRupColl.Parameters.Add("sel1", OracleDbType.Varchar2).Value = fromDate; // from date
                    //objRupColl.Parameters.Add("sel2", OracleDbType.Varchar2).Value = toDate; // to date
                    //DataTable dtRupColl = classDt.GetData(objRupColl);
                    //ReportDataSource RupColl = new ReportDataSource("RupshiCollection", dtRupColl);

                    ///// Tea Sales Daily
                    //OracleCommand objTeaSale = new OracleCommand();
                    //objTeaSale.CommandText = "Rpt_sales_deposit.TeaSales";
                    //objTeaSale.CommandType = CommandType.StoredProcedure;
                    //objTeaSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    //objTeaSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = fromDate; // from date
                    //objTeaSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = toDate; // to date
                    //DataTable dtTeaSale = classDt.GetData(objTeaSale);
                    //ReportDataSource TeaSale = new ReportDataSource("TeaSale", dtTeaSale);

                    ///// Tea Sales Monthly
                    //OracleCommand objTeaSaleMonthly = new OracleCommand();
                    //objTeaSaleMonthly.CommandText = "Rpt_sales_deposit.TeaSales";
                    //objTeaSaleMonthly.CommandType = CommandType.StoredProcedure;
                    //objTeaSaleMonthly.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    //objTeaSaleMonthly.Parameters.Add("sel1", OracleDbType.Varchar2).Value = FstDateMonth; // from date
                    //objTeaSaleMonthly.Parameters.Add("sel2", OracleDbType.Varchar2).Value = LstDateMonth; // to date
                    //DataTable dtTeaSaleMonthly = classDt.GetData(objTeaSaleMonthly);
                    //ReportDataSource TeaSaleMonthly = new ReportDataSource("TeaSaleMonthly", dtTeaSaleMonthly);


                    ////Rahima Foods Casew Nuts Daily
                    //OracleCommand objRahimaSale = new OracleCommand();
                    //objRahimaSale.CommandText = "Rpt_sales_deposit.RahimaSales";
                    //objRahimaSale.CommandType = CommandType.StoredProcedure;
                    //objRahimaSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    //objRahimaSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = fromDate; // from date
                    //objRahimaSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = toDate; // to date
                    //DataTable dtRahimaSale = classDt.GetData(objRahimaSale);
                    //ReportDataSource RahimaSale = new ReportDataSource("RahimaSales", dtRahimaSale);

                    //Rahima Foods Casew Nuts Daily
                    OracleCommand objLpgSale = new OracleCommand();
                    objLpgSale.CommandText = "Rpt_sales_deposit.LPGSales";
                    objLpgSale.CommandType = CommandType.StoredProcedure;
                    objLpgSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objLpgSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = fromDate; // from date
                    objLpgSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = toDate; // to date
                    DataTable dtLpgSale = classDt.GetData(objLpgSale);
                    ReportDataSource LPGSale = new ReportDataSource("LPGSales", dtLpgSale);

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
                    ReportViewer1.LocalReport.DataSources.Add(RupSale);
                    ReportViewer1.LocalReport.DataSources.Add(LPGSale);
                    //ReportViewer1.LocalReport.DataSources.Add(RupSaleMonthly);
                    //ReportViewer1.LocalReport.DataSources.Add(RupSaleTillDate);
                    //ReportViewer1.LocalReport.DataSources.Add(RupColl);
                    //ReportViewer1.LocalReport.DataSources.Add(TeaSale);
                    //ReportViewer1.LocalReport.DataSources.Add(TeaSaleMonthly);
                    //ReportViewer1.LocalReport.DataSources.Add(RahimaSale);
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

                    string deviceInfo =
                      @"<DeviceInfo>
                        <OutputFormat>PDF</OutputFormat>
                        <PageWidth>8.27in</PageWidth>
                        <PageHeight>11.69in</PageHeight>
                        <MarginTop>0.3in</MarginTop>
                        <MarginLeft>0.25in</MarginLeft>
                        <MarginRight>0in</MarginRight>
                        <MarginBottom>0.3in</MarginBottom>
                    </DeviceInfo>";

                    byte[] file = ReportViewer1.LocalReport.Render("pdf", deviceInfo);

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "inline;filename=Top Sheet.pdf");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.BinaryWrite(file);

                    Response.End();
                }

                if (reportType == "DailyDOCollectionCheque")
                {

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





                    DateTime selectedFromDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime selectedToDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DataTable combinedDT = new DataTable();



                    while (Convert.ToDateTime(selectedFromDate) <= Convert.ToDateTime(selectedToDate))
                    {
                        /// cheque in hand
                        OracleCommand objCmdChequeInHand = new OracleCommand();
                        objCmdChequeInHand.CommandText = "Rpt_sales_coll_cheque.cheque_inhand";
                        objCmdChequeInHand.CommandType = CommandType.StoredProcedure;
                        objCmdChequeInHand.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdChequeInHand.Parameters.Add("t_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdChequeInHand.Parameters.Add("call_name", OracleDbType.Varchar2).Value = null;
                        objCmdChequeInHand.Parameters.Add("sel1", OracleDbType.Varchar2).Value = null;
                        objCmdChequeInHand.Parameters.Add("sel2", OracleDbType.Varchar2).Value = null; // 
                        objCmdChequeInHand.Parameters.Add("sel3", OracleDbType.Varchar2).Value = selectedFromDate.ToString("dd/MM/yyyy"); // from date;
                        objCmdChequeInHand.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
                        objCmdChequeInHand.Parameters.Add("sel5", OracleDbType.Varchar2).Value = null;
                        DataTable dtChequeInHand = classDt.GetData(objCmdChequeInHand);
                        combinedDT.Merge(dtChequeInHand);
                        selectedFromDate = Convert.ToDateTime(selectedFromDate).AddDays(1);
                    }

                    ReportDataSource chequeInHand = new ReportDataSource("ChequeInHand", combinedDT);







                    /// collection section
                    OracleCommand objCmdCol = new OracleCommand();
                    objCmdCol.CommandText = "Rpt_sales_coll_cheque.daily_collection";
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
                    objCmdDOSale.CommandText = "Rpt_sales_coll_cheque.sales_summ_noadv"; // ";
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


                    /// auto rice sale
                    OracleCommand objAutRice = new OracleCommand();
                    objAutRice.CommandText = "Rpt_sales_coll_cheque.auto_rice_sale";
                    objAutRice.CommandType = CommandType.StoredProcedure;
                    objAutRice.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objAutRice.Parameters.Add("sel1", OracleDbType.Varchar2).Value = fromDate; // from date
                    objAutRice.Parameters.Add("sel2", OracleDbType.Varchar2).Value = toDate; // to date
                    DataTable dtAR = classDt.GetData(objAutRice);



                    //*************////// end last month till today data calculation/////**********
                    //******************************************************************
                    //******************************************************************

                    /// do sale with auto rice merge
                    DataTable DOWithAR = new DataTable();
                    DOWithAR = dtDOSale.Copy();
                    DOWithAR.Merge(dtAR);
                    DOWithAR.Merge(dtCol);
                    DOWithAR.Merge(combinedDT); // checque in hand


                    ReportDataSource dosale = new ReportDataSource("DOSale", DOWithAR);


                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    LocalReport LocalReport = new LocalReport();
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SaleTopSheet/DOIssueCollecCheque.rdlc");


                    ReportViewer1.LocalReport.DataSources.Add(collection);
                    ReportViewer1.LocalReport.DataSources.Add(dosale); /// lagbe
                    ReportViewer1.LocalReport.DataSources.Add(chequeInHand);


                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });

                    LocalReport.EnableHyperlinks = true;
                    LocalReport.DataSources.Clear();

                    LocalReport.SetParameters(p1);
                    LocalReport.SetParameters(p2);
                    LocalReport.SetParameters(p3);

                    string deviceInfo =
                      @"<DeviceInfo>
                        <OutputFormat>PDF</OutputFormat>
                        <PageWidth>8.27in</PageWidth>
                        <PageHeight>11.69in</PageHeight>
                        <MarginTop>0.3in</MarginTop>
                        <MarginLeft>0.25in</MarginLeft>
                        <MarginRight>0in</MarginRight>
                        <MarginBottom>0.3in</MarginBottom>
                    </DeviceInfo>";

                    byte[] file = ReportViewer1.LocalReport.Render("pdf", deviceInfo);

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "inline;filename=Top Sheet.pdf");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.BinaryWrite(file);

                    Response.End();
                }

                else
                {
                    // activity monitoring
                    string reportFilteringData = company + "-" + location + "-" + fromDate + "-" + toDate;
                    ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                    string autoId = reportActivityMonitoring.RequestStart("Month Year Sales Comparison", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

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

                    DateTime firstDay = new DateTime(DateTime.Now.Year-2, 1, 1);

                    OracleCommand objLastMonthDOSale = new OracleCommand();
                    objLastMonthDOSale.CommandText = "RPT_SALES_DEPOSIT_PRE_SALE.MonthYearDOSales";
                    objLastMonthDOSale.CommandType = CommandType.StoredProcedure;
                    objLastMonthDOSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objLastMonthDOSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = firstDay.ToString("dd/MM/yyyy"); // firstDayOfLastMonth.Date.ToString("dd/MM/yyyy"); // from date
                    objLastMonthDOSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = DateTime.Now.Date.ToString("dd/MM/yyyy"); // to date
                    DataTable dtLastMonthDOSale = classDt.GetData(objLastMonthDOSale);


                    /// sodoy and sodoyer biporite do sale
                    OracleCommand objCmdAdvanceSale = new OracleCommand();
                    objCmdAdvanceSale.CommandText = "RPT_SALES_DEPOSIT_PRE_SALE.MonthYearDOfromSadoy";
                    objCmdAdvanceSale.CommandType = CommandType.StoredProcedure;
                    objCmdAdvanceSale.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmdAdvanceSale.Parameters.Add("sel1", OracleDbType.Varchar2).Value = firstDay.ToString("dd/MM/yyyy"); // firstDayOfLastMonth.Date.ToString("dd/MM/yyyy"); // from date
                    objCmdAdvanceSale.Parameters.Add("sel2", OracleDbType.Varchar2).Value = DateTime.Now.Date.ToString("dd/MM/yyyy"); // to date
                    DataTable dtDOfromSadoy = classDt.GetData(objCmdAdvanceSale);

                    //Newly Added                    
                    OracleCommand objCmdCol = new OracleCommand();
                    objCmdCol.CommandText = "RPT_SALES_DEPOSIT_PRE_SALE.MonthYearCollection";
                    objCmdCol.CommandType = CommandType.StoredProcedure;
                    objCmdCol.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmdCol.Parameters.Add("sel1", OracleDbType.Varchar2).Value = null; // location
                    objCmdCol.Parameters.Add("sel2", OracleDbType.Varchar2).Value = null; // company
                    objCmdCol.Parameters.Add("sel3", OracleDbType.Varchar2).Value = null;
                    objCmdCol.Parameters.Add("sel4", OracleDbType.Varchar2).Value = null;
                    objCmdCol.Parameters.Add("sel5", OracleDbType.Varchar2).Value = firstDay.ToString("dd/MM/yyyy"); // from date
                    objCmdCol.Parameters.Add("sel6", OracleDbType.Varchar2).Value = DateTime.Now.Date.ToString("dd/MM/yyyy");// to date
                    objCmdCol.Parameters.Add("sel7", OracleDbType.Varchar2).Value = null;
                    DataTable dtCol = classDt.GetData(objCmdCol);


                    DataTable lastMonthTotalSale = new DataTable();
                    lastMonthTotalSale = dtLastMonthDOSale.Copy();
                    lastMonthTotalSale.Merge(dtDOfromSadoy);
                    lastMonthTotalSale.Merge(dtCol);

                    ReportDataSource LastMonthTotalSale = new ReportDataSource("LastMonthTotalSale", lastMonthTotalSale);

                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    LocalReport LocalReport = new LocalReport();
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SaleTopSheet/SalesComparison.rdlc");


                    ReportViewer1.LocalReport.DataSources.Add(LastMonthTotalSale);



                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });

                    LocalReport.EnableHyperlinks = true;
                    LocalReport.DataSources.Clear();

                    LocalReport.SetParameters(p1);
                    LocalReport.SetParameters(p2);
                    LocalReport.SetParameters(p3);

                    // writing db for report process end time
                    reportActivityMonitoring.RequestEnd(autoId);

                    string deviceInfo =
                      @"<DeviceInfo>
                        <OutputFormat>PDF</OutputFormat>
                        <PageWidth>8.27in</PageWidth>
                        <PageHeight>11.69in</PageHeight>
                        <MarginTop>0.3in</MarginTop>
                        <MarginLeft>0.25in</MarginLeft>
                        <MarginRight>0in</MarginRight>
                        <MarginBottom>0.3in</MarginBottom>
                    </DeviceInfo>";

                    byte[] file = ReportViewer1.LocalReport.Render("pdf", deviceInfo);

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "inline;filename=Month-Year Comparison.pdf");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.BinaryWrite(file);

                    Response.End();
                }

            }
        }
    }
}