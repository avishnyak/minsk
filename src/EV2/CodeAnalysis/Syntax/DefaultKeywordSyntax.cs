namespace EV2.CodeAnalysis.Syntax
{
    internal sealed partial class DefaultKeywordSyntax : ExpressionSyntax
    {
        internal DefaultKeywordSyntax(SyntaxTree syntaxTree, SyntaxToken keyword)
            : base(syntaxTree)
        {
            Keyword = keyword;
        }

        public override SyntaxKind Kind => SyntaxKind.DefaultKeyword;

        public SyntaxToken Keyword { get; }
    }
}