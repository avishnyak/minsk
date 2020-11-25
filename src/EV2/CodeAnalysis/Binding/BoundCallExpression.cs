using System.Collections.Immutable;
using EV2.CodeAnalysis.Symbols;
using EV2.CodeAnalysis.Syntax;

namespace EV2.CodeAnalysis.Binding
{
    internal sealed class BoundCallExpression : BoundExpression
    {
        public BoundCallExpression(SyntaxNode syntax, FunctionSymbol function, ImmutableArray<BoundExpression> arguments)
            : base(syntax)
        {
            Function = function;
            Arguments = arguments;
        }

        public BoundCallExpression(SyntaxNode syntax, BoundVariableExpression instance, FunctionSymbol function, ImmutableArray<BoundExpression> arguments)
            : this (syntax, function, arguments)
        {
            Instance = instance;
        }

        public BoundCallExpression(SyntaxNode syntax, BoundFieldAccessExpression instance, FunctionSymbol function, ImmutableArray<BoundExpression> arguments)
            : this (syntax, function, arguments)
        {
            Instance = instance;
        }

        public override BoundNodeKind Kind => BoundNodeKind.CallExpression;
        public override TypeSymbol Type => Function.ReturnType;
        public BoundExpression? Instance { get; }
        public FunctionSymbol Function { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }
    }
}
