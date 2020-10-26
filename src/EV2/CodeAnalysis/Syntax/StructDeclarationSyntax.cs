namespace EV2.CodeAnalysis.Syntax
{
    public sealed partial class StructDeclarationSyntax : MemberSyntax
    {
        internal StructDeclarationSyntax(SyntaxTree syntaxTree, SyntaxToken structKeyword, SyntaxToken identifier, MemberBlockStatementSyntax body)
        : base(syntaxTree)
        {
            StructKeyword = structKeyword;
            Identifier = identifier;
            Body = body;
        }

        public override SyntaxKind Kind => SyntaxKind.StructDeclaration;

        public SyntaxToken StructKeyword { get; }
        public SyntaxToken Identifier { get; }
        public MemberBlockStatementSyntax Body { get; }
    }
}