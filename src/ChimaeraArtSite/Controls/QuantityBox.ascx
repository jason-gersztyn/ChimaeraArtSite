<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuantityBox.ascx.cs" Inherits="ChimaeraArtSite.Controls.QuantityBox" %>

<script type="text/javascript">
    function addQuantity() {
        var quantityBox = document.getElementById('<%= tbQuantity.ClientID %>');
        var value = parseInt(quantityBox.value);
        quantityBox.value = value + 1;
    }

    function checkValue(quantityBox) {
        if ((quantityBox.value % 1) === 0) {
            var value = parseInt(quantityBox.value);
            if (value < 0)
                quantityBox.value = "0";
        }
        else {
            quantityBox.value = "0";
        }
    }

    function minusQuantity() {
        var quantityBox = document.getElementById('<%= tbQuantity.ClientID %>');
        var value = parseInt(quantityBox.value);
        quantityBox.value = value - 1;
    }
</script>

<asp:ImageButton ID="iMinus" runat="server" CssClass="qButton" OnClientClick="minusQuantity()" ImageUrl="~/Images/minus.png" />
<asp:TextBox ID="tbQuantity" runat="server" CssClass="qText" onchange="javascript: checkValue(this);" />
<asp:ImageButton ID="iAdd" runat="server" CssClass="qButton" OnClientClick="addQuantity()" ImageUrl="~/Images/plus.png" /> 