using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EV2.CodeAnalysis;
using EV2.CodeAnalysis.Syntax;
using EV2.Host;

namespace EV2.CompilerService {
    public class Server
    {
        private readonly IHost _host;
        private bool _shutDownRequested = false;
        private bool _isExiting = false;

        public Server(IHost host)
        {
            _host = host;
        }

        /// <summary>
        /// Initialize the compiler service
        /// </summary>
        /// <param name="cancellationToken">Cancel initialization</param>
        /// <returns>Task</returns>
        public Task Initialize(CancellationToken cancellationToken = default) {
            return Task.CompletedTask;
        }

        public IEnumerable<Diagnostic> Validate(string source, string sourcePath)
        {
            var tree = SyntaxTree.Parse(source, sourcePath);

            if (tree.Diagnostics.Count() > 0)
            {
                _host.PublishDiagnostics(tree.Diagnostics);
            }

            var program = Compilation.Create(tree);
            var diagnostics = program.Validate();

            if (diagnostics.Count() > 0)
            {
                _host.PublishDiagnostics(diagnostics);
            }

            return tree.Diagnostics.Concat(diagnostics);
        }

        public async Task<IEnumerable<SyntaxTree>> Parse(IList<string> sourcePaths,
                                                         CancellationToken cancellationToken = default)
        {
            var syntaxTrees = new ConcurrentBag<SyntaxTree>();

            // Use ParallelOptions instance to store the CancellationToken
            ParallelOptions po = new ParallelOptions()
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            // If we are compiling a small amount of files, favor sequential processing.
            if (sourcePaths.Count < Environment.ProcessorCount) {
                foreach (var path in sourcePaths)
                {
                    var syntaxTree = SyntaxTree.Load(path);
                    syntaxTrees.Add(syntaxTree);
                }
            }
            else
            {
                // Load files in parallel
                try
                {
                    Parallel.ForEach(sourcePaths, po, (path) =>
                    {
                        var syntaxTree = SyntaxTree.Load(path);
                        syntaxTrees.Add(syntaxTree);
                        po.CancellationToken.ThrowIfCancellationRequested();
                    });

                }
                catch (OperationCanceledException e)
                {
                    await Console.Error.WriteLineAsync("Compilation cancelled.");
                    await Console.Error.WriteLineAsync(e.InnerException?.ToString());

                    throw;
                }
            }

            foreach (var tree in syntaxTrees)
            {
                if (tree.Diagnostics.Count() > 0)
                {
                    _host.PublishDiagnostics(tree.Diagnostics);
                }
            }

            return syntaxTrees;
        }

        public bool EmitBinary(IEnumerable<SyntaxTree> syntaxTrees,
                         string moduleName,
                         string[] referencePaths,
                         string outputPath)
        {
            var compilation = Compilation.Create(syntaxTrees.ToArray());

            if (compilation == null) {
                return false;
            }

            var diagnostics = compilation.Emit(moduleName, referencePaths, outputPath);

            if (diagnostics.Count() > 0)
            {
                _host.PublishDiagnostics(diagnostics);
            }

            return true;
        }

        /// <summary>
        /// Request that the compiler service start it's shutdown routine.
        ///
        /// Must be called before Exit
        /// </summary>
        /// <param name="cancellationToken">Cancel shutdown</param>
        /// <returns>Task</returns>
        public Task Shutdown(CancellationToken cancellationToken = default) {
            _shutDownRequested = true;

            return Task.CompletedTask;
        }

        /// <summary>
        /// A notification to ask the server to exit its process.
        /// </summary>
        /// <returns>0 on success, 1 if the exit request has already been received before.</returns>
        public int Exit() {
            if (!_shutDownRequested || _isExiting) {
                return 1;
            }

            _isExiting = true;

            return 0;
        }
    }
}
