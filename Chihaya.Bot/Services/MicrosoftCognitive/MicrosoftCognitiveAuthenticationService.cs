using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Chihaya.Bot.Services
{
    public class MicrosoftCognitiveAuthenticationService : IMicrosoftCognitiveAuthenticationService
    {
        private const string IssueTokenUrl = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";

        private readonly Dictionary<string, string> cachedTokens = new Dictionary<string, string>();

        public async Task<string> GetToken(string appKey)
        {
            return !this.cachedTokens.ContainsKey(appKey)
                ? await this.RefreshToken(appKey)
                : cachedTokens[appKey];
        }

        public async Task<string> RefreshToken(string appKey)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", appKey);

            var response = await client.PostAsync(MicrosoftCognitiveAuthenticationService.IssueTokenUrl, null);

            string newToken = await response.Content.ReadAsStringAsync();

            if (this.cachedTokens.ContainsKey(appKey))
                this.cachedTokens[appKey] = newToken;
            else
                this.cachedTokens.Add(appKey, newToken);

            return this.cachedTokens[appKey];
        }
    }
}
