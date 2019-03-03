<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="JobCardList.aspx.cs" Inherits="TechPrint.Transaction.JobCardList" %>

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
                    QUOTATION/JOB SHEET ENTRY LIST
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
                                <asp:ListItem Value="1">QUOTATION NO</asp:ListItem>
                                <asp:ListItem Value="2">DATE</asp:ListItem>
                                <asp:ListItem Value="4">QuotationNO</asp:ListItem>
                                <asp:ListItem Value="5">CUSTOMER</asp:ListItem>
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
                            <asp:GridView ID="gvQuotationList" runat="server" DataKeyNames="QuotationID"
                                AllowPaging="True" Visible="true" AutoGenerateColumns="False" GridLines="None"
                                CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                OnPageIndexChanging="gvQuotationList_PageIndexChanging" OnRowCommand="gvQuotationList_RowCommand"
                                OnRowEditing="gvQuotationList_RowEditing" PageSize="100">
                                <Columns>
                                    <asp:TemplateField>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                                 <asp:TemplateField HeaderText="QUOTATION NO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuotationNO" runat="server" Text='<%# Eval("QuotationNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DATE" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="65px">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuotationDate" runat="server" Text='<%# Eval("QuotationDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                                     <asp:TemplateField HeaderText="ITEM" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="160px">
                                <ItemTemplate>
                                    <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                                    <asp:TemplateField HeaderText="CUSTOMER" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GST" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblGSTNo" runat="server" Text='<%# Eval("GSTNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                                     <asp:TemplateField HeaderText="BILL AMT" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalBillingAmount" runat="server" Text='<%# Eval("TotalBillingAmount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                                         <asp:TemplateField HeaderText="DISOCUNT" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("Discount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                             
                                   <asp:TemplateField HeaderText="TAX" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTaxableAmount" runat="server" Text='<%# Eval("TaxableAmount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>   
                                    <asp:TemplateField HeaderText="NET AMT" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountPayable" runat="server" Text='<%# Eval("AmountPayable") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                                     <asp:TemplateField HeaderText="PAID AMT" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountPaid" runat="server" Text='<%# Eval("AmountPaid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                                    <asp:TemplateField HeaderText="DUE AMT" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblOutstandingAmount" runat="server" Text='<%# Eval("OutstandingAmount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                                    
                                    <asp:TemplateField HeaderText="EDIT" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnedit" runat="server" ImageUrl="~/Image/edit.png" Height="14"
                                                ToolTip="Edit Note" Width="18" CommandName="Edit" CommandArgument='<%#Eval("QuotationID")%>'
                                                CausesValidation="false" />
                                             <asp:ImageButton ID="ibtnDownload" runat="server" ImageUrl="~/Image/Download.png"
                                                Height="14" ToolTip="Download Bill" Width="18" CausesValidation="false" CommandName="Download"
                                                CommandArgument='<%#Eval("QuotationID")%>' />
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
