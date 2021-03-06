﻿<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.Editor" %>

<%@ Register Src="~/Apps/Footprint/EditorRegionList.ascx" TagPrefix="uc1" TagName="EditorRegionList" %>
<%@ Register Src="~/Apps/Footprint/ConfirmModal.ascx" TagPrefix="uc1" TagName="ConfirmModal" %>
<%@ Register Src="~/Apps/Footprint/CircleModal.ascx" TagPrefix="uc1" TagName="CircleModal" %>
<%@ Register Src="~/Apps/Footprint/EditorCanvas.ascx" TagPrefix="uc1" TagName="EditorCanvas" %>
<%@ Register Src="~/Apps/Footprint/RectangleModal.ascx" TagPrefix="uc1" TagName="RectangleModal" %>
<%@ Register Src="~/Apps/Footprint/CustomRegionModal.ascx" TagPrefix="uc1" TagName="CustomRegionModal" %>
<%@ Register Src="~/Apps/Footprint/MultipointRegionModal.ascx" TagPrefix="uc1" TagName="MultipointRegionModal" %>
<%@ Register Src="~/Apps/Footprint/CombinedRegionModal.ascx" TagPrefix="uc1" TagName="CombinedRegionModal" %>
<%@ Register Src="~/Apps/Footprint/SaveFootprintModal.ascx" TagPrefix="uc1" TagName="SaveFootprintModal" %>

<asp:Content runat="server" ContentPlaceHolderID="toolbar">
    
    <asp:HiddenField runat="server" ID="owner" ClientIDMode="Static" />
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
                    <asp:CheckBox runat="server" ID="autoRotate" ClientIDMode="Static" Text="auto center" Checked="true" /></li>
                <li>
                    <asp:CheckBox runat="server" ID="autoZoom" ClientIDMode="Static" Text="auto zoom" Checked="true" /></li>
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
                <li><a href="#" id="circle">circle</a></li>
                <li><a href="#" id="rectangle">rectangle</a></li>
                <li><a href="#" id="polygon">polygon</a></li>
                <li><a href="#" id="cHull">convex hull</a></li>
                <li role="separator" class="divider"></li>
                <li><a href="#" id="customRegion">custom region</a></li>
                <li><a href="#" id="upload">upload file</a></li>
            </ul>
        </div>
        <%--
        <div class="dropdown">
            <a class="dropdown-toggle" data-toggle="dropdown">adjust</a>
            <ul class="dropdown-menu">
                <li><a href="#" id="invert">invert</a></li>
                <li><a href="#" id="grow">grow</a></li>
                <li><a href="#" id="mec">minimal circle</a></li>
            </ul>
        </div>
        --%>
        <div class="dropdown">
            <a class="dropdown-toggle" data-toggle="dropdown">combine<span class="caret"></span></a>
            <ul class="dropdown-menu">
                <li><a href="#" id="union">union</a></li>
                <li><a href="#" id="intersect">intersect</a></li>
                <li><a href="#" id="subtract">subtract</a></li>
            </ul>
        </div>
        <a id="delete">delete</a>

        <span class="separator"></span>

        <a id="clear">clear</a>
        <a id="save">save</a>
        <a data-toggle="modal" data-target="#downloadModal">download</a>

        <span class="span"></span>

    </div>
</asp:Content>
<asp:Content ID="Editor" ContentPlaceHolderID="middle" runat="server">
    <asp:ScriptManagerProxy runat="server">
        <Scripts>
            <asp:ScriptReference Path="../../Api/V1/Editor.svc/proxy.js" />
            <asp:ScriptReference Path="../../Api/V1/Footprint.svc/proxy.js" />
            <asp:ScriptReference Path="Editor.aspx.js" />
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
            <uc1:ConfirmModal runat="server" ID="confirmModal" />
            <uc1:CircleModal runat="server" ID="circleModal" />
            <uc1:RectangleModal runat="server" id="rectangleModal" />
            <uc1:CustomRegionModal runat="server" id="customRegionModal" />
            <uc1:MultipointRegionModal runat="server" ID="multipointRegionModal" />
            <uc1:CombinedRegionModal runat="server" id="combinedRegionModal" />
            <uc1:SaveFootprintModal runat="server" id="saveFootprintModal" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
