using System.Web.Mvc;

namespace MvcRazorQueryResults
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}