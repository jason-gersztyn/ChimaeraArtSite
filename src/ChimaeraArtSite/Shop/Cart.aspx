<%@ Page Title="Cart" Language="C#" MasterPageFile="~/Master/Chimaera.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="ChimaeraArtSite.Shop.Cart" %>
<%@ Register Src="~/Controls/CartGrid.ascx" TagPrefix="CA" TagName="CartGrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <CA:CartGrid ID="cgCurrent" runat="server" />
    <br />
    <div style="border:1px solid black;width:350px;margin:0 auto;padding:12px;">
        <h4>Have a discount code? Apply it now!</h4>
        <asp:TextBox ID="tbCode" runat="server" CssClass="orderBox" />
        <br />
        <br />
        <asp:Button ID="btDiscount" runat="server" OnClick="btDiscount_Click" Text="Apply Discount" CssClass="chimButton" />
        <br />
        <asp:Label ID="lDiscount" runat="server" Text="" ForeColor="Maroon" />
    </div>
    <br />
    <asp:Button ID="btCheckout" runat="server" OnClick="btCheckout_Click" Text="Check Out" CssClass="chimButton" />
    <br />
    <asp:Label ID="lStatus" runat="server" Text="" ForeColor="Maroon" />
</asp:Content>
