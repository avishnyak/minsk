using EV2.CodeAnalysis.Symbols;
using EV2.CodeAnalysis.Syntax;

namespace EV2.CodeAnalysis.Binding
{
    internal sealed class BoundLocalFieldAccessExpression : BoundExpression
    {
        public BoundLocalFieldAccessExpression(SyntaxNode syntax, StructSymbol instance, VariableSymbol structMember)
            : base(syntax)
        {
            Instance = instance;
            StructMember = structMember;
        }

        public override TypeSymbol Type => StructMember.Type;
        public override BoundNodeKind Kind => BoundNodeKind.ThisExpression;
        public StructSymbol Instance { get; }
        public VariableSymbol StructMember { get; }
    }
}
