﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LLTSharp
{
	/// <summary>
	/// Represents a node in the prompt template hierarchy.
	/// </summary>
	public abstract class TextTemplateNode
	{
		/// <summary>
		/// Gets the value indicating whether this node can be rendered.
		/// </summary>
		public virtual bool Renderable => true;

		/// <summary>
		/// Renders the prompt template node using the provided data accessor.
		/// </summary>
		/// <param name="context">The context accessor to use for rendering.</param>
		/// <returns>The rendered prompt template as a string.</returns>
		public abstract string Render(TemplateContextAccessor context);

		/// <summary>
		/// Refines the template after parsing an AST to remove indents and unnecessary leading/trailing whitespaces.
		/// </summary>
		/// <param name="depth">The current depth of refinement. Used for indentation purposes.</param>
		public virtual void Refine(int depth)
		{
		}
	}
}