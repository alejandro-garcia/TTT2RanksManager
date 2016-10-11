<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CargaMasiva.aspx.cs" Inherits="TTT2RanksManager.CargaMasiva" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link rel="stylesheet" href="Scripts/jquery-ui.css" />
    <link rel="Stylesheet" href="Scripts/ranksFormStyles.css" />
    <script type="text/javascript" src="Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="Scripts/jqDatePicker.js"></script>
</head>
<body>
    <asp:SqlDataSource ID="dsPeleadores" runat="server" ConnectionString="<%$ ConnectionStrings:TekkenCnn %>" 
            ProviderName="System.Data.SqlClient" SelectCommand="SELECT charId, name as Nombre FROM Peleadores"></asp:SqlDataSource>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">          
                <ContentTemplate>
        <div>
        <table>
        <tr>
            <td class="auto-style5">Fecha</td>
            <!-- CssClass="From-Date" -->
             <td class="style4" nowrap="nowrap"><asp:TextBox ID="datepicker"  runat="server" 
                     Width="73px"  CssClass="From-Date" ontextchanged="datepicker_TextChanged" AutoPostBack="true"></asp:TextBox></td>
             <td class="style4">
                 <asp:Label ID="lblVictorias" runat="server" Text="0"></asp:Label>
                </td>
             <td class="style4">
                 <asp:Label ID="lblDerrotas" runat="server" Text="0"></asp:Label>
                </td>
             
        </tr>
        <tr>
            <td>
                Peleador1:
            </td>
            <td>
                <asp:DropDownList ID="ddlPeleador1" runat="server" DataSourceID="dsPeleadores" DataTextField="nombre" DataValueField="charId"></asp:DropDownList>
            </td>
        </tr>
            <tr>
            <td>
                Peleador2:
            </td>
            <td>
                <asp:DropDownList ID="ddlPeleador2" runat="server" DataSourceID="dsPeleadores" DataTextField="nombre" DataValueField="charId"></asp:DropDownList>
            </td>

            </tr>
            <tr>
                <td>Combates:</td>
                <td>
                    <asp:TextBox ID="txtCombates" runat="server" TextMode="MultiLine" Height="94px" MaxLength="4000" OnTextChanged="txtCombates_TextChanged" Width="382px"></asp:TextBox></td>
            </tr>
        <tr>
            <td>
                <asp:Button ID="BtnGuardar" runat="server" Text="Button" OnClick="BtnGuardar_Click" />
            </td>
        </tr>
    </table>        
        
    </div>
    </ContentTemplate>
    </asp:UpdatePanel> 
    </form>
</body>
</html>
