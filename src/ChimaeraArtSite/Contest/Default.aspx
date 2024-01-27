<%@ Page Title="Super Secret Contest Portal" Language="C#" MasterPageFile="~/Master/Chimaera.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ChimaeraArtSite.Contest.Default" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:TextBox ID="tbContestKey" runat="server" CssClass="orderBox" />
    <br />
    <br />
    <asp:Button ID="btSubmit" runat="server" CssClass="chimButton" OnClick="btSubmit_Click" Text="Submit Code" />
    <br />
    <asp:Label ID="lbResult" runat="server" ForeColor="Maroon" Text="" />
</asp:Content>
