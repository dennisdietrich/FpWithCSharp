#define FP

// Term "Method-Core Injection" coined by Ann Lewkowicz
// http://www.annlewkowicz.com/2022/12/method-core-injection-c-pattern-for.html

using System.Text.Json;

namespace MethodCoreInjection
{
    public record Session(string Speaker, string Title);

    internal static class Program
    {
        internal static void Main()
        {
            var session = new Session("Dennis Dietrich", "So you think you know functions");
#if METHODS || FP
            WriteToJsonFile("session.json", session);
            WriteToTxtFile("session.txt", session);
#endif
#if TEMPLATE_METHOD_PATTERN
            new SessionJsonFile().CreateNew("session.json", session);
            new SessionTxtFile().CreateNew("session.txt", session);
#endif
        }

        private static void CreateNewFile(string filename, Action<FileStream> action)
        {
            using var fileStream = new FileStream(filename, FileMode.CreateNew);
            action(fileStream);
            File.SetAttributes(filename, FileAttributes.ReadOnly);
        }

        private static void WriteToJsonFile(string filename, Session session) =>
            CreateNewFile(filename, s => JsonSerializer.Serialize(s, session));

        private static void WriteToTxtFile(string filename, Session session) =>
            CreateNewFile(filename, s =>
            {
                using var streamWriter = new StreamWriter(s);
                streamWriter.Write($"{session.Speaker}: {session.Title}");
            });

#if METHODS
        private static void WriteToJsonFile(string filename, Session session)
        {
            using var fileStream = new FileStream(filename, FileMode.CreateNew);
            JsonSerializer.Serialize(fileStream, session);
            File.SetAttributes(filename, FileAttributes.ReadOnly);
        }

        private static void WriteToTxtFile(string filename, Session session)
        {
            using var fileStream = new FileStream(filename, FileMode.CreateNew);
            using var streamWriter = new StreamWriter(fileStream);
            streamWriter.Write($"{session.Speaker}: {session.Title}");
            File.SetAttributes(filename, FileAttributes.ReadOnly);
        }
#endif
    }
}