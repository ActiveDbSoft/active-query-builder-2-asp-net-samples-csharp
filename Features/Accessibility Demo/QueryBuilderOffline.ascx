<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryBuilderOffline.ascx.cs" Inherits="Samples.QueryBuilderOffline" %>
<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" TagPrefix="AQB" %>
<AQB:QueryBuilderControl OnSleepModeChanged="SleepModeChanged" ID="QueryBuilderControl1" runat="server" OnInit="QueryBuilderControl1_Init" Language="en" />
<div id="all">
	<div class="header">
		Use keys 1-5 for navigation
	</div>
	<div id="content-container">
		<div id="qb-ui">
			<AQB:ObjectTreeView  ID="ObjectTree1" runat="server"  />
			<div id="center">
				<AQB:SubQueryNavigationBar ID="SubQueryNavigationBar1" runat="server" />
				<div class="qb-ui-canvas-container block-flat">

				<AQB:Canvas ID="Canvas1" runat="server" />
				<AQB:StatusBar ID="StatusBar1" runat="server" />
				<AQB:Grid ID="Grid1" runat="server" />
			</div></div>

			<div class="clear">
			</div>
		</div>
	</div>
	<AQB:SqlEditor ID="SQLEditor1" runat="server"></AQB:SqlEditor>
	</div>
