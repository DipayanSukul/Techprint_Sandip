<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="JobCardSummery.aspx.cs" Inherits="TechPrint.Report.JobCardSummery" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register TagPrefix="Ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <center>
                <table>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="head_tag">
                        <td colspan="4">
                          &nbsp;DATE WISE JOB SUMMERY REPORT
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
                <br />
                <rsweb:ReportViewer ID="ReportViewer1" runat="server">
                </rsweb:ReportViewer>
                </center>
</asp:Content>
