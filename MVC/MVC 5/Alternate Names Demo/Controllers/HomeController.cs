using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.UI;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Mvc.Filters;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace Alternate_Names.Controllers
{
    public class HomeController : Controller
    {
        [Initialize]
        public ActionResult Index()
        {
            return View();
        }
    }

    public class Initialize : InitializeQueryBuilderAttribute
    {
        private class ExchangeClass
        {
            public string SQL;
            public string AlternateSQL;
        }

        protected override void Init(ActionExecutingContext filterContext, SessionStoreItem item)
        {
            // Get instance of QueryBuilder
            QueryBuilder queryBuilder = item.QueryBuilder;

            queryBuilder.BehaviorOptions.UseAltNames = false;
            item.PlainTextSQLBuilder.UseAltNames = false;

            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
            queryBuilder.BehaviorOptions.AllowSleepMode = true;
            queryBuilder.SyntaxProvider = new DB2SyntaxProvider();

            queryBuilder.SQLUpdated += OnSQLUpdated;

            queryBuilder.OfflineMode = true;
            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/XmlMetaData] key
            try
            {
                var path = ConfigurationManager.AppSettings["XmlMetaData"];
				var xml = Path.Combine(Server.MapPath(""), path);
				queryBuilder.MetadataContainer.ImportFromXML(xml);
								
				queryBuilder.MetadataStructure.Refresh();
                item.Message.Information("Metadata loaded");
            }
            catch (Exception ex)
            {
                string message =
                "Error loading metadata from the database." +
                "Check the 'configuration\\connectionStrings' key in the [web.config] file.";
                Logger.Error(message, ex);
                item.Message.Error(message + " Check log.txt for details.");
            }

            queryBuilder.SQL = @"Select ""Employees"".""Employee ID"", ""Employees"".""First Name"", ""Employees"".""Last Name"", ""Employee Photos"".""Photo Image"", ""Employee Resumes"".Resume From ""Employee Photos"" Inner Join
			""Employees"" On ""Employee Photos"".""Employee ID"" = ""Employees"".""Employee ID"" Inner Join
			""Employee Resumes"" On ""Employee Resumes"".""Employee ID"" = ""Employees"".""Employee ID""";
        }

        public void OnSQLUpdated(object sender, EventArgs e)
        {
            QueryBuilder queryBuilder = SessionStore.Current.QueryBuilder;
            PlainTextSQLBuilder plainTextSQLBuilder = new PlainTextSQLBuilder { QueryBuilder = queryBuilder, KeywordFormat = KeywordFormat.UpperCase, UseAltNames = false };
            PlainTextSQLBuilder plainTextSQLBuilderWithAltNames = new PlainTextSQLBuilder { QueryBuilder = queryBuilder, KeywordFormat = KeywordFormat.UpperCase, UseAltNames = true };

            SessionStore.Current.Exchange.Data = new ExchangeClass
            {
                SQL = plainTextSQLBuilder.SQL,
                AlternateSQL = plainTextSQLBuilderWithAltNames.SQL
            };
        }
    }
}