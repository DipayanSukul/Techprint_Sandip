<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="PrintRate.aspx.cs" Inherits="TechPrint.Transaction.PrintRate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/StyleSheet.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <%--<asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <center>
    <table style="margin: 0 0 0 0;">
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr class="head_tag">
            <td colspan="4">
               &nbsp;PRINT RATE CALCULATION
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;&nbsp;CUTTING:
            </td>
            <td>
               &nbsp;&nbsp; <asp:DropDownList ID="ddlCutting" runat="server" class="styled-select" BackColor="White" ForeColor="Black" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlCutting_SelectedIndexChanged">
                   </asp:DropDownList>
                <asp:RequiredFieldValidator ID="ddlCutting_RequiredFieldValidator" runat="server"
                                ControlToValidate="ddlCutting" ErrorMessage="Please select cutting size" InitialValue="0"
                                ForeColor="#FF3300" SetFocusOnError="True" ValidationGroup="ValidEntry">*</asp:RequiredFieldValidator>
            </td>
            <td>
                  <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="ValidEntry" />
               &nbsp;&nbsp; <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="myButton" ValidationGroup="ValidEntry"
                        OnClick="btnSave_Click" />

            </td>
        </tr>
    </table>
    <div class="grid-overflow" style="min-height: 100%;">
        <table id="gridtable" runat="server" border="0">
            <tr>
                <td>
                    <asp:GridView ID="gvParam" runat="server" AutoGenerateColumns="False" GridLines="None" CssClass="mGrid" EmptyDataText="No Record Found"
                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" DataKeyNames="PlateID, ColorID">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Plate" HeaderText="PLATE" />
                            <asp:BoundField DataField="Color" HeaderText="COLOUR" />
                            <asp:TemplateField HeaderText="PLATE 1K COST" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPlateCost1K" runat="server" Width="100px" Height="18px" Text='<%# Eval("PlateCost1K") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="COLOR 1K COST" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtColorCost1K" runat="server" Width="100px" Height="18px" Text='<%# Eval("ColorCost1K") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PLATE 10K COST" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPlateCost10K" runat="server" Width="100px" Height="18px" Text='<%# Eval("PlateCost10K") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="COLOR 10K COST" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtColorCost10K" runat="server" Width="100px" Height="18px" Text='<%# Eval("ColorCost10K") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
                 </center>
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
