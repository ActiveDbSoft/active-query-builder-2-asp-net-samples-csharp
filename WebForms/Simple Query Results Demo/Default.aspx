<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.Default" %>

<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" TagPrefix="AQB" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>ActiveQueryBilder ASP.NET - Query Results Demo</title>
    <style>
        #tabs {
            width: 1170px;
        }

        .ui-datepicker {
            font-size: 62.5%;
        }

        #all {
            float: none;
        }

        #qb-ui-editor textarea {
            font-size: 16px;
            font-family: monospace;
        }

        .ui-tabs-nav {
            border: 0;
        }

        #qb-ui-canvas {
            height: 297px;
        }

        #qb-ui-grid {
            height: 157px;
        }

        #queryResults {
            font-size: 12px;
            overflow: auto;
        }

        #dataGridView1 th {
            padding: 0 2px;
        }

        .ui-tabs .ui-tabs-panel {
            padding: 2px;
        }

        #qb-ui-criteria-builder {
            margin-left: 0;
        }

        #dialog-form table {
            color: #222222;
            border: 1px solid #AAAAAA;
            border-right: 0;
            border-bottom: 0;
            width: 100%;
        }

            #dialog-form table.noborder, #dialog-form table.noborder td {
                border: none;
            }

            #dialog-form table th {
                border: 1px solid #D3D3D3;
                color: #333333;
            }

            #dialog-form table td, #dialog-form table th {
                padding: 2px 4px 2px 4px;
                border: 1px solid #AAAAAA;
                border-left: 0;
                border-top: 0;
            }

        #dialog-form input {
            width: 100%;
            padding: 0;
        }

        .ui-tabs .ui-tabs-nav li {
            height: 35px;
            font-size: 12px;
        }

            .ui-tabs .ui-tabs-nav li a {
                padding: 3px 6px;
                line-height: 22px;
            }

        #tabs .ui-tabs-nav .ui-icon {
            float: left;
        }

        #tabs .ui-icon-bricks {
            background: url("/img/icons/bricks.ico") center center no-repeat;
            width: 22px;
            height: 22px;
        }

        #tabs .ui-icon-database-go {
            background: url("/img/icons/database_go.ico") center center no-repeat;
            width: 22px;
            height: 22px;
        }
        
        div.slideup,
        div.slidedown {
            margin-top: 0;
        }

        #qb-ui-editor-statusbar {
            position: relative;
        }

        #dataGridView1 td,
        #dataGridView1 th
        {
            padding: 1px 2px;
        }

    </style>

</head>
<body>
    <form id="Form1" runat="server">
        <script type="text/javascript">
            var $dialog = null;
            var params = [];

            $(function () {
                $.datepicker.setDefaults({ dateFormat: '<%=System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern.Replace("M", "m").Replace("yy", "y")%>' });
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

			    QB.Web.Application.CriteriaBuilder.on(QB.Web.CriteriaBuilder.Events.CriteriaBuilderChanged, function(e, args) {
			        if (args.criteriaBuilder.isValid())
			            executeQuery();
			    });
			});

            function executeQuery() {
                QB.Web.Application.syncCriteriaBuilder(function () {
                    if (QB.Web.Application.SQL.trim() === '')
                        return alert('Query is empty');

                    var button = document.getElementById("<%= Button1.ClientID %>");
                    var name = button.name;

                    if (name == null || name == '')
                        name = button.id;

                    __doPostBack(name, "");
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


        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div id="tabs">
            <ul>
                <li><a href="#queryBuilder"><span class="ui-icon ui-icon-bricks"></span>Query Builder</a></li>
                <li><a href="#queryResults"><span class="ui-icon ui-icon-database-go"></span>Query Results</a></li>
            </ul>
            <div id="queryBuilder">

                <AQB:QueryBuilderControl OnSleepModeChanged="SleepModeChanged" ID="QueryBuilderControl1" runat="server" OnInit="QueryBuilderControl1_Init" />
                <div id="all">
                    <div id="dialog-form" title="Query parameters" style="display: none"></div>
                    <div id="content-container">
                        <div id="qb-ui">
                            <AQB:ObjectTreeView  ID="ObjectTree1" runat="server" SortingType="Name" />
                            <div id="center">
                                <AQB:SubQueryNavigationBar ID="SubQueryNavigationBar1" runat="server" />

                                <div class="qb-ui-canvas-container block-flat">
                                    <AQB:Canvas ID="Canvas1" runat="server" />
                                    <AQB:StatusBar ID="StatusBar1" runat="server"/>
                                    <AQB:Grid ID="Grid1" runat="server" />
                                    <div id="qb-ui-editor-statusbar">
                                        <div id="qb-ui-editor-statusbar-message"></div>
                                        <div id="qb-ui-editor-statusbar-controls">
                                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Execute query" OnClientClick="return clientClick();" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="clear">
                            </div>
                        </div>
                    </div>
                    <AQB:SqlEditor ID="SQLEditor1" runat="server"></AQB:SqlEditor>
                </div>
            </div>
            <div id="queryResults">
                <AQB:CriteriaBuilder ID="CriteriaBuilder1" runat="server" />
                <br />
                <br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="false">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="messageLabel" ForeColor="red"></asp:Label>
                        <asp:GridView ID="dataGridView1" runat="server" EnableViewState="false" AllowPaging="True" AllowSorting="True" EnableSortingAndPagingCallbacks="True" OnPageIndexChanging="OnPageIndexChanging" OnSorting="OnSorting"></asp:GridView>
                    <textarea style="width: 100%; height: 200px;"> <%= CriteriaBuilder1.QueryTransformer.Sql %></textarea>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Button1" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
