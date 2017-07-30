<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FootprintRegionList.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.FootprintRegionList" %>
<asp:ObjectDataSource ID="footprintRegionDataSource" runat="server" OnObjectCreating="dataSource_ObjectCreating" SelectCountMethod="Count" SelectMethod="Find" TypeName="Jhu.Footprint.Web.Lib.FootprintRegionSearch"
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
