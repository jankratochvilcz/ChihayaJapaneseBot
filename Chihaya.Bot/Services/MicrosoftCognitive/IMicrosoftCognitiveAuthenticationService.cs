using System.Threading.Tasks;

namespace Chihaya.Bot.Services
{
    public interface IMicrosoftCognitiveAuthenticationService
    {
        Task<string> GetToken(string apiKey);
        Task<string> RefreshToken(string apiKey);
    }
}
