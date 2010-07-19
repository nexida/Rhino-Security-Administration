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
	public class AuthorizationRepositoryFactory
	{
		ISessionFactory _sessionFactory;
		public AuthorizationRepositoryFactory(ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;
		}

		public IAuthorizationRepository Create()
		{
			return new AuthorizationRepository(_sessionFactory.GetCurrentSession());
		}
	}
}
