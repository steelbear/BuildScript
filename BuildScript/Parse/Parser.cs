using System;

using BuildScript.AST;
using BuildScript.Util;

using static BuildScript.Util.Checker;

namespace BuildScript.Parse
{
    public class Parser
    {
        private Lexer lexer;
        private Script script;
        private Token current;

        public Parser(SourceText source)
        {
            CheckNull(source, nameof(source));

            lexer = new Lexer(source);
        }

        public Script ParseScript()
        {
            script = new Script();

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
                ParseScriptElement();
            while (current.Type != TokenType.EOF);
        }

        private void ParseScriptElement()
        {
            
        }
    }
}
