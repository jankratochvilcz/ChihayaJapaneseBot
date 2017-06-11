using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chihaya.Bot.Services.Japanese.Entities.JishoOrg
{
    public class SearchResult
    {
        [JsonProperty(PropertyName = "is_common")]
        public bool IsCommon { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags { get; set; }

        [JsonProperty(PropertyName = "japanese")]
        public List<JapaneseWord> Japanese { get; set; }

        [JsonProperty(PropertyName = "senses")]
        public List<Senses> Senses { get; set; }
    }
}
