<%@ Page Language="C#" MasterPageFile="~/UI.master" AutoEventWireup="true" CodeBehind="MyRegion.aspx.cs" Inherits="Jhu.Footprint.Web.UI.MyRegion" %>

<asp:Content ID="MyRegion" ContentPlaceHolderID="middle" runat="server">
    <div class="container">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:ObjectDataSource ID="footprintRegionDataSource" runat="server" OnObjectCreating="footprintRegionDataSource_ObjectCreating" SelectCountMethod="Count" SelectMethod="Find" TypeName="Jhu.Footprint.Web.Lib.FootprintRegionSearch"
                    DataObjectTypeName="Jhu.Footprint.Web.Lib.FootprintRegion" />
                <asp:ListView ID="regionList" runat="server" DataSourceID="footprintRegionDataSource" Visible="False" ClientIDMode="Static">
                    <LayoutTemplate>
                        <table>
                            <div class="h1"><%# Eval("FootprintName") %></div>
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

                            <div class="row">
                                <div class="col-sm-10"><%# Eval("Name") %></div>
                                <div class="checkbox col-sm-2 hidden DeleteFootprints">
                                    <input type="checkbox" name="deleteCheckbox" value='<%# Eval("FootprintName") %>'>
                                </div>
                                </br>                                    
                                        <img src='<%# String.Format("http://localhost/footprint/api/v1/Footprint.svc/users/{0}/footprints/{1}/regions/{2}/thumbnail", Eval("FootprintOwner"), Eval("FootprintName"),Eval("Name"))%>' /><br />
                            </div>
                        </td>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        No footprints found.
                    </EmptyDataTemplate>
                </asp:ListView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
