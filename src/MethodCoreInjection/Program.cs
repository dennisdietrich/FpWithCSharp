#define METHODS

// Term "Method-Core Injection" coined by Ann Lewkowicz
// http://www.annlewkowicz.com/2022/12/method-core-injection-c-pattern-for.html

using System.Text.Json;

namespace MethodCoreInjection
{
    public sealed record Session(string Speaker, string Title);

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
#if WITH_EXCEPTION_HANDLING
            var handler = new IOExceptionHandler();

            new SessionJsonFileWriterWithExceptionHandling { ExceptionHandler = handler }.CreateNew("session.json", session);
            new SessionTxtFileWriterWithExceptionHandling { ExceptionHandler = handler }.CreateNew("session.txt", session);
#else
            new SessionJsonFileWriter().CreateNew("session.json", session);
            new SessionTxtFileWriter().CreateNew("session.txt", session);
#endif
#endif
        }

#if FP
#if WITH_EXCEPTION_HANDLING
        private static void CreateNewFile(string filename, Action<FileStream> action, Func<IOException, bool>? handler = null)
        {
            try
            {
                using var fileStream = new FileStream(filename, FileMode.CreateNew);
                action(fileStream);
                File.SetAttributes(filename, FileAttributes.ReadOnly);
            }
            catch (IOException e) when (handler != null)
            {
                if (handler(e))
                    throw;
            }
        }

        private static void WriteToJsonFile(string filename, Session session) =>
            CreateNewFile(
                filename,
                s => JsonSerializer.Serialize(s, session),
                e =>
                {
                    Console.WriteLine(e);
                    return false;
                });

        private static void WriteToTxtFile(string filename, Session session) =>
            CreateNewFile(
                filename,
                s =>
                {
                    using var streamWriter = new StreamWriter(s);
                    streamWriter.Write($"{session.Speaker}: {session.Title}");
                },
                e =>
                {
                    Console.WriteLine(e);
                    return false;
                });
#else
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
#endif
#endif

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