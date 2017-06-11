using System.Threading.Tasks;

namespace Chihaya.Bot.Services
{
    public interface IWordLookupService
    {
        Task<WordLookupResult> Lookup(string term);
    }
}
