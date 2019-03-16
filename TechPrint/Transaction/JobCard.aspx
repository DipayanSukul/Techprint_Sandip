<%@ Page Title="" Language="C#" MasterPageFile="~/Account/TechPrint.Master" AutoEventWireup="true" CodeBehind="JobCard.aspx.cs" Inherits="TechPrint.Transaction.JobCard" %>

<%@ Register TagPrefix="Ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: #000000;
            filter: alpha(opacity=40);
            opacity: 0.5;
        }

        .ModalWindow {
            border: solid 1px #000000;
            background: #ffffff;
            position: absolute;
            top: -1000px;
        }
    </style>
    <script type="text/javascript">
        function OnSuccessFetch(data) {
            var _txtClientAddress = document.getElementById('<%= txtClientAddress.ClientID %>');
            var _txtClientPhoneNo = document.getElementById('<%= txtClientPhoneNo.ClientID %>');
            var _txtClientEmail = document.getElementById('<%= txtClientEmail.ClientID %>');
            _txtClientAddress.value = data.d[0]["Address"];
            _txtClientPhoneNo.value = data.d[0]["Telephone"];
            _txtClientEmail.value = data.d[0]["Email"];
        };
        function getDecimal(n) {
            return (n - Math.floor(n));
        }
        function GSTCalculation() {
            var TotalAmount = 0;
            var GSTWOAmount = 0;
            var GSTAmount = 0;
            var txtGSTPercenatge = document.getElementById("<%= txtGSTPercenatge.ClientID %>");
            var txtGSTWOAmount = document.getElementById("<%= txtGSTWOAmount.ClientID %>");
            var txtGSTAmount = document.getElementById("<%= txtGSTAmount.ClientID %>");
            var txtBillDiscountAmount = document.getElementById("<%= txtBillDiscountAmount.ClientID %>");
            var txtTotalBillAmount = document.getElementById("<%= txtTotalBillAmount.ClientID %>");
            var txtRoundOff = document.getElementById("<%= txtRoundOff.ClientID %>");
            var txtNetAmount = document.getElementById("<%= txtNetAmount.ClientID %>");
            
            if (txtGSTPercenatge.value == '') {
                txtGSTPercenatge.value = '0.00';
            }
            if (txtGSTWOAmount.value == '') {
                txtGSTWOAmount.value = '0.00';
            }
            if (txtGSTAmount.value == '') {
                txtGSTAmount.value = '0.00';
            }
            if (txtTotalBillAmount.value == '') {
                txtTotalBillAmount.value = '0.00';
            }
            if (txtBillDiscountAmount.value == '') {
                txtBillDiscountAmount.value = '0.00';
            }
            if (txtRoundOff.value == '') {
                txtRoundOff.value = '0.00';
            }
            if (txtNetAmount.value == '') {
                txtNetAmount.value = '0.00';
            }

            //Paper Detail Costing
            var grid = document.getElementById("<%= gvPaperDetail.ClientID %>");
            if (grid.rows.length > 0) {
                var totalPaperGST = 0;
                var PaperGST = 0;

                for (i = 0; i < grid.rows.length - 1; i++) {
                    var txtNoofSheet = $("[id*=txtNoofSheet]")
                    var PaperLength = $("[id*=txtPaperLength]")
                    var txtPaperwidth = $("[id*=txtPaperwidth]")
                    var txtGSMValue = $("[id*=txtGSMValue]")

                    var txtPaperRate = $("[id*=txtPaperRate]")
                    var txtTotalCost = $("[id*=txtAmount]")
                    var txtPaperDetailGST = $("[id*=txtPaperDetailGST]")

                    //if (parseInt(txtNoofSheet[i].value) != 0 && parseFloat(txtPaperRate[i].value) != 0) {
                    if (txtNoofSheet[i].value != '' && txtPaperRate[i].value != '') {
                        debugger;
                        var PaperWeight500 = (parseFloat(PaperLength[i].value) * parseFloat(txtPaperwidth[i].value) * parseFloat(txtGSMValue[i].value) / 3100);

                        if (parseFloat(PaperWeight500) > 0) {
                            var PaperRate = 0;
                            var arrValue = PaperWeight500.toString().split(".");
                            var intPart = arrValue[0];
                            var decimalPart = arrValue[1];
                            var firstDecimal = parseInt(decimalPart.substr(0, 1));
                            var SecondDecimal = parseInt(decimalPart.substr(1, 1));

                            if (parseInt(SecondDecimal) > 1) {
                                firstDecimal = parseInt(firstDecimal) + 1;
                            }

                            firstDecimal = parseFloat(parseInt(firstDecimal) / 10);

                            PaperRate = parseFloat(intPart) + parseFloat(firstDecimal);

                            // var LineAmount = (PaperWeight500 * parseFloat(txtPaperRate[i].value) / 500) * parseFloat(txtNoofSheet[i].value);
                            var LineAmount = (parseFloat(txtPaperRate[i].value) * parseFloat(PaperRate) / 500) * parseFloat(txtNoofSheet[i].value);
                        }
                        else {
                            var LineAmount = parseFloat(txtPaperRate[i].value) * parseFloat(txtNoofSheet[i].value);
                        }
                        txtTotalCost[i].value = LineAmount.toFixed(2);
                        //TotalAmount += parseFloat(LineAmount);
                        

                        //GST Calculation....
                        if (parseFloat(txtGSTPercenatge.value) <= 12) {
                            // GST upto 12%
                            var LineGSTWOAmount = (parseFloat(txtTotalCost[i].value) / (100 + 12)) * 100;
                            var PaperGST = parseFloat(LineAmount) - parseFloat(LineGSTWOAmount);
                            txtPaperDetailGST[i].value = PaperGST.toFixed(2);
                            GSTAmount += PaperGST;
                            totalPaperGST += PaperGST;
                            txtTotalCost[i].value = LineGSTWOAmount.toFixed(2);
                            TotalAmount += LineGSTWOAmount + PaperGST;
                        }
                        else {
                            // Get Previous 12% GST 
                            var LineGSTWOPreviousGST = (parseFloat(txtTotalCost[i].value) / (100 + 12)) * 100;
                            // GST for 18% and others
                            var LineGSTWOAmount = parseFloat(LineGSTWOPreviousGST) * parseFloat(txtGSTPercenatge.value) / 100;
                            PaperGST = parseFloat(LineGSTWOAmount);
                            txtPaperDetailGST[i].value = PaperGST.toFixed(2);
                            GSTAmount += PaperGST;
                            totalPaperGST += PaperGST;
                            txtTotalCost[i].value = LineGSTWOPreviousGST.toFixed(2);
                            TotalAmount += LineGSTWOPreviousGST + PaperGST;
                        }
                    }
                }
            }
            //txtGSTWOAmount.value = GSTWOAmount.toFixed(2);

            
            //Printing Detail Costing
            var gridPrintDetail = document.getElementById("<%= gvPrintingDetail.ClientID %>");
            if (gridPrintDetail != null) {
                if (gridPrintDetail.rows.length > 0) {
                    for (i = 0; i < gridPrintDetail.rows.length - 1; i++) {
                        //var txtPrintingQuantity = $("[id*=txtPrintingQuantity]")
                        //var txtPrintingRate = $("[id*=txtPrintingRate]")
                        var txtPrinintgAmount = $("[id*=txtPrinintgAmount]")
                        var txtPrintingGST = $("[id*=txtPrintingGST]")

                        //if (txtPrintingQuantity[i].value != '' && txtPrintingRate[i].value != '') {
                        //    var Amount = parseFloat(txtPrintingQuantity[i].value) * parseFloat(txtPrintingRate[i].value);
                        //    txtPrinintgAmount[i].value = Amount.toFixed(2);
                        //    TotalAmount += parseFloat(Amount);

                        //    var LinePrintingGST = parseFloat(txtPrinintgAmount[i].value) * parseFloat(txtGSTPercenatge.value) / 100;
                        //    txtPrintingGST[i].value = LinePrintingGST.toFixed(2);
                        //    GSTAmount += parseFloat(LinePrintingGST);
                        //}

                        if (txtPrinintgAmount[i].value != '') {
                            var Amount = txtPrinintgAmount[i].value;
                            TotalAmount += parseFloat(Amount);

                            var LinePrintingGST = parseFloat(txtPrinintgAmount[i].value) * parseFloat(txtGSTPercenatge.value) / 100;
                            txtPrintingGST[i].value = LinePrintingGST.toFixed(2);
                            GSTAmount += parseFloat(LinePrintingGST);
                        }
                    }
                }
            }

            //Fabricator Costing
            var grid = document.getElementById("<%= gvFabricator.ClientID %>");
            if (grid.rows.length > 0) {
                for (i = 0; i < grid.rows.length - 1; i++) {
                    var FabricationQuantity = $("[id*=txtFabricationQuantity]")
                    var FabricationRate = $("[id*=txtFabricationRate]")
                    var FabricationAmount = $("[id*=txtFabricationAmount]")
                    var txtFabricationGST = $("[id*=txtFabricationGST]")

                    if (FabricationQuantity[i].value != '' && FabricationRate[i].value != '') {
                        var Amount = parseFloat(FabricationQuantity[i].value) * parseFloat(FabricationRate[i].value);
                        FabricationAmount[i].value = Amount.toFixed(2);
                        TotalAmount += parseFloat(Amount);

                        var LineFabricationGST = parseFloat(FabricationAmount[i].value) * parseFloat(txtGSTPercenatge.value) / 100;
                        txtFabricationGST[i].value = LineFabricationGST.toFixed(2);
                        GSTAmount += parseFloat(LineFabricationGST);
                    }
                }
            }

            //Packaging Costing
            var grid = document.getElementById("<%= gvPackaging.ClientID %>");
            if (grid.rows.length > 0) {
                for (i = 0; i < grid.rows.length - 1; i++) {
                    var txtPackaingQuantity = $("[id*=txtPackaingQuantity]")
                    var txtPackaingRate = $("[id*=txtPackaingRate]")
                    var txtPackaingAmount = $("[id*=txtPackaingAmount]")
                    var txtPackaingGST = $("[id*=txtPackaingGST]")

                    if (txtPackaingQuantity[i].value != '' && txtPackaingAmount[i].value != '') {
                        var Amount = parseFloat(txtPackaingQuantity[i].value) * parseFloat(txtPackaingRate[i].value);
                        txtPackaingAmount[i].value = Amount.toFixed(2);
                        TotalAmount += parseFloat(Amount);

                        var LinePackingGST = parseFloat(txtPackaingAmount[i].value) * parseFloat(txtGSTPercenatge.value) / 100;
                        txtPackaingGST[i].value = LinePackingGST.toFixed(2);
                        GSTAmount += parseFloat(LinePackingGST);
                    }
                }
            }

            //Packaging Costing
            var grid = document.getElementById("<%= gvTransport.ClientID %>");
            if (grid.rows.length > 0) {
                for (i = 0; i < grid.rows.length - 1; i++) {
                    var txtLabourCharge = $("[id*=txtLabourCharge]")
                    var txtCarFare = $("[id*=txtCarFare]")
                    var txtTransportQty = $("[id*=txtTransportQty]")
                    var txtTransportRate = $("[id*=txtTransportRate]")
                    var txtTransportAmount = $("[id*=txtTransportAmount]")
                    var txtTransportGST = $("[id*=txtTransportGST]")

                    if (txtTransportQty[i].value != '' && txtTransportRate[i].value != '' && txtLabourCharge[i].value != '' && txtCarFare[i].value != '') {
                        var Amount = parseFloat(txtTransportQty[i].value) * parseFloat(txtTransportRate[i].value) + parseFloat(txtLabourCharge[i].value) + parseFloat(txtCarFare[i].value);
                        txtTransportAmount[i].value = Amount.toFixed(2);
                        TotalAmount += parseFloat(Amount);

                        var LinePackageGST = parseFloat(txtTransportAmount[i].value) * parseFloat(txtGSTPercenatge.value) / 100;
                        txtTransportGST[i].value = LinePackageGST.toFixed(2);
                        GSTAmount += parseFloat(LinePackageGST);
                    }
                }
            }

            //Design Costing
            var grid = document.getElementById("<%= gvDesignEdit.ClientID %>");
            if (grid.rows.length > 0) {
                for (i = 0; i < grid.rows.length - 1; i++) {
                    var txtDesignRate = $("[id*=txtDesignRate]")
                    var txtDesignAmountGST = $("[id*=txtDesignAmountGST]")

                    if (txtDesignRate[i].value != '') {
                        var Amount = parseFloat(txtDesignRate[i].value);
                        TotalAmount += parseFloat(Amount);

                        var LineDesignGST = parseFloat(txtDesignRate[i].value) * parseFloat(txtGSTPercenatge.value) / 100;
                        txtDesignAmountGST[i].value = LineDesignGST.toFixed(2);
                        GSTAmount += parseFloat(LineDesignGST);
                    }
                }
            }

            debugger;
            txtGSTAmount.value = GSTAmount.toFixed(2);

            //txtGSTAmount.value = GSTWOAmount.toFixed(2);
            var GSTWithoutPaper = parseFloat(GSTAmount) - parseFloat(totalPaperGST);
            txtGSTWOAmount.value = GSTWithoutPaper.toFixed(2);
            var NetAmount = 0;
            TotalAmount = TotalAmount + GSTWithoutPaper;
            txtTotalBillAmount.value = TotalAmount.toFixed(2);
            NetAmount = parseFloat(txtTotalBillAmount.value) - parseFloat(txtBillDiscountAmount.value);//+ parseFloat(txtGSTAmount.value);
            var roundoff = Math.round(NetAmount) - NetAmount;
            NetAmount = NetAmount + roundoff;
            txtNetAmount.value = NetAmount.toFixed(2);
            txtRoundOff.value = roundoff.toFixed(2);
        }
    </script>

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
                          &nbsp; QUOTATION WITH JOB CREATION
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Quotation No:
                        </td>
                        <td>
                            <asp:TextBox ID="txtQuotationNo" runat="server" CssClass="textbox" Width="150px" BorderStyle="Inset"
                                BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off">
                            </asp:TextBox>
                        </td>
                        <td>
                            Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="textbox" Width="75px" onFocus="this.select()"
                                autocomplete="off" BackColor="White" ForeColor="Black"></asp:TextBox>
                            <Ajax:CalendarExtender ID="txtDate_CalendarExtender" runat="server" PopupButtonID="btnCalenderPopup1"
                                TargetControlID="txtDate"
                                Format="dd/MM/yyyy">
                            </Ajax:CalendarExtender>
                            <asp:ImageButton ID="btnCalenderPopup1" runat="server" ImageUrl="~/Image/Calendar_scheduleHS.png"
                                Height="16px" />
                        </td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chkJobCreation" runat="server" Text="Job sheet" /> &nbsp;</td>
                        <td colspan="3"><b> Quotation Number create while saved data if user leave to blank </b></td>
                    </tr>
                    <tr>
                        <td>
                            Customer:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlClient" runat="server" class="styled-select" BackColor="White"  AutoPostBack="true"
                                ForeColor="Black" Width="360px" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="btnCreateNewCustomer" runat="server" Text="+" OnClick="btnCreateNewCustomer_Click" />
                            <asp:RequiredFieldValidator ID="ddlClient_RequiredFieldValidator" runat="server"
                                ControlToValidate="ddlClient" ErrorMessage="Please select respected client" InitialValue="0"
                                ForeColor="#FF3300" SetFocusOnError="True" ValidationGroup="ValidEntry">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           Address:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtClientAddress" runat="server" CssClass="textbox" Width="385px" BorderStyle="Inset" TextMode="MultiLine" Height="50px"
                                BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" Enabled="false">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                         Phone No:
                        </td>
                        <td>
                            <asp:TextBox ID="txtClientPhoneNo" runat="server" autocomplete="off" BackColor="White" Enabled="false" BorderStyle="Inset" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="150px">
                            </asp:TextBox>
                        </td>
                        <td>
                            Email:
                        </td>
                        <td>
                            <asp:TextBox ID="txtClientEmail" runat="server" autocomplete="off" BackColor="White" Enabled="false" BorderStyle="Inset" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="150px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Item</td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlItem" runat="server" class="styled-select" BackColor="White"  AutoPostBack="true"
                                ForeColor="Black" Width="390px" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td>Description</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtItemDescription" runat="server" CssClass="textbox" Width="385px" BorderStyle="Inset" TextMode="MultiLine" Height="50px"
                                BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            GST:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGST" runat="server" BackColor="White" class="styled-select" ForeColor="Black" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlGST_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            GST(%):
                        </td>
                        <td>
                            <asp:TextBox ID="txtGSTPercenatge" runat="server" autocomplete="off" Enabled="false" onkeyup="return GSTCalculation();" BackColor="White" BorderStyle="Inset" Style="text-align: right" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="50px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            GST W/O Paper:
                        </td>
                        <td>
                            <asp:TextBox ID="txtGSTWOAmount" runat="server" autocomplete="off" Enabled="false" onkeyup="return GSTCalculation();" BackColor="White" BorderStyle="Inset" Style="text-align: right" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="100px">
                            </asp:TextBox>
                        </td>
                         <td>
                            GST Amount:
                        </td>
                        <td>
                            <asp:TextBox ID="txtGSTAmount" runat="server" autocomplete="off" Enabled="false" onkeyup="return GSTCalculation();" BackColor="White" BorderStyle="Inset" Style="text-align: right" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="100px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Billing Amount:
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalBillAmount" runat="server" autocomplete="off" onkeyup="return GSTCalculation();" Enabled="false" BackColor="White" BorderStyle="Inset" CssClass="textbox" ForeColor="Black" Style="text-align: right" onFocus="this.select()" Width="100px">
                            </asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            Discount Amount:
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillDiscountAmount" runat="server" autocomplete="off" onkeyup="return GSTCalculation();" BackColor="White" BorderStyle="Inset" Style="text-align: right" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="100px">
                            </asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            Round Off (+/-):
                        </td>
                        <td>
                            <asp:TextBox ID="txtRoundOff" runat="server" autocomplete="off" onkeyup="return GSTCalculation();" BackColor="White" BorderStyle="Inset" Style="text-align: right" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="100px">
                            </asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            Payable Amount :
                        </td>
                        <td>
                            <asp:TextBox ID="txtNetAmount" runat="server" autocomplete="off" BackColor="White" onkeyup="return GSTCalculation();" Enabled="false" BorderStyle="Inset" CssClass="textbox" ForeColor="Black" Style="text-align: right" onFocus="this.select()" Width="100px"></asp:TextBox>
                        </td>
                         <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Amount Received
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaidAmount" runat="server" autocomplete="off" BackColor="White" BorderStyle="Inset" Style="text-align: right" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="100px" Text="0.00" ValidationGroup="ValidEntry"/>
                            <asp:RequiredFieldValidator ID="req" runat="server"
                                ControlToValidate="txtPaidAmount" ErrorMessage="Enter Amount." InitialValue="0.00"
                                ForeColor="#FF3300" SetFocusOnError="True" ValidationGroup="ValidEntry">*</asp:RequiredFieldValidator>
                               </td>
                        <td colspan="2">
                          <span style="color:red; font-weight:bold;"> Without payment Job cann't be saved.</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Jobsheet Type :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlJobSheetType" runat="server">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Pakka" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Kachcha" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                         <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Payment Mode:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPaymentMode" runat="server" class="styled-select" BackColor="White"
                                ForeColor="Black" Width="150px" ValidationGroup="ValidEntry">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="req2" runat="server"
                                ControlToValidate="ddlPaymentMode" ErrorMessage="Select Payment Mode" InitialValue="0"
                                ForeColor="#FF3300" SetFocusOnError="True" ValidationGroup="ValidEntry">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Payment Detail:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaymentDetail" runat="server" TextMode="MultiLine" autocomplete="off" BackColor="White" BorderStyle="Inset" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="280px" Height="40px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                
                <div style="min-height: 100%; overflow: auto">
                    <asp:GridView ID="gvPaperDetail" runat="server" Visible="true" AutoGenerateColumns="False"
                        GridLines="None" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        OnRowEditing="gvPaperDetail_RowEditing" OnRowDeleting="gvPaperDetail_RowDeleting"
                        Width="80%">
                        <Columns>
                             <asp:TemplateField HeaderText="NO/SHEET" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNoofSheet" runat="server" Width="70px" Height="18px" Text='<%# Eval("NoofSheet") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="GSM" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlGSM" AutoPostBack="true" runat="server" class="styled-select" OnSelectedIndexChanged="ddlGSM_SelectedIndexChanged"
                                        Width="80px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="GSM" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtGSMValue" runat="server" Width="50px" Height="18px" Text='<%# Eval("GSMValue") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="PAPER SIZE" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPaperSize" AutoPostBack="true" runat="server" class="styled-select" OnSelectedIndexChanged="DdlPaperSize_SelectedIndexChanged"
                                        Width="100px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="LENGTH" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPaperLength" runat="server" Width="50px" Height="18px" Text='<%# Eval("PaperLength") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WIDTH" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPaperwidth" runat="server" Width="50px" Height="18px" Text='<%# Eval("Paperwidth") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PAPER TYPE" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPaperType" runat="server" class="styled-select" AutoPostBack="true" OnSelectedIndexChanged="ddlPaperType_SelectedIndexChanged"
                                        Width="210px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PAPER COMPANY" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPaperManufature" AutoPostBack="true" runat="server" class="styled-select" OnSelectedIndexChanged="ddlPaperManufature_SelectedIndexChanged"
                                        Width="150px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                                                    
                            <asp:TemplateField HeaderText="RATE" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPaperRate" runat="server" Width="60px" Height="18px" Text='<%# Eval("LineRate") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AMOUNT" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAmount" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineAmount") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GST" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPaperDetailGST" runat="server" Width="60px" Height="18px" Text='<%# Eval("LineGST") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnAddPaperCosting" runat="server" ImageUrl="~/Image/AddRow.png" Height="18"
                                        Width="18" CausesValidation="false" OnClick="ibtnAddPaperCosting_Click" />
                                    <asp:ImageButton ID="ibtnDeletePaperCosting" runat="server" ImageUrl="~/Image/delete.png" Height="18"
                                        Width="18" CausesValidation="false" CommandName="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:Button ID="btnPopulatePrintingDetail" runat="server" Text="Add Printing Detail" CssClass="myButton"
                        OnClick="btnPopulatePrintingDetail_Click" />
                     <asp:Button ID="btnRefreshPrintingDetail" runat="server" Text="Refresh Detail" CssClass="myButton"
                        OnClick="btnRefreshPrintingDetail_Click" />

                <div style="min-height: 100%; overflow: auto">
                    <asp:GridView ID="gvPrintingDetail" runat="server" Visible="true" AutoGenerateColumns="False"
                        GridLines="None" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        Width="80%">
                        <Columns>
                             <asp:TemplateField HeaderText="NO/SHEET" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNoofSheetPrintDetail" runat="server" Width="65px" Height="18px" Text='<%# Eval("NoOfSheetPrintDetail") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PAPER SIZE" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPaperSizePrintDetail" runat="server" class="styled-select" Enabled="false"
                                        Width="75px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="CUTTING" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlCutting" runat="server" class="styled-select" AutoPostBack="true" 
                                        OnSelectedIndexChanged="ddlPlateType_SelectedIndexChanged"
                                        Width="80px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CUT.PIC" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNoofCuttingPices" runat="server" Width="50px" Height="18px" Text='<%# Eval("NoofCuttingPices") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                             
                            <asp:TemplateField HeaderText="FINISH SIZE" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFinishSize" runat="server" Width="70px" Height="18px" Text='<%# Eval("FinishSize") %>'
                                        BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="SIDE" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlSide" runat="server" class="styled-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSide_SelectedIndexChanged"
                                        Width="100px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="IMPRESSION" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtImpression" runat="server" Width="75px" Height="18px" Text='<%# Eval("Impression") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="COLOR" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlColor" runat="server" class="styled-select" AutoPostBack="true" OnSelectedIndexChanged="ddlPlateType_SelectedIndexChanged"
                                        Width="140px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="No Of Set" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPrintingQuantity" runat="server" Width="60px" Height="18px" Text='<%# Eval("LineQuantity") %>'
                                        BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PLATE" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPlateType" runat="server" class="styled-select" OnSelectedIndexChanged="ddlPlateType_SelectedIndexChanged" AutoPostBack="true"
                                        Width="70px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="MACHINE" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMachine" runat="server" class="styled-select"
                                        Width="110px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="RATE" HeaderStyle-HorizontalAlign="Right" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPrintingRate" runat="server" Width="50px" Height="18px" Text='<%# Eval("LineRate") %>'
                                        BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AMOUNT" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPrinintgAmount" runat="server" Width="50px" Height="18px" Text='<%# Eval("LineAmount") %>'
                                        BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="GST" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPrintingGST" runat="server" Width="50px" Height="18px" Text='<%# Eval("LineGST") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div style="min-height: 100%; overflow: auto">
                    <asp:GridView ID="gvFabricator" runat="server" Visible="true" AutoGenerateColumns="False"
                        GridLines="None" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        OnRowEditing="gvFabricator_RowEditing" OnRowDeleting="gvFabricator_RowDeleting"
                        Width="80%">
                        <Columns>
                             <asp:TemplateField HeaderText="FABRICATION ITEM" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlfabricationItem" runat="server" class="styled-select"
                                        Width="300px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SIZE" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFabricationSize" runat="server" Width="300px" Height="18px" Text='<%# Eval("FabricationItemSize") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QUANTITY" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFabricationQuantity" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineQuantity") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RATE" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFabricationRate" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineRate") %>'
                                        BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AMOUNT" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFabricationAmount" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineAmount") %>'
                                        BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GST" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFabricationGST" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineGST") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STATUS" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtStatus" runat="server" Text="" Visible = "false" />
                                    <asp:DropDownList ID="ddlSelect" runat="server">
                                        <asp:ListItem Text="SELECT" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="OPEN" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="WIP" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="CLOSED" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnAddFabricationCosting" runat="server" ImageUrl="~/Image/AddRow.png" Height="18"
                                        Width="18" CausesValidation="false" OnClick="ibtnAddFabricationCosting_Click" />
                                    <asp:ImageButton ID="ibtnDeleteFabricationCosting" runat="server" ImageUrl="~/Image/delete.png" Height="18"
                                        Width="18" CausesValidation="false" CommandName="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div style="min-height: 100%; overflow: auto">
                    <asp:GridView ID="gvPackaging" runat="server" Visible="true" AutoGenerateColumns="False"
                        GridLines="None" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        OnRowEditing="gvPackaging_RowEditing" OnRowDeleting="gvPackaging_RowDeleting"
                        Width="80%">
                        <Columns>
                             <asp:TemplateField HeaderText="PACKAGING ITEM" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPackagingItem" runat="server" class="styled-select"
                                        Width="690px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="QUANTITY" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPackaingQuantity" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineQuantity") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="RATE" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPackaingRate" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineRate") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AMOUNT" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPackaingAmount" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineAmount") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GST" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPackaingGST" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineGST") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnAddPackaingCosting" runat="server" ImageUrl="~/Image/AddRow.png" Height="18"
                                        Width="18" CausesValidation="false" OnClick="ibtnAddpackaingCosting_Click" />
                                    <asp:ImageButton ID="ibtnDeletePackaingCosting" runat="server" ImageUrl="~/Image/delete.png" Height="18"
                                        Width="18" CausesValidation="false" CommandName="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div style="min-height: 100%; overflow: auto">
                    <asp:GridView ID="gvTransport" runat="server" Visible="true" AutoGenerateColumns="False"
                        GridLines="None" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        OnRowEditing="gvTransport_RowEditing" OnRowDeleting="gvTransport_RowDeleting"
                        Width="80%">
                        <Columns>
                            <asp:TemplateField HeaderText="LABOUR CH" HeaderStyle-HorizontalAlign="Right" >
                                <ItemTemplate>
                                    <asp:TextBox ID="txtLabourCharge" runat="server" Width="85px" Height="18px" Text='<%# Eval("LabourCharge") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CAR CHARGE" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCarFare" runat="server" Width="85px" Height="18px" Text='<%# Eval("TransportCharge") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="QUANTITY" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTransportQty" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineQuantity") %>'
                                        BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="NARRATION">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNarration" runat="server" Width="280px" Height="18px" Text=""
                                                 BorderStyle="Inset" CssClass="textbox"
                                                 BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" Style="text-align: Left"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="UNIT" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTransportUnit" runat="server" class="styled-select"
                                        Width="200px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="RATE" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTransportRate" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineRate") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="AMOUNT" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTransportAmount" runat="server" Width="85px" Height="18px" Text='<%# Eval("LineAmount") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GST" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTransportGST" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineGST") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnAddTransportCosting" runat="server" ImageUrl="~/Image/AddRow.png" Height="18"
                                        Width="18" CausesValidation="false" OnClick="ibtnAddTransportCosting_Click" />
                                    <asp:ImageButton ID="ibtnDeleteTransportCosting" runat="server" ImageUrl="~/Image/delete.png" Height="18"
                                        Width="18" CausesValidation="false" CommandName="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div style="min-height: 100%; overflow: auto">
                    <asp:GridView ID="gvDesignEdit" runat="server" Visible="true" AutoGenerateColumns="False"
                        GridLines="None" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        OnRowEditing="gvDesignEdit_RowEditing" OnRowDeleting="gvDesignEdit_RowDeleting"
                        Width="80%">
                        <Columns>
                             <asp:TemplateField HeaderText="OPERATOR" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlOperator" runat="server" class="styled-select"
                                        Width="270px" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="NARRATION">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNarration" runat="server" Width="580px" Height="18px" Text='<%# Eval("Narration") %>'
                                        BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" Style="text-align: Left"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="AMOUNT" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDesignRate" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineAmount") %>'
                                       BorderStyle="Inset" CssClass="textbox"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GST" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDesignAmountGST" runat="server" Width="75px" Height="18px" Text='<%# Eval("LineGST") %>'
                                       BorderStyle="Inset" CssClass="textbox" Enabled="false"
                                       BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off" 
                                       onkeyup="return GSTCalculation();" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnAddDesignCosting" runat="server" ImageUrl="~/Image/AddRow.png" Height="18"
                                        Width="18" CausesValidation="false" OnClick="ibtnAddDesignCosting" />
                                    <asp:ImageButton ID="ibtnDeleteDesignCosting" runat="server" ImageUrl="~/Image/delete.png" Height="18"
                                        Width="18" CausesValidation="false" CommandName="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div style="margin: 0 0 0 0">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="ValidEntry" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="myButton" ValidationGroup="ValidEntry"
                        OnClick="btnSave_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="myButton" OnClientClick="return confirm('Are you sure? want to delete this Quotation Entry Note.');"
                        OnClick="btnDelete_Click" />
                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="myButton" OnClick="btnView_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="myButton" OnClick="btnReset_Click" />
                </div>

                 <%--<asp:LinkButton ID="lnkbtnMaster" runat="server"></asp:LinkButton>
            <Ajax:ModalPopupExtender ID="mndlPopupMaster" runat="server" BehaviorID="sdfkhjksd" TargetControlID="lnkbtnMaster"
                PopupControlID="pnlMasterHeader" BackgroundCssClass="modalBackground" CancelControlID="btnCloseMasterPopup" PopupDragHandleControlID="pnlMasterHeader">
            </Ajax:ModalPopupExtender>
            <asp:Panel ID="pnlMasterHeader" runat="server" CssClass="ModalWindow">
               <table style="margin: 0 0 0 0;">
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
                    <asp:Button ID="btnMasterData" runat="server" Text="Save" CssClass="myButton"
                        OnClick="btnMasterData_Click"/>
                    <asp:Button ID="btnCloseMasterPopup" runat="server" Text="Close" CssClass="myButton" />
                </div>
            </td>
        </tr>
    </table>
            </asp:Panel>--%>

