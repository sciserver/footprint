<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditorRegionList.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.EditorRegionList" %>

<div class="dock-top gw-list-frame-top">
    <div class="gw-list-header">
        <div class="gw-list-row">
            <span style="width: 24px"></span>
            <span class="gw-list-span">region name</span>
        </div>
    </div>
</div>
<div class="gw-list-frame dock-fill" type="editorRegionList" id="<%= ID %>">
    <%--<div class="gw-list-item">
        <span style="width: 24px">
            <input type="checkbox" /></span>
        <span class="gw-list-span">region_name</span>
    </div>--%>
</div>
<div class="dock-bottom gw-list-frame-bottom">
    <div class="gw-list-footer">
        <div class="gw-list-row">
        </div>
    </div>
</div>

