using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chihaya.Bot.Services
{
    public class KuromojiTokenizationResponse
    {
        [JsonProperty("input")]
        public string Input { get; set; }

        [JsonProperty("tokens")]
        public List<JapaneseLanguageToken> Tokens { get; set; }
    }
}
