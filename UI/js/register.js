var duplicateEmail;
var duplicateName;
var invalid;
var pwdlength;
var confirmPwdError;

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
	$("#nameOk").hide();
	$("#pwdOk").hide();
	$("#confirmOk").hide();

	duplicateEmail = $("#duplicateEmail").length ? $("#duplicateEmail").val() : null;
	duplicateName = $("#duplicateName").length ? $("#duplicateName").val() : null;
	invalid = $("#invalid").length ? $("#invalid").val() : null;
	pwdlength = $("#pwdlength").length ? $("#pwdlength").val() : null;
	confirmPwdError = $("#confirmPwdError").length ? $("#confirmPwdError").val():null;

	if ($("#Email").length) {
		$("#Email").bind("keyup keydown keypress", function (e)
		{
			$("#alert").hide();
			$("#emailOk").hide();
		});
	}
	
	if ($("#UserName").length) {
		$("#UserName").bind("keyup keydown keypress", function (e)
		{
			$("#alert").hide();
			$("#nameOk").hide();
		});
	}
	
	if ($("#Password").length) {
		$("#Password").bind("keyup keydown keypress", function (e)
		{
			$("#alert").hide();
			$("#pwdOk").hide();
		});
	}
	
	if ($("#ConfirmPassword").length) {
		$("#ConfirmPassword").bind("keyup keydown keypress", function (e)
		{
			$("#alert").hide();
			$("#confirmOk").hide();
		});
	}
	
	if ($("#Email").length) {
		$("#Email").bind("change", function (e)
		{
			checkMail();
		});
	}
	
	if ($("#UserName").length) {
		$("#UserName").bind("change", function (e)
		{
			checkUserName();
		});
	}
	
	if ($("#Password").length) {
		$("#Password").bind("change", function (e)
		{
			checkPassword();
		});
	}
	
	if ($("#ConfirmPassword").length) {
		$("#ConfirmPassword").bind("change", function (e)
		{
			checkConfirmPassword();
		});
	}
	

})(jQuery);

function checkConfirmPassword() {
	var cp = $("#ConfirmPassword").val();
	var p = $("#Password").val();
	if (cp == p) {
		$("#alert").hide();
		$("#confirmOk").show();
	} else {
		$("#alert").show().text(confirmPwdError);
		$("#ConfirmPassword").focus();
	}
}

function checkUserName()
{
	var name = $("#UserName").val();
	$.ajax({
		url: '/WebServices/WcServices.asmx/CheckIfDuplicatedUserName',
		type: 'POST',
		dataType: 'json',
		contentType: "application/json; charset=utf-8",
		data: JSON.stringify({ name: name }),
		success: function (data)
		{
			var bRet = data.d;
			if (bRet) {
				$("#alert").show().text(duplicateName);
				$("#UserName").focus();
			} else
			{
				$("#alert").hide();
				$("#nameOk").show();
			}
		},
		error: function (data)
		{
			showError(data.Message);
		}
	});
}

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
				$("#alert").show().text(duplicateEmail);
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
			showError(data.Message);
		}
	});
}

function checkPassword()
{
	var pwd = $("#Password").val();

	$.ajax({
		url: '/WebServices/WcServices.asmx/CheckPasswordLength',
		type: 'POST',
		dataType: 'json',
		contentType: "application/json; charset=utf-8",
		data: JSON.stringify({ "package": pwd }),
		success: function (data) {
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
			showError(data.Message);
		}
	});
}

function showError(error)
{
	$('#alert').show().text('Error: ' + error);
}