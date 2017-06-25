using System;
using System.Linq;

namespace Chihaya.Bot.Services
{
    public static class PreferredKanaTypeExtensions
    {
        public static KanaType ToPreferredKanaType(this string x)
        {
            var name = Enum.GetNames(typeof(KanaType))
                .FirstOrDefault(y => y.Equals(x, StringComparison.OrdinalIgnoreCase));

            return (KanaType)Enum.Parse(typeof(KanaType), name);
        }
    }
}
