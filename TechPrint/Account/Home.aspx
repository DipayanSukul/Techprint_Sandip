<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TechPrint.Account.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: center; width: 800px;">
        <asp:Label ID="lblTotalBooking" runat="server" Text="Today's Booking" Font-Size="24" ForeColor="Tomato" />
        <br />
        <asp:Label ID="lblTotalCollection" runat="server" Text="Today's Collection" Font-Size="24" ForeColor="Tomato" />
    </div>
</asp:Content>
