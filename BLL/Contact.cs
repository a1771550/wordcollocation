using System.ComponentModel.DataAnnotations;
using THResources;

namespace BLL
{
	public class Contact
	{
		[Display(Name = "UserName", ResourceType = typeof(Resources))]
		[Required(ErrorMessageResourceType = typeof(Resources),
				  ErrorMessageResourceName = "UserNameRequired")]
		public string UserName { get; set; }

		[Display(Name = "Email", ResourceType = typeof(Resources))]
		[Required(ErrorMessageResourceType = typeof(Resources),
				  ErrorMessageResourceName = "EmailRequired")]
		public string Email { get; set; }

		[Display(Name = "Message", ResourceType = typeof(Resources))]
		[Required(ErrorMessageResourceType = typeof(Resources),
				  ErrorMessageResourceName = "MessageRequired")]
		public string Message { get; set; }
	}
}