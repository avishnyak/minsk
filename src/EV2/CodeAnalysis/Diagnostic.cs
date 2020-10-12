using System;
using System.IO;
using EV2.CodeAnalysis.Text;
using EV2.Host;
using Range = EV2.Host.Range;

namespace EV2.CodeAnalysis
{
    public sealed class Diagnostic : IDiagnostic
    {
        private DiagnosticLocation? _diagnosticLocation;

        private Diagnostic(bool isError, TextLocation location, string message)
        {
            IsError = isError;
            Location = location;
            Message = message;
            IsWarning = !IsError;
        }

        public bool IsError { get; }
        public TextLocation Location { get; }
        public string Message { get; }
        public bool IsWarning { get; }

        public DiagnosticLocation DiagnosticLocation {
            get {
                // Lazy initialize external location
                if (_diagnosticLocation == null) {
                    var start = new Position(Location.StartLine + 1, Location.StartCharacter + 1);
                    var end = new Position(Location.EndLine + 1, Location.EndCharacter + 1);
                    var range = new Range(start, end);

                    _diagnosticLocation = new DiagnosticLocation(new Uri(Path.GetFullPath(Location.FileName)), range);
                }

                return _diagnosticLocation;
            }
        }

        public string? ContextSourceSnippet {
            get {
                var start = Location.Text.Lines[Location.StartLine].Start;
                var end = Location.Text.Lines[Location.EndLine].End;

                return Location.Text.ToString(start, end - start);
            }
        }
        public string? TargetSourceSnippet {
            get {
                return Location.Text.ToString(Location.Span.Start, Location.Span.End - Location.Span.Start);
            }
        }

        public override string ToString() => Message;

        public static Diagnostic Error(TextLocation location, string message)
        {
            return new Diagnostic(isError: true, location, message);
        }

        public static Diagnostic Warning(TextLocation location, string message)
        {
            return new Diagnostic(isError: false, location, message);
        }
    }
}