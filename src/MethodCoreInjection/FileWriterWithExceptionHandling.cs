using System.Text.Json;

namespace MethodCoreInjection;

internal abstract class FileWriterWithExceptionHandling<TCont, TEx> where TEx : Exception
{
    internal IExceptionHandler<TEx>? ExceptionHandler { get; init; }

    internal void CreateNew(string filename, TCont content)
    {
        try
        {
            using var fileStream = new FileStream(filename, FileMode.CreateNew);
            CreateNewImpl(fileStream, content);
            File.SetAttributes(filename, FileAttributes.ReadOnly);
        }
        catch (TEx e) when (ExceptionHandler != null)
        {
            if (ExceptionHandler.Handle(e))
                throw;
        }
    }

    protected abstract void CreateNewImpl(FileStream fileStream, TCont session);
}

internal sealed class JsonFileWriterWithExceptionHandling<TCont, TEx> : FileWriterWithExceptionHandling<TCont, TEx> where TEx : Exception
{
    protected override void CreateNewImpl(FileStream fileStream, TCont session) =>
        JsonSerializer.Serialize(fileStream, session);
}

internal sealed class SessionTxtFileWriterWithExceptionHandling<TEx> : FileWriterWithExceptionHandling<Session, TEx> where TEx : Exception
{
    protected override void CreateNewImpl(FileStream fileStream, Session session)
    {
        using var streamWriter = new StreamWriter(fileStream);
        streamWriter.Write($"{session.Speaker}: {session.Title}");
    }
}