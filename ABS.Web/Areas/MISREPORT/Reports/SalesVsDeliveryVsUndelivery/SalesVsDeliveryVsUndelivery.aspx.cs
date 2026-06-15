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

namespace ABS.Web.Areas.MISREPORT.Reports.SalesVsDeliveryVsUndelivery
{
    public partial class SalesVsDeliveryVsUndelivery : System.Web.UI.Page
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

              //  commonQueryString = startDate + ',' + endDate + ',' + nationalId + ','
              //+ divisionId + ',' + regionId + ',' + zoneId + ',' + distributorId + ',' + reportFilter;


                string[] queryStrings = Request["queryString"].ToString().Split(',');
                string fromDate = queryStrings[0];
                string toDate = queryStrings[1];
                string nationalId = queryStrings[2];
                string divisionId = queryStrings[3];
                string regionId = queryStrings[4];
                string zoneId = queryStrings[5];
                string distributorId = queryStrings[6];
                string reportFilter = queryStrings[7];
                string brandId = queryStrings[8];
                string productId = queryStrings[9];
                string reportType = queryStrings[10];

                // activity monitoring
                string reportFilteringData = reportFilter + "-" + brandId + "-" + productId + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Sales VS Delivery VS Undelivery", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                if (Session["UserID"].ToString() == "08414")
                    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

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

