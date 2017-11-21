BuildScript Language Specifications
=======================================
_v0.1_

이 문서는 BuildScript의 개념, API, 문법 구조 등을 설명합니다. 아직 초안이므로 문서의 내용은 __언제든지__ 변경될 수 있습니다.

Index
---------------------------------------
* [Concepts](Specification.md#concepts)
    * [Projects](Specification.md#projects)
    * [Targets](Specification.md#targets)
    * [Tasks](Specification.md#tasks)
    * [Statements](Specification.md#statements)
    * [Expressions](Specification.md#expressions)
    * [Variables](Specification.md#variables)
    * [Types and Values](Specification.md#types-and-values)
* [Grammars](Specification.md#grammars)
* [APIs](Specification.md#apis)

Concepts
---------------------------------------
BuildScript는 [MSBuild](https://docs.microsoft.com/ko-kr/visualstudio/msbuild/msbuild)와 [Gradle](https://gradle.org)의 영향을 받아 제작되었습니다. 대부분의 문법 구조는 Gradle의 DSL인 [Groovy](http://groovy-lang.org)을 기반으로 설계되었고, 프로젝트나 타겟, 태스크, 프로퍼티 등의 개념은 MSBuild의 기반으로 작성되었습니다.

### Projects

### Targets

### Tasks

### Statements

### Expressions

### Variables

### Types and Values
BuildScript의 타입구조에는 정수(Integer), 문자열(String), 리스트(List), 논리(Boolean) 그리고 클로저(Closure) 5가지의 타입과 "정의되지 않음"(Undefined)이 존재합니다.

#### Integer Type
정수(Integer) 타입은

#### String Type
문자열(String) 타입은 

#### List Type
리스트(List) 타입은

#### Boolean Type
논리(Boolean) 타입은

#### Closure Type
클로저(Closure) 타입은

#### Undefined
"정의되지 않음"은 변수 혹은 프로퍼티의 멤버 참조시 없는 멤버를 참조할 시 반환되는 값, 혹은 값을 반환하지 않는 클로저나 태스크의 반환값입니다. C언어 계열 언어의 `NULL`(`null`, `nil` 등)과 비슷하지만 javascript의 `undefined`와 더 가깝습니다.

키워드 `undefined`를 통해 정의됩니다.

Grammars
---------------------------------------

## Lexical Elements
_*여기 기술되는 모든 문법은 ANTLR 문법으로 기술됩니다._

### Token
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

### Identifiers

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

### Keywords

```
keyword
    : 'break'     | 'case'      | 'default'   | 'else'
    | 'elseif'    | 'false'     | 'for'       | 'global'
    | 'if'        | 'in'        | 'match'     | 'not'
    | 'project'   | 'raise'     | 'repeat'    | 'return'
    | 'target'    | 'task'      | 'true'      | 'undefined'
    | 'until'     | 'var'       | 'while'
    ;
```

### Integer literals
정수형 문자열(Integer literals)은 숫자를 나타내는 문자열입니다. 정수형 문자열은 10진수와 16진수 두 가지 방법으로 나타낼 수 있습니다.

```
integer
    : decimal_integer
    | hexadecimal_integer
    ;

decimal_integer
    : decimal_digit+
    ;

hexadecimal_integer
    : '0x' hexadecimal_digit+
    | '0X' hexadecimal_digit+
    ;

decimal_digit
    : '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9'
    ;

hexadecimal_digit
    : '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9'
    | 'a' | 'b' | 'c' | 'd' | 'e' | 'f'
    | 'A' | 'B' | 'C' | 'D' | 'E' | 'F'
    ;
```

### String literals
문자열(String literals)은 일반 문자열(String)과 보간 문자열(Interpolated string)이 있습니다. 일반 문자열은 `'`으로 시작과 끝을, 보간 문자열은 `"`으로 시작과 끝을 나타냅니다.

보간 문자열은 `#{` 와 `}` 사이에 수식(Expression)을 적을 수 있습니다. (예: `"Hello, #{name}"`) 수식은 그 문자열이 평가(Evaluate)될 때 평가되며, 평가된 결과물은 일반 문자열이 됩니다.

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

#### Function Declaration
```
function_declaration
    : 'function' identifier '(' parameter_list? ')' block
    ;

parameter_list
    : identifier
    | parameter_list ',' identifier
    ;
```

### Statements
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

#### If Statement
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

#### Match-Case Statement
Restriction:
- Only one `default_labeled_statement` in `match_case_statement` allowed.

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

#### For Statement
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

#### Repeat-Until Statement
```
repeat_until_statement
    : 'repeat' block 'until' condition
    ;
```

#### Raise Statement
```
raise_statement
    : 'raise' expression
    ;
```

#### Return Statement
```
return_statement
    : 'return' expression?
    | 'return' '?' expression optional_expression?
    ;

optional_expression
    : ':' expression
    ;
```

#### Break Statement
Restriction:
- Not determined
```
break_statement
    : 'break' expression?
    | 'break' '?' expression
    ;
```

### Expressions

#### Expression
```
expression
    : logical_or_expression
    | assignment_expression
    | closure_expression
    ;
```

#### Assignment Expression
```
assignment_expression
    : identifier assignment_operator expression optional_expression?
    ;

assignment_operator
    : '=' | '+=' | ':='
    ;
```

#### Closure Expression
```
closure_expression
    : '{' closure_parameter_list? statements '}'
    ;

closure_parameter_list
    : parameter_list? '->'
```

#### Logical OR Expression
```
logical_or_expression
    : logical_and_expression
    | logical_or_expression '||' logical_and_expression
```

#### Logical AND Expression
```
logical_and_expression
    : equality_expression
    | logical_and_expression '&&' equality_expression
    ;
```

#### Equality Expression
```
equality_expression
    : realtional_expression
    | equality_expression equality_operator relational_expression
    ;

equality_operator
    : '==' | '!='
    ;
```

#### Relational Expression
```
relational_expression
    : additive_expression
    | relational_expression relational_operator additive_expression
    ;

relational_operator
    : '<' | '<=' | '>' | '>='
    | 'in'
    | 'not' 'in'
    ; 
```

#### Additive Expression
```
additive_expression
    : multiplicative_expression
    | additive_expression '+' multiplicative_expression
    | additive_expression '-' multiplicative_expression
    ;
```

#### Multiplicative Expression
```
multiplicative_expression
    : postfix_expression
    | multiplicative_expression '*' postfix_expression
    | multiplicative_expression '/' postfix_expression
    | multiplicative_expression '%' postfix_expression
    ;
```

#### Postfix Expression
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

#### Primary Expression
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

APIs
---------------------------------------

TBD