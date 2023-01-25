namespace Generator
{
    public interface IGenerator<T>
    {
        public bool TryGetNext(out T next);
    }
}