<div style="min-height:800px; overflow: auto">
                
                 <asp:LinkButton ID="lnkbtn" runat="server"></asp:LinkButton>
            <Ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BehaviorID="sdfkhjksd" TargetControlID="lnkbtn"
                PopupControlID="pnlCustomerHeader" BackgroundCssClass="modalBackground" CancelControlID="BtnClosePopup" PopupDragHandleControlID="pnlCustomerHeader">
            </Ajax:ModalPopupExtender>
            <asp:Panel ID="pnlCustomerHeader" runat="server" CssClass="ModalWindow">
                <table width="100%" cellspacing="2" cellpadding="4" style="margin: 0 0 0 0;">
                    <tr class="head_tag">
                        <td colspan="4">&nbsp; NEW CUSTOMER REGISTRATION
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp Name:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNewCustomerName" runat="server" autocomplete="off" BackColor="White" BorderStyle="Inset" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="340px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp Address:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNewCustomerAddress" runat="server" CssClass="textbox" Width="335px" BorderStyle="Inset" TextMode="MultiLine" Height="40px"
                                BackColor="White" ForeColor="Black" onFocus="this.select()" autocomplete="off">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp Phone No:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNewCustomerPhoneNo" runat="server" autocomplete="off" BackColor="White" BorderStyle="Inset" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="150px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp Email:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNewCustomerEmail" runat="server" autocomplete="off" BackColor="White" BorderStyle="Inset" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="150px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp GST NO:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNewCustomerGST" runat="server" autocomplete="off" BackColor="White" BorderStyle="Inset" CssClass="textbox" ForeColor="Black" onFocus="this.select()" Width="150px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp</td>
                        <td>
                            <asp:Button ID="BtnNewCustomer" runat="server" Text="Save" CssClass="myButton"
                                OnClick="BtnNewCustomer_Click" />
                            <asp:Button ID="BtnClosePopup" runat="server" Text="Cancel" CssClass="myButton" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp Search by:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlSearchByCustomer" runat="server" Width="100px" class="styled-select" AppendDataBoundItems="true" BackColor="White" ForeColor="Black">
                        <asp:ListItem>Name</asp:ListItem>
                        <asp:ListItem>Telephone</asp:ListItem>
                        <asp:ListItem>GST No</asp:ListItem>
                    </asp:DropDownList>
                            <asp:TextBox ID="txtSearchbyCustomer" runat="server" Width="150px"></asp:TextBox>
                            <asp:Button ID="btnSearchCustomer" runat="server" Text="Search" CssClass="myButton"
                                OnClick="btnSearchCustomer_Click"/>
                        </td>
                    </tr>
                </table>
                
                    <asp:GridView ID="gvCustomer" runat="server" Visible="true" AutoGenerateColumns="False" DataKeyNames="CustomerID" PageSize="5"
                        GridLines="None" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" vi
                        OnRowEditing="gvCustomer_RowEditing" OnRowCommand="gvCustomer_RowCommand" OnPageIndexChanged="gvCustomer_PageIndexChanged"
                        Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="NAME" ItemStyle-HorizontalAlign="Left">
                                 <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>' Width="300px" Style="white-space: nowrap;"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TELEPHONE">
                                 <ItemTemplate>
                                    <asp:Label ID="lblTelephone" runat="server" Text='<%# Eval("Telephone") %>' Width="100px" Style="white-space: nowrap;"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EMAIL">
                                 <ItemTemplate>
                                    <asp:Label ID="lblGSTNo" runat="server" Text='<%# Eval("GSTNo") %>' Width="100px" Style="white-space: nowrap;"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Button ID="btnSelectCustomer" runat="server" Text="select" CausesValidation="false" CommandName="Select" CommandArgument='<%#Eval("CustomerID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                            </asp:Panel>
</div>
            </center>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCreateNewCustomer" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSearchCustomer" EventName="Click" />
            <%--<asp:PostBackTrigger ControlID="btnNewGSM" />
            <asp:PostBackTrigger ControlID="btnNewPaperSize" />
            <asp:PostBackTrigger ControlID="btnNewPaperType" />
            <asp:PostBackTrigger ControlID="btnNewPaperManufature" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
