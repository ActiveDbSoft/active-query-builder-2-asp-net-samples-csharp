using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ActiveDatabaseSoftware.ActiveQueryBuilder;

namespace Samples
{
	public partial class QueryBuilderOffline : System.Web.UI.UserControl
	{
	    private IDbConnection dbConnection;

        protected void Page_Load(object sender, EventArgs e)
        {
            var connString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            dbConnection = new SqlConnection(connString);
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
			if (queryBuilder.SyntaxProvider == null) // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
				queryBuilder.BehaviorOptions.AllowSleepMode = true;
			queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();
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

		private EventMetadataProvider way1EventMetadataProvider;
		private EventMetadataProvider Way1EventMetadataProvider
		{
			get
			{
				if (way1EventMetadataProvider == null)
				{
					way1EventMetadataProvider = new EventMetadataProvider();
					way1EventMetadataProvider.ExecSQL += way3EventMetadataProvider_ExecSQL;
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

        protected void btn3_Click(object sender, EventArgs e)
		{
			var queryBuilder1 = QueryBuilderControl1.QueryBuilder;

			if (dbConnection != null)
			{
				try
				{
					dbConnection.Close();
					dbConnection.Close();
					dbConnection.Open();

					// allow QueryBuilder to request metadata
					queryBuilder1.OfflineMode = false;

					ResetQueryBuilderMetadata();

					queryBuilder1.MetadataProvider = Way1EventMetadataProvider;
					queryBuilder1.MetadataStructure.Refresh();
                    StatusBar1.Message.Information("Metadata loaded");
                }
				catch (Exception ex)
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
	}
}