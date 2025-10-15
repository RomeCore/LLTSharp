﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LLTSharp
{
	// The expression:
	// (a.b.c * d[0]) / e.g
	// Will be parsed into:
	// Bop(Bop(Path(this, a, b, c), Path(this, d, [0]), '*'), Path(this, e, g), '/')

	/// <summary>
	/// Represents an expression node in the template data structure. This is an abstract base class.
	/// </summary>
	public abstract class TemplateExpressionNode
	{
		/// <summary>
		/// Evaluates the template data node with the given data.
		/// </summary>
		/// <param name="context">The context data to evaluate the expression against.</param>
		/// <returns>The result of evaluating the template data node.</returns>
		public abstract TemplateDataAccessor Evaluate(TemplateContextAccessor context);
	}
}