namespace Chihaya.Bot.Services
{
    public interface IFallbackIntentRecognitionService
    {
        string RecognizePhraseLookup(string utterance);
    }
}