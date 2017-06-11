using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Chihaya.Bot.Services
{
    public class MicrosoftCognitiveTranslationService : IJapaneseTranslationService
    {
        private const string TranslateUrl = "https://api.microsofttranslator.com/V2/Http.svc/Translate";
        private const string AppKey = "8f789c9c8926426d99068e87369310e6";

        private readonly IMicrosoftCognitiveAuthenticationService cognitiveServicesAuthenticationService;

        public MicrosoftCognitiveTranslationService(
            IMicrosoftCognitiveAuthenticationService cognitiveServicesAuthenticationService)
        {
            this.cognitiveServicesAuthenticationService = cognitiveServicesAuthenticationService;
        }

        public async Task<string> Translate(string text)
        {
            var token = await this.cognitiveServicesAuthenticationService.GetToken(MicrosoftCognitiveTranslationService.AppKey);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var url = $"{MicrosoftCognitiveTranslationService.TranslateUrl}?text={text}&to=ja";

            var response = await client.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await this.cognitiveServicesAuthenticationService.RefreshToken(MicrosoftCognitiveTranslationService.AppKey);
                return await this.Translate(text);
            }

            if (!response.IsSuccessStatusCode)
                return null;

            var serializer = new XmlSerializer(typeof(string));
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                return (string)serializer.Deserialize(stream);
            }
        }
    }
}
