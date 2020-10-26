using System.Collections.Immutable;

namespace EV2.CodeAnalysis.Syntax
{
    public sealed partial class MemberBlockStatementSyntax : MemberSyntax
    {
        internal MemberBlockStatementSyntax(SyntaxTree syntaxTree, SyntaxToken openBrace, ImmutableArray<StatementSyntax> statements, SyntaxToken closeBrace)
            : base(syntaxTree)
        {
            OpenBrace = openBrace;
            Statement = statements;
            CloseBrace = closeBrace;
        }

        public override SyntaxKind Kind => SyntaxKind.MemberBlockStatement;

        public SyntaxToken OpenBrace { get; }
        public ImmutableArray<StatementSyntax> Statement { get; }
        public SyntaxToken CloseBrace { get; }
    }
}