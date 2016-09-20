<%@ Page Language="C#" MasterPageFile="~/UI.master" AutoEventWireup="true" CodeBehind="MyFootprint.aspx.cs" Inherits="Jhu.Footprint.Web.UI.MyFootprint" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="toolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middle" runat="server">
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
                    <td >
                            <%# Eval("FootprintName") %></br>
                            <%# Eval("Name") %></br>

                            <img src='<%# String.Format("http://localhost/footprint/api/v1/Footprint.svc/users/{0}/footprints/{1}/regions/{2}/thumbnail", Eval("FootprintOwner"), Eval("FootprintName"),Eval("Name"))%>' alt="" /><br />

                            <asp:Button runat="server" Text="Edit" CssClass="btn btn-default" />

                            <asp:Button runat="server" Text="Delete" CssClass="btn btn-danger" />
                        
                    </td>
                </ItemTemplate>
                <EmptyDataTemplate>
                    No footprints found.
                </EmptyDataTemplate>
            </asp:ListView>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
