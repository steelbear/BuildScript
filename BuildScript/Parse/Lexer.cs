/*
 * Lexer.cs
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
using System.Collections.Generic;
using System.Diagnostics;

using BuildScript.AST;
using BuildScript.Util;

namespace BuildScript.Parse
{
    class Lexer
    {
        private static readonly Dictionary<string, TokenType> Keywords =
            new Dictionary<string, TokenType>()
            {
                { "break",     TokenType.Break     },
                { "case",      TokenType.Case      },
                { "default",   TokenType.Default   },
                { "else",      TokenType.Else      },
                { "elseif",    TokenType.ElseIf    },
                { "false",     TokenType.False     },
                { "for",       TokenType.For       },
                { "global",    TokenType.Global    },
                { "if",        TokenType.If        },
                { "import",    TokenType.Import    },
                { "in",        TokenType.In        },
                { "match",     TokenType.Match     },
                { "not",       TokenType.Not       },
                { "raise",     TokenType.Raise     },
                { "return",    TokenType.Return    },
                { "target",    TokenType.Target    },
                { "task",      TokenType.Task      },
                { "true",      TokenType.True      },
                { "undefined", TokenType.Undefined },
                { "var",       TokenType.Var       },
                { "while",     TokenType.While     }
            };

        private SourceText source;
        private int line = 1;
        private int cursor;
        private int lineStart;
        private bool isNewLine = false;

        internal Lexer(SourceText source)
        {
            Debug.Assert(source != null);

            this.source = source;
        }

        internal Token LexToken()
        {
            if (isNewLine)
            {
                ++line;
                lineStart = cursor;
                isNewLine = false;
            }

            SkipWhitespace();

            // Ignore comments
            if (AdvanceIfDesired('#'))
            {
                SkipUntilEOL();
            }

            if (source.Length <= cursor)
            {
                return new Token(default(Location), TokenType.EOF);
            }

            char ch = source[cursor];

            if (IsDecimal(ch)) return LexInteger();

            if (IsIdentifier(ch)) return LexIdentifierOrKeyword();

            /* else */ return LexPunctuator();
        }

        private Token LexInteger()
        {
            var location = GetLocation();
            var start = cursor++;
            string image;

            if (source[start] == '0')
            {
                if (AdvanceIfDesired('x') || AdvanceIfDesired('X'))
                {
                    while (source.Length > cursor && IsHexadecimal(source[cursor])) ++cursor;

                    image = source.GetString(start, cursor - start);
                    return new Token(location, TokenType.Integer, image);
                }
            }

            while (source.Length > cursor && IsDecimal(source[cursor])) ++cursor;

            image = source.GetString(start, cursor - start);
            return new Token(location, TokenType.Integer, image);
        }

        private Token LexIdentifierOrKeyword()
        {
            var location = GetLocation();
            var start = cursor++;

            while (source.Length > cursor && IsIdentifier(source[cursor])) ++cursor;

            var image = source.GetString(start, cursor - start);
            
            if (!Keywords.TryGetValue(image, out TokenType type))
            {
                type = TokenType.Identifier;
            }

            return new Token(location, type, image);
        }

        private Token LexString(char startChar)
        {
            var location = GetLocation();
            var type = startChar == '"' ? TokenType.InterpolatedString : TokenType.String;
            var start = cursor;

            do
            {
                ++cursor;

                if (source.Length <= cursor || IsEOL(source[cursor]))
                    return new Token(GetLocation(), TokenType.Unknown);

            } while (source[cursor] != startChar);

            var image = source.GetString(start, cursor - start);
            ++cursor; // eat startChar

            return new Token(location, type, image);
        }

        private Token LexPunctuator()
        {
            var location = GetLocation();
            var ch = source[cursor++ /* accept current character */];
            var type = TokenType.Undefined;

            switch (ch)
            {
                case '\r':
                    AdvanceIfDesired('\n');
                    isNewLine = true;
                    type = TokenType.EOL;
                    break;

                case '\n':
                    isNewLine = true;
                    type = TokenType.EOL;
                    break;

                case '(':
                    type = TokenType.LeftParen;
                    break;

                case ')':
                    type = TokenType.RightParen;
                    break;

                case '{':
                    type = TokenType.LeftBrace;
                    break;

                case '}':
                    type = TokenType.RightBrace;
                    break;

                case '[':
                    type = TokenType.LeftSquare;
                    break;

                case ']':
                    type = TokenType.RightSquare;
                    break;

                case '.':
                    type = TokenType.Dot;
                    break;

                case ',':
                    type = TokenType.Comma;
                    break;

                case '?':
                    type = TokenType.ConditionMarker;
                    break;

                case '=':
                    if (AdvanceIfDesired('=')) type = TokenType.Equal;
                    else type = TokenType.Assign;
                    break;

                case '+':
                    if (AdvanceIfDesired('=')) type = TokenType.Append;
                    // else type = TokenType.Undefined;
                    break;

                case '-':
                    if (AdvanceIfDesired('>')) type = TokenType.Arrow;
                    // else type = TokenType.Undefined;
                    break;

                case ':':
                    if (AdvanceIfDesired('=')) type = TokenType.ConditionalAssign;
                    else type = TokenType.Colon;
                    break;

                case '!':
                    if (AdvanceIfDesired('=')) type = TokenType.NotEqual;
                    // else type = TokenType.Unknown
                    break;

                case '&':
                    if (AdvanceIfDesired('&')) type = TokenType.LogicalAND;
                    // else type = TokenType.Unknown
                    break;

                case '|':
                    if (AdvanceIfDesired('|')) type = TokenType.LogicalOR;
                    // else type = TokenType.Unknown
                    break;

                case '\'':
                case '"':
                    return LexString(ch);

                default:
                    // type = TokenType.Unknown
                    break;
            }

            return new Token(location, type);
        }

        private bool AdvanceIfDesired(char desired)
        {
            if (source.Length > cursor && source[cursor] == desired)
            {
                ++cursor;
                return true;
            }

            return false;
        }

        private Location GetLocation() => new Location(line, (cursor - lineStart) + 1);

        private void SkipWhitespace()
        {
            while (source.Length > cursor && IsWhitespace(source[cursor])) cursor++;
        }

        private void SkipUntilEOL()
        {
            char ch = source[cursor];

            while ((source.Length > cursor) && IsEOL(ch))
            {
                ch = source[++cursor];
            }
        }

        private static bool IsWhitespace(char ch) => (ch == ' ') || (ch == '\t');

        private static bool IsDecimal(char ch) => ('0' <= ch) && (ch <= '9');

        private static bool IsHexadecimal(char ch) => ('a' <= ch && ch <= 'f') || ('A' <= ch && ch <= 'F') || IsDecimal(ch);

        private static bool IsIdentifier(char ch) 
            => ('a' <= ch && ch <= 'z') || ('A' <= ch && ch <= 'Z') || (ch == '_') || (ch == '$') || IsDecimal(ch);

        private static bool IsEOL(char ch) => (ch == '\r') || (ch == '\n');
    }
}
