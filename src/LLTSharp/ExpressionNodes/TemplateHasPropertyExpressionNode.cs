using System;
using LLTSharp.DataAccessors;

namespace LLTSharp.ExpressionNodes
{
	/// <summary>
	/// Represents a property check expression node in a template.
	/// </summary>
	public class TemplateHasPropertyExpressionNode : TemplateExpressionNode
	{
		/// <summary>
		/// Gets the child expression node that has the property to check.
		/// </summary>
		public TemplateExpressionNode Child { get; }

		/// <summary>
		/// Gets the name of the property to check.
		/// </summary>
		public TemplateExpressionNode PropertyName { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="TemplateHasPropertyExpressionNode"/> class.
		/// </summary>
		/// <param name="child">The child expression node that has the property to check.</param>
		/// <param name="propertyName">The name of the property to check.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public TemplateHasPropertyExpressionNode(TemplateExpressionNode child, TemplateExpressionNode propertyName)
		{
			Child = child ?? throw new ArgumentNullException(nameof(child));
			PropertyName = propertyName ?? throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));
		}

		public override TemplateDataAccessor Evaluate(TemplateContextAccessor context)
		{
			var child = Child.Evaluate(context);
			var propertyName = PropertyName.Evaluate(context).ToString();
			return new TemplateBooleanAccessor(child.HasProperty(propertyName));
		}

		public override string ToString()
		{
			return $"{Child} ?: {PropertyName}";
		}
	}
}