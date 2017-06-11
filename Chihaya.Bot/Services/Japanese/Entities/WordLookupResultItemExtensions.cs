using System.Linq;

namespace Chihaya.Bot.Services
{
    public static class WordLookupResultItemExtensions
    {
        public static string EnglishDefinitionsToString(this WordLookupResultItem x)
            => x.EnglishDefinitions.Aggregate(string.Empty, (accDefs, currDef) => $"{accDefs}{(accDefs == string.Empty ? "" : " - ")}{currDef}");
    }
}
