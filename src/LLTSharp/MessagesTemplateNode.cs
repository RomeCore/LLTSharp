using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.AI;

namespace LLTSharp
{
	/// <summary>
	/// The base class for all messages template nodes used for rendering prompts based on templates.
	/// </summary>
	public abstract class MessagesTemplateNode
	{
		/// <summary>
		/// Renders the template node based on the provided data.
		/// </summary>
		/// <param name="context">The context accessor containing the data to render.</param>
		/// <returns>A collection of messages representing the rendered template node.</returns>
		public abstract IEnumerable<ChatMessage> Render(TemplateContextAccessor context);

		/// <summary>
		/// Refines the template after parsing an AST to remove indents and unnecessary leading/trailing whitespaces.
		/// </summary>
		/// <param name="depth">The current depth of refinement. Used for indentation purposes.</param>
		public virtual void Refine(int depth)
		{
		}
	}
}