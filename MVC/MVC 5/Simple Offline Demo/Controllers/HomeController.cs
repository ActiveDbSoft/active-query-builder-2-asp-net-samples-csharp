using System;
using System.Web.Mvc;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Mvc.Filters;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace MVC5Demo.Controllers
{
    public class HomeController : Controller
    {
        [QueryBuilderInit]
        public ActionResult Index()
        {
            return View();
        }
    }

    internal class QueryBuilderInitAttribute: InitializeQueryBuilderAttribute
    {
        protected override void Init(ActionExecutingContext filterContext, SessionStoreItem item)
        {
            // Get instance of the QueryBuilder object
            var queryBuilder = item.QueryBuilder;

            // Create an instance of the proper syntax provider for your database server.
            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

            // Denies metadata loading requests from the metadata provider
            queryBuilder.OfflineMode = true;

            var path = ConfigurationManager.AppSettings["XmlMetaData"];
			var xml = Path.Combine(Server.MapPath(""), path);
			queryBuilder.MetadataContainer.ImportFromXML(xml);

            // Initialization of the Metadata Structure object that's
            // responsible for representation of metadata in a tree-like form
            try
            {
                // Clears and loads the first level of metadata structure tree
                queryBuilder.MetadataStructure.Refresh();
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading metadata", ex);
            }
        }
    }
}