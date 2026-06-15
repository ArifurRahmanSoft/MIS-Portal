using ABS.Web.Utility;
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

namespace ABS.Web.Areas.MISREPORT.Reports.DepotClosingStockByRate
{
    public partial class DepotClosingStockByRate : System.Web.UI.Page
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

                string companyID = queryStrings[0];
                string locationID = queryStrings[1];
                string brandOID = queryStrings[2];
                string skuOID = queryStrings[3];
                string fromDate = queryStrings[4];
                string toDate = queryStrings[5];
                string selectdLocation = queryStrings[6];
                string reportType = queryStrings[7];

                if (companyID == "" || companyID == "undefined" || companyID == "null")
                    companyID = null;

                if (locationID == "" || locationID == "undefined" || locationID == "null")
                    locationID = null;

                if (brandOID == "" || brandOID == "undefined" || brandOID == "null")
                    brandOID = null;

                if (skuOID == "" || skuOID == "undefined" || skuOID == "null")
                    skuOID = null;

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();
                ReportParameter p4 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);

                p2.Name = "ToDate";
                p2.Values.Add(toDate);

                p3.Name = "UserName";
                p3.Values.Add(UserName);

                p4.Name = "SelectdLocation";
                p4.Values.Add(selectdLocation);

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();
         
                if (reportType == "Details")
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DepotClosingStockByRate/DepoStockByRate.rdlc");

                if (reportType == "Depots")
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DepotClosingStockByRate/ByDepotsSkuWise.rdlc");

                if (reportType == "Brand")
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DepotClosingStockByRate/ByDepotsBrandWise.rdlc");

                if (reportType == "ProductMrps")
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DepotClosingStockByRate/ByDepotsMRPWiseSimplified.rdlc");

                objCmd.CommandText = "DEPOSTOCK_RATE.depot_stock_by_rate";

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("p_LOCATION", OracleDbType.Varchar2).Value = locationID;
                objCmd.Parameters.Add("p_Company", OracleDbType.Varchar2).Value = companyID;
                objCmd.Parameters.Add("p_Brand", OracleDbType.Varchar2).Value = brandOID;
                objCmd.Parameters.Add("p_Product", OracleDbType.Varchar2).Value = skuOID;
                objCmd.Parameters.Add("p_START_DATE", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("p_END_DATE", OracleDbType.Varchar2).Value = toDate;
                               
                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DepoStockByRate", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();
            }
        }
    }
}