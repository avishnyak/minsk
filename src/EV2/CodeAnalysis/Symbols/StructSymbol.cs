using System.Collections.Immutable;
using EV2.CodeAnalysis.Syntax;

namespace EV2.CodeAnalysis.Symbols
{
    public sealed class StructSymbol : TypeSymbol
    {
        internal StructSymbol(string name, ImmutableArray<ParameterSymbol> ctorParameters, ImmutableArray<VariableSymbol> members, StructDeclarationSyntax? declaration = null) : base(name, null)
        {
            Declaration = declaration;
            CtorParameters = ctorParameters;
            Members = members;
        }

        public override SymbolKind Kind => SymbolKind.Struct;

        public StructDeclarationSyntax? Declaration { get; }
        public ImmutableArray<ParameterSymbol> CtorParameters { get; }
        public ImmutableArray<VariableSymbol> Members { get; }
    }
}