(function ($)
{
	$("#alert").hide();
	$("#emailOk").hide();
	$("#pwdOk").hide();
	var duplicate = $("#duplicate").val();
	var invalid = $("#invalid").val();
	var pwdlength = $("#pwdlength").val();

	$("#Email").bind("keyup keydown keypress", function (e)
	{
		$("#alert").hide();
		$("#emailOk").hide();
	});

	$("#Password").bind("keyup keydown keypress", function(e) {
		$("#alert").hide();
		$("#pwdOk").hide();
	});

	$("#Email").bind("change", function (e)
	{
		var email = $("#Email").val();

		$.ajax({
			url: '/WebServices/WcServices.asmx/CheckEmail',
			type: 'POST',
			dataType: 'json',
			contentType: "application/json; charset=utf-8",
			data: JSON.stringify({ email: email }),
			success: function (data)
			{
				console.log(data.d);
				var bRet = data.d;
				if (!bRet[0])
				{
					$("#alert").show().text(duplicate);
					$("#Email").focus();
				}
				else if (!bRet[1])
				{
					$("#alert").show().text(invalid);
					$("#Email").focus();
				}
				else if (bRet[0] && bRet[1])
				{
					$("#alert").hide();
					$("#emailOk").show();
				}

			},
			error: function (data)
			{
				$('#alert').show().text('Error: ' + data.Message);
			}
		});
	});

	$("#Password").bind("change", function (e)
	{
		var pwd = $("#Password").val();

		$.ajax({
			url: '/WebServices/WcServices.asmx/CheckPasswordLength',
			type: 'POST',
			dataType: 'json',
			contentType: "application/json; charset=utf-8",
			data: JSON.stringify({ "package": pwd }),
			success: function (data)
			{
				console.log(data.d);
				var bRet = data.d;
				if (!bRet)
				{
					$("#alert").show().text(pwdlength);
					$("#Password").focus();
				}
				else
				{
					$("#alert").hide();
					$("#pwdOk").show();
				}

			},
			error: function (data)
			{
				$('#alert').show().text('Error: ' + data.Message);
			}
		});
	});

})(jQuery);