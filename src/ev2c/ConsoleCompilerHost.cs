using System;
using System.Collections.Generic;
using System.Linq;
using EV2.Host;
using EV2.IO;

namespace EV2
{
    class ConsoleCompilerHost : IHost, IDisposable
    {
        public int Errors = 0;
        public int Warnings = 0;

        public void Dispose()
        {
        }

        public void PublishDiagnostics(IEnumerable<IDiagnostic> diagnostics)
        {
            Errors += diagnostics.Count(d => d.IsError);
            Warnings += diagnostics.Count(d => d.IsWarning);

            Console.Error.WriteDiagnostics(diagnostics);
        }

        public void RequestShutdown()
        {
            Console.Error.WriteLine("Compiler crashed");
            Environment.Exit(1);
        }
    }
}