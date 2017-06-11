using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chihaya.Bot.Services.Japanese.Entities.JishoOrg
{
    public class Senses
    {
        [JsonProperty(PropertyName = "english_definitions")]
        public List<string> EnglishDefinitions { get; set; }

        [JsonProperty(PropertyName = "parts_of_speech")]
        public List<string> PartsOfSpeech { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags { get; set; }
    }
}
