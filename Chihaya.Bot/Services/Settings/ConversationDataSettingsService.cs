using Microsoft.Bot.Builder.Dialogs;
using System;

namespace Chihaya.Bot.Services
{
    [Serializable]
    public class ConversationDataSettingsService : IConversationSettingsService
    {
        private const string PreferredKanaTypeKey = "Settings.PreferredKanaType";

        public void SetPreferredKanaType(KanaType value, IDialogContext context)
            => context.ConversationData.SetValue(ConversationDataSettingsService.PreferredKanaTypeKey, value);

        public KanaType GetPreferredKanaType(IDialogContext context)
            => context.ConversationData.ContainsKey(ConversationDataSettingsService.PreferredKanaTypeKey)
                    ? context.ConversationData.GetValue<KanaType>(ConversationDataSettingsService.PreferredKanaTypeKey)
                    : default(KanaType);
    }
}
