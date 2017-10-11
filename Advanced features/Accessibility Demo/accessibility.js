$(function() {
    $(document).focusin(function(event){
    	$(event.target).addClass("ui-global-focus");
    })
    .focusout(function(event){
    	$(event.target).removeClass("ui-global-focus");
    });

	$(document).keydown(function (e) {
		if ($(e.target).is('input[type=text]') || $(e.target).is('textbox')) return;

		switch (e.keyCode) {
			// 1 Focus on Schema Tree
			case 49:
				$('#qb-ui-tree-selects .ui-selectmenu:first').focus();
				break;
			// 2 Focus on query structure
			case 50:
				$('#qb-ui-canvas-navbar-plus-button').focus();
				break;
			// 3 Focus on canvas
			case 51:
				$('#qb-ui-canvas .qb-ui-table:first').focus();
				break;
			// 4 Focus on Grid
			case 52:
				$('#qb-ui-grid').focus();
				break;
			// 5 Focus on SQL Text editor
			case 53:
				$('#qb-ui-editor textarea').focus();
				e.preventDefault();
				break;
		}
	});
});