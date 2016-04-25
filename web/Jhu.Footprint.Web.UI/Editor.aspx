<%@ Page Title="" Language="C#" MasterPageFile="~/UI.master" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" Inherits="Jhu.Footprint.Web.UI.Editor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="toolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middle" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-md-10">
                        <asp:Image ID="PlotCanvas" runat="server" CssClass="img-responsive" />
                    </div>
                    <div class="col-md-2">
                        <div class="btn-group-vertical">
                            <button class="btn btn-lg btn-default">Add region</button>
                            <button class="btn btn-lg btn-default">Load footprint</button>
                            <button class="btn btn-lg btn-default">Grow</button>
                            <button class="btn btn-lg btn-success disabled" >Save</button>
                            <button class="btn btn-lg btn-success">Download</button>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
