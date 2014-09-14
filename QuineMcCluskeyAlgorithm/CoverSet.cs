using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace QuineMcCluskeyAlgorithm
{
	// 集合被覆アルゴリズム
	public static class CoverSet
	{
		// baseset ∪ (∪ S) = XとなるようなSを求める(複数返す)
		static public IEnumerable<IEnumerable<BitField>> Solve
			(IEnumerable<BitField> subsets, BitField baseset = null)
		{
			// 自明の場合
			if (subsets.Count() == 0) {
				return Enumerable.Empty<IEnumerable<BitField>>();
			}
			if (baseset == null) {
				baseset = new BitField (subsets.First().Length);
			}

			// １つしか選択肢がないものを抽出
			var subsets1 = subsets.ToList ();
			var need = new List<BitField> ();

			for (int i = 0; i < baseset.Length; i ++) {
				if (baseset [i])
					continue;
				BitField candidate = null;
				int candiCount = 0;
				foreach (BitField aSubset in subsets1) {
					if (aSubset [i]) {
						candiCount ++;
						if (candidate == null)
							candidate = aSubset;
						else
							break;
					}
				}
				// １つも無ければエラー
				if (candiCount < 1) {
					return Enumerable.Empty<IEnumerable<BitField>>();
				} else if (candiCount == 1) {
					subsets1.Remove (candidate);
					need.Add (candidate);
					baseset |= candidate;
				}
			}
			// 包含関係を除去
			var subsets2 = subsets1.Where
				(x => subsets1.All (y => ReferenceEquals(x, y) || !y.Contains(x)));

			// 0から総当り
			var first = Tuple.Create (Enumerable.Empty<BitField>(), baseset);
			var candidates = first.ToEnumerable ();
			while (candidates.SingleOrDefault () != null) {
				var ok = candidates.Where (x => x.Item2.IsMax).Select (x => x.Item1);
				if (ok.SingleOrDefault() != null) {
					return ok.Select (x => x.Concat(need));
				}
				candidates = candidates.SelectMany
					(src => subsets2
					 .Where(x => !src.Item2.Contains(x))
					 .Select(x => Tuple.Create(src.Item1.Add(x), src.Item2 | x)));
			}

			return Enumerable.Empty<IEnumerable<BitField>>();
		}
	}
}

