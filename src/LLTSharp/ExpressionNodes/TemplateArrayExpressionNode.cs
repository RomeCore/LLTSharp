using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LLTSharp.DataAccessors;

namespace LLTSharp.ExpressionNodes
{
	/// <summary>
	/// Represents an array creation expression node: <c>[expr1, expr2, ...]</c>.
	/// Evaluates each element as an expression and returns a <see cref="TemplateArrayAccessor"/>.
	/// </summary>
	public class TemplateArrayExpressionNode : TemplateExpressionNode
	{
		private readonly TemplateExpressionNode[] _items;

		/// <summary>
		/// Gets the items of the array expression.
		/// </summary>
		public IReadOnlyList<TemplateExpressionNode> Items { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateArrayExpressionNode"/> class.
		/// </summary>
		/// <param name="items">The expressions for each element in the array.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
		public TemplateArrayExpressionNode(IEnumerable<TemplateExpressionNode> items)
		{
			_items = items?.ToArray() ?? throw new ArgumentNullException(nameof(items));
			Items = new ReadOnlyCollection<TemplateExpressionNode>(_items);
		}

		public override TemplateDataAccessor Evaluate(TemplateContextAccessor context)
		{
			var evaluatedItems = new TemplateDataAccessor[_items.Length];
			for (int i = 0; i < _items.Length; i++)
			{
				var item = _items[i].Evaluate(context);
				if (item == null)
					throw new TemplateRuntimeException($"Array element at index {i} evaluated to null.", dataAccessor: null);
				evaluatedItems[i] = item;
			}
			return new TemplateArrayAccessor(evaluatedItems);
		}

		public override string ToString()
		{
			return "[" + string.Join(", ", _items.Select(i => i.ToString())) + "]";
		}
	}
}
