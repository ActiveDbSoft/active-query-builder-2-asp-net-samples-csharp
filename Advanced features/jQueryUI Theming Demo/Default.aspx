<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.Default" %>

<%@ Register TagPrefix="uc" TagName="QueryBuilderUC" Src="QueryBuilderOffline.ascx" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>ActiveQueryBilder ASP.NET - Theming</title>
	<link type="text/css" rel="Stylesheet" href="user.css" />
	<style>
		.header {
			height: 32px;
		}
        #switcher .jquery-ui-switcher-link {
            height: 24px!important;
        }
	</style>
</head>
<body>
	<script type="text/javascript" language="JavaScript" src="/jquery.themeswitcher.min.js"></script>
	<form id="Form1" runat="server">
		<script>
			$(document).ready(function () {
				$("#switcher").themeswitcher({
					imgpath: "/themeroller/"
				});
			});
		</script>
		<uc:QueryBuilderUC ID="QueryBuilderUC1" runat="server" />
	</form>
</body>
</html>
