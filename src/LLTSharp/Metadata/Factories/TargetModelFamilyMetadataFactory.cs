using LLTSharp.Metadata.Types;

namespace LLTSharp.Metadata.Factories
{
	public class TargetModelFamilyMetadataFactory : MetadataFactory
	{
		public override bool TryCreateMetadata(string key, TemplateDataAccessor value, out IMetadata metadata)
		{
			if (key == "model_family")
			{
				metadata = new TargetModelFamilyMetadata(value.ToString());
				return true;
			}
			metadata = null;
			return false;
		}
	}
}