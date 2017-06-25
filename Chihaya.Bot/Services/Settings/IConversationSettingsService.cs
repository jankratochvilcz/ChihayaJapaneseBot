using Microsoft.Bot.Builder.Dialogs;

namespace Chihaya.Bot.Services
{
    public interface IConversationSettingsService
    {
        void SetPreferredKanaType(KanaType value, IDialogContext context);

        KanaType GetPreferredKanaType(IDialogContext context);
    }
}
