<!--
Grammars.md
author: numver8638(numver8638@naver.com)

This file is part of BuildScript.

BuildScript is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>
-->

Grammars
=======================================
여기에서는 BuildScript의 문법 구조와 의미 등을 설명합니다.

**여기에서는 문법 구조 설명에 ANTLR 문법을 사용합니다.*

## Lexical elements
BuildScript의 스크립트는 0개 이상의 토큰(Token), 공백 문자(Whitespace), 주석(Comment)으로 구성되어있습니다. 구문 분석에는 토큰만이 이용됩니다.

```antlr
input
    : input_element*
    ;

input_element
    : token
    | whitespaces
    | comment
    ;

whitespaces
    : whitespace*
    ;

whitespace
    : ' '  // U+0020
    | '\t' // U+0009
    ;

new_line
    : '\r'   // U+000A
    | '\n'   // U+000D
    | '\r\n'
    ;

comment
    : '#' comment_content
    ;

comment_content
    : comment_char*
    ;

comment_char
    : '<new_line을 제외한 아무 문자>'
    ;
```

토큰(Token)은 구문 분석에 이용되는 최소 어휘 요소입니다. 토큰의 종류에는 크게 키워드(Keyword), 식별자(Identifier), 숫자 리터럴(Integer literal), 문자열 리터럴(String literal), 문장 부호(Punctuator)가 있습니다.

```antlr
token
    : keyword
    | identifier
    | integer_literal
    | string_literal
    | punctuator
    ;
```

### Keywords
키워드(Keyword)는 BuildScript에서 사용되는 예약된 식별자입니다. 키워드는 대소문자를 구분하며 원래의 목적 외 다른 목적으로 사용 할 수 없습니다.

```antlr
keyword
    : 'break'     | 'case'      | 'default'   | 'else'
    | 'elseif'    | 'false'     | 'for'       | 'global'
    | 'if'        | 'import'    | 'in'        |  'match'
    | 'not'       | 'project'   | 'raise'     | 'repeat'
    | 'return'    | 'target'    | 'task'      | 'true'
    | 'undefined' | 'until'     | 'var'       | 'while'
    ;
```

### Identifiers
식별자(Identifier)는 숫자와 숫자가 아닌 문자의 연속으로 표현되는 다른것과 구분하기 위한 이름입니다.

```antlr
identifier
    : nondigit
    | identifier digit
    | identifier nondigit
    ;

nondigit
    : 'a' | 'b' | 'c' | 'd' | 'e' | 'f' | 'g'
    | 'h' | 'i' | 'j' | 'k' | 'l' | 'm' | 'n'
    | 'o' | 'p' | 'q' | 'r' | 's' | 't' | 'u'
    | 'v' | 'w' | 'x' | 'y' | 'z'
    | 'A' | 'B' | 'C' | 'D' | 'E' | 'F' | 'G'
    | 'H' | 'I' | 'J' | 'K' | 'L' | 'M' | 'N'
    | 'O' | 'P' | 'Q' | 'R' | 'S' | 'T' | 'U'
    | 'V' | 'W' | 'X' | 'Y' | 'Z'
    | '_' | '$'
    ;

digit
    : '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9'
    ;
```
**구문 분석의 용이성을 위해 `_` 와 `$`, 숫자, 알파벳 대소문자만 허용됩니다.*

### Literals
리터럴(Literal)에는 숫자 리터럴(Integer literal)과 문자열 리터럴(String literal), 논리 리터럴(Boolean literal), "정의되지 않음" 리터럴("Undefined" literal)이 있고, 문자열 리터럴에는 일반 문자열(Plain string)과 보간 문자열(Interpolated string)이 있습니다.

BuildScript에서는 숫자 리터럴을 지원하지만 정수에 대한 연산(가감승제, 비트연산 등)을 __지원하지 않습니다__*. 따라서 숫자 리터럴도 일반 문자열과 같이 취급됩니다.

**빌드작업시 숫자보다 문자열의 조작이 더 빈번한 경우가 많아 기능을 포함하지 않음.*

논리 리터럴은 참과 거짓을 표현하는 리터럴입니다.

