using System;
using System.Collections.Generic;
using NHibernate;

namespace Conversation.NHibernate
{
	public class DesktopCurrentConversationHolder : CurrentConversationHolder
	{
		[ThreadStatic]
		private static IDictionary<ISessionFactory, ISession> _map;

		public override IDictionary<ISessionFactory, ISession> Map
		{
			get { return _map; }
			set { _map = value; }
		}
	}
}