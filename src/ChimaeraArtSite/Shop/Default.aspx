<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ChimaeraArtSite.Shop.Default" MasterPageFile="~/Master/Chimaera.Master" Title="Shop" %>
<%@ Register Src="~/Controls/ShopGrid.ascx" TagName="ShopGrid" TagPrefix="CA" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <CA:ShopGrid ID="shopGrid" runat="server" />
</asp:Content>