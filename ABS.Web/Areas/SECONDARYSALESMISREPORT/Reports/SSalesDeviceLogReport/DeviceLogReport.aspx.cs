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

namespace CGSS.Web.ViewReport
{
    public partial class DeviceLogReport : System.Web.UI.Page
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
                    Response.Redirect("~/Account/Login");
                }

                UserName = Session["UserFullName"].ToString();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string nationalId = queryStrings[0];
                string division_Id = queryStrings[1];
                string region_Id = queryStrings[2];
                string zone_Id = queryStrings[3];
                string dist_Id = queryStrings[4];
                string reportFilter = queryStrings[5];
                string reportOption = queryStrings[6];


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
                

                LocalReport LocalReport = new LocalReport();
                LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/SSalesDeviceLogReport/SSalesDeviceLogReport.rdlc");


                OracleCommand objCmd = new OracleCommand();
                objCmd.CommandText = "PKG_CSSAP_DEVICE.GET_CSSAP_DEVICE_LOG_BY_PARAM";
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = "1";
                objCmd.Parameters.Add("p_national_id", OracleDbType.Varchar2).Value = nationalId == "0" ? null : nationalId;
                objCmd.Parameters.Add("p_division_id", OracleDbType.Varchar2).Value = division_Id == "0" ? null : division_Id;
                objCmd.Parameters.Add("p_region_id", OracleDbType.Varchar2).Value = region_Id == "0" ? null : region_Id;
                objCmd.Parameters.Add("p_zone_id", OracleDbType.Varchar2).Value = zone_Id == "0" ? null : zone_Id;
                objCmd.Parameters.Add("p_dist_id", OracleDbType.Varchar2).Value = dist_Id == "0" ? null : dist_Id;
                objCmd.Parameters.Add("p_so_id", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("p_report_option", OracleDbType.Varchar2).Value = reportOption;



                DataTable dt = classDt.GetSecondaryData(objCmd);

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);

                ReportDataSource datasource = new ReportDataSource("DeviceLogDS", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });
            }
        }

    }


}