<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="PaymentList.aspx.cs" Inherits="TechPrint.Transaction.PaymentList" %>

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
                <br />
                <div class="head_tag">
                    PAYMENT COLLECTION ENTRY LIST
                </div>
                <table id="tblGRNSearch" runat="server" border="0">
                    <tr>
                        <td>
                            Financial Year :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFinancialYear" runat="server" class="styled-select" BackColor="White"
                                ForeColor="Black" AppendDataBoundItems="true" Width="75px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Search By :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Search" runat="server" class="styled-select" BackColor="White"
                                ForeColor="Black" AppendDataBoundItems="true" Width="150px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_Search_SelectedIndexChanged">
                                <asp:ListItem Value="0">- Select -</asp:ListItem>
                                <asp:ListItem Value="1">PAYMENT NO</asp:ListItem>
                                <asp:ListItem Value="2">DATE</asp:ListItem>
                                <asp:ListItem Value="3">CUSTOMER</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchText" runat="server" CssClass="textbox" Width="250px" autocomplete="off"
                                onFocus="this.select()" BorderStyle="Inset"></asp:TextBox>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="textbox" Enabled="false" BackColor="White"
                                Width="100px" ForeColor="Black"></asp:TextBox>
                            <Ajax:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Enabled="True"
                                PopupButtonID="btnCalenderPopup1" TargetControlID="txtFromDate"
                                Format="dd/MM/yyyy">
                            </Ajax:CalendarExtender>
                            <asp:ImageButton ID="btnCalenderPopup1" runat="server" ImageUrl="~/Image/Calendar_scheduleHS.png"
                                Height="16px" />
                            &nbsp;
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="textbox" Enabled="false" BackColor="White"
                                Width="100px" ForeColor="Black"></asp:TextBox>
                            <Ajax:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                PopupButtonID="btnCalenderPopup2" TargetControlID="txtToDate"
                                Format="dd/MM/yyyy">
                            </Ajax:CalendarExtender>
                            <asp:ImageButton ID="btnCalenderPopup2" runat="server" ImageUrl="~/Image/Calendar_scheduleHS.png"
                                Height="16px" />
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" runat="server" Text="Search" CssClass="myButton" OnClick="btn_Search_Click" />
                        </td>
                    </tr>
                </table>
                <table id="gridtable" runat="server" border="0" style="width: 100%;">
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="gvPaymentList" runat="server" DataKeyNames="PaymentID"
                                AllowPaging="True" Visible="true" AutoGenerateColumns="False" GridLines="None"
                                CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                OnPageIndexChanging="gvPaymentList_PageIndexChanging" OnRowCommand="gvPaymentList_RowCommand"
                                OnRowEditing="gvPaymentList_RowEditing" PageSize="100">
                                <Columns>
                                 <asp:TemplateField>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                                       <asp:BoundField DataField="PaymentNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="PAYMENT NO"
                                        HeaderStyle-Width="160px" />
                                    <asp:BoundField DataField="PaymentDate" HeaderStyle-HorizontalAlign="Left"
                                        DataFormatString="{0:dd/MM/yyyy}" HeaderText="DATE" HeaderStyle-Width="65px" />
                                    <asp:BoundField DataField="CustomerName" HeaderStyle-HorizontalAlign="Left" HeaderText="CUSTOMER"
                                        HeaderStyle-Width="300px" />
                                    <asp:BoundField DataField="PaidAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderText="PAID" 
                                        HeaderStyle-Width="100px" />
                                    <asp:BoundField DataField="PMode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="MODE"
                                        HeaderStyle-Width="130px" />
                                    <asp:BoundField DataField="PaymentDetail" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="PAYMENT DETAIL"
                                        HeaderStyle-Width="350px" />
                                    <asp:TemplateField HeaderText="EDIT" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnedit" runat="server" ImageUrl="~/Image/edit.png" Height="14"
                                                ToolTip="Edit Note" Width="18" CommandName="Edit" CommandArgument='<%#Eval("PaymentID")%>'
                                                CausesValidation="false" />
                                             <asp:ImageButton ID="ibtnDownload" runat="server" ImageUrl="~/Image/Download.png"
                                                Height="14" ToolTip="Print Payment" Width="18" CausesValidation="false" CommandName="Download"
                                                CommandArgument='<%#Eval("PaymentID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
