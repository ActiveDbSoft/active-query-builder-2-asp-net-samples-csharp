using System;
using System.Configuration;
using System.IO;
using System.Xml;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace Samples
{
    public partial class QueryStructure : System.Web.UI.UserControl
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

public void QueryBuilderControl1_OnSQLUpdated(object sender, EventArgs e, ClientData clientdata)
        {
            QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
            if (queryBuilder == null) return;
            var data = new ExchangeClass();

            data.Statistics = GetQueryStatistic(queryBuilder.QueryStatistics);
            data.SubQueries = DumpSubQueries(queryBuilder);
            data.QueryStructure = DumpQueryStructureInfo(queryBuilder.ActiveSubQuery);
            data.UnionSubQuery = new UnionSubQueryExchangeClass();

            data.UnionSubQuery.SelectedExpressions = DumpSelectedExpressionsInfoFromUnionSubQuery(queryBuilder.ActiveSubQuery.ActiveUnionSubquery);
            data.UnionSubQuery.DataSources = DumpDataSourcesInfoFromUnionSubQuery(queryBuilder.ActiveSubQuery.ActiveUnionSubquery);
            ;
            data.UnionSubQuery.Links = DumpLinksInfoFromUnionSubQuery(queryBuilder.ActiveSubQuery.ActiveUnionSubquery);
            data.UnionSubQuery.Where = GetWhereInfo(queryBuilder.ActiveSubQuery.ActiveUnionSubquery);
//            data.UnionSubQuery.Where

            SessionStore.Current.Exchange.Data = data;
        }
    

      
public void QueryBuilderControl1_Init(object sender, EventArgs e)
        {
            // Get instance of QueryBuilder
            QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
queryBuilder.BehaviorOptions.AllowSleepMode = true;
queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

            queryBuilder.OfflineMode = true;
            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/XmlMetaData] key
            try
            {
                queryBuilder.MetadataContainer.ImportFromXML(Page.Server.MapPath(ConfigurationManager.AppSettings["/Northwind.xml"]));
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
            queryBuilder.SQL = SQLEditor1.ControlSQL;
        }

        private XmlDocument LoadXml(string xmlFileName)
        {
            var doc = new XmlDocument();
            try
            {
                doc.Load(xmlFileName);
            }
            catch (Exception ex)
            {
                string message;
                if (ex is FileNotFoundException)
                {
                    message = string.Format("Can't find XML file '{0}'.", xmlFileName);
                    Logger.Error(message, ex);
                    StatusBar1.Message.Error(message + " Check log.txt for details.");
                }
                else if (ex is IOException)
                {
                    message = string.Format("Can't read XML file '{0}'.", xmlFileName);
                    Logger.Error(message, ex);
                    StatusBar1.Message.Error(message + " Check log.txt for details.");
                }
                else if (ex is XmlException)
                {
                    message = string.Format("Can't parse XML file '{0}'.", xmlFileName);
                    Logger.Error(message, ex);
                    StatusBar1.Message.Error(message + " Check log.txt for details.");
                }
                else
                {
                    message = string.Format("Unknown error during load XML file '{0}'", xmlFileName);
                    Logger.Error(message, ex);
                    StatusBar1.Message.Error(message + " Check log.txt for details.");
                }
                doc = null;
            }
            return doc;
        }
    }

}