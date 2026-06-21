using System;
using System.Collections.Generic;
using System.Text;
using LLTSharp.DataAccessors;

namespace LLTSharp.ExpressionNodes
{
	/// <summary>
	/// Represents a property access expression node in a template.
	/// </summary>
	public class TemplatePropertyExpressionNode : TemplateExpressionNode
	{
		/// <summary>
		/// Gets the child expression node that has the property to access.
		/// </summary>
		public TemplateExpressionNode Child { get; }

		/// <summary>
		/// Gets the name of the property to access.
		/// </summary>
		public string PropertyName { get; }

		/// <summary>
		/// Whether to return <see cref="TemplateNullAccessor"/> instead of exception if property does not exist.
		/// </summary>
		public bool SafeMode { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="TemplatePropertyExpressionNode"/> class.
		/// </summary>
		/// <param name="child">The child expression node that has the property to access.</param>
		/// <param name="propertyName">The name of the property to access.</param>
		/// <param name="safe">Whether to return <see cref="TemplateNullAccessor"/> instead of exception if property does not exist.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public TemplatePropertyExpressionNode(TemplateExpressionNode child, string propertyName, bool safe)
		{
			Child = child ?? throw new ArgumentNullException(nameof(child));
			PropertyName = string.IsNullOrEmpty(propertyName) ? throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName)) : propertyName;
			SafeMode = safe;
		}

		public override TemplateDataAccessor Evaluate(TemplateContextAccessor context)
		{
			var child = Child.Evaluate(context);
			return child.Property(PropertyName, SafeMode);
		}

		public override string ToString()
		{
			return $"{Child}.{PropertyName}";
		}
	}
}