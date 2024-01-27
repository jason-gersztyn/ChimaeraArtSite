<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Master/Chimaera.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="ChimaeraArtSite.Shop.Checkout" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/Controls/CartGrid.ascx" TagPrefix="CA" TagName="CartGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Verify Shipping Address</h2>

    <table class="addressTable" style="margin-top:50px;">
        <tr>
            <td>Recipient First Name:
            </td>
            <td>
                <asp:TextBox ID="shipFName" runat="server" CssClass="orderBox" AutoCompleteType="FirstName" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="firstNameValidator" runat="server" ControlToValidate="shipFName" ErrorMessage="First name is required" ForeColor="Red" ValidationGroup="Shipping" />
            </td>
        </tr>
        <tr>
            <td>Recipient Last Name:
            </td>
            <td>
                <asp:TextBox ID="shipLName" runat="server" CssClass="orderBox" AutoCompleteType="LastName" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="lastNameValidator" runat="server" ControlToValidate="shipLName" ErrorMessage="Last name is required" ForeColor="Red" ValidationGroup="Shipping" />
            </td>
        </tr>
        <tr>
            <td>Shipping Address 1:
            </td>
            <td>
                <asp:TextBox ID="shipAddress1" runat="server" CssClass="orderBox" AutoCompleteType="HomeStreetAddress" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="address1Validator" runat="server" ControlToValidate="shipAddress1" ErrorMessage="Address line 1 is required" ForeColor="Red" ValidationGroup="Shipping" />
            </td>
        </tr>
        <tr>
            <td>Shipping Address 2:
            </td>
            <td>
                <asp:TextBox ID="shipAddress2" runat="server" CssClass="orderBox" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td>Shipping City:
            </td>
            <td>
                <asp:TextBox ID="shipCity" runat="server" CssClass="orderBox" AutoCompleteType="HomeCity" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="cityValidator" runat="server" ControlToValidate="shipCity" ErrorMessage="City is required" ForeColor="Red" ValidationGroup="Shipping" />
            </td>
        </tr>
        <tr>
            <td>Postal Code:
            </td>
            <td>
                <asp:TextBox ID="shipZip" runat="server" CssClass="orderBox" OnTextChanged="shipZip_TextChanged" AutoPostBack="true" AutoCompleteType="HomeZipCode" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="zipValidator" runat="server" ControlToValidate="shipZip" ErrorMessage="Postal code is required" ForeColor="Red" ValidationGroup="Shipping" />
            </td>
        </tr>
        <tr>
            <td>Shipping Country:
            </td>
            <td>
                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="orderDropdown" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="">--Select--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="countryValidator" runat="server" ControlToValidate="ddlCountry" ErrorMessage="Country is required" ForeColor="Red" InitialValue="--Select--" ValidationGroup="Shipping" />
            </td>
        </tr>
        <tr>
            <td>Shipping State:
            </td>
            <td>
                <asp:DropDownList ID="ddlStates" runat="server" CssClass="orderDropdown" Visible="false">
                    <asp:ListItem>--Select--</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="shipState" runat="server" CssClass="orderBox" AutoCompleteType="HomeState" />
            </td>
            <td>
                <asp:Label ID="lState" runat="server" ForeColor="Red" Text="" />
            </td>
        </tr>
        <tr>
            <td>Email:
            </td>
            <td>
                <asp:TextBox ID="shipEmail" runat="server" CssClass="orderBox" AutoCompleteType="Email" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="emailValidator" runat="server" ControlToValidate="shipEmail" ErrorMessage="Email is required" ForeColor="Red" ValidationGroup="Shipping" />
            </td>
        </tr>
        <tr>
            <td>Phone:
            </td>
            <td>
                <asp:TextBox ID="shipPhone" runat="server" CssClass="orderBox" AutoCompleteType="HomePhone" />
            </td>
        </tr>
    </table>
    <br />
    <asp:Button ID="btConfirm" runat="server" Text="Confirm" CssClass="chimButton" OnClick="btConfirm_Click" ValidationGroup="Shipping" />
    <br />
    <asp:Label ID="lError" runat="server" Text="" ForeColor="Red" />
    <br />
    <CA:CartGrid ID="checkoutCart" runat="server" />
</asp:Content>
