<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryStructure.ascx.cs" Inherits="Samples.QueryStructure" %>
<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" TagPrefix="AQB" %>
<AQB:QueryBuilderControl OnSleepModeChanged="SleepModeChanged" ID="QueryBuilderControl1" runat="server" oninit="QueryBuilderControl1_Init" OnSQLUpdated="QueryBuilderControl1_OnSQLUpdated"/>
<div id="all">
    <div class="header block-flat">
        <div id="sample-selector" class="qb-ui-syntax-selector">
            <span class="qb-ui-syntax-selector-label">Load sample queries:</span>
            <input id="sample-1" type="button" value="SELECT" />&nbsp;
            <input id="sample-2" type="button" value="SELECT FROM WHERE" />&nbsp;
            <input id="sample-3" type="button" value="SELECT FROM JOIN" />&nbsp;
            <input id="sample-4" type="button" value="SELECT FROM with subqueries" />&nbsp;
            <input id="sample-5" type="button" value="MULTIPLE UNIONs" />
        </div>
    </div>
    <div id="content-container">
        <div id="qb-ui">
            <AQB:ObjectTreeView  ID="ObjectTree1" runat="server" ShowFields="true" />
            <div id="center">
				<AQB:SubQueryNavigationBar ID="SubQueryNavigationBar1" runat="server" />
				<div class="qb-ui-canvas-container block-flat">

                <AQB:Canvas ID="Canvas1" runat="server" DefaultDatasourceWidth="200"/>
                <AQB:StatusBar ID="StatusBar1" runat="server" />
                <AQB:Grid ID="Grid1" runat="server" />
            </div></div>

            <div class="clear"></div>
        </div>
    </div>

	<div id="main-tabs" class="block-flat">
		<ul>
			<li><a href="#qb-ui-editor">SQL</a></li>
			<li><a href="#statistics">Statistics</a></li>
			<li><a href="#sub-queries">SubQueries</a></li>
			<li><a href="#query-structure">Query structure</a></li>
			<li><a href="#union-sub-query">UnionSubQuery</a></li>
		</ul>
		<AQB:SqlEditor ID="SQLEditor1" runat="server"></AQB:SqlEditor>
		<div id="statistics"></div>
		<div id="sub-queries"></div>
		<div id="query-structure"></div>
		<div id="union-sub-query">
			<div id="union-sub-query-tabs">
				<ul>
					<li><a href="#selected-expressions">Selected Expressions</a></li>
					<li><a href="#datasources">DataSources</a></li>
					<li><a href="#links">Links</a></li>
					<li><a href="#where">Where</a></li>
				</ul>
				<div id="selected-expressions"></div>
				<div id="datasources"></div>
				<div id="links"></div>
				<div id="where"></div>
			</div>
		</div>
	</div>

</div>
