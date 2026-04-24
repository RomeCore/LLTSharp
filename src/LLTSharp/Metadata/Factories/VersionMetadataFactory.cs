using LLTSharp.Metadata.Types;
using System;

namespace LLTSharp.Metadata.Factories
{
	public class VersionMetadataFactory : MetadataFactory
	{
		public override bool TryCreateMetadata(string key, TemplateDataAccessor value, out IMetadata metadata)
		{
			if (key == "version")
			{
				metadata = new VersionMetadata(Version.Parse(value.ToString()));
				return true;
			}
			metadata = null;
			return false;
		}
	}
}