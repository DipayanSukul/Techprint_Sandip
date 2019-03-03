<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReport.aspx.cs" Inherits="TechPrint.Report.ViewReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<html>
<body>
    <form id="formPrint" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div style="padding-left: 10px; padding-top: 10px;">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1100px" Height="650px" ShowPrintButton="True">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
