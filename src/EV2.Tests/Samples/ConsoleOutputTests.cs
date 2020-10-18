using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EV2.Tests.Snippets
{
    public class ConsoleOutputTests
    {
        [Theory]
        [InlineData("hello")]
        public async Task SamplesTests(string filenamePrefix)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
                WorkingDirectory = Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\samples\", filenamePrefix),
                Arguments = "run",
                FileName = "dotnet"
            };

            var output = new StringBuilder();
            using Process process = Process.Start(psi);
            using ManualResetEvent mreOut = new ManualResetEvent(false), mreErr = new ManualResetEvent(false);

            process.OutputDataReceived += (o, e) => { if (e.Data == null) mreOut.Set(); else output.Append(e.Data); };
            process.BeginOutputReadLine();
            process.ErrorDataReceived += (o, e) => { if (e.Data == null) mreErr.Set(); else output.Append(e.Data); };
            process.BeginErrorReadLine();

            process.StandardInput.Close();
            process.WaitForExit();

            mreOut.WaitOne();
            mreErr.WaitOne();


            // Compare stdout to outputfile
            var outputPath = Path.GetFullPath(Path.Combine( @"..\..\..\..\samples", filenamePrefix, filenamePrefix + ".out"));

            Assert.Equal(await File.ReadAllTextAsync(outputPath), output.ToString());
        }
    }
}
