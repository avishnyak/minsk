using System.Collections.Immutable;
using EV2.CodeAnalysis.Syntax;

namespace EV2.CodeAnalysis.Symbols
{
    public sealed class FunctionSymbol : Symbol
    {
        internal FunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameters, TypeSymbol type, FunctionDeclarationSyntax? declaration = null, FunctionSymbol? overloadFor = null)
            : base(name)
        {
            Parameters = parameters;
            Type = type;
            Declaration = declaration;
            OverloadFor = overloadFor;
        }

        public override SymbolKind Kind => SymbolKind.Function;
        public FunctionDeclarationSyntax? Declaration { get; }
        public ImmutableArray<ParameterSymbol> Parameters { get; }
        public TypeSymbol Type { get; }
        public FunctionSymbol? OverloadFor { get; }
    }
}