using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Security.Cryptography;
using CTGroup.Utility.Common;
using ABS.Web.Utility;
using CTGroup.Utility;

namespace CGSS.Web.ViewReport
{
    public partial class ActivityReportBak : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
        protected void Page_Load(object sender, EventArgs e)
        {
            string iid = "2";//Request.QueryString["Id"];           

            if (!IsPostBack)
            {
                string UserName = "";
                string UserId = Session["UserID"].ToString();

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

                ReportViewer1.ProcessingMode = ProcessingMode.Local;                


                if (iid == "2" )
                {
                    string[] queryStrings = Request["queryString"].ToString().Split(',');
                    string fromDate = queryStrings[0];
                    string toDate = queryStrings[1];
                    string nationalId = queryStrings[2];
                    string division_Id = queryStrings[3];
                    string region_Id = queryStrings[4];
                    string zone_Id = queryStrings[5];
                    string dist_Id = queryStrings[6];
                    string reportFilter = queryStrings[7];
                    string so_Id = "0";//Request.QueryString["so_Id"];                  
                    string arrangeType = "6";//Request.QueryString["arrangeType"];

                    Utils utility = new Utils();
                    if (utility.IsNotValidActivity(UserId, nationalId))
                    {
                        Session.Abandon(); // Does nothing
                        Session.Clear();   // Removes the data contained in the session
                        Response.Redirect("~/Account/Login");
                    }

                    // activity monitoring
                    string reportFilteringData = reportFilter + "-" + "-" + fromDate + "-" + toDate;
                    ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                    string autoId = reportActivityMonitoring.RequestStart("Activity Report", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                    if (Session["UserID"].ToString() == "08414")
                        fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

                    if (nationalId == "")
                        nationalId = "0";

                    if (division_Id == "")
                        division_Id = "0";

                    if (region_Id == "")
                        region_Id = "0";

                    if (zone_Id == "")
                        zone_Id = "0";

                    if (dist_Id == "")
                        dist_Id = "0";                   


                    ReportParameter p1 = new ReportParameter();
                    ReportParameter p2 = new ReportParameter();

                    p1.Name = "FromDate";
                    p1.Values.Add(fromDate);
                    p2.Name = "ToDate";
                    p2.Values.Add(toDate);

                    LocalReport LocalReport = new LocalReport();

                    if (arrangeType == "2")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/ViewReport/Activity_Report/DIVISION_ActivityReport.rdlc");
                    }

                    if (arrangeType == "3")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/ViewReport/Activity_Report/REGION_ActivityReport.rdlc");
                    }


                    if (arrangeType == "4")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/ViewReport/Activity_Report/ZONE_ActivityReport.rdlc");
                    }

                    if (arrangeType == "5")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/ViewReport/Activity_Report/DIST_ActivityReport.rdlc");
                    }

                    if (arrangeType == "6")
                    {
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ActivityReport/SOActivityReport.rdlc");
                    }

                    OracleCommand objCmd = new OracleCommand();
                    objCmd.CommandText = "PKG_ACTIVITY_REPORT.getOrderActivitySummary";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = arrangeType=="6"?"1":"2";
                    objCmd.Parameters.Add("p_s_date", OracleDbType.Varchar2).Value = fromDate;
                    objCmd.Parameters.Add("p_e_date", OracleDbType.Varchar2).Value = toDate;
                    objCmd.Parameters.Add("p_division_id", OracleDbType.Varchar2).Value = division_Id=="0"?null: division_Id;
                    objCmd.Parameters.Add("p_region_id", OracleDbType.Varchar2).Value = region_Id == "0" ? null : region_Id;
                    objCmd.Parameters.Add("p_zone_id", OracleDbType.Varchar2).Value = zone_Id == "0" ? null : zone_Id;
                    objCmd.Parameters.Add("p_dist_id", OracleDbType.Varchar2).Value = dist_Id == "0" ? null : dist_Id;
                    objCmd.Parameters.Add("p_so_id", OracleDbType.Varchar2).Value = so_Id == "0" ? null : so_Id;


                    DataTable dt = classDt.GetSecondaryData(objCmd);

                    LocalReport.EnableHyperlinks = true;
                    LocalReport.DataSources.Clear();

                    LocalReport.SetParameters(p1);
                    LocalReport.SetParameters(p2);

                    ReportDataSource datasource = new ReportDataSource("ActivityReportDataSet", dt);

                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });

                    reportActivityMonitoring.RequestEnd(autoId);

                    //        localReports.Add(LocalReport);

                    //        string deviceInfo =
                    //     @"<DeviceInfo>
                    //    <OutputFormat>PDF</OutputFormat>
                    //    <PageWidth>16in</PageWidth>
                    //    <PageHeight>8.5in</PageHeight>
                    //    <MarginTop>0.50in</MarginTop>
                    //    <MarginLeft>0.25in</MarginLeft>
                    //    <MarginRight>0.25in</MarginRight>
                    //    <MarginBottom>0.50in</MarginBottom>
                    //</DeviceInfo>";

                    //        byte[] file = LocalReport.Render("pdf", deviceInfo);

                    //        Response.ContentType = "application/pdf";
                    //        Response.AddHeader("content-disposition", "inline;filename=DC-" + iid + ".pdf");
                    //        Response.Buffer = true;
                    //        Response.Clear();
                    //        Response.BinaryWrite(file);

                    //        Response.End();

                }
            }
        }


        public static string Decrypt(string EncryptedString, string UserName)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(UserName));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(EncryptedString);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

    }


}