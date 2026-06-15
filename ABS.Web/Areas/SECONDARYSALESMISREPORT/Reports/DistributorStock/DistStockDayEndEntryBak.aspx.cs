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

namespace ABS.Web.Areas.SECONDARYSALESMISREPORT.Reports.DistributorStock
{
    public partial class DistStockDayEndEntryBak : System.Web.UI.Page
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
                string UserId = Session["UserID"].ToString();

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
                string report_Type = queryStrings[10];

                // activity monitoring
                string reportFilteringData = reportFilter + "-" + brand_Id + "-" + product_Id + "-" + fromDate + "-" + toDate;

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

                //
                if(nationalId == "SNTNLxxxxxxxxxN00001")
                {
                    if (report_Type == "InPcsParty")
                    {
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_ON_TEST_BCT.RPT_GET_DIST_STOCK";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_ON_TEST_JMW.RPT_GET_DIST_STOCK";
                        }
                        else
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_ON_TEST.RPT_GET_DIST_STOCK";
                        }
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/DistributorStock/DistStockInPcsParty.rdlc");
                    }
                    if (report_Type == "InCtnParty")
                    {
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_ON_TEST_BCT.RPT_GET_DIST_STOCK_CTN";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_ON_TEST_JMW.RPT_GET_DIST_STOCK_CTN";
                        }
                        else
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_ON_TEST.RPT_GET_DIST_STOCK_CTN";
                        }
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/DistributorStock/DistStockInCtnParty.rdlc");
                    }
                    if (report_Type == "InMTonParty")
                    {
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_ON_TEST_BCT.RPT_GET_DIST_STOCK_MTN";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_ON_TEST_JMW.RPT_GET_DIST_STOCK_MTN";
                        }
                        else
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_ON_TEST.RPT_GET_DIST_STOCK_MTN";
                        }
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/DistributorStock/DistStockInMtnParty.rdlc");
                    }
                } // REPORT FOR CONSUMER  NATIONAL

                if (nationalId == "SNTNLxxxxxxxxxN00007") // REPORT FOR SUN NATIONAL
                {
                    if (report_Type == "InPcsParty")
                    {
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_SUN_BCT.RPT_GET_DIST_STOCK";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_SUN_JMW.RPT_GET_DIST_STOCK";
                        }
                        else
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_SUN.RPT_GET_DIST_STOCK";
                        }
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/DistributorStock/DistStockInPcsParty.rdlc");
                    }
                    if (report_Type == "InCtnParty")
                    {
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_SUN_BCT.RPT_GET_DIST_STOCK_CTN";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_SUN_JMW.RPT_GET_DIST_STOCK_CTN";
                        }
                        else
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_SUN.RPT_GET_DIST_STOCK_CTN";
                        }
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/DistributorStock/DistStockInCtnParty.rdlc");
                    }
                    if (report_Type == "InMTonParty")
                    {
                        if (UserId == "07686") // tea - Iqbal Chowdhury
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_SUN_BCT.RPT_GET_DIST_STOCK_MTN";
                        }
                        else if (UserId == "06443") // water - Ahmmed Ali
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_SUN_JMW.RPT_GET_DIST_STOCK_MTN";
                        }
                        else
                        {
                            objCmd.CommandText = "PKG_DIST_DELIVERY_SUN.RPT_GET_DIST_STOCK_MTN";
                        }
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/DistributorStock/DistStockInMtnParty.rdlc");
                    }
                }
                //if (report_Type == "Detail")
                //{
                //    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/DistributorStock/StockSummary.rdlc");
                //}



                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = "1";

                objCmd.Parameters.Add("p_s_date", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("p_e_date", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("p_brand_id", OracleDbType.Varchar2).Value = brand_Id == "0" ? null : brand_Id;
                objCmd.Parameters.Add("p_product_id", OracleDbType.Varchar2).Value = product_Id == "0" ? null : product_Id;
                objCmd.Parameters.Add("p_national_id", OracleDbType.Varchar2).Value = nationalId == "0" ? null : nationalId;
                objCmd.Parameters.Add("p_division_id", OracleDbType.Varchar2).Value = division_Id == "0" ? null : division_Id;
                objCmd.Parameters.Add("p_region_id", OracleDbType.Varchar2).Value = region_Id == "0" ? null : region_Id;
                objCmd.Parameters.Add("p_zone_id", OracleDbType.Varchar2).Value = zone_Id == "0" ? null : zone_Id;
                objCmd.Parameters.Add("p_distributor_id", OracleDbType.Varchar2).Value = dist_Id == "0" ? null : dist_Id;

                DataTable dt = classDt.GetSecondaryData(objCmd);

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                LocalReport.SetParameters(p4);


                ReportDataSource datasource = new ReportDataSource("PartyStockBasedOnDailyInput", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
            }
        }
    }
}