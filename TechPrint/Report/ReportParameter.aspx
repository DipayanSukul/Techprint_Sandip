<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="ReportParameter.aspx.cs" Inherits="TechPrint.Report.ReportParameter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Put here date & other parameter for different reports</h2>
    <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="myButton" OnClick="btnPrint_Click" />
</asp:Content>
