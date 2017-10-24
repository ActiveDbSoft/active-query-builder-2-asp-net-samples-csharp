using System;
using System.IO;
using System.Net;
using System.Data;
using System.Data.OleDb;
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

            // Initialization of the Metadata Structure object that's
            // responsible for representation of metadata in a tree-like form
            try
            {
                queryBuilder.MetadataProvider.Connection = CreateConnection(name);
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

        private IDbConnection CreateConnection(string dbname)
        {
            //var provider = "Microsoft.ACE.OLEDB.12.0";
            var provider = "Microsoft.Jet.OLEDB.4.0";
            var path = @"..\..\..\Sample databases\" + dbname;
            var db = Path.Combine(Server.MapPath("~"), path);
            var connectionString = string.Format("Provider={0};Data Source={1};Persist Security Info=False;", provider, db);
            return new OleDbConnection(connectionString);
        }
    }

    public class InitilizeAttribute : InitializeQueryBuilderAttribute
    {
        protected override void Init(ActionExecutingContext filterContext, SessionStoreItem item)
        {

            // Get instance of the QueryBuilder object
            var queryBuilder = item.QueryBuilder;

            // Create an instance of the proper syntax provider for your database server.
            queryBuilder.SyntaxProvider = new MSAccessSyntaxProvider();
            queryBuilder.MetadataProvider = new OLEDBMetadataProvider();
        }
    }
}