                if(reportType == "brandwise")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesVsDeliveryVsUndelivery/SalesVsDeliveryVsUndeliveryReport_Brand.rdlc");
                }
                else if (reportType == "brandwisedepo")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesVsDeliveryVsUndelivery/SalesVsDeliveryVsUndeliveryReport_Brand_Depo.rdlc");
                }
                else if (reportType == "skuwisefree")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesVsDeliveryVsUndelivery/SalesVsDeliveryVsUndeliverySKUWiseFree.rdlc");
                }
                else if (reportType == "brandwisefree")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesVsDeliveryVsUndelivery/SalesVsDeliveryVsUndeliveryBrandWiseFree.rdlc");
                }
                else if (reportType == "skuwisedepo")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesVsDeliveryVsUndelivery/SalesVsDeliveryVsUndeliveryReport_Depo.rdlc");
                }
                else
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/SalesVsDeliveryVsUndelivery/SalesVsDeliveryVsUndeliveryReport.rdlc");
                }

                if (nationalId == "")
                    nationalId = null;

                if (divisionId == "")
                    divisionId = null;

                if (regionId == "")
                    regionId = null;

                if (zoneId == "")
                    zoneId = null;

                if (distributorId == "")
                    distributorId = null;

                if (brandId == "")
                    brandId = null;

                if (productId == "")
                    productId = null;


                DataTable dtAll = new DataTable();

                /////////////////////// FREE SECTION ////////////////////////////////////

                if (reportType == "skuwisefree" || reportType == "brandwisefree")
                {
                    if (nationalId == null || nationalId == "undefined" || nationalId == "") // without any national selection
                    {
                        OracleCommand objCmd = new OracleCommand();
                        objCmd.CommandText = "PKG_RPT_SAL_DEL_UDEL_FREE.SALES_FA";
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtSales = classDt.GetData(objCmd);


                        OracleCommand objCmdDel = new OracleCommand();
                        objCmdDel.CommandText = "PKG_RPT_SAL_DEL_UDEL_FREE.DELIVERY_FA";
                        objCmdDel.CommandType = CommandType.StoredProcedure;
                        objCmdDel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdDel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdDel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdDel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdDel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdDel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdDel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdDel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdDel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdDel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtDelivery = classDt.GetData(objCmdDel);


                        OracleCommand objCmdUndel = new OracleCommand();
                        objCmdUndel.CommandText = "PKG_RPT_SAL_DEL_UDEL_FREE.UNDELIVERY_FA";
                        objCmdUndel.CommandType = CommandType.StoredProcedure;
                        objCmdUndel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdUndel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdUndel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdUndel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdUndel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdUndel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdUndel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdUndel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdUndel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdUndel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtUndelivery = classDt.GetData(objCmdUndel);


                        dtAll = dtSales.Copy();
                        dtAll.Merge(dtDelivery);
                        dtAll.Merge(dtUndelivery);
                    }
                    else // for national selection
                    {
                        OracleCommand objCmd = new OracleCommand();
                        objCmd.CommandText = "PKG_RPT_SAL_DEL_UDEL_AREA_FREE.SALES_AREA";
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtSales = classDt.GetData(objCmd);


                        OracleCommand objCmdDel = new OracleCommand();
                        objCmdDel.CommandText = "PKG_RPT_SAL_DEL_UDEL_AREA_FREE.DELIVERY_AREA";
                        objCmdDel.CommandType = CommandType.StoredProcedure;
                        objCmdDel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdDel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdDel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdDel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdDel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdDel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdDel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdDel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdDel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdDel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtDelivery = classDt.GetData(objCmdDel);


                        OracleCommand objCmdUndel = new OracleCommand();
                        objCmdUndel.CommandText = "PKG_RPT_SAL_DEL_UDEL_AREA_FREE.UNDELIVERY_AREA";
                        objCmdUndel.CommandType = CommandType.StoredProcedure;
                        objCmdUndel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdUndel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdUndel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdUndel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdUndel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdUndel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdUndel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdUndel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdUndel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdUndel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtUndelivery = classDt.GetData(objCmdUndel);


                        dtAll = dtSales.Copy();
                        dtAll.Merge(dtDelivery);
                        dtAll.Merge(dtUndelivery);
                    }
                }

                ////////////////////////////////////////// SALES SECTION REPORTS /////////////// WITHOUT FREE ///////////////////////////////
                else if (reportType != "brandwisedepo" && reportType != "skuwisedepo")
                {
                    if (nationalId == null || nationalId == "undefined" || nationalId == "") // without any national selection
                    {
                        OracleCommand objCmd = new OracleCommand();
                        objCmd.CommandText = "PKG_REPORT_SAL_DEL_UNDEL.SALES_FA";
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtSales = classDt.GetData(objCmd);


                        OracleCommand objCmdDel = new OracleCommand();
                        objCmdDel.CommandText = "PKG_REPORT_SAL_DEL_UNDEL.DELIVERY_FA";
                        objCmdDel.CommandType = CommandType.StoredProcedure;
                        objCmdDel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdDel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdDel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdDel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdDel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdDel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdDel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdDel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdDel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdDel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtDelivery = classDt.GetData(objCmdDel);


                        OracleCommand objCmdUndel = new OracleCommand();
                        objCmdUndel.CommandText = "PKG_REPORT_SAL_DEL_UNDEL.UNDELIVERY_FA";
                        objCmdUndel.CommandType = CommandType.StoredProcedure;
                        objCmdUndel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdUndel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdUndel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdUndel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdUndel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdUndel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdUndel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdUndel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdUndel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdUndel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtUndelivery = classDt.GetData(objCmdUndel);


                        dtAll = dtSales.Copy();
                        dtAll.Merge(dtDelivery);
                        dtAll.Merge(dtUndelivery);
                    }
                    else // for national selection
                    {
                        OracleCommand objCmd = new OracleCommand();
                        objCmd.CommandText = "PKG_REPORT_SAL_DEL_UNDEL_AREA.SALES_AREA";
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtSales = classDt.GetData(objCmd);


                        OracleCommand objCmdDel = new OracleCommand();
                        objCmdDel.CommandText = "PKG_REPORT_SAL_DEL_UNDEL_AREA.DELIVERY_AREA";
                        objCmdDel.CommandType = CommandType.StoredProcedure;
                        objCmdDel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdDel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdDel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdDel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdDel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdDel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdDel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdDel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdDel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdDel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtDelivery = classDt.GetData(objCmdDel);


                        OracleCommand objCmdUndel = new OracleCommand();
                        objCmdUndel.CommandText = "PKG_REPORT_SAL_DEL_UNDEL_AREA.UNDELIVERY_AREA";
                        objCmdUndel.CommandType = CommandType.StoredProcedure;
                        objCmdUndel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdUndel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdUndel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdUndel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdUndel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdUndel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdUndel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdUndel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdUndel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdUndel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtUndelivery = classDt.GetData(objCmdUndel);


                        dtAll = dtSales.Copy();
                        dtAll.Merge(dtDelivery);
                        dtAll.Merge(dtUndelivery);
                    }
                }
                ///////////////////////////////////////// DEPO SECTION REPORTS //////////////////////////////////////////////////////////////////
                else
                {
                    if (nationalId == null || nationalId == "undefined" || nationalId == "") // without any national selection
                    {
                        OracleCommand objCmd = new OracleCommand();
                        objCmd.CommandText = "PKG_REPORT_SAL_DEL_UNDEL_DEPO.SALES_FA";
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtSales = classDt.GetData(objCmd);


                        OracleCommand objCmdDel = new OracleCommand();
                        objCmdDel.CommandText = "PKG_REPORT_SAL_DEL_UNDEL_DEPO.DELIVERY_FA";
                        objCmdDel.CommandType = CommandType.StoredProcedure;
                        objCmdDel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdDel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdDel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdDel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdDel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdDel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdDel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdDel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdDel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdDel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtDelivery = classDt.GetData(objCmdDel);


                        OracleCommand objCmdUndel = new OracleCommand();
                        objCmdUndel.CommandText = "PKG_REPORT_SAL_DEL_UNDEL_DEPO.UNDELIVERY_FA";
                        objCmdUndel.CommandType = CommandType.StoredProcedure;
                        objCmdUndel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdUndel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdUndel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdUndel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdUndel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdUndel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdUndel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdUndel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdUndel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdUndel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtUndelivery = classDt.GetData(objCmdUndel);


                        dtAll = dtSales.Copy();
                        dtAll.Merge(dtDelivery);
                        dtAll.Merge(dtUndelivery);
                    }
                    else // for national selection
                    {
                        OracleCommand objCmd = new OracleCommand();
                        objCmd.CommandText = "PKG_REPORT_SAL_DEL_UNDEL_DEPO_AREA.SALES_AREA";
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmd.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmd.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmd.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmd.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmd.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmd.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmd.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmd.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtSales = classDt.GetData(objCmd);


                        OracleCommand objCmdDel = new OracleCommand();
                        objCmdDel.CommandText = "PKG_REPORT_SAL_DEL_UNDEL_DEPO_AREA.DELIVERY_AREA";
                        objCmdDel.CommandType = CommandType.StoredProcedure;
                        objCmdDel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdDel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdDel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdDel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdDel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdDel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdDel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdDel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdDel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdDel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtDelivery = classDt.GetData(objCmdDel);


                        OracleCommand objCmdUndel = new OracleCommand();
                        objCmdUndel.CommandText = "PKG_REPORT_SAL_DEL_UNDEL_DEPO_AREA.UNDELIVERY_AREA";
                        objCmdUndel.CommandType = CommandType.StoredProcedure;
                        objCmdUndel.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        objCmdUndel.Parameters.Add("fromDate", OracleDbType.Varchar2).Value = fromDate;
                        objCmdUndel.Parameters.Add("toDate", OracleDbType.Varchar2).Value = toDate;
                        objCmdUndel.Parameters.Add("nationalId", OracleDbType.Varchar2).Value = nationalId;
                        objCmdUndel.Parameters.Add("divisionId", OracleDbType.Varchar2).Value = divisionId;
                        objCmdUndel.Parameters.Add("regionId", OracleDbType.Varchar2).Value = regionId;
                        objCmdUndel.Parameters.Add("zoneId", OracleDbType.Varchar2).Value = zoneId;
                        objCmdUndel.Parameters.Add("distributorId", OracleDbType.Varchar2).Value = distributorId;
                        objCmdUndel.Parameters.Add("brandId", OracleDbType.Varchar2).Value = brandId;
                        objCmdUndel.Parameters.Add("productId", OracleDbType.Varchar2).Value = productId;
                        DataTable dtUndelivery = classDt.GetData(objCmdUndel);


                        dtAll = dtSales.Copy();
                        dtAll.Merge(dtDelivery);
                        dtAll.Merge(dtUndelivery);
                    }
                }

               

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                LocalReport.SetParameters(p4);

                ReportDataSource datasource = new ReportDataSource("SalesVsDeliveryVsUndelivery", dtAll);

                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                reportActivityMonitoring.RequestEnd(autoId);
            }
        }
    }
}