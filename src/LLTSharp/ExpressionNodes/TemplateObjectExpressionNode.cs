using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LLTSharp.DataAccessors;

namespace LLTSharp.ExpressionNodes
{
	/// <summary>
	/// Represents an object/dictionary creation expression node: <c>{ key: expr, ... }</c>.
	/// Evaluates each value as an expression and returns a <see cref="TemplateDictionaryAccessor"/>.
	/// </summary>
	public class TemplateObjectExpressionNode : TemplateExpressionNode
	{
		private readonly KeyValuePair<TemplateExpressionNode, TemplateExpressionNode>[] _pairs;

		/// <summary>
		/// Gets the key-expression pairs of the object expression.
		/// </summary>
		public IReadOnlyList<KeyValuePair<TemplateExpressionNode, TemplateExpressionNode>> Pairs { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateObjectExpressionNode"/> class.
		/// </summary>
		/// <param name="pairs">The key-expression pairs for the object.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="pairs"/> is null.</exception>
		public TemplateObjectExpressionNode(IEnumerable<KeyValuePair<TemplateExpressionNode, TemplateExpressionNode>> pairs)
		{
			_pairs = pairs?.ToArray() ?? throw new ArgumentNullException(nameof(pairs));
			Pairs = new ReadOnlyCollection<KeyValuePair<TemplateExpressionNode, TemplateExpressionNode>>(_pairs);
		}

		public override TemplateDataAccessor Evaluate(TemplateContextAccessor context)
		{
			var dict = new Dictionary<string, TemplateDataAccessor>(_pairs.Length);
			foreach (var pair in _pairs)
			{
				var key = pair.Key.Evaluate(context).ToString();
				var value = pair.Value.Evaluate(context);
				dict[key] = value;
			}
			return new TemplateDictionaryAccessor(dict);
		}

		public override string ToString()
		{
			return "{" + string.Join(", ", _pairs.Select(p => $"{p.Key}: {p.Value}")) + "}";
		}
	}
}
