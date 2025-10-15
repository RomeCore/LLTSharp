using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.AI;

namespace LLTSharp.TemplateNodes
{
	/// <summary>
	/// Represents a sequential node in a messages template, which contains a list of child nodes to be executed sequentially.
	/// </summary>
	public class MessagesTemplateSequentialNode : MessagesTemplateNode
	{
		/// <summary>
		/// The children nodes of this sequential node.
		/// </summary>
		public IReadOnlyList<MessagesTemplateNode> Children { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MessagesTemplateSequentialNode"/> class.
		/// </summary>
		/// <param name="children">The child nodes to be executed sequentially.</param>
		/// <exception cref="ArgumentNullException">Thrown when the <paramref name="children"/> parameter is null.</exception>
		public MessagesTemplateSequentialNode(IEnumerable<MessagesTemplateNode> children)
		{
			Children = children?.ToArray() ?? throw new ArgumentNullException(nameof(children));
		}

		public override IEnumerable<ChatMessage> Render(TemplateContextAccessor context)
		{
			List<ChatMessage> messages = new List<ChatMessage>();
			foreach (var child in Children)
			{
				messages.AddRange(child.Render(context));
			}
			return messages;
		}

		public override void Refine(int depth)
		{
			foreach (var child in Children)
				child.Refine(depth + 1);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			foreach (var child in Children)
				sb.Append(child);

			return sb.ToString();
		}
	}
}