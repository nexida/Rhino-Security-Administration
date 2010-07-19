using System.Linq;
using NHibernate.Linq;
using Rhino.Security.Mgmt.Infrastructure;

namespace Rhino.Security.Mgmt.Data
{
	public class EntityTypeRepository : Rhino.Security.Mgmt.Infrastructure.IRepository
	{

				private NHibernate.ISessionFactory _northwindWithSecurity;
				

		public EntityTypeRepository(NHibernate.ISessionFactory northwindWithSecurity)	
		{

						_northwindWithSecurity = northwindWithSecurity;
						
		}
		
		public void Create(Rhino.Security.Model.EntityType v)
		{
			_northwindWithSecurity.GetCurrentSession().Save(v);
		}

		public Rhino.Security.Model.EntityType Read(System.Guid id)
		{
			return _northwindWithSecurity.GetCurrentSession().Load<Rhino.Security.Model.EntityType>(id);
		}

		public void Update(Rhino.Security.Model.EntityType v)
		{
			_northwindWithSecurity.GetCurrentSession().Update(v);
		}

		public void Delete(Rhino.Security.Model.EntityType v)
		{
			_northwindWithSecurity.GetCurrentSession().Delete(v);
		}

				public IPresentableSet<Rhino.Security.Model.EntityType> Search(System.Guid? id, string name)
				{
					IQueryable<Rhino.Security.Model.EntityType> queryable = _northwindWithSecurity.GetCurrentSession().Linq<Rhino.Security.Model.EntityType>();
								if(id != default(System.Guid?))
								{
									queryable = queryable.Where(x => x.Id == id);
								}
											if(!string.IsNullOrEmpty(name))
								{
									queryable = queryable.Where(x => x.Name.StartsWith(name));
								}
								
					return new Rhino.Security.Mgmt.Infrastructure.QueryablePresentableSet<Rhino.Security.Model.EntityType>(queryable);
				}
				
	}
}