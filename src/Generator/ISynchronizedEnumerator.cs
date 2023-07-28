namespace Generator
{
    // See also: https://codeblog.jonskeet.uk/2009/10/23/iterating-atomically/
    public interface ISynchronizedEnumerator<T> : IDisposable
    {
        public bool GetNext(out T? nextValue);
    }
}