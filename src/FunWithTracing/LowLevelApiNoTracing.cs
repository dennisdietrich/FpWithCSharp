namespace FunWithTracing
{
    public sealed class LowLevelApiNoTracing
    {
        public TimeSpan DefaultTtl { get; set; }

        public IList<FileInfo> GetExpiredFiles()
        {
            List<FileInfo> expiredFiles = new();

            foreach (var file in GetAllFiles())
            {
                var effectiveTtl = EffectiveTtl(file);
                var lastAccess = File.GetLastAccessTimeUtc(file.FullName);
                var isExpired = lastAccess + effectiveTtl < DateTime.UtcNow;
                // What if we want to trace here?
                if (isExpired)
                    expiredFiles.Add(file);
            }

            return expiredFiles;
        }

        // Dummies so we have something to call representing further business logic
        private IEnumerable<FileInfo> GetAllFiles() => Enumerable.Empty<FileInfo>();
        private TimeSpan EffectiveTtl(FileInfo file) => DefaultTtl;
    }
}