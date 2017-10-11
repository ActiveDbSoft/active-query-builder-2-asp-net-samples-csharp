<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.Default" %>

<%@ Register TagPrefix="uc" TagName="QueryBuilderUC" Src="QueryBuilderOffline.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>ActiveQueryBilder ASP.NET - Simple Offline Demo</title>
	<link type="text/css" rel="Stylesheet" href="user.css" />
	<link href="accessibility.css" rel="stylesheet" type="text/css" media="all">
	<link href="css_accessibility/themes/jquery-ui.css" rel="stylesheet" type="text/css" media="all">
	<style type="text/css">
		body {
			font-size: 20px;
		}

		.qb-ui-tree .hitarea {
			margin-top: 0.3em;
		}

		.qb-ui-tree li.collapsable, .qb-ui-tree li.expandable {
			background-position: 0 -171px;
		}


		#qb-ui-editor textarea {
			font-size: 2em;
		}

		#qb-ui-editor-statusbar {
			line-height: 35px;
			min-height: 35px;
		}

		#qb-ui-editor-statusbar-message {
			font-size: 1.5em;
			line-height: 35px;
		}

		

        .qb-ui-tree span.table, .qb-ui-tree li.expandable span.table {
            height: auto;
        }
	</style>

</head>
<body>
	<script type="text/javascript" language="javascript" src="accessibility.js"> </script>
	<form id="Form1" runat="server">
				<uc:QueryBuilderUC ID="QueryBuilderUC1" runat="server" />
	</form>
</body>
</html>
