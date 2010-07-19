using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Security.Interfaces;
using NHibernate;
using Rhino.Security.Services;
using Rhino.Security.Mgmt.Infrastructure;

namespace Rhino.Security.Mgmt.Infrastructure
{
	public class PermissionsServiceFactory
	{
		ISessionFactory _sessionFactory;
		AuthorizationRepositoryFactory _authorizationRepositoryFactory;

		public PermissionsServiceFactory(AuthorizationRepositoryFactory authorizationRepositoryFactory, ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;
			_authorizationRepositoryFactory = authorizationRepositoryFactory;
		}

		public IPermissionsService Create()
		{
			return new PermissionsService(_authorizationRepositoryFactory.Create(), _sessionFactory.GetCurrentSession());
		}
	}
}
