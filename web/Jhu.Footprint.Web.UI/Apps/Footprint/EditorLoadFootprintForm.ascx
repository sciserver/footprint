<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditorLoadFootprintForm.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.LoadFootprintView" %>

<div id="LoadModal" class="modal" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                <h4 class="modal-title">Load Footprint</h4>
            </div>
            <div class="modal-body">

                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="form-group form-inline">
                            <label for="FootprintSelect" class="control-label">Footprint: </label>
                            <asp:DropDownList runat="server" ID="FootprintSelect" OnSelectedIndexChanged="FootprintSelect_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control">
                            </asp:DropDownList>

                        </div>

                        <div class="form-group form-inline ">
                            <label for="RegionSelect" class="control-label">Region:  </label>
                            <asp:DropDownList runat="server" ID="RegionSelect" CssClass="form-control">
                            </asp:DropDownList>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer">
                <asp:Button runat="server" Text="Load" CssClass="btn btn-success" ID="LoadRegionButton" OnClick="LoadRegionButton_OnClick" ClientIDMode="Static" />
                <button type="button" class="btn btn-danger " data-dismiss="modal">Cancel</button>
            </div>
        </div>
    </div>
</div>
