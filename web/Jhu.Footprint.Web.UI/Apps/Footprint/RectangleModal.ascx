<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RectangleModal.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.RectangleModal" %>
<jgwuc:Form runat="server" ID="rectangleModal" ClientIDMode="Static" IsModal="true" Text="Add rectangle">
    <FormTemplate>
        <p>
            Please specify the two diagonal corners of the rectangle.
        </p>
        <ul>
            <li>Use decimal a sexagesimal degrees for coordinates.</li>
        </ul>
        <table class="gw-form">
            <tr>
                <td class="gw-form-label">Region name:</td>
                <td class="gw-form-field">
                    <asp:TextBox runat="server" ID="regionName" ClientIDMode="Static"
                        CssClass="narrow" Text="new_region" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="regionName" ValidationGroup="rectangleModal" Display="Dynamic">
                        <br />Name is required.
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="regionNameFormatValidator" ControlToValidate="regionName" ValidationGroup="rectangleModal" Display="Dynamic">
                        <br />Name format is invalid.
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="gw-form-label">RA Dec:</td>
                <td class="gw-form-field">
                    <asp:TextBox runat="server" ID="rectangleRa1" ClientIDMode="Static" CssClass="narrow" Text="01:15:00" />
                    <asp:TextBox runat="server" ID="rectangleDec1" ClientIDMode="Static" CssClass="narrow" Text="+13:00:00" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="rectangleRa1" ValidationGroup="rectangleModal" Display="Dynamic">
                        <br />RA is required.
                    </asp:RequiredFieldValidator>
                    <jsa:AngleFormatValidator runat="server" ControlToValidate="rectangleRa1" ValidationGroup="rectangleModal" Display="Dynamic">
                        <br />Invalid RA format, use decimal or hh:mm:ss.
                    </jsa:AngleFormatValidator>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="rectangleDec1" ValidationGroup="rectangleModal" Display="Dynamic">
                        <br />Dec is required.
                    </asp:RequiredFieldValidator>
                    <jsa:AngleFormatValidator runat="server" ControlToValidate="rectangleDec1" ValidationGroup="rectangleModal" Display="Dynamic">
                        <br />Invalid Dec format, use decimal or dd:mm:ss.
                    </jsa:AngleFormatValidator>
                </td>
            </tr>
            <tr>
                <td class="gw-form-label">RA Dec:</td>
                <td class="gw-form-field">
                    <asp:TextBox runat="server" ID="rectangleRa2" ClientIDMode="Static" CssClass="narrow" Text="01:55:00" />
                    <asp:TextBox runat="server" ID="rectangleDec2" ClientIDMode="Static" CssClass="narrow" Text="+03:00:00" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="rectangleRa2" ValidationGroup="rectangleModal" Display="Dynamic">
                        <br />RA is required.
                    </asp:RequiredFieldValidator>
                    <jsa:AngleFormatValidator runat="server" ControlToValidate="rectangleRa2" ValidationGroup="rectangleModal" Display="Dynamic">
                        <br />Invalid RA format, use decimal or hh:mm:ss.
                    </jsa:AngleFormatValidator>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="rectangleDec2" ValidationGroup="rectangleModal" Display="Dynamic">
                        <br />Dec is required.
                    </asp:RequiredFieldValidator>
                    <jsa:AngleFormatValidator runat="server" ControlToValidate="rectangleDec2" ValidationGroup="rectangleModal" Display="Dynamic">
                        <br />Invalid Dec format, use decimal or dd:mm:ss.
                    </jsa:AngleFormatValidator>
                </td>
            </tr>
        </table>
    </FormTemplate>
    <ButtonsTemplate>
        <input type="button" id="ok" value="OK" />
        <input type="button" id="cancel" value="Cancel" data-dismiss="modal" />
    </ButtonsTemplate>
</jgwuc:Form>
