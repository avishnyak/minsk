namespace EV2.CodeAnalysis.Syntax
{
    internal sealed partial class ThisKeywordSyntax : ExpressionSyntax
    {
        internal ThisKeywordSyntax(SyntaxTree syntaxTree, SyntaxToken keyword)
            : base(syntaxTree)
        {
            Keyword = keyword;
        }

        public override SyntaxKind Kind => SyntaxKind.ThisKeyword;

        public SyntaxToken Keyword { get; }
    }
}