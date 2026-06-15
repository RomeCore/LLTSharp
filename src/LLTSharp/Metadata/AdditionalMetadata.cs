using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using LLTSharp.Utils;

namespace LLTSharp.Metadata
{
	/// <summary>
	/// Represents a dynamic metadata object that can store additional properties at runtime.
	/// </summary>
	public class AdditionalMetadata : IAdditionalMetadata, IEquatable<AdditionalMetadata>, IEquatable<IAdditionalMetadata>
	{
		public string Key { get; }

		public object? Value { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AdditionalMetadata"/> class with initial key-value pair.
		/// </summary>
		/// <param name="key">The key of the initial metadata item.</param>
		/// <param name="value">The value of the initial metadata item.</param>
		public AdditionalMetadata(string key, object? value)
		{
			Key = key;
			Value = value;
		}

		public override bool Equals(object? obj)
		{
			return obj is AdditionalMetadata other && Equals(other);
		}

		public bool Equals(AdditionalMetadata? other)
		{
			return other != null && Key == other.Key && Value == other.Value;
		}

		public bool Equals(IAdditionalMetadata? other)
		{
			return other != null && Key == other.Key && Value == other.Value;
		}

		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 397 + Key.GetHashCode();
			hash = hash * 397 + (Value?.GetHashCode() ?? 0);
			return hash;
		}
	}
}