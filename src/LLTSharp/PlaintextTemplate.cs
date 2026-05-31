using System;
using System.Collections.Generic;
using System.Text;
using LLTSharp.Metadata;

namespace LLTSharp
{
	public class PlaintextTemplate : ITextTemplate
	{
		/// <summary>
		/// The content of the template.
		/// </summary>
		public string Content { get; }

		public IMetadataCollection Metadata { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PlaintextTemplate"/> class.
		/// </summary>
		/// <param name="content">The content of the template. Cannot be <see langword="null"/>.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public PlaintextTemplate(string content)
		{
			Content = content ?? throw new ArgumentNullException(nameof(content));
			Metadata = new MetadataCollection();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PlaintextTemplate"/> class.
		/// </summary>
		/// <param name="content">The content of the template. Cannot be <see langword="null"/>.</param>
		/// <param name="metadata">The metadata associated with this template. Cannot be <see langword="null"/>.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public PlaintextTemplate(string content, IEnumerable<IMetadata> metadata)
		{
			Content = content ?? throw new ArgumentNullException(nameof(content));
			Metadata = new MetadataCollection(metadata) ?? throw new ArgumentNullException(nameof(metadata));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PlaintextTemplate"/> class.
		/// </summary>
		/// <param name="content">The content of the template. Cannot be <see langword="null"/>.</param>
		/// <param name="metadata">The metadata associated with this template. Cannot be <see langword="null"/>.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public PlaintextTemplate(string content, IMetadataCollection metadata)
		{
			Content = content ?? throw new ArgumentNullException(nameof(content));
			Metadata = new MetadataCollection(metadata) ?? throw new ArgumentNullException(nameof(metadata));
		}

		public string Render(object? context = null, TemplateFunctionSet? functions = null)
		{
			return Content;
		}

		object ITemplate.Render(object? context, TemplateFunctionSet? functions)
		{
			return Render(context, functions);
		}
	}
}