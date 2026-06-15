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

namespace ABS.Web.Areas.INVENTORY.Reports.PurchaseOrderReport
{
    public partial class PurchaseOrderReport : System.Web.UI.Page
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

                string FromDate    = queryStrings[0];
                string ToDate      = queryStrings[1];
                string reportType  = queryStrings[2];
                string productID   = queryStrings[3];
                string Supplier    = queryStrings[4];
                string CompanyUnit = queryStrings[5];
                string ItemGroup   = queryStrings[6];



                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(FromDate);
                p2.Name = "ToDate";
                p2.Values.Add(ToDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);

                if (productID == "" || productID == "Undefine")
                    productID = null;

                if (Supplier == "" || Supplier == "Undefine")
                    Supplier = null;

                if (CompanyUnit == "" || CompanyUnit == "Undefine")
                    CompanyUnit = null;

                if (ItemGroup == "" || ItemGroup == "Undefine")
                    ItemGroup = null;

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                if (reportType == "topsheet")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/PurchaseOrderReport/PurchaseOrderReportTopSheet.rdlc");
                    objCmd.CommandText = "p_reports.get_po_rpt";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_callname", OracleDbType.Varchar2).Value = "topsheet";
                    objCmd.Parameters.Add("p_cunit_id", OracleDbType.Varchar2).Value = CompanyUnit;
                    objCmd.Parameters.Add("p_itm_grp", OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_frm_dt", OracleDbType.Varchar2).Value = FromDate;
                    objCmd.Parameters.Add("p_to_dt", OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_supp_id", OracleDbType.Varchar2).Value = Supplier;
                    objCmd.Parameters.Add("p_user", OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_user_id", OracleDbType.Varchar2).Value = "";
                }
                if (reportType == "details")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/PurchaseOrderReport/PurchaseOrderReportDetails.rdlc");
                    objCmd.CommandText = "p_reports.get_po_rpt";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("callname", OracleDbType.Varchar2).Value = "details";
                    objCmd.Parameters.Add("p_cunit_id", OracleDbType.Varchar2).Value = CompanyUnit;
                    objCmd.Parameters.Add("p_itm_grp", OracleDbType.Varchar2).Value = ItemGroup;
                    objCmd.Parameters.Add("p_frm_dt", OracleDbType.Varchar2).Value = FromDate;
                    objCmd.Parameters.Add("p_to_dt", OracleDbType.Varchar2).Value = ToDate;
                    objCmd.Parameters.Add("p_supp_id", OracleDbType.Varchar2).Value = Supplier;
                    objCmd.Parameters.Add("p_user", OracleDbType.Varchar2).Value = "";
                    objCmd.Parameters.Add("p_user_id", OracleDbType.Varchar2).Value = "";
                }

                DataTable dt = classDt.GetSCMData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSPurchaseOrderReport", dt);
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
            }
        }
    }
}