using System;
using System.Configuration;
using System.IO;
using System.Xml;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace Samples
{
    public partial class VirtualObjectsAndFields : System.Web.UI.UserControl
    {
        private class ExchangeClass
        {
            public string SQL;
            public string AlternateSQL;
        }
        
        private PlainTextSQLBuilder plainTextSQLBuilder;
        private PlainTextSQLBuilder plainTextSQLBuilder2;

        protected void Page_Load(object sender, EventArgs e)
        {
	        if (!Page.IsPostBack) SQLEditor1.SQL = @"SELECT dummy_alias._qry_OrderId_plus_1, dummy_alias._qry_CustomerName FROM Orders_qry dummy_alias";
        }

        protected void SleepModeChanged(object sender, EventArgs e)
		{
			QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
			if (queryBuilder.SleepMode) StatusBar1.Message.Error("Unsupported SQL statement.");
		}

public void QueryBuilderControl1_OnSQLUpdated(object sender, EventArgs e, ClientData clientdata)
        {
            SessionStore.Current.Exchange.Data = new ExchangeClass()
            {
                SQL = plainTextSQLBuilder.SQL,
                AlternateSQL = plainTextSQLBuilder2.SQL
            };
        }

        
public void QueryBuilderControl1_Init(object sender, EventArgs e)
        {
            // Get instance of QueryBuilder
            QueryBuilder queryBuilder = QueryBuilderControl1.QueryBuilder;
            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
			queryBuilder.BehaviorOptions.AllowSleepMode = true;
			queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

            plainTextSQLBuilder = new PlainTextSQLBuilder { QueryBuilder = queryBuilder, KeywordFormat = KeywordFormat.UpperCase };
            plainTextSQLBuilder2 = new PlainTextSQLBuilder {QueryBuilder = queryBuilder, KeywordFormat = KeywordFormat.UpperCase, UseAltNames = false, ExpandVirtualObjects = true};
            
            queryBuilder.OfflineMode = true;
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

            MetadataObject o;
            MetadataField f;

            // Virtual fields for real object
            // ===========================================================================
			o = queryBuilder.MetadataContainer.FindItem<MetadataObject>("Orders");

			// first test field - simple expression
			f = o.AddField("_OrderId_plus_1");
			f.Expression = "orders.OrderId + 1";

			// second test field - correlated sub-query
			f = o.AddField("_CustomerName");
			f.Expression = "(select c.CompanyName from Customers c where c.CustomerId = orders.CustomerId)";

			// Virtual object (table) with virtual fields
			// ===========================================================================

			o = queryBuilder.MetadataContainer.AddTable("Orders_tbl");
			o.Expression = "Orders";

			// first test field - simple expression
			f = o.AddField("_tbl_OrderId_plus_1");
			f.Expression = "Orders_tbl.OrderId + 1";

			// second test field - correlated sub-query
			f = o.AddField("_tbl_CustomerName");
			f.Expression = "(select c.CompanyName from Customers c where c.CustomerId = Orders_tbl.CustomerId)";

			// Virtual object (sub-query) with virtual fields
			// ===========================================================================

			o = queryBuilder.MetadataContainer.AddTable("Orders_qry");
			o.Expression = "(select OrderId, CustomerId, OrderDate from Orders) as dummy_alias";

			// first test field - simple expression
			f = o.AddField("_qry_OrderId_plus_1");
			f.Expression = "Orders_qry.OrderId + 1";

			// second test field - correlated sub-query
			f = o.AddField("_qry_CustomerName");
			f.Expression = "(select c.CompanyName from Customers c where c.CustomerId = Orders_qry.CustomerId)";

			// kick queryBuilder to initialize its metadata tree
			queryBuilder.InitializeDatabaseSchemaTree();

			queryBuilder.SQL = "SELECT dummy_alias._qry_OrderId_plus_1, dummy_alias._qry_CustomerName FROM Orders_qry dummy_alias";
		}

    }
}