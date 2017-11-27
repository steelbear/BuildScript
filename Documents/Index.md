<!--
Index.md
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

BuildScript Language Specifications
=======================================
_v0.1_

이 문서는 BuildScript의 개념, API, 문법 구조 등을 설명합니다. 아직 초안이므로 문서의 내용은 __언제든지__ 변경될 수 있습니다.

이 문서를 작성하기 위해 C나 C#의 draft문서를 예시삼아 작성하였습니다. 그렇기에 비슷한부분이 많을 수 있습니다. 문제될 시 수정하겠습니다.

To write this specification, C and C# draft specifications are referenced as an example and it helped a lot. So you possibily find many similar parts(e.g document layout) in document. If it makes any problem, I'll correct the problem.

Index
---------------------------------------
* [Concepts](Concepts.md)
    * [Globals](Concepts.md#globals)
    * [Projects](Concepts.md#projects)
    * [Targets](Concepts.md#targets)
    * [Tasks](Concepts.md#tasks)
    * [Statements](Concepts.md#statements)
    * [Expressions](Concepts.md#expressions)
    * [Variables and properties](Concepts.md#variables-and-properties)
    * [Types](Concepts.md#types)
* [Grammars](Grammars.md)
    * [Lexical elements](Grammars.md#lexical-elements)
    * [Statements](Grammars.md#statements)
    * [Expressions](Grammars.md#expressions)
* [APIs](Apis.md)

