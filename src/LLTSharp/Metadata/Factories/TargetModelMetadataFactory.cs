using LLTSharp.Metadata.Types;

namespace LLTSharp.Metadata.Factories
{
	public class TargetModelMetadataFactory : MetadataFactory
	{
		public override bool TryCreateMetadata(string key, TemplateDataAccessor value, out IMetadata metadata)
		{
			if (key == "model")
			{
				metadata = new TargetModelMetadata(value.ToString());
				return true;
			}
			metadata = null;
			return false;
		}
	}
}