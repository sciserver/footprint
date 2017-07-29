<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultipointRegionModal.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.MultipointRegionModal" %>
<jgwuc:Form runat="server" ID="multipointRegionModal" ClientIDMode="Static" IsModal="true" Text="Add polygon or convex hull">
    <FormTemplate>
        <p>
            Please specify a list of coordinate pairs
        </p>
        <ul>
            <li>Specify at least three points that are not on the same great circle.</li>
            <li>Polygons must be convex and cannot contain bowties.</li>
            <li>Convex hulls are computed for a single hemisphere only.</li>
        </ul>
        <table class="gw-form">
            <tr>
                <td class="gw-form-label">Region name:</td>
                <td class="gw-form-field">
                    <asp:TextBox runat="server" ID="regionName" ClientIDMode="Static"
                        CssClass="narrow" Text="new_region" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="regionName" ValidationGroup="multipointRegionModal" Display="Dynamic">
                        <br />Name is required.
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="regionNameFormatValidator" ControlToValidate="regionName" ValidationGroup="multipointRegionModal" Display="Dynamic">
                        <br />Name format is invalid.
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="gw-form-label">Mode:</td>
                <td class="gw-form-field">
                    <asp:RadioButton runat="server" ID="multipointRegionPoly" ClientIDMode="Static" Text="Polygon" GroupName="multipointRegionMode" /><br />
                    <asp:RadioButton runat="server" ID="multipointRegionCHull" ClientIDMode="Static" Text="Convex hull" GroupName="multipointRegionMode" />
            </tr>
            <tr>
                <td class="gw-form-label">Coordinate list:</td>
                <td class="gw-form-field">
                </td>
            </tr>
            <tr>
                <td colspan="2" class="gw-form-field">
                    <asp:TextBox runat="server" ID="multipointRegionCoordinates" ClientIDMode="Static" TextMode="MultiLine" 
                        Height="120px" Text="23:40:00 -05:00:00 
00:20:00 -05:00:00
00:00:00 05:00:00" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="multipointRegionCoordinates" ValidationGroup="multipointRegionModal" Display="Dynamic">
                        <br />A list of coordinates is required.
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </FormTemplate>
    <ButtonsTemplate>
        <input type="button" id="ok" value="OK" />
        <input type="button" id="cancel" value="Cancel" data-dismiss="modal" />
    </ButtonsTemplate>
</jgwuc:Form>
