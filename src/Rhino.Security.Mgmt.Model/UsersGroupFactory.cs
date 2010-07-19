using Rhino.Security.Model;
namespace Rhino.Security.Mgmt.Model

{
	public class UsersGroupFactory : Rhino.Security.Mgmt.Infrastructure.IFactory<UsersGroup>
	{
		public UsersGroup Create()
		{
			return new UsersGroup();
		}
	}
}