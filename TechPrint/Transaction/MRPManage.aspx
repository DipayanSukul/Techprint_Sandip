<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="MRPManage.aspx.cs" Inherits="TechPrint.Transaction.MRPManage" %>

<%@ Register TagPrefix="Ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/StyleSheet.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <center>
                <table style="margin: 0 0 0 0;">
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr class="head_tag">
                        <td colspan="4">
                           &nbsp;Paper Rate
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;">
                            <b>PAPER SIZE</b>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPaperSize" runat="server" class="styled-select" BackColor="White" ForeColor="Black" Width="100px">
                               </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="ddlPaperSize_RequiredFieldValidator" runat="server"
                                            ControlToValidate="ddlPaperSize" ErrorMessage="Please select Paper size" InitialValue="0"
                                            ForeColor="#FF3300" SetFocusOnError="True" ValidationGroup="ValidEntry">*</asp:RequiredFieldValidator>
                        </td>
                         <td style="text-align:right;">
                             <b>GSM</b>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGSM" runat="server" class="styled-select" BackColor="White" ForeColor="Black" Width="100px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="ddlGSM_RequiredFieldValidator" runat="server"
                                            ControlToValidate="ddlGSM" ErrorMessage="Please select GSM" InitialValue="0"
                                            ForeColor="#FF3300" SetFocusOnError="True" ValidationGroup="ValidEntry">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                    <td style="text-align:right;">
                        <b>Paper Type</b>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlPaperType" runat="server" class="styled-select" BackColor="White" ForeColor="Black" Width="400px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="ddlPaperType_RequiredFieldValidator" runat="server"
                                ControlToValidate="ddlPaperType" ErrorMessage="Please select Plate Type" InitialValue="0"
                                ForeColor="#FF3300" SetFocusOnError="True" ValidationGroup="ValidEntry">*</asp:RequiredFieldValidator>
                    </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;">
                            <b>Paper Company</b>
                        </td>
                        <td colspan="3">
                             <asp:DropDownList ID="ddlPaperCompany" runat="server" class="styled-select" BackColor="White" ForeColor="Black" Width="400px" AutoPostBack="true" OnSelectedIndexChanged="ddlPaperCompany_SelectedIndexChanged">
                             </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="ddlPaperCompany_RequiredFieldValidator" runat="server"
                                            ControlToValidate="ddlPaperCompany" ErrorMessage="Please select Paper company" InitialValue="0"
                                            ForeColor="#FF3300" SetFocusOnError="True" ValidationGroup="ValidEntry">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;">
                            <b>Paper Rate</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRP" runat="server" CssClass="textbox" Width="100px" autocomplete="off"
                                onFocus="this.select()" BorderStyle="Inset" Style="text-align: right" MaxLength="6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="3">
                            <div style="margin: 0 0 0 0">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                    ShowSummary="False" ValidationGroup="ValidEntry" />
                                <asp:HiddenField ID="hidPrintingParameterID" runat="server" Value="0" />
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="myButton" ValidationGroup="ValidEntry"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="myButton" OnClientClick="return confirm('Are you sure? want to delete');"
                                    OnClick="btnDelete_Click" Visible="false"/>
                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="myButton" OnClick="btnReset_Click" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Select Excel File</b>
                        </td>
                        <td>
                            <asp:FileUpload ID="fup1" runat="server" ViewStateMode="Disabled"/>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><asp:Button ID="btnUploadPaperRate" runat="server" Text="Upload Paper Rate" CssClass="myButton" OnClick="btnUploadPaperRate_Click"/></td>
                    </tr>
                </table>
                <div class="grid-overflow" style="min-height: 100%;">
                    <table id="gridtable" runat="server" border="0">
                        <tr>
                            <td>
                                <asp:GridView ID="gvParamMRP" runat="server" AutoGenerateColumns="False" GridLines="None" DataKeyNames="PrintingParameterID" CssClass="mGrid" 
                                        EmptyDataText="No Record Found" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" PageSize="50" ShowFooter="true"  AllowPaging="true"
                                        OnPageIndexChanging="gvParamMRP_PageIndexChanging" OnRowCommand="gvParamMRP_RowCommand" OnRowEditing="gvParamMRP_RowEditing" >
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="PAPER SIZE" HeaderStyle-HorizontalAlign="Center" DataField="PaperSize"/>
                                        <asp:BoundField HeaderText="GSM" HeaderStyle-HorizontalAlign="Center" DataField="GSM"/>
                                        <asp:BoundField HeaderText="PAPER TYPE" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="250px" DataField="PaperType"/>
                                        <asp:BoundField HeaderText="PAPER COMPANY" HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="150px" DataField="PaperCompany"/>
                                        <asp:BoundField HeaderText="Rate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" DataField="MRP"/>
                                        <asp:TemplateField HeaderText="EDIT" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30px" Visible="false">
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
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
