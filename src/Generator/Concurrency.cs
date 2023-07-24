namespace Generator
{
    public static class Concurrency
    {
        public static Func<T> Synchronized<T>(Func<T> func)
        {
            object syncRoot = new();

            return () =>
            {
                lock (syncRoot)
                    return func();
            };
        }
    }
}