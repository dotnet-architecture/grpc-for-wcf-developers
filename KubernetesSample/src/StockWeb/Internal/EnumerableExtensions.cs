using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StockWeb.Internal
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ExcludeNulls<T>(this IEnumerable<T> source) where T : class => source.Where(x => !(x is null));
    }
}