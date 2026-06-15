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
using CTGroup.Utility.Common;

namespace CTGroup.Web.Areas.Sales.Reports
{
    public partial class SFIncentiveReport : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                //LocalReport.ReportPath = Server.MapPath("~/ViewReport/Product_Rate_Report/Report_Product_Rate.rdlc");

                LocalReport.ReportPath = Server.MapPath("~/Areas/Sales/Reports/SalesForceIncentiveCalculation.rdlc");
                
                OracleCommand objCmd = new OracleCommand();
                objCmd.CommandText = "REPORTS.SFIncentiveCalculation";
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                DataTable dt = classDt.GetData(objCmd);

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();
                //ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                ReportDataSource datasource = new ReportDataSource("SFIncentiveDS", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);

            }
        }
    }
}