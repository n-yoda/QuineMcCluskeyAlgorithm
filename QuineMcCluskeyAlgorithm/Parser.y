%using System.Linq.Expressions
%using System.Collections.Generic
%namespace QuineMcCluskeyAlgorithm

%{
	public Expression Expression;
	public SortedList<string, ParameterExpression> Parameters
		= new SortedList<string, ParameterExpression> ();
%}

%start start

%union { public string Name;
		 public Expression Expression; }

%type <Expression> start exp

%token Bra Ket
%token Var And Or Xor Not
%token <Name> Var

%nonassoc Bra Ket
%left Or
%left Xor
%left And
%nonassoc Not

%%

start:
	  exp			{ this.Expression = $1; }
;

exp: Var			{
	  					if (!Parameters.ContainsKey($1)) {
	  						Parameters.Add($1, Expression.Parameter(typeof(bool), $1));
	  					}
	  					$$ = Parameters[$1];
	  				}

	| Bra exp Ket	{ $$ = $2; }
	| exp Or  exp	{ $$ = Expression.Or ($1, $3); }
	| exp Xor exp	{ $$ = Expression.ExclusiveOr ($1, $3); }
	| exp And exp	{ $$ = Expression.And ($1, $3); }
	| Not exp		{ $$ = Expression.Not ($2); }
;

%%

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
