<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.Default" %>

<%@ Register TagPrefix="uc" TagName="QueryBuilderUC" Src="UserFields.ascx" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>ActiveQueryBilder ASP.NET - Simple OLE DB Demo</title>
    <style type="text/css">
        #qb-ui-tree-view {
            height: auto;
        }

        #qb-ui {
            height: 600px;
        }
    </style>
</head>
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
</body>
</html>
