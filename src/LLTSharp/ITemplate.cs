using System.Collections.Generic;
using LLTSharp.Metadata;

namespace LLTSharp
{
	/// <summary>
	/// Interface for a template that can be used to generate contents.
	/// </summary>
	public interface ITemplate : IMetadataProvider
	{
		/// <summary>
		/// Renders the template with the given context.
		/// </summary>
		/// <param name="context">The context to use for rendering the template. Can be null.</param>
		/// <returns>The result of rendering the template.</returns>
		object Render(object? context = null);
	}
	
	/// <summary>
	/// Interface for a template that can be used to generate content.
	/// </summary>
	/// <typeparam name="TResult">The type of result produced by the template.</typeparam>
	public interface ITemplate<TResult> : ITemplate
	{
		/// <inheritdoc cref="ITemplate.Render(object?)"/>
		new TResult Render(object? context = null);
	}

	/// <summary>
	/// Represents a text template that produces a string result.
	/// </summary>
	public interface ITextTemplate : ITemplate<string>
	{
	}

	/// <summary>
	/// Represents a messages template that produces a collection of messages.
	/// </summary>
	public interface IMessagesTemplate : ITemplate<IEnumerable<Message>>
	{
	}
}