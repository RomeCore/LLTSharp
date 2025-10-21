using System;
using System.Text;

namespace LLTSharp.TemplateNodes
{
	/// <summary>
	/// Represents a node in the text template that executes a child node while a condition is true.
	/// </summary>
	public class TextTemplateWhileNode : TextTemplateNode
	{
		/// <summary>
		/// Gets the condition expression controlling the loop execution.
		/// </summary>
		public TemplateExpressionNode Condition { get; }

		/// <summary>
		/// Gets the child node that will be executed each iteration.
		/// </summary>
		public TextTemplateNode Child { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="TextTemplateWhileNode"/> class.
		/// </summary>
		public TextTemplateWhileNode(TemplateExpressionNode condition, TextTemplateNode child)
		{
			Condition = condition ?? throw new ArgumentNullException(nameof(condition));
			Child = child ?? throw new ArgumentNullException(nameof(child));
		}

		public override string Render(TemplateContextAccessor context)
		{
			StringBuilder result = new StringBuilder();

			context.PushFrame();
			try
			{
				while (EvaluateCondition(context))
				{
					var childResult = Child.Render(context);
					if (!string.IsNullOrEmpty(childResult))
						result.AppendLine(childResult);
				}
			}
			finally
			{
				context.PopFrame();
			}

			if (result.Length > 0)
				result.Length -= Environment.NewLine.Length; // Remove trailing newline

			return result.ToString();
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
			return $"@while {Condition} {{\n{Child}\n}}";
		}
	}

}