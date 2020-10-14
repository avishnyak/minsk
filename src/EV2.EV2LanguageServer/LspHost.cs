using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EV2.Host;
using System.Linq;
using EV2.CompilerService;
using System.Threading;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Microsoft.Extensions.Logging;
using Uri = OmniSharp.Extensions.LanguageServer.Protocol.DocumentUri;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;

namespace EV2.EV2LanguageServer
{
    internal sealed class LspHost : IHost
    {
        private readonly Uri? _workerSpaceRoot;
        private readonly int _maxNumberOfProblems = 1000;
        private readonly IDictionary<Uri, TextDocumentItem> _documents;
        private readonly Server _server;
        private readonly ILogger<LspHost> _logger;
        private readonly ILanguageServerFacade _languageServer;

        public LspHost(ILogger<LspHost> logger, ILanguageServerFacade languageServer)
        {
            _documents = new Dictionary<Uri, TextDocumentItem>();
            _server = new Server(this);
            _logger = logger;
            _languageServer = languageServer;
        }

        internal void DidOpenTextDocument(TextDocumentItem document)
        {
            _logger.LogInformation($"Document opened {document.Uri.GetFileSystemPath()} ({document.Version})");
            _documents.Add(document.Uri, document);
        }

        internal void DidChangeTextDocument(VersionedTextDocumentIdentifier document, Container<TextDocumentContentChangeEvent> changes)
        {
            _logger.LogInformation($"Document changed {document.Uri.GetFileSystemPath()} ({document.Version})");

            Uri docUri = document.Uri;
            if (!_documents.ContainsKey(docUri))
            {
                return;
            }

            var doc = _documents[docUri];

            // We only handle a full document update right now
            doc.Version = document.Version;
            doc.Text = changes.First().Text;
        }

        internal void DidCloseTextDocument(TextDocumentItem document)
        {
            _logger.LogInformation($"Document closed {document.Uri.GetFileSystemPath()} ({document.Version})");

            Uri docUri = document.Uri;
            if (_documents.ContainsKey(docUri))
            {
                _documents.Remove(docUri);
            }
        }

        internal async Task<IEnumerable<IDiagnostic>> ValidateTextDocumentAsync(Uri docUri,
                                                                                CancellationToken cancellationToken)
        {
            if (!_documents.ContainsKey(docUri))
            {
                _logger.LogError("File is not open for validation.");
                return Array.Empty<IDiagnostic>();
            }

            var doc = _documents[docUri];

            return await _server.Validate(doc.Text, doc.Uri.GetFileSystemPath(), cancellationToken);
        }

        // void ValidateTextDocument(TextDocumentItem document)
        // {
        //     _logger.LogInformation($"Validating {document.Uri.GetFileSystemPath()} ({document.Version})");

        //     var validationTask = Task.Run(() => _server.Validate(document.text, document.uri.ToString(), CancellationToken), CancellationToken);

        //     try
        //     {
        //         var diagnostics = validationTask.Result;

        //         if (diagnostics.Count() > 0)
        //         {
        //             Logger.Instance.Info($"Found {diagnostics.Count()} issues.");
        //         }
        //     }
        //     catch (AggregateException ae)
        //     {
        //         Logger.Instance.Error(ae.ToString());
        //     }

        //     // TODO: Save syntax tree in LRU cache
        // }

        public void RequestShutdown()
        {
            _logger.LogInformation("Compiler requested shutdown.");
        }

        public void ClearDiagnotics(Uri documentUri)
        {
            _languageServer.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams()
            {
                Uri = documentUri,
                Diagnostics = Array.Empty<Diagnostic>()
            });
        }

        public void PublishDiagnostics(IEnumerable<IDiagnostic> diagnostics, CancellationToken cancellationToken)
        {
            var groupedDiagnostics = diagnostics.GroupBy(k => k.DiagnosticLocation.Uri);

            foreach (var kv in groupedDiagnostics)
            {
                _languageServer.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams
                {
                    Uri = kv.Key,
                    Diagnostics = kv.Select(d => new Diagnostic()
                    {
                        Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                            new OmniSharp.Extensions.LanguageServer.Protocol.Models.Position(d.DiagnosticLocation.Range.Start.Line, d.DiagnosticLocation.Range.Start.Character),
                            new OmniSharp.Extensions.LanguageServer.Protocol.Models.Position(d.DiagnosticLocation.Range.End.Line, d.DiagnosticLocation.Range.End.Character)
                        ),
                        Severity = d.IsError ? DiagnosticSeverity.Error : DiagnosticSeverity.Warning,
                        Source = "EV2",
                        Message = d.Message
                    }).ToArray()
                });

                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        public void Dispose()
        {
        }
    }
}