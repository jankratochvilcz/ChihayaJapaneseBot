using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chihaya.Bot
{
    public static class StringExtensions
    {
        public static string NormalizeUtterance(this string utterance)
            => utterance?
                .Trim()
                .ToLowerInvariant()
                ?? string.Empty;
    }
}
