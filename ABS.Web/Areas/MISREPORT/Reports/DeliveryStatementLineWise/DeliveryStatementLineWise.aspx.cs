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

namespace ABS.Web.Areas.MISREPORT.Reports.DeliveryStatementLineWise
{
    public partial class DeliveryStatementLineWise : System.Web.UI.Page
    {
        ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string UserName = "";
                //  string UserId = Session["UserID"].ToString();

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
                string salesLine = queryStrings[2];
                string salesCompany = queryStrings[3];
                string locationId = queryStrings[4];
                string brandId = queryStrings[5];
                string productId = queryStrings[6];
                string salesCustomerId = queryStrings[7];
                string reportType = queryStrings[8];
                string reportType0 = queryStrings[9];
                string reportType1 = queryStrings[10];

                // activity monitoring
                string reportFilteringData = fromDate + "-" + toDate + "-" + salesLine + "-" + salesCompany + "-" + locationId + "-" + brandId + "-" + productId + "-" + salesCustomerId + "-" + reportType;

                ReportActivityMonitoring reportActivityMonitoring = new ReportActivityMonitoring();
                string autoId = reportActivityMonitoring.RequestStart("Sales Statment", reportFilteringData, Session["UserID"].ToString(), Session["UserFullName"].ToString());

                //if (Session["UserID"].ToString() == "08414") // rashed sir logic, stopped now, as team member increased
                //    fromDate = reportActivityMonitoring.GetReportDateValidation(fromDate);

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

                if (string.IsNullOrEmpty(locationId))
                    locationId = null;
                if (string.IsNullOrEmpty(brandId))
                    brandId = null;
                if (string.IsNullOrEmpty(productId))
                    productId = null;
                if (string.IsNullOrEmpty(salesCustomerId))
                    salesCustomerId = null;

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                LocalReport LocalReport = new LocalReport();
                OracleCommand objCmd = new OracleCommand();

                if (reportType == "LineWise")
                {
                    if (reportType0 == "details")
                    {
                        p4.Values.Add("Line Wise");
                        if (reportType1 == "Requisition")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Group_Wise_Details.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_line";
                        }
                        if (reportType1 == "Gropwise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Customer_Wise_Details.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_line";
                        }
                        if (reportType1 == "ReqWise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Wise_Details.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_line";
                        }

                    }
                    else if (reportType0 == "summary")
                    {
                        p4.Values.Add("Line Wise");

                        if (reportType1 == "Requisition")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Group_Wise_Summary.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_line";
                        }
                        if (reportType1 == "Gropwise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Customer_Wise_Summary.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_line";
                        }
                        if (reportType1 == "ReqWise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Wise_Summary.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_line";
                        }
                    }
                }
                else if (reportType == "CompanyWise")
                {
                    if (reportType0 == "details")
                    {
                        p4.Values.Add("Company Wise");
                        if (reportType1 == "Requisition")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Group_Wise_Details.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_comp";
                        }
                        if (reportType1 == "Gropwise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Customer_Wise_Details.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_comp";
                        }
                        if (reportType1 == "ReqWise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Wise_Details.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_comp";
                        }

                    }
                    else if (reportType0 == "summary")
                    {
                        p4.Values.Add("Company Wise");

                        if (reportType1 == "Requisition")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Group_Wise_Summary.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_comp";
                        }
                        if (reportType1 == "Gropwise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Customer_Wise_Summary.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_comp";
                        }
                        if (reportType1 == "ReqWise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Wise_Summary.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_comp";
                        }
                    }
                }
                else if (reportType == "LocationWise")
                {
                    if (reportType0 == "details")
                    {
                        p4.Values.Add("Location Wise");
                        if (reportType1 == "Requisition")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Group_Wise_Details_Location.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_sgloc";
                        }
                        if (reportType1 == "Gropwise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Customer_Wise_Details_Location.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_sgloc";
                        }
                        if (reportType1 == "ReqWise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Wise_Details_Location.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_sgloc";
                        }

                    }
                    else if (reportType0 == "summary")
                    {
                        p4.Values.Add("Location Wise");

                        if (reportType1 == "Requisition")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Group_Wise_Summary_Location.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_sgloc";
                        }
                        if (reportType1 == "Gropwise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Customer_Wise_Summary_Location.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_sgloc";
                        }
                        if (reportType1 == "ReqWise")
                        {
                            LocalReport.ReportPath = Server.MapPath("~/Areas/MISREPORT/Reports/DeliveryStatementLineWise/Delivery_Statement_Item_Wise_Summary_Location.rdlc");
                            objCmd.CommandText = "Repot_pack_delivery_line.getdata_sgloc";
                        }
                    }
                }


                objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = salesLine;
                objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = salesCompany;
                objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = salesCustomerId;
                objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = brandId;
                objCmd.Parameters.Add("sel5", OracleDbType.Varchar2).Value = productId;
                objCmd.Parameters.Add("sel6", OracleDbType.Varchar2).Value = fromDate;
                objCmd.Parameters.Add("sel7", OracleDbType.Varchar2).Value = toDate;
                objCmd.Parameters.Add("sel8", OracleDbType.Varchar2).Value = locationId;
                objCmd.Parameters.Add("sel9", OracleDbType.Varchar2).Value = null;
                objCmd.Parameters.Add("sel10", OracleDbType.Varchar2).Value = null;

                objCmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = classDt.GetData(objCmd);

                ReportDataSource datasource = new ReportDataSource("DSDeliveryLine", dt);
                ReportViewer1.LocalReport.ReportPath = LocalReport.ReportPath;
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });

                LocalReport.EnableHyperlinks = true;
                LocalReport.DataSources.Clear();

                LocalReport.SetParameters(p1);
                LocalReport.SetParameters(p2);
                LocalReport.SetParameters(p3);
                LocalReport.SetParameters(p4);
                reportActivityMonitoring.RequestEnd(autoId);

            }
        }
    }
}