<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VirtualObjectsAndFields.ascx.cs" Inherits="Samples.VirtualObjectsAndFields" %>
<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" TagPrefix="AQB" %>
<AQB:QueryBuilderControl OnSleepModeChanged="SleepModeChanged" ID="QueryBuilderControl1" runat="server"
    OnInit="QueryBuilderControl1_Init"
    OnSQLUpdated="QueryBuilderControl1_OnSQLUpdated" />
<div id="all">
    <div class="header block-flat">
        This example demonstrates creation and using of virtual database objects and fields.
Switching the bottom tabs you can see the query text with virtual objects or the real query where the virtual objects 
are expanded to theirs expressions.
See the source code for details.
    </div>
    <div id="content-container">
        <div id="qb-ui">
            <AQB:ObjectTreeView ID="ObjectTree1" runat="server" />
            <div id="center">
                <AQB:SubQueryNavigationBar ID="SubQueryNavigationBar1" runat="server" />
                <div class="qb-ui-canvas-container block-flat">

                    <AQB:Canvas ID="Canvas1" runat="server" />
                    <AQB:StatusBar ID="StatusBar1" runat="server" />
                    <AQB:Grid ID="Grid1" runat="server" />
                </div>
            </div>

            <div class="clear">
            </div>
        </div>
    </div>
    <AQB:SqlEditor ID="SQLEditor1" runat="server"></AQB:SqlEditor>
    <div class="ui-dialog ui-widget sql">
        <div class="ui-dialog-titlebar ui-widget-header">
            <span class="ui-dialog-title-dialog">Real query text with virtual objects expanded to their expressions</span>
        </div>
        <div class="ui-dialog-content ui-widget-content" id="alternate-sql">
            <textarea></textarea>
        </div>
    </div>
</div>
