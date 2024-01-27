<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Support.aspx.cs" Inherits="ChimaeraArtSite.About.Support" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>From:</td>
                <td><asp:TextBox ID="tbFrom" runat="server" /></td>
            </tr>
            <tr>
                <td>Title:</td>
                <td><asp:TextBox ID="tbTitle" runat="server" /></td>
            </tr>
            <tr>
                <td>Message:</td>
                <td><asp:TextBox ID="tbMessage" runat="server" TextMode="MultiLine" /></td>
            </tr>
        </table>
        <asp:Button ID="spSend" runat="server" Text="Send" />
        <asp:Button ID="spCancel" runat="server" Text="Cancel" />
    </form>
</body>
</html>
