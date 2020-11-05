namespace EV2.CodeAnalysis.Syntax
{
    public sealed partial class MemberAccessExpressionSyntax : ExpressionSyntax
    {
        internal MemberAccessExpressionSyntax(SyntaxTree syntaxTree, ExpressionSyntax expression, SyntaxToken operatorToken, SyntaxToken identifierToken)
            : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
            OperatorToken = operatorToken;
            Expression = expression;
        }

        public override SyntaxKind Kind => SyntaxKind.MemberAccessExpression;
        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Expression { get; }

    }
}