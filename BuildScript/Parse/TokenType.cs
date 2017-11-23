﻿/*
 * TokenType.cs
 * author: numver8638(numver8638@naver.com)
 *
 * This file is part of BuildScript.
 *
 * BuildScript is free and unencumbered software released into the public domain.
 * 
 * Anyone is free to copy, modify, publish, use, compile, sell, or
 * distribute this software, either in source code form or as a compiled
 * binary, for any purpose, commercial or non-commercial, and by any
 * means.
 * 
 * In jurisdictions that recognize copyright laws, the author or authors
 * of this software dedicate any and all copyright interest in the
 * software to the public domain. We make this dedication for the benefit
 * of the public at large and to the detriment of our heirs and
 * successors. We intend this dedication to be an overt act of
 * relinquishment in perpetuity of all present and future rights to this
 * software under copyright law.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 * 
 * For more information, please refer to <http://unlicense.org>
 */
namespace BuildScript.Parse
{
    enum TokenType
    {
        Unknown,
        EOF,
        EOL,

        Identifier,        // identifier
        Integer,           // 123, 0x123
        String,            // 'string'
        InterpolatedString,// "#{interpolated} string"

        // Keywords
        Break,             // break
        Case,              // case
        Default,           // default
        Else,              // else
        ElseIf,            // elseif
        False,             // false
        For,               // for
        Global,            // global
        If,                // if
        In,                // in
        Import,            // import
        Match,             // match
        Not,               // not
        Project,           // project
        Raise,             // raise
        Repeat,            // repeat
        Return,            // return
        Target,            // target
        Task,              // task
        True,              // true
        Undefined,         // undefined
        Until,             // until
        Var,               // var
        While,             // while

        // Punctuators
        LeftParen,         // (
        RightParen,        // )
        LeftBrace,         // {
        RightBrace,        // }
        LeftSquare,        // [
        RightSquare,       // ]
        Arrow,             // ->
        Assign,            // =
        Append,            // +=
        ConditionalAssign, // :=
        Equal,             // ==
        NotEqual,          // !=
        LogicalAND,        // &&
        LogicalOR,         // ||
        Dot,               // .
        Comma,             // ,
        Colon,             // :
        ConditionMarker    // ?
    }
}
