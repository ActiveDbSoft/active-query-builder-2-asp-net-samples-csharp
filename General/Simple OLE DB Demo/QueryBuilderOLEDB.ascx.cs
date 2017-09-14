using System;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control;
using Logger=ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server.Logger;

namespace Samples
{
    public partial class QueryBuilderOLEDB : System.Web.UI.UserControl
    {
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack) SQLEditor1.SQL = @"Select o.Order_ID, c.ID As a1, c.First_Name, s.ID
From Orders o Inner Join
  Customers c On o.Customer_ID = c.ID Inner Join
  Shippers s On s.ID = o.Shipper_ID
Where o.Ship_City = 'A'";
		}

        protected void SleepModeChanged(object sender, EventArgs e)
		{
			QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
			if (queryBuilder.SleepMode) StatusBar1.Message.Error("Unsupported SQL statement.");
		}

public void QueryBuilderControl1_Init(object sender, EventArgs e)
        {
            // Get instance of QueryBuilder
            ActiveDatabaseSoftware.ActiveQueryBuilder.QueryBuilder queryBuilder = (QueryBuilderControl1 as QueryBuilderControl).QueryBuilder;
            queryBuilder.OfflineMode = false;
            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
queryBuilder.BehaviorOptions.AllowSleepMode = true;
queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

            // you may load metadata from the database connection using live database connection and metadata provider
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
            if (string.IsNullOrEmpty(connection.ConnectionString))
            {
                string message = "Can't find in [web.config] key <configuration>/<connectionStrings><add key=\"YourDB\" connectionString=\"...\">!";
                Logger.Error(message);
                StatusBar1.Message.Error(message + " Check log.txt for details.");
                queryBuilder.OfflineMode = true;
                return;
            }

            try
            {
                OLEDBMetadataProvider metadataProvider = new OLEDBMetadataProvider();
                metadataProvider.Connection = connection;
                queryBuilder.MetadataProvider = metadataProvider;
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
    }
}