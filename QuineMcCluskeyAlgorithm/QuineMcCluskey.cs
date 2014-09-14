using System;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace QuineMcCluskeyAlgorithm
{
	/// <summary>
	/// TruthTableを与え、QM法で真理値表を圧縮するクラス。
	/// 途中状態は、各メソッド、プロパティで参照できる。
	/// </summary>
	public class QuineMcCluskey
	{
		// 元のテーブル
		public TruthTable Source { get; private set; }

		// 番号付け用。 BitFieldのn番目がこの配列のn番目に対応。
		private QMColumn[] trueCols;

		// 初期化
		public QuineMcCluskey (TruthTable table)
		{

			Source = table;
			var trues = table.Columns.Where (x => x.Result);
			var trueCount = trues.Count ();
			trueCols = trues.Select((x, i) => {
				var col = new QMColumn(x, trueCount);
				col.Flags[i] = true;
				return col;
			}).ToArray ();
			combined = new IEnumerable<IEnumerable<QMColumn>>[table.Columns[0].Size + 1];
		}

		// 1の数で分けたグループ
		private IEnumerable<QMColumn>[] bitCountGroup = null;
		public IEnumerable<QMColumn>[] BitCountGroup {
			get {
				if (bitCountGroup == null) {
				    var empty = Enumerable.Empty<QMColumn>();
					bitCountGroup = Enumerable.Repeat (empty, combined.Length).ToArray ();
					var groups = trueCols.GroupBy (
						col => col.Count (bit => bit.HasValue && bit.Value)
					);
					foreach (IGrouping<int, QMColumn> grp in groups) {
						bitCountGroup [grp.Key] = grp;
					}
				}
				return bitCountGroup;
			}
		}

		// 合成結果
		private IEnumerable<IEnumerable<QMColumn>>[] combined;
		public IEnumerable<IEnumerable<QMColumn>> GetCombined (int count)
		{
			if (count < combined.Length) {
				if (combined [count] == null) {
					if (count == 0) {
						combined [count] = BitCountGroup;
					} else {
						combined [count] = combineClolumns (GetCombined (count - 1));
					}
				}
				return combined [count];
			} else {
				return Enumerable.Empty<IEnumerable<QMColumn>> ();
			}
		}

		private IEnumerable<IEnumerable<QMColumn>> combineClolumns
			(IEnumerable<IEnumerable<QMColumn>> source)
		{
			return source.Skip(1).Zip (source, combineGroups);
		}

		private IEnumerable<QMColumn> combineGroups
			(IEnumerable<QMColumn> a, IEnumerable<QMColumn> b)
		{

			var combined =
				from col1 in a
				from col2 in b
				select QMColumn.TryCombine (col1, col2) into col
				where col != null
				select col;
			var result = new List<QMColumn> ();
			foreach (QMColumn col in combined) {
				var same = result.FirstOrDefault (x => x.Flags == col.Flags);
				if (same == null) {
					result.Add (col);
				} else {
					same.Flags |= col.Flags;
				}
			}
			return result;
		}

		// マークされていないリスト
		private List<QMColumn> unmarked;
		public IEnumerable<QMColumn> Unmarked
		{
			get {
				if (unmarked == null)
                {
    				BitField flags = new BitField (trueCols.Length);
    				unmarked = Enumerable.Range(0, combined.Length)
    					.Reverse ()
    					.SelectMany(i => GetCombined(i))
    					.SelectMany (x => x)
    					.Where (col => {
    						if (flags.Contains(col.Flags)) {
    							return false;
    						} else {
    							flags |= col.Flags;
    							return true;
    						}
    					}).ToList();
                }
				return unmarked;
			}
		}

		// 圧縮結果
		private List<List<QMColumn>> compressed;
		private Dictionary <BitField, QMColumn> flagToCols;

		public IEnumerable<IEnumerable<QMColumn>> Compressed
		{
			get {
				if (compressed == null)
                {
    				flagToCols = Unmarked.ToDictionary (x => x.Flags);
    				compressed = CoverSet.Solve (flagToCols.Keys)
    					.Select(x => x.Select (y => flagToCols[y]).ToList())
    					.ToList();
                }
				return compressed;
			}
		}

	}

	// QM法のためのフラグつきTruthTableColumn
	public class QMColumn : TruthTableColumn
	{
		public BitField Flags;
		public QMColumn (TruthTableColumn ttc, int colCount) : base (ttc)
		{
			Result = ttc.Result;
			Flags = new BitField (colCount);
		}
		public QMColumn (int size, int colCount) : base (size)
		{
			Flags = new BitField (colCount);
		}
		public static QMColumn TryCombine (QMColumn a, QMColumn b)
		{
			var c = new QMColumn (a.Size, a.Flags.Length);
			if (a.Result == b.Result) {
				c.Result = a.Result;
			} else {
				return null;
			}
			var already = false;
			for (int i = 0; i < a.Size; i++) {
				if (a [i] == b [i]) {
					c [i] = a [i];
				} else if (!already && a [i].HasValue && b [i].HasValue) {
					c [i] = null;
					already = true;
				} else {
					return null;
				}
			}
			c.Flags |= a.Flags;
			c.Flags |= b.Flags;
			return c;
		}
	}
}

