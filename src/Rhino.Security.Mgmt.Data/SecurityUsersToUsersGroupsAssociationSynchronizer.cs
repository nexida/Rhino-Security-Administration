using Rhino.Security.Mgmt.Infrastructure;
namespace Rhino.Security.Mgmt.Data
{
	public class SecurityUsersToUsersGroupsAssociationSynchronizer
	{
		AuthorizationRepositoryFactory _authorizationRepositoryFactory;
		public SecurityUsersToUsersGroupsAssociationSynchronizer(AuthorizationRepositoryFactory authorizationRepositoryFactory)
		{
			_authorizationRepositoryFactory = authorizationRepositoryFactory;
		}

		public void Associate(Rhino.Security.Model.User user, Rhino.Security.Model.UsersGroup usersGroup)
		{
			user.Groups.Add(usersGroup);
			_authorizationRepositoryFactory.Create().AssociateUserWith(user, usersGroup.Name);
		}

		public void Disassociate(Rhino.Security.Model.User user, Rhino.Security.Model.UsersGroup usersGroup)
		{
			user.Groups.Remove(usersGroup);
			_authorizationRepositoryFactory.Create().DetachUserFromGroup(user, usersGroup.Name);
		}

	}
}