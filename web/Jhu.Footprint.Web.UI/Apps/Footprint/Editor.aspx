<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.Editor" %>

<%@ Register Src="~/Apps/Footprint/EditorRegionList.ascx" TagPrefix="uc1" TagName="EditorRegionList" %>
<%@ Register Src="~/Apps/Footprint/CircleModal.ascx" TagPrefix="uc1" TagName="CircleModal" %>
<%@ Register Src="~/Apps/Footprint/EditorCanvas.ascx" TagPrefix="uc1" TagName="EditorCanvas" %>
<%@ Register Src="~/Apps/Footprint/RectangleModal.ascx" TagPrefix="uc1" TagName="RectangleModal" %>
<%@ Register Src="~/Apps/Footprint/CustomRegionModal.ascx" TagPrefix="uc1" TagName="CustomRegionModal" %>
<%@ Register Src="~/Apps/Footprint/MultipointRegionModal.ascx" TagPrefix="uc1" TagName="MultipointRegionModal" %>



<asp:Content runat="server" ContentPlaceHolderID="toolbar">
    <div id="toolbar" class="toolbar">

        <div runat="server" id="projectionDiv" style="min-width: 140px">
            <asp:Label ID="projectionLabel" runat="server" Text="Projection:" /><br />
            <asp:DropDownList runat="server" ID="projection" ClientIDMode="Static">
                <asp:ListItem Text="Stereographic" Value="Stereographic" Selected="True" />
                <asp:ListItem Text="Orthographic" Value="Orthographic" />
                <asp:ListItem Text="Aitoff" Value="Aitoff" />
                <asp:ListItem Text="Hammer-Aitoff" Value="HammerAitoff" />
                <asp:ListItem Text="Mollweide" Value="Mollweide" />
                <asp:ListItem Text="Equirectangular" Value="Equirectangular" />
            </asp:DropDownList>
        </div>
        <a id="refresh" style="min-width: 40px; text-align: center"><span class="glyphicon glyphicon-refresh"></span></a>
        <div class="dropdown">
            <a class="dropdown-toggle" data-toggle="dropdown">view<span class="caret"></span></a>
            <ul class="dropdown-menu" id="viewDropdown">
                <li>
                    <asp:CheckBox runat="server" ID="autoRotate" ClientIDMode="Static" Text="auto center" /></li>
                <li>
                    <asp:CheckBox runat="server" ID="autoZoom" ClientIDMode="Static" Text="auto zoom" /></li>
                <li role="separator" class="divider"></li>
                <li>
                    <asp:CheckBox runat="server" ID="grid" ClientIDMode="Static" Text="grid" Checked="true" /></li>
                <li>
                    <asp:RadioButton runat="server" GroupName="coordsys" ID="equatorial" ClientIDMode="Static" Text="equatorial" Checked="true" /></li>
                <li>
                    <asp:RadioButton runat="server" GroupName="coordsys" ID="galactic" ClientIDMode="Static" Text="galactic" /></li>
                <li role="separator" class="divider"></li>
                <li>
                    <asp:RadioButton runat="server" GroupName="degreeStyle" ID="decimal" ClientIDMode="Static" Text="decimal" Checked="true" /></li>
                <li>
                    <asp:RadioButton runat="server" GroupName="degreeStyle" ID="hms" ClientIDMode="Static" Text="hms dms" /></li>
            </ul>
        </div>

        <span class="separator"></span>

        <div class="dropdown">
            <a class="dropdown-toggle" data-toggle="dropdown">new region<span class="caret"></span></a>
            <ul class="dropdown-menu">
                <li><a href="#" id="newCircle">circle</a></li>
                <li><a href="#" id="newRectangle">rectangle</a></li>
                <li><a href="#" id="newPolygon">polygon</a></li>
                <li><a href="#" id="newCHull">convex hull</a></li>
                <li role="separator" class="divider"></li>
                <li><a href="#" id="newCustomRegion">custom region</a></li>
                <li><a href="#" id="newFile">upload file</a></li>
            </ul>
        </div>
        <div class="dropdown">
            <a class="dropdown-toggle" data-toggle="dropdown">combine<span class="caret"></span></a>
            <ul class="dropdown-menu">
                <li><a href="#" id="union">union</a></li>
                <li><a href="#" id="intersect">intersect</a></li>
                <li><a href="#" id="subtract">subtract</a></li>
            </ul>
        </div>
        <a id="delete" style="min-width: 40px; text-align: center"><span class="glyphicon glyphicon-remove"></span></a>

        <span class="separator"></span>

        <a data-toggle="modal" data-target="#clearModal">clear</a>
        <a data-toggle="modal" data-target="#saveModal">save</a>
        <a data-toggle="modal" data-target="#downloadModal">download</a>

        <span class="span"></span>

    </div>
</asp:Content>
<asp:Content ID="Editor" ContentPlaceHolderID="middle" runat="server">
    <asp:ScriptManagerProxy runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/Api/V1/Editor.svc/proxy.js" />
            <asp:ScriptReference Path="Editor.aspx.js" />
            <asp:ScriptReference Path="~/Scripts/astro.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel runat="server" class="dock-container dock-fill">
        <ContentTemplate>

            <%-- Region List  --%>
            <div class="dock-right dock-container" style="width: 248px; margin-left: 8px;">
                <uc1:EditorRegionList runat="server" ID="regionList" />
            </div>

            <%-- Canvas  --%>
            <div class="dock-container dock-fill" style="border: solid 1px #000000;">
                <uc1:EditorCanvas runat="server" ID="editorCanvas" />
            </div>

            <%-- Modal windows  --%>
            <uc1:CircleModal runat="server" ID="circleModal" />
            <uc1:RectangleModal runat="server" id="rectangleModal" />
            <uc1:CustomRegionModal runat="server" id="customRegionModal" />
            <uc1:MultipointRegionModal runat="server" ID="multipointRegionModal" />
            
            <%--
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
                --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
