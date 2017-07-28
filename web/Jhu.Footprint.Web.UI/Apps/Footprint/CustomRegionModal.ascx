<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomRegionModal.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.CustomRegionModal" %>
<jgwuc:Form runat="server" ID="customRegionModal" ClientIDMode="Static" IsModal="true" Text="Add rectangle">
    <FormTemplate>
        <p>
            Please specify a custom region with a region string.
        </p>
        <ul>
            <li><a href="#">Click here</a> for a description on the syntax.</li>
        </ul>
        <table class="gw-form">
            <tr>
                <td class="gw-form-label">Region name:</td>
                <td class="gw-form-field">
                    <asp:TextBox runat="server" ID="customRegionName" ClientIDMode="Static"
                        CssClass="narrow" Text="new_region" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="customRegionName" ValidationGroup="customRegionModal" Display="Dynamic">
                        <br />Name is required.
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="customRegionNameFormatValidator" ControlToValidate="customRegionName" ValidationGroup="customRegionModal" Display="Dynamic">
                        <br />Name format is invalid.
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="gw-form-label">Region string:</td>
                <td class="gw-form-field">
                </td>
            </tr>
            <tr>
                <td colspan="2" class="gw-form-field">
                    <asp:TextBox runat="server" ID="customRegionString" ClientIDMode="Static" TextMode="MultiLine" Text="REGION CIRCLE J2000 10 10 300" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="customRegionString" ValidationGroup="customRegionModal" Display="Dynamic">
                        <br />Region text is required.
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
