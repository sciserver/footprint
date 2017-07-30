<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SaveFootprintModal.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.SaveFootprintModal" %>
<jgwuc:Form runat="server" ID="saveFootprintModal" ClientIDMode="Static" IsModal="true"
    Text="Save footprint">
    <FormTemplate>
        <p>
            Please specify a name for the new footprint
        </p>
        <ul>
            <li>You can make the footprint publicly visible.</li>
        </ul>
        <table class="gw-form">
            <tr>
                <td class="gw-form-label">Footprint name:</td>
                <td class="gw-form-field">
                    <asp:TextBox runat="server" ID="footprintName" ClientIDMode="Static"
                        CssClass="narrow" Text="new_footprint" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="footprintName" ValidationGroup="saveFootprintModal" Display="Dynamic">
                        <br />Name is required.
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="footprintNameFormatValidator" ControlToValidate="footprintName" ValidationGroup="saveFootprintModal" Display="Dynamic">
                        <br />Name format is invalid.
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="gw-form-label"></td>
                <td class="gw-form-field"><asp:CheckBox runat="server" ID="public" ClientIDMode="Static" Text="Make footprint public" /></td>
            </tr>
            <tr>
                <td class="gw-form-label"></td>
                <td class="gw-form-field"><asp:CheckBox runat="server" ID="overwrite" ClientIDMode="Static" Text="Force overwrite" /></td>
            </tr>
        </table>
    </FormTemplate>
    <ButtonsTemplate>
        <input type="button" id="ok" value="OK" />
        <input type="button" id="cancel" value="Cancel" data-dismiss="modal" />
    </ButtonsTemplate>
</jgwuc:Form>
