using System;
using System.Data.SqlClient;
using System.Net;
using System.Web.Mvc;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Mvc.Filters;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace MVC5ChangeConnection.Controllers
{
    public class HomeController : Controller
    {
        [Initilize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeConnection(string name)
        {
            var item = SessionStore.Current;
            var queryBuilder = item.QueryBuilder;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = @".\sqlexpress",
                InitialCatalog = name,
                IntegratedSecurity = true
            };

            // Initialization of the Metadata Structure object that's
            // responsible for representation of metadata in a tree-like form
            try
            {
                queryBuilder.MetadataProvider.Connection = new SqlConnection(builder.ConnectionString);
                // Clears and loads the first level of metadata structure tree
                queryBuilder.MetadataContainer.Clear();
                queryBuilder.MetadataStructure.Refresh();
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading metadata, connection failed", ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
            return new EmptyResult();
        }
    }

    public class InitilizeAttribute : InitializeQueryBuilderAttribute
    {
        protected override void Init(ActionExecutingContext filterContext, SessionStoreItem item)
        {

            // Get instance of the QueryBuilder object
            var queryBuilder = item.QueryBuilder;

            // Create an instance of the proper syntax provider for your database server.
            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();
            queryBuilder.MetadataProvider = new MSSQLMetadataProvider();
        }
    }
}