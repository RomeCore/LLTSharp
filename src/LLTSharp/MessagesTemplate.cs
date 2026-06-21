using System;
using System.Collections.Generic;
using System.Text;
using LLTSharp.Metadata;

namespace LLTSharp
{
	/// <summary>
	/// Represents a prompt template that produces a collection of messages.
	/// </summary>
	public class MessagesTemplate : IMessagesTemplate
	{
		/// <summary>
		/// The main node of the prompt template. This is where the actual messages are defined.
		/// </summary>
		public MessagesTemplateNode MainNode { get; }

		public IMetadataCollection Metadata { get; }

		/// <summary>
		/// Gets the local library associated with this prompt template.
		/// </summary>
		public TemplateLibrary LocalLibrary { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MessagesTemplate"/> class.
		/// </summary>
		/// <param name="mainNode">The main node of the template.</param>
		/// <param name="metadata">The metadata associated with this template.</param>
		/// <param name="localLibrary">The local library associated with this prompt template.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public MessagesTemplate(MessagesTemplateNode mainNode, IMetadataCollection metadata, TemplateLibrary localLibrary)
		{
			MainNode = mainNode ?? throw new ArgumentNullException(nameof(mainNode));
			Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
			LocalLibrary = localLibrary ?? throw new ArgumentNullException(nameof(localLibrary));
		}

		public IEnumerable<Message> Render(object? context = null, TemplateFunctionSet? functions = null)
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
