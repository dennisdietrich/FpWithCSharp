using System.Runtime.CompilerServices;

namespace Generator
{
    public static class IEnumeratorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetNext<T>(this IEnumerator<T> enumerator)
        {
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("No more elements in sequence.");

            return enumerator.Current;
        }
    }
}