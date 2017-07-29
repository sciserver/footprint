<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CircleModal.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.CircleModal" %>
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
                <td class="gw-form-label">Region name:</td>
                <td class="gw-form-field">
                    <asp:TextBox runat="server" ID="regionName" ClientIDMode="Static"
                        CssClass="narrow" Text="new_region" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="regionName" ValidationGroup="circleModal" Display="Dynamic">
                        <br />Name is required.
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="regionNameFormatValidator" ControlToValidate="regionName" ValidationGroup="circleModal" Display="Dynamic">
                        <br />Name format is invalid.
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="gw-form-label">Center:</td>
                <td class="gw-form-field">
                    <asp:TextBox runat="server" ID="circleCenterRa" ClientIDMode="Static" CssClass="narrow" Text="01:15:00" />
                    <asp:TextBox runat="server" ID="circleCenterDec" ClientIDMode="Static" CssClass="narrow" Text="+05:00:00" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="circleCenterRa" ValidationGroup="circleModal" Display="Dynamic">
                        <br />RA is required.
                    </asp:RequiredFieldValidator>
                    <jsa:AngleFormatValidator runat="server" ControlToValidate="circleCenterRa" ValidationGroup="circleModal" Display="Dynamic">
                        <br />Invalid RA format, use decimal or hh:mm:ss.
                    </jsa:AngleFormatValidator>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="circleCenterDec" ValidationGroup="circleModal" Display="Dynamic">
                        <br />Dec is required.
                    </asp:RequiredFieldValidator>
                    <jsa:AngleFormatValidator runat="server" ControlToValidate="circleCenterDec" ValidationGroup="circleModal" Display="Dynamic">
                        <br />Invalid Dec format, use decimal or dd:mm:ss.
                    </jsa:AngleFormatValidator>
                </td>
            </tr>
            <tr>
                <td class="gw-form-label">Radius:</td>
                <td class="gw-form-field">
                    <asp:TextBox runat="server" ID="circleRadius" ClientIDMode="Static" CssClass="narrow" Text="300" />
                    arc min
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="circleRadius" ValidationGroup="circleModal" Display="Dynamic">
                        <br />Radius is required.
                    </asp:RequiredFieldValidator>
                    <jsa:AngleFormatValidator runat="server" ControlToValidate="circleRadius" ValidationGroup="circleModal" Display="Dynamic" AngleStyle="Decimal">
                        <br />Invalid radius format, use decimal.
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
