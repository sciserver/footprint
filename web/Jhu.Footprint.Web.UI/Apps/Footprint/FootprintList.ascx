<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FootprintList.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.FootprintList" %>
<asp:ObjectDataSource ID="dataSource" runat="server" OnObjectCreating="dataSource_ObjectCreating"
    SelectCountMethod="Count" SelectMethod="Find" TypeName="Jhu.Footprint.Web.Lib.FootprintSearch"
    DataObjectTypeName="Jhu.Footprint.Web.Lib.Footprint" />
<asp:ListView ID="listView" runat="server" DataSourceID="dataSource">
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
            <%# Eval("Name") %><br />
            <asp:Image runat="server" ImageUrl='<%# String.Format("../../Api/V1/Footprint.svc/users/{0}/footprints/{1}/thumbnail", Eval("Owner"), Eval("Name"), Eval("Name"))%>' /><br />
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        No footprints found.
    </EmptyDataTemplate>
</asp:ListView>
