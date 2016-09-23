<%@ Page Language="C#" MasterPageFile="~/UI.master" AutoEventWireup="true" CodeBehind="MyFootprint.aspx.cs" Inherits="Jhu.Footprint.Web.UI.MyFootprint" %>


<asp:Content ID="MyFootprint" ContentPlaceHolderID="middle" runat="server">
    <asp:ScriptManagerProxy runat="server">
        <Scripts>
            <asp:ScriptReference Path="Scripts/generalFunctions.js" />
            <asp:ScriptReference Path="MyFootprint.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <div class="container">
        <div class="row col-sm-12">
            <asp:Button runat="server" Text="Edit" CssClass="btn btn-default" />
            <%-- TODO: implement switch for delete footprint mode (toggling checkboxes and delete footprint(s) button visibility) --%>
            <button type="button" class="btn btn-default hidden">Delete Footprint(s)</button>
            <button type="button" id="DeleteFootprints" class="btn btn-danger">Delete Footprint(s)</button>
            
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
                            <td>
                                <%# Eval("FootprintName") %></br>
                        <%# Eval("Name") %></br>

                        <img src='<%# String.Format("http://localhost/footprint/api/v1/Footprint.svc/users/{0}/footprints/{1}/regions/{2}/thumbnail", Eval("FootprintOwner"), Eval("FootprintName"),Eval("Name"))%>' alt="" /><br />
                                </br>
                        <div class="checkbox">
                            <input type="checkbox" name="deleteCheckbox" value='<%# Eval("FootprintName") %>'>
                        </div>
                                </br>

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
