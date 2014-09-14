using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QuineMcCluskeyAlgorithm
{
	/// <summary>
	/// 論理式に出て来る変数の同一性（単純に名前が同じかどうか）を判定するComparer.
	/// </summary>
	public class ParameterComparer : IComparer<ParameterExpression>
	{
		StringComparer stringComparer;
		public ParameterComparer ()
		{
			stringComparer = new StringComparer ();
		}
		public int Compare (ParameterExpression x, ParameterExpression y)
		{
			return stringComparer.Compare (x.Name, y.Name);
		}
	}
}
