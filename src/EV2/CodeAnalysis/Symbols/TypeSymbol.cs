namespace EV2.CodeAnalysis.Symbols
{
    public class TypeSymbol  : Symbol
    {
        public static readonly TypeSymbol Error = new TypeSymbol("?", null);
        public static readonly TypeSymbol Any = new TypeSymbol("any", null);
        public static readonly TypeSymbol Bool = new TypeSymbol("bool", false);
        public static readonly TypeSymbol Int = new TypeSymbol("int", 0);
        public static readonly TypeSymbol String = new TypeSymbol("string", string.Empty);
        public static readonly TypeSymbol Void = new TypeSymbol("void", null);

        internal TypeSymbol(string name, object? defaultValue)
            : base(name)
        {
            DefaultValue = defaultValue;
        }

        public override SymbolKind Kind => SymbolKind.Type;

        public object? DefaultValue { get; }
    }
}