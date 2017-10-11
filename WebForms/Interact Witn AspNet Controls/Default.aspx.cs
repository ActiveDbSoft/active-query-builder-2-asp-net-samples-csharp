using System;
using System.Collections.Generic;
using System.Web.UI;
using ActiveDatabaseSoftware.ActiveQueryBuilder;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Server;

namespace Samples
{
    public static class PresetHelper
    {
        public class Preset
        {
            public string XmlMetaData;
            public string SQL;
        }

        public static int CurrentPresetNumber = 0;
        public static Preset CurrentPreset
        {
            get { return Presets[CurrentPresetNumber]; }
        }

        public static void SwitchPreset()
        {
            if (++CurrentPresetNumber >= Presets.Count) CurrentPresetNumber = 0;
        }

        public static void ResetPreset()
        {
            CurrentPresetNumber = 0;
        }


        public static List<Preset> Presets = new List<Preset> { 
            new Preset{
                XmlMetaData = "/Northwind.xml", 
                SQL = @"Select o.OrderID, c.CustomerID As a1, c.ContactName
From Orders o Inner Join
  Customers c On o.CustomerID = c.CustomerID Inner Join
  Shippers On Shippers.ShipperID = c.Country Inner Join
  Region On Shippers.CompanyName = Region.RegionID
Where o.ShipCity = 'A'"}, 
            new Preset{
                XmlMetaData = "/db2_sample_with_alt_names.xml", 
                SQL = @"Select Employees.[Employee ID], Employees.[First Name],
  [Employee Photos].[Photo Image], [Employee Resumes].Resume
From [Employee Photos] Inner Join
  Employees On Employees.[Employee ID] = [Employee Photos].[Employee ID]
  Inner Join
  [Employee Resumes] On Employees.[Employee ID] =
    [Employee Resumes].[Employee ID]"}, 
        };
    }

    public partial class InteractWitnAspNetControls : Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PresetHelper.ResetPreset();
				SQLEditor1.SQL = @"Select o.OrderID, c.CustomerID, s.ShipperID, o.ShipCity
From Orders o Inner Join
  Customers c On o.Customer_ID = c.ID Inner Join
  Shippers s On s.ID = o.Shipper_ID
Where o.ShipCity = 'A'";
            }
            label.Text = "Preset #" + (PresetHelper.CurrentPresetNumber + 1);
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
            // Pass loaded XML to QueryBuilder component
            try
            {
                var sql = queryBuilder.SQL;
                queryBuilder.SQL = string.Empty;
                queryBuilder.MetadataContainer.ImportFromXML(Server.MapPath(PresetHelper.CurrentPreset.XmlMetaData));
                queryBuilder.MetadataStructure.Refresh();
                queryBuilder.SQL = sql;
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


        protected void button_Click(object sender, EventArgs e)
        {
            PresetHelper.SwitchPreset();
            label.Text = "Preset #" + (PresetHelper.CurrentPresetNumber+1);
        }
    }
}