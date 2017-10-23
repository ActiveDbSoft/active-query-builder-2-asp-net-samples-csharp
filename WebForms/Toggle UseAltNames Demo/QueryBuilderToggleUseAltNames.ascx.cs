using System;
using ActiveDatabaseSoftware.ActiveQueryBuilder;

namespace ToggleUseAltNames
{
    public partial class QueryBuilderToggleUseAltNames : System.Web.UI.UserControl
    {
        public void QueryBuilderControl1_Init(object sender, EventArgs e)
        {
            // Get instance of QueryBuilder
            QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;

            queryBuilder.SyntaxProvider = new DB2SyntaxProvider();

            queryBuilder.BehaviorOptions.UseAltNames = true;

            queryBuilder.OfflineMode = true;
            
			var path = ConfigurationManager.AppSettings["XmlMetaData"];
			var xml = Path.Combine(Server.MapPath(""), path);
			queryBuilder.MetadataContainer.ImportFromXML(xml);
        }

        protected void ToggleOnClick(object sender, EventArgs e)
        {
            QueryBuilderControl1.QueryBuilder.BehaviorOptions.UseAltNames = !QueryBuilderControl1.QueryBuilder.BehaviorOptions.UseAltNames;
            QueryBuilderControl1.PlainTextSQLBuilder.UseAltNames = !QueryBuilderControl1.PlainTextSQLBuilder.UseAltNames;
            QueryBuilderControl1.QueryBuilder.MetadataStructure.Refresh();
        }
    }
}