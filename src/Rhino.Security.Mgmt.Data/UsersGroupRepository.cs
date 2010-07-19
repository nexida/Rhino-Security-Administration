using System.Linq;
using NHibernate.Linq;
using Rhino.Security.Mgmt.Infrastructure;

namespace Rhino.Security.Mgmt.Data
{
	public class UsersGroupRepository : Rhino.Security.Mgmt.Infrastructure.IRepository
	{
		private NHibernate.ISessionFactory _northwindWithSecurity;
		AuthorizationRepositoryFactory _authorizationRepositoryFactory;

		public UsersGroupRepository(NHibernate.ISessionFactory northwindWithSecurity, AuthorizationRepositoryFactory authorizationRepositoryFactory)
		{
			_authorizationRepositoryFactory = authorizationRepositoryFactory;
			_northwindWithSecurity = northwindWithSecurity;
		}

		public Rhino.Security.Model.UsersGroup Create(Rhino.Security.Model.UsersGroup v)
		{
			return _authorizationRepositoryFactory.Create().CreateUsersGroup(v.Name);
		}

		public Rhino.Security.Model.UsersGroup Read(System.Guid id)
		{
			return _northwindWithSecurity.GetCurrentSession().Load<Rhino.Security.Model.UsersGroup>(id);
		}

		public Rhino.Security.Model.UsersGroup Update(Rhino.Security.Model.UsersGroup v)
		{
			_northwindWithSecurity.GetCurrentSession().Update(v);
			return v;
		}

		public void Delete(Rhino.Security.Model.UsersGroup v)
		{
			_authorizationRepositoryFactory.Create().RemoveUsersGroup(v.Name);
		}

		public IPresentableSet<Rhino.Security.Model.UsersGroup> Search(string name)
		{
#warning we might need to use the AuthZRepo for this operation
			IQueryable<Rhino.Security.Model.UsersGroup> queryable = _northwindWithSecurity.GetCurrentSession().Linq<Rhino.Security.Model.UsersGroup>();
			if (!string.IsNullOrEmpty(name))
			{
				queryable = queryable.Where(x => x.Name.StartsWith(name));
			}

			return new Rhino.Security.Mgmt.Infrastructure.QueryablePresentableSet<Rhino.Security.Model.UsersGroup>(queryable);
		}

	}
}