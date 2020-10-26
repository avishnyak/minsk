using System.Collections.Immutable;
using EV2.CodeAnalysis.Syntax;

namespace EV2.CodeAnalysis.Binding
{
    internal sealed class BoundMemberBlockStatement : BoundStatement
    {
        public BoundMemberBlockStatement(SyntaxNode syntax, ImmutableArray<BoundStatement> statements)
            : base(syntax)
        {
            Statements = statements;
        }

        public override BoundNodeKind Kind => BoundNodeKind.MemberBlockStatement;

        public ImmutableArray<BoundStatement> Statements { get; }
    }
}
