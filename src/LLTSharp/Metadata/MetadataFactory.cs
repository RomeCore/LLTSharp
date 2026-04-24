using System;
using System.Collections.Generic;
using System.Text;

namespace LLTSharp.Metadata
{
	public abstract class MetadataFactory
	{
		/// <summary>
		/// Tries to create a new instance of <see cref="IMetadata"/> based on the provided key and value.
		/// </summary>
		/// <param name="key">The key associated with the metadata.</param>
		/// <param name="value">The value associated with the metadata.</param>
		/// <param name="metadata">When this method returns, contains the newly created instance of <see cref="IMetadata"/> if successful; otherwise, null. This parameter is passed uninitialized.</param>
		/// <returns>true if a new instance of <see cref="IMetadata"/> was created successfully; otherwise, false.</returns>
		public abstract bool TryCreateMetadata(string key, TemplateDataAccessor value, out IMetadata metadata);
	}
}