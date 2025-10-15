using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LLTSharp.ExpressionNodes
{
	/// <summary>
	/// Represents a function call expression node in a template.
	/// </summary>
	public class TemplateFunctionCallExpressionNode : TemplateExpressionNode
	{
		/// <summary>
		/// Gets the function associated with this function call expression node.
		/// </summary>
		public TemplateFunction Function { get; }

		/// <summary>
		/// Gets the arguments passed to the function associated with this function call expression node.
		/// </summary>
		public IReadOnlyList<TemplateExpressionNode> Arguments { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateFunctionCallExpressionNode"/> class.
		/// </summary>
		/// <param name="function">The function associated with this function call expression node.</param>
		/// <param name="arguments">The arguments passed to the function associated with this function call expression node.</param>
		public TemplateFunctionCallExpressionNode(TemplateFunction function, IEnumerable<TemplateExpressionNode> arguments)
		{
			Function = function;
			Arguments = arguments.ToArray();
		}

		public override TemplateDataAccessor Evaluate(TemplateContextAccessor context)
		{
			return Function.Call(Arguments.Select(arg => arg.Evaluate(context)).ToArray());
		}

		public override string ToString()
		{
			return $"{Function.Name}({string.Join(", ", Arguments.Select(arg => arg.ToString()))})";
		}
	}
}