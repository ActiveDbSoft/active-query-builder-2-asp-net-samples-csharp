<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.Default" %>

<%@ Register TagPrefix="uc" TagName="QueryBuilderUC" Src="MetadataStructure.ascx" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
        	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>ActiveQueryBilder ASP.NET - Simple Offline Demo</title>
	<link type="text/css" rel="Stylesheet" href="user.css" />
    <style type="text/css">
		

		
	</style>
</head>
<body>
	<form id="Form1" runat="server">
			<uc:QueryBuilderUC ID="QueryBuilderUC1" runat="server" />
	</form>
</body>
</html>
