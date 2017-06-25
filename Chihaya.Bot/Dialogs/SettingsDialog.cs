using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Chihaya.Bot.Services;
using Microsoft.Bot.Builder.Luis.Models;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Reflection;

namespace Chihaya.Bot.Dialogs
{
    [Serializable]
    public class SettingsDialog : IDialog<IMessageActivity>
    {
        readonly IConversationSettingsService conversationSettingsService;
        readonly IKanaTranscriptionService kanaTranscriptionService;

        public string SettingName { get; set; }

        public string KanaType { get; set; }

        public SettingsDialog(
            IConversationSettingsService conversationSettingsService,
            IKanaTranscriptionService kanaTranscriptionService)
        {
            this.kanaTranscriptionService = kanaTranscriptionService;
            this.conversationSettingsService = conversationSettingsService;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageRecieved);

            return Task.CompletedTask;
        }

        private async Task MessageRecieved(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (KnownSettingNames.Kana.Equals(this.SettingName, StringComparison.OrdinalIgnoreCase))
            {
                this.conversationSettingsService.SetPreferredKanaType(this.KanaType.ToPreferredKanaType(), context);
                await context.PostAsync(this.kanaTranscriptionService.TranscribeToPreferredKana("わかりました！", context));
            }
                
            else context.Done(context.Activity);

            context.Wait(this.ForwardToRootDialog);
        }

        private Task ForwardToRootDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(context.Activity);

            return Task.CompletedTask;
        }

        public void FillFromLuisResult(LuisResult luisResult)
        {
            var settings = new Expression<Func<string>>[]
            {
                () => this.SettingName,
                () => this.KanaType
            };

            foreach (var setting in settings)
            {
                var property = ((PropertyInfo)((MemberExpression)setting.Body).Member);
                var value = luisResult.Entities.FirstOrDefault(x => x.Type == property.Name)?.Entity;

                property.SetValue(this, value);
            }
        }
    }
}
