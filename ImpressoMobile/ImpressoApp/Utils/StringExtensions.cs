using System;
namespace ImpressoApp.Utils
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string source, string toCheck, StringComparison comp = StringComparison.InvariantCultureIgnoreCase)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

    }
}
