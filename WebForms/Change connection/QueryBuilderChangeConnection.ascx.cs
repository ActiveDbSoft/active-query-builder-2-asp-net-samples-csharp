using System;
using System.Data.SqlClient;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using Logger = ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server.Logger;

namespace ChangeConnection
{
    public partial class QueryBuilderChangeConnection : System.Web.UI.UserControl
    {
        public void QueryBuilderControl1_Init(object sender, EventArgs e)
        {
            // Get instance of QueryBuilder
            QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();
            queryBuilder.MetadataProvider = new MSSQLMetadataProvider();
        }

        protected void FirstOnClick(object sender, EventArgs e)
        {
            SetConnection("AdventureWorks2014");
        }

        protected void SecondOnClick(object sender, EventArgs e)
        {
            SetConnection("TestSelfLinks");
        }

        private void SetConnection(string dbName)
        {
            var queryBuilder = QueryBuilderControl1.QueryBuilder;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = @".\sqlexpress",
                InitialCatalog = dbName,
                IntegratedSecurity = true
            };

            queryBuilder.MetadataProvider.Connection = new SqlConnection(builder.ConnectionString);

            try
            {
                queryBuilder.MetadataContainer.Clear();
                queryBuilder.MetadataStructure.Refresh();
                StatusBar1.Message.Information("Metadata loaded");
            }
            catch (Exception ex)
            {
                string message =
                "Error loading metadata from the database.";
                Logger.Error(message, ex);
                StatusBar1.Message.Error(message + " Check log.txt for details.");
            }
        }
    }
}