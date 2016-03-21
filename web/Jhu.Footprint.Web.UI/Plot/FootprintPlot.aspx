<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI.master" CodeBehind="FootprintPlot.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Plot.FootprintPlot" %>


<asp:Content ID="plot" ContentPlaceHolderID="middle" runat="server">
    <div>
        <table class="block">
            <tr>
                <td class="block_canvas">
                    <spherical:PlotCanvas runat="server" ID="canvas" Width="840" Height="450" CssClass="plot" />
                </td>

                <td class="block_buttons">

                    <table>
                        <tr>
                            <td>
                                <b>Projection</b>
                                <asp:RadioButtonList ID="plotProjectionStyle" runat="server">
                                    <asp:ListItem Selected="True">Aitoff</asp:ListItem>
                                    <asp:ListItem>Equirectangular</asp:ListItem>
                                    <asp:ListItem Value="HammerAitoff">Hammer Aitoff</asp:ListItem>
                                    <asp:ListItem>Mollweide</asp:ListItem>
                                    <asp:ListItem>Orthographic</asp:ListItem>
                                    <asp:ListItem>Stereographic</asp:ListItem>
                                </asp:RadioButtonList>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButtonList runat="server" AutoPostBack="True">
                                    <asp:ListItem Selected="True">Decimal</asp:ListItem>
                                    <asp:ListItem Value="Sexagesimal">HMS-DMS</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
