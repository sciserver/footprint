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
                                <div id="AdditionTypeSelector" class="text-center">
                                    <h3 class="FormLabel">Select addition type:</h3>
                                    <div class="btn-group" data-toggle="buttons">
                                        <label class="btn btn-primary">
                                            <input type="radio" name="options" value="new">
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
                                </div>

                                <hr />

                                <div id="RegionTypeSelector" class="text-center hidden">
                                    <h3 class="FormLabel">Select region type:</h3>
                                    <div class="btn-group" data-toggle="buttons">
                                        <label class="btn btn-primary active">
                                            <input type="radio" name="options" value="circle" checked="checked">
                                            Circle
                                        </label>
                                        <label class="btn btn-primary">
                                            <input type="radio" name="options" value="polygon">
                                            Polygon
                                        </label>
                                        <label class="btn btn-primary">
                                            <input type="radio" name="options" value="costum">
                                            Costum
                                        </label>
                                    </div>
                                <hr />
                                </div>


                                <div id="CircleRegionForm" class="AddRegionForms hidden">
                                    <div class="form-inline">
                                        <label for="CircleRA" class="FormLabel">Center:</label>
                                        <input id="CircleRA"type="text" class="form-control" placeholder="12:00:00.00" data-toogle="tooltip" title="Right Ascension"/>                                    
                                        <input id="CricleDec" type="text" class="form-control" placeholder="00:00:00.00" data-toogle="tooltip" title="Declination"/>
                                    </div>
                                    <div class="form-inline">
                                        <label for="CircleRaidus" class="FormLabel">Radius:</label>
                                        <input id="CircleRadius" type="text" class="form-control" placeholder="120" data-toogle="tooltip" title="Arcmin"/>                                          
                                    </div>
                                </div>
                                <div id="PolygonRegionForm" class="AddRegionForms hidden">
                                    <div class="form-inline">
                                        <label for="PolygonPoints" class="FormLabel">Radius:</label>
                                        <textarea rows="8" id="PolygonPoints" class="form-control col-lg" data-toogle="tooltip" title="Specify one polygon point in each line. Use sexagesimal or decimal format.">
                                            12:00:00.00,   00:00:00.00
                                            12:00:00.00,   10:00:00.00
                                            11:00:00.00,   10:00:00.00
                                            11:00:00.00,   00:00:00.00
                                        </textarea>
                                    </div>
                                </div>
                                <div id="CostumRegionForm" class="AddRegionForms hidden">
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
