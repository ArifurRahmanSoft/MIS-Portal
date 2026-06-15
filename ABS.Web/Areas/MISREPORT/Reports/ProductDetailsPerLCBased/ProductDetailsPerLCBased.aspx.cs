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

namespace ABS.Web.Areas.MISREPORT.Reports.ProductDetailsPerLCBased
{
    public partial class ProductDetailsPerLCBased : System.Web.UI.Page
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

                string productID = queryStrings[0];
                string fromDate = queryStrings[1];
                string toDate = queryStrings[2];

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);
                p2.Name = "ToDate";
                p2.Values.Add(toDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);


                if (productID == "")
                    productID = null;


                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();


                LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/ProductDetailsPerLCBased/ReportProductDetailsPerLCBased.rdlc");
                objCmd.CommandText = "PKG_PRODUCT_DETAILS_PER_LC.PRODUCT_DETAILS_PER_LC_BASED";


                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("fresult", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                objCmd.Parameters.Add("productID", OracleDbType.Varchar2).Value = productID;
                objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;

                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSSProductDetailsPerLCBased", dt);
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