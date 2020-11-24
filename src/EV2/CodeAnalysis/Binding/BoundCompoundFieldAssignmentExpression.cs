using EV2.CodeAnalysis.Symbols;
using EV2.CodeAnalysis.Syntax;

namespace EV2.CodeAnalysis.Binding
{
    internal sealed class BoundCompoundFieldAssignmentExpression : BoundExpression
    {
        public BoundCompoundFieldAssignmentExpression(SyntaxNode syntax, BoundExpression structInstance, VariableSymbol structMember, BoundBinaryOperator op, BoundExpression expression)
            : base(syntax)
        {
            StructInstance = structInstance;
            StructMember = structMember;
            Op = op;
            Expression = expression;
        }

        public override BoundNodeKind Kind => BoundNodeKind.CompoundFieldAssignmentExpression;
        public override TypeSymbol Type => Expression.Type;

        public BoundExpression StructInstance { get; }
        public VariableSymbol StructMember { get; }
        public BoundBinaryOperator Op {get; }
        public BoundExpression Expression { get; }
    }
}
