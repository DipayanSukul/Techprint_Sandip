<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="OutstandingReport.aspx.cs" Inherits="TechPrint.Report.OutstandingReport" %>

<%@ Register TagPrefix="Ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                            <asp:Label ID="lblQuotationID" runat="server" BackColor="White" ForeColor="White" Text="lblQuotationID"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head_tag">
                        <td colspan="4">
                          &nbsp; CUSTOMER WISE PAYMENT SUMMERY REPORT
                        </td>
                    </tr>
                    <tr>
                        <td>
                           From date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtfromDate" runat="server" CssClass="textbox" Width="75px" onFocus="this.select()"
                                autocomplete="off" BackColor="White" ForeColor="Black"></asp:TextBox>
                            <Ajax:CalendarExtender ID="txtDate_CalendarExtender" runat="server" PopupButtonID="btnFromDate"
                                TargetControlID="txtfromDate"
                                Format="dd/MM/yyyy">
                            </Ajax:CalendarExtender>
                            <asp:ImageButton ID="btnFromDate" runat="server" ImageUrl="~/Image/Calendar_scheduleHS.png"
                                Height="16px" />
                        </td>
                        <td>
                          To date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="textbox" Width="75px" onFocus="this.select()"
                                autocomplete="off" BackColor="White" ForeColor="Black"></asp:TextBox>
                            <Ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="btnToDate"
                                TargetControlID="txtToDate"
                                Format="dd/MM/yyyy">
                            </Ajax:CalendarExtender>
                            <asp:ImageButton ID="btnToDate" runat="server" ImageUrl="~/Image/Calendar_scheduleHS.png"
                                Height="16px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Customer:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlClient" runat="server" class="styled-select" BackColor="White"
                                ForeColor="Black" Width="300px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="3">
                    <asp:Button ID="btnGetReport" runat="server" Text="Get Report" CssClass="myButton"
                        OnClick="btnGetReport_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="myButton" OnClick="btnReset_Click" /> 
                        </td>
                    </tr>
                </table>
                <div style="min-height: 100%; overflow: auto">
                    <asp:GridView ID="gvReport" runat="server" Visible="true" AutoGenerateColumns="False" DataMember="QuotationID"
                        GridLines="None" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        Width="65%">
                        <Columns>
                            <asp:BoundField DataField="QuotationNO" HeaderStyle-HorizontalAlign="Left"
                                HeaderText="QUOTATION NO" HeaderStyle-Width="200px" />
                            <asp:BoundField DataField="QuotationDate" HeaderStyle-HorizontalAlign="Left"
                                DataFormatString="{0:dd/MM/yyyy}" HeaderText="DATE" HeaderStyle-Width="50px" />
                            <asp:BoundField DataField="CustomerName" HeaderStyle-HorizontalAlign="Left" HeaderText="CUSTOMER"
                                HeaderStyle-Width="450px" />
                            <asp:BoundField DataField="TotalBillingAmount" HeaderStyle-HorizontalAlign="Right"
                                HeaderText="BILL AMT" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="AmountPayable" HeaderStyle-HorizontalAlign="Right"
                                HeaderText="NET AMT" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="AmountPaid" HeaderStyle-HorizontalAlign="Right"
                                HeaderText="PAID AMT" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="OutstandingAmount" HeaderStyle-HorizontalAlign="Right"
                                HeaderText="DUE AMT" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Right" />
                        </Columns>
                    </asp:GridView>
                </div>
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
