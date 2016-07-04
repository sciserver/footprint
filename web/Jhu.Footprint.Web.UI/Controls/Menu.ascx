<%@ Control Language="C#" AutoEventWireup="true" Inherits="Jhu.Footprint.Web.UI.Controls.Menu" Codebehind="Menu.ascx.cs" %>

<jgwc:Toolbar runat="server" SkinID="Menu">
    <jgwc:ToolbarButton ID="Home" runat="server" Text="home" SkinID="Menu" />
    <jgwc:ToolbarButton ID="Search" runat="server" Text="search" SkinID="Menu" />
    <jgwc:ToolbarButton ID="Edit" runat="server" Text="edit" SkinID="Menu" />
    <jgwc:ToolbarButton ID="MyFootprint" runat="server" Text="my footprints" SkinID="Menu" />
    <jgwc:ToolbarButton ID="Api" runat="server" Text="api" SkinID="Menu" />
    <jgwc:ToolbarButton ID="Plot" runat="server" Text="plot" SkinID="Menu" />
    <jgwc:ToolbarButton ID="Docs" runat="server" Text="docs" SkinID="Menu" />
    <jgwc:ToolbarSpan runat="server" SkinID="Menu" />
</jgwc:Toolbar>
