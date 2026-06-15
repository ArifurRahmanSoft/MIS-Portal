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

namespace ABS.Web.Areas.MISREPORT.Reports.DeliveryStatement
{
    public partial class DeliveryStatementBak : System.Web.UI.Page
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
                string divisionId = queryStrings[3];
                string regionId = queryStrings[4];
                string zoneId = queryStrings[5];
                string distributorId = queryStrings[6];
                string reportFilter = queryStrings[7];
                string brandId = queryStrings[8];
                string productId = queryStrings[9];
                string company = queryStrings[10];
                string locationID = queryStrings[11];
                string reportType = queryStrings[12];
                string isBrndGroup = queryStrings[13];
                string brndGroupId = queryStrings[14];

                Utils utility = new Utils();
                if (utility.IsNotValidActivity(UserId, nationalId))
                {
                    Session.Abandon(); // Does nothing
                    Session.Clear();   // Removes the data contained in the session
                    Response.Redirect("~/Account/Login");
                }

                // activity monitoring
                string reportFilteringData = reportFilter + "-" + brandId + "-" + productId + "-" + company + "-" + locationID + "-" + reportType + "-" + fromDate + "-" + toDate;
                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();

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

                if (locationID == "")
                    locationID = null;

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

                if (brandId == "" || brandId == "undefined")
                    brandId = null;

                if (productId == "" || productId == "undefined")
                    productId = null;

                if (company == "" || company == "undefined")
                    company = null;

                if (locationID == "" || locationID == "undefined")
                    locationID = null;

                if (brndGroupId == "")
                    brndGroupId = null;

                OracleCommand objCmd = new OracleCommand();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport LocalReport = new LocalReport();

                if (reportType == "customer_wise")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatement/ReportDeliveryStatementCustomerWiseDetials.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_BCT.get_challanDetail";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_JMW.get_challanDetail";
                    //}
                    //else
                    //{
                    objCmd.CommandText = "pack_challanDetail_new.get_challanDetail";
                    //}
                }
                if (reportType == "sku_wise")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatement/ReportDeliveryStatementSKUWise.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_BCT.get_SKUWiseDelivery";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_JMW.get_SKUWiseDelivery";
                    //}
                    //else
                    //{
                    if (!string.IsNullOrEmpty(nationalId))
                    {
                        objCmd.CommandText = "pack_challanDetail_new.get_SKUWiseDelivery";
                    }
                    else
                    {
                        objCmd.CommandText = "pack_challanDetail_new.get_SKUWiseDelivery_FA";
                    }
                    //}
                }
                if (reportType == "brand_wise")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatement/ReportDeliveryStatementBrandWise.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_BCT.get_SKUWiseDelivery";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_JMW.get_SKUWiseDelivery";
                    //}
                    //else
                    //{
                    if (!string.IsNullOrEmpty(nationalId))
                    {
                        objCmd.CommandText = "pack_challanDetail_new.get_SKUWiseDelivery";
                    }
                    else
                    {
                        objCmd.CommandText = "pack_challanDetail_new.get_SKUWiseDelivery_FA";
                    }
                    //}
                }

                if (reportType == "brand_wise_location")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatement/ReportDeliveryStatementLocationBrandWise.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_BCT.get_SKUWiseDelivery";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_JMW.get_SKUWiseDelivery";
                    //}
                    //else
                    //{
                    if (!string.IsNullOrEmpty(nationalId))
                    {
                        objCmd.CommandText = "pack_challanDetail_new.get_SKUWiseDelivery_Location";
                    }
                    else
                    {
                        objCmd.CommandText = "pack_challanDetail_new.get_SKUWiseDelivery_Location_FA";
                    }
                    //}
                }
                if (reportType == "summary")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatement/ReportDeliveryStatementAreaSummary.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_BCT.DeliveryAreaWise";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_JMW.DeliveryAreaWise";
                    //}
                    //else
                    //{
                    objCmd.CommandText = "pack_challanDetail_new.DeliveryAreaWise";
                    //}
                }
                if (reportType == "summaryton")
                {
                    //LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatement/ReportDeliveryStatementAreaSummaryTon.rdlc");
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatement/ReportDeliveryStatementAreaMTonSummary.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_BCT.DeliveryAreaWiseMTon";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_JMW.DeliveryAreaWiseMTon";
                    //}
                    //else
                    //{
                    //objCmd.CommandText = "pack_challanDetail.DeliveryAreaWiseMTon";
                    objCmd.CommandText = "pack_challanDetail_new.DeliveryAreaWiseMTon";
                    //}
                }
                if (reportType == "detail")
                {
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatement/ReportDeliveryStatementAreaDetailNew.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_BCT.DeliveryAreaWise";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_JMW.DeliveryAreaWise";
                    //}
                    //else
                    //{
                    objCmd.CommandText = "pack_challanDetail_new.DeliveryAreaWise";
                    //}
                }
                if (reportType == "detailton")
                {
                    //LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatement/ReportDeliveryStatementAreaDetailTon.rdlc");
                    LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatement/ReportDeliveryStatementAreaMTonDetail.rdlc");
                    //if (UserId == "07686") // tea - Iqbal Chowdhury
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_BCT.DeliveryAreaWiseMTon";
                    //}
                    //else if (UserId == "06443") // water - Ahmmed Ali
                    //{
                    //    objCmd.CommandText = "pack_challanDetail_JMW.DeliveryAreaWiseMTon";
                    //}
                    //else
                    //{
                    //objCmd.CommandText = "pack_challanDetail.DeliveryAreaWiseMTon";
                    objCmd.CommandText = "pack_challanDetail_new.DeliveryAreaWiseMTon";
                    //}
                }

                // save previouse // 
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
                objCmd.Parameters.Add("companyId", OracleDbType.Varchar2).Value = company;
                objCmd.Parameters.Add("locationId", OracleDbType.Varchar2).Value = locationID;

                //New Param
                objCmd.Parameters.Add("isBrndGroup", OracleDbType.Varchar2).Value = isBrndGroup;
                objCmd.Parameters.Add("brndGroupId", OracleDbType.Varchar2).Value = brndGroupId;
                objCmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = UserId;
                //New Param

                string autoId = reportActivityMonitoring.RequestStart("Delivery Statement", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());
                DataTable dt = classDt.GetData(objCmd);
                reportActivityMonitoring.RequestEnd(autoId);

                if (reportType == "customer_wise" || reportType == "customer_wise_location")
                {
                    ReportDataSource datasource = new ReportDataSource("DDSDeliveryStatementCustomerWiseDetails", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "sku_wise" || reportType == "brand_wise" || reportType == "brand_wise_location")
                {
                    ReportDataSource datasource = new ReportDataSource("DDSDeliveryStatementCustomerWiseDetails", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "detail")
                {
                    ReportDataSource datasource = new ReportDataSource("DSAreaBrandWise", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "summary")
                {
                    ReportDataSource datasource = new ReportDataSource("DSAreaBrandWise", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "summaryton")
                {
                    //ReportDataSource datasource = new ReportDataSource("DSAreaBrandWiseTon", dt);
                    ReportDataSource datasource = new ReportDataSource("DSAreaBrandWise", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                if (reportType == "detailton")
                {
                    //ReportDataSource datasource = new ReportDataSource("DSAreaBrandWiseTon", dt);
                    ReportDataSource datasource = new ReportDataSource("DSAreaBrandWise", dt);
                    ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });
                }

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                LocalReport.SetParameters(p4);
            }
        }
    }
}