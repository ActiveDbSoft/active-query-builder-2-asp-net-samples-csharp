using System;
using System.Configuration;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace Samples
{
    public partial class MetadataStructure : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) SQLEditor1.SQL = @"SELECT Orders.OrderID, Orders.CustomerID, Orders.OrderDate, [Order Details].ProductID,
										[Order Details].UnitPrice, [Order Details].Quantity, [Order Details].Discount
									  FROM Orders INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID
									  WHERE Orders.OrderID > 0 AND [Order Details].Discount > 0";
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
            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/XmlMetaData] key
            try
            {
                queryBuilder.MetadataContainer.ImportFromXML(Page.Server.MapPath(ConfigurationManager.AppSettings["XmlMetaData"]));
            }
            catch (Exception ex)
            {
                string message = "Can't load metadata from XML.";
                Logger.Error(message, ex);
                StatusBar1.Message.Error(message + " Check log.txt for details.");
            }


            // Initialization of the Metadata Structure object that's
            // responsible for representation of metadata in a tree-like form
            try
            {
                // Disable the automatic metadata structure creation
                queryBuilder.MetadataStructure.AllowChildAutoItems = false;

                // queryBuilder.DatabaseSchemaTreeOptions.DefaultExpandLevel = 0;

                MetadataFilterItem filter;

                // Create a top-level folder containing all objects
                MetadataStructureItem allObjects = new MetadataStructureItem();
                allObjects.Caption = "All objects";
                allObjects.ImageIndex = queryBuilder.MetadataStructure.Structure.Options.FolderImageIndex;
                filter = allObjects.MetadataFilter.Add();
                filter.ObjectTypes = MetadataType.All;
                queryBuilder.MetadataStructure.Items.Add(allObjects);

                // Create "Favorites" folder
                MetadataStructureItem favorites = new MetadataStructureItem();
                favorites.Caption = "Favorites";
                favorites.ImageIndex = queryBuilder.MetadataStructure.Structure.Options.FolderImageIndex;
                queryBuilder.MetadataStructure.Items.Add(favorites);

                MetadataItem metadataItem;
                MetadataStructureItem item;

                // Add some metadata objects to "Favorites" folder
                metadataItem = queryBuilder.MetadataContainer.FindItem<MetadataItem>("Orders");
                item = new MetadataStructureItem();
                item.MetadataItem = metadataItem;
                item.ImageIndex = queryBuilder.MetadataStructure.Structure.Options.UserTableImageIndex;
                favorites.Items.Add(item);

                metadataItem = queryBuilder.MetadataContainer.FindItem<MetadataItem>("Order Details");
                item = new MetadataStructureItem();
                item.MetadataItem = metadataItem;
                item.ImageIndex = queryBuilder.MetadataStructure.Structure.Options.UserTableImageIndex;
                favorites.Items.Add(item);

                // Create folder with filter
                MetadataStructureItem filteredFolder = new MetadataStructureItem(); // creates dynamic node
                filteredFolder.Caption = "Filtered by 'Prod%'";
                filteredFolder.ImageIndex = queryBuilder.MetadataStructure.Structure.Options.FolderImageIndex;
                filter = filteredFolder.MetadataFilter.Add();
                filter.ObjectTypes = MetadataType.Table | MetadataType.View;
                filter.Object = "Prod%";
                queryBuilder.MetadataStructure.Items.Add(filteredFolder);

                // Clears and loads the first level of the metadata structure tree 
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