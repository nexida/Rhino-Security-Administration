using System.Collections.Generic;
using System.Web;
using NHibernate;

namespace Conversation.NHibernate
{
	public class WebCurrentConversationHolder : CurrentConversationHolder
	{
		public override IDictionary<ISessionFactory, ISession> Map
		{
			get { return (IDictionary<ISessionFactory, ISession>)HttpContext.Current.Items[GetType().FullName]; }
			set { HttpContext.Current.Items[GetType().FullName] = value; }
		}
	}
}