<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopSidebar.ascx.cs" Inherits="ChimaeraArtSite.Controls.ShopSidebar" %>

<div class="shopMenu">
    <asp:HyperLink ID="lHome" runat="server" Text="Home" NavigateUrl="~/Shop/Default.aspx" CssClass="shopMenuItem" />
    <asp:HyperLink ID="lTs" runat="server" Text="T-Shirts" NavigateUrl="~/Shop/Default.aspx?GenreID=1" CssClass="shopMenuItem" />
    <asp:HyperLink ID="lHoodies" runat="server" Text="Hoodies" NavigateUrl="~/Shop/Default.aspx?GenreID=2" CssClass="shopMenuItem" />
    <asp:HyperLink ID="lCheckout" runat="server" Text="Checkout" NavigateUrl="~/Shop/Checkout.aspx" CssClass="shopMenuItem" />
</div>
