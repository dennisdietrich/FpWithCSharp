namespace Generator
{
    public static class FuncExtensions
    {
        public static Func<T> Synchronized<T>(this Func<T> func)
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