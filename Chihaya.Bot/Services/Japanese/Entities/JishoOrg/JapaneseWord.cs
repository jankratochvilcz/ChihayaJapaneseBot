using Newtonsoft.Json;

namespace Chihaya.Bot.Services.Japanese.Entities.JishoOrg
{
    public class JapaneseWord
    {
        [JsonProperty(PropertyName = "word")]
        public string WordWithKanji { get; set; }

        [JsonProperty(PropertyName = "reading")]
        public string WordWithKana { get; set; }
    }
}
