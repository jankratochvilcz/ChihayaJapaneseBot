using Newtonsoft.Json;
using System;

namespace Chihaya.Bot.Services
{
    [Serializable]
    public class JapaneseLanguageToken
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("reading")]
        public string Reading { get; set; }

        [JsonProperty("surface")]
        public string Surface { get; set; }

        [JsonProperty("pronounciation")]
        public string Pronounciation { get; set; }
    }
}
