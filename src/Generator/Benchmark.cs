using BenchmarkDotNet.Attributes;

namespace Generator
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        private const int Iterations = 100_000_000;
        private const int Interval = 1;

        private readonly DateTime _startTime = new(2023, 01, 23, 20, 28, 0);
        private IEnumerator<DateTime> _timesEnumerator;
        private Func<DateTime> _timesFunc;

        [IterationSetup]
        public void IterationSetup()
        {
            _timesEnumerator = TimeGenerator.CreateEnumerable(_startTime, Interval).GetEnumerator();
            _timesFunc = TimeGenerator.CreateFunction(_startTime, Interval);
        }

        [IterationCleanup]
        public void IterationCleanup() => _timesEnumerator.Dispose();

        [Benchmark(Baseline = true)]
        public void TimeEnumerator()
        {
            for (var i = 0; i < Iterations; i++)
            {
                _timesEnumerator.MoveNext();
                _ = _timesEnumerator.Current;
            }
        }

        [Benchmark]
        public void TimeFunction()
        {
            for (var i = 0; i < Iterations; i++)
                _ = _timesFunc();
        }
    }
}