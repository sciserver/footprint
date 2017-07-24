<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditorSaveFootprintForm.ascx.cs" Inherits="Jhu.Footprint.Web.UI.Apps.Footprint.SaveFootprintForm" %>

<div id="SaveModal" class="modal" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                        <h4 class="modal-title">Save Region</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group"><label for="SaveUserInput" class="control-label">User/Group: </label>
                            <input id="SaveUserInput" class="form-control" disabled="disabled" /><br />
                            <label for="SaveUserFootprintName" class="control-label">Footprint: </label>
                            <div class="row">
                                <div class="col-sm-8">
                                    <input id="SaveUserFootprintName" class="form-control" placeholder="Enter footprint name." /><br />
                                </div>
                                <div class="col-sm-4">
                                    <label class="radio-inline">
                                        <input type="radio" value="intersect" name="FootprintCombinationMethod" />Intersect</label>
                                    <label class="radio-inline">
                                        <input type="radio" value="union" name="FootprintCombinationMethod" checked="checked"/>Union</label>
                                </div>
                            </div>
                            <label for="SaveUserRegionName" class="control-label">Region: </label>
                            <input id="SaveUserRegionName" class="form-control" placeholder="Enter region name." />
                            <hr />
                            <label for="SaveFillFactorInput" class="control-label">Fill factor:</label>
                            <input id="SaveFillFactorInput" class="form-control" disabled="disabled" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" id="SaveRegionButton">Save</button>
                        <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>