﻿using System.Text.Json;

namespace MethodCoreInjection
{
    internal abstract class FileWriter<T>
    {
        internal void CreateNew(string filename, T content)
        {
            using var fileStream = new FileStream(filename, FileMode.CreateNew);
            CreateNewImpl(fileStream, content);
            File.SetAttributes(filename, FileAttributes.ReadOnly);
        }

        protected abstract void CreateNewImpl(FileStream fileStream, T session);
    }

    internal sealed class JsonFileWriter<T> : FileWriter<T>
    {
        protected override void CreateNewImpl(FileStream fileStream, T session) =>
            JsonSerializer.Serialize(fileStream, session);
    }

    internal sealed class SessionTxtFileWriter : FileWriter<Session>
    {
        protected override void CreateNewImpl(FileStream fileStream, Session session)
        {
            using var streamWriter = new StreamWriter(fileStream);
            streamWriter.Write($"{session.Speaker}: {session.Title}");
        }
    }
}