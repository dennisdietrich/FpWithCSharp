using Microsoft.Extensions.Logging;

namespace FunWithTracing
{
    public sealed class HighLevelCallerWithLogger
    {
        private readonly LowLevelApiWithLogger _lowLevelApi = new();

        public ILogger Logger { get; set; }

        public void Foo()
        {
            foreach (var file in _lowLevelApi.GetExpiredFiles(Logger))
                ProcessExpiredFile(file);
        }

        // Dummy so we have something to call representing further business logic
        private void ProcessExpiredFile(FileInfo info) { }
        // Dummy call to make syntax highlighting happy
        private void Bar() => Foo();
    }
}