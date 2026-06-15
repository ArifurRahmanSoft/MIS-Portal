using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Data.OleDb;
using System.IO;
using Newtonsoft.Json;

namespace CTGroup.Utility
{
    public static class Conversion
    {
        public static short TryCastShort(object value)
        {
            if (value != null)
            {
                short retVal = 0;
                //string numberToParse = RemoveGroupping(value.ToString());
                string numberToParse = value.ToString();

                if (short.TryParse(numberToParse, out retVal))
                {
                    return retVal;
                }
            }

            return 0;
        }

        public static long TryCastLong(object value)
        {
            if (value != null)
            {
                long retVal = 0;
                //string numberToParse = RemoveGroupping(value.ToString());
                string numberToParse = value.ToString();

                if (long.TryParse(numberToParse, out retVal))
                {
                    return retVal;
                }
            }

            return 0;
        }

        public static float TryCastSingle(object value)
        {
            if (value != null)
            {
                float retVal = 0;
                //string numberToParse = RemoveGroupping(value.ToString());
                string numberToParse = value.ToString();

                if (float.TryParse(numberToParse, out retVal))
                {
                    return retVal;
                }
            }

            return 0;
        }

        public static double TryCastDouble(object value)
        {
            if (value != null)
            {
                double retVal = 0;
                //string numberToParse = RemoveGroupping(value.ToString());
                string numberToParse = value.ToString();

                if (double.TryParse(numberToParse, out retVal))
                {
                    return retVal;
                }
            }

            return 0;
        }

