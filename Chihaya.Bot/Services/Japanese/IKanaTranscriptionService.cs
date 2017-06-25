using Microsoft.Bot.Builder.Dialogs;

namespace Chihaya.Bot.Services
{
    public interface IKanaTranscriptionService
    {
        string Transcribe(string text, KanaType toKanaType);
        string TranscribeToPreferredKana(string text, IDialogContext context);
    }
}