using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using LLTSharp.DataAccessors;
using LLTSharp.ExpressionNodes;

namespace LLTSharp
{
	/// <summary>
	/// Abstract base class for accessing template data for rendering templates.
	/// </summary>
	public abstract class TemplateDataAccessor : IDisposable
	{
		/// <summary>
		/// Gets the type of the data. This can be used to determine how to handle the data during rendering.
		/// </summary>
		public virtual string Type => "unknown";

		/// <summary>
		/// Gets the length of the data if it is an array or a string.
		/// </summary>
		public virtual int Length => 0;

		/// <summary>
		/// Determines if a property exists in the data.
		/// </summary>
		/// <param name="name">The name of the property to check.</param>
		/// <returns>True if the property exists; otherwise, false.</returns>
		public virtual bool HasProperty(string name) => false;

		/// <summary>
		/// Gets the template data property associated with the specified property name.
		/// </summary>
		/// <param name="name">The property name to retrieve the template data for.</param>
		/// <param name="safe">Indicates whether to return a null accessor if the property does not exist.</param>
		/// <returns>The template data that is associated with the specified property name.</returns>
		public virtual TemplateDataAccessor Property(string name, bool safe) =>
			safe ? TemplateNullAccessor.Instance :
				throw new TemplateRuntimeException($"Cannot get value for key '{name}'", this);

		/// <summary>
		/// Gets the template data associated with the specified index.
		/// </summary>
		/// <param name="index">The index to retrieve the template data for.</param>
		/// <param name="safe">Indicates whether to return a null accessor if the index does not exist.</param>
		/// <returns>The template data that is at the specified index.</returns>
		public virtual TemplateDataAccessor Index(TemplateDataAccessor index, bool safe) =>
			safe ? TemplateNullAccessor.Instance :
				throw new TemplateRuntimeException($"Cannot get value for index '{index}'.", this);

		/// <summary>
		/// Calls a method or function on the current data.
		/// </summary>
		/// <param name="methodName">The name of the method to call.</param>
		/// <param name="safe">Indicates whether to return a null accessor if the method does not exist.</param>
		/// <param name="arguments">The arguments to pass to the method.</param>
		/// <returns>The result of the method call.</returns>
		public virtual TemplateDataAccessor Call(string methodName, bool safe, TemplateDataAccessor[] arguments)
		{
			if (safe)
				return TemplateNullAccessor.Instance;
			throw new TemplateRuntimeException(
				$"Method '{methodName}' is not supported on type '{GetType().Name}'",
				dataAccessor: this);
		}

		/// <summary>
		/// Gets the value of the template data with the applied unary operator.
		/// </summary>
		/// <param name="type">The unary operator to apply.</param>
		/// <returns>The value of the template data with the applied unary operator, or <see cref="TemplateNullAccessor.Instance"/> if no data is found.</returns>
		public virtual TemplateDataAccessor Operator(UnaryOperatorType type)
		{
			switch (type)
			{
				case UnaryOperatorType.Negate:
					throw new TemplateRuntimeException("Can't apply negate operator to non-numeric data", dataAccessor: this);

				case UnaryOperatorType.LogicalNot:
					return new TemplateBooleanAccessor(!AsBoolean());

				case UnaryOperatorType.LengthOf:
					return new TemplateNumberAccessor(Length);

				default:
					throw new TemplateRuntimeException("Invalid operator type.", dataAccessor: this);
			}
		}

