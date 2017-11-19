namespace BuildScript.AST
{
    public interface INode
    {
        Location Location { get; }
        void Dump();
    }
}
