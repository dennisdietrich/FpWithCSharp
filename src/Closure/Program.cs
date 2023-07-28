#define PURE
#define IMPURE

#if IMPURE
(Func<int> Inc, Func<int> Dec) CreateIncDecFuncPair(int increment)
{
    int val = 0;

    return (() => val += increment,
            () => val -= increment);
}

var funcs = CreateIncDecFuncPair(3);

Console.WriteLine($"Calling funcs.Inc(), new value is: {funcs.Inc()}");
Console.WriteLine($"Calling funcs.Inc(), new value is: {funcs.Inc()}");
Console.WriteLine($"Calling funcs.Inc(), new value is: {funcs.Inc()}");
Console.WriteLine($"Calling funcs.Dec(), new value is: {funcs.Dec()}");

Console.WriteLine();
#endif

#if PURE
var logins = Array.Empty<Login>();
//var last24Hours = logins.Where(l => l.Timestamp >= DateTime.Now.AddDays(-1));

var minTimestamp = DateTime.Now.AddDays(-1);
var last24Hours = logins.Where(l => l.Timestamp >= minTimestamp);

sealed record Login(DateTime Timestamp, string Username, bool Successful);
#endif

#if !(IMPURE || PURE)
Console.WriteLine("Nothing to do...");
#endif