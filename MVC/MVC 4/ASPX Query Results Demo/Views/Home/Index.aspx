<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" TagPrefix="AQB" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Home Page - My ASP.NET MVC Application
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
		var $dialog = null;
		var params = [];

		$(function () {
			$.datepicker.setDefaults({ dateFormat: '@System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern.Replace("M", "m").Replace("yy", "y")' });
			$("#tabs").tabs({
				activate: function (event, ui) {
					if (ui.newTab.index() == 1) {
						QB.Web.Application.getQueryParams(getQueryParams);
					}
				}
			});
		});

		OnApplicationReady(function () {
			$dialog = $("#dialog-form").dialog({
				autoOpen: false,
				height: 300,
				width: 350,
				modal: true,
				buttons: {
					"OK": function () {
						$("#dialog-form input").each(function (index, input) {
							params[index].Value = $(input).val();
						});
						$(this).dialog("close");
						QB.Web.Application.setQueryParamValues(params, executeQuery);
					}
				},
				close: function () {

				}
			});

		    QB.Web.Application.CriteriaBuilder.on(QB.Web.CriteriaBuilder.Events.CriteriaBuilderChanged, function(event, args) {
                if(args.criteriaBuilder.isValid())
		            executeQuery();
		    });
		});

		function executeQuery(data) {
			QB.Web.Application.syncCriteriaBuilder(function () {
				$.post('<%=Url.Action("RefreshQueryResultPartial")%>', function (data) {
				    $('#QueryResultPartialDiv').html(data);
				});
		});
	}

	function getQueryParams(queryParams) {
		params = queryParams;

		if (params == null || params.length == 0) {
			executeQuery();
			return;
		}

		$("#dialog-form").html('Loading....');
		$("#dialog-form").dialog('open');

		var html = '<table class="ui-widget ui-qb-grid" cellspacing="0" cellpadding="0" border="0">';
		html += '<tr><th>Name</th><th>DbType</th><th width="100%">Value</th></tr>';
		for (var i = 0; i < params.length; i++) {
			var param = params[i];
			html += '<tr>';
			html += '<td>' + param.FullName + '</td>';
			html += '<td>' + param.DataType + '</td>';
			html += '<td><input type="text"></td>';
			html += '</tr>';
		}
		html += '</table>';
		$("#dialog-form").html(html);
	}

	function clientClick() {
		$('#tabs').tabs('option', 'active', 1);
		return false;
	}

	</script>

	<div id="dialog-form" title="Query parameters" style="display: none"></div>
	<div id="tabs">
		<ul>
			<li><a href="#queryBuilder"><span class="ui-icon ui-icon-bricks"></span>Query Builder</a></li>
			<li><a href="#queryResults"><span class="ui-icon ui-icon-database-go"></span>Query Results</a></li>
		</ul>

		<div id="queryBuilder">
			<AQB:QueryBuilderControl ID="QueryBuilderControl1" runat="server" LoadJScript="False" />
			<div id="all">
				<div id="content-container">
					<div id="qb-ui">
						<AQB:ObjectTreeView  ID="ObjectTree1" runat="server" />
						<div id="center">
							<AQB:SubQueryNavigationBar ID="SubQueryNavigationBar1" runat="server" />
				<div class="qb-ui-canvas-container block-flat">

							<AQB:Canvas ID="Canvas1" runat="server" />
							<AQB:Grid ID="Grid1" runat="server" />
							<div id="qb-ui-editor-statusbar">
								<div id="qb-ui-editor-statusbar-message">
								</div>
								<div id="qb-ui-editor-statusbar-controls">
									<input type="button" value="Execute query" onclick="return clientClick();" />
								</div>
							</div>
						</div></div>

						<div class="clear">
						</div>
					</div>
				</div>
				<AQB:SqlEditor ID="SQLEditor1" runat="server"></AQB:SqlEditor>
			</div>
		</div>
		<div id="queryResults">
			<AQB:CriteriaBuilder ID="CriteriaBuilder1" runat="server"></AQB:CriteriaBuilder>
			<br />
			<br />
			<div id="QueryResultPartialDiv"></div>
		</div>
	</div>
</asp:Content>
