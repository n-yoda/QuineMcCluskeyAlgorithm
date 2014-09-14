using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace QuineMcCluskeyAlgorithm
{
	/// <summary>
	/// 論理式を見やすく出力する
	/// </summary>
	public static class DisplayExpression
	{
		public static string ToSimpleString (this Expression exp)
		{

			if (exp is ParameterExpression) {
				return ((ParameterExpression)exp).Name;
			} else if (exp is UnaryExpression) {
				// 単項演算子
				var uexp = exp as UnaryExpression;
				switch (exp.NodeType) {
				case ExpressionType.Not:
					switch (uexp.Operand.NodeType) {
					case ExpressionType.Not:
					case ExpressionType.Parameter:
						return "!" + uexp.Operand.ToSimpleString ();
					default:
						return "!(" + uexp.Operand.ToSimpleString () + ")";
					}
				}
			} else if (exp is BinaryExpression) {
				// 二項演算子
				var bexp = exp as BinaryExpression;
				string op = "";
				switch (exp.NodeType) {
				case ExpressionType.And:
					op = "*";
					break;
				case ExpressionType.Or:
					op = "+";
					break;
				case ExpressionType.ExclusiveOr:
					op = "^";
					break;
				default:
					throw new NotSupportedException ();
				}
				string left = bexp.Left.ToSimpleString ();
				if (bexp.Left.NodeType != ExpressionType.Parameter
					&& bexp.Left.NodeType != bexp.NodeType
				    && !(bexp.Left is UnaryExpression)) {
					left = "(" + left + ")";
				}
				string right = bexp.Right.ToSimpleString ();
				if (bexp.Right.NodeType != ExpressionType.Parameter
					&& !(bexp.Right is UnaryExpression)) {
					right = "(" + right + ")";
				}
				return left + " " + op + " " + right;
			}
			throw new NotSupportedException ();
		}
	}
}

