using System.Linq;
using NHibernate.Linq;
using Rhino.Security.Mgmt.Infrastructure;

namespace Rhino.Security.Mgmt.Data
{
	public class EntityReferenceRepository : Rhino.Security.Mgmt.Infrastructure.IRepository
	{

				private NHibernate.ISessionFactory _northwindWithSecurity;
				

		public EntityReferenceRepository(NHibernate.ISessionFactory northwindWithSecurity)	
		{

						_northwindWithSecurity = northwindWithSecurity;
						
		}
		
		public void Create(Rhino.Security.Model.EntityReference v)
		{
			_northwindWithSecurity.GetCurrentSession().Save(v);
		}

		public Rhino.Security.Model.EntityReference Read(System.Guid id)
		{
			return _northwindWithSecurity.GetCurrentSession().Load<Rhino.Security.Model.EntityReference>(id);
		}

		public void Update(Rhino.Security.Model.EntityReference v)
		{
			_northwindWithSecurity.GetCurrentSession().Update(v);
		}

		public void Delete(Rhino.Security.Model.EntityReference v)
		{
			_northwindWithSecurity.GetCurrentSession().Delete(v);
		}

				public IPresentableSet<Rhino.Security.Model.EntityReference> Search(System.Guid? id, System.Guid? entitySecurityKey)
				{
					IQueryable<Rhino.Security.Model.EntityReference> queryable = _northwindWithSecurity.GetCurrentSession().Linq<Rhino.Security.Model.EntityReference>();
								if(id != default(System.Guid?))
								{
									queryable = queryable.Where(x => x.Id == id);
								}
											if(entitySecurityKey != default(System.Guid?))
								{
									queryable = queryable.Where(x => x.EntitySecurityKey == entitySecurityKey);
								}
								
					return new Rhino.Security.Mgmt.Infrastructure.QueryablePresentableSet<Rhino.Security.Model.EntityReference>(queryable);
				}
				
	}
}