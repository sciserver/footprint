<%@ Page Title="" Language="C#" MasterPageFile="~/UI.master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="toolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middle" runat="server">
    <div class="container">
        <div class="dock-fill">
            <h1>Footprint Catalog Search</h1>
            <div class="row">
                <div class="col-sm-12">
                    <div id="ObjectSearchDiv" class="form-group col-sm-6">
                        <asp:Label ID="nameLabel" runat="server" Text="Name:" CssClass="control-label"></asp:Label>
                        <asp:TextBox runat="server" ID="name" CssClass="form-control" />

                    </div>
                    <div id="PointSearchDiv" class="hidden">
                        <asp:Label ID="raLabel" runat="server" Text="Name:" CssClass="control-label"></asp:Label>
                        <asp:TextBox runat="server" ID="raInput" CssClass="form-control" />
                        <asp:Label ID="decLabel" runat="server" Text="Name:" CssClass="control-label"></asp:Label>
                        <asp:TextBox runat="server" ID="decInput" CssClass="form-control" />
                    </div>

                </div>
            </div>

            <br />
            <asp:Button ID="ok" runat="server" OnClick="ok_Click" Text="search" />

            <asp:ObjectDataSource ID="footprintRegionDataSource" runat="server" OnObjectCreating="footprintRegionDataSource_ObjectCreating" SelectCountMethod="Count" SelectMethod="Find" TypeName="Jhu.Footprint.Web.Lib.FootprintRegionSearch"
                DataObjectTypeName="Jhu.Footprint.Web.Lib.FootprintRegion" />
            <asp:ListView ID="footprintList" runat="server" DataSourceID="footprintRegionDataSource" Visible="False">
                <LayoutTemplate>
                    <table>
                        <asp:PlaceHolder runat="server" ID="groupPlaceholder" />
                    </table>
                </LayoutTemplate>
                <GroupTemplate>
                    <tr>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tr>
                </GroupTemplate>
                <ItemTemplate>
                    <td>
                        <%# Eval("Name") %>
                    </td>
                </ItemTemplate>
                <EmptyDataTemplate>
                    No footprints found.
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
    </div>
</asp:Content>
