<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistroPeleas2.aspx.cs" Inherits="TTT2RanksManager.RegistroPeleas2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
        .style2
        {
            height: 23px;
            width: 130px;
        }
        .style3
        {
            height: 23px;
            width: 52px;
        }
        .style4
        {
            height: 26px;
        }
        .style5
        {
            height: 23px;
            width: 209px;
        }
        .auto-style1
        {
            width: 209px;
            height: 26px;
        }
        .From-Date
        {}
        .auto-style2
        {
            font-weight: bold;
            height: 24px;
        }
        .auto-style3
        {
            height: 24px;
        }
        .auto-style5
        {
            font-weight: bold;
            height: 26px;
        }
        .auto-style6
        {
            background-color: #9900FF;
            color: white;
            font-weight: bold;
            height: 23px;
        }
        .auto-style7
        {
            font-weight: bold;
            background-color: blue;
            color: white;
            height: 23px;
        }
        </style>
    <link rel="stylesheet" href="Scripts/jquery-ui.css" />
    <link rel="Stylesheet" href="Scripts/ranksFormStyles.css" />
    <script type="text/javascript" src="Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="Scripts/jqDatePicker.js"></script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
        <div>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">          
            <ContentTemplate>
        <table id="peleasTbl">
        <tr>
            <td class="sectionTitle">Datos Pelea</td>

        </tr>
            <tr>        <td></td>    <td></td>
            <td class="titleRow">Victorias</td>
                <td class="titleRow">Derrotas</td>
            </tr>
        <tr>
         <td class="auto-style5">Fecha</td>
         <td class="style4" nowrap="nowrap"><asp:TextBox ID="datepicker" CssClass="From-Date" runat="server" 
                 Width="73px" ontextchanged="datepicker_TextChanged" AutoPostBack="true"></asp:TextBox></td>
         <td class="style4">
             <asp:Label ID="lblVictorias" runat="server" Text="0"></asp:Label>
            </td>
         <td class="auto-style1">
             <asp:Label ID="lblDerrotas" runat="server" Text="0"></asp:Label>
            </td>
        </tr>
        </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="datepicker" EventName="TextChanged" />
                </Triggers>
                </asp:UpdatePanel>
            </div>
        <div>
           
        

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">          
            <ContentTemplate>
            <table><tr id ="peleas_encab">
        <td class="style1"></td>
        <td class="titleRow">Nombre</td>

        <td class="titleRow">Rango</td>
        <td class="titleRow">Oportunidad Cambio de Rango </td>      
        </tr><tr><td class="auto-style2">Peleador1</td>
            <td class="style4">
                <asp:TextBox ID="txtPeleador1" AutoPostBack="true" EnableViewState="true" runat="server" OnTextChanged="txtPeleador1_OnTextChanged"></asp:TextBox>
                <%--<asp:HiddenField ID="hfPeleador1" runat="server" EnableViewState="True" />--%>
                <input type="hidden" id="hfPeleador1" name="hfPeleador1"/>
            </td>
            <td class="style4">
            <asp:Label ID="lblRank1" runat="server" Text="Rango1"></asp:Label>
            </td>
                         <td class="auto-style4">
            
            <asp:Panel ID="Panel1" runat="server" Width="225px">
                <asp:RadioButton ID="rbNoAplica1" runat="server" Text="N/A" Checked="True" GroupName="fighter1" />
                <asp:RadioButton ID="rbAscenso1" runat="server" Text="Ascenso" GroupName="fighter1" OnCheckedChanged="rbAscenso1_CheckedChanged" />
                <asp:RadioButton ID="rbDescenso1" runat="server" Text="Descenso" GroupName="fighter1" />
            </asp:Panel>
            
            </td>
 
                            </tr>
                </table>                                    
            </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtPeleador1" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
            </div>
        
           <div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table>
                <tr>
                    <td class="fieldTitle">Peleador2</td>
                <td class="style4">
                    <asp:TextBox ID="txtPeleador2" runat="server" OnTextChanged="txtPeleador2_OnTextChanged"></asp:TextBox>
                    <asp:HiddenField ID="hfPeleador2" runat="server" />
                </td>
                <td class="style4">
                <asp:Label ID="lblRank2" runat="server" Text="Rango2"></asp:Label>
                </td>
                       <td class="auto-style10">
            
            <asp:Panel ID="Panel2" runat="server" Height="21px" Width="225px">
                <asp:RadioButton ID="rbNoAplica2" runat="server" Text="N/A" Checked="True" GroupName="fighter2" />
                <asp:RadioButton ID="rbAscenso2" runat="server" Text="Ascenso" GroupName="fighter2" />
                <asp:RadioButton ID="rbDescenso2" runat="server" Text="Descenso" GroupName="fighter2" />
            </asp:Panel>
            
            </td>
                </tr>                               
                </table>
            </ContentTemplate>
             <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtPeleador2" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel> 
               
               </div>                   
          <!--</table>-->
     </div>
        <div>
                    
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
        <table><tr>
        <td class="sectionTitle">Rival</td>

        </tr>
        <tr>
        <td class="auto-style7">Nombre</td>
        <td class="auto-style7">Rango1</td>

        <td class="auto-style7">Rango2</td>
        </tr>
        <tr>
        <td class="style1">
            <asp:TextBox ID="txtNombreRival" runat="server" Height="22px" OnTextChanged="txtNombreRival_TextChanged" Width="89px" AutoPostBack="true"></asp:TextBox>
            <asp:HiddenField ID="hfRivalId" runat="server" />
            <asp:Button ID="Button1" runat="server" Text="Button" />
            </td>
        <td class="style2">
            <asp:TextBox ID="txtRivalRank1" runat="server"></asp:TextBox>
            <asp:HiddenField ID="hfRivalRank1Id" runat="server" />
            </td>

        <td class="style3">
            <asp:TextBox ID="txtRivalRank2" runat="server"></asp:TextBox>
            <asp:HiddenField ID="hfRivalRank2Id" runat="server" />
            </td>
        </tr>
            <tr>

                <td class="fieldTitle">Peleador1</td>
                <td class="style4">
                    <asp:TextBox ID="TxtRivalPeleador1" runat="server" AutoPostBack="true" EnableViewState="true"></asp:TextBox>
                    <asp:HiddenField ID="hfRivalPeleador1Id" runat="server" />
                </td>
                <td>
                    

                </td>
            </tr>
        <tr><td class="fieldTitle">Peleador2</td><td>
                <asp:TextBox ID="TxtRivalPeleador2" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hfRivalPeleador2Id" runat="server" />
          </td></tr></table>
        </ContentTemplate>
        <Triggers>
              <asp:AsyncPostBackTrigger ControlID="txtNombreRival" EventName="TextChanged" />
        </Triggers>
        </asp:UpdatePanel>
            </div>
        <div>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
           <table>
             <tr>
                    <td class="auto-style6">Resultado</td>
                </tr>
               <tr><td class="auto-style7">Puntos</td></tr>
          <tr>
        <td class="style1">
            <asp:TextBox ID="txtPuntos" runat="server"></asp:TextBox>
            </td>
        <td class="style2">
            </td>

        <td class="style3"></td>

        </tr>
        <tr>
        <td class="style1"></td>
        <td class="style2">
            &nbsp;</td>

        <td class="style3"></td>

        </tr>
        <tr>
        <td class="style1">
            <asp:Button ID="btnGuardar" runat="server" onclick="btnGuardar_Click" 
                Text="Guardar" />
            </td>
        <td class="style2">
            &nbsp;</td>

        <td class="style3">
            &nbsp;</td>

        </tr>
            </table>
            </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
        
    </div>
    </form>    
</body>
</html>
