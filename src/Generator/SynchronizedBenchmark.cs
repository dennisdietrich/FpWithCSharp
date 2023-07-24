using BenchmarkDotNet.Attributes;
using static Generator.Concurrency;

namespace Generator
{
    [MemoryDiagnoser]
    public class SynchronizedBenchmark
    {
        private const int Iterations = 20_000_000;
        private const int Interval = 1;

        private readonly DateTime _startTime = new(2023, 01, 23, 20, 28, 0);
        private ISynchronizedEnumerator<DateTime> _timesEnumerator;
        private Func<DateTime> _timesFunc;

        [IterationSetup]
        public void IterationSetup()
        {
            _timesEnumerator = TimeGenerator.CreateEnumerable(_startTime, Interval).GetSynchronizedEnumerator();
            _timesFunc = Synchronized(TimeGenerator.CreateFunction(_startTime, Interval));
        }

        [IterationCleanup]
        public void IterationCleanup() => _timesEnumerator.Dispose();

        [Benchmark(Baseline = true)]
        public void SyncTimeEnumerator()
        {
            for (var i = 0; i < Iterations; i++)
                _timesEnumerator.GetNext(out _);
        }

        [Benchmark]
        public void SyncTimeFunction()
        {
            for (var i = 0; i < Iterations; i++)
                _ = _timesFunc();
        }
    }
}