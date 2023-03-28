namespace MethodCoreInjection
{
    internal interface IExceptionHandler<in T> where T : Exception
    {
        bool Handle(T e);
    }
}