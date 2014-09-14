using System;
using System.Linq;
using System.Collections.Generic;

namespace QuineMcCluskeyAlgorithm
{
	public static class MyEnumerable
	{
		public static IEnumerable<T> ToEnumerable<T>(this T item)
		{
			yield return item;
		}

		public static IEnumerable<T> Add<T>(this IEnumerable<T> src, T one)
		{
			return src.Concat (one.ToEnumerable());
		}
	}
}

