using System.Linq;
using NHibernate.Linq;
using Rhino.Security.Mgmt.Infrastructure;
using Rhino.Security.Mgmt.Infrastructure;
using Rhino.Security.Interfaces;
using Rhino.Security.Model;

namespace Rhino.Security.Mgmt.Data
{
	public class PermissionRepository : Rhino.Security.Mgmt.Infrastructure.IRepository
	{
		private NHibernate.ISessionFactory _northwindWithSecurity;
		PermissionsBuilderServiceFactory _permissionBuilderServiceFactory;
		PermissionsServiceFactory _permissionsServiceFactory;

		public PermissionRepository(NHibernate.ISessionFactory northwindWithSecurity, PermissionsBuilderServiceFactory permissionBuilderServiceFactory, PermissionsServiceFactory permissionsServiceFactory)
		{
			_northwindWithSecurity = northwindWithSecurity;
			_permissionBuilderServiceFactory = permissionBuilderServiceFactory;
			_permissionsServiceFactory = permissionsServiceFactory;
		}

		public Permission Create(Permission permission)
		{
			var builder = _permissionBuilderServiceFactory.Create();

			var forPermissionBuilder = ((permission.Allow) ? builder.Allow(permission.Operation) : builder.Deny(permission.Operation));
			if (permission.User != null)
			{
 				return forPermissionBuilder.For(permission.User).OnEverything().DefaultLevel().Save();
			}
			else if (permission.UsersGroup != null)
			{
				return forPermissionBuilder.For(permission.UsersGroup).OnEverything().DefaultLevel().Save();
			}

			return null;
		}

		public Rhino.Security.Model.Permission[] ReadByOperation(string operationName)
		{
			var permissionsService = _permissionsServiceFactory.Create();
			return permissionsService.GetPermissionsFor(operationName);
		}

		public Rhino.Security.Model.Permission Read(System.Guid id)
		{
			return _northwindWithSecurity.GetCurrentSession().Load<Permission>(id);
		}

		public void Delete(Permission p)
		{
			_northwindWithSecurity.GetCurrentSession().Delete(p);
		}
	}
}