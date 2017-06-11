using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chihaya.Bot.Services.Japanese.Entities.JishoOrg
{
    public class SearchResults
    {
        [JsonProperty(PropertyName = "data")]
        public List<SearchResult> Results { get; set; }
    }
}
