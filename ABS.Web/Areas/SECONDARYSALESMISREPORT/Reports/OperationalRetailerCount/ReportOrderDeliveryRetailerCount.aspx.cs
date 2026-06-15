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

namespace CGSS.Web.Areas.Reports.OrderDeliveryRetailerCount
{
    public partial class ReportOrderDeliveryRetailerCount : System.Web.UI.Page
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

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string national_Id = queryStrings[2];
                string division_Id = queryStrings[3];
                string region_Id = queryStrings[4];
                string zone_Id = queryStrings[5];
                string dist_Id = queryStrings[6];
                string so_Id = queryStrings[7];
                string route_Id = queryStrings[8];

                string brand_Id = queryStrings[9];

                string isBrndGroup = queryStrings[10];
                string brndGroupId = queryStrings[11];

                string reportFilter = queryStrings[12];
                string reportType = queryStrings[13];
                string allordelivery = queryStrings[14];


                Utils utility = new Utils();
                if (utility.IsNotValidActivity(UserId, national_Id))
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }

                string reportMode = "";

                // activity monitoring
                string reportFilteringData = reportFilter + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Operational Retailer Count", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                if (Session["UserID"].ToString() == "08414")
                    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

                if (national_Id == "")
                    national_Id = "0";

                if (division_Id == "")
                    division_Id = "0";

                if (region_Id == "")
                    region_Id = "0";

                if (zone_Id == "")
                    zone_Id = "0";

                if (dist_Id == "")
                    dist_Id = "0";

                if (so_Id == "")
                    so_Id = "0";

                if (route_Id == "")
                    route_Id = "0";

                if (brand_Id == "" || brand_Id == "undefined")
                    brand_Id = "0";

                if (brndGroupId == "")
                    brndGroupId = null;

                UserName = Session["UserFullName"].ToString();

                ReportParameter p1 = new ReportParameter();
                ReportParameter p2 = new ReportParameter();
                ReportParameter p3 = new ReportParameter();
                ReportParameter p4 = new ReportParameter();
                ReportParameter p5 = new ReportParameter();

                p1.Name = "FromDate";
                p1.Values.Add(fromDate);
                p2.Name = "ToDate";
                p2.Values.Add(toDate);
                p3.Name = "UserName";
                p3.Values.Add(UserName);
                p4.Name = "ReportFilter";
                p4.Values.Add(reportFilter);
                p5.Name = "AllOrDelivery";
                p5.Values.Add(allordelivery);



                LocalReport LocalReport = new LocalReport();

                OracleCommand objCmd = new OracleCommand();

                if (allordelivery == "All") // No device log checking
                {
                    objCmd.CommandText = "PKG_ORD_DLV_RET_COUNT_ONE.GET_ORD_DLV_COUNT_NATIONAL";

                    if (reportType == "1") //// summary
                    {
                        //if (division_Id == "0" && dist_Id == "0") // when consumer national selection
                        //{
                        //    reportMode = "DIVISION";
                        //    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountNationalWise.rdlc");
                        //}

                        //if (division_Id != "0" && region_Id == "0" && dist_Id == "0") // when division selection
                        //{
                        //    reportMode = "REGION";
                        //    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountDivisionWise.rdlc");
                        //}

                        //if (region_Id != "0" && zone_Id == "0" && dist_Id == "0") // when region selection
                        //{
                        //    reportMode = "ZONE";
                        //    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountRegionWise.rdlc");
                        //}

                        //if (zone_Id != "0" && dist_Id == "0") // when zone selection
                        //{
                        //    reportMode = "DISTRIBUTOR";
                        //    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountZoneWise.rdlc");
                        //}

                        //if (dist_Id != "0" && route_Id == "0") // when distributor selection
                        //{
                        //    reportMode = "ROUTE";
                        //    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountDistwise.rdlc");
                        //}

                        //if (route_Id != "0") // when route selection
                        //{
                        //    reportMode = "RET";
                        //    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountRouteWise.rdlc");
                        //}

                        //if (so_Id != "0")
                        //{
                        //    reportMode = "SO";
                        //    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountDistwise.rdlc");
                        //}

                        reportMode = "NATIONAL";
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountSummary.rdlc");
                    }
                    if (reportType == "2") //// detail
                    {
                        reportMode = "NATIONAL";
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountDetail.rdlc");
                        //OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["SecondarySales"].ConnectionString);
                        //OracleCommand cmdSet = new OracleCommand();
                        //using (cmdSet.Connection = con)
                        //{                            
                        //    cmdSet.CommandText = "PKG_ORD_DLV_RET_COUNT_NEW.SET_SALES_AREA_RETAILER";
                        //    cmdSet.CommandType = CommandType.StoredProcedure;
                        //    cmdSet.Parameters.Add("p_result", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                        //    cmdSet.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = reportMode;
                        //    cmdSet.Parameters.Add("p_s_date", OracleDbType.Varchar2).Value = fromDate;
                        //    cmdSet.Parameters.Add("p_e_date", OracleDbType.Varchar2).Value = toDate;
                        //    cmdSet.Parameters.Add("p_national_id", OracleDbType.Varchar2).Value = national_Id == "0" ? null : national_Id;
                        //    cmdSet.Parameters.Add("p_division_id", OracleDbType.Varchar2).Value = division_Id == "0" ? null : division_Id;
                        //    cmdSet.Parameters.Add("p_region_id", OracleDbType.Varchar2).Value = region_Id == "0" ? null : region_Id;
                        //    cmdSet.Parameters.Add("p_zone_id", OracleDbType.Varchar2).Value = zone_Id == "0" ? null : zone_Id;
                        //    cmdSet.Parameters.Add("p_distributor_id", OracleDbType.Varchar2).Value = dist_Id == "0" ? null : dist_Id;
                        //    cmdSet.Parameters.Add("p_so_id", OracleDbType.Varchar2).Value = so_Id == "0" ? null : so_Id;
                        //    cmdSet.Parameters.Add("p_route_id", OracleDbType.Varchar2).Value = route_Id == "0" ? null : route_Id;
                        //    var bts = DateTime.Now;
                        //    cmdSet.Connection.Open();
                        //    cmdSet.ExecuteNonQuery();
                        //    cmdSet.Connection.Close();
                        //    string result = cmdSet.Parameters["p_result"].Value.ToString();
                        //    var ets = DateTime.Now;
                        //}
                    }

                    //if (reportType == "3") //// detail
                    //{
                    //    reportMode = "NATIONAL";
                    //    LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountSODetail.rdlc");
                    //    //objCmd.CommandText = "PKG_ORD_DLV_RET_COUNT_NEW.GET_ORD_DLV_COUNT_NATIONAL";
                    //}
                }
                else //// delivery based data. which distributors got device
                {
                    objCmd.CommandText = "PKG_ORD_DLV_RET_COUNT_NEW.GET_ORD_DLV_COUNT_DEL_BASED";

                    if (reportType == "1") //// summary
                    {
                        if (division_Id == "0" && dist_Id == "0") // when consumer national selection
                        {
                            reportMode = "DIVISION";
                            LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountNationalWise.rdlc");
                        }

                        if (division_Id != "0" && region_Id == "0" && dist_Id == "0") // when division selection
                        {
                            reportMode = "REGION";
                            LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountDivisionWise.rdlc");
                        }

                        if (region_Id != "0" && zone_Id == "0" && dist_Id == "0") // when region selection
                        {
                            reportMode = "ZONE";
                            LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountRegionWise.rdlc");
                        }

                        if (zone_Id != "0" && dist_Id == "0") // when zone selection
                        {
                            reportMode = "DISTRIBUTOR";
                            LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountZoneWise.rdlc");
                        }

                        if (dist_Id != "0" && route_Id == "0") // when distributor selection
                        {
                            reportMode = "ROUTE";
                            LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountDistwise.rdlc");
                        }

                        if (route_Id != "0") // when route selection
                        {
                            reportMode = "RET";
                            LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountRouteWise.rdlc");
                        }

                        if (so_Id != "0")
                        {
                            reportMode = "SO";
                            LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountDistwise.rdlc");
                        }
                    }
                    if (reportType == "2") //// detail
                    {
                        reportMode = "NATIONAL";
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountDetail.rdlc");
                    }

                    if (reportType == "3") //// detail
                    {
                        reportMode = "NATIONAL";
                        LocalReport.ReportPath = Server.MapPath("~/Areas/SECONDARYSALESMISREPORT/Reports/OperationalRetailerCount/Report_OperationalRetailerCountSODetail.rdlc");
                    }
                }

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("p_mode", OracleDbType.Varchar2).Value = reportMode;

                objCmd.Parameters.Add("p_s_date", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("p_e_date", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("p_brand_id", OracleDbType.Varchar2).Value = brand_Id == "0" ? null : brand_Id;
                objCmd.Parameters.Add("p_product_id", OracleDbType.Varchar2).Value = null;//product_Id == "0" ? null : product_Id;
                objCmd.Parameters.Add("p_national_id", OracleDbType.Varchar2).Value = national_Id == "0" ? null : national_Id;
                objCmd.Parameters.Add("p_division_id", OracleDbType.Varchar2).Value = division_Id == "0" ? null : division_Id;
                objCmd.Parameters.Add("p_region_id", OracleDbType.Varchar2).Value = region_Id == "0" ? null : region_Id;
                objCmd.Parameters.Add("p_zone_id", OracleDbType.Varchar2).Value = zone_Id == "0" ? null : zone_Id;
                objCmd.Parameters.Add("p_distributor_id", OracleDbType.Varchar2).Value = dist_Id == "0" ? null : dist_Id;
                objCmd.Parameters.Add("p_so_id", OracleDbType.Varchar2).Value = so_Id == "0" ? null : so_Id;
                objCmd.Parameters.Add("p_route_id", OracleDbType.Varchar2).Value = route_Id == "0" ? null : route_Id;
                objCmd.Parameters.Add("p_retailer_id", OracleDbType.Varchar2).Value = null; //retailer_Id == "0" ? null : retailer_Id;

                objCmd.Parameters.Add("isBrndGroup", OracleDbType.Varchar2).Value = isBrndGroup;
                objCmd.Parameters.Add("brndGroupId", OracleDbType.Varchar2).Value = brndGroupId;
                objCmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = UserId;

                var bt = DateTime.Now;
                DataTable dt = classDt.GetSecondaryDataMaxTime(objCmd);
                var et = DateTime.Now;
                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();
                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                LocalReport.SetParameters(p4);
                ReportDataSource datasource = new ReportDataSource("DS_OrderDeliveryRetailerCount", dt);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5 });
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}