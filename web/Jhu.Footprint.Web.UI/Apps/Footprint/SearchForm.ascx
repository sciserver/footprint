<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchForm.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.SearchForm" %>

<jgwuc:Form runat="server" ID="searchForm" Text="Footprint search">
    <FormTemplate>

        <%--<div class="row" id="FastSearchPanel">
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="FastSearchInput" CssClass="form-control"></asp:TextBox>

            </div>
        </div>--%>
        <div class="row">
            <div class="col-sm-12">
                <ul runat="server" id="searchTabContainer" class="nav nav-tabs gw-tabs">
                    <li class="active"><a id="keywordSearch" data-toggle="tab" href="#keywordSearchPanel">Keyword search</a></li>
                    <li><a id="objectSearch" data-toggle="tab" href="#objectSearchPanel">Object search</a></li>
                    <li><a id="pointSearch" data-toggle="tab" href="#pointSearchPanel">Point search</a></li>
                    <li><a id="coneSearch" data-toggle="tab" href="#coneSearchPanel">Cone search</a></li>
                    <li><a id="regionSearch" data-toggle="tab" href="#regionSearchPanel">Region search</a></li>
                </ul>
                <div class="tab-content" style="height:200px">
                    <div id="keywordSearchPanel" class="tab-pane fade in active">
                        <table class="gw-form">
                            <tr>
                                <td class="gw-form-label">Keyword:</td>
                                <td class="gw-form-field">
                                    <asp:TextBox runat="server" ID="keyword" CssClass="wide" /></td>
                            </tr>
                        </table>
                    </div>
                    <div id="objectSearchPanel" class="tab-pane fade">
                        <table class="gw-form">
                            <tr>
                                <td class="gw-form-label">Object name:</td>
                                <td class="gw-form-field">
                                    <asp:TextBox runat="server" ID="objectName" CssClass="wide" /></td>
                            </tr>
                            <tr>
                                <td class="gw-form-label">Resolved coordinates:</td>
                                <td class="gw-form-field">
                                    <asp:TextBox runat="server" ID="objectRa" CssClass="narrow" ReadOnly="true" />
                                    <asp:TextBox runat="server" ID="objectDec" CssClass="narrow" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="pointSearchPanel" class="tab-pane fade">
                        <table class="gw-form">
                            <tr>
                                <td class="gw-form-label">Point:</td>
                                <td class="gw-form-field">
                                    <asp:TextBox runat="server" ID="pointRa" CssClass="narrow" Text="01:15:00" />
                                    <asp:TextBox runat="server" ID="pointDec" CssClass="narrow" Text="+05:00:00" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="pointRa" ValidationGroup="pointSearch" Display="Dynamic"><br />RA is required.</asp:RequiredFieldValidator>
                                    <jsa:AngleFormatValidator runat="server" ControlToValidate="pointRa" ValidationGroup="pointSearch" Display="Dynamic"><br />Invalid RA format, use decimal or hh:mm:ss.</jsa:AngleFormatValidator>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="pointDec" ValidationGroup="pointSearch" Display="Dynamic"><br />Dec is required.</asp:RequiredFieldValidator>
                                    <jsa:AngleFormatValidator runat="server" ControlToValidate="pointDec" ValidationGroup="pointSearch" Display="Dynamic"><br />Invalid Dec format, use decimal or dd:mm:ss.</jsa:AngleFormatValidator>
                            </tr>
                        </table>
                    </div>
                    <div id="coneSearchPanel" class="tab-pane fade">
                        <table class="gw-form">
                            <tr>
                                <td class="gw-form-label">Center:</td>
                                <td class="gw-form-field">
                                    <asp:TextBox runat="server" ID="coneRa" CssClass="narrow" Text="01:15:00" />
                                    <asp:TextBox runat="server" ID="coneDec" CssClass="narrow" Text="+05:00:00" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="coneRa" ValidationGroup="coneSearch" Display="Dynamic"><br />RA is required.</asp:RequiredFieldValidator>
                                    <jsa:AngleFormatValidator runat="server" ControlToValidate="coneRa" ValidationGroup="coneSearch" Display="Dynamic"><br />Invalid RA format, use decimal or hh:mm:ss.</jsa:AngleFormatValidator>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="coneDec" ValidationGroup="coneSearch" Display="Dynamic"><br />Dec is required.</asp:RequiredFieldValidator>
                                    <jsa:AngleFormatValidator runat="server" ControlToValidate="coneDec" ValidationGroup="coneSearch" Display="Dynamic"><br />Invalid Dec format, use decimal or dd:mm:ss.</jsa:AngleFormatValidator>
                            </tr>
                            <tr>
                                <td class="gw-form-label">Radius:</td>
                                <td class="gw-form-field">
                                    <asp:TextBox runat="server" ID="coneRadius" ClientIDMode="Static" CssClass="narrow" Text="300" />
                                    arc min
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="coneRadius" ValidationGroup="coneSearch" Display="Dynamic"><br />Radius is required.</asp:RequiredFieldValidator>
                                    <jsa:AngleFormatValidator runat="server" ControlToValidate="coneRadius" ValidationGroup="coneSearch" Display="Dynamic" AngleStyle="Decimal"><br />Invalid radius format, use decimal.</jsa:AngleFormatValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="gw-form-label">Containment:</td>
                                <td class="gw-form-field">
                                    <asp:RadioButton runat="server" ID="coneIntersect" Text="Intersect" GroupName="coneSearch" Checked="true" />
                                    <asp:RadioButton runat="server" ID="coneCover" Text="Cover" GroupName="coneSearch" />
                                    <asp:RadioButton runat="server" ID="coneContain" Text="Contain" GroupName="coneSearch" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="regionSearchPanel" class="tab-pane fade">
                        <table class="gw-form">
                            <tr>
                                <td class="gw-form-label">Region string:</td>
                                <td class="gw-form-field"></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="gw-form-field">
                                    <asp:TextBox runat="server" ID="regionString" TextMode="MultiLine" Height="120px" Text="REGION CIRCLE J2000 10 10 300" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="regionString" ValidationGroup="regionSearch" Display="Dynamic"><br />Region text is required.</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="gw-form-label">Containment:</td>
                                <td class="gw-form-field">
                                    <asp:RadioButton runat="server" ID="regionIntersect" Text="Intersect" GroupName="regionSearch" Checked="true" />
                                    <asp:RadioButton runat="server" ID="regionCover" Text="Cover" GroupName="regionSearch" />
                                    <asp:RadioButton runat="server" ID="regionContain" Text="Contain" GroupName="regionSearch" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 gw-details-container">
                <p class="gw-form-details">
                    <jgwc:DetailsButton runat="server" Text="more options" />
                </p>
                <div class="gw-details-panel">
                    <table class="gw-form">
                        <tr>
                            <td class="gw-form-label">Visibility:</td>
                            <td class="gw-form-field">
                                <asp:DropDownList runat="server" ID="visibility" CssClass="wide">
                                    <asp:ListItem Text="Public and private footprints" Value="All" Selected="True" />
                                    <asp:ListItem Text="Public footprints only" Value="Public" />
                                    <asp:ListItem Text="Private footprints only" Value="Private" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gw-form-label">Search mode:</td>
                            <td class="gw-form-field">
                                <asp:DropDownList runat="server" ID="mode" CssClass="wide">
                                    <asp:ListItem Text="Search among entire footprints" Value="Footprint" />
                                    <asp:ListItem Text="Search among individual regions" Value="Region" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </FormTemplate>
    <ButtonsTemplate>
        <input type="button" id="ok" value="OK" />
    </ButtonsTemplate>
</jgwuc:Form>
