﻿using System;
using System.Collections.Generic;
using System.Text;
using LLTSharp.Metadata;

namespace LLTSharp.Metadata
{
	/// <summary>
	/// The metadata for template identifier-related information.
	/// </summary>
	public class TemplateIdentifierMetadata : IMetadata
	{
		/// <summary>
		/// Gets the identifier for this metadata.
		/// </summary>
		public string Identifier { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateIdentifierMetadata"/> class with the specified identifier.
		/// </summary>
		/// <param name="identifier">The identifier for this metadata.</param>
		public TemplateIdentifierMetadata(string identifier)
		{
			Identifier = identifier;
		}

		public override string ToString()
		{
			return $"ID: '{Identifier}'";
		}

		public override bool Equals(object? obj)
		{
			return obj is TemplateIdentifierMetadata other && Identifier == other.Identifier;
		}

		public override int GetHashCode()
		{
			int hash = 19;
			hash *= 397 + Identifier.GetHashCode();
			return hash;
		}
	}
}