<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.Editor" %>

<asp:Content runat="server" ContentPlaceHolderID="toolbar">
    <div id="toolbar" class="toolbar">
        <a data-toggle="modal" data-target="#clearModal">clear</a>
        <a data-toggle="modal" data-target="#saveModal">save</a>
        <a data-toggle="modal" data-target="#downloadModal">download</a>
        <span class="separator"></span>
        <div class="dropdown">
            <a class="dropdown-toggle" data-toggle="dropdown">union with<span class="caret"></span>
            </a>
            <ul class="dropdown-menu">
                <li><a href="#" data-item="union" data-arg="circle">circle</a></li>
                <li><a href="#" data-item="union" data-arg="rectangle">rectangle</a></li>
                <li><a href="#" data-item="union" data-arg="polygon">polygon</a></li>
                <li><a href="#" data-item="union" data-arg="chull">convex hull</a></li>
            </ul>
        </div>
        <div class="dropdown">
            <a class="dropdown-toggle" data-toggle="dropdown">intersect with<span class="caret"></span>
            </a>
            <ul class="dropdown-menu">
                <li><a href="#" data-item="intersect" data-arg="circle">circle</a></li>
                <li><a href="#" data-item="intersect" data-arg="rectangle">rectangle</a></li>
                <li><a href="#" data-item="intersect" data-arg="polygon">polygon</a></li>
                <li><a href="#" data-item="intersect" data-arg="chull">convex hull</a></li>
            </ul>
        </div>
        <a data-toggle="modal" data-target="#GrowModal">grow shape</a>

        <span class="span"></span>

        <div runat="server" id="projectionDiv" style="min-width: 140px">
            <asp:Label ID="projectionLabel" runat="server" Text="Projection:" /><br />
            <select id="projection">
                <option value="Stereographic" selected="selected">Stereographic</option>
                <option value="Orthographic">Orthographic</option>
                <option value="Aitoff">Aitoff</option>
                <option value="HammerAitoff">Hammer-Aitoff</option>
                <option value="Mollweide">Mollweide</option>
                <option value="Equirectangular">Equirectangular</option>
            </select>
        </div>
        <div style="min-width: 80px; text-align: left">
            <input type="checkbox" checked="checked" id="autoRotate" />
            <asp:Label runat="server" ID="autoRotateLabel" Text="Auto center" /><br />
            <input type="checkbox" checked="checked" id="autoZoom" />
            <asp:Label runat="server" ID="autoZoomLabel" Text="Auto zoom" />
        </div>
        <a id="refresh" data-item="refresh"><span class="glyphicon glyphicon-refresh"></span></a>
    </div>
