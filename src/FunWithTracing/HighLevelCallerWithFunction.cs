using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FunWithTracing
{
    public sealed class HighLevelCallerWithFunction
    {
        private readonly LowLevelApiWithFunction _lowLevelApi = new();

        public ILogger Logger { get; set; }

        public void Foo()
        {
            foreach (var file in _lowLevelApi.GetExpiredFiles(Logger.LogTrace))
                ProcessExpiredFile(file);

            foreach (var file in _lowLevelApi.GetExpiredFiles(WriteTrace))
                ProcessExpiredFile(file);
        }

        private static void WriteTrace(string message, params object?[] args) =>
            Trace.WriteLine(string.Format(message, args));

        // Dummy so we have something to call representing further business logic
        private void ProcessExpiredFile(FileInfo info) { }
        // Dummy call to make syntax highlighting happy
        private void Bar() => Foo();
    }
}