namespace Generator.Oop
{
    public static class TimeGenerator
    {
        public static IEnumerable<DateTime> Create(DateTime start, int interval)
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
    }
}