"정의되지 않음" 리터럴은 변수나 프로퍼티에 없는 멤버의 참조나 반환값이 없는 클로저나 태스크가 반환하는 "정의되지 않음"을 표현하는 리터럴입니다. "정의되지 않음"에 대한 자세한 사항은 [여기](Concepts.md#undefined)에서 확인 할 수 있습니다. 

보간 문자열은 일반 문자열과 비슷하지만 중간에 식(Expression)이 포함될 수 있는 문자열입니다. 문자열 안에서 `#{`와 `}` 사이에 식이 들어갑니다.

보간 문자열에서 `}`은 `#{`가 선행에 있었을 때 닫는 괄호로 인식됩니다. 따라서 `}`기호를 출력하기 위해 다른 작업이 필요하지 않습니다. 하지만 문자열 내에서 `#{`은 식이 포함되는 구간임을 알리는 기호이므로 이 기호열을 사용하기 위해서는 `'\#{'`와 같이 표현해야합니다.

예:
```
var variable = 'value'

Print "#{variable}"   # 이는 'value'을 표시합니다.
Print "\#{variable}"  # 이는 '#{variable}'을 표시합니다.
```

Syntax:
```antlr
integer_literal
    : decimal_literal
    | hexadecimal_literal
    ;

decimal_literal
    : digit+
    ;

hexadecimal_literal
    : '0x' hexadecimal_digit+
    | '0X' hexadecimal_digit+
    ;

hexadecimal_digit
    : '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9'
    | 'a' | 'b' | 'c' | 'd' | 'e' | 'f'
    | 'A' | 'B' | 'C' | 'D' | 'E' | 'F'
    ;

string_literal
    : plain_string_literal
    | interpolated_string_literal
    ;

plain_string_literal
    : '\'' character* '\''
    ;

plain_string_character
    : '<new_line과 \'(U+0027), \\(U+005C)을 제외한 아무 문자>'
    | escape_sequence
    ;

escape_sequence
    : '\\\'' | '\\"' | '\\a' | '\\b' | '\\f'
    | '\\n'  | '\\r' | '\\t' | '\\v' | '\\\\'
    ;
    
interpolated_string_literal
    : '"' interpolated_string_character* '"'
    ;

interpolated_string_character
    : '<new_line과 expression_promoter, \"(U+0022), \\(U+005C)을 제외한 아무 문자>'
    | escape_sequence
    | '\\#'
    | expression_promoter expression '}'
    ;

expression_promoter
    : '#{'
    ;

boolean_literal
    : 'true' | 'false'
    ;

undefined_literal
    : 'undefined'
    ;
```

### Punctuators
문장 부호(Punctuator)는 식(Expression)이나 문(Statement) 등에서 사용되는 문장을 구분하거나 연산자로 사용되는 기호들 입니다.

```antlr
punctuator
    : '('  | ')'  | '{'  | '}'  | '['  | ']'
    | '.'  | ','  | ':'  | '?'  | '='  | ':='
    | '+=' | '->' | '||' | '&&'
    ;
```

## Expressions
식(Expression)은 값을 계산하거나 대상을 참조/호출하는 등 부수적인 효과를 발생시키고 결과값이 발생하는 연산자와 피연산자의 나열입니다.


### Assignment expression
변수에 값을 대입하는 식 입니다. 선언되지 않은 변수에 값을 대입하더라도 BuildScript는 암묵적 변수 선언을 지원하므로 선언되지 않은 변수라면 자동적으로 변수가 생성되며 정의된 변수라면 조건에 따라 값을 대입합니다.

* 대입 연산 : `=` 연산자로 표현하며 값을 대입하는 연산입니다. 기존에 값이 있었을경우 덮어쓰기(Overriding)됩니다.
* 추가 연산 : `+=` 연산자로 표현하며 기존 값의 타입이 리스트라면 값의 추가를, 리스트 타입이 아닐경우 값을 보존한 상태로 리스트 타입으로 변환 한 뒤 추가됩니다. 단, 클로저 타입일경우 런타임 예외가 발생합니다.
* 조건부 대입 연산 : `:=` 연산자로 표현하며 변수가 비어있는 경우(혹은 변수가 정의되지 않은 경우) 값이 대입됩니다.

모든 대입 연산에는 값 조건 식(Value-conditional expression)을 사용 할 수 있습니다. 이 식은 `:` 을 기준으로 왼쪽 식의 반환값이 `undefined`일 경우 오른쪽 식의 값을 반환하고 아닌경우 왼쪽 식의 값을 반환하는 특수 식 입니다.

```antlr
assignment_expression
    : identifier assignment_operator value_conditional_expression?
    ;

assignment_operator
    : '=' | '+=' | ':='
    ;

value_conditional_expression
    : expression (':' expression)?
    ;
```

### Closure expression

```antlr
closure_expression
    : '{' closure_parameter_list? statement_list '}'
    ;

closure_parameter_list
    : parameter_list? '->'
    ;

parameter_list
    : identifier
    : parameter_list ',' identifier
    ;
```

## Statements
스크립트에서 프로그램의 행동은 문(Statement)의 나열로 표현됩니다. C-like 언어들과 비슷한 문법의 형태를 띄고 있지만 일부 키워드가 다르거나 추가적인 기능이 있는 등 문법의 변형이 있습니다.

### Block
블록(Block)은 `{ }` 으로 둘러싸인 0개 이상의 문의 집합입니다. C언어에서는 이를 복합문(Compound statement)으로 일종의 문으로 표현하지만 BuildScript에서는 조건문이나 반복문에 필수적으로 들어가는 문법 요소로 정의하고 있습니다.

```antlr
block
    : '{' statement_list? '}'
    ;
```

### Statement list
문 목록(Statement list)은 1개 이상의 문의 집합입니다. `match`문의 레이블문이나 블록에 사용됩니다.

```antlr
statement_list
    : statement+
    ;

statement
    : if_statement
    | match_statement
    | for_statement
    | while_statement
    | repeat_statement
    | break_statement
    | return_statement
    | raise_statement
    | expression_statement
    ;
```

### `if` statement
조건을 판단할 때 사용되는 문 입니다. 괄호 안의 식의 값에 따라 실행할 문을 결정합니다.

`if`문은 C-like 언어들과 거의 흡사한 형태를 띄고 있습니다. 하지만 BuildScript에서의 다른점은 `elseif` 키워드가 별도로 존재한다는 점 입니다. 이로서 Dangling-else 문제를 해결하고 문법적으로 블록이 사용되는 상황에서 C-like 언어에서의 `else if`를 가능하게 합니다.

제약사항:
* `if`문이나 `elseif`문에 포함되는 `condition`에 사용되는 식은 반드시 논리 타입의 값을 반환해야 합니다. 논리 타입의 값이 아닌 값은 런타임 예외가 발생합니다.

```antlr
if_statement
    : 'if' condition block elseif_statements? else_statement?
    ;

elseif_statements
    : elseif_statement+
    ;

elseif_statement
    : 'elseif' condition block
    ;

else_statement
    : 'else' block
    ;

condition
    : '(' expression ')'
    ;
```

### `match` statement
조건을 판단할 때 사용하는 문 입니다. 괄호 안의 값에 따라 적절한 레이블과 연관된 실행할 문을 결정합니다.

`match`문은 C언어의 `switch`문과 다르게 Fall-through를 허용하지 않습니다. 따라서 레이블의 끝에 `break`문을 작성할 필요가 없습니다.

제약사항:
* 하나의 `match`문에는 1개 이하의 `default` 레이블만이 선언될 수 있습니다.
* `case` 레이블의 값에는 상수만이 허용됩니다.
* `match`문의 식의 반환값은 리스트 타입을 제외한 값만 허용됩니다. 리스트 타입의 값은 런타임 예외가 발생합니다.

```antlr
match_statement
    : 'match' '(' expression ')' match_body
    ;

match_body
    : '{' match_section+ '}'
    ;

match_section
    : match_label+ statement_list
    ;

match_label
    : 'case' constant_value ':'
    : 'default' ':'
    ;
```

### `for` statement
리스트를 순회하는 반복문 입니다. C++, Java의 범위 기반 `for`문이나 C#의 `foreach`문과 흡사하거나 같은 형태입니다.

제약사항:
* `for`문에 사용된 식은 리스트 타입의 값을 반환해야 합니다. 리스트 타입이 아닌 값일경우 원소가 하나인 리스트처럼 행동하지만(경고와 함꼐) 반환값이 `undefined`일경우 런타임 예외가 발생합니다.

```antlr
for_statement
    : 'for' '(' identifier 'in' expression ')' block
    ;
```

### `while` statement
식의 조건이 참일경우일동안 반복하는 반복문 입니다. C-like 언어의 그것과 같습니다.

제약사항:
* `while`문에 사용된 식의 반환값은 논리 타입의 값만 허용됩니다. 논리 타입이 아닌 값이 반환될경우 런타임 예외가 발생합니다.

```antlr
while_statement
    : 'while' condition block
    ;
```

### `repeat` statement

```antlr
repeat_statement
    : 'repeat' block 'until' condition
    ;
```

### `break` statement

```antlr
break_statement
    : 'break' ('?' expression)?
    ;
```

### `return` statement

```antlr
return_statement
    : 'return' expression?
    | 'return' '?' expression (':' expression)?
    ;
```

### `raise` statement

```antlr
raise_statement
    : 'raise' expression
    ;
```

### `import` statement

```antlr
import_statement
    : 'import' expression
    ;
```

### Variable declaration statement
BuildScript는 암묵적 변수 선언을 지원합니다. 하지만 명시적으로 변수를 선언할수도 있습니다. 명시적으로 변수를 선언 할 때 초기값을 대입할 수 있으며 여기에서도 값 조건 식을 사용 할 수 있습니다.

```antlr
variable_declaration_statement
    : 'var' identifier variable_initializer?
    ;

variable_initializer
    : '=' value_conditional_expression?
    ;
```

### Expression statement

## Declarations

### `global` declaration

```antlr
global_declaration
    : 'global' block
    ;
```

### `project` declaration

```antlr
project_declaration
    : 'project' identifier project_body
    ;

project_body
    : '{' project_element* '}'

project_element
    : statement
    | target_declaration
    | task_declaration
    ;
```

### `target` declaration

```antlr
target_declaration
    : 'target' identifier block
    ;
```

### `task` declaration

```antlr
task_declaration
    : 'task' identifier '(' parameter_list? ')' block
    ;
```

## Script
BuildScript의 스크립트는 0개 이상의 선언과 문, `import`문의 집합으로 이루어져 있습니다.

```antlr
script
    : script_element*
    ;

script_element
    : declaration
    : statement
    | import_statement
    ;
```