        public static int TryCastInteger(object value)
        {
            if (value != null)
            {
                if (value is bool)
                {
                    if (Convert.ToBoolean(value, CultureInfo.InvariantCulture))
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

                int retVal = 0;
                //string numberToParse = RemoveGroupping(value.ToString());
                string numberToParse = value.ToString();

                if (int.TryParse(numberToParse, out retVal))
                {
                    return retVal;
                }
            }

            return 0;
        }

        public static DateTime TryCastDate(object value)
        {
            try
            {
                if (value == DBNull.Value)
                {
                    return DateTime.MinValue;
                }

                return Convert.ToDateTime(value, System.Threading.Thread.CurrentThread.CurrentCulture);
            }
            catch (FormatException)
            {
                //swallow the exception
            }
            catch (InvalidCastException)
            {
                //swallow the exception
            }

            return DateTime.MinValue;
        }

        public static decimal TryCastDecimal(object value)
        {
            if (value != null)
            {
                decimal retVal = 0;
                //string numberToParse = RemoveGroupping(value.ToString());
                string numberToParse = value.ToString();

                if (decimal.TryParse(numberToParse, out retVal))
                {
                    return retVal;
                }
            }

            return 0;
        }

        public static bool TryCastBoolean(object value)
        {
            if (value != null)
            {
                if (value is string)
                {
                    if (value.ToString().ToLower(System.Threading.Thread.CurrentThread.CurrentCulture).Equals("yes"))
                    {
                        return true;
                    }

                    if (value.ToString().ToLower(System.Threading.Thread.CurrentThread.CurrentCulture).Equals("true"))
                    {
                        return true;
                    }
                }

                bool retVal = false;
                if (bool.TryParse(value.ToString(), out retVal))
                {
                    return retVal;
                }
            }

            return false;
        }

        public static bool IsNumeric(string value)
        {
            double number;
            return double.TryParse(value, out number);
        }

        public static string TryCastString(object value)
        {
            try
            {
                if (value != null)
                {
                    if (value is bool)
                    {
                        if (Convert.ToBoolean(value, CultureInfo.InvariantCulture) == true)
                        {
                            return "true";
                        }
                        else
                        {
                            return "false";
                        }
                    }
                    else
                    {
                        if (value == System.DBNull.Value)
                        {
                            return string.Empty;
                        }
                        else
                        {
                            string retVal = value.ToString();
                            return retVal;
                        }
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (FormatException)
            {
                //swallow the exception
            }
            catch (InvalidCastException)
            {
                //swallow the exception            
            }

            return string.Empty;
        }

        public static string HashSha512(string password, string salt)
        {
            if (password == null)
            {
                return null;
            }

            if (salt == null)
            {
                return null;
            }

            byte[] bytes = Encoding.Unicode.GetBytes(password + salt);
            using (SHA512CryptoServiceProvider hash = new SHA512CryptoServiceProvider())
            {
                byte[] inArray = hash.ComputeHash(bytes);
                return Convert.ToBase64String(inArray);
            }
        }

        public static DateTime GetLocalDateTime(string timeZone, DateTime utc)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            return TimeZoneInfo.ConvertTimeFromUtc(utc, zone);
        }

        public static string GetLocalDateTimeString(string timeZone, DateTime utc)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            DateTime time = TimeZoneInfo.ConvertTimeFromUtc(utc, zone);
            return time.ToLongDateString() + " " + time.ToLongTimeString() + " " + zone.DisplayName;
        }

        public static System.Data.DataTable ConvertListToDataTable<T>(System.Collections.Generic.IList<T> list)
        {
            if (list == null)
            {
                return null;
            }

            System.ComponentModel.PropertyDescriptorCollection props = System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));

            using (System.Data.DataTable table = new System.Data.DataTable())
            {
                table.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;

                for (int i = 0; i < props.Count; i++)
                {
                    System.ComponentModel.PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (T item in list)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
                return table;
            }
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }


            return dt;
        }

        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {

                oledbConn.Open();
                using (DataTable Sheets = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
                {

                    for (int i = 0; i < Sheets.Rows.Count; i++)
                    {
                        string worksheets = Sheets.Rows[i]["TABLE_NAME"].ToString();
                        OleDbCommand cmd = new OleDbCommand(String.Format("SELECT * FROM [{0}]", worksheets), oledbConn);
                        OleDbDataAdapter oleda = new OleDbDataAdapter();
                        oleda.SelectCommand = cmd;

                        oleda.Fill(ds);
                    }

                    dt = ds.Tables[0];
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {

                oledbConn.Close();
            }

            return dt;

        }

        public static DateTime StringToFormattedDate(string fstdate)
        {
            return DateTime.ParseExact(fstdate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static string FirstOfMonth(DateTime fstdate, int mnth, int day)
        {
            string strDate = string.Empty;
            DateTime fstDate = new DateTime(fstdate.Year, fstdate.Month, 1);
            strDate = fstDate.AddMonths(mnth).AddDays(day).ToString("dd/MM/yyyy");
            return strDate;
        }
        public static string LastOfMonth(DateTime fstdate, int mnth, int day)
        {
            string strDate = string.Empty;
            DateTime fstDate = new DateTime(fstdate.Year, fstdate.Month, 1);
            return strDate = fstDate.AddMonths(mnth).AddDays(day).ToString("dd/MM/yyyy");
        }

        public static string FstDateOfMonth(DateTime fstdate)
        {
            string strDate = string.Empty;
            strDate = new DateTime(fstdate.Year, fstdate.Month, 1).ToString("dd/MM/yyyy");
            return strDate;
        }

        public static string LstDateOfMonth(DateTime fstdate)
        {
            string strDate = string.Empty;
            DateTime curDate = DateTime.Now;
            string currMonYear = curDate.ToString("MM-yyyy");
            string getMonYear = fstdate.ToString("MM-yyyy");
            if (currMonYear == getMonYear)
            {
                strDate = curDate.ToString("dd/MM/yyyy");
            }
            else
            {
                DateTime fstDate = new DateTime(fstdate.Year, fstdate.Month, 1);
                strDate = fstDate.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");
            }
            return strDate;
        }

        public static DataTable GetReportDataTable(object dataList)
        {
            DataTable dataTable = new DataTable();
            var serializeData = JsonConvert.SerializeObject(dataList);
            var deSerializeData = JsonConvert.DeserializeObject(serializeData);
            dataTable = JsonConvert.DeserializeObject<DataTable>(deSerializeData.ToString());
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                dataTable = new DataTable();
                DataRow dr = dataTable.NewRow();
                dataTable.Rows.InsertAt(dr, 0);
            }
            return dataTable;
        }

        public static DataTable GetDataTable(object dataList)
        {
            DataTable dataTable = new DataTable();
            var serializeData = JsonConvert.SerializeObject(dataList);
            var deSerializeData = JsonConvert.DeserializeObject(serializeData);
            dataTable = JsonConvert.DeserializeObject<DataTable>(deSerializeData.ToString());
            return dataTable;
        }

        public static dynamic GetJsonFromDataTable(DataTable dataList)
        {
            dynamic data = null;
            data = JsonConvert.SerializeObject(dataList);
            return data;
        }

        public static DataTable AddDataSetNameValue(DataTable dataTable, string dataColumn, string dataValue)
        {
            DataColumn dataCol = new DataColumn(dataColumn, typeof(String));
            dataCol.DefaultValue = dataValue;
            dataTable.Columns.Add(dataCol);
            return dataTable;
        }
    }
}
