<%@ Page Title="Contest Prize Selection" Language="C#" MasterPageFile="~/Master/Chimaera.Master" AutoEventWireup="true" CodeBehind="Prize.aspx.cs" Inherits="ChimaeraArtSite.Contest.Prize" %>
<%@ Register Src="~/Controls/PrizeGrid.ascx" TagName="PrizeGrid" TagPrefix="CA" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <CA:PrizeGrid ID="PrizeGrid" runat="server" />
</asp:Content>
