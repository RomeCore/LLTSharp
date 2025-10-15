using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.AI;

namespace LLTSharp.TemplateNodes
{
	/// <summary>
	/// Represents a conditional node in a messages template.
	/// </summary>
	public class MessagesTemplateIfElseNode : MessagesTemplateNode
	{
		/// <summary>
		/// The condition to evaluate.
		/// </summary>
		public TemplateExpressionNode Condition { get; }

		/// <summary>
		/// The node to execute if the condition is true.
		/// </summary>
		public MessagesTemplateNode IfBranch { get; }

		/// <summary>
		/// The node to execute if the condition is false.
		/// </summary>
		public MessagesTemplateNode? ElseBranch { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="MessagesTemplateIfElseNode"/> class.
		/// </summary>
		/// <param name="condition">The condition to evaluate.</param>
		/// <param name="ifBranch">The node to execute if the condition is true.</param>
		/// <param name="elseBranch">The node to execute if the condition is false. Can be null.</param>
		/// <exception cref="ArgumentNullException">Thrown when any of the parameters are null, except for <paramref name="elseBranch"/>.</exception>
		public MessagesTemplateIfElseNode(TemplateExpressionNode condition, MessagesTemplateNode ifBranch, MessagesTemplateNode? elseBranch)
		{
			Condition = condition ?? throw new ArgumentNullException(nameof(condition));
			IfBranch = ifBranch ?? throw new ArgumentNullException(nameof(ifBranch));
			ElseBranch = elseBranch;
		}

		public override IEnumerable<ChatMessage> Render(TemplateContextAccessor context)
		{
			IEnumerable<ChatMessage>? result = null;

			var conditionResult = Condition.Evaluate(context);

			context.PushFrame();

			if (conditionResult.AsBoolean())
				result = IfBranch.Render(context);
			else if (ElseBranch != null)
				result = ElseBranch.Render(context);

			context.PopFrame();

			return result ?? Enumerable.Empty<ChatMessage>();
		}

		public override void Refine(int depth)
		{
			IfBranch.Refine(depth + 1);

			if (ElseBranch == null)
				return;

			if (ElseBranch is MessagesTemplateIfElseNode)
				ElseBranch?.Refine(depth); // Same depth for nested if-else
			else
				ElseBranch?.Refine(depth + 1);
		}

		public override string ToString()
		{
			if (ElseBranch == null)
				return $"@messages if {Condition} \n {{\n{IfBranch}\n}} \n";
			return $"@messages if {Condition} \n {{\n{IfBranch}\n}} \n else \n {{\n{ElseBranch}\n}}";
		}
	}
}