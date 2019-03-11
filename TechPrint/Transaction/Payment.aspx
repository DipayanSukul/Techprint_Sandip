<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="TechPrint.Transaction.Payment" %>

<%@ Register TagPrefix="Ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/StyleSheet.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <center>
                <table>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="head_tag">
                        <td colspan="4">
                          &nbsp;PAYMENT COLLECTION
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Payment No:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtPaymentEntryNo" runat="server" CssClass="textbox" Width="150px" BorderStyle="Inset"
                                BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" Enabled="false">
                            </asp:TextBox>
                            Date:
                            <asp:TextBox ID="txtDate" runat="server" CssClass="textbox" Width="75px" onFocus="this.select()"
                                autocomplete="off" BackColor="White" ForeColor="Black"></asp:TextBox>
                            <Ajax:CalendarExtender ID="txtDate_CalendarExtender" runat="server" PopupButtonID="btnPaymentDt"
                                TargetControlID="txtDate"
                                Format="dd/MM/yyyy">
                            </Ajax:CalendarExtender>
                            <asp:ImageButton ID="btnPaymentDt" runat="server" ImageUrl="~/Image/Calendar_scheduleHS.png"
                                Height="16px" />

                    </tr>
                    <tr>
                        <td>
                            Customer:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlClient" runat="server" class="styled-select" BackColor="White" AutoPostBack="true"
                                ForeColor="Black" Width="280px" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Total Due:
                        </td>
                        <td>
                            <asp:TextBox ID="txtOutstanding" runat="server" autocomplete="off" Enabled="false" BackColor="White" BorderStyle="Inset" Style="text-align: right" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="85px">
                            </asp:TextBox>
                        </td>
                         
                    </tr>
                    <tr>
                        <td>
                            Payment Mode:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlPaymentMode" runat="server" class="styled-select" BackColor="White"
                                ForeColor="Black" Width="150px">
                            </asp:DropDownList>
                            &nbsp;
                            Paid
                            <asp:TextBox ID="txtPaidAmount" runat="server" autocomplete="off" BackColor="White" BorderStyle="Inset" Style="text-align: right" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="75px">
                            </asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td>
                           Payment Detail:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtPaymentDetail" runat="server" TextMode="MultiLine" autocomplete="off" BackColor="White" BorderStyle="Inset" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="280px" Height="40px">
                            </asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <div style="margin: 0 0 0 0">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="ValidEntry" />
                    <asp:Label ID="lblPaymentID" runat="server" Text="MasterId" ForeColor="White"></asp:Label>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="myButton" ValidationGroup="ValidEntry"
                        OnClick="btnSave_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="myButton" OnClientClick="return confirm('Are you sure? want to delete this payment entry note.');"
                        OnClick="btnDelete_Click" />
                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="myButton" OnClick="btnView_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="myButton" OnClick="btnReset_Click" />
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
