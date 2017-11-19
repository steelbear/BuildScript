using System;

using BuildScript.AST;

namespace BuildScript.Parse
{
    public class ParserException : Exception
    {
        private Location location;
        
        public ParserException(Location location, string message)
            : base(message)
        {
            this.location = location;
        }

        public Location Location
        {
            get => location;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1} - {2}", Location.Line, Location.Column, Message);
        }
    }
}
