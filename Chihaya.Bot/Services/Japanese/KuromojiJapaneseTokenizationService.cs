using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Chihaya.Bot.Services
{
    [Serializable]
    public class KuromojiJapaneseTokenizationService : IJapaneseTokenizationService
    {
        private const string TokenizeUrl = "http://atilika.org/kuromoji/rest/tokenizer/tokenize";

        public async Task<List<JapaneseLanguageToken>> Tokenize(string expression)
        {
            var client = new HttpClient();

            var response = await client.PostAsync(KuromojiJapaneseTokenizationService.TokenizeUrl, new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"text", expression }
            }));

            using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<KuromojiTokenizationResponse>(jsonReader).Tokens;
            }
        }
    }
}
