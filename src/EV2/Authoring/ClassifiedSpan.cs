using EV2.CodeAnalysis.Text;

namespace EV2.Authoring
{
    public sealed class ClassifiedSpan
    {
        public ClassifiedSpan(TextSpan span, Classification classification)
        {
            Span = span;
            Classification = classification;
        }

        public TextSpan Span { get; }
        public Classification Classification { get; }
    }
}
