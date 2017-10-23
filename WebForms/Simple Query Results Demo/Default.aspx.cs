using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.QueryTransformer;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace Samples
{
	public partial class Default : System.Web.UI.Page
	{
		protected void SleepModeChanged(object sender, EventArgs e)
		{
			QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
			if (queryBuilder.SleepMode) SessionStore.Current.Message.Error("Unsupported SQL statement.");
		}

public void QueryBuilderControl1_Init(object sender, EventArgs e)
		{
			// Get instance of QueryBuilder
			QueryBuilder queryBuilder = (QueryBuilderControl1).QueryBuilder;
			queryBuilder.OfflineMode = false;
			// Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
queryBuilder.BehaviorOptions.AllowSleepMode = true;
queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

			// you may load metadata from the database connection using live database connection and metadata provider
			var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString);

			if (string.IsNullOrEmpty(connection.ConnectionString))
			{
				string message = "Can't find in [web.config] key <configuration>/<connectionStrings><add key=\"YourDB\" connectionString=\"...\">!";
				Logger.Error(message);
				queryBuilder.OfflineMode = true;
				return;
			}

			try
			{
				var metadataProvider = new MSSQLMetadataProvider();
				metadataProvider.Connection = connection;
				queryBuilder.MetadataProvider = metadataProvider;
			}
			catch (Exception ex)
			{
				string message = "Can't setup metadata provider!";
				Logger.Error(message, ex);
				return;
			}


			// Initialization of the Metadata Structure object that's
			// responsible for representation of metadata in a tree-like form
			try
			{
				// Clears and loads the first level of the metadata structure tree 
				queryBuilder.MetadataStructure.Refresh();
                StatusBar1.Message.Information("Metadata loaded");
            }
			catch (Exception ex)
			{
                string message =
                "Error loading metadata from the database." +
                "Check the 'configuration\\connectionStrings' key in the [web.config] file.";
                Logger.Error(message, ex);
                StatusBar1.Message.Error(message + " Check log.txt for details.");
            }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
//			if (!Page.IsPostBack) SQLEditor1.SQL = @"Select * From Production.Product";
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			try
			{
				UpdateGrid();
				messageLabel.Text = "";
			}
			catch (Exception ex)
			{
				string message = "SQL query error";
				Logger.Error(message, ex);
				messageLabel.Text = "Query execute error: " + ex.Message;
			}
		}
		public class VirtualItemCountTableAdapter
		{
			private DataTable data;
			private Int32 virtualItemCount;

			public VirtualItemCountTableAdapter(DataTable data, Int32 virtualItemCount)
			{
				this.data = data;
				this.virtualItemCount = virtualItemCount;
			}
			public DataTable GetData()
			{
				return data;
			}

			public Int32 GetVirtualItemCount()
			{
				return virtualItemCount;
			}

			public DataTable GetData(int startRow, int maxRows)
			{
				return data;
			}
		}
		private DataTable _dt;
		private int _vic;
		private void UpdateGrid()
		{
			QueryBuilder queryBuilder1 = (QueryBuilderControl1).QueryBuilder;

			dataGridView1.DataSource = null;
			if (queryBuilder1.MetadataProvider == null) return;

			var cmd = (SqlCommand)queryBuilder1.MetadataProvider.Connection.CreateCommand();
			cmd.CommandTimeout = 30;
			cmd.CommandText = CriteriaBuilder1.QueryTransformer.Sql;

			try
			{
				var adapter = new SqlDataAdapter(cmd);
				foreach (var paramDto in SessionStore.Current.ClientQueryParams)
				{
					cmd.Parameters.Add(paramDto.Name, paramDto.Value);
				}

				var dataset = new DataSet();
				adapter.Fill(dataset, "QueryResult");

				_dt = dataset.Tables["QueryResult"];
				_vic = GetCount();


				var ods = new ObjectDataSource();
				ods.ID = "ods" + dataGridView1.ID;
				ods.EnablePaging = dataGridView1.AllowPaging;
				ods.TypeName = "Samples.Default+VirtualItemCountTableAdapter"; //be sure to prefix the namespace of your application !!! e.g. MyApp.MyTableAdapter
				ods.SelectMethod = "GetData";
				ods.SelectCountMethod = "GetVirtualItemCount";
				ods.StartRowIndexParameterName = "startRow";
				ods.MaximumRowsParameterName = "maxRows";
				ods.EnableViewState = false;
				ods.ObjectCreating += ods_ObjectCreating;

				dataGridView1.DataSource = ods;
				dataGridView1.DataBind();
			}
			catch (Exception ex)
			{
				string message = "SQL query error";
				Logger.Error(message, ex);
				SessionStore.Current.Message.Error(message + " Check log.txt for details.");
			}
		}

		private void ods_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
		{
			e.ObjectInstance = new VirtualItemCountTableAdapter(_dt, _vic);
		}

		public int GridPageSize = 20;

		private SortDirection GridViewSortDirection
		{
			get
			{
				if (ViewState["GridViewSortDirection"] == null) ViewState["GridViewSortDirection"] = SortDirection.Ascending;
				return (SortDirection)ViewState["GridViewSortDirection"];
			}
			set { ViewState["GridViewSortDirection"] = value; }
		}

		private string GridViewSortExpression
		{
			get
			{
				if (ViewState["GridViewSortExpression"] == null) ViewState["GridViewSortExpression"] = string.Empty;
				return (string)ViewState["GridViewSortExpression"];
			}
			set { ViewState["GridViewSortExpression"] = value; }
		}

		private int GridCurrentPage
		{
			get
			{
				if (ViewState["GridCurrentPage"] == null) ViewState["GridCurrentPage"] = 1;
				return (int)ViewState["GridCurrentPage"];
			}
			set { ViewState["GridCurrentPage"] = value; }
		}

		protected void OnSorting(object sender, GridViewSortEventArgs e)
		{
			if (GridViewSortExpression == e.SortExpression)
				ToggleSortingDirection();
			else
			{
				GridViewSortDirection = SortDirection.Ascending;
				GridCurrentPage = 0;
				dataGridView1.PageIndex = GridCurrentPage;
			}

			GridViewSortExpression = e.SortExpression;

			CriteriaBuilder1.QueryTransformer.Sortings.Clear();
			CriteriaBuilder1.QueryTransformer.OrderBy(GridViewSortExpression, GridViewSortDirection == SortDirection.Ascending);
			UpdateGrid();
		}

		private void ToggleSortingDirection()
		{
			GridViewSortDirection = GridViewSortDirection == SortDirection.Ascending
				? SortDirection.Descending
				: SortDirection.Ascending;
		}


		protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			GridCurrentPage = e.NewPageIndex;
			dataGridView1.PageIndex = GridCurrentPage;

			if (string.IsNullOrEmpty(GridViewSortExpression) && CriteriaBuilder1.QueryTransformer.Columns.Count > 0)
				GridViewSortExpression = CriteriaBuilder1.QueryTransformer.Columns[0].Name;

			CriteriaBuilder1.QueryTransformer.Sortings.Clear();
			CriteriaBuilder1.QueryTransformer.OrderBy(GridViewSortExpression, GridViewSortDirection == SortDirection.Ascending);
			CriteriaBuilder1.QueryTransformer.Take(GridPageSize.ToString()).Skip((GridCurrentPage * GridPageSize).ToString());

			UpdateGrid();
		}
		protected int GetCount()
		{
			QueryBuilder queryBuilder1 = (QueryBuilderControl1).QueryBuilder;
			if (queryBuilder1.MetadataProvider == null) return 0;

			CriteriaBuilder1.QueryTransformer.ResultCount = null;
			CriteriaBuilder1.QueryTransformer.ResultOffset = null;
			var col = new SelectedColumn(null, "count(*)");
			CriteriaBuilder1.QueryTransformer.Aggregations.Add(col, "CNT");

			try
			{
				queryBuilder1.MetadataProvider.Connection.Open();
			}
			catch(Exception){}

			var cmd = (SqlCommand)queryBuilder1.MetadataProvider.Connection.CreateCommand();
			cmd.CommandTimeout = 30;
			cmd.CommandText = CriteriaBuilder1.QueryTransformer.Sql;

			CriteriaBuilder1.QueryTransformer.Aggregations.Remove(col);
			try
			{
				foreach (var paramDto in SessionStore.Current.ClientQueryParams)
				{
					cmd.Parameters.Add(paramDto.Name, paramDto.Value);
				}
				return (int)cmd.ExecuteScalar();
			}
			catch (Exception ex)
			{
				string message = "SQL query error";
				Logger.Error(message, ex);
				SessionStore.Current.Message.Error(message + " Check log.txt for details.");
			}
			return 0;
		}

	}
}
