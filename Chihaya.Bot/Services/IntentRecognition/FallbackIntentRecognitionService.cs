using System;

namespace Chihaya.Bot.Services
{
    [Serializable]
    public class FallbackIntentRecognitionService : IFallbackIntentRecognitionService
    {
        public string RecognizePhraseLookup(string utterance)
        {
            if (!utterance.StartsWith("translate", StringComparison.Ordinal)) return null;

            return utterance
                .Substring(9)
                .NormalizeUtterance();
        }
    }
}
