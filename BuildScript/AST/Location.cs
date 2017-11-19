namespace BuildScript.AST
{
    public struct Location
    {
        public readonly int Line, Column;

        internal Location(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public override string ToString() => "Line: " + Line + ", Column: " + Column;
    }
}
