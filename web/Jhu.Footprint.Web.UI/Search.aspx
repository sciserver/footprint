<%@ Page Title="" Language="C#" MasterPageFile="~/UI.master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="toolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middle" runat="server">
    <div class="dock-fill">
        <h1>Footprint Catalog Search</h1>
        <table>
            <tr>
                <td>
                    <asp:Label ID="nameLabel" runat="server" Text="Name:"></asp:Label>
                </td>
                <td><asp:TextBox runat="server" ID="name" /></td>
            </tr>
        </table>
        <br />
        <asp:Button ID="ok" runat="server" OnClick="ok_Click" Text="search" />

        <asp:ObjectDataSource ID="footprintDataSource" runat="server" OnObjectCreating="footprintDataSource_ObjectCreating" SelectCountMethod="Count" SelectMethod="Find" TypeName="Jhu.Footprint.Web.Lib.FootprintFolderSearch"
             DataObjectTypeName="Jhu.Footprint.Web.Lib.FootprintFolder" />
        <asp:ListView ID="footprintList" runat="server" DataSourceID="footprintDataSource" Visible="False">
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
</asp:Content>
