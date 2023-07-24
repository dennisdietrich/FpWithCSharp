namespace Generator
{
    public static class IGeneratorExtensions
    {
        public static IGenerator<T> Synchronized<T>(this IGenerator<T> generator) => new SynchronizedGenerator<T>(generator);

        private class SynchronizedGenerator<T> : IGenerator<T>
        {
            private readonly IGenerator<T> _target;
            private readonly object _syncRoot = new();

            internal SynchronizedGenerator(IGenerator<T> target) => _target = target;

            public bool GetNext(out T next)
            {
                lock (_syncRoot)
                    return _target.GetNext(out next);
            }
        }
    }
}