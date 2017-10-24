using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
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
            queryBuilder.SyntaxProvider = new MSAccessSyntaxProvider();
            queryBuilder.MetadataProvider = new OLEDBMetadataProvider();
        }

        protected void FirstOnClick(object sender, EventArgs e)
        {
            SetConnection("Nwind.mdb");
        }

        protected void SecondOnClick(object sender, EventArgs e)
        {
            SetConnection("demo.mdb");
        }

        private void SetConnection(string dbName)
        {
            var queryBuilder = QueryBuilderControl1.QueryBuilder;
            
            queryBuilder.MetadataProvider.Connection = CreateConnection(dbName);

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

        private IDbConnection CreateConnection(string dbname)
        {
            //var provider = "Microsoft.ACE.OLEDB.12.0";
            var provider = "Microsoft.Jet.OLEDB.4.0";
            var path = @"..\..\Sample databases\" + dbname;
   
               var xml = Path.Combine(Server.MapPath(""), path);
            var connectionString = string.Format("Provider={0};Data Source={1};Persist Security Info=False;", provider, xml);
            return new OleDbConnection(connectionString);
        }
    }
}