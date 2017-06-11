using Chihaya.Bot.Services.Japanese.Entities.JishoOrg;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Chihaya.Bot.Services
{
    [Serializable]
    public class HttpJishoOrgWordLookupService : IWordLookupService
    {
        private const string SearchEndpoint = "http://jisho.org/api/v1/search/words?";

        public async Task<WordLookupResult> Lookup(string term)
        {
            var uri = $"{HttpJishoOrgWordLookupService.SearchEndpoint}keyword={WebUtility.UrlEncode(term)}";

            var client = new HttpClient();
            var response = await client.GetAsync(uri);

            var jishoResponse = await response.Content.ReadAsAsync<SearchResults>();

            var result = new WordLookupResult
            {
                SearchedTerm = term,
                Results = jishoResponse.Results
                    .Select(x => new WordLookupResultItem
                    {
                        ReadingWithKana = x.Japanese.First().WordWithKana,
                        ReadingWithKanji = x.Japanese.First().WordWithKanji,
                        EnglishDefinitions = x.Senses.First().EnglishDefinitions
                    }).ToList()
            };

            return result;
        }
    }
}
