<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="CustomerMaster.aspx.cs" Inherits="TechPrint.Transaction.CustomerMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="../CSS/StyleSheet.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <center>
    <table style="margin: 0 0 0 0;">
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr class="head_tag">
            <td colspan="4">
               &nbsp;&nbsp; Customer Master
            </td>
        </tr>
        <tr>
            <td>
                Name:
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtName" runat="server" CssClass="textbox" Width="350px" autocomplete="off"
                    onFocus="this.select()" BorderStyle="Inset"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>
                Address
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtAddress" runat="server" CssClass="textbox" Width="350px" TextMode="MultiLine" Height="50" autocomplete="off"
                    onFocus="this.select()" BorderStyle="Inset"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Telephone
            </td>
            <td>
                <asp:TextBox ID="txttelephone" runat="server" Width="150px" CssClass="textbox" autocomplete="off" onFocus="this.select()" BorderStyle="Inset"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>
                Email
            </td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" Width="150px" CssClass="textbox" autocomplete="off" onFocus="this.select()" BorderStyle="Inset"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>
                GST
            </td>
            <td>
                <asp:TextBox ID="txtGSTNo" runat="server" Width="150px" CssClass="textbox" autocomplete="off" onFocus="this.select()" BorderStyle="Inset"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td colspan="3">
                <div style="margin: 0 0 0 0">
                    <asp:Label ID="lblCustomerId" runat="server" Text="MasterId" ForeColor="White"></asp:Label>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="myButton"
                        OnClick="btnSave_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="myButton" OnClientClick="return confirm('Are you sure? want to delete');"
                        OnClick="btnDelete_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="myButton" OnClick="btnReset_Click" />
                </div>
            </td>
        </tr>
    </table>
    <div class="grid-overflow" style="min-height: 100%;">
        <table id="gridtable" runat="server" border="0">
            <tr>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>  <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="myButton" OnClick="btnSearch_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvCustomer" runat="server" DataKeyNames="CustomerID" AllowPaging="True"
                        Visible="true" AutoGenerateColumns="False" GridLines="None" CssClass="mGrid" EmptyDataText="No Record Found"
                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvCustomer_PageIndexChanging"
                        OnRowCommand="gvCustomer_RowCommand" OnRowEditing="gvCustomer_RowEditing" PageSize="25">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Address" HeaderStyle-Width="400px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>' Style="white-space: nowrap;"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Telephone" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTelephone" runat="server" Text='<%# Eval("Telephone") %>' Style="white-space: nowrap;"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>' Style="white-space: nowrap;"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GSTNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblGSTNo" runat="server" Text='<%# Eval("GSTNo") %>' Style="white-space: nowrap;"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EDIT" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnedit" runat="server" ImageUrl="~/Image/edit.png" Height="14"
                                        Width="18" CommandName="Edit" CommandArgument='<%#Eval("CustomerID")%>' CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
                 </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
