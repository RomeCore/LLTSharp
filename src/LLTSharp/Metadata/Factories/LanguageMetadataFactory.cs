using LLTSharp.Metadata.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace LLTSharp.Metadata.Factories
{
	public class LanguageMetadataFactory : MetadataFactory
	{
		public override bool TryCreateMetadata(string key, TemplateDataAccessor value, out IMetadata metadata)
		{
			if (key == "lang")
			{
				metadata = new LanguageMetadata(value.ToString());
				return true;
			}
			metadata = null;
			return false;
		}
	}
}