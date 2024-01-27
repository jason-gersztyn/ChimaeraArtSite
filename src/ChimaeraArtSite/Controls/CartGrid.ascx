<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartGrid.ascx.cs" Inherits="ChimaeraArtSite.Controls.CartGrid" %>

<h1>
    <asp:Label Text="Cart" runat="server" ID="titleLabel" />
</h1>

<div style="margin-bottom:20px">
     Cart Subtotal: <asp:Label ID="cartSub" runat="server" CssClass="cartLabel" Text="" /><br />
     <asp:Label ID="discSub" runat="server" CssClass="cartLabel" Text="" ForeColor="Red" />
</div>
<asp:ListView runat="server" ID="CartView" ItemPlaceholderID="itemPlaceHolder" GroupPlaceholderID="groupPlaceHolder" GroupItemCount="4">
    <LayoutTemplate>
        <table runat="server" id="tblCart" class="itemTable">
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
            <asp:Label ID="cartSummary" runat="server" CssClass="cartLabel" Text='<%# "$" + ((decimal)Eval("UnitPrice")).ToString("0.00") + " * " + Eval("Quantity") + " = $" + ((decimal)Eval("Subtotal")).ToString("0.00") %>' /><br />
            <asp:LinkButton ID="cartRemove" runat="server" CssClass="cartRemove" Text="Remove from Cart" CommandName="Remove" CommandArgument='<%# Eval("CartItemID") %>' OnClick="cartRemove_Click" />
        </td>
    </ItemTemplate>
    <EmptyDataTemplate>
        <h3>
            The truth is out there, but it's not free. Support the cause, buy a T-shirt.
        </h3>
    </EmptyDataTemplate>
</asp:ListView>