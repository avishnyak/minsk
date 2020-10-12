using EV2.CodeAnalysis.Syntax;
using EV2.CodeAnalysis.Text;

namespace EV2.CodeAnalysis.Binding
{
    internal sealed class BoundSequencePointStatement : BoundStatement
    {
        public BoundSequencePointStatement(SyntaxNode syntax, BoundStatement statement, TextLocation location)
            : base(syntax)
        {
            Statement = statement;
            Location = location;
        }

        public override BoundNodeKind Kind => BoundNodeKind.SequencePointStatement;

        public BoundStatement Statement { get; }
        public TextLocation Location { get; }
    }
}
