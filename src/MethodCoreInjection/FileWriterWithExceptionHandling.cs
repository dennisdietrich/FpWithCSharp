using System.Text.Json;

namespace MethodCoreInjection;

internal abstract class FileWriterWithExceptionHandling<T>
{
    internal IExceptionHandler<IOException>? ExceptionHandler { get; init; }

    internal void CreateNew(string filename, T content)
    {
        try
        {
            using var fileStream = new FileStream(filename, FileMode.CreateNew);
            CreateNewImpl(fileStream, content);
            File.SetAttributes(filename, FileAttributes.ReadOnly);
        }
        catch (IOException e) when (ExceptionHandler != null)
        {
            if (ExceptionHandler.Handle(e))
                throw;
        }
    }

    protected abstract void CreateNewImpl(FileStream fileStream, T session);
}

internal sealed class SessionJsonFileWriterWithExceptionHandling : FileWriterWithExceptionHandling<Session>
{
    protected override void CreateNewImpl(FileStream fileStream, Session session) =>
        JsonSerializer.Serialize(fileStream, session);
}

internal sealed class SessionTxtFileWriterWithExceptionHandling : FileWriterWithExceptionHandling<Session>
{
    protected override void CreateNewImpl(FileStream fileStream, Session session)
    {
        using var streamWriter = new StreamWriter(fileStream);
        streamWriter.Write($"{session.Speaker}: {session.Title}");
    }
}