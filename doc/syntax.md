# Syntax Rules

MAGES follows most of the syntax rules of YAMP, which is close to MATLAB. This is, however, a quite vague statement and does not reveal anything beyond speculation. Therefore, the following paragraphs outline the syntax details as specific as possible using something close to BNF.

## Characters

Ignorable characters (space characters) are defined via:

```
space ::= [#x9 - #xd] | #x20 | #x85 | #xa0 | #x1680 | #x180e | (#x2000 - #x200a) | #x2028 | #x2029 | #x202f | #x205f | #x3000
```

Other important characters are also fixed in a definition:

```
sign ::= '+' | '-'
comma ::= ','
colon ::= ':'
semicol ::= ';'
letter ::= [A - Z] | [a - z]
digit ::= [0 - 9]
``` 

## Primitives

A number is the usual suspect:

```
binary_character ::= '0' | '1'
hex_character ::= digit | [a - f] | [A - F]
binary ::= '0' ('b' | 'B') binary_character+
hex ::= '0' ('x' | 'X') hex_character+
float ::= digit+ (. digit* (('e' | 'E') sign? digit+)?)?
number ::= float | binary | hex
```

Boolean primitive values are given by keywords:

```
boolean ::= 'true' | 'false'
```

The full list of keywords is actually a bit longer. These values are reserved (even though most are not used in the current specification):

```
keywords ::= 'true' | 'false' | 'return' | 'var' | 'let' | 'const' | 'for' | 'while' | 'do' | 'module' | 'if' | 'else' | 'break' | 'yield' | 'async' | 'await' | 'class' | 'static' | 'new' | 'delete'
```

A string literal is specified as follows:

```
string_character ::= [#x0 - #xffff] - '"'
escape_character ::= '\' 
string ::= '"' (string_character | escape_character)* '"'
```

Identifiers are given by:

```
name_start_character ::= [#x80 - #xFFFF] | letter | "_"
name_character ::= name_start_character | digit
identifier ::= name_start_character name_character*
```

## Literals

A matrix is defined via columns and rows:

```
columns ::= expr space* (comma space* expr space*)*
rows ::= columns (semicol space* columns)*
matrix ::= '[' space* rows? ']'
```

The language also contains object literals. They are defined as:

```
property ::= identifier space* colon space* expr space*
properties ::= property (comma space* property)* comma? space*
object ::= '{' space* properties? '}'
```

All the former definitions lead to primitives and literals in general. Primitives are fixed blocks of information, while in general literals may be composed of these fixed blocks.

```
primitive ::= string | boolean | number
literal ::= primitive | matrix | object
```

## Functions

Another important concept are anonymous functions, which are commonly referred to as lambda expressions:

```
parameters ::= '(' space* (identifier space* (',' space* identifier space*)*)? ')'
function ::= (parameters | identifier) space* '=>' space* expr
```

Functions can be called by using the function call operator in combination with a suitable amount of arguments.

```
arguments ::= '(' space* (expr (space* (',' space* expr space*)*)? ')'
call ::= expr space* arguments
```

Besides a couple of unary and binary operators, two tertiary operators are also included. We have:

```
range ::= expr space* ':' (space* expr space* ':')? space* expr
condition ::= expr space* '?' space* expr space* ':' space* expr
```

## Operators

A special kind of binary operator is the member operator, which is used in conjunction with objects:

```
member ::= expr space* '.' space* identifier
```

All other binary operators can be summarized as follows:

```
binary_operator ::= '+' | '-' | '*' | '/' | '\' | '%' | '^' | '>' | '>=' | '<=' | '<' | '==' | '~=' | '&&' | '||'
binary ::= expr space* binary_operator space* expr
```

There is one exception to this rule: The multiplication operator also works in special cases without a symbol (`*`). This may be summarized as follows:

```
explicit_multiplication ::= expr space* '*' space* expr
implicit_multiplication ::= expr space* (number | identifier)
multiplication ::= explicit_multiplication | implicit_multiplication
```

Furthermore, two types of unary operations exist:

```
left_unary_operator ::= '+' | '-' | '~' | '++' | '--'
right_unary_operator ::= '!' | ''' | '++' | '--'
pre_unary ::= left_unary_operator space* expr
post_unary ::= expr space* right_unary_operator
unary ::= pre_unary | post_unary
```

## Expressions

Currently, we are missing the definition of `expr`. Since `expr` encloses objects and matrices one had to be first, which - in this case - was chosen to be objects and matrices.

```
assignable_expr ::= identifier | assignment | member | call
computing_expr ::= literal | binary | unary | function | literal | condition | range | arguments
expr ::= assignable_expr | computing_expr
```

The recursion of `assignable_expr` only works if we know the definition of this special expression:

```
assignment ::= assignable_expr space* '=' space* expr
```

## Statements

Introducing a new (local) variable can then be defined by the `var_statement`:

```
var_statement ::= 'var' space+ assignment
```

Statements are generally given by the following construct:

```
statement_line ::= expr | var_statement
statement ::= statement_line space* ';'
```

