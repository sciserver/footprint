<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Plot.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.Plot" %>


<asp:Content ID="plot" ContentPlaceHolderID="middle" runat="server"><asp:ScriptManagerProxy runat="server">
        <Scripts>
            <asp:ScriptReference Path="Scripts/generalFunctions.js" />
            <asp:ScriptReference Path="Plot.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <div class="container">
        <div class="row">
            <div class="col-sm-10">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div id="PlotCanvasContainer">
                            <asp:Image runat="server" ID="PlotCanvas" CssClass="img-responsive" Width="1080" Height="600" />
                            <button type="button" class="btn btn-sm" id="refreshCanvasButton"><span class="glyphicon glyphicon-refresh"></span></button>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="cl-sm-2">
                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#LoadModal">Load Region</button>
                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#SaveModal">Download</button>
                <hr />
                <p class="cm-header">Projection</p>
                <asp:RadioButtonList ID="plotProjectionStyle" runat="server" AutoPostBack="True">
                    <asp:ListItem Value="Aitoff" Selected="True">Aitoff</asp:ListItem>
                    <asp:ListItem Value="Equirectangular">Equirectangular</asp:ListItem>
                    <asp:ListItem Value="HammerAitoff">Hammer Aitoff</asp:ListItem>
                    <asp:ListItem Value="Mollweide">Mollweide</asp:ListItem>
                    <asp:ListItem Value="Orthographic">Orthographic</asp:ListItem>
                    <asp:ListItem Value="Stereographic">Stereographic</asp:ListItem>
                </asp:RadioButtonList>
                <hr />
                <asp:RadioButtonList runat="server" AutoPostBack="True" ID="plotDegreeStyle">
                    <asp:ListItem Value="Decimal" Selected="True">Decimal</asp:ListItem>
                    <asp:ListItem Value="Sexagesimal">HMS-DMS</asp:ListItem>
                </asp:RadioButtonList>
                <hr />
                <asp:RadioButtonList runat="server" AutoPostBack="True" ID="plotSystem">
                    <asp:ListItem Value="Equatorial" Selected="True">Equatorial</asp:ListItem>
                    <asp:ListItem Value="Galactic">Galactic</asp:ListItem>
                </asp:RadioButtonList>
                <hr />
                <asp:CheckBoxList AutoPostBack="True" runat="server">
                    <asp:ListItem ID="plotGrid" runat="server" Selected="True">Grid</asp:ListItem>
                    <asp:ListItem ID="plotAutoZoom" runat="server">Auto Zoom</asp:ListItem>
                    <asp:ListItem ID="plotAutoRotate" runat="server" Selected="true">Auto Rotate</asp:ListItem>

                </asp:CheckBoxList>

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
                        <asp:Button runat="server" Text="Load" CssClass="btn btn-success" ID="LoadRegionButton" ClientIDMode="Static" OnClick="LoadRegionButton_OnClick" />
                        <button type="button" class="btn btn-danger " data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>

        </div>

        <div id="SaveModal" class="modal" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                        <h4 class="modal-title">Download Footprint</h4>
                    </div>
                    <div class="modal-body">
                                TODO: size + Format selector
                                + JS script to make the ajax call

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" id="Button1">Download</button>
                        <button type="button" class="btn btn-danger " data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>

        </div>


    </div>
</asp:Content>
