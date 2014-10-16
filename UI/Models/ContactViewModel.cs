using System.ComponentModel.DataAnnotations;
using THResources;
using UI.Models.Abstract;

namespace UI.Models
{
	public class ContactViewModel:ViewModelBase
	{
		[Display(ResourceType = typeof (Resources), Name = "Name")]
		[Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Resources))]
		public string Name { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "Email")]
		[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resources))]
		public string Email { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "Opinion")]
		[Required(ErrorMessageResourceName = "OpinionRequired", ErrorMessageResourceType = typeof(Resources))]
		public string MessageText { get; set; }
	}
}