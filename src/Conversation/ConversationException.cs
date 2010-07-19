using System;

namespace Conversation
{
	public class ConversationException : Exception
	{
		public ConversationException(string message) : base(message) {}
	}
}