using System;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace QuineMcCluskeyAlgorithm
{
	class MainClass
	{
		/*
		 * オプション
		 * -n no caption
		 * -T Truth Table
		 * -C Canonical Form
		 * -D Disjunctive Form
		 * -qT QM Compressed Truth Table
		 * -qD QM Compressed Disjunctive Form
		 * -cT Check result for Truth Table
		 * -cC Check result for Canonical Form
		 * -cD Check result for 
		 */
		static bool noCaption = false;
		public static void Main (string[] args)
		{
			string line;
			noCaption = args.Contains("-n");
			while((line = Console.ReadLine ()) != null) {
				var parser = Parser.Parse (line);

				if (parser.Expression != null) {
					var table = new TruthTable (parser.Parameters.Values.ToArray (), parser.Expression);
					// 真理値表
					if (args.Contains("-T")) {
						caption ("[Truth Table]");
						echo (table.ToString());
					}
					// 主加法標準形
					if (args.Contains("-D")) {
						caption ("[Disjunctive]");
						echo (table.ToDisjunctiveCanonicalExpression ().ToSimpleString ());
					}
					// 主乗法標準形
					if (args.Contains("-C")) {
						caption ("[Conjunctive]");
						echo (table.ToConjunctiveCanonicalExpression ().ToSimpleString ());
					}

					// 省力化
					if (!args.Any (s => s.StartsWith("-q") || s.StartsWith("-c"))) {
						Console.WriteLine ();
						continue;
					}

					// QM法
					var qmc = new QuineMcCluskey (table);
					foreach (IEnumerable<TruthTableColumn> result in qmc.Compressed) {
						var newTable = new TruthTable (table.Parameters, result);
						// QM法　真理値
						if (args.Contains("-qT")) {
							caption ("[QM'ed Truth Table]");
							echo (newTable.ToString ());
						}
						// QM法　主加法標準形
						if (args.Contains("-qD")) {
							caption ("[QM'ed Disjunctive]");
							echo (newTable.ToDisjunctiveCanonicalExpression ().ToSimpleString ());
						}

						var checkTable = new TruthTable (table.Parameters, newTable.ToDisjunctiveCanonicalExpression ());
						// 確認用 真理値表
						if (args.Contains("-cT")) {
							caption ("[Check Truth Table]");
							echo (checkTable.ToString ());
						}
						// 確認用　主乗法標準形
						if (args.Contains("-cC")) {
							caption ("[Check Conjunctive]");
							echo (checkTable.ToConjunctiveCanonicalExpression ().ToSimpleString ());
						}
						// 確認用　主加法標準形
						if (args.Contains("-cD")) {
							caption ("[Check Disjunctive]");
							echo (checkTable.ToDisjunctiveCanonicalExpression ().ToSimpleString ());
						}
					}
					Console.WriteLine ();
				} else {
					Console.Error.WriteLine("Parse Error.");
				}
			}
		}

		static void caption(string caption)
		{
			if (!noCaption) {
				Console.WriteLine ();
				Console.WriteLine (caption);
			}
		}

		static void line()
		{
			Console.WriteLine ();
		}

		static void echo(string str)
		{
			Console.WriteLine (str);
		}
	}
}
