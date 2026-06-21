using System.Collections.Generic;
using System.Linq;
using LLTSharp.DataAccessors;

namespace LLTSharp
{
	/// <summary>
	/// Represents a collection of predefined template functions.
	/// </summary>
	public static class TemplateFunctions
	{
		/// <summary>
		/// Returns a collection of all predefined template functions.
		/// </summary>
		public static IEnumerable<TemplateFunction> All => new TemplateFunction[]
		{
			Type,
			Length,
			Strcat,
			Substr
		};

		public static TemplateFunction Type { get; } = new TemplateFunction("type",
			(self, args) => args[0].Type, canBeMethod: false);

		public static TemplateFunction Length { get; } = new TemplateFunction("length",
			(self, args) => args[0].Length, canBeMethod: false);

		public static TemplateFunction Strcat { get; } = new TemplateFunction("strcat",
			(self, args) => string.Join("", args.Select(a => a.GetValue().ToString())), canBeMethod: false);

		public static TemplateFunction Substr { get; } = new TemplateFunction("substr",
			(self, args) => args[0].ToString().Substring(args[1].GetValue<int>(), args[2].GetValue<int>()), canBeMethod: false);
	}

}