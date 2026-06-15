using ABS.Web.Utility;
using CTGroup.Utility;
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

namespace ABS.Web.Areas.SECONDARYSALESMISREPORT.Reports.ProductAudit
{
    public partial class ProductAudit : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
        protected void Page_Load(object sender, EventArgs e)
        {
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

                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string nationalId = queryStrings[2];
                string division_Id = queryStrings[3];
                string region_Id = queryStrings[4];
                string zone_Id = queryStrings[5];
                string dist_Id = queryStrings[6];
                string reportFilter = queryStrings[7];
                string brand_Id = queryStrings[8];
                string product_Id = queryStrings[9];
                string reporttype = queryStrings[10];
                string route_Id = queryStrings[11];

                Utils utility = new Utils();
                if (utility.IsNotValidActivity(UserId, nationalId))
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }

                string so_Id = "0";//Request.QueryString["so_Id"];   


                // activity monitoring
                string reportFilteringData = reportFilter + "-" + brand_Id + "-" + product_Id + "-" + reporttype
                                             + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Retailer Proudct Audit", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

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

                if (route_Id == "" || route_Id == "null" || brand_Id == "undefined")
                    route_Id = "0";

                if (brand_Id == "" || brand_Id == "undefined")
                    brand_Id = "0";

                if (product_Id == "" || product_Id == "undefined")
                    product_Id = "0";

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
                p4.Name = "ReportFilter";
                p4.Values.Add(reportFilter);

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                LocalReport LocalReport = new LocalReport();

                OracleCommand objCmd = new OracleCommand();

                if (reporttype == "Summary")
                {
                    objCmd.CommandText = "PKG_REPORT_PRODUCT_AUDIT.GET_AUDIT_DATA_SUMMARY";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductAudit/ProductAuditSummaryReprotFile.rdlc");
                }
                if (reporttype == "Detail")
                {
                    objCmd.CommandText = "PKG_REPORT_PRODUCT_AUDIT.GET_AUDIT_DATA_BY_ROUTE";
                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductAudit/ProductAuditDetailReprotFile.rdlc");
                }

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = reporttype;

