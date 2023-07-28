namespace Generator
{
    public interface IGenerator<T>
    {
        public bool GetNext(out T next);
    }
}