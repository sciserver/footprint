<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CombinedRegionModal.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.CombinedRegionModal" %>
<jgwuc:Form runat="server" ID="combinedRegionModal" ClientIDMode="Static" IsModal="true"
    Text="Combine regions">
    <FormTemplate>
        <p>
            Please specify a name for the resulting region.
        </p>
        <ul>
            <li>Compose a unique name with letters, numbers and dashes or underscores.</li>
        </ul>
        <table class="gw-form">
            <tr>
                <td class="gw-form-label">Region name:</td>
                <td class="gw-form-field">
                    <asp:TextBox runat="server" ID="regionName" ClientIDMode="Static"
                        CssClass="narrow" Text="new_region" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="regionName" ValidationGroup="combinedRegionModal" Display="Dynamic">
                        <br />Name is required.
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="regionNameFormatValidator" ControlToValidate="regionName" ValidationGroup="combinedRegionModal" Display="Dynamic">
                        <br />Name format is invalid.
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="gw-form-label"></td>
                <td class="gw-form-field">
                    <asp:CheckBox runat="server" ID="keepOriginal" ClientIDMode="Static" Text="Keep original regions" Checked="true" />
                </td>
            </tr>
        </table>
    </FormTemplate>
    <ButtonsTemplate>
        <input type="button" id="ok" value="OK" />
        <input type="button" id="cancel" value="Cancel" data-dismiss="modal" />
    </ButtonsTemplate>
</jgwuc:Form>
