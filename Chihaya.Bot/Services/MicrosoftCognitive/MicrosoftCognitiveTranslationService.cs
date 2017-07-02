using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace Chihaya.Bot.Services
{
    [Serializable]
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

        public async Task<string> Translate(string text, SupportedLanguage language)
        {
            var token = await this.cognitiveServicesAuthenticationService.GetToken(MicrosoftCognitiveTranslationService.AppKey);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var url = $"{MicrosoftCognitiveTranslationService.TranslateUrl}?text={text}&to={language.ToCode()}";

            var response = await client.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await this.cognitiveServicesAuthenticationService.RefreshToken(MicrosoftCognitiveTranslationService.AppKey);
                return await this.Translate(text, language);
            }

            if (!response.IsSuccessStatusCode)
                return null;

            var document = new XmlDocument();
            document.LoadXml(await response.Content.ReadAsStringAsync());

            return document.InnerText;
        }
    }
}
