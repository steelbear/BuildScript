<!--
Concepts.md
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

Concepts
=======================================
BuildScript는 [MSBuild](https://docs.microsoft.com/ko-kr/visualstudio/msbuild/msbuild)와 [Gradle](https://gradle.org)의 영향을 받아 제작된 스크립트 기반 빌드 프로그램 입니다. 문법 구조는 C-like 언어들의 문법을 일부 변형하여 설계되었고, 프로젝트나 타겟, 태스크 등의 개념은 MSBuild의 기반으로 작성되었습니다.

BuildScript에서의 핵심 컨셉은 __프로젝트(Project), 타겟(Target), 태스크(Task)__ 입니다.

스크립트는 프로젝트와 타겟, 태스크들의 선언과 문의 집합으로 구성되어있으며 프로젝트는 다시 타겟과 태스크들의 선언, 실행가능한 문들의 집합으로 구성되어있습니다. 

## Globals
전역(Global) 객체는 BuildScript 실행주기동안 스크립트 전역에서 접근할 수 있는 객체입니다. 전역 객체가 선언된 프로젝트(혹은 스크립트)의 하위 프로젝트 전체에서 그 전역객체를 상속받아 사용할 수 있습니다.

전역 객체는 덮어쓰기(Override)가 가능하며 오버라이드된 전역 객체는 오버라이드한 프로젝트(혹은 스크립트)와 그의 하위 프로젝트에만 영향을 미칩니다. 

작성 형태는 다음과 같습니다:
```
global {
    value = 1234
    ...
}
```

전역 객체 내부에서 선언된 변수들은 전역 객체의 프로퍼티 참조로서 이용할 수 있습니다.(위를 예를들면 `global.value` 와 같은 형태) 프로퍼티 참조에 대한 자세한 사항은 [여기](Concepts.md#properties)에서 확인 할 수 있습니다.

전역 객체 블록 안에는 변수 선언 뿐만 아니라 다른 문(Statement)도 올 수 있습니다. 문(Statement)에 대한 자세한 사항은 [여기](Concepts.md#statements)에서 확인 할 수 있습니다.

## Projects
작성 형태는 다음과 같습니다:
```
project ProjectName {
    ...
}
```

## Targets
작성 형태는 다음과 같습니다:
```
target TargetName {
    ...
}
```

## Tasks
작성 형태는 다음과 같습니다:
```
task TaskName(Argument1, Argument2, ...) {
    ...
}
```

## Statements
문(Statement)은 스크립트의 행동을 표현하는 문장입니다. BuildScript에서는 아래 종류의 문이 있습니다.

#### Variable declaration statement
```
var a = 10
var b = c : 20 # c is undefined variable, so 20 is assigned to b.
```

#### `if` statement
```
if (a == b) {
    ...
}
elseif (b == c) {
    ...
}
else {
    ...
}
```

#### `match` statement
```
match (value) {
    case 1:
    case 2:
        print 'one or two'
    
    case 'name':
        print 'what is your name?'

    case undefined:
        print 'value is undefined!'

    default:
        print 'I don't know what it is'
}
```

#### `for` statement
```
for (value in list) {
    print "#{value} is contained in list"
}
```

#### `while` statement
```
while (...) {
    ...
}
```

#### `repeat` statement
```
repeat {
    ...
} until (...)
```

#### `raise` statement
```
if (variable == undefined) {
    raise 'variable is undefined!'
}
```

#### `break` statement
```
while (true) {
    if (GetValue() == 1234) {
        break
    }

    # or can be...
    break? GetValue() == 1234
}
```

#### `return` statement
```
for (value in list) {
    if (value == "value") {
        return value
    }
    
    # or can be...
    return? value == "value" : value
}
```

#### `import` statement
```
import 'script.what.I.wanna.import'
import listOfScripts
```

## Expressions
식(Expression)은 값을 계산하거나 대상을 참조/호출하는 등 부수적인 효과를 발생시키고 결과값이 발생하는 연산자와 피연산자의 나열입니다. BuildScript에서는 아래 종류의 식이 있습니다.

| 범주           | 식           | 설명                                                     |
|----------------|--------------|----------------------------------------------------------|
| Primary        | `x.y`        | 멤버 참조(Member access) 연산                            |
|                | `x()`        | 호출(Invocation) 연산                                    |
| Equality       | `x == y`     | 일치 비교                                                |
|                | `x != y`     | 불일치 비교                                              |
| Relational     | `x in y`     | `x`가 `y`에 포함되어있는지 평가합니다.                   |
|                | `x not in y` | `x`가 `y`에 포함되어있자 않은지 평가합니다.              |
| Logical AND    | `x && y`     | 논리 곱 연산 - `x`가 참일 경우에만 `y`를 평가합니다.     |
| Logical OR     | `x || y`     | 논리 합 연산 - `x`가 거짓일 경우에만  `y` 를 평가합니다. |
| Assignment     | `x op= y`    | 대입 연산 (`=`, `+=`, `:=`)                              |
| Closure        | `{ x -> y }` | 익명 함수 객체(클로저)                                   |

## Variables and properties

### Variables
변수는 값을 읽고 쓰기위해 지정한 메모리 주소의 이름입니다.

BuildScript에서는 변수는 대입시 생성되면서 대입되는 값으로 초기화되거나 변수 선언문에서(대입 초기화를 하지 않을 시) 빈 문자열 타입으로 초기화됩니다.

### Properties
프로퍼티(Property)는 객체의 속성에 접근하게 하는 읽기 전용 변수입니다.

## Types
BuildScript의 타입구조에는 문자열(String), 리스트(List), 논리(Boolean) 그리고 클로저(Closure) 4가지의 타입과 "정의되지 않음"(Undefined)이 존재합니다.

### String
문자열(String) 타입은 문자열을 저장하는 타입입니다. BuildScript내에서 가장 기본적이고 자주 이용되는 타입입니다.

### List
리스트(List) 타입은 문자열들을 저장하는 컨테이너 타입입니다. 리스트에 저장되는 모든 값은 문자열 타입으로 저장됩니다. 단 클로저 타입은 런타임 예외가 발생합니다.(함수를 문자화 할 수 없음)

### Boolean
논리(Boolean) 타입은 참(True)과 거짓(False)를 지정하는 타입입니다. 이와 대응되는 논리 리터럴로 각각 `true`와 `false`가 있습니다. 조건문이나 반복문에서 문의 실행 여부를 판단할 때, 혹은 참 또는 거짓을 표현할 때 사용하는 타입입니다.

### Closure
클로저(Closure) 타입은 함수를 객체화시킨 타입입니다. 태스크의 액션을 정의하거나 몇몇 내장함수 등에서 유용하게 사용됩니다.

클로저 타입은 값을 나타내는 타입이 아니기에 몇가지 제한사항이 있습니다:
* 클로저 타입의 변수는 추가대입 연산이 불가능 합니다.
* TBD

### Undefined
"정의되지 않음"은 변수 혹은 프로퍼티의 멤버 참조시 없는 멤버를 참조할 시 반환되는 값, 혹은 값을 반환하지 않는 클로저나 태스크의 반환값입니다. C언어 계열 언어의 `NULL`(`null`, `nil` 등)과 비슷하지만 javascript의 `undefined`와 더 가깝습니다.

키워드 `undefined`를 통해 정의됩니다.

