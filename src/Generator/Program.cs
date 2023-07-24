#define ENUMERATOR
#define CUSTOM_GENERATOR
#define FP
#define BENCHMARK
#define SYNCHRONIZED

using BenchmarkDotNet.Running;
using Generator;
using static Generator.Concurrency;

const int iterations = 5;
const int interval = 3;
var startTime = new DateTime(2023, 01, 23, 20, 28, 0);

#if ENUMERATOR
{
    Console.WriteLine($"Generating {iterations} times with {interval} seconds interval using generator implementation...");

#if SYNCHRONIZED
    using ISynchronizedEnumerator<DateTime> times = TimeGenerator.CreateEnumerable(startTime, interval).GetSynchronizedEnumerator();
    for (var i = 0; i < iterations; i++)
    {
        times.GetNext(out DateTime time);
        Console.WriteLine($"{time:u}");
    }
#else
    using IEnumerator<DateTime> times = TimeGenerator.CreateEnumerable(startTime, interval).GetEnumerator();
    for (var i = 0; i < iterations; i++)
    {
        times.MoveNext();
        Console.WriteLine($"{times.Current:u}");
        //Console.WriteLine($"{times.GetNext():u}");
    }
#endif

    Console.WriteLine();
}
#endif

#if CUSTOM_GENERATOR
{
    Console.WriteLine($"Generating {iterations} times with {interval} seconds interval using custom generator implementation...");

#if SYNCHRONIZED
    IGenerator<DateTime> times = new TimeGenerator(startTime, interval).Synchronized();
    for (var i = 0; i < iterations; i++)
    {
        times.GetNext(out DateTime time);
        Console.WriteLine($"{time:u}");
    }
#else
    IGenerator<DateTime> times = new TimeGenerator(startTime, interval);
    for (var i = 0; i < iterations; i++)
    {
        times.GetNext(out DateTime time);
        Console.WriteLine($"{time:u}");
    }
#endif

    Console.WriteLine();
}
#endif

#if FP
{
    Console.WriteLine($"Generating {iterations} times with {interval} seconds interval using functional implementation...");

#if SYNCHRONIZED
    Func<DateTime> times = Synchronized(TimeGenerator.CreateFunction(startTime, interval));
    for (var i = 0; i < iterations; i++)
        Console.WriteLine($"{times():u}");
#else
    Func<DateTime> times = TimeGenerator.CreateFunction(startTime, interval);
    for (var i = 0; i < iterations; i++)
        Console.WriteLine($"{times():u}");
#endif

    Console.WriteLine();
}
#endif

#if BENCHMARK
#if SYNCHRONIZED
BenchmarkRunner.Run<SynchronizedBenchmark>();
#else
BenchmarkRunner.Run<Benchmark>();
#endif
#endif

#if !(ENUMERATOR || CUSTOM_GENERATOR || FP || BENCHMARK)
Console.WriteLine("Nothing to do...");
#endif