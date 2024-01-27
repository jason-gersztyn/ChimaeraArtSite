<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Master/Chimaera.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ChimaeraArtSite.About.Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>For inquiries or trouble tickets concerning a previous order, please contact us ASAP at <asp:LinkButton ID="hlSupport" runat="server">support@chimaeraconspiracy.com</asp:LinkButton>. Having your order number present in the e-mail will help expediate the process.</p>
    <br />
    <p>For all other general comments, let us know what you're thinking at our twitter handle. <a href="https://twitter.com/ChimConspiracy" class="twitter-follow-button" data-show-count="false" data-size="large" data-show-screen-name="false">Follow @ChimConspiracy</a>
    <script>!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0],p=/^http:/.test(d.location)?'http':'https';if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src=p+'://platform.twitter.com/widgets.js';fjs.parentNode.insertBefore(js,fjs);}}(document, 'script', 'twitter-wjs');</script></p>

    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="spPanel" TargetControlID="hlSupport" CancelControlID="spCancel" BackgroundCssClass="supportBackground"></cc1:ModalPopupExtender>

    <asp:Panel ID="spPanel" runat="server" align="Center" style="display:none;">
        <iframe style="width:350px;height:300px;" id="ifrm1" src="Support.aspx" runat="server"></iframe>
        <br />
        <asp:Button ID="spCancel" runat="server" Text="Cancel" />
    </asp:Panel>
</asp:Content>
