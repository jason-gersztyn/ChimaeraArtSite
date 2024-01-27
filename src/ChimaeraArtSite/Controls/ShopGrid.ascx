<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopGrid.ascx.cs" Inherits="ChimaeraArtSite.Controls.ShopGrid" %>

<asp:ListView runat="server" ID="ShopListView" ItemPlaceholderID="itemPlaceHolder" GroupPlaceholderID="groupPlaceHolder" GroupItemCount="4">
    <LayoutTemplate>
        <h1>
            <asp:Label Text="Shop" runat="server" ID="titleLabel" />

        </h1>
        <table runat="server" id="tblImages" class="itemTable">
            <tr runat="server" id="groupPlaceholder"></tr>
        </table>
    </LayoutTemplate>

    <GroupTemplate>
        <tr runat="server" id="imageRow" class="imageRow">
            <td runat="server" id="itemPlaceholder"></td>
        </tr>
    </GroupTemplate>

    <ItemTemplate>
        <td runat="server" id="imageCell" class="imageCell">
            <a id="shopLink" runat="server" href='<%# "~/Shop/ShopItem.aspx?SeriesID=" + Eval("ProductSeriesID") %>' class="imageLink">
                <asp:Image ID="imagePane" runat="server" CssClass="imageThumb" ImageUrl='<%# Eval("ImageURL") %>' /><br />
                <asp:Label ID="imageName" runat="server" CssClass="imageName" Text='<%# Eval("ProductSeriesName") %>' />
            </a>
        </td>
    </ItemTemplate>
    <EmptyItemTemplate>
        <td />
    </EmptyItemTemplate>
    <EmptyDataTemplate>
        <h3>No images here</h3>
    </EmptyDataTemplate>
</asp:ListView>

<asp:DataPager ID="dpPages" runat="server" PagedControlID="ShopListView" PageSize="16">
    <Fields>
        <asp:NumericPagerField ButtonType="Link" />
    </Fields>
</asp:DataPager>