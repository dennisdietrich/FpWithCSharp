using System.Text.Json;

namespace MethodCoreInjection
{
    internal abstract class SessionFile
    {
        internal void CreateNew(string filename, Session session)
        {
            using var fileStream = new FileStream(filename, FileMode.CreateNew);
            CreateNewImpl(fileStream, session);
            File.SetAttributes(filename, FileAttributes.ReadOnly);
        }

        protected abstract void CreateNewImpl(FileStream fileStream, Session session);
    }

    internal class SessionJsonFile : SessionFile
    {
        protected override void CreateNewImpl(FileStream fileStream, Session session) =>
            JsonSerializer.Serialize(fileStream, session);
    }

    internal class SessionTxtFile : SessionFile
    {
        protected override void CreateNewImpl(FileStream fileStream, Session session)
        {
            using var streamWriter = new StreamWriter(fileStream);
            streamWriter.Write($"{session.Speaker}: {session.Title}");
        }
    }
}