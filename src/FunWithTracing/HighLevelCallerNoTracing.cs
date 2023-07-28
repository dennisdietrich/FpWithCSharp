namespace FunWithTracing
{
    public sealed class HighLevelCallerNoTracing
    {
        private readonly LowLevelApiNoTracing _lowLevelApi = new();

        public void Foo()
        {
            foreach (var file in _lowLevelApi.GetExpiredFiles())
                ProcessExpiredFile(file);
        }

        // Dummy so we have something to call representing further business logic
        private void ProcessExpiredFile(FileInfo info) { }
        // Dummy call to make syntax highlighting happy
        private void Bar() => Foo();
    }
}