using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLTSharp.Metadata;

namespace LLTSharp
{
	/// <summary>
	/// Represents a text template for generating strings.
	/// </summary>
	public class TextTemplate : ITextTemplate
	{
		/// <summary>
		/// The main node of the text template.
		/// </summary>
		public TextTemplateNode MainNode { get; }

		public IMetadataCollection Metadata { get; }

		/// <summary>
		/// Gets the local library associated with this prompt template.
		/// </summary>
		public TemplateLibrary LocalLibrary { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TextTemplate"/> class.
		/// </summary>
		/// <param name="mainNode">The main node of the template.</param>
		/// <param name="metadata">The metadata associated with this template.</param>
		/// <param name="localLibrary">The local library associated with this prompt template.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public TextTemplate(TextTemplateNode mainNode, IMetadataCollection metadata, TemplateLibrary localLibrary)
		{
			MainNode = mainNode ?? throw new ArgumentNullException(nameof(mainNode));
			Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
			LocalLibrary = localLibrary ?? throw new ArgumentNullException(nameof(localLibrary));
		}

		public string Render(object? context = null, TemplateFunctionSet? functions = null)
		{
			var ctx = new TemplateContextAccessor(TemplateDataAccessor.Create(context), Metadata, functions: functions, library: LocalLibrary);
			return MainNode.Render(ctx);
		}

		object ITemplate.Render(object? context, TemplateFunctionSet? functions)
		{
			var ctx = new TemplateContextAccessor(TemplateDataAccessor.Create(context), Metadata, functions: functions, library: LocalLibrary);
			return MainNode.Render(ctx);
		}
	}
}