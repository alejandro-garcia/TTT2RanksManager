<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RangoPeleador.aspx.cs" Inherits="TTT2RanksManager.RangoPeleador" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 22px;
        }
        .style2
        {
            width: 22px;
        }
        .style3
        {
            width: 22px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:ScriptManager ID="ScriptManager2" runat="server" EnablePartialRendering="true">
      </asp:ScriptManager>
           <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate> 
           <table>
                 	<tr>
      		<td class="style3">Peleador</td>
            <td class="style2">Rango</td>
            <td class="style1">Ptos.Rango</td>
      	</tr>
           <tr>  
           <td class="style3">

               <asp:DropDownList ID="ddlPeleador" runat="server" OnSelectedIndexChanged="ddlPeleador_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true">
               </asp:DropDownList>
               
            </td>
            <td class="style2">
                <asp:DropDownList ID="ddlRango" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtPuntos" runat="server" Width="72px" 
                    ontextchanged="txtPuntos_TextChanged"></asp:TextBox>
            </td>
            </tr>
            </table>
            </ContentTemplate>
               <Triggers>
                   <asp:AsyncPostBackTrigger ControlID="ddlPeleador" EventName="SelectedIndexChanged" />      
               </Triggers>
               </asp:UpdatePanel>
        
           

    </div><div>
     <asp:Button ID="btnGuardar" runat="server" Text="Button" 
                onclick="btnGuardar_Click" />
                </div>
    </form>
</body>
</html>
