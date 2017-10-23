using System;
using System.Web.Mvc;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Mvc.Filters;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace MvcAspx.Controllers
{
	public class HomeController : Controller
	{
        [QueryBuilderInit]
        public ActionResult Index()
		{
			ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

			return View();
		}
    }

    internal class QueryBuilderInitAttribute : InitializeQueryBuilderAttribute
    {
        protected override void Init(ActionExecutingContext filterContext, SessionStoreItem item)
        {
            // Get instance of QueryBuilder
            QueryBuilder queryBuilder = item.QueryBuilder;
            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();
            queryBuilder.OfflineMode = true;

            // Load MetaData from XML document
            try
            {
				var path = ConfigurationManager.AppSettings["XmlMetaData"];
				var xml = Path.Combine(Server.MapPath(""), path);
				queryBuilder.MetadataContainer.ImportFromXML(xml);
                
				queryBuilder.MetadataStructure.Refresh();
            }
            catch (Exception ex)
            {
                string message = "Can't load metadata from XML.";
                Logger.Error(message, ex);
                item.Message.Error(message + " Check log.txt for details.");
            }
        }
    }
}
