#define FP
#define WITH_EXCEPTION_HANDLING

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
#if METHODS || (FP && !WITH_EXCEPTION_HANDLING)
            WriteToJsonFile("session.json", session);
            WriteToTxtFile("session.txt", session);
#elif FP && WITH_EXCEPTION_HANDLING
            var handler = (IOException e) =>
            {
                Console.WriteLine(e);
                return false;
            };
            var writeToJsonFile = (string n, Session s) => WithExceptionHandler(() => WriteToJsonFile(n, s), handler);
            var writeToTxtFile = (string n, Session s) => WithExceptionHandler(() => WriteToTxtFile(n, s), handler);

            writeToJsonFile("session.json", session);
            writeToTxtFile("session.txt", session);
#endif

#if TEMPLATE_METHOD_PATTERN
#if WITH_EXCEPTION_HANDLING
            var handler = new IOExceptionHandler();

            new JsonFileWriterWithExceptionHandling<Session> { ExceptionHandler =
 handler }.CreateNew("session.json", session);
            new SessionTxtFileWriterWithExceptionHandling { ExceptionHandler =
 handler }.CreateNew("session.txt", session);
#else
            new JsonFileWriter<Session>().CreateNew("session.json", session);
            new SessionTxtFileWriter().CreateNew("session.txt", session);
#endif
#endif
        }

#if FP
        private static void WithExceptionHandler<T>(Action action, Func<T, bool> handler) where T : Exception
        {
            try
            {
                action();
            }
            catch (T e)
            {
                if (handler(e))
                    throw;
            }
        }

        private static void CreateNewFile(string filename, Action<FileStream> action)
        {
            using var fileStream = new FileStream(filename, FileMode.CreateNew);
            action(fileStream);
            File.SetAttributes(filename, FileAttributes.ReadOnly);
        }

        private static void WriteToJsonFile<T>(string filename, T session) =>
            CreateNewFile(filename, s => JsonSerializer.Serialize(s, session));

        private static void WriteToTxtFile(string filename, Session session) =>
            CreateNewFile(filename, s =>
            {
                using var streamWriter = new StreamWriter(s);
                streamWriter.Write($"{session.Speaker}: {session.Title}");
            });
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