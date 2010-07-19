using System;
using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;

namespace Conversation.NHibernate
{
	[Serializable]
	public class ConversationSessionContext : ICurrentSessionContext
	{
		private readonly ISessionFactoryImplementor _factory;

		public ConversationSessionContext(ISessionFactoryImplementor factory)
		{
			_factory = factory;
		}

		public ISession CurrentSession()
		{
			return CurrentConversationHolder.Instance.GetSession(_factory);
		}
	}
}