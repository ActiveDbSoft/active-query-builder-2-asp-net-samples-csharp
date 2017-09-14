using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control;
using log4net.Core;

namespace Samples
{
	public partial class QueryBuilderOffline : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, EventArgs e)
		{

			if (!Page.IsPostBack)
			{
				QueryBuilderControl1.QueryBuilder.SQL = @"SELECT SUM(o.Payment) AS Total, c.Name, c.Address
FROM MyDB.MySchema.Orders o
	INNER JOIN MyDB.MySchema.Customers c ON o.CustomerID = c.CustomerID
		LEFT JOIN MyDB.MySchema.OrderDetails od ON od.OrderID = o.OrderID
WHERE
	o.Special = 1
	OR
		o.CustomerID = c.CustomerID
		AND
		o.OrderID > 0
GROUP BY o.CustomerID, c.Name, c.Address
HAVING SUM(o.Payment) > 1000
ORDER BY Total DESC, c.Name";
				textBox1.Text = QueryBuilderControl1.PlainTextSQLBuilder.SQL;
			}
			else
			{
				try
				{
					QueryBuilderControl1.QueryBuilder.SQL = textBox1.Text;
				}
				catch (System.Exception ex)
				{
					tbResult.Text = ex.Message;
					return;
				}
			}
		}

		protected void SleepModeChanged(object sender, EventArgs e)
		{
			QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
		}

		public void QueryBuilderControl1_Init(object sender, EventArgs e)
		{
			// Get instance of QueryBuilder
			QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
			// Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
			queryBuilder.BehaviorOptions.AllowSleepMode = true;
			if (queryBuilder.SyntaxProvider == null)
				queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();
			queryBuilder.OfflineMode = true;
			queryBuilder.MetadataContainer.ImportFromXML(Page.Server.MapPath("Northwind.xml"));
			queryBuilder.MetadataStructure.Refresh();
        }

		protected void QueryBuilderControl1_SQLUpdated(object sender, EventArgs e, ClientData clientdata)
		{
		}

		protected void buttonAnalyzeQuery_Click(object sender, EventArgs e)
		{
			var queryBuilder = QueryBuilderControl1.QueryBuilder;
			try
			{
				queryBuilder.SQL = textBox1.Text;
			}
			catch (System.Exception ex)
			{
				tbResult.Text = ex.Message;
				return;
			}
			AnalyzeQuery();
		}

		protected void buttonQueryStatistics_Click(object sender, EventArgs e)
		{
			var queryBuilder = QueryBuilderControl1.QueryBuilder;
			try
			{
				queryBuilder.SQL = textBox1.Text;
			}
			catch (System.Exception ex)
			{
				tbResult.Text = ex.Message;
				return;
			}
			QueryStatistics();
		}

		protected void buttonAnalyzeMetadataContainer_Click(object sender, EventArgs e)
		{
			var queryBuilder = QueryBuilderControl1.QueryBuilder;
			try
			{
				queryBuilder.SQL = textBox1.Text;
			}
			catch (System.Exception ex)
			{
				tbResult.Text = ex.Message;
				return;
			}
			AnalyzeMetadataContainerContent();
		}

		protected void buttonAnalyzeWhereClause_Click(object sender, EventArgs e)
		{

			var queryBuilder = QueryBuilderControl1.QueryBuilder;
			try
			{
				queryBuilder.SQL = textBox1.Text;
			}
			catch (System.Exception ex)
			{
				tbResult.Text = ex.Message;
				return;
			}

			AnalyzeWhereClause();

		}

		public void AnalyzeWhereClause()
		{
			var queryBuilder = QueryBuilderControl1.QueryBuilder;


			// find a union-subquery to analyze
			SQLSubQuerySelectExpression select = FindFirstSelect(queryBuilder.ResultQueryAST);

			if (select.Where != null)
			{

				tbResult.Text = LoadCondition(0, select.Where);

			}
		}

		private string LoadCondition(int level, SQLExpressionItem condition)
		{
			string result = "";
			String s;

			if (condition == null)
			{
				// NULL reference protection
				Append(ref result, level, "--null--");
			}
			else if (condition is SQLExpressionBrackets)
			{
				// The condition is actually the "Brackets" structural node.
				// Create the "brackets" tree node and load the brackets content as children tree nodes.
				Append(ref result, level, "( )");
				Append(ref result, LoadCondition(level + 1, ((SQLExpressionBrackets)condition).LExpression));
			}
			else if (condition is SQLExpressionOr)
			{
				// The condition is actually the "OR Collection" structural node.
				// Create the "OR" tree node and load all items of the collection as children tree nodes.
				Append(ref result, level, "OR");

				for (int i = 0; i < ((SQLExpressionOr)condition).Count; i++)
				{
					Append(ref result, LoadCondition(level + 1, ((SQLExpressionOr)condition)[i]));
				}
			}
			else if (condition is SQLExpressionAnd)
			{
				// The condition is actually the "AND Collection" structural node.
				// Create the "AND" tree node and load all items of the collection as children tree nodes.
				Append(ref result, level, "AND");

				for (int i = 0; i < ((SQLExpressionAnd)condition).Count; i++)
				{
					Append(ref result, LoadCondition(level + 1, ((SQLExpressionAnd)condition)[i]));
				}
			}
			else if (condition is SQLExpressionNot)
			{
				// The condition is actually the "NOT" structural node.
				// Create the "NOT" tree node and load content of the "NOT" operator as children tree nodes.
				Append(ref result, level, "NOT");
				Append(ref result, LoadCondition(level + 1, ((SQLExpressionNot)condition).LExpression));
			}
			else if (condition is SQLExpressionOperatorBinary)
			{
				// The condition is actually the "Binary Operator" structural node.
				// Create a tree node containing the operator value and two leaf nodes with the operator arguments.
				s = ((SQLExpressionOperatorBinary)condition).OperatorObj.OperatorName;
				Append(ref result, level, s);
				// left argument of the binary operator
				Append(ref result, LoadCondition(level + 1, ((SQLExpressionOperatorBinary)condition).LExpression));
				// right argument of the binary operator
				Append(ref result, LoadCondition(level + 1, ((SQLExpressionOperatorBinary)condition).RExpression));
			}
			else
			{
				// Other types of structureal nodes.
				// Just show them as a text.
				s = condition.GetSQL(condition.SQLContext.SQLBuilderExpression);
				Append(ref result, level, s);
			}

			return result;
		}

		private SQLSubQuerySelectExpression FindFirstSelect(SQLSubQueryExpressions queryGroup)
		{
			SQLSubQuerySelectExpression result = null;

			for (int i = 0; i < queryGroup.Count; i++)
			{
				if (queryGroup[i] is SQLSubQuerySelectExpression)
				{
					return (SQLSubQuerySelectExpression)queryGroup[i];
				}
				else if (queryGroup[i] is SQLSubQueryExpressions)
				{
					result = FindFirstSelect((SQLSubQueryExpressions)queryGroup[i]);

					if (result != null)
					{
						return result;
					}
				}
			}

			return result;
		}

		public void AnalyzeMetadataContainerContent()
		{

			var queryBuilder = QueryBuilderControl1.QueryBuilder;

			StringBuilder s;
			StringBuilder tables = new StringBuilder("Tables:\r\n");
			StringBuilder views = new StringBuilder("Views:\r\n");

			IList<MetadataObject> metadataObjects = queryBuilder.MetadataContainer.FindItems<MetadataObject>(MetadataType.Objects);

			foreach (MetadataObject mo in metadataObjects)
			{
				if (mo.Type == MetadataType.Table)
				{
					s = tables;
				}
				else if (mo.Type == MetadataType.View)
				{
					s = views;
				}
				else
				{
					s = null; // process only tables and views
				}

				if (s != null)
				{
					// append object name
					s.Append("\t" + mo.NameFull + "\r\n");

					// append fields
					foreach (MetadataField referencingField in mo.Items.Fields)
					{
						// append field name
						s.Append("\t\t" + referencingField.Name);

						// search relations for this field
						foreach (MetadataForeignKey fk in mo.Items.ForeignKeys)
						{
							for (int i = 0; i < fk.Fields.Count; i++)
							{
								if (fk.Fields[i] == referencingField.Name)
								{
									s.Append(" -- > " + fk.ReferencedObject.NameFull + "." + fk.ReferencedFields[i]);
								}
							}
						}

						s.Append("\r\n");
					}
				}
			}

			tbResult.Text = tables.ToString() + "\r\n" + views.ToString();
		}

		public void QueryStatistics()
		{
			var queryBuilder = QueryBuilderControl1.QueryBuilder;

			string stats = "";

			QueryStatistics qs = queryBuilder.QueryStatistics;

			stats = "Used Objects (" + qs.UsedDatabaseObjects.Count + "):\r\n";
			for (int i = 0; i < qs.UsedDatabaseObjects.Count; i++)
			{
				stats += "\r\n" + qs.UsedDatabaseObjects[i].ObjectName.QualifiedName;
			}

			stats += "\r\n\r\n" + "Used Columns (" + qs.UsedDatabaseObjectFields.Count + "):\r\n";
			for (int i = 0; i < qs.UsedDatabaseObjectFields.Count; i++)
			{
				stats += "\r\n" + qs.UsedDatabaseObjectFields[i].FullName.QualifiedName;
			}

			stats += "\r\n\r\n" + "Output Expressions (" + qs.OutputColumns.Count + "):\r\n";
			for (int i = 0; i < qs.OutputColumns.Count; i++)
			{
				stats += "\r\n" + qs.OutputColumns[i].Expression;
			}

			tbResult.Text = stats;
		}

		public void AnalyzeQuery()
		{
			var queryBuilder = QueryBuilderControl1.QueryBuilder;

			// if you want to hide default database and schema from object names, uncomment lines below
			//queryBuilder.MetadataContainer.DefaultDatabaseNameStr = "MyDB";
			//queryBuilder.MetadataContainer.DefaultSchemaNamesStr = "MySchema";

			// fill the tree with query structure

			// Note: if you don't need to analyze the whole query structure
			// with all sub-queries and unions, you may simply use the
			// queryBuilder.Query.FirstSelect to get access to the main query.
			// You may uncomment the next line and comment the line aftet that
			// to load information about main query only.

			//LoadUnionSubQuery(f.treeView1.Nodes, queryBuilder.Query.FirstSelect);

			var result = LoadQuery(0, queryBuilder.Query);

			tbResult.Text = result;

			// show the XML structure of the query in the browser control

			string tempFile = System.IO.Path.GetTempPath() + "temp.xml";

			System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(tempFile, null);
			writer.Formatting = System.Xml.Formatting.Indented;

			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
			doc.LoadXml(queryBuilder.Query.StructureXml);
			doc.WriteTo(writer);
			writer.Flush();
			writer.Close();


			File.Delete(tempFile);
		}

		private void Append(ref string src, string str)
		{
			Append(ref src, 0, str);
		}
		private void Append(ref string src, int level, string str)
		{
			for (var i = 0; i < level; i++)
			{
				src += "  ";
			}
			src += str + "\n";
		}

		private string LoadQuery(int level, SubQuery q)
		{
			var result = "";
			Append(ref result, level, q.Caption);

			// union parts
			if (q.Count == 1 && q[0] is UnionSubQuery)
			{
				Append(ref result, LoadUnionSubQuery(level + 1, (UnionSubQuery)q[0]));
			}
			else
			{
				Append(ref result, LoadUnionGroup(level + 1, q, "Unions"));
			}

			// subqueries
			for (int i = 0; i < q.SubQueryCount; i++)
			{
				Append(ref result, LoadQuery(level + 1, (SubQuery)q.SubQueries[i]));
			}

			return result;
		}

		private string LoadUnionGroup(int level, UnionGroup unionGroup, String caption)
		{
			string unionNode = "";

			Append(ref unionNode, level, caption);

			for (int i = 0; i < unionGroup.Count; i++)
			{
				if (unionGroup[i] is UnionGroup)
				{
					Append(ref unionNode, LoadUnionGroup(level + 1, (UnionGroup)unionGroup[i], "( )"));
				}
				else if (unionGroup[i] is UnionSubQuery)
				{
					UnionSubQuery unionSubQuery = (UnionSubQuery)unionGroup[i];
					Append(ref unionNode, level, unionSubQuery.GetResultSQL(unionSubQuery.SQLContext.SQLBuilderExpressionForServer));
					Append(ref unionNode, LoadUnionSubQuery(level + 1, unionSubQuery));
				}
			}
			return unionNode;
		}

		private string LoadUnionSubQuery(int level, UnionSubQuery unionSubQuery)
		{
			// Notice: At this stage you can retrieve whole sub-query or it's different parts in string form:
			// whole sub-query:
			//			String unionSubqueryString = unionSubQuery.GetResultSQL();
			// parts:
			// 			String selectListString = unionSubQuery.SelectListString;
			// 			String fromString = unionSubQuery.FromClauseString;
			// 			String whereString = unionSubQuery.WhereClauseString;
			// 			String groupByString = unionSubQuery.GroupByClauseString;
			// 			String havingString = unionSubQuery.HavingClauseString;
			// 			String orderByString = unionSubQuery.OrderByClauseString;


			var listNode = "";
			Append(ref listNode, level, "Fields");

			// Get structure of union subquery components:

			// SELECT list

			QueryColumnList QueryColumnList = unionSubQuery.QueryColumnList;

			for (int i = 0; i < QueryColumnList.Count; i++)
			{
				QueryColumnListItem QueryColumnListItem = QueryColumnList[i];

				if (QueryColumnListItem.Select)
				{
					string s = QueryColumnListItem.ExpressionString;

					if (QueryColumnListItem.Aggregate != null)
					{
						s = QueryColumnListItem.AggregateString + "(" + s + ")";
					}

					if (!String.IsNullOrEmpty(QueryColumnListItem.AliasString))
					{
						s += " " + QueryColumnListItem.AliasString;
					}

					Append(ref listNode, level + 1, s);
				}
			}

			// FROM clause

			if (unionSubQuery.FromClause.Count > 0)
			{
				Append(ref listNode, level, "FROM");

				Append(ref listNode, LoadFromGroup(level + 1, unionSubQuery.FromClause));
			}

			// WHERE clause

			// compute ORs count
			int orsCount = 0;

			for (int i = 0; i < QueryColumnList.Count; i++)
			{
				orsCount = Math.Max(QueryColumnList[i].ConditionCount, orsCount);
			}

			// collect conditions
			if (orsCount > 0)
			{
				Append(ref listNode, level, "WHERE");

				for (int or = 0; or < orsCount; or++)
				{
					// new OR item
					Append(ref listNode, level + 1, "OR");

					for (int i = 0; i < QueryColumnList.Count; i++)
					{
						QueryColumnListItem QueryColumnListItem = QueryColumnList[i];

						if (QueryColumnListItem.ConditionType == ConditionType.Where && !String.IsNullOrEmpty(QueryColumnListItem.ConditionStrings[or]))
						{
							// add AND item
							Append(ref listNode, level + 2, "AND " + QueryColumnListItem.ExpressionString + " " + QueryColumnListItem.ConditionStrings[or]);
						}
					}
				}
			}

			// GROUP BY clause

			// compute GROUPBYs count
			int groupbyCount = 0;

			for (int i = 0; i < QueryColumnList.Count; i++)
			{
				if (QueryColumnList[i].Grouping)
				{
					groupbyCount++;
				}
			}

			if (groupbyCount > 0)
			{
				Append(ref listNode, level, "GROUP BY");

				for (int i = 0; i < QueryColumnList.Count; i++)
				{
					if (QueryColumnList[i].Grouping)
					{
						Append(ref listNode, level + 1, QueryColumnList[i].ExpressionString);
					}
				}
			}

			// HAVING clause
			int havingsCount = 0;

			for (int or = 0; or < orsCount; or++)
			{
				for (int i = 0; i < QueryColumnList.Count; i++)
				{
					QueryColumnListItem QueryColumnListItem = QueryColumnList[i];

					if (QueryColumnListItem.ConditionType == ConditionType.Having &&
						!String.IsNullOrEmpty(QueryColumnListItem.ConditionStrings[or]))
					{
						havingsCount++;
					}
				}
			}

			if (havingsCount > 0)
			{
				Append(ref listNode, level, "HAVING");

				for (int or = 0; or < orsCount; or++)
				{
					// new OR item
					Append(ref listNode, level + 1, "OR");

					for (int i = 0; i < QueryColumnList.Count; i++)
					{
						QueryColumnListItem QueryColumnListItem = QueryColumnList[i];

						if (QueryColumnListItem.ConditionType == ConditionType.Having &&
							!String.IsNullOrEmpty(QueryColumnListItem.ConditionStrings[or]))
						{
							// add AND item
							Append(ref listNode, level + 2, "AND " + QueryColumnListItem.ExpressionString + " " + QueryColumnListItem.ConditionStrings[or]);
						}
					}
				}
			}

			// ORDER BY clause

			// collect ORDERBYs items

			List<QueryColumnListItem> orderbyList = new List<QueryColumnListItem>();

			for (int i = 0; i < QueryColumnList.Count; i++)
			{
				if (!String.IsNullOrEmpty(QueryColumnList[i].SortOrderString))
				{
					orderbyList.Add(QueryColumnList[i]);
				}
			}

			if (orderbyList.Count > 0)
			{
				// sort ORDER BY items by theirs index
				orderbyList.Sort(CompareOrderByItems);

				Append(ref listNode, level, "ORDER BY");

				for (int i = 0; i < orderbyList.Count; i++)
				{
					string s = orderbyList[i].ExpressionString + " " + orderbyList[i].SortTypeString;
					Append(ref listNode, level + 1, s);
				}
			}
			return listNode;
		}

		private static int CompareOrderByItems(QueryColumnListItem x, QueryColumnListItem y)
		{
			if (x.SortOrder > y.SortOrder)
			{
				return 1;
			}

			if (x.SortOrder < y.SortOrder)
			{
				return -1;
			}

			return 0;
		}

		private string LoadFromGroup(int level, DataSourceGroup f)
		{
			string result = "";
			for (int i = 0; i < f.Count; i++)
			{
				if (f[i] is DataSourceGroup)
				{
					LoadFromGroup(level, (DataSourceGroup)f[i]);
				}
				else
				{
					Append(ref result, level, f[i].GetResultSQL(f[i].SQLContext.SQLBuilderExpressionForServer));
				}
			}

			if (f is DataSourceGroup) // FROM clause
			{
				if (f.LinkCount > 0)
				{
					Append(ref result, level, "LINKS");

					for (int i = 0; i < f.LinkCount; i++)
					{
						string join;

						if (f.Links[i].LeftType == LinkSideType.Inner)
						{
							if (f.Links[i].RightType == LinkSideType.Inner)
							{
								join = "INNER JOIN";
							}
							else
							{
								join = "RIGHT JOIN";
							}
						}
						else if (f.Links[i].RightType == LinkSideType.Inner)
						{
							join = "LEFT JOIN";
						}
						else
						{
							join = "FULL JOIN";
						}

						join += " " + f.Links[i].LinkExpression.GetSQL(f.SQLContext.SQLBuilderExpressionForServer);

						Append(ref result, level + 1, join);
					}
				}
			}
			return result;
		}
	}
}