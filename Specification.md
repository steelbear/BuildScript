# BuildScript Language Specifications


# Token
```
token
    : identifier
    | keyword
    | integer
    | string
    | interpolated_string
    | punctuator
    | new_line
    ;

new_line
    : '\r' | '\n' | '\r\n'
    ;
```
**Whitespaces and comments (starts with '#' and until new_line or end of file) are ignored.*

### Identifiers
To make lexer simple, only alphabet can be used in identifier.

```
identifier
    : identifier_start identifier_character*
    ;

identifier_start
    : '_'
    | '$'
    | '<An upper or lower case of alphabet>'
    ;

identifier_character
    : identifier_start
    | digit
    ;
```

### Keyword
```
keyword
    : 'break'     | 'case'      | 'default'   | 'else'
    | 'elseif'    | 'false'     | 'for'       | 'function'
    | 'global'    | 'if'        | 'in'        | 'match'
    | 'not'       | 'project'   | 'raise'     | 'repeat'
    | 'return'    | 'target'    | 'true'      | 'undefined'
    | 'until'     | 'var'       | 'while'
    ;
```

### Integers
```
integer
    : digit+
    ;

digit
    : '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9'
    ;
```

### Strings
```
string
    : '\'' character* '\''
    ;

character
    : '<Any character except \' and new_line>'
    | '\\\''
    ;

interpolated_string
    : '"' interpolated_string_character* '"'
    ;

interpolated_string_character
    : '<Any character except '"', new_line, interpolating_expression_intro>'
    | interpolating_expression_intro expression '}'
    | '\\"'
    | '\\#'
    ;

interpolating_expression_intro
    : '#{'
    ;
```

### Punctuators
```
punctuator
    : '('  | ')'  | '{'  | '}'  | '['  | ']'  | '.'
    | ','  | ':'  | '+'  | '-'  | '*'  | '/'  | '%'
    | '='  | '<'  | '>'  | '?'  | '->' | '<=' | '>='
    | '==' | '!=' | '+=' | ':=' | '||' | '&&'
    ;
```

### New Line
```
new_line
    : '\r'
    | '\n'
    | '\r\n'
    ;
```

# Grammars

### Function Declaration
```
function_declaration
    : 'function' identifier '(' parameter_list? ')' block
    ;

parameter_list
    : identifier
    | parameter_list ',' identifier
    ;
```

## Statements
```
statement
    : if_statement
    | match_case_statement
    | for_statement
    | while_statement
    | repeat_until_statement
    | raise_statement
    | return_statement
    | break_statement
    | new_line
    ;

statement_list
    : statement
    | statement_list EOL statement
    ;

block
    : '{' statement_list '}'
    ;
```

### If Statement
```
if_statement
    : 'if' condition block elseif_statement* else_statement?
    ;

elseif_statement
    : 'elseif' condition block
    ;

else_statement
    : 'else' block
    ;

condition
    : '(' expression ')'
```

### Match-Case Statement
Restriction:
- only one `default_labeled_statement` in `match_case_statement` allowed.

```
match_cast_statement
    : 'match' '(' expression ')' '{' labeled_statement+ '}'
    ;

labeled_statement
    : case_labeled_statement
    | default_labeled_statement
    ;

case_labeled_statement
    : 'case' constant_value ':' statement_list?
    ;

default_labeled_statement
    : 'default' ':' statement_list?
    ;
```

### For Statement
Restriction:
- `expression` should return array-type value.

```
for_statement
    : 'for' '(' identifier 'in' expression ')' block
    ;
```

### While Statement
```
while_statement
    : 'while' condition block
    ;
```

### Repeat-Until Statement
```
repeat_until_statement
    : 'repeat' block 'until' condition
    ;
```

### Raise Statement
```
raise_statement
    : 'raise' expression
    ;
```

### Return Statement
Restriction:
- only can be used in `block` in `function_declaration`.
```
return_statement
    : 'return' expression?
    | 'return' '?' expression optional_expression?
    ;

optional_expression
    : ':' expression
    ;
```

### Break Statement
```
break_statement
    : 'break' expression?
    | 'break' '?' expression
    ;
```

## Expressions

### Expression
```
expression
    : logical_or_expression
    | assignment_expression
    | closure_expression
    ;
```

### Assignment Expression
```
assignment_expression
    : identifier assignment_operator expression optional_expression?
    ;

assignment_operator
    : '=' | '+=' | ':='
    ;
```

### Closure Expression
```
closure_expression
    : '{' closure_parameter_list? statements '}'
    ;

closure_parameter_list
    : parameter_list? '->'
```

### Logical OR Expression
```
logical_or_expression
    : logical_and_expression
    | logical_or_expression '||' logical_and_expression
```

### Logical AND Expression
```
logical_and_expression
    : equality_expression
    | logical_and_expression '&&' equality_expression
    ;
```

### Equality Expression
```
equality_expression
    : realtional_expression
    | equality_expression equality_operator relational_expression
    ;

equality_operator
    : '==' | '!='
    ;
```

### Relational Expression
```
relational_expression
    : additive_expression
    | relational_expression relational_operator additive_expression
    ;

relational_operator
    : '<' | '<=' | '>' | '>='
    | 'not'? 'in' // right-hand value must be scalar and left-hand value must be array
    ; 
```

### Additive Expression
```
additive_expression
    : multiplicative_expression
    | additive_expression '+' multiplicative_expression
    | additive_expression '-' multiplicative_expression
    ;
```

### Multiplicative Expression
```
multiplicative_expression
    : postfix_expression
    | multiplicative_expression '*' postfix_expression
    | multiplicative_expression '/' postfix_expression
    | multiplicative_expression '%' postfix_expression
    ;
```

### Postfix Expression
```
postfix_expression
    : primary_expression
    | member_access_expression
    | invocation_expression
    ;

member_access_expression
    : primary_expression '.' identifier
    ;

invocation_expression
    : primary_expression '(' expression_list? ')'
    | primary_expression expression_list
    ;

expression_list
    : expression
    | expression_list ',' expression
    ;
```

### Primary Expression
```
primary_expression
    : value
    | array
    | parenthesis_expression
    | project_access_expression
    ;

value
    : identifier
    | interpolated_string
    | constant_value
    ;

constant_value
    : integer
    | string
    | 'true'
    | 'false'
    | 'undefined'
    ;

array
    : '[' value_list? ']'
    ;

value_list:
    : value
    | value_list ',' value
    ;

parenthesis_expression
    : '(' expression ')'
    ;

project_access_expression
    : 'project' '.' identifier
    ;
```