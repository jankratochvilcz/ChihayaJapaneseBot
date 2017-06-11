using System;
using System.Collections.Generic;

namespace Chihaya.Bot.Services
{
    [Serializable]
    public class WordLookupResult
    {
        public string SearchedTerm { get; set; }
        public List<WordLookupResultItem> Results { get; set; }
    }
}
