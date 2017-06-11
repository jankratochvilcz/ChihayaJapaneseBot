using System.Collections.Generic;

namespace Chihaya.Bot.Services
{
    public class WordLookupResult
    {
        public string SearchedTerm { get; set; }
        public List<WordLookupResultItem> Results { get; set; }
    }
}
