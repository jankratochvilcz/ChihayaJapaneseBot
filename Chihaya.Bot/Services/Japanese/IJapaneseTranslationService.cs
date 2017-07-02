using System.Threading.Tasks;

namespace Chihaya.Bot.Services
{
    public interface IJapaneseTranslationService
    {
        Task<string> Translate(string text, SupportedLanguage language);
    }
}