using Microsoft.Extensions.Logging;

namespace FunWithTracing
{
    public sealed class LowLevelApiWithLogger
    {
        public TimeSpan DefaultTtl { get; set; }

        public IList<FileInfo> GetExpiredFiles(ILogger? logger = null) =>
            GetAllFiles().Where(f =>
            {
                var effectiveTtl = EffectiveTtl(f);
                var lastAccess = File.GetLastAccessTimeUtc(f.FullName);
                var isExpired = lastAccess + effectiveTtl < DateTime.UtcNow;
                logger?.LogTrace("File {0} with access time {1} and TTL {2} has {3}expired", f.FullName, lastAccess, effectiveTtl, isExpired ? "" : "not ");
                return isExpired;
            }).ToList();

        // Dummies so we have something to call representing further business logic
        private IEnumerable<FileInfo> GetAllFiles() => Enumerable.Empty<FileInfo>();
        private TimeSpan EffectiveTtl(FileInfo file) => DefaultTtl;
    }
}