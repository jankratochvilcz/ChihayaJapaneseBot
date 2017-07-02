using Chihaya.Bot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chihaya.Bot.Dialogs
{
    [Serializable]
    [LuisModel("96aa24ab-e3f6-46df-9ef2-81445a4f2dcb", "96dcf98e02974c08b0178ef88c9c4c9a")]
    public class RootDialog : LuisDialog<object>
    {
        readonly WordLookUpDialog wordLookupDialog;
        readonly TranslateDialog translateDialog;
        readonly IFallbackIntentRecognitionService fallbackIntentRecognitionService;
        readonly SettingsDialog settingsDialog;
        readonly IConversationSettingsService conversationSettingsService;

        public RootDialog(
            WordLookUpDialog wordLookupDialog,
            TranslateDialog translateDialog,
            SettingsDialog settingsDialog,
            IFallbackIntentRecognitionService fallbackIntentRecognitionService,
            IConversationSettingsService conversationSettingsService)
        {
            this.conversationSettingsService = conversationSettingsService;
            this.settingsDialog = settingsDialog;
            this.fallbackIntentRecognitionService = fallbackIntentRecognitionService;
            this.translateDialog = translateDialog;
            this.wordLookupDialog = wordLookupDialog;
        }

        [LuisIntent(KnownIntents.WordLookUp)]
        public async Task LookupWordIntent(IDialogContext context, LuisResult result)
        {
            var word = result.Entities
                .SingleOrDefault(x => x.Type == KnownEntities.WordToLookUp)
                ?.Entity;

            if (word == null)
                word = this.fallbackIntentRecognitionService.RecognizeWordLookup(result.Query);

            if (word == null)
            {
                await this.UnknownIntent(context, result);
                return;
            }

            this.wordLookupDialog.WordToLookUp = word;

            await context.Forward(
                this.wordLookupDialog,
                new ResumeAfter<IMessageActivity>(this.MessageReceived),
                result,
                CancellationToken.None);
        }

        [LuisIntent(KnownIntents.Translate)]
        public async Task TranslateIntent(IDialogContext context, LuisResult result)
        {
            var phrase = result.Entities
                .SingleOrDefault(x => x.Type == KnownEntities.PhraseToLookUp)
                ?.Entity;

            if (phrase == null)
                phrase = this.fallbackIntentRecognitionService.RecognizeTranslateLookup(result.Query);

            if (phrase == null)
            {
                await this.UnknownIntent(context, result);
                return;
            }

            this.translateDialog.PhraseToTranslate = phrase;

            await context.Forward(
                this.translateDialog,
                new ResumeAfter<IMessageActivity>(this.MessageReceived),
                result,
                CancellationToken.None);
        }

        [LuisIntent(KnownIntents.Settings)]
        public async Task SettingsIntent(IDialogContext context, LuisResult result)
        {
            this.settingsDialog.FillFromLuisResult(result);

            await context.Forward(
                this.settingsDialog,
                new ResumeAfter<IMessageActivity>(this.MessageReceived),
                result,
                CancellationToken.None);
        }

        [LuisIntent(KnownIntents.Help)]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            message.AddKeyboardCard(string.Empty, new[] { "look up spring", "translate Have a nice evening!" });
            message.Text = "Hi! Use me to quickly translate from and to Japanese. If hiragana is easier to read for you than katakana, you can type \"Change kana to hiragana\" any time. ";

            await context.PostAsync(message);
        }

        [LuisIntent(KnownIntents.Unknown)]
        public async Task UnknownIntent(IDialogContext context, LuisResult result)
        {
            var utteranceToTranslate = this.fallbackIntentRecognitionService.RecognizeTranslateLookup(result.Query);
            if (utteranceToTranslate != null)
            {
                await this.TranslateIntent(context, result);
                return;
            }

            var utteranceToLookup = this.fallbackIntentRecognitionService.RecognizeTranslateLookup(result.Query);
            if (utteranceToLookup != null)
            {
                await this.LookupWordIntent(context, result);
                return;
            }

            var message = context.MakeMessage();
            message.AddKeyboardCard(string.Empty, new[] { "hello" });
            message.Text = "Sorry, try saying hello to see what you can do.";

            await context.PostAsync(message);
        }
    }
}
