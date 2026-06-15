//using CTGroup.Models;
using CTGroup.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using CTGroup.Models.ViewModel.Sales;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using CTGroup.Utility.Common;
using System.Configuration;
using System.Diagnostics;
//using CTGroup.Models.ViewModel.Expense;
using CTGroup.OracleModel.ViewModel.Expense;
using CTGroup.Service.Expense.Interfaces;

namespace CTGroup.Service.Expense.Factories
{
    public class ExpenseEntryMgt : iExpenseEntryMgt
    {
        readonly ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
        public IEnumerable<vmExpenseEntry> GetExpenseMaster(vmCmnParameters objcmnParam, out long recordsTotal)
        {
            IEnumerable<vmExpenseEntry> objExpenseMaster = null;
            IEnumerable<vmExpenseEntry> objExpenseMasterWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_EXPENSE.GET_EXPENSEMASTER";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            DataTable dt = classDt.GetDataEkhon(objCmd);
            objExpenseMasterWithOutPaging = ConvertDataTableToGenericList.BindList<vmExpenseEntry>(dt);
            objExpenseMaster = objExpenseMasterWithOutPaging.OrderByDescending(x => x.EXPENSEID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objExpenseMasterWithOutPaging.Count();
            return objExpenseMaster;
        }

        public string SaveUpdateExpense(vmCmnParameters objcmnParam, vmExpenseEntry objExEntry, List<vmExpenseEntry> expenseEntries)
        {
            string EXPENSEID = string.Empty;

            int isValidCollection = 0;
            OracleCommand cmd = new OracleCommand();

            cmd.CommandText = "BASIC_INFO.GET_COLL_UNPOST_VALIDATION";
            cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("P_PAYMENT_MONTH", OracleDbType.Varchar2).Value = objExEntry.EXPENSEMONTH.Value.ToString("MMMM");
            cmd.Parameters.Add("P_PAYMENT_YEAR", OracleDbType.Decimal).Value = objExEntry.EXPENSEMONTH.Value.Year;

            cmd.Parameters.Add("P_RENT_MONTH", OracleDbType.Varchar2).Value = objExEntry.EXPENSEMONTH.Value.ToString("MMMM");
            cmd.Parameters.Add("P_RENT_YEAR", OracleDbType.Decimal).Value = objExEntry.EXPENSEMONTH.Value.Year;

            cmd.CommandType = CommandType.StoredProcedure;

            DataTable dt = classDt.GetDataEkhon(cmd);

            foreach (DataRow row in dt.Rows)
            {
                isValidCollection = Convert.ToInt16(row["ISRENTMONTHACTIVE"]);
            }

            string ExpenseList = "";
            ExpenseList = GetDetails(expenseEntries);

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["EkhonDbContext"].ConnectionString);

            if (isValidCollection == 1)
            {
                try
                {
                    using (OracleCommand objCmd = new OracleCommand())
                    {
                        using (objCmd.Connection = con)
                        {
                            objCmd.CommandType = CommandType.StoredProcedure;
                            if (objExEntry.EXPENSEID > 0)
                            {
                                objCmd.CommandText = "PKG_EXPENSE.UPDATE_EXPENSEENTRY";

                                objCmd.Parameters.Add("P_EXPENSEID", OracleDbType.Decimal).Value = objExEntry.EXPENSEID;

                                objCmd.Parameters.Add("P_BUILDINGID", OracleDbType.Decimal).Value = objExEntry.BUILDINGID;
                                objCmd.Parameters.Add("P_EXPENSEMONTH", OracleDbType.Date).Value = objExEntry.EXPENSEMONTH;

                                objCmd.Parameters.Add("P_UPDATEBY", OracleDbType.Int32).Value = objcmnParam.loggeduser;
                                objCmd.Parameters.Add("P_UPDATEON", OracleDbType.Date).Value = DateTime.Now;
                                objCmd.Parameters.Add("P_UPDATEIP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();

                                objCmd.Parameters.Add("P_EXPENSELIST", OracleDbType.Varchar2, 10000).Value = ExpenseList;

                                objCmd.Connection.Open();
                                objCmd.ExecuteNonQuery();
                                objCmd.Connection.Close();

                                EXPENSEID = "updated"; //objExEntry.EXPENSEID.ToString();
                            }
                            else
                            {
                                objCmd.CommandText = "PKG_EXPENSE.SET_EXPENSEENTRY";
                                objCmd.Parameters.Add("P_EXPENSEID", OracleDbType.Long, 35).Direction = ParameterDirection.Output;
                                objCmd.Parameters.Add("P_BUILDINGID", OracleDbType.Decimal).Value = objExEntry.BUILDINGID;
                                objCmd.Parameters.Add("P_EXPENSEMONTH", OracleDbType.Date).Value = objExEntry.EXPENSEMONTH;
                                objCmd.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;
                                objCmd.Parameters.Add("P_CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                objCmd.Parameters.Add("P_CREATED_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();

                                objCmd.Parameters.Add("P_EXPENSELIST", OracleDbType.Varchar2, 10000).Value = ExpenseList;

                                objCmd.Connection.Open();
                                objCmd.ExecuteNonQuery();
                                objCmd.Connection.Close();

                                EXPENSEID = "saved"; //objCmd.Parameters["P_EXPENSEID"].Value.ToString();
                            }
                        }
                    }
                }
                catch (OracleException exception)
                {
                    var frame = new StackTrace(true).GetFrame(0);
                    var filename = frame.GetFileName();
                    var line = frame.GetFileLineNumber();
                }
            }
            else
            {
                EXPENSEID = "-1";
            }
            return EXPENSEID;
        }

        public object[] GetExpenseByID(vmCmnParameters objcmnParam, Int64 EXPENSEID)
        {
            object[] lstExpense = null;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["EkhonDbContext"].ConnectionString);

            DataTable t1 = new DataTable();            

            List<vmExpenseEntry> expenseList = null;

            con.Open();
            string docquery = "select EXM.BUILDINGID, FLOORNO, FLATNO, EXPENSEAREAHEADID, " +
                "EH.EXPENSEHEADNAME,REQUISITIONBY,EXPENSEDATE,TOTALCOST, APPROVEDBY,EXPENSEDETAIL  " +
                "from T_EXPENSEMASTER EXM INNER JOIN T_EXPENSEDETAIL EXD ON EXM.EXPENSEID= EXD.EXPENSEID " +
                "INNER JOIN T_EXPENSEHEAD EH ON EXD.EXPENSEAREAHEADID = EH.EXPENSEID  where EXD.EXPENSEID = " + EXPENSEID + " ";

            OracleCommand cmd = new OracleCommand(docquery, con);

            t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();
            expenseList = ConvertDataTableToGenericList.BindList<vmExpenseEntry>(t1);

            try
            {
                lstExpense = (from exp in expenseList
                                select new
                                {
                                    exp.BUILDINGID,
                                    exp.EXPENSEAREAHEADID,
                                    exp.EXPENSEHEADNAME,
                                    exp.FLOORNO,
                                    exp.FLATNO,
                                    exp.REQUISITIONBY,
                                    exp.EXPENSEDATE,
                                    exp.TOTALCOST,
                                    exp.APPROVEDBY,
                                    exp.EXPENSEDETAIL                                  
                                }).ToList().ToArray();
            }

            catch (Exception ex)
            {
                ex.ToString();
            }
            return lstExpense;
        }


        public string GetDetails(List<vmExpenseEntry> ExpenseMaster)
        {
            string multivalue = "";

            foreach (var item in ExpenseMaster)
            {
                decimal? TOTALCOST = 0;
                decimal? EXPENSEAREAHEADID = 0;
                string EXPENSEDETAIL = "";
                string REQUISITIONBY = "";
                string APPROVEDBY = "";
                string[] flatList = new string[20];
                string flatno = "";

                TOTALCOST = item.TOTALCOST;
                EXPENSEAREAHEADID = item.EXPENSEAREAHEADID;
                string EXPENSEDATE = item.EXPENSEDATE.ToString("dd-MMMM-yyyy");
                REQUISITIONBY = item.REQUISITIONBY == "" ? "0" : item.REQUISITIONBY;
                APPROVEDBY = item.APPROVEDBY == "" ? "0" : item.APPROVEDBY;
                EXPENSEDETAIL = item.EXPENSEDETAIL == "" ? "0" : item.EXPENSEDETAIL;


                if (item.FLOORFLATNO != null && item.FLOORFLATNO != "" && item.FLOORFLATNO != " - ")
                {
                    flatList = item.FLOORFLATNO.Split('-');
                    flatno = item.FLOORFLATNO.Remove(0, 4);
                }
                else
                {
                    flatList[0] = "77";
                    flatno = "77";
                }
                string singlevlaue = "x" + ':' + EXPENSEAREAHEADID + ':' + flatList[0].Trim() + ':' + flatno.Trim() + ':'
                              + REQUISITIONBY + ':' + EXPENSEDATE +
                               ':' + TOTALCOST + ':' + APPROVEDBY + ':' + EXPENSEDETAIL + ';';

                multivalue += singlevlaue;
            }
            return multivalue;
        }
    }
}
