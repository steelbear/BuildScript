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
                { "in",        TokenType.In        },
                { "match",     TokenType.Match     },
                { "not",       TokenType.Not       },
                { "project",   TokenType.Project   },
                { "raise",     TokenType.Raise     },
                { "repeat",    TokenType.Repeat    },
                { "return",    TokenType.Return    },
                { "target",    TokenType.Target    },
                { "task",      TokenType.Task      },
                { "true",      TokenType.True      },
                { "undefined", TokenType.Undefined },
                { "until",     TokenType.Until     },
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
            Retry:
            if (isNewLine)
            {
                ++line;
                lineStart = cursor;
                isNewLine = false;
            }

            SkipWhitespace();

            if (source.Length <= cursor)
            {
                return new Token(default(Location), TokenType.EOF);
            }

            char ch = source[cursor];

            if (IsDecimal(ch)) return LexInteger();

            if (IsIdentifier(ch)) return LexIdentifierOrKeyword();

            var location = GetLocation();
            var type = TokenType.Unknown;
            ++cursor; // eat character

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

                case '*':
                    type = TokenType.Multiply;
                    break;

                case '/':
                    type = TokenType.Divide;
                    break;

                case '%':
                    type = TokenType.Remainder;
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

                case '<':
                    if (AdvanceIfDesired('=')) type = TokenType.LessOrEqual;
                    else type = TokenType.Less;
                    break;

                case '>':
                    if (AdvanceIfDesired('=')) type = TokenType.GraterOrEqual;
                    else type = TokenType.Grater;
                    break;

                case '+':
                    if (AdvanceIfDesired('=')) type = TokenType.Append;
                    else type = TokenType.Plus;
                    break;

                case '-':
                    if (AdvanceIfDesired('>')) type = TokenType.Arrow;
                    else type = TokenType.Minus;
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

                case '#': // comment
                    SkipUntilEOL();
                    goto Retry;

                default:
                    // type = TokenType.Unknown
                    break;
            }

            return new Token(location, type);
        }

        private Token LexInteger()
        {
            var location = GetLocation();
            var start = cursor++;

            while (source.Length > cursor && IsDecimal(source[cursor])) ++cursor;

            return new Token(location, TokenType.Integer, source.GetString(start, cursor - start));
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

        private bool AdvanceIfDesired(char desired)
        {
            if (source[cursor] == desired)
            {
                ++cursor;
                return true;
            }

            return false;
        }

        private Location GetLocation()
        {
            return new Location(line, (cursor - lineStart) + 1);
        }

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

        private static bool IsIdentifier(char ch) 
            => ('a' <= ch && ch <= 'z') || ('A' <= ch && ch <= 'Z') || (ch == '_') || (ch == '$') || IsDecimal(ch);

        private static bool IsEOL(char ch) => (ch == '\r') || (ch == '\n');
    }
}
