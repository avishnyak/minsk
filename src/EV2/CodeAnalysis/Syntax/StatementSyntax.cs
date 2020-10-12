namespace EV2.CodeAnalysis.Syntax
{
    public abstract class StatementSyntax : SyntaxNode
    {
        private protected StatementSyntax(SyntaxTree syntaxTree)
            : base(syntaxTree)
        {
        }
    }
}