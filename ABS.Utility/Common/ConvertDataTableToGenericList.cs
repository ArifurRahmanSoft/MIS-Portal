using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

namespace CTGroup.Utility.Common
{
    public class ConvertDataTableToGenericList
    {
        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }
        public static List<T> BindList<T>(DataTable dt)
        {
            // Example 1:
            // Get private fields + non properties
            //var fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Example 2: Your case
            // Get all public fields
            var fields = typeof(T).GetProperties();

            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            string type = dr[dc.ColumnName].GetType().FullName;
                            object value = new object();
                            if (type == "System.DBNull")
                            {
                                value = ConvertFromDBVal<string>(dr[dc.ColumnName]);
                            }
                            else if (type == "System.Boolean")
                            {
                                value = dr[dc.ColumnName] == null ? 0 : 1;
                            }
                            else
                            {
                                value = dr[dc.ColumnName];
                            }

                            // Set the value into the object
                            try
                            {
                                fieldInfo.SetValue(ob, value);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Generic Exception Handler: {0}", e.ToString());
                            }
                            break;
                        }
                    }
                }
                lst.Add(ob);
            }
            return lst;
        }
        public static List<T> BindListExcelData<T>(DataTable dt)
        {
            // Example 1:
            // Get private fields + non properties
            //var fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Example 2: Your case
            // Get all public fields
            var fields = typeof(T).GetProperties();

            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            string type = dr[dc.ColumnName].GetType().FullName;
                            object value = new object();
                            if (type == "System.DBNull")
                            {
                                value = ConvertFromDBVal<string>(dr[dc.ColumnName]);
                            }
                            else if (type == "System.Boolean")
                            {
                                value = dr[dc.ColumnName] == null ? 0 : 1;
                            }
                            else if (type == "System.Double")
                            {
                                value = Convert.ToDecimal(dr[dc.ColumnName]);
                            }
                            else
                            {
                                value = dr[dc.ColumnName];
                            }

                            // Set the value into the object
                            try
                            {
                                fieldInfo.SetValue(ob, value);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Generic Exception Handler: {0}", e.ToString());
                            }
                            break;
                        }
                    }
                }
                lst.Add(ob);
            }
            return lst;
        }
        public DataTable GetDataBasic(OracleCommand objCmd)
        {
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        sda.SelectCommand = objCmd;
                        sda.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }
        public DataTable GetSecondaryBasic(OracleCommand objCmd)
        {
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["SecondarySales"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        sda.SelectCommand = objCmd;
                        sda.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }

        public DataTable GetDataArif(OracleCommand objCmd, string strConn)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OracleConnection con = new OracleConnection(strConn))
                {
                    con.Open();

                    //DataTable dt = new DataTable();

                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        //objCmd.ExecuteNonQuery();
                        sda.SelectCommand = objCmd;

                        sda.Fill(dt);
                        con.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                //throw;
            }

            return dt;
        }

        public DataTable Getproduct(OracleCommand objCmd)
        {
            DataTable dt = new DataTable();
            //using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContextReport"].ConnectionString))
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["productInsert"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        sda.SelectCommand = objCmd;
                        sda.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }


        //-----------------------------Start-----------------------------
       public DataTable GetDataDropDown(OracleCommand objCmd, string strConn)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OracleConnection con = new OracleConnection(strConn))
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        //objCmd.ExecuteNonQuery();
                        sda.SelectCommand = objCmd;

                        sda.Fill(dt);
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                //throw;
            }

            return dt;
        }


        /*public DataTable GetDataProduct(OracleCommand objCmd, string strConn)
        {
            DataTable dt = new DataTable();

            try
            {
                using (OracleConnection con = new OracleConnection(strConn))
                {
                    con.Open();
                    objCmd.Connection = con;
                    objCmd.ExecuteNonQuery(); // Execute the stored procedure

                    // Check if the output parameter contains a CLOB
                    OracleClob clob = objCmd.Parameters["gresult"].Value as OracleClob;
                    string clobData = "";

                    if (clob != null && !clob.IsNull)
                    {
                        using (StreamReader reader = new StreamReader(clob, Encoding.UTF8))
                        {
                            clobData = reader.ReadToEnd();
                        }
                    }

                   // Create DataTable and add a column for the CLOB data
                    dt.Columns.Add("Result", typeof(string));
                    dt.Rows.Add(clobData);
                    

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return dt;
        }

*/

        public string ExecuteNonQueryOutClob(string spQuery, OracleCommand cmd, string OutParameter, string conString)
        {
            //return Task.Run(() =>
            //{
            string result = string.Empty;
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    con.Open();
                    cmd.CommandText = spQuery;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.ExecuteNonQueryAsync();
                    OracleClob res = (OracleClob)cmd.Parameters[OutParameter].Value;
                    result = string.IsNullOrEmpty(res.Value) ? string.Empty : res.Value.ToString();
                    cmd.Connection.Close();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                //Logs.WriteBug(ex);
            }

            return result;
            //});
        }


        //End-----------------------

        public DataTable GetData(OracleCommand objCmd)
        {
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContextReport"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        sda.SelectCommand = objCmd;
                        sda.Fill(dt);
                    }
                }
                catch (Exception ex)                
                {
                    ex.ToString();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }

        public DataTable GetDataCityn(OracleCommand objCmd)
        {
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["dbCityn"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        sda.SelectCommand = objCmd;
                        sda.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }

        public DataTable GetDataEkhon(OracleCommand objCmd)
        {
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["EkhonDbContext"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        sda.SelectCommand = objCmd;
                        sda.Fill(dt);
                    }
                }
                catch (Exception ex)

                {
                    ex.ToString();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }

        public DataTable GetSecondaryData(OracleCommand objCmd)
        {
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["SecondarySalesReport"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        sda.SelectCommand = objCmd;
                        sda.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }

        public DataTable GetCssapData(OracleCommand objCmd)
        {
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["cssap_db"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        sda.SelectCommand = objCmd;
                        sda.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }

        public DataTable GetSecondaryDataMaxTime(OracleCommand objCmd)
        {
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["SecondarySalesReport"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.FetchSize = objCmd.FetchSize * 18192;
                        objCmd.Connection = con;
                        sda.SelectCommand = objCmd;
                        sda.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }

        public DataTable GetSCMData(OracleCommand objCmd)
        {
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbSCM"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        //objCmd.ExecuteNonQuery();
                        sda.SelectCommand = objCmd;
                        sda.Fill(dt);
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }
        public string GetGUID()
        {
            string GUID = "";
            OracleCommand cmd = new OracleCommand();

            cmd.CommandText = "SELECT RAWTOHEX(SYS_GUID()) GUID FROM DUAL";

            cmd.CommandType = CommandType.Text;

            DataTable dt = GetData(cmd);

            foreach (DataRow row in dt.Rows)
            {
                GUID = row["GUID"].ToString();

            }

            return GUID;
        }
        public static List<T> BindListFiltered<T>(DataTable dt)
        {
            // Example 1:
            // Get private fields + non properties
            //var fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Example 2: Your case
            // Get all public fields
            var fields = typeof(T).GetProperties();

            List<T> lst = new List<T>();



            DataTable newDT = dt.Clone();


            foreach (DataRow dr in dt.Rows)
            {
                if (dr["CUST_ID"] != DBNull.Value)
                {
                    newDT.ImportRow(dr);
                }


            }


            foreach (DataRow dr in newDT.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            string type = dr[dc.ColumnName].GetType().FullName;
                            object value = new object();
                            if (type == "System.DBNull")
                            {
                                value = ConvertFromDBVal<string>(dr[dc.ColumnName]);
                            }
                            else if (type == "System.Boolean")
                            {
                                value = dr[dc.ColumnName] == null ? 0 : 1;
                            }
                            else
                            {
                                if (dc.ColumnName == "SO_ID" && type!= "System.String")
                                {
                                    value = dr[dc.ColumnName].ToString();
                                }
                                else
                                {
                                    value = dr[dc.ColumnName];
                                }
                            }

                            // Set the value into the object
                            try
                            {
                                fieldInfo.SetValue(ob, value);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Generic Exception Handler: {0}", e.ToString());
                            }
                            break;
                        }
                    }
                }

                lst.Add(ob);
            }
            return lst;
        }

        public static List<T> AutoRiceSalesUploadList<T>(DataTable dt)
        {
            var fields = typeof(T).GetProperties();
            List<T> lst = new List<T>();
            DataTable newDT = dt.Clone();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["OID"] != DBNull.Value)
                {
                    newDT.ImportRow(dr);
                }
            }
            foreach (DataRow dr in newDT.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            string type = dr[dc.ColumnName].GetType().FullName;
                            object value = new object();
                            if (type == "System.DBNull")
                            {
                                value = ConvertFromDBVal<string>(dr[dc.ColumnName]);
                            }
                            else if (type == "System.Boolean")
                            {
                                value = dr[dc.ColumnName] == null ? 0 : 1;
                            }
                            else
                            {
                                value = dr[dc.ColumnName];
                            }

                            // Set the value into the object
                            try
                            {
                                fieldInfo.SetValue(ob, value);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Generic Exception Handler: {0}", e.ToString());
                            }
                            break;
                        }
                    }
                }

                lst.Add(ob);
            }
            return lst;
        }

        public static List<T> AutoRiceSalesUploadLists<T>(DataTable dt)
        {
            var fields = typeof(T).GetProperties();
            List<T> lst = new List<T>();
            DataTable newDT = dt.Copy();

            foreach (DataRow dr in newDT.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in newDT.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            string type = dr[dc.ColumnName].GetType().FullName;
                            object value = new object();
                            if (type == "System.DBNull")
                            {
                                value = ConvertFromDBVal<string>(dr[dc.ColumnName]);
                            }
                            else if (type == "System.Boolean")
                            {
                                value = dr[dc.ColumnName] == null ? 0 : 1;
                            }
                            else
                            {
                                value = dr[dc.ColumnName];
                            }

                            // Set the value into the object
                            try
                            {
                                fieldInfo.SetValue(ob, value);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Generic Exception Handler: {0}", e.ToString());
                            }
                            break;
                        }
                    }
                }

                lst.Add(ob);
            }
            return lst;
        }


        public DataTable GetData(OracleCommand objCmd, string conString)
        {
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings[conString].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        objCmd.Connection = con;
                        //objCmd.ExecuteNonQuery();
                        sda.SelectCommand = objCmd;
                        sda.Fill(dt);
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }
    }
}
