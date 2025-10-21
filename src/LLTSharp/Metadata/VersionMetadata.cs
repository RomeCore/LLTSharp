using System;
using System.Collections.Generic;
using System.Text;

namespace LLTSharp.Metadata
{
	/// <summary>
	/// Represents the version metadata.
	/// </summary>
	public class VersionMetadata : IMetadata
	{
		/// <summary>
		/// Gets the version code associated with this metadata.
		/// </summary>
		public int Version { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="VersionMetadata"/> class with the specified version code.
		/// </summary>
		/// <param name="version">The version code associated with this metadata.</param>
		public VersionMetadata(int version)
		{
			Version = version;
		}

		public override string ToString()
		{
			return $"Version: {Version}";
		}

		public override bool Equals(object? obj)
		{
			return obj is VersionMetadata other && Version == other.Version;
		}

		public override int GetHashCode()
		{
			int hash = 17;
			hash *= 397 + Version.GetHashCode();
			return hash;
		}
	}
}