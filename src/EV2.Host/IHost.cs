using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EV2.Host
{
    public interface IHost : IDisposable
    {
        /// <Summary>Request the host to restart the compiler service.</Summary>
        void RequestShutdown();

        /// <Summary>Notify host of new diagnostic messages.</Summary>
        void PublishDiagnostics(IEnumerable<IDiagnostic> diagnostics);
    }
}
