<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryBuilderOffline.ascx.cs" Inherits="Samples.QueryBuilderOffline" %>
<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" TagPrefix="AQB" %>
<AQB:QueryBuilderControl OnSleepModeChanged="SleepModeChanged" ID="QueryBuilderControl1" runat="server" OnInit="QueryBuilderControl1_Init" OnSQLUpdated="QueryBuilderControl1_SQLUpdated" />
<table>
	<tr>
		<td colspan="2">
			<asp:TextBox ID="textBox1" runat="server" TextMode="MultiLine" Columns="100" Rows="15"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<asp:Button ID="buttonAnalyzeQuery" runat="server" Text="Analyze query" OnClick="buttonAnalyzeQuery_Click" />
			<asp:Button ID="buttonQueryStatistics" runat="server" Text="Query statistics" OnClick="buttonQueryStatistics_Click" />
			<asp:Button ID="buttonAnalyzeMetadataContainer" runat="server" Text="Analyze metadata container" OnClick="buttonAnalyzeMetadataContainer_Click" />
			<asp:Button ID="buttonAnalyzeWhereClause" runat="server" Text="Analyze WHERE clause" OnClick="buttonAnalyzeWhereClause_Click" />

			<p>See the source code for:</p>
			<p>- how to analyze a query</p>
			<p>- how to fill the metadata container with custom objects</p>
			<p>- how to create a query programmatically</p>
			<p>- how to get objects from the metadata container</p>
		</td>
	</tr>
	<tr>
		<td>
			<asp:TextBox ID="tbResult" runat="server" TextMode="MultiLine" Columns="100" Rows="25"></asp:TextBox>
		</td>
	</tr>
</table>
