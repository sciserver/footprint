<%@ Page Title="" Language="C#" MasterPageFile="~/UI.master" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Editor" %>

<asp:Content ID="Editor" ContentPlaceHolderID="middle" runat="server">

    <div class="container">
        <div class="row">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="col-md-10">
                        <%--<asp:Image ID="PlotCanvas" runat="server" CssClass="img-responsive" />--%>
                        <img id="PlotCanvas" class="img-responsive" height="" width="" src="#" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-2" id="editorMainButtonGroup">
            <div class="">
                <button type="button" class="btn btn-lg btn-default btn-block" id="LaunchAddRegionModalButton">Add region</button>
                <button type="button" class="btn btn-lg btn-default btn-block" id="LaunchLoadModalButton">Load footprint</button>
                <button type="button" class="btn btn-lg btn-default btn-block" id="LaunchGrowModalButton">Grow</button>
                <button type="button" class="btn btn-lg btn-success btn-block disabled">Save</button>
                <button type="button" class="btn btn-lg btn-success btn-block">Download</button>

            </div>
        </div>



        <asp:UpdatePanel ID="UpdateModal" runat="server">
            <ContentTemplate>

                <div id="GrowModal" class="modal" tabindex="-1" role="dialog">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                                <h4 class="modal-title">Grow region</h4>

                            </div>
                            <div class="modal-body">
                                <div class="form-inline">
                                    <label for="RadiusInput" class="control-label">Radius (arcmin): </label>
                                    <input type="text" class="form-control" id="RadiusInput" placeholder="10"></input>
                                    <span class="glyphicon glyphicon-info-sign" data-toggle="tooltip" title="Use negative value to reduce. Valid range: -120 .. 120 arcmin"></span>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-success" id="GrowButton">Grow</button>
                                <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="AddRegionModal" class="modal" tabindex="-1" role="dialog">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                                <h4 class="modal-title">Add Region</h4>
                            </div>
                            <div class="modal-body">
                                <div class="btn-group span12" data-toggle="buttons">
                                    <label class="btn btn-primary active">
                                        <input type="radio" name="options" value="new" checked="checked">
                                        New
                                    </label>
                                    <label class="btn btn-primary">
                                        <input type="radio" name="options" value="union">
                                        Union
                                    </label>
                                    <label class="btn btn-primary">
                                        <input type="radio" name="options" value="intersect">
                                        Intersect
                                    </label>
                                </div>

                                <div id="CircleRegionInput" class="hidden">
                                    circle
                                </div>
                                <div id="PolygonRegionInput" class="hidden">
                                    poly
                                </div>
                                <div id="CostumRegionInput" class="hidden">
                                    costum
                                </div>

                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-success">Add</button>
                                <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>


                <div id="LoadModal" class="modal" tabindex="-1" role="dialog">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                                <h4 class="modal-title">Load Footprint</h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-inline">
                                    <div class="form-group">
                                        <label for="FolderSelect" class="control-label">Folder: </label>
                                        <select id="FolderSelect" class="form-control ">
                                            <option selected="selected" disabled="disabled">Please select...</option>
                                        </select>
                                    </div>

                                    <div class="form-group ">
                                        <label for="FootprintSelect" class="control-label">Footprint:  </label>
                                        <select id="FootprintSelect" class="form-control">
                                            <option disabled="disabled">Please select...</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-success" id="LoadFootprintButton">Load</button>
                                    <button type="button" class="btn btn-danger " data-dismiss="modal">Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <div id="SaveModal">
                </div>

                <div id="DownloadModal">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
