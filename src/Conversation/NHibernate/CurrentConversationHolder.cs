using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Engine;

namespace Conversation.NHibernate
{
	public abstract class CurrentConversationHolder
	{
		public static readonly CurrentConversationHolder Instance = CreateInstance();

		public abstract IDictionary<ISessionFactory, ISession> Map { get; set; }

		private static CurrentConversationHolder CreateInstance()
		{
			if(HttpContext.Current != null)
			{
				return new WebCurrentConversationHolder();
			}
			return new DesktopCurrentConversationHolder();
		}

		public void Bind(IDictionary<ISessionFactory, ISession> map)
		{
			if(map == null)
			{
				throw new ArgumentNullException("map");
			}
			if(Map != null)
			{
				throw new ConversationException("Another conversation is currently in SetAsCurrent");
			}
			Map = map;
		}

		public void UnBind(IDictionary<ISessionFactory, ISession> map)
		{
			if(!ReferenceEquals(map, Map))
			{
				throw new ConversationException("Cannot unbind a conversation that is not currently in SetAsCurrent");
			}
			Map = null;
		}

		public ISession GetSession(ISessionFactoryImplementor factory)
		{
			ISession session;
			if(Map != null && Map.TryGetValue(factory, out session) && session != null)
			{
				return session;
			}
			throw new ConversationException("No current conversation. Make sure the operation is executed with a Opened conversation in SetAsCurrent");
		}
	}
}