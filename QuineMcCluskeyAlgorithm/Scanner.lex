%namespace QuineMcCluskeyAlgorithm
%option nofiles

alpha	[a-zA-Z]
num		[0-9]
name	{alpha}+{num}*

%%

\(			{ return (int)Tokens.Bra; }
\)			{ return (int)Tokens.Ket; }
and			{ return (int)Tokens.And; }
or			{ return (int)Tokens.Or;  }
xor			{ return (int)Tokens.Xor; }
not			{ return (int)Tokens.Not; }
*			{ return (int)Tokens.And; }
+			{ return (int)Tokens.Or;  }
&			{ return (int)Tokens.And; }
|			{ return (int)Tokens.Or;  }
\^			{ return (int)Tokens.Xor; }
!			{ return (int)Tokens.Not; }

{name}		{ yylval.Name = yytext;
			  return (int)Tokens.Var; }

%%