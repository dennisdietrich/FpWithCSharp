#define LINQ

namespace FunWithTracing
{
    public sealed class LowLevelApiNoTracing
    {
        public TimeSpan DefaultTtl { get; set; }

#if LINQ
        public IList<FileInfo> GetExpiredFiles() =>
            GetAllFiles().Where(f =>
            {
                var effectiveTtl = EffectiveTtl(f);
                var lastAccess = File.GetLastAccessTimeUtc(f.FullName);
                var isExpired = lastAccess + effectiveTtl < DateTime.UtcNow;
                // What if we want to trace here?
                return isExpired;
            }).ToList();
#else
        public IList<FileInfo> GetExpiredFiles()
        {
            List<FileInfo> expiredFiles = new();

            foreach (var file in GetAllFiles())
            {
                // What if we want to trace here?
                if (File.GetLastAccessTimeUtc(file.FullName) + EffectiveTtl(file) < DateTime.UtcNow)
                    expiredFiles.Add(file);
            }

            return expiredFiles;
        }
#endif

        // Dummies so we have something to call representing further business logic
        private IEnumerable<FileInfo> GetAllFiles() => Enumerable.Empty<FileInfo>();
        private TimeSpan EffectiveTtl(FileInfo file) => DefaultTtl;
    }
}