using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace Conversation.NHibernate
{
	/// <summary>
	/// 	This is an implementation of Conversation per Business Transaction pattern.
	/// 	Doesn't work with tables with "assigned" generator class (like tables with IDENTITY column in SQL Server)
	/// 	See http://fabiomaulo.blogspot.com/2008/12/identity-never-ending-story.html
	/// </summary>
	public class NHibernateConversation : IConversation
	{
		private readonly IDictionary<ISessionFactory, ISession> _map;
		private ConversationState _state;

		public NHibernateConversation(IDictionary<ISessionFactory, ISession> map)
		{
			_map = map;
			_state = ConversationState.Opened;
		}

		public IDisposable SetAsCurrent()
		{
			CheckState(ConversationState.Opened);
			foreach(ISession session in _map.Values)
			{
				session.BeginTransaction();
			}
			CurrentConversationHolder.Instance.Bind(_map);
			_state = ConversationState.InContext;
			return new DisposeAction(ResetCurrent);
		}

		public void ResetCurrent()
		{
			CheckState(ConversationState.InContext);
			CurrentConversationHolder.Instance.UnBind(_map);
			foreach(ISession session in _map.Values)
			{
				session.Transaction.Commit();
			}
			_state = ConversationState.Opened;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Close()
		{
			CheckState(ConversationState.Opened);
			foreach(ISessionFactory sessionFactory in _map.Keys.ToArray())
			{
				_map[sessionFactory].Close();
				_map[sessionFactory].Dispose();
				_map[sessionFactory] = null;
			}
			_state = ConversationState.Closed;
		}

		public void Flush()
		{
			CheckState(ConversationState.Opened, ConversationState.InContext);
			foreach(ISession session in _map.Values)
			{
				if (_state == ConversationState.Opened)
				{
					session.BeginTransaction();
				}
				try
				{
					session.Flush();
					if (_state == ConversationState.Opened)
					{
						session.Transaction.Commit();
					}
				}
				finally
				{
					if (_state == ConversationState.Opened)
					{
						session.Transaction.Dispose();
					}
				}
			}
		}

		private void CheckState(params ConversationState[] validStates)
		{
			foreach(ConversationState validState in validStates)
			{
				if(_state == validState)
				{
					return;
				}
			}
			throw new ConversationException(string.Concat("Operation not allowed in ", _state, " state"));
		}

		protected void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(_state == ConversationState.InContext)
				{
					ResetCurrent();
				}
				if(_state == ConversationState.Opened)
				{
					Close();
				}
			}
		}

		~NHibernateConversation()
		{
			Dispose(false);
		}

		public ISession GetASession()
		{
			CheckState(ConversationState.Opened);
			return _map.Values.FirstOrDefault();
		}
	}
}