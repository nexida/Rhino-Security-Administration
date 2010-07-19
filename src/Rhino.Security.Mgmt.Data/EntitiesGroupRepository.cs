using System.Linq;
using NHibernate.Linq;
using Rhino.Security.Mgmt.Infrastructure;

namespace Rhino.Security.Mgmt.Data
{
	public class EntitiesGroupRepository : Rhino.Security.Mgmt.Infrastructure.IRepository
	{

				private NHibernate.ISessionFactory _northwindWithSecurity;
				

		public EntitiesGroupRepository(NHibernate.ISessionFactory northwindWithSecurity)	
		{

						_northwindWithSecurity = northwindWithSecurity;
						
		}
		
		public void Create(Rhino.Security.Model.EntitiesGroup v)
		{
			_northwindWithSecurity.GetCurrentSession().Save(v);
		}

		public Rhino.Security.Model.EntitiesGroup Read(System.Guid id)
		{
			return _northwindWithSecurity.GetCurrentSession().Load<Rhino.Security.Model.EntitiesGroup>(id);
		}

		public void Update(Rhino.Security.Model.EntitiesGroup v)
		{
			_northwindWithSecurity.GetCurrentSession().Update(v);
		}

		public void Delete(Rhino.Security.Model.EntitiesGroup v)
		{
			_northwindWithSecurity.GetCurrentSession().Delete(v);
		}

				public IPresentableSet<Rhino.Security.Model.EntitiesGroup> Search(System.Guid? id, string name)
				{
					IQueryable<Rhino.Security.Model.EntitiesGroup> queryable = _northwindWithSecurity.GetCurrentSession().Linq<Rhino.Security.Model.EntitiesGroup>();
								if(id != default(System.Guid?))
								{
									queryable = queryable.Where(x => x.Id == id);
								}
											if(!string.IsNullOrEmpty(name))
								{
									queryable = queryable.Where(x => x.Name.StartsWith(name));
								}
								
					return new Rhino.Security.Mgmt.Infrastructure.QueryablePresentableSet<Rhino.Security.Model.EntitiesGroup>(queryable);
				}
				
	}
}