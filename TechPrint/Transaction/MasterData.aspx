<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="MasterData.aspx.cs" Inherits="TechPrint.Transaction.MasterData" %>

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
    <table style="margin: 0 0 0 0;">
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr class="head_tag">
            <td colspan="4">
               &nbsp;&nbsp; <asp:Label ID="lblFormHeader" runat="server" Text="#"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMasterParam" runat="server">MasterParam:</asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtMasterParam" runat="server" CssClass="textbox" Width="350px" autocomplete="off"
                    onFocus="this.select()" BorderStyle="Inset"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>
                <asp:Label ID="lblMasterParamDescription" runat="server">Description:</asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtMasterParamDescription" runat="server" CssClass="textbox" Width="350px" TextMode="MultiLine" Height="50" autocomplete="off"
                    onFocus="this.select()" BorderStyle="Inset"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMRP" runat="server">MRP:</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMRP" runat="server" Width="100px" CssClass="textbox" autocomplete="off" onFocus="this.select()" Style="text-align: right" BorderStyle="Inset"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblParamValue1" runat="server">ParamValue1</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtParamValue1" runat="server" CssClass="textbox" Width="100px" autocomplete="off"
                    onFocus="this.select()" BorderStyle="Inset" Style="text-align: right" MaxLength="11"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lblParamValue2" runat="server">ParamValue2</asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtParamValue2" runat="server" CssClass="textbox" Width="100px" autocomplete="off"
                    onFocus="this.select()" BorderStyle="Inset" Style="text-align: right" MaxLength="11"></asp:TextBox>
            </td>

        </tr>
        <tr>
            <td>
                <asp:Label ID="lblParamValue3" runat="server">ParamValue3</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtParamValue3" runat="server" CssClass="textbox" Width="100px" autocomplete="off"
                    onFocus="this.select()" BorderStyle="Inset" Style="text-align: right" onkeyup="return CBMCalculate();"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td colspan="3">
                <div style="margin: 0 0 0 0">
                    <asp:HiddenField ID="hidPrintingParameterID" runat="server" Value="0"></asp:HiddenField>
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
                    <asp:GridView ID="gvMasterParam" runat="server" DataKeyNames="PrintingParameterID" AllowPaging="True"
                        Visible="true" AutoGenerateColumns="False" GridLines="None" CssClass="mGrid" EmptyDataText="No Record Found"
                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvMasterParam_PageIndexChanging"
                        OnRowCommand="gvMasterParam_RowCommand" OnRowEditing="gvMasterParam_RowEditing" PageSize="25">
                        <Columns>
                            <asp:TemplateField HeaderText="MasterParam" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="300px">
                                <ItemTemplate>
                                    <asp:Label ID="lblMasterParam" runat="server" Text='<%# Eval("PrintingParameterName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ParamValue1" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblParamValue1" runat="server" Text='<%# Eval("ParameterValue1") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ParamValue2" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblParamValue2" runat="server" Text='<%# Eval("ParameterValue2") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ParamValue3" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblParamValue3" runat="server" Text='<%# Eval("ParameterValue3") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MRP" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblMRP" runat="server" Text='<%# Eval("SellPrice") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PrintingParameterDescription" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrintingParameterDescription" runat="server" Text='<%# Eval("PrintingParameterDescription") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EDIT" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnedit" runat="server" ImageUrl="~/Image/edit.png" Height="14"
                                        Width="18" CommandName="Edit" CommandArgument='<%#Eval("PrintingParameterID")%>' CausesValidation="false" />
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
