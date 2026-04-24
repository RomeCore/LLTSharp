using System;
using System.Collections.Generic;
using System.Linq;

namespace LLTSharp.Locale
{
	/// <summary>
	/// Provides a hierarchical language fallback scheme that walks up the language tag hierarchy (e.g., en-US → en)
	/// and then tries any available sibling variant of the same language root before falling back to a default or the first available language.
	/// </summary>
	public class HierarchicalLanguageFallbackScheme : ILanguageFallbackScheme
	{
		// Да, да, это нейрослоп

		private readonly LanguageCode? _defaultLanguage;

		/// <summary>
		/// Creates a new instance of the scheme.
		/// </summary>
		/// <param name="defaultLanguage">
		/// Optional default language code to be used if the target language and none of its linguistic relatives are available.
		/// If <see langword="null"/>, the first available language from the collection will be returned as a last resort.
		/// </param>
		public HierarchicalLanguageFallbackScheme(LanguageCode? defaultLanguage = null)
		{
			_defaultLanguage = defaultLanguage;
		}

		public LanguageCode GetFallbackLanguage(LanguageCode targetLanguage, IEnumerable<LanguageCode> availableLanguages)
		{
			if (availableLanguages == null)
				throw new ArgumentNullException(nameof(availableLanguages));

			// Materialise to a list to avoid multiple enumeration.
			List<LanguageCode> availableList = availableLanguages as List<LanguageCode> ?? availableLanguages.ToList();

			if (availableList.Count == 0)
				throw new ArgumentException("Available languages collection is empty.", nameof(availableLanguages));

			// 1. Exact match is already the best choice.
			if (availableList.Any(l => l == targetLanguage))
				return targetLanguage;

			// 2. Walk up the parent chain: e.g., zh-Hans-CN → zh-Hans → zh
			LanguageCode current = targetLanguage;
			while (true)
			{
				LanguageCode parent = current.GetSuperLanguage();
				if (parent == current)
					break;

				if (availableList.Any(l => l == parent))
					return parent;

				current = parent;
			}

			// 3. Look for any sibling belonging to the same root language (e.g., fr-FR when fr-CA was requested)
			var sibling = availableList.FirstOrDefault(l => l.IsSubLanguageOf(current));
			if (sibling.FullCode != null)
				return sibling;

			// 4. Fall back to the explicitly configured default language (if available).
			if (_defaultLanguage.HasValue && availableList.Any(l => l == _defaultLanguage.Value))
				return _defaultLanguage.Value;

			// 5. Ultimate fallback – return the first language from the available set.
			return availableList[0];
		}
	}
}