		/// <summary>
		/// Gets the value of the template data with the applied binary operator.
		/// </summary>
		/// <param name="other">The other accessor to use for the operator.</param>
		/// <param name="type">The binary operator to apply.</param>
		/// <returns>The value of the template data with the applied binary operator, or <see cref="TemplateNullAccessor.Instance"/> if no data is found.</returns>
		public virtual TemplateDataAccessor Operator(TemplateDataAccessor other, BinaryOperatorType type)
		{
			switch (type)
			{
				case BinaryOperatorType.Add:
				case BinaryOperatorType.Subtract:
				case BinaryOperatorType.Multiply:
				case BinaryOperatorType.Divide:
				case BinaryOperatorType.Modulus:
				case BinaryOperatorType.LessThan:
				case BinaryOperatorType.LessThanOrEqual:
				case BinaryOperatorType.GreaterThan:
				case BinaryOperatorType.GreaterThanOrEqual:
					throw new TemplateRuntimeException($"Can't apply binary operator: '{type}' to non-numeric data", dataAccessor: this);

				case BinaryOperatorType.Equal:
					return new TemplateBooleanAccessor(Equals(GetValue(), other.GetValue()));

				case BinaryOperatorType.NotEqual:
					return new TemplateBooleanAccessor(!Equals(GetValue(), other.GetValue()));

				case BinaryOperatorType.LogicalAnd:
					return new TemplateBooleanAccessor(AsBoolean() && other.AsBoolean());

				case BinaryOperatorType.LogicalOr:
					return new TemplateBooleanAccessor(AsBoolean() || other.AsBoolean());

				case BinaryOperatorType.Coalesce:
					return this is TemplateNullAccessor ? other : this;

				default:
					throw new TemplateRuntimeException("Invalid operator type.", dataAccessor: this);
			}
		}

		/// <summary>
		/// Gets the value of the current context as a boolean value.
		/// </summary>
		/// <returns>The value of the current context as a boolean value, or <see langword="false"/> if no data is found.</returns>
		public abstract bool AsBoolean();

		/// <summary>
		/// Gets the value of the current context.
		/// </summary>
		/// <returns>The value of the current context.</returns>
		public abstract object GetValue();

		/// <summary>
		/// Gets the value of the current context converted to a specific type.
		/// </summary>
		/// <typeparam name="T">The type to convert the current context to.</typeparam>
		/// <returns>The value of the current context converted to the specified type. If no data is found, returns the default value for the type.</returns>
		/// <exception cref="TemplateRuntimeException">Thrown when value conversion is failed.</exception>
		public T GetValue<T>()
		{
			var value = GetValue();
			if (value is T res1)
				return res1;
			if (value is null)
				return default;

			try
			{
				return Convert.ChangeType(value, typeof(T)) is T res2 ? res2 : default;
			}
			catch (Exception ex)
			{
				throw new TemplateRuntimeException($"Cannot convert value '{value}' to type '{typeof(T)}'", ex, this);
			}
		}

		/// <summary>
		/// Converts the template data to a string representation.
		/// </summary>
		/// <param name="format">The format to use for converting the template data, or <see langword="null"/> to use the default format.</param>
		/// <returns>A string representing the template data.</returns>
		public abstract string ToString(string? format = null);
		public override string ToString()
		{
			return ToString(null);
		}

		private bool isDisposed;
		protected virtual void Dispose(bool disposing)
		{
		}

		/// <summary>
		/// Converts the template data to an expression node.
		/// </summary>
		/// <returns>An expression node representing the template data.</returns>
		public TemplateExpressionNode AsExpression()
		{
			return new TemplateDataAccessorExpressionNode(this);
		}

		~TemplateDataAccessor()
		{
			if (!isDisposed)
			{
				Dispose(disposing: false);
				isDisposed = true;
			}
		}

		public void Dispose()
		{
			if (!isDisposed)
			{
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Creates a new template data accessor based on the provided object.
		/// </summary>
		/// <param name="value">The object to create a template data accessor for.</param>
		/// <param name="options">The options to use when creating the template data accessor.</param>
		/// <returns>A new instance of <see cref="TemplateDataAccessor"/>.</returns>
		public static TemplateDataAccessor Create(object? value, DataAccessorCreationOptions options = DataAccessorCreationOptions.None)
		{
			return DataAccessorFactory.Create(value, options);
		}
	}
}