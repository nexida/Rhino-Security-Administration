using System.Linq;
using NHibernate.Linq;
using Rhino.Security.Mgmt.Infrastructure;

namespace Rhino.Security.Mgmt.Data
{
	public class UserRepository : Rhino.Security.Mgmt.Infrastructure.IRepository
	{
		private NHibernate.ISessionFactory _northwindWithSecurity;
		AuthorizationRepositoryFactory _authorizationRepositoryFactory;

		public UserRepository(NHibernate.ISessionFactory northwindWithSecurity, AuthorizationRepositoryFactory authorizationRepositoryFactory)
		{
			_authorizationRepositoryFactory = authorizationRepositoryFactory;
			_northwindWithSecurity = northwindWithSecurity;
		}

		public Rhino.Security.Model.User Create(Rhino.Security.Model.User v)
		{
			_northwindWithSecurity.GetCurrentSession().Save(v);
			return v;
		}

		public Rhino.Security.Model.User Read(System.Int64 id)
		{
			return _northwindWithSecurity.GetCurrentSession().Load<Rhino.Security.Model.User>(id);
		}

		public Rhino.Security.Model.User Update(Rhino.Security.Model.User v)
		{
			_northwindWithSecurity.GetCurrentSession().Update(v);
			return v;
		}

		public void Delete(Rhino.Security.Model.User v)
		{
			// remove user from Rhino first
			_authorizationRepositoryFactory.Create().RemoveUser(v);
			_northwindWithSecurity.GetCurrentSession().Delete(v);
		}

		public IPresentableSet<Rhino.Security.Model.User> Search(System.Int64? id, string name)
		{
			IQueryable<Rhino.Security.Model.User> queryable = _northwindWithSecurity.GetCurrentSession().Linq<Rhino.Security.Model.User>();
			if (id != default(System.Int64?))
			{
				queryable = queryable.Where(x => x.Id == id);
			}
			if (!string.IsNullOrEmpty(name))
			{
				queryable = queryable.Where(x => x.Name.StartsWith(name));
			}

			return new Rhino.Security.Mgmt.Infrastructure.QueryablePresentableSet<Rhino.Security.Model.User>(queryable);
		}

		public IPresentableSet<Rhino.Security.Model.User> SearchByGroup(Rhino.Security.Model.UsersGroup Group)
		{
			IQueryable<Rhino.Security.Model.User> queryable = _northwindWithSecurity.GetCurrentSession().Linq<Rhino.Security.Model.User>();
			if (Group != default(Rhino.Security.Model.UsersGroup))
			{
				queryable = queryable.Where(x => x.Groups.Contains(Group));
			}

			return new Rhino.Security.Mgmt.Infrastructure.QueryablePresentableSet<Rhino.Security.Model.User>(queryable);
		}
	}
}