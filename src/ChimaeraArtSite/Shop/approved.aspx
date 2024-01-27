<%@ Page Language="C#" Title="Final Review" AutoEventWireup="true" MasterPageFile="~/Master/Chimaera.Master" CodeBehind="approved.aspx.cs" Inherits="ChimaeraArtSite.Shop.approved" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:HiddenField ID="payerid" runat="server" />
    <asp:HiddenField ID="paymentid" runat="server" />

    <h2>Please review your order one last time</h2>
    <asp:ListView runat="server" ID="CartView" ItemPlaceholderID="itemPlaceHolder" GroupPlaceholderID="groupPlaceHolder" GroupItemCount="4">
        <LayoutTemplate>
            <table runat="server" id="tblCart" style="margin: 0px auto;">
                <tr runat="server" id="groupPlaceholder"></tr>
            </table>
        </LayoutTemplate>

        <GroupTemplate>
            <tr runat="server" id="cartRow" class="imageRow">
                <td runat="server" id="itemPlaceholder"></td>
            </tr>
        </GroupTemplate>

        <ItemTemplate>
            <td runat="server" id="cartCell" class="imageCell">
                <asp:Image ID="cartImage" runat="server" CssClass="imageThumb" ImageUrl='<%# Eval("ImageLocation") %>' /><br />
                <asp:Label ID="cartSelection" runat="server" CssClass="cartLabel" Text='<%# Eval("ProductSize") %>' /><br />
                <asp:Label ID="cartSummary" runat="server" CssClass="cartLabel" Text='<%# "$" + ((decimal)Eval("UnitPrice")).ToString("0.00") + " * " + Eval("Quantity") + " = $" + ((decimal)Eval("Subtotal")).ToString("0.00") %>' />
            </td>
        </ItemTemplate>
    </asp:ListView>
    <div style="font-weight:bold;">
        <hr />
        <br />
        <asp:Label ID="lSubTotal" runat="server" Text="Subtotal: $" CssClass="costSummaryLabel" />
        <br />
        <asp:Label ID="lDiscount" runat="server" Text="Discount: $" CssClass="costSummaryLabel" Visible="false" ForeColor="Red" />
        <br />
        <asp:Label ID="lShipTotal" runat="server" Text="S&H: $" CssClass="costSummaryLabel" />
        <br />
        <asp:Label ID="lTotal" runat="server" Text="Total: $" CssClass="costSummaryLabel" />
    </div>
    <br />
    <br />
    <asp:Button ID="completeSale" runat="server" OnClick="completeSale_Click" Text="Complete Purchase" CssClass="chimButton" />
</asp:Content>
