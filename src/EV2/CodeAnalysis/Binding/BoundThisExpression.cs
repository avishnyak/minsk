using EV2.CodeAnalysis.Symbols;
using EV2.CodeAnalysis.Syntax;

namespace EV2.CodeAnalysis.Binding
{

    internal sealed class BoundThisExpression : BoundExpression
    {
        public BoundThisExpression(SyntaxNode syntax, StructSymbol instance)
            : base(syntax)
        {
            Instance = instance;
        }

        public override TypeSymbol Type => Instance;
        public override BoundNodeKind Kind => BoundNodeKind.ThisExpression;
        public StructSymbol Instance { get; }
    }
}
