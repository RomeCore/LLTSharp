using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LLTSharp.Metadata
{
	/// <summary>
	/// The class that contains various extensions for metadata.
	/// </summary>
	public static partial class MetadataExtensions
	{
		/// <summary>
		/// Converts <see cref="IEnumerable{T}"/> to <see cref="MetadataCollection"/>.
		/// </summary>
		public static MetadataCollection ToMetadataCollection(this IEnumerable<IMetadata> enumerable)
		{
			return new MetadataCollection(enumerable);
		}

		/// <summary>
		/// Converts <see cref="IEnumerable{T}"/> to <see cref="MetadataCollection"/>.
		/// </summary>
		public static MetadataCollection ToMetadataCollection<T>(this IEnumerable<T> enumerable)
			where T : IMetadata
		{
			return new MetadataCollection(enumerable.Cast<IMetadata>());
		}

		public static T? TryGetAdditional<T>(this IMetadataCollection collection, string key)
		{
			foreach (var metadata in collection.GetAll<IAdditionalMetadata>())
				if (metadata.Key == key)
					return (T)metadata.Value;

			return default;
		}
	}
}