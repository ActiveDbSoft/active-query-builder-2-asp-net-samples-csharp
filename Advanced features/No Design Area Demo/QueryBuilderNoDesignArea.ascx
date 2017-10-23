<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryBuilderNoDesignArea.ascx.cs" Inherits="Samples.QueryBuilderNoDesignArea" %>
<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" TagPrefix="AQB" %>
<AQB:QueryBuilderControl OnSleepModeChanged="SleepModeChanged" ID="QueryBuilderControl1" runat="server" OnInit="QueryBuilderControl1_Init" />
<div id="all">
    <div class="header"></div>
    <div id="content-container">
        <div id="qb-ui">
            <AQB:ObjectTreeView ID="ObjectTree1" runat="server" ShowFields="true" />
            <div id="center">
                <AQB:StatusBar ID="StatusBar1" runat="server" />
                <AQB:Grid ID="Grid1" runat="server" />
                <AQB:SqlEditor ID="SQLEditor1" runat="server"></AQB:SqlEditor>
            </div>
        </div>

        <div class="clear"></div>
    </div>
</div>
<div class="foot"></div>
