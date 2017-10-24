<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryBuilderChangeConnection.ascx.cs" Inherits="ChangeConnection.QueryBuilderChangeConnection" %>
<%@ Register TagPrefix="AQB" Namespace="ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control" Assembly="ActiveDatabaseSoftware.ActiveQueryBuilder2.Web.Control, Culture=neutral, PublicKeyToken=3cbcbcc9bf57ecde" %>
<h3 style="display: inline-block">Connect to:</h3>
<asp:Button runat="server" OnClick="FirstOnClick" Text="Database 1" />
<asp:Button runat="server" OnClick="SecondOnClick" Text="Database 2" />
<AQB:QueryBuilderControl ID="QueryBuilderControl1" runat="server" OnInit="QueryBuilderControl1_Init" />
<div id="all">
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
