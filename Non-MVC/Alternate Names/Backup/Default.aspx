<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.Default" %>

<%@ Register TagPrefix="uc" TagName="QueryBuilderUC" Src="AlternateNames.ascx" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>ActiveQueryBilder ASP.NET - Alternate Names</title>
	<link type="text/css" rel="Stylesheet" href="user.css" />
</head>
<body>
	<script language="javascript" type="text/javascript" src="user.js"></script>
	<form id="Form1" runat="server">
				<uc:QueryBuilderUC ID="QueryBuilderUC1" runat="server" />
	</form>
</body>
</html>
