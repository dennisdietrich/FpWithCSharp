namespace MethodCoreInjection;

internal sealed class IOExceptionHandler : IExceptionHandler<IOException>
{
    public bool Handle(IOException e)
    {
        Console.WriteLine(e);
        return false;
    }
}