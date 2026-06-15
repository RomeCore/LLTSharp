using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace LLTSharp.Metadata
{
	/// <summary>
	/// Represents a custom metadata object.
	/// </summary>
	public interface IAdditionalMetadata : IMetadata
	{
		/// <summary>
		/// Gets the key associated with this metadata.
		/// </summary>
		string Key { get; }

		/// <summary>
		/// Gets the value associated with this metadata.
		/// </summary>
		object? Value { get; }
	}
}