using System.Linq;
using NHibernate.Linq;
using Rhino.Security.Mgmt.Infrastructure;

namespace Rhino.Security.Mgmt.Data
{
	public class OperationRepository : Rhino.Security.Mgmt.Infrastructure.IRepository
	{
		private NHibernate.ISessionFactory _northwindWithSecurity;
		AuthorizationRepositoryFactory _authorizationRepositoryFactory;

		public OperationRepository(NHibernate.ISessionFactory northwindWithSecurity, AuthorizationRepositoryFactory authorizationRepositoryFactory)
		{
			_authorizationRepositoryFactory = authorizationRepositoryFactory;
			_northwindWithSecurity = northwindWithSecurity;
		}

		public Rhino.Security.Model.Operation Create(Rhino.Security.Model.Operation v)
		{
			return _authorizationRepositoryFactory.Create().CreateOperation(v.Name);
		}

		public Rhino.Security.Model.Operation Read(System.Guid id)
		{
			return _northwindWithSecurity.GetCurrentSession().Load<Rhino.Security.Model.Operation>(id);
		}

		public Rhino.Security.Model.Operation Update(Rhino.Security.Model.Operation v)
		{
			_northwindWithSecurity.GetCurrentSession().Update(v);
			return v;
		}

		public void Delete(Rhino.Security.Model.Operation v)
		{
			_authorizationRepositoryFactory.Create().RemoveOperation(v.Name);
		}

		public IPresentableSet<Rhino.Security.Model.Operation> GetAll()
		{
			return new Rhino.Security.Mgmt.Infrastructure.QueryablePresentableSet<Rhino.Security.Model.Operation>(_northwindWithSecurity.GetCurrentSession().Linq<Rhino.Security.Model.Operation>());
		}

		public IPresentableSet<Rhino.Security.Model.Operation> Search(System.Guid? id, string name, string comment)
		{
			IQueryable<Rhino.Security.Model.Operation> queryable = _northwindWithSecurity.GetCurrentSession().Linq<Rhino.Security.Model.Operation>();
			if (id != default(System.Guid?))
			{
				queryable = queryable.Where(x => x.Id == id);
			}
			if (!string.IsNullOrEmpty(name))
			{
				queryable = queryable.Where(x => x.Name.StartsWith(name));
			}
			if (!string.IsNullOrEmpty(comment))
			{
				queryable = queryable.Where(x => x.Comment.StartsWith(comment));
			}

			return new Rhino.Security.Mgmt.Infrastructure.QueryablePresentableSet<Rhino.Security.Model.Operation>(queryable);
		}

	}
}