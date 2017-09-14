<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryBuilderOffline.ascx.cs" Inherits="Samples.QueryBuilderOffline" %>
<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" TagPrefix="AQB" %>
<script>
    $(function () {
        var index = 'key';
        var dataStore = window.sessionStorage;
        try {
            var oldIndex = dataStore.getItem(index);
        } catch (e) {
            var oldIndex = 0;
        }
        $('#tabs').tabs({
            active: oldIndex,
            activate: function (event, ui) {
                var newIndex = ui.newTab.parent().children().index(ui.newTab);
                dataStore.setItem(index, newIndex);
            }
        });
    });
</script>
<AQB:QueryBuilderControl OnSleepModeChanged="SleepModeChanged" ID="QueryBuilderControl1" runat="server" OnInit="QueryBuilderControl1_Init" />
<div id="all">
    <div class="header block-flat">
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1">Direct filling of MetadataContainer</a></li>
                <li><a href="#tabs-2">On-demand filling using ItemMetadataLoading event</a></li>
                <li><a href="#tabs-3">Using the ExecSQL event</a></li>
                <li><a href="#tabs-4">Fill from DataSet</a></li>
            </ul>
            <div id="tabs-1">
                <p>This method demonstrates the direct access to the internal matadata objects collection (MetadataContainer).</p>
                <asp:Button ID="btn1" runat="server" Text="Load Metadata" OnClick="btn1_Click" Style="height: 26px" />
            </div>
            <div id="tabs-2">
                <p>This method demonstrates manual filling of metadata structure using MetadataContainer.ItemMetadataLoading event.</p>
                <asp:Button ID="btn2" runat="server" Text="Load Metadata" OnClick="btn2_Click" />
            </div>
            <div id="tabs-3">
                <p>This method demonstrates loading of metadata through .NET data providers unsupported by our QueryBuilder component. If such data provider is able to execute SQL queries, you can use our EventMetadataProvider with handling it's ExecSQL event. In this event the EventMetadataProvider will provide you SQL queries it use for the metadata retrieval. You have to execute a query and return resulting data reader object.</p>
                <p>Note: In this sample we are using GenericSyntaxProvider that tries to detect the the server type automatically. In some conditions it's unable to detect the server type and it also has limited syntax parsing abilities. For this reason you have to use specific syntax providers in your application, e.g. MySQLSyntaxProver, OracleSyntaxProvider, etc.</p>
                <p style="color: red">Please setup a database connection in web.config file before testing this method.</p>
                <asp:Button ID="btn3" runat="server" Text="Load Metadata" OnClick="btn3_Click" />
            </div>
            <div id="tabs-4">
                <p>This method demonstrates manual filling of metadata structure from stored DataSet.</p>
                <asp:Button ID="btn4" runat="server" Text="Load Metadata" OnClick="btn4_Click" />
            </div>
        </div>

    </div>
    <div id="content-container">
        <div id="qb-ui">
            <AQB:ObjectTreeView ID="ObjectTree1" runat="server" />
            <div id="center">
                <AQB:SubQueryNavigationBar ID="SubQueryNavigationBar1" runat="server" />
                <div class="qb-ui-canvas-container block-flat">

                    <AQB:Canvas ID="Canvas1" runat="server" />
                    <AQB:StatusBar ID="StatusBar1" runat="server" />
                    <AQB:Grid ID="Grid1" runat="server" />
                </div>
            </div>

            <div class="clear">
            </div>
        </div>
    </div>
    <AQB:SqlEditor ID="SQLEditor1" runat="server"></AQB:SqlEditor>
</div>
