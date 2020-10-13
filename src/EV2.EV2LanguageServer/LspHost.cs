using LanguageServer;
using LanguageServer.Client;
using LanguageServer.Parameters.General;
using LanguageServer.Parameters.TextDocument;
using LanguageServer.Parameters.Workspace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EV2.Host;
using System.Linq;
using EV2.CompilerService;

namespace EV2.EV2LanguageServer
{
    public class App : ServiceConnection, IHost
    {
        private Uri? _workerSpaceRoot;
        private int _maxNumberOfProblems = 1000;
        private readonly TextDocumentManager _documents;
        private readonly Server _server;

        public App(Stream input, Stream output)
            : base(input, output)
        {
            _documents = new TextDocumentManager();
            _documents.Changed += Documents_Changed;
            _server = new Server(this);
        }

        private void Documents_Changed(object? sender, TextDocumentChangedEventArgs e)
        {
            ValidateTextDocument(e.Document);
        }

        protected override Result<InitializeResult, ResponseError<InitializeErrorData>> Initialize(InitializeParams @params)
        {
            _workerSpaceRoot = @params.rootUri;
            var result = new InitializeResult
            {
                capabilities = new ServerCapabilities
                {
                    textDocumentSync = TextDocumentSyncKind.Full,
                    // documentSymbolProvider = true
                }
            };
            return Result<InitializeResult, ResponseError<InitializeErrorData>>.Success(result);
        }

        // protected override Result<DocumentSymbolResult, ResponseError> DocumentSymbols(DocumentSymbolParams @params)
        // {
        //     throw new NotImplementedException();
        // }

        protected override void DidOpenTextDocument(DidOpenTextDocumentParams @params)
        {
            _documents.Add(@params.textDocument);
            Logger.Instance.Log($"{@params.textDocument.uri} opened.");
        }

        protected override void DidChangeTextDocument(DidChangeTextDocumentParams @params)
        {
            _documents.Change(@params.textDocument.uri, @params.textDocument.version, @params.contentChanges);
            Logger.Instance.Log($"{@params.textDocument.uri} changed.");
        }

        protected override void DidCloseTextDocument(DidCloseTextDocumentParams @params)
        {
            _documents.Remove(@params.textDocument.uri);
            Logger.Instance.Log($"{@params.textDocument.uri} closed.");
        }

        protected override void DidChangeConfiguration(DidChangeConfigurationParams @params)
        {
            _maxNumberOfProblems = @params?.settings?.languageServerExample?.maxNumberOfProblems ?? _maxNumberOfProblems;
            Logger.Instance.Log($"maxNumberOfProblems is set to {_maxNumberOfProblems}.");

            foreach (var document in _documents.All)
            {
                ValidateTextDocument(document);
            }
        }

        private void ValidateTextDocument(TextDocumentItem document)
        {
            Logger.Instance.Log($"Validating file: {document.uri} v{document.version}");

            // Clear existing diagnostics for the file
            Proxy.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams()
            {
                uri = document.uri,
                diagnostics = new Diagnostic[] { }
            });

            var diagnostics = _server.Validate(document.text, document.uri.ToString());

            if (diagnostics.Count() > 0)
            {
                Logger.Instance.Log($"Found {diagnostics.Count()} issues.");
            }

            // TODO: Save syntax tree in LRU cache
        }

        protected override void DidChangeWatchedFiles(DidChangeWatchedFilesParams @params)
        {
            Logger.Instance.Log("We received an file change event");
        }

        protected override VoidResult<ResponseError> Shutdown()
        {
            Logger.Instance.Log("Language Server is about to shutdown.");
            // WORKAROUND: Language Server does not receive an exit notification.
            Task.Delay(1000).ContinueWith(_ => Environment.Exit(0));
            return VoidResult<ResponseError>.Success();
        }

        public void RequestShutdown()
        {
            Logger.Instance.Log("Compiler requested shutdown");
        }

        public void PublishDiagnostics(IEnumerable<IDiagnostic> diagnostics)
        {
            var groupedDiagnostics = diagnostics.GroupBy(k => k.DiagnosticLocation.Uri);

            foreach (var kv in groupedDiagnostics)
            {
                Proxy.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams
                {
                    uri = kv.Key,
                    diagnostics = kv.Select(d => new Diagnostic()
                    {
                        range = new LanguageServer.Parameters.Range()
                        {
                            start = new LanguageServer.Parameters.Position() { line = d.DiagnosticLocation.Range.Start.Line, character = d.DiagnosticLocation.Range.Start.Character },
                            end = new LanguageServer.Parameters.Position() { line = d.DiagnosticLocation.Range.End.Line, character = d.DiagnosticLocation.Range.End.Character }
                        },
                        severity = d.IsError ? DiagnosticSeverity.Error : DiagnosticSeverity.Warning,
                        source = "EV2",
                        message = d.Message
                    }).ToArray()
                });
            }
        }

        public void Dispose()
        {
        }
    }
}