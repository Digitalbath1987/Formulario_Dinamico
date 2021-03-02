<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Default.Master" CodeBehind="Default.aspx.cs" Inherits="ALO.WebSite.Default1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UPDATE_PANEL_CONTENEDOR" runat="server" >
    <ContentTemplate>

        <%--===========================================================================================================--%>
        <%--===========================================================================================================--%>
        <div class="container">
        
            <%--=======================================================================================================--%>
            <%--=======================================================================================================--%>
            <div class="row">
                <asp:Panel ID="PNL_Mensaje"  CssClass="alert alert-warning" runat="server" Visible = "false">
                    <strong> ADVERTENCIA! </strong> <asp:Label ID="LBL_Mensaje" runat="server" Text=""></asp:Label>
                </asp:Panel>
            </div>

        </div>


    </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

