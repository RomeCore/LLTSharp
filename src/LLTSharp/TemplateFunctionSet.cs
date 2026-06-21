using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LLTSharp.DataAccessors;

namespace LLTSharp
{
	/// <summary>
	/// A set of functions that can be called inside a template.
	/// </summary>
	public class TemplateFunctionSet : IEnumerable<TemplateFunction>
	{
		private readonly Dictionary<string, TemplateFunction> _functions;

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateFunctionSet"/> class.
		/// </summary>
		/// <param name="functions">A collection of template functions. Functions must have unique not-<see langword="null"/> names.</param>
		/// <exception cref="ArgumentNullException">Thrown when the <paramref name="functions"/> parameter is null.</exception>
		public TemplateFunctionSet(IEnumerable<TemplateFunction> functions)
		{
			_functions = functions?.GroupBy(f => f.Name).Select(f => f.Last())
				.ToDictionary(k => k.Name, v => v) ?? throw new ArgumentNullException(nameof(functions));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateFunctionSet"/> class.
		/// </summary>
		/// <param name="includeDefault">If set to true, include default functions specified in <see cref="TemplateFunctions.All"/>.</param>
		/// <param name="functions">A collection of template functions. Functions must have unique not-<see langword="null"/> names.</param>
		/// <exception cref="ArgumentNullException">Thrown when the <paramref name="functions"/> parameter is null.</exception>
		public TemplateFunctionSet(bool includeDefault, IEnumerable<TemplateFunction> functions)
		{
			if (includeDefault && functions != null)
				functions = TemplateFunctions.All.Concat(functions);
			_functions = functions?.GroupBy(f => f.Name).Select(f => f.Last())
				.ToDictionary(k => k.Name, v => v) ?? throw new ArgumentNullException(nameof(functions));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateFunctionSet"/> class.
		/// </summary>
		/// <param name="functions">A collection of template functions. Functions must have unique not-<see langword="null"/> names.</param>
		/// <exception cref="ArgumentNullException">Thrown when the <paramref name="functions"/> parameter is null.</exception>
		public TemplateFunctionSet(params TemplateFunction[] functions)
		{
			_functions = functions?.GroupBy(f => f.Name).Select(f => f.Last())
				.ToDictionary(k => k.Name, v => v) ?? throw new ArgumentNullException(nameof(functions));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateFunctionSet"/> class.
		/// </summary>
		/// <param name="includeDefault">If set to true, include default functions specified in <see cref="TemplateFunctions.All"/>.</param>
		/// <param name="functions">A collection of template functions. Functions must have unique not-<see langword="null"/> names.</param>
		/// <exception cref="ArgumentNullException">Thrown when the <paramref name="functions"/> parameter is null.</exception>
		public TemplateFunctionSet(bool includeDefault, params TemplateFunction[] functions)
		{
			var __functions = functions ?? Enumerable.Empty<TemplateFunction>();
			if (includeDefault && functions != null)
				__functions = TemplateFunctions.All.Concat(functions);
			_functions = __functions?.GroupBy(f => f.Name).Select(f => f.Last())
				.ToDictionary(k => k.Name, v => v) ?? throw new ArgumentNullException(nameof(functions));
		}

		/// <summary>
		/// Determines whether a function with the specified name exists in this set.
		/// </summary>
		/// <param name="functionName">The name of the function to check.</param>
		/// <returns><see langword="true"/> if a function with the specified name exists in this set; otherwise, <see langword="false"/>.</returns>
		public bool Exists(string functionName)
		{
			return _functions.ContainsKey(functionName);
		}

		/// <summary>
		/// Gets the function with the specified name.
		/// </summary>
		/// <param name="functionName">The name of the function.</param>
		/// <returns>The function with the specified name, or <see langword="null"/> if no such function exists.</returns>
		public TemplateFunction? TryGetFunction(string functionName)
		{
			if (_functions.TryGetValue(functionName, out var function))
				return function;
			return null;
		}

		/// <summary>
		/// Gets the function with the specified name. Throws an exception if no such function exists.
		/// </summary>
		/// <param name="functionName">The name of the function.</param>
		/// <returns>The function with the specified name.</returns>
		public TemplateFunction GetFunction(string functionName)
		{
			if (_functions.TryGetValue(functionName, out var function))
				return function;
			throw new KeyNotFoundException($"No function named '{functionName}' found in the set.");
		}

		/// <summary>
		/// Executes a function with the specified name and arguments.
		/// </summary>
		/// <param name="functionName">The name of the function.</param>
		/// <param name="args">The arguments to pass to the function.</param>
		/// <returns>The result of executing the function.</returns>
		public TemplateDataAccessor CallFunction(string functionName, TemplateDataAccessor[] args)
		{
			var function = GetFunction(functionName);
			return function.Call(null, args);
		}

		/// <summary>
		/// Executes a function with the specified name, context and arguments.
		/// </summary>
		/// <param name="functionName">The name of the function.</param>
		/// <param name="self">The data accessor representing the current context.</param>
		/// <param name="args">The arguments to pass to the function.</param>
		/// <returns>The result of executing the function.</returns>
		public TemplateDataAccessor CallFunction(string functionName, TemplateDataAccessor? self, TemplateDataAccessor[] args)
		{
			var function = GetFunction(functionName);
			return function?.Call(self, args) ?? TemplateNullAccessor.Instance;
		}

		public IEnumerator<TemplateFunction> GetEnumerator()
		{
			return _functions.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Gets the default set of functions.
		/// </summary>
		public static TemplateFunctionSet Default { get; } = new TemplateFunctionSet(TemplateFunctions.All);
	}
}