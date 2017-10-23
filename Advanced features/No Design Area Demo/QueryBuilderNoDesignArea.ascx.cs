using System;
using System.Configuration;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace Samples
{
    public partial class QueryBuilderNoDesignArea : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) SQLEditor1.SQL = @"Select o.OrderID, c.CustomerID, s.ShipperID, o.ShipCity
From Orders o Inner Join
  Customers c On o.Customer_ID = c.ID Inner Join
  Shippers s On s.ID = o.Shipper_ID
Where o.ShipCity = 'A'";
        }

        protected void SleepModeChanged(object sender, EventArgs e)
        {
            QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
            if (queryBuilder.SleepMode) StatusBar1.Message.Error("Unsupported SQL statement.");
        }

        public void QueryBuilderControl1_Init(object sender, EventArgs e)
        {
            // Get instance of QueryBuilder
            QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
            queryBuilder.BehaviorOptions.AllowSleepMode = true;
            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

            queryBuilder.OfflineMode = true;

            queryBuilder.BehaviorOptions.DeleteUnusedObjects = true;
            queryBuilder.BehaviorOptions.AddLinkedObjects = true;

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/XmlMetaData] key
            try
            {
                var path = ConfigurationManager.AppSettings["XmlMetaData"];
				var xml = Path.Combine(Server.MapPath(""), path);
				queryBuilder.MetadataContainer.ImportFromXML(xml);

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