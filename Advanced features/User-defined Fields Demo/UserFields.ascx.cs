using System;
using System.Configuration;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;
using Logger = ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server.Logger;

namespace Samples
{
    public partial class UserFields : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                SQLEditor1.SQL = @"Select o.OrderID, o.DetailsCount From Orders o";
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
            queryBuilder.OfflineMode = true;

            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

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
                string message = "Error loading metadata from the file.";
                Logger.Error(message, ex);
                StatusBar1.Message.Error(message + " Check log.txt for details.");
            }

            AddUserFields(queryBuilder);
        }

        private void AddUserFields(QueryBuilder queryBuilder)
        {
            MetadataObject order = queryBuilder.MetadataContainer.FindItem<MetadataObject>("Orders"); 
            order.AddUserField("DetailsCount", "(select count(*) from [Order Details] od where od.OrderId = Orders.OrderId)");
        }

        protected void QueryBuilderControl1_OnSQLUpdated(object sender, EventArgs e, ClientData clientdata)
        {
            SessionStore.Current.Exchange.Data = SessionStore.Current.UserObjectsSQL;
        }
    }
}