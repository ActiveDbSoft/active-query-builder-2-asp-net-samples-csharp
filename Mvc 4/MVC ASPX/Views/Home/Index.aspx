<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" TagPrefix="AQB" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Home Page - My ASP.NET MVC Application
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
	<AQB:QueryBuilderControl ID="QueryBuilderControl1" runat="server" LoadJScript="False" LoadCSS="False" />
	<div id="all">
		<div id="content-container">
			<div id="qb-ui">
				<AQB:ObjectTreeView  ID="ObjectTree1" runat="server" />
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
</asp:Content>
