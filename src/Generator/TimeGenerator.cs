namespace Generator
{
    public static class TimeGenerator
    {
        public static IEnumerable<DateTime> CreateEnumerable(DateTime start, int interval)
        {
            if (interval < 1)
                throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be greater than zero.");

            while (true)
            {
                var nextTime = start.AddSeconds(interval);
                yield return start;
                start = nextTime;
            }
        }

        public static Func<DateTime> CreateFunction(DateTime start, int interval)
        {
            if (interval < 1)
                throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be greater than zero.");

            return () =>
            {
                var currentTime = start;
                start = start.AddSeconds(interval);
                return currentTime;
            };
        }
    }
}