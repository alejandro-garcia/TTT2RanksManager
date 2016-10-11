<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FighterSelector.ascx.cs" Inherits="TTT2RanksManager.FighterSelector" %>

<asp:GridView ID="GridView1" runat="server" DataSourceID="SQLDSPeleadores" OnRowCreated="GridView1_OnRowCreated" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
    <Columns>
        <asp:ImageField></asp:ImageField>
    </Columns>
</asp:GridView>

<asp:SqlDataSource ID="SQLDSPeleadores" runat="server" 
    ConnectionString="<%$ ConnectionStrings:TekkenCnn %>" 
    SelectCommand="SELECT DISTINCT [charId], [name], image FROM [Peleadores]" 
    ProviderName="<%$ ConnectionStrings:TekkenCnn.ProviderName %>">
</asp:SqlDataSource>