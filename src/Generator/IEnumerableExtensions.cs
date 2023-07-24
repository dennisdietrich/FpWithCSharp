namespace Generator
{
    public static class IEnumerableExtensions
    {
        public static ISynchronizedEnumerator<T> GetSynchronizedEnumerator<T>(this IEnumerable<T> enumerable) => new SynchronizedEnumerator<T>(enumerable);

        // See also: https://codeblog.jonskeet.uk/2009/10/23/iterating-atomically/
        private sealed class SynchronizedEnumerator<T> : ISynchronizedEnumerator<T>
        {
            private readonly object _syncRoot = new();
            private readonly IEnumerator<T> _enumerator;

            internal SynchronizedEnumerator(IEnumerable<T> enumerable) => _enumerator = enumerable.GetEnumerator();

            public bool GetNext(out T? nextValue)
            {
                lock (_syncRoot)
                {
                    var valueAvailable = _enumerator.MoveNext();
                    nextValue = valueAvailable ? _enumerator.Current : default;
                    return valueAvailable;
                }
            }

            public void Dispose() => _enumerator.Dispose();
        }
    }
}