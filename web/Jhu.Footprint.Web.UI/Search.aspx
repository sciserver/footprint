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

        <asp:HiddenField runat="server" ID="SearchMethod" Value="Name" ClientIDMode="Static" />

        <div class="dock-fill">
            <h1>Footprint Catalog Search</h1>

            <!-- TODO: Implement fast search (wire with the NED and/or Simbad service) -->
            <div class="row" id="FastSearchPanel">
                <div class="col-sm-6">
                    <asp:TextBox runat="server" ID="FastSearchInput" CssClass="form-control"></asp:TextBox>

                </div>
            </div>

            <div class="row">
                <div id="AdvancedSearchOptionsPanel" class="hidden">

                    <div class="row">
                        <div class="col-sm-6">
                            <ul runat="server" id="SearchTabContainer" class="nav nav-tabs">
                                <li class="active"><a id="NameSearch" data-toggle="tab" href="#NameSearchPanel">Name</a></li>
                                <li><a id="PointSearch" data-toggle="tab" href="#PointSearchPanel">Coordinate</a></li>
                                <li><a id="ConeSearch" data-toggle="tab" href="#ConeSearchPanel">Cone</a></li>
                                <li><a id="IntersectSearch" data-toggle="tab" href="#IntersectSearchPanel">Intersect</a></li>
                                <li><a id="ContainSearch" data-toggle="tab" href="#ContainSearchPanel">Contain</a></li>
                            </ul>

                        </div>
                    </div>
                    <div class="row">
                        <div class="tab-content col-sm-6">
                            <div id="NameSearchPanel" class="tab-pane fade in active">
                                <asp:Label ID="nameLabel" runat="server" Text="Name:" CssClass="control-label"></asp:Label>
                                <asp:TextBox runat="server" ID="name" CssClass="form-control" />
                            </div>
                            <div id="PointSearchPanel" class="tab-pane fade">
                                <asp:Label ID="PointRALabel" runat="server" Text="Ra:" CssClass="control-label"></asp:Label>
                                <asp:TextBox runat="server" ID="PointRAInput" CssClass="form-control" />
                                <asp:Label ID="PointDecLabel" runat="server" Text="Dec:" CssClass="control-label"></asp:Label>
                                <asp:TextBox runat="server" ID="PointDecInput" CssClass="form-control" />
                            </div>

                            <div id="ConeSearchPanel" class="tab-pane fade">
                                <asp:Label ID="ConeRALabel" runat="server" Text="Ra:" CssClass="control-label"></asp:Label>
                                <asp:TextBox runat="server" ID="ConeRAInput" CssClass="form-control" />
                                <asp:Label ID="ConeDecLabel" runat="server" Text="Dec:" CssClass="control-label"></asp:Label>
                                <asp:TextBox runat="server" ID="ConeDecInput" CssClass="form-control" />
                                <asp:Label ID="ConeRadiusLabel" runat="server" Text="Radius:" CssClass="control-label"></asp:Label>
                                <asp:TextBox runat="server" ID="ConeRadiusInput" CssClass="form-control" />
                            </div>

                            <div id="IntersectSearchPanel" class="tab-pane fade">
                                <asp:Label runat="server" Text="Region Description:" CssClass="control-label"></asp:Label>
                                <asp:TextBox runat="server" ID="IntersectRegion" TextMode="MultiLine" Rows="5" Columns="50" CssClass="form-control">CIRCLE J2000 170 13 120</asp:TextBox>
                            </div>
                            <div id="ContainSearchPanel" class="tab-pane fade">
                                <asp:Label runat="server" Text="Region Description:" CssClass="control-label"></asp:Label>
                                <asp:TextBox runat="server" ID="ContainRegion" TextMode="MultiLine" Rows="5" Columns="50" CssClass="form-control">CIRCLE J2000 170 5 200</asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <br />

            <div class="row">
                <div class="col-sm-6">
                    <label class="checkbox-inline">
                        <input type="checkbox" aria-label="AdvancedSearchOptions" id="AdvancedSearchOptionsToggle">Advanced Search</label>
                    <label class="checkbox-inline">
                        <asp:CheckBox runat="server" ID="RegionSearchToggle"></asp:CheckBox>Region</label>
                    <br />
                </div>
            </div>

            <br />

            <asp:Button ID="ok" runat="server" OnClick="ok_Click" Text="search" CssClass="btn btn-primary" />

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:ObjectDataSource ID="footprintRegionDataSource" runat="server" OnObjectCreating="footprintRegionDataSource_ObjectCreating" SelectCountMethod="Count" SelectMethod="Find" TypeName="Jhu.Footprint.Web.Lib.FootprintRegionSearch"
                        DataObjectTypeName="Jhu.Footprint.Web.Lib.FootprintRegion" />
                    <asp:ListView ID="regionList" runat="server" DataSourceID="footprintRegionDataSource" Visible="False">
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
                            <tr>
                                <%# Eval("FootprintName") %><br />
                                <%# Eval("Name") %><br />
                                <img src='<%# String.Format("http://localhost/footprint/api/v1/Footprint.svc/users/{0}/footprints/{1}/regions/{2}/thumbnail", Eval("FootprintOwner"), Eval("FootprintName"),Eval("Name"))%>' alt="" /><br />
                            </tr>

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
