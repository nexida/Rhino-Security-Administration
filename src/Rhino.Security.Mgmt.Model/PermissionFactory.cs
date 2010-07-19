using Rhino.Security.Model;
namespace Rhino.Security.Mgmt.Model

{
	public class PermissionFactory : Rhino.Security.Mgmt.Infrastructure.IFactory<Permission>
	{
		public Permission Create()
		{
			return new Permission();
		}
	}
}