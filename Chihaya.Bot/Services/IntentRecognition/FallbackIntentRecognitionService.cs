using System;

namespace Chihaya.Bot.Services
{
    [Serializable]
    public class FallbackIntentRecognitionService : IFallbackIntentRecognitionService
    {
        public string RecognizeTranslateLookup(string utterance)
        {
            if (!utterance.StartsWith("translate", StringComparison.Ordinal)) return null;

            return utterance
                .Substring(9)
                .NormalizeUtterance();
        }

        public string RecognizeWordLookup(string utterance)
        {
            if (!utterance.StartsWith("look up ", StringComparison.Ordinal)) return null;

            return utterance
                .Substring(8)
                .NormalizeUtterance();
        }
    }
}
