using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chihaya.Bot.Services
{
    public interface IJapaneseTokenizationService
    {
        Task<List<JapaneseLanguageToken>> Tokenize(string expression);
    }
}