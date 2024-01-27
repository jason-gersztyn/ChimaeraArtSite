<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="ChimaeraArtSite.Controls.Header" %>
<%@ Register Src="~/Controls/NavMenu.ascx" TagName="NavMenu" TagPrefix="CA" %>

<div class="header">
    <div style="padding-top:150px;">
        <h1>Chimaera Conspiracy</h1>
        <CA:NavMenu ID="navMenu" runat="server" />
        <asp:HyperLink ID="cartLink" runat="server" ImageUrl="~/Images/Icons/cart.png" ImageHeight="40px" ImageWidth="40px" NavigateUrl="~/Shop/Cart.aspx" CssClass="cartImage" />
    </div>
</div>