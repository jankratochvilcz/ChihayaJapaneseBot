namespace Chihaya.Bot.Services
{
    public interface ILanguageDetectionService
    {
        SupportedLanguage GetLanguage(string utterance);
    }
}
