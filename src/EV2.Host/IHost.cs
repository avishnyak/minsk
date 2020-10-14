using System;
using System.Collections.Generic;
using System.Threading;

namespace EV2.Host
{
    /// <summary>
    /// A common interface that allows the compiler to communicate to the host.
    ///
    /// All methods must be implemented.
    /// </summary>
    public interface IHost : IDisposable
    {
        /// <Summary>Request the host to restart the compiler service.</Summary>
        void RequestShutdown();

        /// <Summary>Notify host of new diagnostic messages.</Summary>
        void PublishDiagnostics(IEnumerable<IDiagnostic> diagnostics, CancellationToken cancellationToken = default);
    }
}
