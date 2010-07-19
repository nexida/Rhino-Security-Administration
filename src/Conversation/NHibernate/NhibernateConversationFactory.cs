using System.Collections.Generic;
using NHibernate;
using NHibernate.Engine;

namespace Conversation.NHibernate
{
	public class NHibernateConversationFactory : IConversationFactory
	{
		private readonly IEnumerable<ISessionFactory> _sessionFactories;

		public NHibernateConversationFactory(IEnumerable<ISessionFactory> sessionFactories)
		{
			_sessionFactories = sessionFactories;
		}

		public IConversation Open()
		{
			var map = new Dictionary<ISessionFactory, ISession>();
			foreach(ISessionFactoryImplementor sessionFactory in _sessionFactories)
			{
				ISession session = sessionFactory.OpenSession(null, false, false, sessionFactory.Settings.ConnectionReleaseMode);
				session.FlushMode = FlushMode.Never;
				map.Add(sessionFactory, session);
			}
			return new NHibernateConversation(map);
		}
	}
}