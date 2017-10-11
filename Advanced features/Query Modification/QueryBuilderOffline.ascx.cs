using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control;

namespace Samples
{
	public partial class QueryBuilderOffline : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, EventArgs e)
		{
			QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
			tbSQL.Text = QueryBuilderControl1.PlainTextSQLBuilder.SQL;
			// prepare parsed names
			CustomersName = queryBuilder.SQLContext.ParseQualifiedName("Northwind.dbo.Customers");
			CustomersAlias = queryBuilder.SQLContext.ParseIdentifier("c");
			OrdersName = queryBuilder.SQLContext.ParseQualifiedName("Northwind.dbo.Orders");
			OrdersAlias = queryBuilder.SQLContext.ParseIdentifier("o");
			JoinFieldName = queryBuilder.SQLContext.ParseQualifiedName("CustomerId");
			CompanyNameFieldName = queryBuilder.SQLContext.ParseQualifiedName("CompanyName");
			OrderDateFieldName = queryBuilder.SQLContext.ParseQualifiedName("OrderDate");
		}

		protected void SleepModeChanged(object sender, EventArgs e)
		{
			QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
			if (queryBuilder.SleepMode) StatusBar1.Message.Error("Unsupported SQL statement.");
		}

		public void QueryBuilderControl1_Init(object sender, EventArgs e)
		{
			// Get instance of QueryBuilder
			QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
			// Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
			queryBuilder.BehaviorOptions.AllowSleepMode = true;
			// Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
			queryBuilder.BehaviorOptions.AllowSleepMode = true;
			if (queryBuilder.SyntaxProvider == null) queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();
			queryBuilder.OfflineMode = true;
			queryBuilder.MetadataContainer.ImportFromXML(Page.Server.MapPath("Northwind.xml"));
			queryBuilder.MetadataStructure.Refresh();
            StatusBar1.Message.Information("Metadata loaded");
        }

		protected void btn1_Click(object sender, EventArgs e)
		{
			var queryBuilder1 = QueryBuilderControl1.QueryBuilder;

			// prevent QueryBuilder to request metadata
			queryBuilder1.OfflineMode = true;

			queryBuilder1.MetadataProvider = null;

			MetadataContainer metadataContainer = queryBuilder1.MetadataContainer;
			metadataContainer.BeginUpdate();

			try
			{
				metadataContainer.Items.Clear();

				MetadataNamespace schemaDbo = metadataContainer.AddSchema("dbo");

				// prepare metadata for table "Orders"
				MetadataObject orders = schemaDbo.AddTable("Orders");
				// fields
				orders.AddField("OrderId");
				orders.AddField("CustomerId");

				// prepare metadata for table "Order Details"
				MetadataObject orderDetails = schemaDbo.AddTable("Order Details");
				// fields
				orderDetails.AddField("OrderId");
				orderDetails.AddField("ProductId");
				// foreign keys
				MetadataForeignKey foreignKey = orderDetails.AddForeignKey("OrderDetailsToOrders");

				using (MetadataQualifiedName referencedName = new MetadataQualifiedName())
				{
					referencedName.Add("Orders");
					referencedName.Add("dbo");

					foreignKey.ReferencedObjectName = referencedName;
				}

				foreignKey.Fields.Add("OrderId");
				foreignKey.ReferencedFields.Add("OrderId");
			}
			finally
			{
				metadataContainer.EndUpdate();
			}

			queryBuilder1.MetadataStructure.Refresh();
            StatusBar1.Message.Information("Metadata loaded");
        }

		protected void btn2_Click(object sender, EventArgs e)
		{
			var queryBuilder1 = QueryBuilderControl1.QueryBuilder;

			// allow QueryBuilder to request metadata
			queryBuilder1.OfflineMode = false;

			queryBuilder1.MetadataProvider = null;
			queryBuilder1.MetadataContainer.Items.Clear();
			queryBuilder1.MetadataContainer.ItemMetadataLoading += way2ItemMetadataLoading;
			queryBuilder1.MetadataStructure.Refresh();
            StatusBar1.Message.Information("Metadata loaded");
        }

		private void way2ItemMetadataLoading(object sender, MetadataItem item, MetadataType types)
		{
			switch (item.Type)
			{
				case MetadataType.Root:
					if ((types & MetadataType.Schema) > 0) item.AddSchema("dbo");
					break;

				case MetadataType.Schema:
					if ((item.Name == "dbo") && (types & MetadataType.Table) > 0)
					{
						item.AddTable("Orders");
						item.AddTable("Order Details");
					}
					break;

				case MetadataType.Table:
					if (item.Name == "Orders")
					{
						if ((types & MetadataType.Field) > 0)
						{
							item.AddField("OrderId");
							item.AddField("CustomerId");
						}
					}
					else if (item.Name == "Order Details")
					{
						if ((types & MetadataType.Field) > 0)
						{
							item.AddField("OrderId");
							item.AddField("ProductId");
						}

						if ((types & MetadataType.ForeignKey) > 0)
						{
							MetadataForeignKey foreignKey = item.AddForeignKey("OrderDetailsToOrder");
							foreignKey.Fields.Add("OrderId");
							foreignKey.ReferencedFields.Add("OrderId");
							using (MetadataQualifiedName name = new MetadataQualifiedName())
							{
								name.Add("Orders");
								name.Add("dbo");

								foreignKey.ReferencedObjectName = name;
							}
						}
					}
					break;
			}

			item.Items.SetLoaded(types, true);
		}

		private ActiveDatabaseSoftware.ActiveQueryBuilder.EventMetadataProvider way1EventMetadataProvider;
		private ActiveDatabaseSoftware.ActiveQueryBuilder.EventMetadataProvider Way1EventMetadataProvider
		{
			get
			{
				if (way1EventMetadataProvider == null)
				{
					way1EventMetadataProvider = new EventMetadataProvider();
					way1EventMetadataProvider.ExecSQL += new ActiveDatabaseSoftware.ActiveQueryBuilder.ExecSQLEventHandler(this.way3EventMetadataProvider_ExecSQL);
				}
				return way1EventMetadataProvider;
			}
		}

		private void way3EventMetadataProvider_ExecSQL(BaseMetadataProvider metadataProvider, string sql, bool schemaOnly, out IDataReader dataReader)
		{
			dataReader = null;

			if (dbConnection != null)
			{
				IDbCommand command = dbConnection.CreateCommand();
				command.CommandText = sql;
				dataReader = command.ExecuteReader();
			}
		}

		private IDbConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString);
		protected void btn3_Click(object sender, EventArgs e)
		{
			var queryBuilder1 = QueryBuilderControl1.QueryBuilder;

			if (dbConnection != null)
			{
				try
				{
					dbConnection.Close();
					dbConnection.Open();

					// allow QueryBuilder to request metadata
					queryBuilder1.OfflineMode = false;

					ResetQueryBuilderMetadata();

					queryBuilder1.MetadataProvider = Way1EventMetadataProvider;
					queryBuilder1.MetadataStructure.Refresh();
                    StatusBar1.Message.Information("Metadata loaded");
                }
				catch (System.Exception ex)
				{
					StatusBar1.Message.Error(ex.Message);
				}
			}
			else
			{
				StatusBar1.Message.Error("Please setup a database connection by clicking on the \"Connect\" menu item before testing this method.");
			}
		}

		private void ResetQueryBuilderMetadata()
		{
			var queryBuilder1 = QueryBuilderControl1.QueryBuilder;

			queryBuilder1.MetadataProvider = null;
			queryBuilder1.MetadataContainer.Items.Clear();
			queryBuilder1.MetadataContainer.ItemMetadataLoading -= way2ItemMetadataLoading;
		}

		protected void btn4_Click(object sender, EventArgs e)
		{
			var queryBuilder1 = QueryBuilderControl1.QueryBuilder;
			queryBuilder1.MetadataProvider = null;
			queryBuilder1.OfflineMode = true; // prevent QueryBuilder to request metadata from connection

			DataSet dataSet = new DataSet();

			// Load sample dataset created in the Visual Studio with Dataset Designer
			// and exported to XML using WriteXmlSchema() method.
			dataSet.ReadXmlSchema(Path.Combine(Server.MapPath("~/"), "StoredDataSetSchema.xml"));

			queryBuilder1.MetadataContainer.BeginUpdate();

			try
			{
				queryBuilder1.MetadataContainer.Items.Clear();

				// add tables
				foreach (DataTable table in dataSet.Tables)
				{
					// add new metadata table
					MetadataObject metadataTable = queryBuilder1.MetadataContainer.AddTable(table.TableName);

					// add metadata fields (columns)
					foreach (DataColumn column in table.Columns)
					{
						// create new field
						MetadataField metadataField = metadataTable.AddField(column.ColumnName);
						// setup field
						metadataField.FieldType = TypeToDbType(column.DataType);
						metadataField.Nullable = column.AllowDBNull;
						metadataField.ReadOnly = column.ReadOnly;

						if (column.MaxLength != -1)
						{
							metadataField.Size = column.MaxLength;
						}

						// detect the field is primary key
						foreach (DataColumn pkColumn in table.PrimaryKey)
						{
							if (column == pkColumn)
							{
								metadataField.PrimaryKey = true;
							}
						}
					}

					// add relations
					foreach (DataRelation relation in table.ParentRelations)
					{
						// create new relation on the parent table
						MetadataForeignKey metadataRelation = metadataTable.AddForeignKey(relation.RelationName);

						// set referenced table
						using (MetadataQualifiedName referencedName = new MetadataQualifiedName())
						{
							referencedName.Add(relation.ParentTable.TableName);

							metadataRelation.ReferencedObjectName = referencedName;
						}

						// set referenced fields
						foreach (DataColumn parentColumn in relation.ParentColumns)
						{
							metadataRelation.ReferencedFields.Add(parentColumn.ColumnName);
						}

						// set fields
						foreach (DataColumn childColumn in relation.ChildColumns)
						{
							metadataRelation.Fields.Add(childColumn.ColumnName);
						}
					}
				}
			}
			finally
			{
				queryBuilder1.MetadataContainer.EndUpdate();
			}

			queryBuilder1.MetadataStructure.Refresh();
            StatusBar1.Message.Information("Metadata loaded");
        }

		private static DbType TypeToDbType(Type type)
		{
			if (type == typeof(System.String)) return DbType.String;
			else if (type == typeof(System.Int16)) return DbType.Int16;
			else if (type == typeof(System.Int32)) return DbType.Int32;
			else if (type == typeof(System.Int64)) return DbType.Int64;
			else if (type == typeof(System.UInt16)) return DbType.UInt16;
			else if (type == typeof(System.UInt32)) return DbType.UInt32;
			else if (type == typeof(System.UInt64)) return DbType.UInt64;
			else if (type == typeof(System.Boolean)) return DbType.Boolean;
			else if (type == typeof(System.Single)) return DbType.Single;
			else if (type == typeof(System.Double)) return DbType.Double;
			else if (type == typeof(System.Decimal)) return DbType.Decimal;
			else if (type == typeof(System.DateTime)) return DbType.DateTime;
			else if (type == typeof(System.TimeSpan)) return DbType.Time;
			else if (type == typeof(System.Byte)) return DbType.Byte;
			else if (type == typeof(System.SByte)) return DbType.SByte;
			else if (type == typeof(System.Char)) return DbType.String;
			else if (type == typeof(System.Byte[])) return DbType.Binary;
			else if (type == typeof(System.Guid)) return DbType.Guid;
			else return DbType.Object;
		}

		protected void btnQueryCustomers_Click(object sender, EventArgs e)
		{
			QueryBuilderControl1.QueryBuilder.SQL = "select * from Northwind.dbo.Customers c";
		}

		protected void btnQueryOrders_Click(object sender, EventArgs e)
		{
			QueryBuilderControl1.QueryBuilder.SQL = "select * from Northwind.dbo.Orders o";
		}

		protected void btnQueryCustomersOrders_Click(object sender, EventArgs e)
		{
			QueryBuilderControl1.QueryBuilder.SQL = "select * from Northwind.dbo.Customers c, Northwind.dbo.Orders o";
		}

		private SQLQualifiedName CustomersName;
		private AstTokenIdentifier CustomersAlias;
		private SQLQualifiedName OrdersName;
		private AstTokenIdentifier OrdersAlias;
		private SQLQualifiedName JoinFieldName;
		private SQLQualifiedName CompanyNameFieldName;
		private SQLQualifiedName OrderDateFieldName;

		private DataSource Customers;
		private DataSource Orders;
		private new QueryColumnListItem CompanyName;
		private QueryColumnListItem OrderDate;

		protected void btnApply_Click(object sender, EventArgs e)
		{
			var queryBuilder = QueryBuilderControl1.QueryBuilder;

			queryBuilder.BeginUpdate();

			try
			{
				// get the active SELECT
				UnionSubQuery usq = queryBuilder.Query.ActiveUnionSubquery;

				#region actualize stored references (if query is modified in GUI)
				#region actualize datasource references
				// if user removed previously added datasources then clear their references
				if (Customers != null && !IsTablePresentInQuery(usq, Customers))
				{
					// user removed this table in GUI
					Customers = null;
				}

				if (Orders != null && !IsTablePresentInQuery(usq, Orders))
				{
					// user removed this table in GUI
					Orders = null;
				}
				#endregion

				// clear CompanyName conditions
				if (CompanyName != null)
				{
					// if user removed entire row OR cleared expression cell in GUI, clear the stored reference
					if (!IsQueryColumnListItemPresentInQuery(usq, CompanyName))
						CompanyName = null;
				}

				// clear all condition cells for CompanyName row
				if (CompanyName != null)
				{
					ClearConditionCells(usq, CompanyName);
				}

				// clear OrderDate conditions
				if (OrderDate != null)
				{
					// if user removed entire row OR cleared expression cell in GUI, clear the stored reference
					if (!IsQueryColumnListItemPresentInQuery(usq, OrderDate))
						OrderDate = null;
				}

				// clear all condition cells for OrderDate row
				if (OrderDate != null)
				{
					ClearConditionCells(usq, OrderDate);
				}
				#endregion

				#region process Customers table
				if (cbCustomers.Checked || cbCompanyName.Checked)
				{
					// if we have no previously added Customers table, try to find one already added by the user
					if (Customers == null)
					{
						Customers = FindTableInQueryByName(usq, CustomersName);
					}

					// there is no Customers table in query, add it
					if (Customers == null)
					{
						Customers = usq.AddObject(CustomersName, CustomersAlias);
					}

					#region process CompanyName condition
					if (cbCompanyName.Enabled && cbCompanyName.Checked && !String.IsNullOrEmpty(tbCompanyName.Text))
					{
						// if we have no previously added grid row for this condition, add it
						if (CompanyName == null)
						{
							CompanyName = usq.QueryColumnList.AddField(Customers, CompanyNameFieldName.QualifiedNameWithoutQuotes);
							// do not append it to the select list, use this row for conditions only
							CompanyName.Select = false;
						}

						// write condition from edit box to all needed grid cells
						AddWhereCondition(usq.QueryColumnList, CompanyName, tbCompanyName.Text);
					}
					else
					{
						// remove previously added grid row
						if (CompanyName != null)
						{
							CompanyName.Dispose();
						}

						CompanyName = null;
					}
					#endregion
				}
				else
				{
					// remove previously added datasource
					if (Customers != null)
					{
						Customers.Dispose();
					}

					Customers = null;
				}
				#endregion

				#region process Orders table
				if (cbOrders.Checked || cbOrderDate.Checked)
				{
					// if we have no previosly added Orders table, try to find one already added by the user
					if (Orders == null)
					{
						Orders = FindTableInQueryByName(usq, OrdersName);
					}

					// there are no Orders table in query, add one
					if (Orders == null)
					{
						Orders = usq.AddObject(OrdersName, OrdersAlias);
					}

					#region link between Orders and Customers
					// we added Orders table,
					// check if we have Customers table too,
					// and if there are no joins between them, create such join
					string joinFieldNameStr = JoinFieldName.QualifiedNameWithoutQuotes;
					if (Customers != null &&
						usq.FromClause.FindLink(Orders, joinFieldNameStr, Customers, joinFieldNameStr) == null &&
						usq.FromClause.FindLink(Customers, joinFieldNameStr, Orders, joinFieldNameStr) == null)
					{
						usq.AddLink(Customers, JoinFieldName, Orders, JoinFieldName);
					}
					#endregion

					#region process OrderDate condition
					if (cbOrderDate.Enabled && cbOrderDate.Checked && !String.IsNullOrEmpty(tbOrderDate.Text))
					{
						// if we have no previously added grid row for this condition, add it
						if (OrderDate == null)
						{
							OrderDate = usq.QueryColumnList.AddField(Orders, OrderDateFieldName.QualifiedNameWithoutQuotes);
							// do not append it to the select list, use this row for conditions only
							OrderDate.Select = false;
						}

						// write condition from edit box to all needed grid cells
						AddWhereCondition(usq.QueryColumnList, OrderDate, tbOrderDate.Text);
					}
					else
					{
						// remove prviously added grid row
						if (OrderDate != null)
						{
							OrderDate.Dispose();
						}

						OrderDate = null;
					}
					#endregion
				}
				else
				{
					if (Orders != null)
					{
						Orders.Dispose();
						Orders = null;
					}
				}
				#endregion
			}
			finally
			{
				queryBuilder.EndUpdate();
			}

			tbSQL.Text = QueryBuilderControl1.PlainTextSQLBuilder.SQL;
		}

		private bool IsTablePresentInQuery(UnionSubQuery unionSubQuery, DataSource table)
		{
			// collect the list of datasources used in FROM
			List<DataSource> dataSources = new List<DataSource>();
			unionSubQuery.FromClause.GetDatasources(dataSources);

			// check given table in list of all datasources
			return dataSources.IndexOf(table) != -1;
		}

		private bool IsQueryColumnListItemPresentInQuery(UnionSubQuery unionSubQuery, QueryColumnListItem item)
		{
			return unionSubQuery.QueryColumnList.IndexOf(item) == -1 || String.IsNullOrEmpty(item.ExpressionString);
		}

		private void ClearConditionCells(UnionSubQuery unionSubQuery, QueryColumnListItem item)
		{
			for (int i = 0; i < unionSubQuery.QueryColumnList.GetMaxConditionCount(); i++)
			{
				item.ConditionStrings[i] = "";
			}
		}

		private DataSource FindTableInQueryByName(UnionSubQuery unionSubQuery, SQLQualifiedName name)
		{
			List<DataSource> foundDatasources = new List<DataSource>();
			unionSubQuery.FromClause.FindTablesByDBName(name, foundDatasources);

			// if found more than one tables with given name in the query, use the first one
			return foundDatasources.Count > 0 ? foundDatasources[0] : null;
		}

		private void AddWhereCondition(QueryColumnList columnList, QueryColumnListItem whereListItem, string condition)
		{
			bool whereFound = false;

			// GetMaxConditionCount: returns the number of non-empty criteria columns in the grid.
			for (int i = 0; i < columnList.GetMaxConditionCount(); i++)
			{
				// CollectCriteriaItemsWithWhereCondition:
				// This function returns the list of conditions that were found in
				// the i-th criteria column, applied to specific clause (WHERE or HAVING).
				// Thus, this function collects all conditions joined with AND
				// within one OR group (one grid column).
				List<QueryColumnListItem> foundColumnItems = new List<QueryColumnListItem>();
				CollectCriteriaItemsWithWhereCondition(columnList, i, foundColumnItems);

				// if found some conditions in i-th column, append condition to i-th column
				if (foundColumnItems.Count > 0)
				{
					whereListItem.ConditionStrings[i] = condition;
					whereFound = true;
				}
			}

			// if there are no cells with "where" conditions, add condition to new column
			if (!whereFound)
			{
				whereListItem.ConditionStrings[columnList.GetMaxConditionCount()] = condition;
			}
		}
		private void CollectCriteriaItemsWithWhereCondition(QueryColumnList criteriaList, int columnIndex, IList<QueryColumnListItem> result)
		{
			result.Clear();

			for (int i = 0; i < criteriaList.Count; i++)
			{
				if (criteriaList[i].ConditionType == ConditionType.Where &&
					criteriaList[i].ConditionCount > columnIndex &&
					criteriaList[i].GetASTCondition(columnIndex) != null)
				{
					result.Add(criteriaList[i]);
				}
			}
		}

		protected void QueryBuilderControl1_SQLUpdated(object sender, EventArgs e, ClientData clientdata)
		{
			tbSQL.Text = QueryBuilderControl1.PlainTextSQLBuilder.SQL;
		}


	}
}