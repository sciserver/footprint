<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI.master" CodeBehind="FootprintPlot.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Plot.FootprintPlot" %>


<asp:Content ID="plot" ContentPlaceHolderID="middle" runat="server">
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <table class="block">
                    <tr>
                        <td class="block_canvas">
                            <asp:Image ID="PlotCanvas" runat="server" />
                        </td>

                        <td class="block_buttons">

                            <table>
                                <tr>
                                    <td>
                                        <b>Projection</b>
                                        <asp:RadioButtonList ID="plotProjectionStyle" runat="server" AutoPostBack="True">
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
                                        <asp:RadioButtonList runat="server" AutoPostBack="True" ID="plotDegreeStyle">
                                            <asp:ListItem Selected="True">Decimal</asp:ListItem>
                                            <asp:ListItem Value="Sexagesimal">HMS-DMS</asp:ListItem>
                                            <asp:ListItem>Galactic</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="plotGrid" runat="server" AutoPostBack="True" Checked="True" Text="Grid" />
                                        <asp:CheckBox ID="plotAutoZoom" runat="server" AutoPostBack="True" Text="Auto Zoom" />
                                        <asp:CheckBox ID="plotAutoRotate" runat="server" AutoPostBack="True" Text="Auto Rotate" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
