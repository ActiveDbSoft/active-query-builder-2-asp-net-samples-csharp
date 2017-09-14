<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.Default" %>

<%@ Register TagPrefix="uc" TagName="QueryBuilderUC" Src="ClientEventHandle.ascx" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>ActiveQueryBilder ASP.NET - Simple Offline Demo</title>
	<link href="jquery.jgrowl.css" type="text/css" rel="stylesheet" />
	<style type="text/css">
		
	</style>
</head>
<body>
	<script type="text/javascript" language="javascript" src="jquery.jgrowl.js"></script>
	<script type="text/javascript" language="javascript">
		OnApplicationReady(function () {
			QB.Web.Application.Canvas.bind(QB.Web.Canvas.Events.CanvasOnAddTable, onAddTableToCanvas);

			QB.Web.Core.bind(QB.Web.Core.Events.DataSending, beforeDataExchange);
			QB.Web.Core.bind(QB.Web.Core.Events.DataReceived, afterDataExchange);
		});

		function beforeDataExchange(e, data) {
			$.jGrowl("Before data exchange", { header: 'Core event' });
		}

		function afterDataExchange(e, data) {
			$.jGrowl("After data exchange", { header: 'Core event' });
		}

		function onAddTableToCanvas(sender, e) {
			$.jGrowl("Add table to canvas", { header: 'Canvas event' });
		}
    </script>
	<form id="Form1" runat="server">
		<uc:QueryBuilderUC ID="QueryBuilderUC1" runat="server" />
	</form>
</body>
</html>
