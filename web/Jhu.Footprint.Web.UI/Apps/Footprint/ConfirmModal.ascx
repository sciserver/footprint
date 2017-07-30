<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfirmModal.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.ConfirmModal" %>
<jgwuc:Form runat="server" ID="confirmModal" ClientIDMode="Static" IsModal="true"
    Text="Add circle">
    <FormTemplate>
        <p id="message">
            Please specify the center of the circle and its radius.
        </p>
    </FormTemplate>
    <ButtonsTemplate>
        <input type="button" id="ok" value="OK" />
        <input type="button" id="cancel" value="Cancel" data-dismiss="modal" />
    </ButtonsTemplate>
</jgwuc:Form>
