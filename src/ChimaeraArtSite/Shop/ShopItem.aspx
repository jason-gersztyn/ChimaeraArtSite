<%@ Page Title="Product" Language="C#" MasterPageFile="~/Master/Chimaera.Master" AutoEventWireup="true" CodeBehind="ShopItem.aspx.cs" Inherits="ChimaeraArtSite.Shop.ShopItem" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .shopItemPreview img {
            -webkit-transition: all 1s ease;
            -moz-transition: all 1s ease;
            -o-transition: all 1s ease;
            -ms-transition: all 1s ease;
            transition: all 1s ease;
        }

            .shopItemPreview img:hover {
                -webkit-transform: scale(2); /* Safari and Chrome */
                -moz-transform: scale(2); /* Firefox */
                -ms-transform: scale(2); /* IE 9 */
                -o-transform: scale(2); /* Opera */
                transform: scale(2);
            }

            .variant:hover {
                cursor:pointer;
            }
    </style>
    <script type="text/javascript">
        function VariantSelect(ID) {
            $.ajax({
                type: "POST",
                url: "ShopServices.asmx/FetchVariationData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ "ID": ID })
            })
            .success(function (data) {
                var jsdata = data.d;

                var nameLabel = $(<%= lName.ClientID %>);
                var proofImage = $(<%= iDesign.ClientID %>);
                var chartimage = $(<%= iSizeChart.ClientID %>);
                var linkchart = $(<%= hlSizeChart.ClientID %>);
                var priceLabel = $(<%= lPrice.ClientID %>);
                var hdnVar = document.getElementById('<%= hdnVariation.ClientID %>');
                var ddlSize = $(<%= ddlSize.ClientID %>);
                var bulletList = $(<%= blMaterial.ClientID %>);

                nameLabel.text(jsdata.Name);
                proofImage.attr("src", jsdata.ProofURL);
                chartimage.attr("src", jsdata.SizeChartURL);
                linkchart.attr("href", jsdata.SizeChartURL);
                priceLabel.text('$' + jsdata.UnitPrice);
                hdnVar.value = jsdata.VariationID;
                ddlSize.empty();
                bulletList.empty();
                $.each(jsdata.Sizes, function (key, value) {
                    ddlSize.append($("<option></option>").val(value).html(key))
                });
                jsdata.MaterialInfo.split(";").forEach(function(bullet) {
                    bulletList.append("<li>" + bullet + "</li>");
                });
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

            <asp:HiddenField ID="designID" runat="server" Visible="false" Value="0" />
            <table class="itemTable">
                <tr>
                    <td class="shopItemPreview">
                        <div class="large"></div>
                        <asp:Image ID="iDesign" runat="server" CssClass="shopItemImage" />
                        <br />
                        <asp:HiddenField ID="hdnVariation" runat="server" />
                    </td>
                    <td class="shopItemSpecs">
                        <asp:Label ID="lName" runat="server" CssClass="shopItemName" Text="" />
                        <br />
                        <br />
                        <asp:Label ID="lPrice" runat="server" CssClass="shopItemPrice" Text="" />
                        <br />
                        <hr />
                        Size:
                        <asp:DropDownList ID="ddlSize" runat="server" CssClass="shopItemSize" />
                        <br />
                        <asp:HyperLink ID="hlSizeChart" runat="server">
                            <asp:Image ID="iSizeChart" runat="server" CssClass="shopItemChart" />
                        </asp:HyperLink>
                        <hr />
                        <br />
                        <asp:Label ID="lDescription" runat="server" CssClass="shopItemDescription" Text="" />
                        <br />
                        <asp:BulletedList ID="blMaterial" runat="server" />
                        <br />
                        <asp:Button ID="btAdd" runat="server" OnClick="btAdd_Click" Text="Add to Cart" CssClass="chimButton" />
                        <br />
                        <asp:Label ID="lMessage" runat="server" ForeColor="Red" Text="" />
                    </td>
                </tr>
            </table>
            <asp:ListView runat="server" ID="VariationView" ItemPlaceholderID="itemPlaceHolder" GroupPlaceholderID="groupPlaceHolder" GroupItemCount="6">
                <LayoutTemplate>
                    <table runat="server" id="tblImages" style="width: 580px; margin-left: 60px;">
                        <tr runat="server" id="groupPlaceholder"></tr>
                    </table>
                </LayoutTemplate>

                <GroupTemplate>
                    <tr runat="server" id="imageRow" style="width: 100%;">
                        <td runat="server" id="itemPlaceholder"></td>
                    </tr>
                </GroupTemplate>

                <ItemTemplate>
                    <td runat="server" id="imageCell">
                        <img id='<%# "imagepane" + Eval("ProductVariationID") %>' class="variant" width="75" height="75" src='<%# Eval("ProofURL") %>' title='<%# Eval("Color") %>'
                            onclick='<%# CreateClick(Eval("ProductVariationID").ToString()) %>' />
                        <br />
                    </td>
                </ItemTemplate>
                <EmptyItemTemplate>
                    <td />
                </EmptyItemTemplate>
                <EmptyDataTemplate>
                    <h3>ACCESS DENIED</h3>
                </EmptyDataTemplate>
            </asp:ListView>
</asp:Content>
