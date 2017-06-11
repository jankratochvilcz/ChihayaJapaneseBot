using System.Threading.Tasks;

namespace Chihaya.Bot.Services
{
    interface IWordLookupService
    {
        Task<WordLookupResult> Lookup(string term);
    }
}
