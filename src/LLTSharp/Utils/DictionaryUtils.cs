using System;
using System.Collections.Generic;
using System.Text;

namespace LLTSharp.Utils
{
	public static class DictionaryUtils
	{
		public static bool DictionariesEqual<TKey, TValue>(
			IDictionary<TKey, TValue> left,
			IDictionary<TKey, TValue> right)
		{
			if (ReferenceEquals(left, right))
				return true;

			if (left == null || right == null)
				return false;

			if (left.Count != right.Count)
				return false;

			foreach (var kvp in left)
			{
				if (!right.TryGetValue(kvp.Key, out var value))
					return false;

				if (!Equals(kvp.Value, value))
					return false;
			}

			return true;
		}

		public static int GetDictionaryHashCode<TKey, TValue>(
			IDictionary<TKey, TValue> dictionary)
		{
			if (dictionary == null)
				return 0;

			int hash = 0;

			foreach (var kvp in dictionary)
			{
				int keyHash = EqualityComparer<TKey>.Default.GetHashCode(kvp.Key);
				int valueHash = kvp.Value != null
					? EqualityComparer<TValue>.Default.GetHashCode(kvp.Value)
					: 0;

				unchecked
				{
					hash ^= keyHash * 397 + valueHash;
				}
			}

			return hash;
		}

	}
}