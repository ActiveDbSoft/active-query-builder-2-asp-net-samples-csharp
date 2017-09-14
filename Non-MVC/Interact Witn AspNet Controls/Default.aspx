<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.InteractWitnAspNetControls" %>

<%@ Register Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control"
    TagPrefix="AQB" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
		
	</style>
</head>
<body>
    <form runat="server" id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <AQB:QueryBuilderControl OnSleepModeChanged="SleepModeChanged" ID="QueryBuilderControl1" runat="server" OnInit="QueryBuilderControl1_Init" />
        <div id="all">
            <div class="header">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="label" Text="DB1" runat="server"></asp:Label>
                        <input onclick="javascript: UpdPanelUpdate('Switch profle');" type="button" value="Load another DB and SQL" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="button" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Button ID="button" runat="server" OnClick="button_Click" Style="display: none;" />
            </div>
            <div id="content-container">
                <div id="qb-ui">
                    <AQB:ObjectTreeView ID="ObjectTree1" runat="server" ShowFields="true" />
                    <div id="center">
                        <AQB:SubQueryNavigationBar ID="SubQueryNavigationBar1" runat="server" />
                        <div class="qb-ui-canvas-container block-flat">
                            <AQB:Canvas ID="Canvas1" runat="server" />
                            <AQB:StatusBar ID="StatusBar1" runat="server" />
                            <AQB:Grid ID="Grid1" runat="server" />
                        </div>
                    </div>

                    <div class="clear"></div>
                </div>
            </div>
            <AQB:SqlEditor ID="SQLEditor1" runat="server"></AQB:SqlEditor>
            <div class="foot"></div>
        </div>



        <script type="text/javascript">
            function UpdPanelUpdate(value) {
                __doPostBack("<%= button.ClientID %>", "");
            }
        </script>

        <script type="text/javascript" language="javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoaded)

            function PageLoaded(sender, args) {
                var panelId = "UpdatePanel1";

                if (args == null)
                    return;

                if (args.get_panelsUpdated() == null)
                    return;

                for (var i = 0; i < args.get_panelsUpdated().length; i++)
                    if (args.get_panelsUpdated()[i].id == panelId)
                        QB.Web.Core.reconnect();
            }
        </script>
    </form>
</body>
</html>