</asp:Content>
<asp:Content ID="Editor" ContentPlaceHolderID="middle" runat="server">
    <asp:ScriptManagerProxy runat="server">
        <Scripts>
            <asp:ScriptReference Path="Footprint.js" />
            <asp:ScriptReference Path="Editor.aspx.js" />
            <asp:ScriptReference Path="~/Scripts/astro.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel runat="server" class="dock-container dock-fill">
        <ContentTemplate>
            <div id="canvas" class="dock-fill" style="background-repeat: no-repeat; background-position: center center;">
            </div>

            <jgwuc:Form runat="server" ID="circleModal" ClientIDMode="Static" IsModal="true"
                Text="Add circle">
                <FormTemplate>
                    <p>
                        Please specify the center of the circle and its radius.
                    </p>
                    <ul>
                        <li>Use decimal a sexagesimal degrees for coordinates.</li>
                        <li>Use decimal arc minutes for radius.</li>
                    </ul>
                    <table class="gw-form">
                        <tr>
                            <td class="gw-form-label">Center:</td>
                            <td class="gw-form-field">
                                <asp:TextBox runat="server" ID="circleCenterRa" ClientIDMode="Static"
                                    CssClass="narrow" placeholder="12:00:00.00" />
                                <asp:TextBox runat="server" ID="circleCenterDec" ClientIDMode="Static"
                                    CssClass="narrow" placeholder="12:00:00.00" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="circleCenterRa" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="circleCenterDec" />
                                <jsa:AngleFormatValidator runat="server" ControlToValidate="circleCenterRa"
                                    Display="Dynamic"><br />Invalid RA format.</jsa:AngleFormatValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="gw-form-label">Radius:</td>
                            <td class="gw-form-field">
                                <asp:TextBox runat="server" ID="circleRadius" ClientIDMode="Static"
                                    CssClass="narror" placeholder="300" />
                                arc min
                            </td>
                        </tr>
                    </table>
                </FormTemplate>
                <ButtonsTemplate>
                    <asp:Button runat="server" Text="OK" ID="circleModalOk" ClientIDMode="Static" />
                    <asp:Button runat="server" Text="Cancel" data-dismiss="modal" />
                </ButtonsTemplate>
            </jgwuc:Form>

            <div id="GrowModal" class="modal" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                            <h4 class="modal-title">Grow region</h4>

                        </div>
                        <div class="modal-body">
                            <div class="form-inline">
                                <label for="GrowRadius" class="control-label">Radius (arcmin): </label>
                                <input type="text" class="form-control" id="GrowRadius" placeholder="10" />
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
                                    <label class="btn btn-primary active">
                                        <input type="radio" name="AdditionTypeSelector" value="new" checked="checked">
                                        New
                                    </label>
                                    <label class="btn btn-primary">
                                        <input type="radio" name="AdditionTypeSelector" value="union">
                                        Union
                                    </label>
                                    <label class="btn btn-primary">
                                        <input type="radio" name="AdditionTypeSelector" value="intersect">
                                        Intersect
                                    </label>
                                    <label class="btn btn-primary">
                                        <input type="radio" name="AdditionTypeSelector" value="subtract">
                                        Subtract
                                    </label>
                                </div>
                            </div>

                            <hr />

                            <div id="RegionTypeSelector" class="text-center">
                                <h3 class="FormLabel">Select region type:</h3>
                                <div class="btn-group" data-toggle="buttons">
                                    <label class="btn btn-primary active">
                                        <input type="radio" name="RegionTypeSelector" value="circle" checked="checked">
                                        Circle
                                    </label>
                                    <label class="btn btn-primary">
                                        <input type="radio" name="RegionTypeSelector" value="polygon">
                                        Polygon
                                    </label>
                                    <label class="btn btn-primary">
                                        <input type="radio" name="RegionTypeSelector" value="costum">
                                        Costum
                                    </label>
                                </div>
                                <hr />
                            </div>


                            <div id="CircleRegionForm" class="AddRegionForms">
                                <div class="form-inline">
                                    <label for="CircleRA" class="FormLabel">Center:</label>
                                    <input id="CircleRA" type="text" class="form-control" placeholder="12:00:00.00" data-toogle="tooltip" title="Right Ascension" />
                                    <input id="CircleDec" type="text" class="form-control" placeholder="00:00:00.00" data-toogle="tooltip" title="Declination" />
                                </div>
                                <div class="form-inline">
                                    <label for="CircleRaidus" class="FormLabel">Radius:</label>
                                    <input id="CircleRadius" type="text" class="form-control" placeholder="120" data-toogle="tooltip" title="Arcmin" />
                                </div>
                            </div>
                            <div id="PolygonRegionForm" class="AddRegionForms text-center hidden">
                                <div class="form-inline">
                                    <label for="PolygonPoints" class="FormLabel">Points: </label>
                                    <textarea rows="8" id="PolygonPoints" class="form-control" style="min-width: 50%" data-toogle="tooltip" title="Specify one polygon point in each line. Use sexagesimal or decimal format.">
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
                            <button type="button" class="btn btn-success" id="AddRegionButton">Add</button>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
