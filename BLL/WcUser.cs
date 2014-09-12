using BLL.Abstract;

namespace BLL
{
	public class WcUser:UserRoleBase
	{
		public string Password { get; set; }
		public string Email { get; set; }
		public int RoleId { get; set; }

		public string RoleName { get; set; }
		
	}
}
