using System;
using System.Collections.Generic;

namespace LLTSharp.TemplateNodes
{
	/// <summary>
	/// Represents a node in the messages template that executes a child node while a condition is true.
	/// </summary>
	public class MessagesTemplateWhileNode : MessagesTemplateNode
	{
		/// <summary>
		/// Gets the condition expression controlling the loop execution.
		/// </summary>
		public TemplateExpressionNode Condition { get; }

		/// <summary>
		/// Gets the child node that will be executed each iteration.
		/// </summary>
		public MessagesTemplateNode Child { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="MessagesTemplateWhileNode"/> class.
		/// </summary>
		public MessagesTemplateWhileNode(TemplateExpressionNode condition, MessagesTemplateNode child)
		{
			Condition = condition ?? throw new ArgumentNullException(nameof(condition));
			Child = child ?? throw new ArgumentNullException(nameof(child));
		}

		public override IEnumerable<Message> Render(TemplateContextAccessor context)
		{
			List<Message> messages = new List<Message>();

			context.PushFrame();
			try
			{
				while (EvaluateCondition(context))
				{
					var childResult = Child.Render(context);
					messages.AddRange(childResult);
				}
			}
			finally
			{
				context.PopFrame();
			}

			return messages;
		}

		private bool EvaluateCondition(TemplateContextAccessor context)
		{
			var value = Condition.Evaluate(context);
			return value.AsBoolean();
		}

		public override void Refine(int depth)
		{
			Child.Refine(depth + 1);
		}

		public override string ToString()
		{
			return $"@messages while {Condition} {{\n{Child}\n}}";
		}
	}

}