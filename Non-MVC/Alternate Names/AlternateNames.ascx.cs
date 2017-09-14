using System;
using System.Configuration;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace Samples
{
    public partial class AlternateNames : System.Web.UI.UserControl
    {
        private class ExchangeClass
        {
            public string SQL;
            public string AlternateSQL;
        }

        private PlainTextSQLBuilder plainTextSQLBuilder;
        private PlainTextSQLBuilder plainTextSQLBuilderWithAltNames;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) SQLEditor1.SQL = @"Select ""Employees"".""Employee ID"", ""Employees"".""First Name"", ""Employees"".""Last Name"", ""Employee Photos"".""Photo Image"", ""Employee Resumes"".Resume From ""Employee Photos"" Inner Join
			""Employees"" On ""Employee Photos"".""Employee ID"" = ""Employees"".""Employee ID"" Inner Join
			""Employee Resumes"" On ""Employee Resumes"".""Employee ID"" = ""Employees"".""Employee ID""";
        }

        protected void SleepModeChanged(object sender, EventArgs e)
        {
            QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
            if (queryBuilder.SleepMode) StatusBar1.Message.Error("Unsupported SQL statement.");
        }

        public void QueryBuilderControl1_OnSQLUpdated(object sender, EventArgs e, ClientData clientdata)
        {
            QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
            plainTextSQLBuilder = new PlainTextSQLBuilder { QueryBuilder = queryBuilder, KeywordFormat = KeywordFormat.UpperCase, UseAltNames = false };
            plainTextSQLBuilderWithAltNames = new PlainTextSQLBuilder { QueryBuilder = queryBuilder, KeywordFormat = KeywordFormat.UpperCase, UseAltNames = true };
            SessionStore.Current.Exchange.Data = new ExchangeClass()
            {
                SQL = plainTextSQLBuilder.SQL,
                AlternateSQL = plainTextSQLBuilderWithAltNames.SQL
            };
        }


        public void QueryBuilderControl1_Init(object sender, EventArgs e)
        {
            // Get instance of QueryBuilder
            QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;

            queryBuilder.BehaviorOptions.UseAltNames = false;
             SessionStore.Current.PlainTextSQLBuilder.UseAltNames = false;
			
            plainTextSQLBuilder = new PlainTextSQLBuilder { QueryBuilder = queryBuilder, KeywordFormat = KeywordFormat.UpperCase, UseAltNames = false };
            plainTextSQLBuilderWithAltNames = new PlainTextSQLBuilder { QueryBuilder = queryBuilder, KeywordFormat = KeywordFormat.UpperCase, UseAltNames = true };

            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
            queryBuilder.BehaviorOptions.AllowSleepMode = true;
            queryBuilder.SyntaxProvider = new DB2SyntaxProvider();

            queryBuilder.OfflineMode = true;
            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/XmlMetaData] key
            try
            {
                queryBuilder.MetadataContainer.ImportFromXML(Page.Server.MapPath(ConfigurationManager.AppSettings["XmlMetaDataDB2"]));
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