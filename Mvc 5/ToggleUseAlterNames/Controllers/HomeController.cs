using System;
using System.Web;
using System.Web.Mvc;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Mvc.Filters;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace ToggleUseAlterNames.Controllers
{
    public class HomeController : Controller
    {
        [Initialize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ToggleAltName()
        {
            var item = SessionStore.Current;
            var queryBuilder = item.QueryBuilder;
            var useAltNames = queryBuilder.BehaviorOptions.UseAltNames;

            item.PlainTextSQLBuilder.UseAltNames = queryBuilder.BehaviorOptions.UseAltNames = !useAltNames;
            
            queryBuilder.MetadataStructure.UnloadChildItems();

            return new EmptyResult();
        }
    }

    public class InitializeAttribute : InitializeQueryBuilderAttribute
    {
        protected override void Init(ActionExecutingContext filterContext, SessionStoreItem item)
        {
            var qb = item.QueryBuilder;

            qb.SyntaxProvider = new DB2SyntaxProvider();
            qb.BehaviorOptions.UseAltNames = true;
            qb.OfflineMode = true;

            var pathToXml = HttpContext.Current.Server.MapPath("db2_sample_with_alt_names.xml");
            qb.MetadataContainer.ImportFromXML(pathToXml);

            try
            {
                qb.MetadataStructure.Refresh();
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading metadata", ex);
            }
        }
    }
}