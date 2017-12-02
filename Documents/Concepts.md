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
BuildScript는 [MSBuild](https://docs.microsoft.com/ko-kr/visualstudio/msbuild/msbuild)와 [Gradle](https://gradle.org)의 영향을 받아 제작된 스크립트 기반 빌드 프로그램 입니다.

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

전역 객체 내부에서 선언된 변수들은 전역 객체의 프로퍼티 참조로서 이용할 수 있습니다.(위로 예를들면 `global.value` 와 같은 형태) 프로퍼티 참조에 대한 자세한 사항은 [여기](Concepts.md#properties)에서 확인 할 수 있습니다.

전역 객체 블록 안에는 변수 선언 뿐만 아니라 다른 문(Statement)도 올 수 있습니다. 문(Statement)에 대한 자세한 사항은 [여기](Concepts.md#statements)에서 확인 할 수 있습니다.

## Projects
TBD

## Targets
타겟(Target)은 빌드에 필요한 일련의 작업(태스크)들을 그룹화하고 빌드 과정을 정의하는 작은 단위입니다. 타겟도 일종의 객체로서 취급되며 블록 내의 내용이 타겟 객체를 설정하는 역할을 합니다.

작성 형태는 다음과 같습니다:
```
target TargetName {
    ...
}
```

### 의존성 설정
타겟이 실행되기 위해 사전에 먼저 실행되어야할 타겟이 있을 수 있습니다. 이런 경우에는 `dependsOn` 메서드로 타겟간 의존성 설정이 가능합니다.

작성 형태는 다음과 같습니다:
```
target Target {
    ...
    dependsOn ['FirstTarget', 'SecondTarget']
    ...
}
```

<!--
### 입력 파일 설정
소스코드를 컴파일하는 것처럼 의존성이 다른 타겟이 아니라 파일에 의존성이 있을 수 있습니다. 이런 경우에는 `input` 메서드로 파일 의존성을 설정할 수 있습니다. 

작성 형태는 다음과 같습니다:
```
target Target {
    ...
    input ['source1.c', 'source2.c', 'source3.c'] # 개별 파일로 입력하는 경우
    input project.sources.endsWith('.cpp')        # 다른 변수/프로퍼티로 입력하는 경우
    ...
}
```

### 액션(Action) 설정

-->
## Tasks
태스크(Task)는 빌드 과정에 필요한 작업(일)을 정의합니다. 사용자가 다른 태스크들을 이용하여 새로운 태스크를 정의할 수 있으며, BuildScript내 `ITask` 인터페이스를 구현함으로서도 새로운 태스크를 정의할 수 있습니다.

`ITask` 인터페이스를 이용하여 새로운 태스크를 구현하는 방법에 대해 자세한 사항은 [여기](#)에서 확인 할 수 있습니다.

작성 형태는 다음과 같습니다:
```
task TaskName(Argument1, Argument2, ...) {
    ...
}
```

`importTasks` 메서드를 이용하여 태스크를 구현하는 어셈블리를 로드하여 사용할 수 있습니다. 첫번째 인자는 태스크를 구현하는 어셈블리의 경로이며 두번째 인자는 태스크를 구현하는 네임스페이스의 리스트입니다. BuildScript는 네임스페이스를 검색하여 `ITask`를 구현하는 클래스를 발견하면 그 클래스를 사용가능한 태스크의 목록에 포함시킵니다.
```
importTasks("MyTasks.dll", ["MyNamespace.MyTasks", "MyNamespace2.AnotherTasks"])
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
    print "${value} is contained in list"
}
```

#### `while` statement
```
while (...) {
    ...
}
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
| Logical OR     | `x \|\| y`     | 논리 합 연산 - `x`가 거짓일 경우에만  `y` 를 평가합니다. |
| Assignment     | `x op= y`    | 대입 연산 (`=`, `+=`, `:=`)                              |
| Closure        | `{ x -> y }` | 익명 함수 객체(클로저)                                   |

## Variables and properties

### Variables
변수는 값을 읽고 쓰기위해 지정한 저장 공간의 이름입니다.

BuildScript에서는 변수는 대입시 생성되면서 대입되는 값으로 초기화되거나 변수 선언문에서(대입 초기화를 하지 않을 시) 빈 리스트 타입으로 초기화됩니다.

### Properties
TBD

## Types
BuildScript의 타입구조에는 문자열(String), 리스트(List), 논리(Boolean) 그리고 클로저(Closure) 4가지의 타입과 "정의되지 않음"(Undefined)이 존재합니다.

### String
문자열(String) 타입은 문자열을 저장하는 타입입니다. BuildScript내에서 가장 기본적이고 자주 이용되는 타입입니다.

### List
리스트(List) 타입은 문자열들을 저장하는 컨테이너 타입입니다. 리스트에 저장되는 모든 값은 문자열 타입으로 저장됩니다. 단, 클로저 타입은 제외됩니다.

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

