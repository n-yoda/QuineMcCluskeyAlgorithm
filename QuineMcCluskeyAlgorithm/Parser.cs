// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  Yoda-MacBookPro.local
// DateTime: 2014/09/14 19:10:49
// UserName: yoda
// Input file <Parser.y - 2013/06/16 2:16:14>

// options: no-lines gplex

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace QuineMcCluskeyAlgorithm
{
public enum Tokens {error=2,EOF=3,Bra=4,Ket=5,Var=6,
    And=7,Or=8,Xor=9,Not=10};

public struct ValueType
{ public string Name;
		 public Expression Expression; }
// Abstract base class for GPLEX scanners
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

// Utility class for encapsulating token information
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public class ScanObj {
  public int token;
  public ValueType yylval;
  public LexLocation yylloc;
  public ScanObj( int t, ValueType val, LexLocation loc ) {
    this.token = t; this.yylval = val; this.yylloc = loc;
  }
}

[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public class Parser: ShiftReduceParser<ValueType, LexLocation>
{
  // Verbatim content from Parser.y - 2013/06/16 2:16:14
	public Expression Expression;
	public SortedList<string, ParameterExpression> Parameters
		= new SortedList<string, ParameterExpression> ();
  // End verbatim content from Parser.y - 2013/06/16 2:16:14

#pragma warning disable 649
  private static Dictionary<int, string> aliases;
#pragma warning restore 649
  private static Rule[] rules = new Rule[9];
  private static State[] states = new State[16];
  private static string[] nonTerms = new string[] {
      "start", "exp", "$accept", };

  static Parser() {
    states[0] = new State(new int[]{6,10,4,11,10,14},new int[]{-1,1,-2,3});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{8,4,9,6,7,8,3,-2});
    states[4] = new State(new int[]{6,10,4,11,10,14},new int[]{-2,5});
    states[5] = new State(new int[]{8,-5,9,6,7,8,3,-5,5,-5});
    states[6] = new State(new int[]{6,10,4,11,10,14},new int[]{-2,7});
    states[7] = new State(new int[]{8,-6,9,-6,7,8,3,-6,5,-6});
    states[8] = new State(new int[]{6,10,4,11,10,14},new int[]{-2,9});
    states[9] = new State(-7);
    states[10] = new State(-3);
    states[11] = new State(new int[]{6,10,4,11,10,14},new int[]{-2,12});
    states[12] = new State(new int[]{5,13,8,4,9,6,7,8});
    states[13] = new State(-4);
    states[14] = new State(new int[]{6,10,4,11,10,14},new int[]{-2,15});
    states[15] = new State(-8);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-3, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{-2});
    rules[3] = new Rule(-2, new int[]{6});
    rules[4] = new Rule(-2, new int[]{4,-2,5});
    rules[5] = new Rule(-2, new int[]{-2,8,-2});
    rules[6] = new Rule(-2, new int[]{-2,9,-2});
    rules[7] = new Rule(-2, new int[]{-2,7,-2});
    rules[8] = new Rule(-2, new int[]{10,-2});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
#pragma warning disable 162, 1522
    switch (action)
    {
      case 2: // start -> exp
{ this.Expression = ValueStack[ValueStack.Depth-1].Expression; }
        break;
      case 3: // exp -> Var
{
	  					if (!Parameters.ContainsKey(ValueStack[ValueStack.Depth-1].Name)) {
	  						Parameters.Add(ValueStack[ValueStack.Depth-1].Name, Expression.Parameter(typeof(bool), ValueStack[ValueStack.Depth-1].Name));
	  					}
	  					CurrentSemanticValue.Expression = Parameters[ValueStack[ValueStack.Depth-1].Name];
	  				}
        break;
      case 4: // exp -> Bra, exp, Ket
{ CurrentSemanticValue.Expression = ValueStack[ValueStack.Depth-2].Expression; }
        break;
      case 5: // exp -> exp, Or, exp
{ CurrentSemanticValue.Expression = Expression.Or (ValueStack[ValueStack.Depth-3].Expression, ValueStack[ValueStack.Depth-1].Expression); }
        break;
      case 6: // exp -> exp, Xor, exp
{ CurrentSemanticValue.Expression = Expression.ExclusiveOr (ValueStack[ValueStack.Depth-3].Expression, ValueStack[ValueStack.Depth-1].Expression); }
        break;
      case 7: // exp -> exp, And, exp
{ CurrentSemanticValue.Expression = Expression.And (ValueStack[ValueStack.Depth-3].Expression, ValueStack[ValueStack.Depth-1].Expression); }
        break;
      case 8: // exp -> Not, exp
{ CurrentSemanticValue.Expression = Expression.Not (ValueStack[ValueStack.Depth-1].Expression); }
        break;
    }
#pragma warning restore 162, 1522
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliases != null && aliases.ContainsKey(terminal))
        return aliases[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

Parser() : base(null) {}

public static Parser Parse (string input)
{
	Parser parser = new Parser ();
	Scanner scanner = new Scanner ();
	scanner.SetSource (input, 0);
	parser.Scanner = scanner;
	parser.Parse();
	return parser;
}
}
}
