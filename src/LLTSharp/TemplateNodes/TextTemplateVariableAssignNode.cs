﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LLTSharp.TemplateNodes
{
	/// <summary>
	/// Represents a node in a text template that assigns a value to a variable.
	/// </summary>
	public class TextTemplateVariableAssignNode : TextTemplateNode
	{
		public override bool Renderable => false;

		/// <summary>
		/// The name of the variable to assign.
		/// </summary>
		public string VariableName { get; }

		/// <summary>
		/// The expression to evaluate and assign to the variable.
		/// </summary>
		public TemplateExpressionNode Expression { get; }

		/// <summary>
		/// Indicates whether this node assigns a value to an existing variable or creates a new one.
		/// </summary>
		public bool AssignsToExisting { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TextTemplateVariableAssignNode"/> class.
		/// </summary>
		/// <param name="variableName">The name of the variable to assign.</param>
		/// <param name="expression">The expression to evaluate and assign to the variable.</param>
		/// <param name="assignsToExisting">Indicates whether this node assigns a value to an existing variable or creates a new one.</param>
		public TextTemplateVariableAssignNode(string variableName, TemplateExpressionNode expression, bool assignsToExisting = false)
		{
			VariableName = variableName;
			Expression = expression;
			AssignsToExisting = assignsToExisting;
		}

		public override string Render(TemplateContextAccessor context)
		{
			if (AssignsToExisting)
				context.AssignVariable(VariableName, Expression.Evaluate(context));
			else
				context.SetVariable(VariableName, Expression.Evaluate(context));
			return string.Empty;
		}
	}
}