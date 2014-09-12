using System.ComponentModel.DataAnnotations;
using THResources;

namespace BLL.Abstract
{
	public abstract class UserRoleBase
	{
		[Key]
		public virtual int Id { get; set; }

		[Required(ErrorMessageResourceType = typeof (Resources), ErrorMessageResourceName = "NameRequired")]
		public virtual string Name { get; set; }
		public virtual byte[] RowVersion { get; set; }

	}
}
