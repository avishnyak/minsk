using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace EV2.EV2LanguageServer
{
    internal class MyDocumentSymbolHandler : DocumentSymbolHandler
    {
        private readonly LspHost _lspHost;

        public MyDocumentSymbolHandler(LspHost lspHost) : base(
            new DocumentSymbolRegistrationOptions {
                DocumentSelector = DocumentSelector.ForLanguage("ev2")
            }
        )
        {
            _lspHost = lspHost;
        }

        public override async Task<SymbolInformationOrDocumentSymbolContainer> Handle( DocumentSymbolParams request, CancellationToken cancellationToken)
        {
            var symbols = new List<SymbolInformationOrDocumentSymbol>();

            return symbols;
        }
    }
}