var duplicate;
var invalid;
var pwdlength;

(function ($)
{
	// clear all input fields first, especially for chrome...
	//$('input:not([type=image],[type=button],[type=submit])')
	$(window).on('load', function ()
	{
		setTimeout(function () { $('input:not([type=hidden])').val('-').val(null); }, 1);
	});

	

	$("#alert").hide();
	$("#emailOk").hide();
	$("#pwdOk").hide();
	duplicate = $("#duplicate").val();
	invalid = $("#invalid").val();
	pwdlength = $("#pwdlength").val();

	$("#Email").bind("keyup keydown keypress", function (e)
	{
		$("#alert").hide();
		$("#emailOk").hide();

		//var keyCode = e.keyCode || e.which;
		//// catch tap event
		//if (keyCode == 9) {
		//	e.preventDefault();
		//	checkMail();
		//}
	});

	$("#Password").bind("keyup keydown keypress", function (e)
	{
		$("#alert").hide();
		$("#pwdOk").hide();

		//var keyCode = e.keyCode || e.which;
		//// catch tap event
		//if (keyCode == 9)
		//{
		//	e.preventDefault();
		//	checkPassword();
		//}
	});

	$("#Email").bind("change", function (e) {
		checkMail();
	});

	$("#Password").bind("change", function (e) {
		checkPassword();
	});

})(jQuery);

function checkMail()
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
			//console.log(data.d);
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
}

function checkPassword() {
	var pwd = $("#Password").val();

	$.ajax({
		url: '/WebServices/WcServices.asmx/CheckPasswordLength',
		type: 'POST',
		dataType: 'json',
		contentType: "application/json; charset=utf-8",
		data: JSON.stringify({ "package": pwd }),
		success: function (data)
		{
			//console.log(data.d);
			var bRet = data.d;
			if (!bRet)
			{
				$("#alert").show().text(pwdlength);
				$("#pwdOk").hide();
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
}
