using System.Text.Json;

namespace MethodCoreInjection;

internal abstract class SessionFileWithExceptionHandling
{
    internal void CreateNew(string filename, Session session)
    {
        try
        {
            using var fileStream = new FileStream(filename, FileMode.CreateNew);
            CreateNewImpl(fileStream, session);
            File.SetAttributes(filename, FileAttributes.ReadOnly);
        }
        catch (IOException e) when (this is IExceptionHandler<IOException>)
        {
            if (((IExceptionHandler<IOException>)this).Handle(e))
                throw;
        }
    }

    protected abstract void CreateNewImpl(FileStream fileStream, Session session);
}

internal class SessionJsonFileWithExceptionHandling : SessionFileWithExceptionHandling, IExceptionHandler<IOException>
{
    protected override void CreateNewImpl(FileStream fileStream, Session session) =>
        JsonSerializer.Serialize(fileStream, session);

    bool IExceptionHandler<IOException>.Handle(IOException e)
    {
        Console.WriteLine(e);
        return false;
    }
}

internal class SessionTxtFileWithExceptionHandling : SessionFileWithExceptionHandling, IExceptionHandler<IOException>
{
    protected override void CreateNewImpl(FileStream fileStream, Session session)
    {
        using var streamWriter = new StreamWriter(fileStream);
        streamWriter.Write($"{session.Speaker}: {session.Title}");
    }

    bool IExceptionHandler<IOException>.Handle(IOException e)
    {
        Console.WriteLine(e);
        return false;
    }
}