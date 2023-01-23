#define OOP
#define FP

using Generator;
using Generator.Oop;

const int iterations = 5;
const int interval = 3;
var startTime = new DateTime(2023, 01, 23, 20, 28, 0);

#if OOP
Console.WriteLine($"Generating {iterations} times with {interval} seconds interval using object-oriented implementation...");

using var times = TimeGenerator.Create(startTime, interval).GetEnumerator();
for (var i = 0; i < iterations; i++)
{
    times.MoveNext();
    Console.WriteLine($"{times.Current:u}");
    //Console.WriteLine($"{times.GetNext():u}");
}

Console.WriteLine();
#endif

#if FP
Console.WriteLine($"Generating {iterations} times with {interval} seconds interval using functional implementation...");
#endif

#if !(OOP || FP)
Console.WriteLine("Nothing to do...");
#endif