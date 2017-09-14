<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.Default" %>

<%@ Register TagPrefix="uc" TagName="QueryBuilderUC" Src="QueryBuilderNoDesignArea.ascx" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<link type="text/css" rel="Stylesheet" href="/user.css"/>
	<title>ActiveQueryBilder ASP.NET - No design area demo</title>
</head>
<body>
	<form id="Form1" runat="server">
				<uc:QueryBuilderUC ID="QueryBuilderUC1" runat="server" />
	</form>
</body>
</html>
