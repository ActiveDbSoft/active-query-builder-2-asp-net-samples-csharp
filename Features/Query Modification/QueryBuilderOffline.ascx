<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryBuilderOffline.ascx.cs" Inherits="Samples.QueryBuilderOffline" %>
<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" TagPrefix="AQB" %>
<script>
	$(function () {
		var index = 'key';
		var dataStore = window.sessionStorage;
		try {
			var oldIndex = dataStore.getItem(index);
		} catch (e) {
			var oldIndex = 0;
		}
		$('#tabs').tabs({
			active: oldIndex,
			activate: function (event, ui) {
				var newIndex = ui.newTab.parent().children().index(ui.newTab);
				dataStore.setItem(index, newIndex);
			}
		}).show();
	});
</script>
<div id="tabs">
	<ul>
		<li><a href="#tabs-1">SQL Text</a></li>
		<li><a href="#tabs-2">Query builder</a></li>
	</ul>
	<div id="tabs-1">
		<table>
			<tr>
				<td colspan="2">
					<asp:TextBox ID="tbSQL" runat="server" TextMode="MultiLine" Columns="100" Rows="25"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<p>Modification Settings:</p>
					<p>Check tables you wish to add to the query.</p>
					<p>Check fields you wish to define criteria for.</p>
					<asp:CheckBox ID="cbCustomers" runat="server" Text="Customers" />
					<br />
					<asp:CheckBox ID="cbCompanyName" runat="server" Text="Customers & CompanyName" /><asp:TextBox ID="tbCompanyName" runat="server" Text="Like 'C%'"></asp:TextBox>
					<br />
					<asp:CheckBox ID="cbOrders" runat="server" Text="Orders" />
					<br />
					<asp:CheckBox ID="cbOrderDate" runat="server" Text="Orders & OrderDate" /><asp:TextBox ID="tbOrderDate" runat="server" Text="= '01/01/2007'"></asp:TextBox>
					<br />
				</td>
				<td>
					<asp:Button ID="btnQueryCustomers" runat="server" Text="Load Sample Query 1" OnClick="btnQueryCustomers_Click" /><br/>
					<asp:Button ID="btnQueryOrders" runat="server" Text="Load Sample Query 2" OnClick="btnQueryOrders_Click"  /><br/>
					<asp:Button ID="btnQueryCustomersOrders" runat="server" Text="Load Sample Query 3" OnClick="btnQueryCustomersOrders_Click"  /><br/>
					<br/>
					<asp:Button ID="btnApply" runat="server" Text="Apply Changes" OnClick="btnApply_Click"  /><br/>
				</td>
			</tr>
		</table>
	</div>
	<div id="tabs-2">
		<AQB:QueryBuilderControl OnSleepModeChanged="SleepModeChanged" ID="QueryBuilderControl1" runat="server" OnInit="QueryBuilderControl1_Init" OnSQLUpdated="QueryBuilderControl1_SQLUpdated" />
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
	</div>
</div>
