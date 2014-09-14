using System.ComponentModel.DataAnnotations;
using THResources;

namespace UI.Models
{
	public class WcLoginViewModel
	{
		/// <summary>
		/// For user enters either name or email upon login
		/// </summary>
		[Required(ErrorMessageResourceName = "UserNameEmailRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
		[Display(Name = "UserNameEmailForLogin", ResourceType = typeof(Resources))]
		public string UserNameEmail { get; set; }

		/// <summary>
		/// For greetings only
		/// </summary>
		public string Name { get; set; }

		[Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
		[DataType(DataType.Password)]
		[Display(Name = "Password", ResourceType = typeof(Resources))]
		public string Password { get; set; }

		[Display(Name = "RememberMe", ResourceType = typeof(Resources))]
		public bool RememberMe { get; set; }
	}

	public class WcRegisterViewModel
	{
		[Required(ErrorMessageResourceName = "UserNameRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
		[Display(Name = "UserName", ResourceType = typeof(Resources))]
		public string UserName { get; set; }

		[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
		[EmailAddress(ErrorMessageResourceName = "EmailFormatError", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
		[Display(Name = "Email", ResourceType = typeof(Resources))]
		public string Email { get; set; }

		[Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
		[StringLength(20, ErrorMessageResourceName = "PasswordLengthError", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null, MinimumLength = 6)]
		//[MaxLength(ErrorMessageResourceName = "MaxPasswordLengthError", ErrorMessageResourceType = typeof(Resources))]
		[DataType(DataType.Password)]
		[Display(Name = "Password", ResourceType = typeof(Resources))]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "ConfirmPassword", ResourceType = typeof(Resources))]
		[Compare("Password", ErrorMessageResourceName = "ConfirmPasswordError", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
		public string ConfirmPassword { get; set; }
	}

}