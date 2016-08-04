<%@ Page Title="" Language="C#" MasterPageFile="~/UI.master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Search" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="toolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middle" runat="server">    
    <asp:ScriptManagerProxy runat="server">
        <Scripts>
            <asp:ScriptReference Path="Search.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <div class="container">
        
        <asp:HiddenField runat="server" id="SearchMethod" Value="NameSearch" ClientIDMode="Static"/>

        <div class="dock-fill">
            <h1>Footprint Catalog Search</h1>
            <div class="row">
                <div class="col-sm-12">
                    <ul runat="server" id="SearchTabContainer" class="nav nav-tabs">
                        <li class="active"><a id="NameSearch" data-toggle="tab" href="#NameSearchPanel">Name</a></li>
                        <li><a ID="PointSearch" data-toggle="tab" href="#PointSearchPanel">Coordinate</a></li>
                        <li><a data-toggle="tab" href="#ConeSearchPanel">Cone</a></li>
                        <li><a data-toggle="tab" href="#IntersectSearchPanel">Intersect</a></li>
                        <li><a data-toggle="tab" href="#ContainSearchPanel">Contain</a></li>
                    </ul>

                    <div class="tab-content col-sm-6">
                        <div id="NameSearchPanel" class="tab-pane fade in active">
                            <asp:Label ID="nameLabel" runat="server" Text="Name:" CssClass="control-label"></asp:Label>
                            <asp:TextBox runat="server" ID="name" CssClass="form-control" />

                            <%--<asp:Button ID="NameSearch" runat="server" OnClick="ok_Click" Text="search" />--%>
                        </div>
                        <div id="PointSearchPanel" class="tab-pane fade">
                            <asp:Label ID="PointRALabel" runat="server" Text="Ra:" CssClass="control-label"></asp:Label>
                            <asp:TextBox runat="server" ID="PointRAInput" CssClass="form-control" />
                            <asp:Label ID="PointDecLabel" runat="server" Text="Dec:" CssClass="control-label"></asp:Label>
                            <asp:TextBox runat="server" ID="PointDecInput" CssClass="form-control" />

                            <%--<asp:Button ID="PointSearch" runat="server" OnClick="ok_Click" Text="search" />--%>
                        </div>

                        <div id="ConeSearchPanel" class="tab-pane fade">
                            <asp:Label ID="ConeRALabel" runat="server" Text="Ra:" CssClass="control-label"></asp:Label>
                            <asp:TextBox runat="server" ID="ConeRAInput" CssClass="form-control" />
                            <asp:Label ID="ConeDecLabel" runat="server" Text="Dec:" CssClass="control-label"></asp:Label>
                            <asp:TextBox runat="server" ID="ConeDecInput" CssClass="form-control" />
                            <asp:Label ID="ConeRadiusLabel" runat="server" Text="Radius:" CssClass="control-label"></asp:Label>
                            <asp:TextBox runat="server" ID="ConeRadiusInput" CssClass="form-control" />

                            <asp:Button ID="ConeSearch" runat="server" OnClick="ok_Click" Text="search" />
                        </div>
                        <div id="IntersectSearchPanel" class="tab-pane fade">
                            TODO
                            
            <asp:Button ID="IntersectSearch" runat="server" OnClick="ok_Click" Text="search" />
                        </div>
                        <div id="ContainSearchPanel" class="tab-pane fade">
                            TODO
                            
            <asp:Button ID="ContainSearch" runat="server" OnClick="ok_Click" Text="search" />
                        </div>
                    </div>
                </div>
            </div>

            <asp:Button ID="ok" runat="server" OnClick="ok_Click" Text="search" />

            <br />

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
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
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>
</asp:Content>
