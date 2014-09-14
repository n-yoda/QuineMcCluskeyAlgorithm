using System;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace QuineMcCluskeyAlgorithm
{
	/// <summary>
	/// 真理値表を表すクラス。
	/// 論理式を与えるか、各行を与えることで初期化出来る。
	/// </summary>
	public class TruthTable
	{
		public const int MaxParameterCount = 16;
		public string[] ParameterNames { get; private set; }
		public ParameterExpression[] Parameters { get; private set; }
		public TruthTableColumn[] Columns { get; private set; }

		// 論理式と変数を与えて、自動で真理値表を作る。
		public TruthTable (ParameterExpression[] parameters, Expression expression)
		{
			// param size restrection
			if (parameters.Length > MaxParameterCount) {
				throw new Exception ("Too many parameters.");
			}

			// init
			Parameters = parameters;
			ParameterNames = parameters.Select(p => p.Name).ToArray();
			Columns = new TruthTableColumn[1 << parameters.Length];

			// compile expression
			var lambda = Expression.Lambda(expression, parameters);
			var func = lambda.Compile();
			var args = new object[parameters.Length];

			// make table
			for (int i = 0; i < Columns.Length; i++) {
				Columns [i] = new TruthTableColumn (parameters.Length);
				for (int j = 0; j < parameters.Length; j++) {
					bool bit = (i & (1 << j)) != 0;
					Columns [i] [j] = bit;
					args [j] = bit;
				}
				Columns [i].Result = (bool)func.DynamicInvoke (args);
			}
		}

		//  各行を与えて真理値表を作る。。
		public TruthTable (ParameterExpression[] parameters, IEnumerable<TruthTableColumn> cols)
		{
			// param size restrection
			if (parameters.Length > MaxParameterCount) {
				throw new Exception ("Too many parameters.");
			}
			Parameters = parameters;
			ParameterNames = parameters.Select(p => p.Name).ToArray();
			Columns = cols.ToArray ();
		}

		// 真理値表を出力する。
		public override string ToString ()
		{
			StringBuilder result = new StringBuilder ();
			int space = ParameterNames.Max (s => s.Length) + 1;
			// name
			foreach (string name in ParameterNames) {
				result.Append (name.PadLeft(space));
			}
			result.AppendLine ();
			// rows
			foreach (TruthTableColumn column in Columns) {
				foreach (bool? bit in column) {
					result.Append (nullableBoolToString(bit).PadLeft (space));
				}
				result.AppendLine (nullableBoolToString (column.Result).PadLeft (space));
			}
			result.Remove (result.Length - 1, 1);
			return result.ToString ();
		}

		// Don't care or 真理値 を文字列に変換。
		private string nullableBoolToString (bool? bit)
		{
			if (bit.HasValue) {
				return bit.Value ? "1" : "0";
			} else {
				return "-";
			}
		}

		// 主加法標準形
		public Expression ToDisjunctiveCanonicalExpression ()
		{
			return Columns
				.Where ( col => col.Result )
					.Select
					(
						col => Parameters
						.Cast<Expression>()
						.Zip (col, (p, bit) => bit.HasValue ? (bit.Value ? p : Expression.Not (p)) : null)
						.Where (e => e != null)
						.Aggregate (Expression.And)
					)
					.Aggregate(Expression.Or);
		}

		// 主乗法標準形
		public Expression ToConjunctiveCanonicalExpression ()
		{
			return Columns
				.Where ( col => !col.Result )
					.Select 
					(
						col => Parameters
						.Cast<Expression>()
						.Zip (col, (p, bit) => bit.HasValue ? (bit.Value ? Expression.Not (p) : p ) : null)
						.Where (e => e != null)
						.Aggregate (Expression.Or)
					)
					.Aggregate(Expression.And);
		}
	}

	/// <summary>
	/// TruthTableの各行。
	/// </summary>
	public class TruthTableColumn : IEnumerable<bool?>
	{
		private bool?[] parameters;
		private bool? result;

		public TruthTableColumn (int size)
		{
			parameters = new bool?[size];
			result = null;
		}

		public TruthTableColumn (IEnumerable<bool?> elems)
		{
			parameters = elems.ToArray ();
			result = null;
		}

		public bool? this [int i]
		{
			get {
				if (i < Size) {
					return parameters [i];
				} else {
					throw new IndexOutOfRangeException ();
				}
			}
			set {	
				if (i < Size) {
					parameters [i] = value;
				} else {
					throw new IndexOutOfRangeException ();
				}
			}
		}

		public bool Result
		{
			get {
				if (result.HasValue) {
					return result.Value;
				} else {
					throw new Exception ("Result has yet to be set.");
				}
			}
			set {
				if (result.HasValue) {
					throw new Exception ("Cannot set result twice.");
				} else {
					result = value;
				}
			}
		}

		IEnumerator<bool?> IEnumerable<bool?>.GetEnumerator ()
		{
			foreach (bool? b in parameters) {
				yield return b;
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			foreach (bool? b in parameters) {
				yield return b;
			}
		}

		public int Size
		{
			get {
				return parameters.Length;
			}
		}
	}
}

