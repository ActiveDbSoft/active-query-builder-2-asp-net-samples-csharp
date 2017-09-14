OnApplicationReady(function() {
	QB.Web.Application.Grid.bindEx(QB.Web.Grid.Events.GridBeforeCustomEditCell, BeforeCustomEditCell, this);
});


BeforeCustomEditCell = function (e, data)
{
	var rowDto = data.dto;
	var rowDto = data.dto;
	var cell = data.cell;
	var html = "";
	var oldValue = data.value;
	if (isEmpty(oldValue)) oldValue = '';
	html += "<p>Expression:" + rowDto.Expression + "</p>";
	html += "<p>Column:" + data.column + "</p>";
	html += "<textarea name='value' cols='36'>" + oldValue + "</textarea>";

	var oldValue = data.value;
	var $dialog = $('<div>Waiting server event handler...</div>').dialog({
	    modal: true,
	    width: 'auto',
		title: 'Custom expression editor',
		buttons: [
			{
				text: "OK",
				click: function () {
					var newValue = $(this).find('textarea').val();
					cell.updateValue(newValue);
					$(this).dialog("close");
				}
			},
			{
				text: "Cancel",
				click: function () {
					$(this).dialog("close");
				}
			}
		]
	});
	QB.Web.Application.Grid.QBGrid('OnBeforeCustomEditCell', rowDto, data.column, data.value, function () {
		$dialog.html(html);
	});
};