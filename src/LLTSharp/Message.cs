using System;
using System.Collections.Generic;
using System.Text;

namespace LLTSharp
{
	/// <summary>
	/// Represents a role in a conversation.
	/// </summary>
	public enum Role
	{
		/// <summary>
		/// System role, typically used for system instructions or context.
		/// </summary>
		System,

		/// <summary>
		/// User role, typically used for user input or questions.
		/// </summary>
		User,

		/// <summary>
		/// Assistant role, typically used for responses or actions.
		/// </summary>
		Assistant,

		/// <summary>
		/// Tool role, typically used for responses from tool calls.
		/// </summary>
		Tool
	}

	/// <summary>
	/// Represents a single message in a conversation with LLM.
	/// </summary>
	public class Message
	{
		/// <summary>
		/// Gets or sets the role of the message sender. Typically "user" or "assistant".
		/// </summary>
		public Role Role { get; }

		/// <summary>
		/// Gets or sets the content of the message.
		/// </summary>
		public string Content { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Message"/> class.
		/// </summary>
		public Message()
		{
			Role = Role.System;
			Content = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Message"/> class.
		/// </summary>
		/// <param name="role">The role of the message sender.</param>
		/// <param name="content">The content of the message.</param>
		public Message(Role role, string content)
		{
			Role = role;
			Content = content;
		}
	}
}