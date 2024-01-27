<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageGrid.ascx.cs" Inherits="ChimaeraArtSite.Controls.ImageGrid" %>

<asp:ListView runat="server" ID="ImageListView" ItemPlaceholderID="itemPlaceHolder" GroupPlaceholderID="groupPlaceHolder" GroupItemCount="4">
    <LayoutTemplate>
        <h1>
            <asp:Label Text="" runat="server" ID="titleLabel" OnLoad="titleLabel_Load" />

        </h1>
        <table runat="server" id="tblImages" class="imageTable">
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
            <asp:Image ID="imagePane" runat="server" CssClass="imageThumb" ImageUrl='<%# Eval("ImageURL") %>' /><br />
            <asp:Label ID="imageName" runat="server" CssClass="imageTitle" Text='<%# Eval("ImageName") %>' /><br />
            <asp:Label ID="imageDescription" runat="server" CssClass="imageDescription" Text='<%# Eval("ImageDescription") %>' />
        </td>
    </ItemTemplate>
    <EmptyItemTemplate>
        <td />
    </EmptyItemTemplate>
    <EmptyDataTemplate>
        <h3>No images here</h3>
    </EmptyDataTemplate>
</asp:ListView>

