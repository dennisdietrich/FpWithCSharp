namespace Generator
{
    public class TimeGenerator : IGenerator<DateTime>
    {
        private readonly int _interval;
        private DateTime _current;

        public TimeGenerator(DateTime start, int interval)
        {
            if (interval < 1)
                throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be greater than zero.");

            _current = start;
            _interval = interval;
        }

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

        public bool TryGetNext(out DateTime next)
        {
            var current = _current;
            _current = _current.AddSeconds(_interval);
            next = current;
            return true;
        }
    }
}