namespace Chihaya.Bot.Services
{
    public interface IFallbackIntentRecognitionService
    {
        string RecognizeTranslateLookup(string utterance);
        string RecognizeWordLookup(string utterance);
    }
}