/*
 * Parser.cs
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
using System;

using BuildScript.AST;
using BuildScript.Util;

using static BuildScript.Util.Checker;

namespace BuildScript.Parse
{
    public partial class Parser
    {
        private Lexer lexer;
        private AST.Script script;
        private Token current;

        public Parser(SourceText source)
        {
            CheckNull(source, nameof(source));

            lexer = new Lexer(source);
        }

        public AST.Script ParseScript()
        {
            script = new AST.Script();

            ParseScriptRoot();

            return script;
        }

        private Location ConsumeToken() => (current = lexer.LexToken()).Location;

        private void ExpectToken(TokenType expected, string message)
        {
            if (current.Type != expected)
                throw new ParserException(current.Location, message);

            ConsumeToken();
        }

        private bool ConsumeIfDesired(TokenType desired)
        {
            if (current.Type == desired)
            {
                ConsumeToken();
                return true;
            }
            else
                return false;
        }

        private void SkipNewLine()
        {
            while (current.Type == TokenType.EOL || current.Type == TokenType.EOF) ConsumeToken();
        }

        private void ParseScriptRoot()
        {
            ConsumeToken(); // load first token

            do
            {
                // ParseScriptElement();
                Console.WriteLine(current);
                ConsumeToken();

           } while (current.Type != TokenType.EOF);
        }

        /*
         * script_element
         *     : declaration
         *     | import_statement
         *     | statement
         *     ;
         */
        private void ParseScriptElement()
        {
            switch (current.Type)
            {
                /*
                 * Declarations
                 */
                case TokenType.Project:
                    break;

                case TokenType.Global:
                    break;

                case TokenType.Target:

                    break;

                case TokenType.Task:

                    break;

                /*
                 * Import statement
                 */
                case TokenType.Import:

                    break;

                /*
                 * Statements
                 */
                default:

                    break;
            }
        }
    }
}
