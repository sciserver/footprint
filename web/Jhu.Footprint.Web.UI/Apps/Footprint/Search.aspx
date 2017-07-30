<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.Search" %>

<%@ Register Src="~/Apps/Footprint/SearchForm.ascx" TagPrefix="uc1" TagName="SearchForm" %>
<%@ Register Src="~/Apps/Footprint/FootprintList.ascx" TagPrefix="uc1" TagName="FootprintList" %>
<%@ Register Src="~/Apps/Footprint/FootprintRegionList.ascx" TagPrefix="uc1" TagName="FootprintRegionList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="toolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middle" runat="server">
    <asp:ScriptManagerProxy runat="server">
        <Scripts>
            <asp:ScriptReference Path="Search.aspx.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <uc1:SearchForm runat="server" ID="searchForm" OnClick="searchForm_Click" />

    <div class="container">
        <div class="dock-fill">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <uc1:FootprintList runat="server" id="footprintList" Visible="false" />
                    <uc1:FootprintRegionList runat="server" id="footprintRegionList" Visible="false" />

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
