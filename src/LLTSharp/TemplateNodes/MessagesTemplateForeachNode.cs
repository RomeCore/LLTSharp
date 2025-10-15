﻿using System;
using System.Collections.Generic;
using System.Text;
using LLTSharp.DataAccessors;
using Microsoft.Extensions.AI;

namespace LLTSharp.TemplateNodes
{
	/// <summary>
	/// Represents a node in the messages template that iterates over a collection of items.
	/// </summary>
	public class MessagesTemplateForeachNode : MessagesTemplateNode
	{
		/// <summary>
		/// Gets the source expression that provides the data for iteration.
		/// </summary>
		public TemplateExpressionNode Source { get; }

		/// <summary>
		/// Gets the child node that will be executed for each item in the collection.
		/// </summary>
		public MessagesTemplateNode Child { get; }

		/// <summary>
		/// Gets the name of the variable that represents each item in the iteration.
		/// </summary>
		public string IterableName { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="MessagesTemplateForeachNode"/> class.
		/// </summary>
		/// <param name="source">The expression that provides the data for iteration.</param>
		/// <param name="child">The node that will be executed for each item in the colletion.</param>
		/// <param name="iterableName">The name of the variable that represents each item in the iteration.</param>
		/// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown when the <paramref name="iterableName"/> is null or empty.</exception>
		public MessagesTemplateForeachNode(TemplateExpressionNode source, MessagesTemplateNode child, string iterableName)
		{
			Source = source ?? throw new ArgumentNullException(nameof(source));
			Child = child ?? throw new ArgumentNullException(nameof(child));
			IterableName = string.IsNullOrEmpty(iterableName) ? throw new ArgumentException("The iterable name cannot be null or empty.", nameof(iterableName)) : iterableName;
		}

		public override IEnumerable<ChatMessage> Render(TemplateContextAccessor context)
		{
			var source = Source.Evaluate(context);

			if (source is not IEnumerableTemplateDataAccessor enumerableSource)
				throw new TemplateRuntimeException($"The source expression does not provide an enumerable data source.",
					dataAccessor: context, expressionNode: Source);

			List<ChatMessage> messages = new List<ChatMessage>();

			context.PushFrame();
			foreach (var item in enumerableSource)
			{
				context.SetVariable(IterableName, item);

				var childResult = Child.Render(context);
				messages.AddRange(childResult);
			}
			context.PopFrame();

			return messages;
		}

		public override void Refine(int depth)
		{
			Child.Refine(depth + 1);
		}

		public override string ToString()
		{
			return $"@messages foreach {IterableName} in {Source} {{\n{Child}\n}}";
		}
	}
}