                objCmd.Parameters.Add("p_s_date", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("p_e_date", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("p_brand_id", OracleDbType.Varchar2).Value = brand_Id == "0" ? null : brand_Id;
                objCmd.Parameters.Add("p_product_id", OracleDbType.Varchar2).Value = product_Id == "0" ? null : product_Id;
                objCmd.Parameters.Add("p_division_id", OracleDbType.Varchar2).Value = division_Id == "0" ? null : division_Id;
                objCmd.Parameters.Add("p_region_id", OracleDbType.Varchar2).Value = region_Id == "0" ? null : region_Id;
                objCmd.Parameters.Add("p_zone_id", OracleDbType.Varchar2).Value = zone_Id == "0" ? null : zone_Id;
                objCmd.Parameters.Add("p_distributor_id", OracleDbType.Varchar2).Value = dist_Id == "0" ? null : dist_Id;
                objCmd.Parameters.Add("p_so_id", OracleDbType.Varchar2).Value = null;//so_Id == "0" ? null : so_Id;
                objCmd.Parameters.Add("p_route_id", OracleDbType.Varchar2).Value = route_Id == "0" ? null : route_Id;
                objCmd.Parameters.Add("p_retailer_id", OracleDbType.Varchar2).Value = null; //retailer_Id == "0" ? null : retailer_Id;


                DataTable dt = classDt.GetSecondaryData(objCmd);



                //string expression = " 'SBRND_NAME != '" + "" + "";

                //DataRow[] result = dt.Select(expression);

                //DataRow[] result = dt.Select("'SBRND_NAME != '" + "''");


                // get the not audited distributor list
                //DataTable audited = new DataTable();
                //audited = dt.Clone();

                //DataTable notAudited = new DataTable();
                //notAudited = dt.Clone();


                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (dr.ItemArray[2] != System.DBNull.Value)
                //    {
                //        audited.Rows.Add(dr.ItemArray);
                //    }
                //}

                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (dr.ItemArray[2] is DBNull)
                //    {
                //        notAudited.ImportRow(dr);
                //    }
                //}


                ReportDataSource dataSourceAudited = new ReportDataSource("DSProductAudit", dt);
               // ReportDataSource datasourceNotAudited = new ReportDataSource("NotAuditedDistributorList", notAudited);


                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();
                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);


                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(dataSourceAudited);
                //ReportViewer1.LocalReport.DataSources.Add(datasourceNotAudited);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}


//using ABS.Web.Utility;
//using CTGroup.Utility.Common;
//using Microsoft.Reporting.WebForms;
//using Oracle.ManagedDataAccess.Client;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//namespace ABS.Web.Areas.SECONDARYSALESMISREPORT.Reports.ProductAudit
//{
//    public partial class ProductAudit : System.Web.UI.Page
//    {
//        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!IsPostBack)
//            {
//                string UserName = "";

//                if (Session["UserFullName"] == null || Session["UserFullName"] == "undefined")
//                {
//                    Session.Abandon(); // Does nothing
//                    Session.Clear();   // Removes the data contained in the session
//                                       //Response.Redirect("");
//                                       //Server.Transfer("http://localhost:15812/Account/Login");
//                                       //Response.Redirect("http://localhost:15812/Account/Login");
//                    Response.Redirect("~/Account/Login");
//                }

//                UserName = Session["UserFullName"].ToString();

//                string[] queryStrings = Request["queryString"].ToString().Split(',');
//                string fromDate = queryStrings[0];
//                string toDate = queryStrings[1];
//                string nationalId = queryStrings[2];
//                string division_Id = queryStrings[3];
//                string region_Id = queryStrings[4];
//                string zone_Id = queryStrings[5];
//                string dist_Id = queryStrings[6];
//                string reportFilter = queryStrings[7];
//                string brand_Id = queryStrings[8];
//                string product_Id = queryStrings[9];
//                string reporttype = queryStrings[10];
//                string route_Id = queryStrings[11];

//                string so_Id = "0";//Request.QueryString["so_Id"];   


//                // activity monitoring
//                string reportFilteringData = reportFilter + "-" + brand_Id + "-" + product_Id + "-" + reporttype
//                                             + "-" + fromDate + "-" + toDate;
//                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
//                string autoId = reportActivityMonitoring.RequestStart("Retailer Proudct Audit", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

//                if (nationalId == "")
//                    nationalId = "0";

//                if (division_Id == "")
//                    division_Id = "0";

//                if (region_Id == "")
//                    region_Id = "0";

//                if (zone_Id == "")
//                    zone_Id = "0";

//                if (dist_Id == "")
//                    dist_Id = "0";

//                if (route_Id == "" || route_Id == "null" || brand_Id == "undefined")
//                    route_Id = "0";

//                if (brand_Id == "" || brand_Id == "undefined")
//                    brand_Id = "0";

//                if (product_Id == "" || product_Id == "undefined")
//                    product_Id = "0";

//                ReportParameter p1 = new ReportParameter();
//                ReportParameter p2 = new ReportParameter();
//                ReportParameter p3 = new ReportParameter();
//                ReportParameter p4 = new ReportParameter();

//                p1.Name = "FromDate";
//                p1.Values.Add(fromDate);
//                p2.Name = "ToDate";
//                p2.Values.Add(toDate);
//                p3.Name = "UserName";
//                p3.Values.Add(UserName);
//                p4.Name = "ReportFilter";
//                p4.Values.Add(reportFilter);

//                ReportViewer1.ProcessingMode = ProcessingMode.Local;
//                LocalReport LocalReport = new LocalReport();

//                OracleCommand objCmd = new OracleCommand();

//                if (reporttype == "Summary")
//                {
//                    objCmd.CommandText = "PKG_REPORT_PRODUCT_AUDIT.GET_AUDIT_DATA_SUMMARY";
//                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductAudit/ProductAuditSummaryReprotFile.rdlc");
//                }
//                if (reporttype == "Detail")
//                {
//                    objCmd.CommandText = "PKG_REPORT_PRODUCT_AUDIT.GET_AUDIT_DATA_BY_ROUTE";
//                    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/ProductAudit/ProductAuditDetailReprotFile.rdlc");
//                }

//                objCmd.CommandType = CommandType.StoredProcedure;
//                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
//                objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = reporttype;

//                objCmd.Parameters.Add("p_s_date", OracleDbType.Varchar2).Value = fromDate;
//                objCmd.Parameters.Add("p_e_date", OracleDbType.Varchar2).Value = toDate;
//                objCmd.Parameters.Add("p_brand_id", OracleDbType.Varchar2).Value = brand_Id == "0" ? null : brand_Id;
//                objCmd.Parameters.Add("p_product_id", OracleDbType.Varchar2).Value = product_Id == "0" ? null : product_Id;
//                objCmd.Parameters.Add("p_division_id", OracleDbType.Varchar2).Value = division_Id == "0" ? null : division_Id;
//                objCmd.Parameters.Add("p_region_id", OracleDbType.Varchar2).Value = region_Id == "0" ? null : region_Id;
//                objCmd.Parameters.Add("p_zone_id", OracleDbType.Varchar2).Value = zone_Id == "0" ? null : zone_Id;
//                objCmd.Parameters.Add("p_distributor_id", OracleDbType.Varchar2).Value = dist_Id == "0" ? null : dist_Id;
//                objCmd.Parameters.Add("p_so_id", OracleDbType.Varchar2).Value = null;//so_Id == "0" ? null : so_Id;
//                objCmd.Parameters.Add("p_route_id", OracleDbType.Varchar2).Value = route_Id == "0" ? null : route_Id;
//                objCmd.Parameters.Add("p_retailer_id", OracleDbType.Varchar2).Value = null; //retailer_Id == "0" ? null : retailer_Id;


//                DataTable dt = classDt.GetSecondaryData(objCmd);



//                //string expression = " 'SBRND_NAME != '" + "" + "";

//                //DataRow[] result = dt.Select(expression);

//                //DataRow[] result = dt.Select("'SBRND_NAME != '" + "''");


//                // get the not audited distributor list
//                DataTable audited = new DataTable();
//                audited = dt.Clone();

//                DataTable notAudited = new DataTable();
//                notAudited = dt.Clone();


//                foreach (DataRow dr in dt.Rows)
//                {
//                    if (dr.ItemArray[2] != System.DBNull.Value)
//                    {
//                        audited.Rows.Add(dr.ItemArray);
//                    }
//                }

//                foreach (DataRow dr in dt.Rows)
//                {
//                    if(dr.ItemArray[2] is DBNull)
//                    {
//                        notAudited.ImportRow(dr);
//                    }
//                }


//                ReportDataSource dataSourceAudited = new ReportDataSource("DSProductAudit", audited);
//                ReportDataSource datasourceNotAudited = new ReportDataSource("NotAuditedDistributorList", notAudited);


//                LocalReport.EnableHyperlinks = true;
//                LocalReport.DataSources.Clear();
//                LocalReport.SetParameters(p1);
//                LocalReport.SetParameters(p2);
//                LocalReport.SetParameters(p3);


//                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
//                ReportViewer1.LocalReport.DataSources.Add(dataSourceAudited);
//                ReportViewer1.LocalReport.DataSources.Add(datasourceNotAudited);
//                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
//                reportActivityMonitoring.RequestEnd(autoId);
//            }
//        }
//    }
//}