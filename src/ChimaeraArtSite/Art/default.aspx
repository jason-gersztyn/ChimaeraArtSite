<%@ Page Title="Art" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ChimaeraArtSite.Art._default" MasterPageFile="~/Master/Chimaera.Master" %>
<%@ Register Src="~/Controls/ImageGrid.ascx" TagName="ImageGrid" TagPrefix="CA" %>

<asp:Content ID="content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <CA:ImageGrid ID="artPreviews" runat="server" />
</asp:Content>
