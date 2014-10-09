(function ($)
{
	// don't move this code...
	if ($("div#progress").length) $("div#progress").hide();
	$("button[name='btnSearch']").click(function (e)
	{
		e.preventDefault();
		var wordrequired = $("#wordRequired").val();
		var colposrequired = $("#colPosRequired").val();
		var word = $("input[name='Word']").val();
		var submit = true;
		if (word == '')
		{
			alert(wordrequired);
			$("input[name='Word']").focus();
			submit = false;
		}
		var id = $("select#ColPosId").val();
		if (id == "0")
		{
			alert(colposrequired);
			$("select#ColPosId").focus();
			submit = false;
		}

		//if (submit) $("form#search-form").submit();

		if (submit)
		{
			showProgress();
			$.ajax({
				url: '/WebServices/WcServices.asmx/SearchCollocation',
				type: 'POST',
				dataType: 'json',
				contentType: "application/json; charset=utf-8",
				data: JSON.stringify({ word: word, id: id }),
				success: function (data, textStatus, jqXHR)
				{
					window.location.assign(data.d);
				},
				error: function (data)
				{
					var msg = "Error: " + data.Messag;
					alert(msg);
					console.log(msg);
				}
			});
		}
	});
})(jQuery);

function showProgress()
{
	$("div#progress").show();
	$("div#progress").dialog({
		closeOnEscape: false,
		open: function (event, ui) { $(".ui-dialog-titlebar-close", ui.dialog || ui).hide(); },
		dialogClass: "modal-dialog",
		buttons: {},
		modal: true,
		width: 150,
		height: 'auto',
	});
}