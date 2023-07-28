using Microsoft.Extensions.Logging;

namespace FunWithTracing
{
    public delegate void Logger(string message, params object?[] args);

    public sealed class LowLevelApiWithFunction
    {
        public TimeSpan DefaultTtl { get; set; }

        public IList<FileInfo> GetExpiredFiles(Logger? logger = null) =>
            GetAllFiles().Where(f =>
            {
                var effectiveTtl = EffectiveTtl(f);
                var lastAccess = File.GetLastAccessTimeUtc(f.FullName);
                var isExpired = lastAccess + effectiveTtl < DateTime.UtcNow;
                logger?.Invoke("File {0} with access time {1} and TTL {2} has {3}expired", f.FullName, lastAccess, effectiveTtl, isExpired ? "" : "not ");
                return isExpired;
            }).ToList();

        // Dummies so we have something to call representing further business logic
        private IEnumerable<FileInfo> GetAllFiles() => Enumerable.Empty<FileInfo>();
        private TimeSpan EffectiveTtl(FileInfo file) => DefaultTtl;
    }
}