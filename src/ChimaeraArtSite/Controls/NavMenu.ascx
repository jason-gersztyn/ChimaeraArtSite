<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NavMenu.ascx.cs" Inherits="ChimaeraArtSite.Controls.NavMenu" %>

<div class="navMenu">
    <asp:HyperLink ID="hlHome" runat="server" NavigateUrl="~/Default.aspx" CssClass="navItem" Text="Home" /> - 
    <asp:HyperLink ID="hlContact" runat="server" NavigateUrl="~/About/Default.aspx" CssClass="navItem" Text="Contact" /> -
    <asp:HyperLink ID="hlShop" runat="server" NavigateUrl="~/Shop/Default.aspx" CssClass="navItem" Text="Shop" />
</div>