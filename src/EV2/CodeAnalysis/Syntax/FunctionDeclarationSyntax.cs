namespace EV2.CodeAnalysis.Syntax
{
    public sealed partial class FunctionDeclarationSyntax : MemberSyntax
    {
        internal FunctionDeclarationSyntax(
                SyntaxTree syntaxTree,
                SyntaxToken functionKeyword,
                SyntaxToken? receiver,
                SyntaxToken? dotToken,
                SyntaxToken identifier,
                SyntaxToken openParenthesisToken,
                SeparatedSyntaxList<ParameterSyntax> parameters,
                SyntaxToken closeParenthesisToken,
                TypeClauseSyntax? type,
                BlockStatementSyntax body
            ) : base(syntaxTree)
        {
            FunctionKeyword = functionKeyword;
            Receiver = receiver;
            DotToken = dotToken;
            Identifier = identifier;
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
            Type = type;
            Body = body;
        }

        public override SyntaxKind Kind => SyntaxKind.FunctionDeclaration;

        public SyntaxToken FunctionKeyword { get; }
        public SyntaxToken? Receiver { get; }
        public SyntaxToken? DotToken { get; }
        public SyntaxToken Identifier { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public SyntaxToken CloseParenthesisToken { get; }
        public TypeClauseSyntax? Type { get; }
        public BlockStatementSyntax Body { get; }
    }
}