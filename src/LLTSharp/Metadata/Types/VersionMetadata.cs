using System;
using System.Collections.Generic;
using System.Text;

namespace LLTSharp.Metadata.Types
{
	/// <summary>
	/// Represents the version metadata.
	/// </summary>
	public class VersionMetadata : IMetadata
	{
		/// <summary>
		/// Gets the version code associated with this metadata.
		/// </summary>
		public Version Version { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="VersionMetadata"/> class with the specified version code.
		/// </summary>
		/// <param name="version">The version code associated with this metadata.</param>
		public VersionMetadata(int version)
		{
			Version = new Version(version, 0);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VersionMetadata"/> class with the specified version code.
		/// </summary>
		/// <param name="version">The version code associated with this metadata.</param>
		public VersionMetadata(Version version)
		{
			Version = version ?? throw new ArgumentNullException(nameof(version));
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