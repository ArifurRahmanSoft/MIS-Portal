<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RetailerInfo.aspx.cs" 
    Inherits="ABS.Web.Areas.SECONDARYSALESMISREPORT.Reports.RetailerInfo.RetailerInfo" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" 
    tagprefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="~/Themes/assets/img/CityGroup/city-favicon.ico" type="image/ico">
    <title>Retailer Information</title>
</head>
<body>
    <form id="form1" runat="server" style="width:100%; height:100%;">
        <div>
        </div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="" ClientIDMode="AutoID" 
            HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" 
            InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" 
            PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" 
            PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" 
            SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" 
            ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" 
            ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" 
            ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" 
            ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" 
            ToolBarItemPressedBorderWidth="1px" 
            ToolBarItemPressedHoverBackColor="153, 187, 226" Height="100%" Width="100%" 
            AsyncRendering="False" SizeToReportContent="True">
          
          <LocalReport EnableHyperlinks="True" >
            
           </LocalReport>
        </rsweb:ReportViewer>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
       
    </form>
</body>
</html>

<script type="text/javascript" language ="javascript" >
        // Internet Explorer:
        if (document.all)
            document.onselectstart =
    function () { return false; };

        // Netscape 4:
        if (document.layers) {
            document.captureEvents(Event.MOUSEDOWN);
            document.onmousedown =
    function (evt) { return false; };
        }

        // Netscape 6:
        document.onmousedown = function () { return false; };


</script>