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
	public class PermissionsBuilderServiceFactory
	{
		ISessionFactory _sessionFactory;
		AuthorizationRepositoryFactory _authorizationRepositoryFactory;

		public PermissionsBuilderServiceFactory(ISessionFactory sessionFactory, AuthorizationRepositoryFactory authorizationRepositoryFactory)
		{
			_sessionFactory = sessionFactory;
			_authorizationRepositoryFactory = authorizationRepositoryFactory;
		}

		public IPermissionsBuilderService Create()
		{
			return new PermissionsBuilderService(_sessionFactory.GetCurrentSession(), _authorizationRepositoryFactory.Create());
		}
	}
}
