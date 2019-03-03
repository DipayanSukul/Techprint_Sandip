<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="TechPrint.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tech Printing- Login Panel</title>
    <link href="CSS/style1.css" rel="stylesheet" type="text/css" />
    <link href="CSS/filemanager.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <center>
            <div id="main">
                <center>
                    <table border="0" cellpadding="10" cellspacing="0" style="text-align: left; width: 100%">
                        <tr>
                            <td style="width: 60%;" align="center">
                                <img src="Image/Inventory-Wallpaper.png" alt="User Logon" width="300px" />
                            </td>
                            <td style="widtd: 70%">
                                <div style="border: 1px solid #bbb !important; border-radius: 15px">
                                    <table id="tblLogin" cellpadding="6" style="width: 100%">
                                        <tr>
                                            <td align="center" colspan="4" style="border-right: black 1px solid; background-position: center center;
                                                border-top: black 1px solid; font-size: 14px; background-image: url(image/blackfade.jpg);
                                                padding-bottom: 3px; border-left: black 1px solid; color: white; padding-top: 3px;
                                                border-bottom: black 1px solid; background-repeat: no-repeat; font-family: tahoma;
                                                text-decoration: none; border-top-left-radius: 15px; border-top-right-radius: 15px">
                                                <font style="font-size: small; font-family: Verdana; font-weight: bold">APPLICATION
                                                    LOGIN PANEL </font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td rowspan="8">
                                                <img src="image/group_128.gif" alt="User Logon" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-weight: bold" class="style1">
                                                &nbsp;
                                            </td>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-weight: bold" class="style1">
                                                <asp:Label ID="lblLoginID" runat="server">Login ID</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtuid" runat="server" CssClass="textbox" Width="220px" onFocus="this.select()"
                                                    autocomplete="off"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="txtuid_RequiredFieldValidator1" runat="server" ControlToValidate="txtuid"
                                                    ErrorMessage="Please Enter Login ID" ForeColor="#FF3300" SetFocusOnError="True"
                                                    ValidationGroup="ValidEntry" Display="Dynamic">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-weight: bold" class="style1">
                                                <asp:Label ID="lblPassWord" runat="server">PassWord</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox TextMode="PassWord" ID="txtpwd" runat="server" CssClass="textbox" autocomplete="off"
                                                    onFocus="this.select()" Width="220px"> </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="PassWord_RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="txtpwd" ErrorMessage="Please Enter PassWord" ForeColor="#FF3300"
                                                    SetFocusOnError="True" ValidationGroup="ValidEntry" Display="Dynamic">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-weight: bold" class="style1">
                                                &nbsp;
                                            </td>
                                            <td style="font-weight: bold; vertical-align: top">
                                                <span>
                                                    <asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember me" />
                                                    &nbsp; </span>
                                            </td>
                                            <td style="font-weight: bold; vertical-align: top">
                                                <span>
                                                    <asp:ImageButton ID="ImgBtnLogin" runat="server" ImageUrl="~/Image/loginbtn.jpeg"
                                                        ValidationGroup="ValidEntry" Width="100px" OnClick="ImgBtnLogin_Click" />
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="font-weight: bold">
                                                <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="#ff0000" Font-Bold="true"
                                                    Width="250" />
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                                    ShowSummary="False" ValidationGroup="ValidEntry" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
        </div>
    </form>
</body>
</html>
