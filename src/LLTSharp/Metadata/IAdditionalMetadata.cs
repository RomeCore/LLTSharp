using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace LLTSharp.Metadata
{
	/// <summary>
	/// Represents a dynamic metadata object that can store additional properties at runtime.
	/// </summary>
	public interface IAdditionalMetadata : IMetadata, IDynamicMetaObjectProvider
	{
		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <typeparam name="T">The type to convert the value to.</typeparam>
		/// <param name="key">The key of the value to get.</param>
		/// <returns>The converted value or default(T) if not found.</returns>
		T Get<T>(string key);

		/// <summary>
		/// Tries to get the value associated with the specified key.
		/// </summary>
		/// <typeparam name="T">The type to convert the value to.</typeparam>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">When this method returns, contains the value if found; otherwise, default(T).</param>
		/// <returns>true if the key was found; otherwise, false.</returns>
		bool TryGet<T>(string key, out T value);
	}
}