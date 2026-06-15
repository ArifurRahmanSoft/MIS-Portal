using System;
using System.Globalization;
using System.Linq;

namespace CTGroup.Utility
{
    public static class UniqueCode
    {        
        public static string GetDateFormat_dd_mm_yyyy(string date)
        {
            string formatedDate = "";
            try
            {
                DateTime newDate = DateTime.Parse(date);
                formatedDate = newDate.ToString("dd/MM/yyyy");
            }
            catch
            {
                formatedDate = "";
            }
            return formatedDate;
        }

        public static string GetDateFormat_dd_mm_yyyy(DateTime date)
        {
            string formatedDate = "";
            try
            {
                formatedDate = date.ToString("dd/MM/yyyy");
            }
            catch
            {
                formatedDate = "";
            }
            return formatedDate;
        }
        public static string GetDateFormat_dd_mm_yyyy(DateTime? chequeDate)
        {
            return (chequeDate != null ? chequeDate.Value.ToString("dd/MM/yyyy") : "n/a");
        }
        public static DateTime GetDateFormat_MM_dd_yyy(string dmyFormat)
        {
            DateTime formatedDate;
            try
            {
                formatedDate = Convert.ToDateTime(DateTime.ParseExact(dmyFormat, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
            }
            catch (Exception exception)
            {
                formatedDate = Convert.ToDateTime(DateTime.ParseExact(dmyFormat, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
            }
            return formatedDate;
        }
    }
}
