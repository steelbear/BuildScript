using BuildScript.AST;

namespace BuildScript.Parse
{
    struct Token
    {
        private Location location;
        private TokenType type;
        private string image;
        
        internal Token(Location location, TokenType type, string image = "")
        {
            this.location = location;
            this.type = type;
            this.image = image;
        }

        public Location Location
        {
            get => location;
        }

        public TokenType Type
        {
            get => type;
        }

        public string Image
        {
            get => image;
        }

        public override string ToString() => "TokenType: " + type + ", Image: " + image + ", Location: { " + location + " }";
    }
}
