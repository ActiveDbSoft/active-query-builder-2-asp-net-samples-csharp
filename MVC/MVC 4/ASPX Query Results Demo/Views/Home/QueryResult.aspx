<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="MvcAspx.Controllers" %>

<script runat="server">
	protected void Page_Load(object sender, EventArgs e)
	{
		//		ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
		//		scriptManager.RegisterAsyncPostBackControl(this.GridView1);

		//		GridView1.DataSource = ViewBag.Data;
		//		GridView1.PageIndex = ViewBag.CurrentPage - 1;
		//		GridView1.DataBind();
	}

	private void OnSorting(object sender, GridViewSortEventArgs e)
	{

	}

	private void PageIndexChanging(object sender, GridViewPageEventArgs e)
	{

	}

</script>
<form runat="server">
	<%
		WebGrid grid = null;
		if (Model != null)
		{
			grid = new WebGrid(
				ajaxUpdateContainerId: "result-grid",
				canSort: true,
				canPage: true,
				rowsPerPage: HomeController.PageSize
				);
			
			grid.PageIndex = ViewBag.CurrentPage - 1;
			grid.Bind(Model, autoSortAndPage: false, rowCount: ViewBag.RowCount);
		}

	%>
	<div id="result-grid">
		<%=grid.GetHtml() %>>
	    <textarea style="width: 100%; height: 200px;"><%=  ViewBag.Sql %></textarea>
    </div>
</form>
