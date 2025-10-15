using System;
using System.Collections.Generic;
using System.Linq;

namespace LLTSharp.ExpressionNodes
{
	/// <summary>
	/// Represents a method call expression node in a template.
	/// </summary>
	public class TemplateMethodCallExpressionNode : TemplateExpressionNode
	{
		/// <summary>
		/// The child expression node that is passed as caller to the method.
		/// </summary>
		public TemplateExpressionNode Child { get; }

		/// <summary>
		/// The name of the method to be called.
		/// </summary>
		public string MethodName { get; }

		/// <summary>
		/// The arguments to the method call expression node.
		/// </summary>
		public IReadOnlyList<TemplateExpressionNode> Arguments { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="TemplateMethodCallExpressionNode"/> class.
		/// </summary>
		/// <param name="child">The child expression node that is passed as caller to the method.</param>
		/// <param name="methodName">The name of the method to be called.</param>
		/// <param name="arguments">The arguments to the method call expression node.</param>
		/// <exception cref="ArgumentNullException">Thrown when the <paramref name="child"/> parameter is null.</exception>
		/// <exception cref="ArgumentException">Thrown when the <paramref name="methodName"/> parameter is null or empty.</exception>
		public TemplateMethodCallExpressionNode(TemplateExpressionNode child, string methodName, IEnumerable<TemplateExpressionNode> arguments)
		{
			Child = child ?? throw new ArgumentNullException(nameof(child));
			MethodName = string.IsNullOrEmpty(methodName) ? throw new ArgumentException("Method name cannot be null or empty.", nameof(methodName)) : methodName;
			Arguments = arguments.ToArray();
		}

		public override TemplateDataAccessor Evaluate(TemplateContextAccessor context)
		{
			var child = Child.Evaluate(context);
			return child.Call(MethodName, Arguments.Select(arg => arg.Evaluate(context)).ToArray());
		}

		public override string ToString()
		{
			return $"{Child}.{MethodName}({string.Join(", ", Arguments.Select(arg => arg.ToString()))})";
		}
	}
}