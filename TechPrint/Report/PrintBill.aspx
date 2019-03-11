<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="PrintBill.aspx.cs" Inherits="TechPrint.Report.PrintBill" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Print Bill</title>
</head>
<body>
    <form id="formPrint" runat="server">
        <div id="dvContents">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" ShowToolBar="false">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
