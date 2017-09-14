<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.Default" %>

<%@ Register TagPrefix="uc" TagName="QueryBuilderUC" Src="QueryBuilderOffline.ascx" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
        	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>ActiveQueryBilder ASP.NET - SQL Syntax Highlighting Demo</title>
	<link type="text/css" rel="Stylesheet" href="user.css" />
    <link href="/css/codemirror.css" type="text/css" rel="stylesheet" />
    <style type="text/css">

        #qb-ui-tree-view { height: auto; }
        #qb-ui { height: 600px; }
        #sql-code {
            display: none;
        }

        #qb-ui-editor {
            height: auto;
        }

        .CodeMirror {
            height: auto;
        }

        .CodeMirror-scroll {
            min-height: 250px;
            max-height: 250px;
        }
    </style></head>
<body>
	<form id="Form1" runat="server">
			<uc:QueryBuilderUC ID="QueryBuilderUC1" runat="server" />
	</form>
    <script>
		$(function () {
			$('#qb-ui').resizable({ handles: "s" });
			$('#qb-ui-canvas').resizable({ handles: "s" });
			$('#qb-ui-tree-view').resizable({ handles: "e" });
		});
	</script>
    <script src="/js/codemirror.js"></script>
    <script src="/js/sql.js"></script>
    <script>
        OnApplicationReady(function () {
            var target = document.getElementById('qb-ui-editor');

            window.editor = CodeMirror(target, {
                mode: 'text/x-sql',
                indentWithTabs: true,
                smartIndent: true,
                lineNumbers: true,
                matchBrackets: true
            });

            function importSQL(e, sql) {
                if (window.editor.getValue() != sql) window.editor.setValue(sql);
                $('.qb-ui-editor-refresh-button-container').hide();
            }
            QB.Web.Core.bind(QB.Web.Core.Events.SQLReceived, importSQL, this);

            window.editor.on("change", function (cm, changeObj) {
                var newText = cm.getValue();
                QB.Web.Application.hiddenSQL = newText;
                QB.Web.Core.ClientSQL = newText;
                $('.qb-ui-editor-refresh-button-container').show();
            });
            $(document).on('click', '#error-controls-go-to-position', function () {
                var error = QB.Web.Application.SQLError;
                var container = $('.qb-ui-editor-refresh-button-container');
                container.removeClass('persistent');
                container.hide();
                if (error != null && error.ErrorPos != null) {
                    window.editor.setCursor(error.ErrorPos.line-1, error.ErrorPos.col-1, { scroll: true });
                    window.editor.focus();
                }
            });
            $(document).on('click', '#error-controls-clear', function () {
                window.editor.setValue('');
                QB.Web.Application.refreshButtonClick();
            });
            $(document).on('click', '#error-controls-restore', function () {
                window.editor.setValue(QB.Web.Application.lastGoodSQL);
                QB.Web.Application.refreshButtonClick();
            });

        });
    </script>
</body>
</html>
