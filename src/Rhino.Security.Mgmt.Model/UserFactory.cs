using Rhino.Security.Model;
namespace Rhino.Security.Mgmt.Model

{
	public class UserFactory : Rhino.Security.Mgmt.Infrastructure.IFactory<User>
	{
		public User Create()
		{
			return new User();
		}
	}
}