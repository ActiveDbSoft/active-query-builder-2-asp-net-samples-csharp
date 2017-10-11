<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.Default" %>

<%@ Register TagPrefix="uc" TagName="QueryBuilderUC" Src="QueryStructure.ascx" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
        	<link type="text/css" rel="Stylesheet" href="/user.css" />
	<link type="text/css" rel="Stylesheet" href="/user.css" />
	<style type="text/css">
		#center {
		}

		#qb-ui-editor textarea {
			height: 100px;
		}

		#alternate-sql {
			border-style: solid;
			border-color: #DDDDDD;
			border-width: 1px;
			display: block;
			position: relative;
			width: 100%;
			overflow: auto;
			top: 0;
			left: 0;
		}

			#alternate-sql textarea {
				width: 100%;
				height: 100px;
				padding: 0;
				border: 0;
				margin: 0;
			}

		.sql {
			position: relative;
			padding: 0;
			width: 100%;
		}

		#qb-ui-canvas {
			height: 244px;
		}


		.ui-tabs {
			padding: 0;
		}

			.ui-tabs .ui-tabs-nav {
				border-left: 0;
				border-right: 0;
				border-top: 0;
				background: none;
				font-weight: bold;
				font-size: 13px;
			}

				.ui-tabs .ui-tabs-nav li a {
					padding: 3px 10px;
					outline: none;
				}

		#qb-ui-editor textarea {
			height: 150px;
		}

		.ui-tabs .ui-tabs-panel {
			padding: 0;
			height: 150px;
			overflow: auto;
		}

		#union-sub-query {
			height: 180px;
		}

		.ui-tabs .ui-tabs-panel textarea, .ui-tabs .ui-tabs-panel {
			font-family: Courier New;
			font-size: 13px;
		}
	</style>
</head>
<body>
	<script language="javascript" type="text/javascript" src="/user.js"></script>
	<script language="javascript" type="text/javascript">
		var SampleSQL = {
			"sample-1":
				"Select 1 as cid, Upper('2'), 3, 4 + 1, 5 + 2 IntExpression ",
			"sample-2":
				"Select c.ID As cid, c.Company, Upper(c.Company), o.Order_ID\n" +
				"From Customers c Inner Join\n" +
				"  Orders o On c.ID = o.Customer_ID\n" +
				"Where c.ID < 10 And o.Order_ID > 0",
			"sample-3":
				"Select c.ID As cid, Upper(c.Company), o.Order_ID + 1, p.Product_Name,\n" +
				"  2 + 2 IntExpression\n" +
				"From Customers c Inner Join\n" +
				"  Orders o On c.ID = o.Customer_ID Inner Join\n" +
				"  Order_Details od On o.Order_ID = od.Order_ID Inner Join\n" +
				"  Products p On p.ID = od.Product_ID",
			"sample-4":
				"Select c.ID As cid, Upper(c.Company), o.Order_ID + 1, p.Product_Name,\n" +
				"  2 + 2 IntExpression\n" +
				"From Customers c Inner Join\n" +
				"  Orders o On c.ID = o.Customer_ID Inner Join\n" +
				"  Order_Details od On o.Order_ID = od.Order_ID Inner Join\n" +
				"  (Select pr.Product_ID, pr.ProductName\n" +
				"    From Products pr) p On p.ID = od.Product_ID",
			"sample-5":
				"Select c.ID As cid, Upper(c.Company), o.Order_ID + 1, p.Product_Name,\n" +
				"  2 + 2 IntExpression\n" +
				"From Customers c Inner Join\n" +
				"  Orders o On c.ID = o.Customer_ID Inner Join\n" +
				"  Order_Details od On o.Order_ID = od.Order_ID Inner Join\n" +
				"  (Select pr.Product_ID, pr.ProductName\n" +
				"    From Products pr) p On p.ID = od.Product_ID\n" +
				"Union All\n" +
				"(Select 1, 2, 3, 4, 5\n" +
				"Union All\n" +
				"Select 6, 7, 8, 9, 0)\n" +
				"Union All\n" +
				"Select (Select Null As [Null]) As EmptyValue, SecondColumn = 2,\n" +
				"  Lower('ThirdColumn') As ThirdColumn, 0 As [Quoted Alias], 2 + 2 * 2"
		}

		$(function () {
			$("#main-tabs").tabs();
			$("#union-sub-query-tabs").tabs();

			$("#sample-selector input").on('click', function (e) {
				$('#sql-code').val(SampleSQL[e.target.id]);

				QB.Web.Application.refreshSql();
			});
		});

		OnApplicationReady(function () {
			QB.Web.Core.bind(QB.Web.Core.Events.UserDataReceived, onUserDataReceived);

		});

		onUserDataReceived = function (e, data) {
			$('#statistics').html(data.Statistics);
			$('#sub-queries').html(data.SubQueries);
			$('#query-structure').html(data.QueryStructure);
			$('#selected-expressions').html(data.UnionSubQuery.SelectedExpressions);
			$('#datasources').html(data.UnionSubQuery.DataSources);
			$('#links').html(data.UnionSubQuery.Links);
			$('#where').html(data.UnionSubQuery.Where);
		};



	</script>
	<form id="Form1" runat="server">
				<uc:QueryBuilderUC ID="QueryBuilderUC1" runat="server" />
	</form>
</body>
</html>
