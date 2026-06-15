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

namespace ABS.Web.Areas.INVENTORY.Reports.ItemPurchaseHistory
{
    public partial class ItemPurchaseHistory : System.Web.UI.Page
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
              
                string FromDate = queryStrings[0];
                string ToDate = queryStrings[1];
                string reportType = queryStrings[2];
                string productID = queryStrings[3];

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(FromDate);
                p2.Name = "ToDate";
                p2.Values.Add(ToDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);

                if (productID == "")
                    productID = null;

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                if (reportType == "partydetails")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/INVENTORY/Reports/ItemPurchaseHistory/ItemPurchaseHistory.rdlc");
                    objCmd.CommandText = "p_po_master_details.rpt_item_purchase_hist";
                }
                
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("p_itm_id", OracleDbType.Varchar2).Value = productID;
                objCmd.Parameters.Add("p_frm_dt", OracleDbType.Varchar2).Value = FromDate;
                objCmd.Parameters.Add("p_to_dt", OracleDbType.Varchar2).Value = ToDate;


                DataTable dt = classDt.GetSCMData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSItemPurchaseHistory", dt);
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